using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using Topics.Radical.ComponentModel;

namespace Jason.Handlers
{
	[Contract]
	public interface ICommandSecurityInterceptor
	{
		Boolean IsAllowed( Object rawCommand, out SecurityException error );
	}
}
