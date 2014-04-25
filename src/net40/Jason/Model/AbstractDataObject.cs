using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Jason.Configuration;

namespace Jason.Model
{
	[ServiceKnownTypeAttribute]
	public class AbstractDataObject
#if !SILVERLIGHT && !NETFX_CORE
		: IExtensibleDataObject
#endif
    {
#if !SILVERLIGHT && !NETFX_CORE

		ExtensionDataObject IExtensibleDataObject.ExtensionData
		{
			get;
			set;
		}

#endif

    }
}
