using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Jason.Factories;
using Jason.Handlers.Commands;
using Jason.Configuration;

namespace Jason.Runtime
{
	public class DefaultCommandHandleProvider : ICommandHandlerProvider
	{
		readonly IJasonDependencyResolver container;
		readonly IJasonServerConfiguration configuration;

		public DefaultCommandHandleProvider( IJasonDependencyResolver container, IJasonServerConfiguration configuration )
		{
			this.container = container;
			this.configuration = configuration;
		}

		public ICommandHandler GetHandlerFor( Object command )
		{
			var ct = command.GetType();
			var generic = typeof( ICommandHandler<> );
			var handlerType = generic.MakeGenericType( ct );

			if ( this.container.IsRegistered( handlerType ) )
			{
				return ( ICommandHandler )this.container.Resolve( handlerType );
			}

			ICommandHandler fallback;
			if ( this.configuration.TryGetFallbackCommandHandler( out fallback ) )
			{
				return fallback;
			}

			throw new ArgumentException( String.Format( "Cannot find any valid command handler for command: {0}", command.GetType() ) );
		}

		public void Release( ICommandHandler handler )
		{
			this.container.Release( handler );
		}
	}
}
