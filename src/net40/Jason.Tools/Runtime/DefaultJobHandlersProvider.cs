using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Jason.Factories;
using Jason.Handlers.Jobs;

namespace Jason.Runtime
{
	public class DefaultJobHandlersProvider : IJobHandlersProvider
	{
		readonly IServiceProvider container;

		public DefaultJobHandlersProvider( IServiceProvider container )
		{
			this.container = container;
		}

		public IJobWorker GetWorkerFor( Model.Job job )
		{
			var jt = job.GetType();
			var generic = typeof( IJobWorker<> );
			var worker = generic.MakeGenericType( jt );

			return ( IJobWorker )this.container.GetService( worker );
		}

		public void Release( Handlers.Jobs.IJobWorker performer )
		{
			
		}

        public Handlers.Jobs.IJobRunner GetRunnerFor( Model.Job job )
		{
			var jt = job.GetType();
			var generic = typeof( IJobRunner<> );
			var runner = generic.MakeGenericType( jt );

			return ( IJobRunner )this.container.GetService( runner );
		}

        public void Release( Handlers.Jobs.IJobRunner runner )
		{
			
		}
	}
}
