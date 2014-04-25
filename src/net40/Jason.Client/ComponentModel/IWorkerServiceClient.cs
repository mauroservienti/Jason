using System;
using Jason.Model;

namespace Jason.Client.ComponentModel
{
	public interface IWorkerServiceClient : IDisposable
	{
		JobExecutionResult WorkOn( Job job );

		Object Execute( Object command );

		void Run( Job job );
	}
}
