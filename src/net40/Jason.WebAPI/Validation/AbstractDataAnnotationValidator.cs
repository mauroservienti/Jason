using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Topics.Radical.Linq;
using Topics.Radical.Validation;

namespace Jason.WebAPI.Validation
{
	public abstract class AbstractDataAnnotationValidator<T> : AbstractValidator<T>
	{
		public AbstractDataAnnotationValidator()
		{

		}

		protected override void OnValidate( ValidationContext<T> context )
		{
			var results = new List<ValidationResult>();
			var ctx = new ValidationContext( context.Entity, null, null );
			Validator.TryValidateObject( context.Entity, ctx, results, true );

			foreach ( var r in results )
			{
				var memberName = r.MemberNames.Single();
				var displayname = this.GetPropertyDisplayName( memberName, context.Entity );

				context.Results.AddError( new ValidationError( 
					memberName, 
					displayname,
					new[] { r.ErrorMessage } ));
			}

			base.OnValidate( context );
		}
	}
}
