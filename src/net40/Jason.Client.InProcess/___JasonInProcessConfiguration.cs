//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Jason.Configuration;

//namespace Jason.Client.InProcess
//{
//	public class JasonInProcessConfiguration : DefaultJasonServerConfiguration
//	{
//		public JasonInProcessConfiguration( String pathToScanForAssemblies, String assemblySelectPattern = "*" )
//			: base( pathToScanForAssemblies, assemblySelectPattern )
//		{
//			CommandsSelector = t => t.IsGenericType && t.Namespace != null && t.Namespace.EndsWith( ".Commands" );
//		}
//	}
//}
