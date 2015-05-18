using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;

namespace SampleJasonWebAPI
{
    public static class WebApiConfig
    {
        public static void Register( HttpConfiguration config )
        {
			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				"DefaultApiWithId",
				"api/{controller}/{id}",
				new { id = RouteParameter.Optional } );

			config.Routes.MapHttpRoute(
				"DefaultApiWithAction",
				"api/{controller}/{action}" );

			config.Routes.MapHttpRoute(
				"DefaultApiWithActionAndId",
				"api/{controller}/{action}/{id}",
				new { id = RouteParameter.Optional } );

			config.Routes.MapHttpRoute(
				"DefaultApiGet",
				"api/{controller}",
				new { action = "Get", controller = "Root" },
				new { httpMethod = new HttpMethodConstraint( HttpMethod.Get ) } );

			config.Routes.MapHttpRoute(
				"DefaultApiPost",
				"api/{controller}",
				new { action = "Post" },
				new { httpMethod = new HttpMethodConstraint( HttpMethod.Post ) } );

        }
    }
}
