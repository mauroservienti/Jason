using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Topics.Radical.ComponentModel;
using Jason.Model;

namespace Jason.Handlers.Jobs
{
	[Contract]
	public interface IJobWorker<TJob>
		: IJobWorker
		where TJob : Job
	{
		JobExecutionResult WorkOn( TJob job );
	}
}
