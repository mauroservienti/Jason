using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Jason.Client.ComponentModel;
using Jason.Model;
using SampleTasks;
using Topics.Radical.Windows.Input;
using Topics.Radical.Windows.Presentation;

namespace SampleJasonServer.Silverlight
{
	public class MainPageViewModel : AbstractViewModel
	{
		IWorkerServiceClientFactory factory;

		public MainPageViewModel( IWorkerServiceClientFactory factory )
		{
			this.factory = factory;

			this.ExecuteEcho = DelegateCommand.Create()
				.OnExecute( o =>
				{
					var context = TaskScheduler.FromCurrentSynchronizationContext();
					using( var client = this.factory.CreateAsyncClient() )
					{
						client.WorkOnAsync( new Job( new EchoJobTask() { Message = this.Message } ) )
							.ContinueWith( e =>
							{
								var r = e.Result.GetTaskResult<EchoJobTaskResult>().EchoedMessage;
								this.EchoResult = r;
							}, context );
					}

					using( var client = this.factory.CreateAsyncClient() )
					{
						client.ExecuteAsync( new SampleCommand() )
							.ContinueWith( e =>
							{
								var r = e.Result as EmptyCommandExecutionResult;
							}, context );
					}
				} );
		}

		public String Message
		{
			get { return this.GetPropertyValue( () => this.Message ); }
			set { this.SetPropertyValue( () => this.Message, value ); }
		}

		public String EchoResult
		{
			get { return this.GetPropertyValue( () => this.EchoResult ); }
			set { this.SetPropertyValue( () => this.EchoResult, value ); }
		}

		public ICommand ExecuteEcho { get; private set; }
	}
}
