using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jason.Model;

namespace Jason.Handlers.Tasks
{
	public sealed class EchoJobTaskWorker : AbstractJobTaskWorker<EchoJobTask>
	{
		protected override JobTaskResult OnWorkOn( EchoJobTask message )
		{
			var response = this.CreateCorrelatedTaskResult<EchoJobTaskResult>( message, r => 
			{
				r.EchoedMessage = String.Format("Echoed from Jason: {0}",  message.Message );
			} );

			return response;
		}
	}
}
