using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Topics.Radical.ServiceModel.Hosting;

namespace SampleJasonServer.Web.Hosting
{
	public class JasonServiceFactory : AbstractServiceHostFactory
	{
		protected override System.ServiceModel.ServiceHost OnCreateServiceHost( Type serviceType, Uri[] baseAddresses )
		{
			return new BasicDependencyInjectionServiceHost( Global.Container, serviceType, baseAddresses ); 
		}
	}
}