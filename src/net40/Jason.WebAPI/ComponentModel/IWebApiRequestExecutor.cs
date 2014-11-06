using System;
using System.Net.Http;
namespace Jason.WebAPI.ComponentModel
{
	public interface IWebApiRequestExecutor
	{
		HttpResponseMessage Handle( HttpRequestMessage request, object command );
	}
}
