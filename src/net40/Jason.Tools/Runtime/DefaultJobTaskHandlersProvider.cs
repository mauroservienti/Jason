using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jason.Factories;
using Jason.Handlers.Tasks;

namespace Jason.Runtime
{
	public class DefaultJobTaskHandlersProvider : IJobTaskHandlersProvider
	{
		readonly IServiceProvider container;

		public DefaultJobTaskHandlersProvider( IServiceProvider container )
		{
			this.container = container;
		}

		public IJobTaskWorker GetWorkerFor( Model.JobTask task )
		{
			var tt = task.GetType();
			var generic = typeof( IJobTaskWorker<> );
			var worker = generic.MakeGenericType( tt );

			return ( IJobTaskWorker )this.container.GetService( worker );
		}

		public void Release( Handlers.Tasks.IJobTaskWorker worker )
		{

		}

        public Handlers.Tasks.IJobTaskRunner GetRunnerFor( Model.JobTask task )
		{
			var tt = task.GetType();
			var generic = typeof( IJobTaskRunner<> );
			var runner = generic.MakeGenericType( tt );

			return ( IJobTaskRunner )this.container.GetService( runner );
		}

        public void Release( Handlers.Tasks.IJobTaskRunner runner )
		{

		}
	}
}
