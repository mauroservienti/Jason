using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Jason.Model;

namespace Jason.ComponentModel
{
	/// <summary>
	/// Defines a custom exception that wraps server side <see cref="ErrorInfo"/>.
	/// </summary>
#if !SILVERLIGHT
	[Serializable]
#endif
	public class ErrorInfoException : Exception
	{
		/// <summary>
		/// Gets the error.
		/// </summary>
		/// <value>The error.</value>
		public IEnumerable<ErrorInfo> Errors { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ErrorInfoException"/> class.
		/// </summary>
		/// <param name="errors">The errors.</param>
		public ErrorInfoException( IEnumerable<ErrorInfo> errors ) 
			: base() 
		{
			this.Errors = errors;
		}

#if !SILVERLIGHT
		/// <summary>
		/// Initializes a new instance of the <see cref="ErrorInfoException"/> class.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
		protected ErrorInfoException( SerializationInfo info, StreamingContext context )
			: base( info, context )
		{

		}
#endif
	}
}
