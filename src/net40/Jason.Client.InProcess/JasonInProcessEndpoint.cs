using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jason.Client.ComponentModel;
using Jason.Client.Runtime;
using Jason.Configuration;

namespace Jason.Client
{
	public class JasonInProcessEndpoint : IJasonServerEndpoint
	{
		public void Initialize( IJasonServerConfiguration configuration, IEnumerable<Type> types )
		{
			configuration.Container.RegisterAsTransient( new[] { typeof( IWorkerServiceClientFactory ) }, typeof( DefaultWorkerServiceClientFactory ) );
		}

		public void Teardown()
		{

		}
	}
}
