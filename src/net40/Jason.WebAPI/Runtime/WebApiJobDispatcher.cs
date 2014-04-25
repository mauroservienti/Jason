using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Text;
using System.Transactions;
using System.Web.Http.ModelBinding;
using Jason.ComponentModel;
using Jason.Configuration;
using Jason.Factories;
using Jason.Handlers.Commands;
using Jason.WebAPI.ComponentModel;
using Topics.Radical.Diagnostics;
using Topics.Radical.Linq;
using Topics.Radical.Validation;

namespace Jason.WebAPI.Runtime
{
	public class WebApiJobDispatcher : IWebApiJobDispatcher
	{
		static readonly TraceSource logger = new TraceSource( "Jason" );

		readonly IWebApiCommandDispatcher commandDispatcher;

		public WebApiJobDispatcher( IWebApiCommandDispatcher commandDispatcher )
		{
			this.commandDispatcher = commandDispatcher;
		}

		Boolean IsErrorResponse( Object response )
		{
			if ( response is HttpResponseMessage )
			{
				return !( ( HttpResponseMessage )response ).IsSuccessStatusCode;
			}

			return false;
		}

		public object DispatchJob( HttpRequestMessage request, Jason.ComponentModel.IJob job )
		{
			//var tx = job.ExecutionBehavior == JobExecutionBehavior.InTransaction ?
			//	new TransactionScope( TransactionScopeOption.Required )
			//	: null;

			//try
			//{
			//	List<Object> responses = new List<object>();
			//	foreach ( var cmd in job.WorkItems )
			//	{
			//		var commandResponse = this.commandDispatcher.DispatchCommand( request, cmd );
			//		if ( !this.IsErrorResponse( commandResponse ) )
			//		{
			//			responses.Add( commandResponse );
			//		}
			//		else 
			//		{
			//			return commandResponse;
			//		}

			//	}

			//	if ( job.ExecutionBehavior == JobExecutionBehavior.InTransaction )
			//	{
			//		tx.Complete();
			//	}
			//}
			//finally
			//{
			//	if ( job.ExecutionBehavior == JobExecutionBehavior.InTransaction )
			//	{
			//		tx.Dispose();
			//	}
			//}

			return null;
		}

		//public Object DispatchCommand( HttpRequestMessage request, Object command )
		//{
		//	HttpResponseMessage response;

		//	if ( !this.IsCommandAllowed( request, command, out response ) )
		//	{
		//		return response;
		//	}

		//	if ( !this.IsCommandValid( request, command, out response ) )
		//	{
		//		return response;
		//	}

		//	logger.Debug( "Loading command interceptors." );
		//	var interceptors = this.interceptorProvider.GetCommandInterceptors( command );

		//	try
		//	{
		//		interceptors.ForEach( i =>
		//		{
		//			logger.Debug( "Invoking command interceptor OnExecute: {0}.", i );
		//			i.OnExecute( command );
		//		} );

		//		var result = this.ExecuteCommandUnderRetryPolicy( command );

		//		interceptors.ForEach( i =>
		//		{
		//			logger.Debug( "Invoking command interceptor OnExecuted: {0}.", i );
		//			i.OnExecuted( command, result );
		//		} );

		//		return result;
		//	}
		//	catch ( Exception error )
		//	{
		//		logger.Error( error.Message, error );

		//		interceptors.ForEach( i =>
		//		{
		//			logger.Debug( "Invoking command interceptor OnException: {0}.", i );
		//			i.OnException( command, error );
		//		} );

		//		logger.Debug( "Returning HTTP-500." );
		//		return request.CreateErrorResponse( HttpStatusCode.InternalServerError, error );
		//	}
		//	finally
		//	{
		//		logger.Debug( "Releasing command interceptors." );
		//		this.interceptorProvider.Release( interceptors );
		//	}
		//}
	}
}
