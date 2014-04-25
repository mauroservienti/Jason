using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jason.ComponentModel
{
	public interface IJob
	{
		JobExecutionBehavior ExecutionBehavior { get; }
		IEnumerable<Object> WorkItems { get; }
	}

	public enum JobExecutionBehavior
	{
		InTransaction = 0,
		None
	}
}
