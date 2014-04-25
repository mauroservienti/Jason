using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jason.Handlers.Commands;
using Jason.Model;

namespace Jason.Factories
{
	public interface ICommandHandlerProvider
	{
		ICommandHandler GetHandlerFor( Object command );

		void Release( ICommandHandler handler );
	}
}
