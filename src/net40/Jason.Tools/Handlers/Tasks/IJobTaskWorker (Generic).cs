using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Topics.Radical.ComponentModel;
using Jason.Model;

namespace Jason.Handlers.Tasks
{
	[Contract]
	public interface IJobTaskWorker<TTask> : IJobTaskWorker
		where TTask : JobTask
	{
		JobTaskResult WorkOn( TTask task );
	}
}
