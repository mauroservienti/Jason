using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Jason.ComponentModel
{
	public static class ServiceKnownTypesProvider
	{
		readonly static HashSet<Type> knownTypes = new HashSet<Type>();

#if !NETFX_CORE

		public static IEnumerable<Type> GetServiceKnownTypes( ICustomAttributeProvider provider )
		{
			return knownTypes.ToArray();
		}

#else

        public static IEnumerable<Type> GetServiceKnownTypes()
		{
			return knownTypes.ToArray();
		}

#endif

		public static void RegisterKnownType( Type type )
		{
			knownTypes.Add( type );
		}
	}
}