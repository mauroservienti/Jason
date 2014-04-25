using System;
using System.Security;
using Jason.Client.ComponentModel;
using Jason.Configuration;
using Jason.Factories;
using Jason.Handlers.Commands;
using Jason.Runtime;
using Topics.Radical.Linq;
using Topics.Radical.Validation;

namespace Jason.Client.Runtime
{
	class WorkerServiceClient : IWorkerServiceClient
	{
		public void Dispose()
		{
			//NOP
		}

		readonly ICommandExecutionRetryPolicy retryPolicy;
		readonly ICommandHandlerProvider handlerProvider;
		readonly IInterceptorProvider interceptorProvider;
		readonly IValidatorsProvider validatorsProvider;

		public WorkerServiceClient( ICommandExecutionRetryPolicy retryPolicy, ICommandHandlerProvider handlerProvider, IInterceptorProvider interceptorProvider, IValidatorsProvider validatorsProvider )
		{
			this.retryPolicy = retryPolicy;
			this.handlerProvider = handlerProvider;
			this.interceptorProvider = interceptorProvider;
			this.validatorsProvider = validatorsProvider;
		}

		public Object Execute( Object command )
		{
			Ensure.That( command ).Named( () => command ).IsNotNull();

			//#error mancano i validatori

			var securityInterceptors = this.interceptorProvider.GetSecurityInterceptors( command );
			try
			{
				foreach ( var item in securityInterceptors )
				{
					SecurityException error;
					if ( !item.IsAllowed( command, out error ) )
					{
						throw error;
					}
				}
			}
			finally
			{
				this.interceptorProvider.Release( securityInterceptors );
			}

			var interceptors = this.interceptorProvider.GetCommandInterceptors( command );

			try
			{
				interceptors.ForEach( i => i.OnExecute( command ) );

				ICommandHandler last = null;
				var result = this.retryPolicy.Execute( command, () =>
				{
					if ( last != null )
					{
						this.handlerProvider.Release( last );
					}
					last = this.handlerProvider.GetHandlerFor( command );
					return last;
				} );

				interceptors.ForEach( i => i.OnExecuted( command, result ) );

				return result;
			}
			catch ( Exception ex )
			{
				interceptors.ForEach( i => i.OnException( command, ex ) );

				throw;
			}
			finally
			{
				this.interceptorProvider.Release( interceptors );
			}
		}
	}
}