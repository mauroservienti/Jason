//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security;
//using System.Text;
//using Topics.Radical.ComponentModel;

//namespace Jason.Handlers
//{
//	[Contract]
//	public interface ISecurityViolationHandler<TContext>
//	{
//		ViolationHandlingResult OnSecurityViolation( TContext context, Object rawCommand, SecurityException exception );
//	}

//	public class ViolationHandlingResult 
//	{
//		public ViolationHandlingBehavior Behavior { get; set; }
//		public Object Result { get; set; }
//	}

//	public enum ViolationHandlingBehavior 
//	{
//		Throw,
//		Ignore,
//		Return
//	}
//}