using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jason.Handlers.Tasks;
using Jason.Model;

namespace Jason.Factories
{
	public interface IJobTaskHandlersProvider
	{
		IJobTaskWorker GetWorkerFor( JobTask task );

		void Release( IJobTaskWorker worker );

		IJobTaskRunner GetRunnerFor( JobTask task );

		void Release( IJobTaskRunner runner );
	}
}
