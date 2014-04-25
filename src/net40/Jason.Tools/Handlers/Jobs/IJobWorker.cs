using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jason.Model;

namespace Jason.Handlers.Jobs
{
	public interface IJobWorker
	{
		JobExecutionResult WorkOn( Job job );
	}
}
