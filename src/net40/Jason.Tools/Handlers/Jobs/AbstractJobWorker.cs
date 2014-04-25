using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Topics.Radical.Validation;
using System.Diagnostics;
using Topics.Radical.Diagnostics;
using Jason.Model;

namespace Jason.Handlers.Jobs
{
	public abstract class AbstractJobWorker<TJob> :
		IJobWorker<TJob>
		where TJob : Job
	{
		protected virtual TResult CreateCorrelatedResult<TResult>( TJob job )
			where TResult : JobExecutionResult, new()
		{
			var result = new TResult();

			if( result.CorrelationId != job.CorrelationId )
			{
				result.CorrelationId = job.CorrelationId;
			}

			return result;
		}

		protected void EnsureCorrelationIsValid( TJob job, JobExecutionResult result )
		{
			Ensure.That( result.CorrelationId )
				.WithMessage( "The CorrelationId on the job result does not match the CorrelationId of the original job." )
				.Is( job.CorrelationId );
		}

		public JobExecutionResult WorkOn( TJob job )
		{
			var result = this.OnWorkOn( job );
			this.EnsureCorrelationIsValid( job, result );

			return result;
		}

		protected abstract JobExecutionResult OnWorkOn( TJob job );

		JobExecutionResult IJobWorker.WorkOn( Job job )
		{
			return this.WorkOn( ( TJob )job );
		}
	}
}
