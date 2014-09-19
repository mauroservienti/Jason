using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Jason.Configuration;
using Jason.WebAPI;
using Jason.WebAPI.Filters;

namespace Jason.WebAPI.Filters
{
	public class InterceptCommandActionFilter : IActionFilter
	{
		readonly IJasonServerConfiguration config;
		readonly JasonWebAPIEndpoint endpoint;

		public InterceptCommandActionFilter( IJasonServerConfiguration config )
		{
			this.config = config;
			this.endpoint = this.config.GetEndpoint<JasonWebAPIEndpoint>();
		}

		public Task<HttpResponseMessage> ExecuteActionFilterAsync( HttpActionContext actionContext, System.Threading.CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation )
		{
			var ok = actionContext.ActionDescriptor.GetCustomAttributes<InterceptCommandActionAttribute>().SingleOrDefault();
			if ( ok == null )
			{
				return continuation();
			}

			return Task.Factory.StartNew( () =>
			{
				var command = actionContext.ActionArguments.Values.OfType<Object>().Single();
				var code = this.endpoint.DefaultSuccessfulHttpResponseCode;
				if ( ok.ResponseCode.HasValue )
				{
					code = ok.ResponseCode.Value;
				}

				this.endpoint.OnCommandActionIntercepted( command );

				var response = new HttpResponseMessage( code );

				return response;
			} );
		}

		public bool AllowMultiple
		{
			get { return false; }
		}
	}

	[AttributeUsage( AttributeTargets.Method, AllowMultiple = false, Inherited = false )]
	public class InterceptCommandActionAttribute : Attribute
	{
		public InterceptCommandActionAttribute()
		{
			
		}

		public InterceptCommandActionAttribute( HttpStatusCode responseCode )
		{
			this.ResponseCode = responseCode;
		}

		public HttpStatusCode? ResponseCode { get; private set; }
	}
}