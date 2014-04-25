using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Topics.Radical.ComponentModel;
using Jason.Model;
using Topics.Radical.Validation;

namespace Jason.Handlers.Commands
{
	[Contract]
	public interface ICommandHandler<TCommand>
		: ICommandHandler
		where TCommand : class
	{
		Object Execute( TCommand command );
	}
}
