using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jason.Model;

namespace Jason.Handlers.Tasks
{
	public abstract class AbstractJobTaskRunner<TTask> : IJobTaskRunner<TTask>
		where TTask : JobTask
	{
		public abstract void Run( TTask task );

		public virtual void Run( JobTask task )
		{
			this.Run( ( TTask )task );
		}
	}
}
