using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Topics.Radical.ComponentModel;
using Jason.Model;

namespace Jason.Handlers.Tasks
{
	[Contract]
	public interface IJobTaskRunner<TTask> : IJobTaskRunner
		where TTask : JobTask
	{
		void Run( TTask task );
	}
}
