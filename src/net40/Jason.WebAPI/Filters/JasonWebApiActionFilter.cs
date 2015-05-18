using Jason.Configuration;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Topics.Radical.Validation;

namespace Jason.WebAPI.Filters
{
	public class JasonWebApiActionFilter : IActionFilter
	{
		readonly String correlationIdHeader;
		readonly IJasonServerConfiguration configuration;
		readonly Func<HttpRequestMessage, Object, HttpResponseMessage> defaultExecutor;

		public JasonWebApiActionFilter( String correlationIdHeader, IJasonServerConfiguration configuration, Func<HttpRequestMessage, Object, HttpResponseMessage> defaultExecutor )
		{
			this.correlationIdHeader = correlationIdHeader;
			this.configuration = configuration;
			this.defaultExecutor = defaultExecutor;
		}

		public Task<HttpResponseMessage> ExecuteActionFilterAsync( HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation )
		{
			Ensure.That( actionContext ).Named( () => actionContext ).IsNotNull();
			Ensure.That( continuation ).Named( () => continuation ).IsNotNull();

			var endpoint = this.configuration.GetEndpoint<JasonWebAPIEndpoint>();
			var interceptCommandAction = actionContext.ActionDescriptor.GetCustomAttributes<InterceptCommandActionAttribute>().SingleOrDefault();
			
			var args = new JasonRequestArgs();
			args.IsJasonExecute = false;
			args.IsCommandInterceptor = interceptCommandAction != null;
			args.HttpRequest = actionContext.Request;

			if( args.IsCommandInterceptor )
			{
				if( actionContext.Request.Headers.Contains( this.correlationIdHeader ) )
				{
					args.CorrelationId = actionContext.Request.Headers.GetValues( this.correlationIdHeader ).Single();
					args.AppendCorrelationIdToResponse = true;
				}

				if( endpoint.OnJasonRequest != null )
				{
					endpoint.OnJasonRequest( args );
				}

				var command = actionContext.ActionArguments.Values.OfType<Object>().Single();
				if( endpoint.OnCommandActionIntercepted != null )
				{
					HttpStatusCode code = HttpStatusCode.NoContent;
					if( interceptCommandAction.ResponseCode.HasValue )
					{
						code = interceptCommandAction.ResponseCode.Value;
					}

					var response = endpoint.OnCommandActionIntercepted( actionContext.Request, command, this.defaultExecutor );
					if( response == null && interceptCommandAction.ResponseCode.HasValue )
					{
						response = new HttpResponseMessage( code );
					}
					else 
					{
						response = new HttpResponseMessage( HttpStatusCode.NoContent );
					}

					return Task.FromResult( response )
						.ContinueWith(t=>
						{
							this.TryAppendHeaders( t, args );
							return t;
						})
						.Unwrap();
				}
				else 
				{
					var response = this.defaultExecutor( args.HttpRequest, command );
					return Task.FromResult( response )
						.ContinueWith( t =>
						{
							this.TryAppendHeaders( t, args );
							return t;
						} )
						.Unwrap();
				}

			}
			else 
			{
				return continuation();
			}
		}

		void TryAppendHeaders( Task<HttpResponseMessage> t, JasonRequestArgs args )
		{
			if( !t.IsFaulted && args.AppendCorrelationIdToResponse && !String.IsNullOrWhiteSpace( args.CorrelationId ) )
			{
				t.Result.Headers.Add( this.correlationIdHeader, args.CorrelationId );
			}
		}

		public bool AllowMultiple
		{
			get { return false; }
		}
	}
}