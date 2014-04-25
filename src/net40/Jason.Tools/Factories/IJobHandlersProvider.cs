using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jason.Handlers.Jobs;
using Jason.Model;

namespace Jason.Factories
{
	public interface IJobHandlersProvider
	{
		IJobWorker GetWorkerFor( Job job );

		void Release( IJobWorker performer );

		IJobRunner GetRunnerFor( Job job );

		void Release( IJobRunner runner );
	}
}
