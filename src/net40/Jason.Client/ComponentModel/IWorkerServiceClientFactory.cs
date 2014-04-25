using System;
using Topics.Radical.ComponentModel;

namespace Jason.Client.ComponentModel
{
	[Contract]
	public interface IWorkerServiceClientFactory
	{
		void QueueChannelFactoryReset();

		Boolean IsChannelFactoryResetQueued { get; }

#if !SILVERLIGHT && !NETFX_CORE

		IWorkerServiceClient CreateClient();

		IWorkerServiceClient CreateClient( String endpointConfigurationName );

#endif

        IAsyncWorkerServiceClient CreateAsyncClient();

		IAsyncWorkerServiceClient CreateAsyncClient( String endpointConfigurationName );
	}
}
