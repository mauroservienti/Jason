using System;
using Topics.Radical.Validation;
using System.Linq;
using System.Collections.Generic;
using Topics.Radical.Linq;
using System.Diagnostics;
using Jason.Model;
using Jason.Handlers.Tasks;
using Jason.Factories;

namespace Jason.Handlers.Jobs
{
	public class DefaultJobWorker : AbstractJobWorker<Job>
	{
		readonly IJobTaskHandlersProvider provider;

		public DefaultJobWorker( IJobTaskHandlersProvider provider )
		{
			Ensure.That( provider ).Named( () => provider ).IsNotNull();

			this.provider = provider;
		}

		protected override JobExecutionResult OnWorkOn( Job job )
		{
			var tmp = new List<JobTaskResult>();

			job.Tasks.ForEach( new { Results = tmp, Provider = this.provider }, ( s, task ) =>
			{
				IJobTaskWorker worker = null; ;

				try
				{
					worker = s.Provider.GetWorkerFor( task );
					var msgResponse = worker.WorkOn( task );
					s.Results.Add( msgResponse );
				}
				finally
				{
					if( worker != null )
					{
						s.Provider.Release( worker );
					}
				}

				return s;
			} );

			var result = this.CreateCorrelatedResult<JobExecutionResult>( job );
			result.TasksResult = tmp.ToArray();

			return result;
		}
	}
}
