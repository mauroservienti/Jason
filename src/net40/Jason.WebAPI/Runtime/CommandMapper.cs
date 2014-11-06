using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jason.WebAPI.Runtime
{
	class CommandMapper
	{
		Dictionary<String, Type> mappings = new Dictionary<string, Type>();

		public void CreateMapping( Type commandType ) 
		{
			if( mappings.ContainsKey( commandType.Name.ToLowerInvariant() ) ) 
			{
				throw new NotSupportedException( "Already mapped" );
			}

			mappings.Add( commandType.Name.ToLowerInvariant(), commandType );
		}

		public Type GetMappedType( String key ) 
		{
			return mappings[ key.ToLowerInvariant() ];
		}
	}
}
