using Jason.ComponentModel;
using Jason.Configuration;
using Jason.WebAPI.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using Topics.Radical.Diagnostics;

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
			if( command is JObject ) 
			{
				var last = request.RequestUri.Segments.Last();
				var type = this.configuration.GetEndpoint<JasonWebAPIEndpoint>().FindCommandType( request, last );
				command = ( ( JObject )command ).ToObject( type );
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
	}
}
