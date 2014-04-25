using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jason.Configuration;
using Jason.Factories;

namespace Jason.Runtime
{
	public class DefaultInterceptorProvider : IInterceptorProvider
	{
		readonly IJasonDependencyResolver container;

		public DefaultInterceptorProvider( IJasonDependencyResolver container )
		{
			this.container = container;
		}

		public IEnumerable<Jason.Handlers.ICommandInterceptor> GetCommandInterceptors( object command )
		{
			return this.container.ResolveAll<Handlers.ICommandInterceptor>();
		}

		public void Release( IEnumerable<Jason.Handlers.ICommandInterceptor> interceptors )
		{
			foreach ( var instance in interceptors ) 
			{
				this.container.Release( instance );
			}
		}

		public IEnumerable<Handlers.ICommandSecurityInterceptor> GetSecurityInterceptors( object command )
		{
			return this.container.ResolveAll<Handlers.ICommandSecurityInterceptor>();
		}

		public void Release( IEnumerable<Handlers.ICommandSecurityInterceptor> interceptors )
		{
			foreach ( var instance in interceptors )
			{
				this.container.Release( instance );
			}
		}
	}
}
