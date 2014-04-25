using System;
using System.Linq.Expressions;
using Topics.Radical.Linq;
using Topics.Radical.Validation;
using Topics.Radical.Reflection;
using System.ComponentModel.DataAnnotations;

namespace Jason.WebAPI.Validation
{
	public abstract class AbstractValidator<T> : ValidatorBase<T>
	{
		protected override string GetPropertyDisplayName( string propertyName, object entity )
		{
			var pi = entity.GetType().GetProperty( propertyName );
			if ( pi != null && pi.IsAttributeDefined<DisplayAttribute>() )
			{
				var a = pi.GetAttribute<DisplayAttribute>();
				return a.GetName();
			}

			return base.GetPropertyDisplayName( propertyName, entity );
		}

		public AbstractValidator()
			: base( null )
		{

		}

		protected void AddRule( Expression<Func<T, Object>> property, Func<ValidationContext<T>, String> error, Func<ValidationContext<T>, Boolean> rule )
		{
			this.AddRule( ctx =>
			{
				var result = rule( ctx );
				if ( !result )
				{
					var propertyName = property.GetMemberName();
					var displayname = this.GetPropertyDisplayName( propertyName, ctx.Entity );
					ctx.Results.AddError( new ValidationError(
						propertyName,
						displayname,
						new[] { error( ctx ) } ) );
				}
			} );
		}
	}
}
