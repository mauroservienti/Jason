//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Security;
//using System.Text;
//using Jason.Configuration;
//using Jason.Factories;
//using Jason.Handlers.Commands;
//using Topics.Radical.Linq;
//using Topics.Radical.Validation;
//using Topics.Radical.Diagnostics;
//using Jason.Handlers;

//namespace Jason.Runtime
//{
//	public class DefaultJasonService : IJasonService
//	{
//		static readonly TraceSource logger = new TraceSource( "Jason" );

//		readonly IJobHandlersProvider jobProvider;
//		readonly ICommandExecutionRetryPolicy retryPolicy;
//		readonly ICommandHandlerProvider handlerProvider;
//		readonly IInterceptorProvider interceptorProvider;
//		readonly ISecurityViolationHandler securityViolationHandler;

//		public DefaultJasonService( IJobHandlersProvider jobProvider, ICommandHandlerProvider handlerProvider, ICommandExecutionRetryPolicy retryPolicy, IInterceptorProvider interceptorProvider, ISecurityViolationHandler securityViolationHandler )
//		{
//			Ensure.That( jobProvider ).Named( () => jobProvider ).IsNotNull();
//			Ensure.That( handlerProvider ).Named( () => handlerProvider ).IsNotNull();
//			Ensure.That( retryPolicy ).Named( () => retryPolicy ).IsNotNull();
//			Ensure.That( interceptorProvider ).Named( () => interceptorProvider ).IsNotNull();
//			Ensure.That( securityViolationHandler ).Named( () => securityViolationHandler ).IsNotNull();

//			this.jobProvider = jobProvider;
//			this.handlerProvider = handlerProvider;
//			this.retryPolicy = retryPolicy;
//			this.interceptorProvider = interceptorProvider;
//			this.securityViolationHandler = securityViolationHandler;
//		}

//		public object Execute( object command )
//		{
//			logger.Debug( "JasonController/Post" );

//			Ensure.That( command )
//				.Named( () => command )
//				.LogErrorsTo( logger )
//				.IsNotNull();

//			logger.Debug( "Command: {0}", command );
//			logger.Debug( "Loading command interceptors." );

//			var securityInterceptors = this.interceptorProvider.GetSecurityInterceptors( command );
//			var interceptors = this.interceptorProvider.GetCommandInterceptors( command );

//			try
//			{
//				foreach ( var item in securityInterceptors )
//				{
//					SecurityException error;
//					if ( !item.IsAllowed( command, out error ) )
//					{
//						logger.Debug( "Permission denied: {0}.", error.Message );
//						var violationResult = this.securityViolationHandler.OnSecurityViolation( command, error );
//						switch ( violationResult.Behavior )
//						{
//							case ViolationHandlingBehavior.Ignore:
//								//NOP
//								break;

//							case ViolationHandlingBehavior.Throw:
//								throw error;

//							case ViolationHandlingBehavior.Return:
//								return violationResult.Result;
//						}
//					}
//				}

//				interceptors.ForEach( i => i.OnExecute( command ) );

//				ICommandHandler last = null;
//				var result = this.retryPolicy.Execute( command, () =>
//				{
//					if ( last != null )
//					{
//						this.handlerProvider.Release( last );
//					}
//					last = this.handlerProvider.GetHandlerFor( command );
//					return last;
//				} );

//				interceptors.ForEach( i => i.OnExecuted( command, result ) );

//				return result;
//			}
//			catch ( Exception ex )
//			{
//				interceptors.ForEach( i => i.OnException( command, ex ) );

//				throw;
//			}
//			finally
//			{
//				this.interceptorProvider.Release( interceptors );
//				this.interceptorProvider.Release( securityInterceptors );
//			}
//		}
//	}
//}
