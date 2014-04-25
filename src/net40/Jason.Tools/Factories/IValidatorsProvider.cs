using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Topics.Radical.ComponentModel.Validation;

namespace Jason.Factories
{
	public interface IValidatorsProvider
	{
		IEnumerable<IValidator> GetValidators( Object command );
		void Release( IEnumerable<IValidator> validators );
	}
}
