using System.ServiceModel;
using System.ServiceModel.Channels;
using System;
using System.Collections.Generic;
using Jason.Model;

namespace Jason.Client.Runtime
{
	public abstract class WorkerServiceClientBase<TChannel> : ClientBase<TChannel>
		where TChannel : class
    {

#if !SILVERLIGHT
		public virtual void Dispose()
		{
			try
			{
#if NETFX_CORE
                ( ( ICommunicationObject )this ).Close();
#else
				this.Close();
#endif

			}
			catch
			{
				this.Abort();
			}
		}
#endif

        protected void ResetConfiguration()
		{
			var tmp = this.Endpoint.Name;
			this.Endpoint.Name = "temp_" + Guid.NewGuid().ToString();
			this.Endpoint.Name = tmp;
		}

		protected WorkerServiceClientBase()
		{

		}

		protected WorkerServiceClientBase( bool resetConfiguration )
		{
			if( resetConfiguration )
			{
				this.ResetConfiguration();
			}
		}

		protected WorkerServiceClientBase( bool resetConfiguration, Binding binding, EndpointAddress remoteAddress )
			: this( binding, remoteAddress )
		{
			if( resetConfiguration )
			{
				this.ResetConfiguration();
			}
		}

		protected WorkerServiceClientBase( string endpointConfigurationName )
			: base( endpointConfigurationName )
		{

		}

		protected WorkerServiceClientBase( string endpointConfigurationName, Boolean resetConfiguration )
			: base( endpointConfigurationName )
		{
			if( resetConfiguration )
			{
				this.ResetConfiguration();
			}
		}

		protected WorkerServiceClientBase( string endpointConfigurationName, string remoteAddress )
			: base( endpointConfigurationName, remoteAddress )
		{

		}

		protected WorkerServiceClientBase( string endpointConfigurationName, EndpointAddress remoteAddress )
			: base( endpointConfigurationName, remoteAddress )
		{

		}

		protected WorkerServiceClientBase( Binding binding, EndpointAddress remoteAddress )
			: base( binding, remoteAddress )
		{

		}

#if !SILVERLIGHT && !NETFX_CORE

        protected WorkerServiceClientBase( InstanceContext callbackInstance )
			: base( callbackInstance )
		{

		}

		protected WorkerServiceClientBase( InstanceContext callbackInstance, string endpointConfigurationName )
			: base( callbackInstance, endpointConfigurationName )
		{

		}

		protected WorkerServiceClientBase( InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress )
			: base( callbackInstance, endpointConfigurationName, remoteAddress )
		{

		}

		protected WorkerServiceClientBase( InstanceContext callbackInstance, string endpointConfigurationName, EndpointAddress remoteAddress )
			: base( callbackInstance, endpointConfigurationName, remoteAddress )
		{

		}

		protected WorkerServiceClientBase( InstanceContext callbackInstance, Binding binding, EndpointAddress remoteAddress )
			: base( callbackInstance, binding, remoteAddress )
		{

		}

#endif
	}
}
