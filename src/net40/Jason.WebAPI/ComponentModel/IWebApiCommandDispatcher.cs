using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Jason.WebAPI.ComponentModel
{
	public interface IWebApiCommandDispatcher
	{
		Object DispatchCommand( HttpRequestMessage request, Object command );
	}
}
