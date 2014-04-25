using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using Jason.Handlers.Commands;

namespace Jason.Configuration
{
	public class RetryEventArgs
	{
		public RetryEventArgs( int currentRetryCount, object command, ICommandHandler handler, Exception error )
		{
			this.CurrentRetryCount = currentRetryCount;
			this.Command = command;
			this.Handler = handler;
			this.Error = error;
		}

		public Exception Error { get; private set; }
		public int CurrentRetryCount { get; private set; }
		public object Command { get; private set; }

		public ICommandHandler Handler { get; private set; }

		public Boolean Retry { get; set; }
		public Int32 Delay { get; set; }
	}

	public class DelegateCommandExecutionRetryPolicy : ICommandExecutionRetryPolicy
	{
		public DelegateCommandExecutionRetryPolicy()
		{
			this.ShouldRetry = s => s.Retry = false;
			this.DefaultRetryDelay = 0;
			this.RecreateHandler = true;
		}

		public object Execute( object command, Func<ICommandHandler> handlerFactory )
		{
			var errors = new List<Exception>();
			var retry = true;
			var retryCount = 0;
			var handler = handlerFactory();

			while ( retry )
			{
				try
				{
					retryCount++;
					return handler.Execute( command );
				}
				catch ( Exception ex )
				{
					errors.Add( ex );

					var args = new RetryEventArgs( retryCount, command, handler, ex )
					{
						Delay = this.DefaultRetryDelay,
					};

					this.ShouldRetry( args );

					if ( args.Retry )
					{
						retry = true;
						if ( this.DefaultRetryDelay > 0 )
						{
							Thread.Sleep( this.DefaultRetryDelay );
						}

						if ( this.RecreateHandler )
						{
							handler = handlerFactory();
						}
					}
					else
					{
						retry = false;
					}
				}
			}

			throw new AggregateException( errors );
		}

		public Action<RetryEventArgs> ShouldRetry { get; set; }

		public int DefaultRetryDelay { get; set; }

		public Boolean RecreateHandler { get; set; }
	}
}
