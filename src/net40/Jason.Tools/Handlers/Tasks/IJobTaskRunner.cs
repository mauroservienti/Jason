using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jason.Model;

namespace Jason.Handlers.Tasks
{
	public interface IJobTaskRunner
	{
		void Run( JobTask task );
	}
}
