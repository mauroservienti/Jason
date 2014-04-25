using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Topics.Radical.Validation;
using Topics.Radical.Diagnostics;
using System.Diagnostics;
using Jason.Model;

namespace Jason.Handlers.Tasks
{
	public abstract class AbstractJobTaskWorker<TTask> : IJobTaskWorker<TTask>
		where TTask : JobTask
	{
		public JobTaskResult WorkOn( TTask task )
		{
			var result = this.OnWorkOn( task );
			this.EnsureCorrelationIsValid( task, result );

			return result;
		}

		protected abstract JobTaskResult OnWorkOn( TTask task );

		protected void EnsureCorrelationIsValid( TTask task, JobTaskResult result )
		{
			Ensure.That( result.CorrelationId )
				.WithMessage( "The CorrelationId on the task result does not match the CorrelationId of the source task." )
				.Is( task.CorrelationId );
		}

		protected TResponse CreateCorrelatedTaskResult<TResponse>( TTask task )
			where TResponse : JobTaskResult, new()
		{
			return this.CreateCorrelatedTaskResult<TResponse>( task, r => { } );
		}

		protected virtual TResult CreateCorrelatedTaskResult<TResult>( TTask task, Action<TResult> interceptor )
			where TResult : JobTaskResult, new()
		{
			var result = new TResult();

			if( result.CorrelationId != task.CorrelationId )
			{
				result.CorrelationId = task.CorrelationId;
			}

			interceptor( result );

			return result;
		}

		JobTaskResult IJobTaskWorker.WorkOn( JobTask task )
		{
			return this.WorkOn( ( TTask )task );
		}
	}
}
