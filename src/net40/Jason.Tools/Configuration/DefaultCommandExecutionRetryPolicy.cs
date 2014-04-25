using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jason.Factories;
using Jason.Handlers.Commands;

namespace Jason.Configuration
{
	public class DefaultCommandExecutionRetryPolicy : ICommandExecutionRetryPolicy
	{
		public virtual object Execute( object command, Func<ICommandHandler> handlerFactory )
		{
			var handler = handlerFactory();

			return handlerFactory().Execute( command );
		}
	}
}
