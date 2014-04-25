using System;

namespace Jason.Client.ComponentModel
{
	public interface IWorkerServiceClient : IDisposable
	{
		Object Execute( Object command );
	}
}
