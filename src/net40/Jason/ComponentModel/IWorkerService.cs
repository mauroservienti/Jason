using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Jason.Model;

namespace Jason.ComponentModel
{
	[ServiceContract(
//#if !NETFX_CORE
//		SessionMode = SessionMode.NotAllowed, 
//#endif
        Namespace = "http://services.topics.it/jason/" )]
	public interface IWorkerService
	{
		[ServiceKnownType( "GetServiceKnownTypes", typeof( ServiceKnownTypesProvider ) )]
		[OperationContract()]
		JobExecutionResult WorkOn( Job job );

		[ServiceKnownType( "GetServiceKnownTypes", typeof( ServiceKnownTypesProvider ) )]
		[OperationContract()]
		Object Execute( Object command );

		[ServiceKnownType( "GetServiceKnownTypes", typeof( ServiceKnownTypesProvider ) )]
		[OperationContract( IsOneWay = true )]
		void Run( Job job );
	}
}
