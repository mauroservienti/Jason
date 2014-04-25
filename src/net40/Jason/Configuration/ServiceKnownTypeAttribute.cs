using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jason.Configuration
{
	[AttributeUsage( AttributeTargets.Class | AttributeTargets.Enum, AllowMultiple = false, Inherited = true )]
	public sealed class ServiceKnownTypeAttribute : Attribute
	{

	}
}
