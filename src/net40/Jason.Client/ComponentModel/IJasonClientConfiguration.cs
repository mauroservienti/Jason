using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Jason.Client.ComponentModel
{
	public interface IJasonClientConfiguration
	{
#if !NETFX_CORE
		Func<Type, bool> CommandsSelector { get; set; }
        Func<String, Boolean> AssemblySelector { get; set; }
#else
        Func<TypeInfo, bool> CommandsSelector { get; set; }
#endif
		Task Initialize();
	}
}
