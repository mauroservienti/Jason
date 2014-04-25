using System;
using System.Threading.Tasks;
using Jason.Model;

namespace Jason.Client.ComponentModel
{
	public interface IAsyncWorkerServiceClient : IDisposable
	{
		IAsyncResult BeginWorkOn( Job job, AsyncCallback callback, object asyncState );

		JobExecutionResult EndWorkOn( IAsyncResult result );

		Task<JobExecutionResult> WorkOnAsync( Job job );

		IAsyncResult BeginExecute( Object command, AsyncCallback callback, object asyncState );

		Object EndExecute( IAsyncResult result );

		Task<Object> ExecuteAsync( Object command );

		void Run( Job job );
	}
}
