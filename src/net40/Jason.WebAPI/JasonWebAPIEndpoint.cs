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
using Newtonsoft.Json.Linq;

namespace Jason.WebAPI
{
	public class JasonWebAPIEndpoint : IJasonServerEndpoint
	{
		InMemoryCommandMapper mapper = new InMemoryCommandMapper();
		readonly HttpConfiguration httpConfiguration;

		public JasonWebAPIEndpoint( HttpConfiguration httpConfiguration )
		{
			this.DefaultSuccessfulHttpResponseCode = HttpStatusCode.OK;
			this.CorrelationIdHeaderName = "x-jason-correlation-id";
			this.IsCommandConvention = t => false;
			this.FindCommandType = ( request, lastSegment ) => mapper.GetMappedType( lastSegment );
			this.ConvertToCommand = ( request, lastSegment, type, obj ) => obj.ToObject( type );
			this.httpConfiguration = httpConfiguration;
		}

		public TypeNameHandling? TypeNameHandling { get; set; }
		public HttpStatusCode DefaultSuccessfulHttpResponseCode { get; set; }
		public Action<JasonRequestArgs> OnJasonRequest { get; set; }

		public Func<HttpRequestMessage, Object, Func<HttpRequestMessage, Object, HttpResponseMessage>, HttpResponseMessage> OnCommandActionIntercepted { get; set; }

		public Func<Type, Boolean> IsCommandConvention { get; set; }

		public Func<HttpRequestMessage, String, Type> FindCommandType { get; set; }

		public Func<HttpRequestMessage, String, Type, JObject, Object> ConvertToCommand { get; set; }

		public String CorrelationIdHeaderName { get; set; }

		public void Initialize( IJasonServerConfiguration configuration, IEnumerable<Type> types )
		{
			//configuration.Container.RegisterAsTransient( new[] { typeof( JasonController ) }, typeof( JasonController ) );
			configuration.Container.RegisterAsTransient( new[] { typeof( IWebApiRequestExecutor ) }, typeof( WebApiRequestExecutor ) );
			configuration.Container.RegisterAsTransient( new[] { typeof( IWebApiCommandDispatcher ) }, typeof( WebApiCommandDispatcher ) );
			configuration.Container.RegisterAsTransient( new[] { typeof( IWebApiJobDispatcher ) }, typeof( WebApiJobDispatcher ) );

			Func<HttpRequestMessage, Object, HttpResponseMessage> defaultExecutor = ( request, cmd ) =>
			{
				var executor = configuration.Container.Resolve<IWebApiRequestExecutor>();
				var result = executor.Handle( request, cmd );

				return result;
			};

			this.httpConfiguration.MessageHandlers.Add( new JasonDelegatingHandler( this.CorrelationIdHeaderName, configuration, defaultExecutor, this.httpConfiguration ) );

			var allCommands = types.Where( this.IsCommandConvention );
			foreach( var cmdType in allCommands )
			{
				this.mapper.CreateMapping( cmdType );
			}

			if( this.TypeNameHandling.HasValue )
			{
				this.httpConfiguration
					.Formatters
					.JsonFormatter
					.SerializerSettings
					.TypeNameHandling = this.TypeNameHandling.Value;
			}

			if( !this.httpConfiguration.Filters.OfType<JasonWebApiActionFilter>().Any() )
			{
				var filter = new JasonWebApiActionFilter( this.CorrelationIdHeaderName, configuration, defaultExecutor );

				this.httpConfiguration.Filters.Add( filter );
			}
		}

		public void Teardown()
		{

		}
	}
}
