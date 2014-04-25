using System;
using System.ServiceModel.Activation;
using System.ServiceModel;
using Topics.Radical.Validation;
using Topics.Radical.Linq;
using Jason.Model;
using Jason.ComponentModel;
using Jason.Factories;
using Jason.Runtime;
using Jason.Configuration;
using Jason.Handlers.Commands;
using System.Security;

namespace Jason.Server.WcfServices
{
	[AspNetCompatibilityRequirements( RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed )]
	[ServiceBehavior( InstanceContextMode = InstanceContextMode.PerCall )]
	public class WorkerService : IWorkerService
	{
		#region IDisposable Members

		~WorkerService()
		{
			this.Dispose( false );
		}

		protected virtual void Dispose( Boolean disposing )
		{

		}

		public void Dispose()
		{
			this.Dispose( true );
			GC.SuppressFinalize( this );
		}

		#endregion

		readonly IJobHandlersProvider jobProvider;
		readonly ICommandExecutionRetryPolicy retryPolicy;
		readonly ICommandHandlerProvider handlerProvider;
		readonly IInterceptorProvider interceptorProvider;

		public WorkerService( IJobHandlersProvider jobProvider, ICommandHandlerProvider handlerProvider, ICommandExecutionRetryPolicy retryPolicy, IInterceptorProvider interceptorProvider )
		{
			Ensure.That( jobProvider ).Named( () => jobProvider ).IsNotNull();
			Ensure.That( handlerProvider ).Named( () => handlerProvider ).IsNotNull();
			Ensure.That( retryPolicy ).Named( () => retryPolicy ).IsNotNull();
			Ensure.That( interceptorProvider ).Named( () => interceptorProvider ).IsNotNull();

			this.jobProvider = jobProvider;
			this.handlerProvider = handlerProvider;
			this.retryPolicy = retryPolicy;
			this.interceptorProvider = interceptorProvider;
		}

		public JobExecutionResult WorkOn( Job job )
		{
			Ensure.That( job ).Named( () => job ).IsNotNull();
			Ensure.That( job.Tasks ).IsNotNull();

			var worker = this.jobProvider.GetWorkerFor( job );
			var result = worker.WorkOn( job );

			return result;
		}

		public void Run( Job job )
		{
			Ensure.That( job ).Named( () => job ).IsNotNull();
			Ensure.That( job.Tasks ).IsNotNull();

			var runner = this.jobProvider.GetRunnerFor( job );
			runner.Run( job );
		}

		public Object Execute( Object command )
		{
			Ensure.That( command ).Named( () => command ).IsNotNull();

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
