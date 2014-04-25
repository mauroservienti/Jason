using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Topics.Radical.ComponentModel;

namespace Jason.Handlers
{
	[Contract]
	public interface ICommandInterceptor
	{
		void OnExecute( Object rawCommand );
		void OnExecuted( Object rawCommand, Object rawResult );
		void OnException( Object rawCommand, Exception exception );
	}
}