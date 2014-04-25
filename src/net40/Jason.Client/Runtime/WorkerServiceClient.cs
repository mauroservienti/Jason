using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Jason.Client.ComponentModel;
using Jason.ComponentModel;
using Jason.Model;

namespace Jason.Client.Runtime
{
	class WorkerServiceClient : WorkerServiceClientBase<IWorkerService>, IWorkerServiceClient
	{
		public WorkerServiceClient()
		{

		}

		public WorkerServiceClient( bool resetConfiguration )
			: base( resetConfiguration )
		{

		}

		public WorkerServiceClient( bool resetConfiguration, Binding binding, EndpointAddress remoteAddress )
			: base( resetConfiguration, binding, remoteAddress )
		{

		}

		public WorkerServiceClient( string endpointConfigurationName )
			: base( endpointConfigurationName )
		{

		}

		public WorkerServiceClient( string endpointConfigurationName, bool resetConfiguration )
			: base( endpointConfigurationName, resetConfiguration )
		{

		}

		public WorkerServiceClient( string endpointConfigurationName, string remoteAddress )
			: base( endpointConfigurationName, remoteAddress )
		{

		}

		public WorkerServiceClient( string endpointConfigurationName, EndpointAddress remoteAddress )
			: base( endpointConfigurationName, remoteAddress )
		{

		}

		public WorkerServiceClient( Binding binding, EndpointAddress remoteAddress )
			: base( binding, remoteAddress )
		{

		}

		public WorkerServiceClient( InstanceContext callbackInstance )
			: base( callbackInstance )
		{

		}

		public WorkerServiceClient( InstanceContext callbackInstance, string endpointConfigurationName )
			: base( callbackInstance, endpointConfigurationName )
		{

		}

		public WorkerServiceClient( InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress )
			: base( callbackInstance, endpointConfigurationName, remoteAddress )
		{

		}

		public WorkerServiceClient( InstanceContext callbackInstance, string endpointConfigurationName, EndpointAddress remoteAddress )
			: base( callbackInstance, endpointConfigurationName, remoteAddress )
		{

		}

		public WorkerServiceClient( InstanceContext callbackInstance, Binding binding, EndpointAddress remoteAddress )
			: base( callbackInstance, binding, remoteAddress )
		{

		}

		public JobExecutionResult WorkOn( Job job )
		{
			JobExecutionResult result = null;

			result = this.Channel.WorkOn( job );

			return result;
		}

		public Object Execute( Object command )
		{
			var result = this.Channel.Execute( command );

			return result;
		}

		public void Run( Job job )
		{
			this.Channel.Run( job );
		}
	}
}