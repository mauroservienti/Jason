using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Text;
using System.Web.Http.ModelBinding;
using Jason.Configuration;
using Jason.Factories;
using Jason.Handlers.Commands;
using Jason.WebAPI.ComponentModel;
using Topics.Radical.Diagnostics;
using Topics.Radical.Linq;
using Topics.Radical.Validation;

namespace Jason.WebAPI.Runtime
{
	public class WebApiCommandDispatcher : IWebApiCommandDispatcher
	{
		static readonly TraceSource logger = new TraceSource( "Jason" );

		readonly ICommandHandlerProvider handlerProvider;
		readonly ICommandExecutionRetryPolicy retryPolicy;
		readonly IInterceptorProvider interceptorProvider;
		readonly IValidatorsProvider validatorsProvider;
		readonly IJasonServerConfiguration configuration;

		public WebApiCommandDispatcher( ICommandExecutionRetryPolicy retryPolicy, ICommandHandlerProvider handlerProvider, IInterceptorProvider interceptorProvider, IValidatorsProvider validatorsProvider, IJasonServerConfiguration configuration )
		{
			this.handlerProvider = handlerProvider;
			this.retryPolicy = retryPolicy;
			this.interceptorProvider = interceptorProvider;
			this.validatorsProvider = validatorsProvider;
			this.configuration = configuration;

			logger.Debug( "WebApiCommandDispatcher.ctor" );
		}

		Boolean IsCommandAllowed( HttpRequestMessage request, Object command, out HttpResponseMessage result )
		{
			logger.Debug( "Command: {0}", command );

			logger.Debug( "Loading command security interceptors." );
			var securityInterceptors = this.interceptorProvider.GetSecurityInterceptors( command );

			try
			{
				foreach ( var item in securityInterceptors )
				{
					logger.Debug( "Querying security interceptor: {0}.", item );

					SecurityException error;
					if ( !item.IsAllowed( command, out error ) )
					{
						logger.Debug( "Permission denied: {0}.", error.Message );

						result = request.CreateErrorResponse( HttpStatusCode.Forbidden, error );
						return false;
					}
				}
			}
			finally
			{
				logger.Debug( "Releasing command security interceptors." );
				this.interceptorProvider.Release( securityInterceptors );
				logger.Debug( "Security handling completed." );
			}

			result = null;
			return true;
		}

		Boolean IsCommandValid( HttpRequestMessage request, Object command, out HttpResponseMessage result )
		{
			logger.Debug( "Loading command validators." );
			var validators = this.validatorsProvider.GetValidators( command );
			var validationResults = new ValidationResults();
			try
			{
				foreach ( var item in validators )
				{
					logger.Debug( "Querying validation interceptor: {0}.", item );

					var validationResult = item.Validate( command );
					if ( !validationResult.IsValid )
					{
						logger.Debug( "Validation failed." );

						validationResult.Errors.ForEach( e => validationResults.AddError( e ) );
					}
					else
					{
						logger.Debug( "Validation successful." );
					}
				}
			}
			finally
			{
				logger.Debug( "Releasing command validators." );
				this.validatorsProvider.Release( validators );
				logger.Debug( "Validation handling completed." );
			}

			if ( !validationResults.IsValid )
			{
				var data = new ModelStateDictionary();
				validationResults.Errors.ForEach( e => data.AddModelError( e.Key, e.ToString() ) );

				result = request.CreateErrorResponse( HttpStatusCode.BadRequest, data );
				return false;
			}

			result = null;
			return true;
		}

		Object ExecuteCommandUnderRetryPolicy( Object command )
		{
			logger.Debug( "Delegating command handling to retry policy: {0}.", this.retryPolicy );
			ICommandHandler last = null;

			var result = this.retryPolicy.Execute( command, () =>
			{
				if ( last != null )
				{
					logger.Debug( "Releasing handler: {0}.", last );
					this.handlerProvider.Release( last );
				}

				logger.Debug( "Loading handler." );
				last = this.handlerProvider.GetHandlerFor( command );
				logger.Debug( "Handler loaded: {0}.", last );

				return last;
			} );

			return result;
		}

		public Object DispatchCommand( HttpRequestMessage request, Object command )
		{
			HttpResponseMessage response;

			if ( !this.IsCommandAllowed( request, command, out response ) )
			{
				return response;
			}

			if ( !this.IsCommandValid( request, command, out response ) )
			{
				return response;
			}

			logger.Debug( "Loading command interceptors." );
			var interceptors = this.interceptorProvider.GetCommandInterceptors( command );

			try
			{
				interceptors.ForEach( i =>
				{
					logger.Debug( "Invoking command interceptor OnExecute: {0}.", i );
					i.OnExecute( command );
				} );

				var result = this.ExecuteCommandUnderRetryPolicy( command );

				interceptors.ForEach( i =>
				{
					logger.Debug( "Invoking command interceptor OnExecuted: {0}.", i );
					i.OnExecuted( command, result );
				} );

				return result;
			}
			catch ( Exception error )
			{
				logger.Error( error.Message, error );

				interceptors.ForEach( i =>
				{
					logger.Debug( "Invoking command interceptor OnException: {0}.", i );
					i.OnException( command, error );
				} );

				logger.Debug( "Returning HTTP-500." );
				return request.CreateErrorResponse( HttpStatusCode.InternalServerError, error );
			}
			finally
			{
				logger.Debug( "Releasing command interceptors." );
				this.interceptorProvider.Release( interceptors );
			}
		}
	}
}
