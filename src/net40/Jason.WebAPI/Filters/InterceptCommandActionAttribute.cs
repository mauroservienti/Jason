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