using System;
using System.Globalization;
using System.Text;

namespace Jason.Model
{
	/// <summary>
	/// Represents a data object specifi to transport error data on the wire.
	/// </summary>
	public class ErrorInfo
	{
		/// <summary>
		/// Builds a new ErrorInfo instance from the given exception.
		/// </summary>
		/// <param name="source">The source excection.</param>
		/// <param name="type">The type of fault.</param>
		/// <returns>
		/// The ErrorInfo instance that represents the given source exception.
		/// </returns>
		public static ErrorInfo FromException( Exception source, FaultType type )
		{
			return new ErrorInfo( source, type );
		}

		/// <summary>
		/// Gets or sets the inner error.
		/// </summary>
		/// <value>The inner error.</value>
		public ErrorInfo InnerError { get; set; }

		/// <summary>
		/// Gets or sets the error message.
		/// </summary>
		/// <value>The message.</value>
		public string Message { get; set; }

		/// <summary>
		/// Gets or sets the stack trace.
		/// </summary>
		/// <value>The stack trace.</value>
		public string StackTrace { get; set; }

		/// <summary>
		/// Gets or sets the source exception type.
		/// </summary>
		/// <value>The exception type.</value>
		public string Type { get; set; }

		/// <summary>
		/// Gets or sets the type of the fault.
		/// </summary>
		/// <value>The type of the fault.</value>
		public FaultType FaultType { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ErrorInfo"/> class.
		/// </summary>
		public ErrorInfo() 
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ErrorInfo"/> class.
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <param name="faultType">Type of the fault.</param>
		private ErrorInfo( Exception exception, FaultType faultType )
		{
			this.Message = exception.Message;
			this.StackTrace = exception.StackTrace;
			this.Type = exception.GetType().ToString();
			this.FaultType = faultType;

			if( exception.InnerException != null )
			{
				this.InnerError = new ErrorInfo( exception.InnerException, faultType );
			}
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return String.Format( CultureInfo.InvariantCulture, 
				"{0}{1}{2}",
				"ErrorInfo:",
				Environment.NewLine,
				this.ToStringHelper( false ) );
		}

		String ToStringHelper( bool isInner )
		{
			var builder = new StringBuilder();
			builder.AppendFormat( "{0}: {1}", this.Type, this.Message );

			if( this.InnerError != null )
			{
				builder.AppendFormat( " ----> {0}", this.InnerError.ToStringHelper( true ) );
			}
			else
			{
				builder.Append( "\n" );
			}

			builder.Append( StackTrace );

			if( isInner )
			{
				builder.AppendFormat( "\n   {0}\n", "--- End of inner ErrorInfo stack trace ---" );
			}

			return builder.ToString();
		}
	}
}