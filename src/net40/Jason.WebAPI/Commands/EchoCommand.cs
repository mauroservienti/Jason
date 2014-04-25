using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jason.Handlers.Commands;

namespace Jason.WebAPI.Commands
{
    public class EchoCommand
    {
        public string Message { get; set; }
    }

    public class EchoCommandResponse
    {
        public string Message { get; set; }
    }

    public class EchoCommandHandler : AbstractCommandHandler<EchoCommand>
    {
        protected override object OnExecute( EchoCommand command )
        {
            return new EchoCommandResponse()
            {
                Message = String.Format( "Echo: {0}", command.Message )
            };
        }
    }
}
