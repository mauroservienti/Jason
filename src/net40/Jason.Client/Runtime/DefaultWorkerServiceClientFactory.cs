using System.ServiceModel;
using Jason.Client.ComponentModel;

namespace Jason.Client.Runtime
{
	public class DefaultWorkerServiceClientFactory : IWorkerServiceClientFactory
	{
		public virtual void QueueChannelFactoryReset()
		{
			this.IsChannelFactoryResetQueued = true;
		}

		public bool IsChannelFactoryResetQueued
		{
			get;
			protected set;
		}

#if !SILVERLIGHT && !NETFX_CORE

		public IWorkerServiceClient CreateClient()
		{
			var client = new WorkerServiceClient( this.IsChannelFactoryResetQueued );

			return client;
		}

		public IWorkerServiceClient CreateClient( string endpointConfigurationName )
		{
			var client = new WorkerServiceClient( endpointConfigurationName, this.IsChannelFactoryResetQueued );

			return client;
		}

#endif

		public IAsyncWorkerServiceClient CreateAsyncClient()
		{
			var client = new AsyncWorkerServiceClient( this.IsChannelFactoryResetQueued );

			return client;
		}

		public IAsyncWorkerServiceClient CreateAsyncClient( string endpointConfigurationName )
		{
			var client = new AsyncWorkerServiceClient( endpointConfigurationName, this.IsChannelFactoryResetQueued );

			return client;
		}
	}
}
