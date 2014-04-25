using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jason.Handlers.Commands;
using Jason.Model;
using SampleTasks;

namespace SampleWorkers
{
    class JasonIgnorantCommandHandler : AbstractCommandHandler<JasonIgnorantCommand>
    {
        protected override Object OnExecute( JasonIgnorantCommand command )
        {
            return new JasonIgnorantCommandResponse();
        }
    }
}
