using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Jason.Configuration;

namespace Jason.Server
{
	public class JasonServiceHost : ServiceHost
	{
		private readonly IJasonDependencyResolver container;
		Type serviceType;

		public JasonServiceHost( IJasonDependencyResolver container, Type serviceType, params Uri[] baseAddresses )
			: base( serviceType, baseAddresses )
		{
			this.container = container;
			this.serviceType = serviceType;
		}

		protected override void OnOpening()
		{
			this.Description.Behaviors.Add( new Topics.Radical.ServiceModel.Hosting.BasicDependencyInjectionServiceBehavior( this.container, this.serviceType)); // DependencyInjectionServiceBehavior( container ) );

			base.OnOpening();
		}
	}
}
