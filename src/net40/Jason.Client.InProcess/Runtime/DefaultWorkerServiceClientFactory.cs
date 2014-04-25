using System;
using Jason.Client.ComponentModel;
using Jason.Configuration;
using Jason.Factories;

namespace Jason.Client.Runtime
{
    public class DefaultWorkerServiceClientFactory : IWorkerServiceClientFactory
    {
		readonly ICommandExecutionRetryPolicy retryPolicy;
        readonly ICommandHandlerProvider handlerProvider;
		readonly IInterceptorProvider interceptorProvider;
		readonly IValidatorsProvider validatorsProvider;

		public DefaultWorkerServiceClientFactory( ICommandExecutionRetryPolicy retryPolicy, ICommandHandlerProvider handlerProvider, IInterceptorProvider interceptorProvider, IValidatorsProvider validatorsProvider )
        {
			this.retryPolicy = retryPolicy;
            this.handlerProvider = handlerProvider;
			this.interceptorProvider = interceptorProvider;
			this.validatorsProvider = validatorsProvider;
        }

        public IWorkerServiceClient CreateClient()
        {
            var client = new WorkerServiceClient( this.retryPolicy, this.handlerProvider, this.interceptorProvider, this.validatorsProvider );

            return client;
        }
    }
}
