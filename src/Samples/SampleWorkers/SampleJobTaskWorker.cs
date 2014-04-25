using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jason.Handlers.Tasks;
using Jason.Model;
using SampleTasks;

namespace SampleWorkers
{
	public class SampleJobTaskWorker : AbstractJobTaskWorker<SampleJobTask>
	{
		protected override Jason.Model.JobTaskResult OnWorkOn( SampleJobTask message )
		{
			return this.CreateCorrelatedTaskResult<EmptyJobTaskResult>( message );
		}
	}
}