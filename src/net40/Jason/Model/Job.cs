using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Topics.Radical.Validation;

namespace Jason.Model
{
	public class Job : AbstractDataObject
	{
		public String CorrelationId { get; set; }

		public Job()
		{
			this.CorrelationId = Guid.NewGuid().ToString();
		}

		public Job( params JobTask[] tasks )
			: this()
		{
			Ensure.That( tasks ).Named( () => tasks );

			this.Tasks = tasks;
		}

		public JobTask[] Tasks { get; set; }
	}
}
