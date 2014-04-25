using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jason
{
	public static class Defaults
	{
		public class Response
		{
			public static readonly Response Ok = new Response() { Status = ResponseStatus.Ok };

			public ResponseStatus Status { get; set; }

			//public static Response CreateFault( Topics.Radical.Validation.ValidationResults validationResult )
			//{
			//	throw new NotImplementedException();
			//}
		}
	}

	public enum ResponseStatus
	{
		Ok,
		//ValidationFailure
	}
}
