using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Topics.Radical.Validation;

namespace Jason.ComponentModel
{
	/// <summary>
	/// used to allow Wcf client inheritors to intercept comunication errors.
	/// </summary>
	public class ComunicationError
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ComunicationError"/> class.
		/// </summary>
		/// <param name="error">The error.</param>
		public ComunicationError( Exception error )
		{
			Ensure.That( error ).Named( () => error ).IsNotNull();

			this.Error = error;
		}

		/// <summary>
		/// Gets the error.
		/// </summary>
		/// <value>The error.</value>
		public Exception Error { get; private set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="ComunicationError"/> is handled.
		/// </summary>
		/// <value><c>true</c> if handled; otherwise, <c>false</c>.</value>
		public Boolean Handled { get; set; }
	}
}
