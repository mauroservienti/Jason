using Jason.ComponentModel;
using Jason.Configuration;
using Jason.WebAPI.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Topics.Radical.Diagnostics;
using Topics.Radical.Validation;

namespace Jason.WebAPI.Runtime
{
	class WebApiRequestExecutor : Jason.WebAPI.ComponentModel.IWebApiRequestExecutor
	{
		static readonly TraceSource logger = new TraceSource( "Jason" );

		readonly IJasonServerConfiguration configuration;
		readonly IWebApiCommandDispatcher commandDispatcher;
		readonly IWebApiJobDispatcher jobDispatcher;

		public WebApiRequestExecutor( IJasonServerConfiguration configuration, IWebApiCommandDispatcher commandDispatcher, IWebApiJobDispatcher jobDispatcher )
		{
			this.configuration = configuration;
			this.commandDispatcher = commandDispatcher;
			this.jobDispatcher = jobDispatcher;
		}

		public HttpResponseMessage Handle( HttpRequestMessage request, Object command )
		{
			try
			{
				logger.Debug( "WebApiRequestExecutor/Handle" );

				Ensure.That( command )
					.Named( () => command )
					.LogErrorsTo( logger )
					.IsNotNull();

				if( command is JObject )
				{
					var last = request.RequestUri.Segments.Last();
					var type = this.configuration.GetEndpoint<JasonWebAPIEndpoint>().FindCommandType( request, last );

					command = this.configuration.GetEndpoint<JasonWebAPIEndpoint>().ConvertToCommand( request, last, type, ( ( JObject )command ) );
				}

				Object result = null;

				if( command is IJob )
				{
					result = this.jobDispatcher.DispatchJob( request, ( IJob )command );
				}
				else
				{
					result = this.commandDispatcher.DispatchCommand( request, command );
				}

				if( result is HttpResponseMessage )
				{
					logger.Debug( "result is HttpResponseMessage, returning as is." );
					return ( HttpResponseMessage )result;
				}
				else if( result == null || result == Jason.Defaults.Response.Ok )
				{
					var defaultCode = this.configuration.GetEndpoint<JasonWebAPIEndpoint>().DefaultSuccessfulHttpResponseCode;

					logger.Debug( "result is '{0}', returning HTTP-Code: {1}.", result == null ? "<null>" : "Ok", defaultCode );
					return request.CreateResponse( defaultCode );
				}
				else
				{
					var defaultCode = this.configuration.GetEndpoint<JasonWebAPIEndpoint>().DefaultSuccessfulHttpResponseCode;
					logger.Debug( "result is custom type, returning wrapped in {0}.", defaultCode );
					return request.CreateResponse( defaultCode, result );
				}
			}
			catch( Exception critical )
			{
				logger.TraceEvent( TraceEventType.Critical, 0, critical.Message );

				logger.Debug( "Returning HTTP-503." );
				return request.CreateErrorResponse( HttpStatusCode.ServiceUnavailable, critical );
			}
			finally
			{
				logger.Debug( "WebApiRequestExecutor/Handle completed." );
			}
		}
	}
}
