using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical.Reflection;

namespace Jason.WebAPI.Runtime
{
	class InMemoryCommandMapper
	{
		Dictionary<String, Type> mappings = new Dictionary<string, Type>();

		public void CreateMapping( Type commandType )
		{
			var key = commandType.Name.ToLowerInvariant();
			if( mappings.ContainsKey( key ) )
			{
				var mapping = mappings[ key ];

				var msg = String.Format( "Cannot map '{0}'. Key '{1}' is already mapped to '{2}'", 
					commandType.ToShortString(), 
					key, 
					mapping.ToShortString() );
				
				throw new NotSupportedException( msg );
			}

			mappings.Add( key, commandType );
		}

		public Type GetMappedType( String key )
		{
			return mappings[ key.ToLowerInvariant() ];
		}
	}
}
