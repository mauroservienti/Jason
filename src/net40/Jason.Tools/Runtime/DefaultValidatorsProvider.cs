using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jason.Configuration;
using Jason.Factories;
using Topics.Radical.ComponentModel.Validation;

namespace Jason.Runtime
{
	public class DefaultValidatorsProvider : IValidatorsProvider
	{
		readonly IJasonDependencyResolver container;
		readonly IJasonServerConfiguration configuration;

		public DefaultValidatorsProvider( IJasonDependencyResolver container, IJasonServerConfiguration configuration )
		{
			this.container = container;
			this.configuration = configuration;
		}

		public IEnumerable<IValidator> GetValidators( object command )
		{
			var validatorType = typeof( IValidator<> ).MakeGenericType( command.GetType() );
			var validators = this.container.ResolveAll( validatorType ).Cast<IValidator>();

			IValidator fallback;
			if ( !validators.Any() && this.configuration.TryGetFallbackValidator( out fallback ) )
			{
				validators = new[] { fallback };
			}

			return validators;
		}

		public void Release( IEnumerable<Topics.Radical.ComponentModel.Validation.IValidator> validators )
		{
			foreach ( var instance in validators )
			{
				this.container.Release( instance );
			}
		}
	}
}
