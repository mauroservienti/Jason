using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Topics.Radical.ComponentModel;
using Jason.Model;

namespace Jason.Handlers.Jobs
{
	[Contract]
	public interface IJobRunner<TJob>
		: IJobRunner
		where TJob : Job
	{
		void Run( TJob operation );
	}
}
