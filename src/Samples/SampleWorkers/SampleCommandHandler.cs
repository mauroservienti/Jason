using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jason.Handlers.Commands;
using Jason.Model;
using SampleTasks;

namespace SampleWorkers
{
	class SampleCommandHandler : AbstractCommandHandler<SampleCommand>
	{
		protected override Object OnExecute( SampleCommand command )
		{
			return new EmptyCommandExecutionResult();
		}
	}
}
