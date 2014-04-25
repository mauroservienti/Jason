using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jason.Model;
using Topics.Radical.Validation;

namespace Jason.Handlers.Commands
{
	public interface ICommandHandler
	{
		Object Execute( Object command );
	}
}
