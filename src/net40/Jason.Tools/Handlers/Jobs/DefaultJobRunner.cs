using System;
using System.Linq;
using System.Threading;
using Topics.Radical.Validation;
using Topics.Radical.Linq;
using System.Diagnostics;
using Topics.Radical.Diagnostics;
using Jason.Model;
using Jason.Handlers.Tasks;
using Jason.Factories;

namespace Jason.Handlers.Jobs
{
	public class DefaultJobRunner : IJobRunner<Job>
	{
		readonly IJobTaskHandlersProvider provider;

		public DefaultJobRunner( IJobTaskHandlersProvider provider )
		{
			Ensure.That( provider ).Named( () => provider ).IsNotNull();

			this.provider = provider;
		}

		public void Run( Job job )
		{
			ThreadPool.QueueUserWorkItem( cb =>
			{
				job.Tasks.ForEach( this.provider, ( state, message ) =>
				{
					IJobTaskRunner runner = null;

					try
					{
						runner = state.GetRunnerFor( message );
						runner.Run( message );
					}
					finally
					{
						if( runner != null )
						{
							state.Release( runner );
						}
					}

					return state;
				} );
			} );
		}

		void IJobRunner.Run( Job job )
		{
			this.Run( ( Job )job );
		}
	}
}
