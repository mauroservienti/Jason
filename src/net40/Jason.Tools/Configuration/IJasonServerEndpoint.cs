using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jason.Configuration
{
	public interface IJasonServerEndpoint
	{
		void Initialize( IJasonServerConfiguration configuration, IEnumerable<Type> types );
		void Teardown();
	}
}
