using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jason.ComponentModel;

namespace Jason.Model
{
	public class JobExecutionResult
	{
		public String CorrelationId { get; set; }

		public JobExecutionResult()
		{
			this.TasksResult = new JobTaskResult[ 0 ];
		}

		public JobTaskResult[] TasksResult { get; set; }

		public IEnumerable<TTaskResult> GetTaskResults<TTaskResult>()
			where TTaskResult : JobTaskResult
		{
			var query = this.TasksResult.OfType<TTaskResult>();

			return query;
		}

		public TTaskResult GetTaskResult<TTaskResult>()
			where TTaskResult : JobTaskResult
		{
			return this.GetTaskResults<TTaskResult>().SingleOrDefault();
		}
	}
}
