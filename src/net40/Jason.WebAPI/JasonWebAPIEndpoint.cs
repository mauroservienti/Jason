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
			this.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Objects;
			this.DefaultSuccessfulHttpResponseCode = HttpStatusCode.OK;
			this.OnExecutingAction = ( cid, request ) => { };
			this.CorrelationIdHeaderName = "x-jason-correlation-id";
		}

		public TypeNameHandling TypeNameHandling { get; set; }
		public HttpStatusCode DefaultSuccessfulHttpResponseCode { get; set; }
		public Action<ExecutingActionArgs, HttpRequestMessage> OnExecutingAction { get; set; }

		public String CorrelationIdHeaderName { get; set; }

		public void Initialize( IJasonServerConfiguration configuration, IEnumerable<Type> types )
		{
			configuration.Container.RegisterAsTransient( new[] { typeof( JasonController ) }, typeof( JasonController ) );
			configuration.Container.RegisterAsTransient( new[] { typeof( IWebApiCommandDispatcher ) }, typeof( WebApiCommandDispatcher ) );
			configuration.Container.RegisterAsTransient( new[] { typeof( IWebApiJobDispatcher ) }, typeof( WebApiJobDispatcher ) );

			GlobalConfiguration.Configuration
				.Formatters
				.JsonFormatter
				.SerializerSettings
				.TypeNameHandling = this.TypeNameHandling;

			if ( !GlobalConfiguration.Configuration.Filters.OfType<ApiCorrelationIdActionFilter>().Any() )
			{
				var filter = new ApiCorrelationIdActionFilter(this.CorrelationIdHeaderName);
				filter.OnExecutingAction = ( cid, request ) =>
				{
					this.OnExecutingAction( cid, request );
				};

				GlobalConfiguration.Configuration.Filters.Add( filter );
			}
		}

		public void Teardown()
		{

		}
	}
}
