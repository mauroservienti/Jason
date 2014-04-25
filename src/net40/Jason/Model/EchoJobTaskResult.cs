using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jason.Model
{
	public sealed class EchoJobTaskResult : JobTaskResult
	{
		public String EchoedMessage { get; set; }
	}
}
