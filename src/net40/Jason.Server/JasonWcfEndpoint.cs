using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Jason.ComponentModel;
using Jason.Configuration;
using Jason.Server.WcfServices;
using Topics.Radical.Linq;
using Topics.Radical.Reflection;
using Topics.Radical.ServiceModel.Hosting;

namespace Jason.Server
{
	public class JasonWcfEndpoint : IJasonServerEndpoint
	{
		ServiceHost host = null;

		public JasonWcfEndpoint()
		{
			this.CommandsSelector = t => false;
			this.WorkerServiceType = typeof( WorkerService );
			this.EnableSelfHosting = false;
		}

		public Func<Type, Boolean> CommandsSelector { get; set; }
		public Type WorkerServiceType { get; set; }
		public Boolean EnableSelfHosting { get; set; }

		public void Initialize( IJasonServerConfiguration configuration, IEnumerable<Type> types )
		{
			configuration.Container.RegisterAsTransient( new[] { typeof( IWorkerService ) }, this.WorkerServiceType );

			types.Where( t =>
			{
				return ( !t.IsGenericType && t.IsAttributeDefined<Jason.Configuration.ServiceKnownTypeAttribute>( true ) )
					|| this.CommandsSelector( t );
			} )
			.ForEach( t => ServiceKnownTypesProvider.RegisterKnownType( t ) );

			if ( this.EnableSelfHosting )
			{
				this.host = new BasicDependencyInjectionServiceHost( configuration.Container, this.WorkerServiceType );
				this.host.Open();
			}
		}

		public void Teardown()
		{
			if ( this.host != null && this.host.State == CommunicationState.Opened )
			{
				this.host.Close();
			}

			var d = this.host as IDisposable;
			if ( d != null )
			{
				d.Dispose();
			}
		}
	}
}
