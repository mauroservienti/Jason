using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jason.Factories
{
	public interface IInterceptorProvider
	{
		IEnumerable<Jason.Handlers.ICommandInterceptor> GetCommandInterceptors( Object command );

		IEnumerable<Jason.Handlers.ICommandSecurityInterceptor> GetSecurityInterceptors( Object command );

		void Release( IEnumerable<Jason.Handlers.ICommandInterceptor> interceptors );

		void Release( IEnumerable<Jason.Handlers.ICommandSecurityInterceptor> interceptors );
	}
}
