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
		[HttpGet]
		public IEnumerable<String> GetNextIdentifiersChunk( Int32 qty = 50 )
		{
			Ensure.That( qty ).Named( () => qty ).IsGreaterThen( 0, Or.NotEqual );

			var temp = new List<String>();

			for( int i = 0; i < qty; i++ )
			{
				temp.Add( Guid.NewGuid().ToString() );
			}

			return temp;
		}
	}
}
