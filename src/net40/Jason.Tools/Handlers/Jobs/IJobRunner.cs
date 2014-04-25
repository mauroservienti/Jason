using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jason.Model;

namespace Jason.Handlers.Jobs
{
	public interface IJobRunner
	{
		void Run( Job job );
	}
}
