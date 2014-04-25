using System;
using System.Linq;
using System.ComponentModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Jason.ComponentModel;
using Jason.Client.ComponentModel;
using Jason.Model;
using System.Threading.Tasks;

namespace Jason.Client.Runtime
{
	public class AsyncWorkerServiceClient : WorkerServiceClientBase<IAsyncWorkerService>, IAsyncWorkerServiceClient
	{
		public event EventHandler<AsyncCompletedEventArgs> OpenCompleted;

		public AsyncWorkerServiceClient()
		{

		}

		public AsyncWorkerServiceClient( bool resetConfiguration )
			: base( resetConfiguration )
		{

		}

		public AsyncWorkerServiceClient( string endpointConfigurationName )
			: base( endpointConfigurationName )
		{

		}

		public AsyncWorkerServiceClient( string endpointConfigurationName, Boolean resetConfiguration )
			: base( endpointConfigurationName, resetConfiguration )
		{

		}

		public AsyncWorkerServiceClient( string endpointConfigurationName, string remoteAddress )
			: base( endpointConfigurationName, remoteAddress )
		{

		}

		public AsyncWorkerServiceClient( string endpointConfigurationName, EndpointAddress remoteAddress )
			: base( endpointConfigurationName, remoteAddress )
		{

		}

		public AsyncWorkerServiceClient( Binding binding, EndpointAddress remoteAddress )
			: base( binding, remoteAddress )
		{

		}

		private IAsyncResult OnBeginOpen( object[] inValues, AsyncCallback callback, object asyncState )
		{
			return ( ( ICommunicationObject )this ).BeginOpen( callback, asyncState );
		}

		private object[] OnEndOpen( IAsyncResult result )
		{
			( ( ICommunicationObject )this ).EndOpen( result );
			return null;
		}

		private void OnOpenCompleted( object state )
		{
			var h = this.OpenCompleted;
			if( h != null )
			{
				var e = ( ( InvokeAsyncCompletedEventArgs )state );
				this.OpenCompleted( this, new AsyncCompletedEventArgs( e.Error, e.Cancelled, e.UserState ) );
			}
		}

		public void OpenAsync()
		{
			this.OpenAsync( null );
		}

		public void OpenAsync( object userState )
		{
			this.InvokeAsync( OnBeginOpen, null, OnEndOpen, OnOpenCompleted, userState );
		}

		private IAsyncResult OnBeginClose( object[] inValues, AsyncCallback callback, object asyncState )
		{
			return ( ( ICommunicationObject )this ).BeginClose( callback, asyncState );
		}

		private object[] OnEndClose( IAsyncResult result )
		{
			( ( ICommunicationObject )this ).EndClose( result );
			return null;
		}

		private void OnCloseCompleted( object state )
		{
			var e = ( ( InvokeAsyncCompletedEventArgs )state );
			this.CloseCompleted( new AsyncCompletedEventArgs( e.Error, e.Cancelled, e.UserState ) );
		}

		void CloseCompleted( AsyncCompletedEventArgs args )
		{
			if( args.Error != null )
			{
				this.Abort();
			}
		}

		public void CloseAsync()
		{
			this.CloseAsync( null );
		}

		public void CloseAsync( object userState )
		{
			this.InvokeAsync( OnBeginClose, null, OnEndClose, OnCloseCompleted, userState );
		}

		public override void Dispose()
		{
			this.CloseAsync();
		}

		IAsyncResult IAsyncWorkerServiceClient.BeginWorkOn( Job job, AsyncCallback callback, object asyncState )
		{
			return this.Channel.BeginWorkOn( job, callback, asyncState );
		}

		JobExecutionResult IAsyncWorkerServiceClient.EndWorkOn( IAsyncResult result )
		{
			return this.Channel.EndWorkOn( result );
		}

		public Task<JobExecutionResult> WorkOnAsync( Job job )
		{
			var task = System.Threading.Tasks.Task.Factory.FromAsync<JobExecutionResult>(
				( ac, obj ) => this.Channel.BeginWorkOn( job, ac, obj ),
				ar => this.Channel.EndWorkOn( ar ),
				job );

			return task;
		}

		IAsyncResult IAsyncWorkerServiceClient.BeginExecute( Object command, AsyncCallback callback, object asyncState )
		{
			return this.Channel.BeginExecute( command, callback, asyncState );
		}

		Object IAsyncWorkerServiceClient.EndExecute( IAsyncResult result )
		{
			return this.Channel.EndExecute( result );
		}

		public Task<Object> ExecuteAsync( Object command )
		{
			var task = Task.Factory.FromAsync<Object>(
				( ac, obj ) => this.Channel.BeginExecute( command, ac, obj ),
				ar => this.Channel.EndExecute( ar ),
				command );

			return task;
		}

		public void Run( Job job ) 
		{
			this.Channel.BeginRun( job, r => { }, null );
		}
	}
}
