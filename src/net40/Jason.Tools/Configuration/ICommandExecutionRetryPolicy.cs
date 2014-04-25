using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jason.Handlers.Commands;

namespace Jason.Configuration
{
	public interface ICommandExecutionRetryPolicy
	{
		object Execute( Object command, Func<ICommandHandler> handlerFactory );
	}
}
