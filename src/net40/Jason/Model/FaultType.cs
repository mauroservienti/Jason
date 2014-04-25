using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jason.Model
{
	/// <summary>
	/// Defines the type of error occurred server side.
	/// </summary>
	public enum FaultType
	{
		/// <summary>
		/// The error type cannot be determined.
		/// </summary>
		Unknown,

		/// <summary>
		/// The error is an infrastructure level error.
		/// </summary>
		Infrastructure,
		
		/// <summary>
		/// The error is in the operation handling user code.
		/// </summary>
		OperationHandling,

		/// <summary>
		/// The error is in the message handling user code.
		/// </summary>
		MessageHandling
	}
}
