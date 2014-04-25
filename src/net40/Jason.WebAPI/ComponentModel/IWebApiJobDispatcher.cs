using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Jason.ComponentModel;

namespace Jason.WebAPI.ComponentModel
{
	public interface IWebApiJobDispatcher
	{
		Object DispatchJob( HttpRequestMessage request, IJob job );
	}
}
