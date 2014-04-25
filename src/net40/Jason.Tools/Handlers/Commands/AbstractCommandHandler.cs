using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Topics.Radical.Validation;
using System.Diagnostics;
using Topics.Radical.Diagnostics;
using Jason.Model;

namespace Jason.Handlers.Commands
{
	public abstract class AbstractCommandHandler<TCommand> :
		ICommandHandler<TCommand>
		where TCommand : class
	{
		public Object Execute( TCommand command )
		{
			var result = this.OnExecute( command );
			
			return result;
		}

		protected abstract Object OnExecute( TCommand command );

		Object ICommandHandler.Execute( object command )
		{
			return this.Execute( ( TCommand )command);
		}
	}
}