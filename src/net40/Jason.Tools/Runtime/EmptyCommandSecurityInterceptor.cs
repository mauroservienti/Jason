using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jason.Handlers;

namespace Jason.Runtime
{
	public class EmptyCommandSecurityInterceptor : ICommandSecurityInterceptor
	{
		public bool IsAllowed( object rawCommand, out System.Security.SecurityException error )
		{
			error = null;
			return true;
		}
	}
}
