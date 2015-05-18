using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jason.Model;

namespace SampleTasks
{
	public class SampleCommand : Command
	{
		public String Descrizione { get; set; }
	}

	public class CreateDescription : Command
	{
		public String Descrizione { get; set; }
	}
}
