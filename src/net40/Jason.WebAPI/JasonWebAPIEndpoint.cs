using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Jason.Configuration;
using Jason.Factories;
using Jason.Handlers;
using Jason.Runtime;
using Jason.WebAPI.ComponentModel;
using Jason.WebAPI.Filters;
using Jason.WebAPI.Runtime;
using Newtonsoft.Json;

namespace Jason.WebAPI
{
	public class JasonWebAPIEndpoint : IJasonServerEndpoint
	{
		public JasonWebAPIEndpoint()
		{
			this.DefaultSuccessfulHttpResponseCode = HttpStatusCode.OK;
			this.OnExecutingAction = ( cid, request ) => { };
			//this.OnCommandActionIntercepted = (request,cmd) => { };
			this.CorrelationIdHeaderName = "x-jason-correlation-id";
		}

		public TypeNameHandling? TypeNameHandling { get; set; }
		public HttpStatusCode DefaultSuccessfulHttpResponseCode { get; set; }
		public Action<ExecutingActionArgs, HttpRequestMessage> OnExecutingAction { get; set; }

		public Func<HttpRequestMessage, Object, Func<HttpRequestMessage, Object, HttpResponseMessage>, HttpResponseMessage> OnCommandActionIntercepted { get; set; }

		public String CorrelationIdHeaderName { get; set; }

		public void Initialize( IJasonServerConfiguration configuration, IEnumerable<Type> types )
		{
			configuration.Container.RegisterAsTransient( new[] { typeof( JasonController ) }, typeof( JasonController ) );
			configuration.Container.RegisterAsTransient( new[] { typeof( IWebApiRequestExecutor ) }, typeof( WebApiRequestExecutor ) );
			configuration.Container.RegisterAsTransient( new[] { typeof( IWebApiCommandDispatcher ) }, typeof( WebApiCommandDispatcher ) );
			configuration.Container.RegisterAsTransient( new[] { typeof( IWebApiJobDispatcher ) }, typeof( WebApiJobDispatcher ) );

			if( this.TypeNameHandling.HasValue )
			{
				GlobalConfiguration.Configuration
					.Formatters
					.JsonFormatter
					.SerializerSettings
					.TypeNameHandling = this.TypeNameHandling.Value;
			}

			if( !GlobalConfiguration.Configuration.Filters.OfType<JasonWebApiActionFilter>().Any() )
			{
				var filter = new JasonWebApiActionFilter( this.CorrelationIdHeaderName, () => configuration.Container.Resolve<IWebApiRequestExecutor>() );
				filter.OnExecutingAction = ( cid, request ) =>
				{
					this.OnExecutingAction( cid, request );
				};
				
				var original = filter.OnCommandActionIntercepted;
				filter.OnCommandActionIntercepted = (request, cmd) =>
				{
					if( this.OnCommandActionIntercepted != null ) 
					{
						var result = this.OnCommandActionIntercepted( request, cmd, original );

						return result;
					}


					return original( request, cmd );
				};

				GlobalConfiguration.Configuration.Filters.Add( filter );
			}
		}

		public void Teardown()
		{

		}
	}
}
