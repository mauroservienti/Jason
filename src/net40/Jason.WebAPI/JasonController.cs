using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Text;
using System.Web.Http;
using Jason.Configuration;
using Jason.Factories;
using Jason.Handlers.Commands;
using Jason.Runtime;
using Topics.Radical.Linq;
using Topics.Radical.Validation;
using Topics.Radical.Diagnostics;
using System.Web.Http.ModelBinding;
using Jason.ComponentModel;
using Jason.WebAPI.ComponentModel;

namespace Jason.WebAPI
{
	public class JasonController : ApiController
	{
		static readonly TraceSource logger = new TraceSource( "Jason" );

		readonly IWebApiRequestExecutor executor;

		public JasonController( IWebApiRequestExecutor executor )
		{
			this.executor = executor;
			
			logger.Debug( "JasonController.ctor" );
		}

		[HttpGet]
		public IEnumerable<String> GetNextIdentifiersChunk( Int32 qty = 50 )
		{
			Ensure.That( qty ).Named( () => qty ).IsGreaterThen( 0, Or.NotEqual );

			var temp = new List<String>();

			for ( int i = 0; i < qty; i++ )
			{
				temp.Add( Guid.NewGuid().ToString() );
			}

			return temp;
		}

		[HttpPost]
		public HttpResponseMessage Execute( Object command ) 
		{
			logger.Debug( "JasonController/Execute" );

			return this.Post( command );
		}

		public HttpResponseMessage Post( Object command )
		{
			try
			{
				logger.Debug( "JasonController/Post" );

				Ensure.That( command )
					.Named( () => command )
					.LogErrorsTo( logger )
					.IsNotNull();

				var result = this.executor.Handle( this.Request, command );

				return result;

				//Object result = null;

				//if ( command is IJob )
				//{
				//	result = this.jobDispatcher.DispatchJob( this.Request, ( IJob )command );
				//}
				//else
				//{
				//	result = this.commandDispatcher.DispatchCommand( this.Request, command );
				//}

				//if ( result is HttpResponseMessage )
				//{
				//	logger.Debug( "result is HttpResponseMessage, returning as is." );
				//	return ( HttpResponseMessage )result;
				//}
				//else if ( result == null || result == Jason.Defaults.Response.Ok )
				//{
				//	var defaultCode = this.configuration.GetEndpoint<JasonWebAPIEndpoint>().DefaultSuccessfulHttpResponseCode;

				//	logger.Debug( "result is '{0}', returning HTTP-Code: {1}.", result == null ? "<null>" : "Ok", defaultCode );
				//	return Request.CreateResponse( defaultCode );
				//}
				//else
				//{
				//	var defaultCode = this.configuration.GetEndpoint<JasonWebAPIEndpoint>().DefaultSuccessfulHttpResponseCode;
				//	logger.Debug( "result is custom type, returning wrapped in {0}.", defaultCode );
				//	return Request.CreateResponse( defaultCode, result );
				//}
			}
			catch ( Exception critical )
			{
				logger.TraceEvent( TraceEventType.Critical, 0, critical.Message );

				logger.Debug( "Returning HTTP-503." );
				return Request.CreateErrorResponse( HttpStatusCode.ServiceUnavailable, critical );
			}
			finally
			{
				logger.Debug( "JasonController/Post completed." );
			}
		}
	}
}
