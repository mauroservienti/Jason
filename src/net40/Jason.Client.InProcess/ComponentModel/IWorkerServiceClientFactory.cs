using System;
using Topics.Radical.ComponentModel;

namespace Jason.Client.ComponentModel
{
	[Contract]
	public interface IWorkerServiceClientFactory
	{
        IWorkerServiceClient CreateClient();
	}
}
