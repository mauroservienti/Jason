using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Jason.Model;

namespace Jason.ComponentModel
{
	[ServiceContract( Name = "IWorkerService", ConfigurationName = "Jason.ComponentModel.IWorkerService", Namespace = "http://services.topics.it/jason/" )]
	public interface IAsyncWorkerService : IDisposable
	{
		[ServiceKnownType( "GetServiceKnownTypes", typeof( ServiceKnownTypesProvider ) )]
		[OperationContract( AsyncPattern = true, Name = "WorkOn" )]
		IAsyncResult BeginWorkOn( Job job, AsyncCallback callback, object asyncState );

		JobExecutionResult EndWorkOn( IAsyncResult result );

		Task<JobExecutionResult> WorkOnAsync( Job job );

		[ServiceKnownType( "GetServiceKnownTypes", typeof( ServiceKnownTypesProvider ) )]
		[OperationContract( AsyncPattern = true, Name = "Execute" )]
		IAsyncResult BeginExecute( Object command, AsyncCallback callback, object asyncState );

		Object EndExecute( IAsyncResult result );

		Task<Object> ExecuteAsync( Object command );

		[ServiceKnownType( "GetServiceKnownTypes", typeof( ServiceKnownTypesProvider ) )]
		[OperationContract( AsyncPattern = true, IsOneWay = true, Name = "Run" )]
		IAsyncResult BeginRun( Job job, AsyncCallback callback, object asyncState );

		void EndRun( IAsyncResult result );
	}
}
