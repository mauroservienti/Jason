using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jason.Handlers;

namespace Jason.Runtime
{
	public class EmptyCommandInterceptor : ICommandInterceptor
	{
		public void OnExecute( object rawCommand )
		{
			
		}

		public void OnExecuted( object rawCommand, object rawResult )
		{
			
		}

		public void OnException( object rawCommand, Exception exception )
		{
			
		}
	}
}
