using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jason.Configuration
{
	public interface IJasonContainer : IJasonDependencyResolver
	{
		void RegisterAsTransient( IEnumerable<Type> contracts, Type implementation );

		void RegisterAsSingleton( IEnumerable<Type> contracts, Type implementation );

		void RegisterInstance( IEnumerable<Type> contracts, Object instance );

		void RegisterTypeAsTransient( Type implementation );

		void RegisterTypeAsSingleton( Type implementation );
	}
}
