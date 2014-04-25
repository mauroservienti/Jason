using System;
using System.Collections.Generic;
using Jason.Handlers.Commands;
using Topics.Radical.ComponentModel.Validation;
namespace Jason.Configuration
{
	//public delegate void Registrar( IEnumerable<Type> contracts, Type implementation );
	//public delegate void InstanceRegistrar( IEnumerable<Type> contracts, Object instance );

	public interface IJasonServerConfiguration
	{
		void Initialize();
		void Teardown();

		T GetEndpoint<T>() where T : IJasonServerEndpoint;
		IJasonServerConfiguration AddEndpoint( IJasonServerEndpoint endpoint );
		IJasonServerConfiguration UsingAsRetryPolicy<TPolicy>() where TPolicy : ICommandExecutionRetryPolicy;
		IJasonServerConfiguration UsingAsRetryPolicy<TPolicy>( TPolicy instance ) where TPolicy : ICommandExecutionRetryPolicy;

		IJasonContainer Container { get; set; }
		Func<String, Boolean> AssemblySelector { get; set; }

		IJasonServerConfiguration UsingAsFallbackCommandHandler<THandler>() where THandler : ICommandHandler;
		IJasonServerConfiguration UsingAsFallbackCommandValidator<TValidator>() where TValidator : IValidator;

		Boolean TryGetFallbackCommandHandler(out ICommandHandler handler);
		Boolean TryGetFallbackValidator(out IValidator validator);
	}
}
