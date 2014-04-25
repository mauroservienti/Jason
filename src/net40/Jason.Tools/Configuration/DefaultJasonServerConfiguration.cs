using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Jason.ComponentModel;
using Jason.Handlers.Commands;
using Jason.Handlers.Jobs;
using Jason.Handlers.Tasks;
using Topics.Radical.ComponentModel;
using Topics.Radical.Linq;
using Topics.Radical;
using Topics.Radical.Reflection;
using Topics.Radical.Validation;
using Jason.Handlers;
using Jason.Runtime;
using Jason.Factories;
using Topics.Radical.ComponentModel.Validation;

namespace Jason.Configuration
{
	public class DefaultJasonServerConfiguration : IJasonServerConfiguration
	{
		readonly List<IJasonServerEndpoint> endpoints = new List<IJasonServerEndpoint>();
		String path;

		public DefaultJasonServerConfiguration( String pathToScanForAssemblies, String assemblySelectPattern = "*" )
		{
			Ensure.That( pathToScanForAssemblies )
				.Named( () => pathToScanForAssemblies )
				.IsNotNullNorEmpty()
				.WithMessage( "Cannot find the given path." )
				.IsTrue( s => Directory.Exists( s ) );

			this.Container = new InternalJasonContainer();
			this.AssemblySelector = name => name.IsLike( "jason.*", assemblySelectPattern );
			this.TypeFilter = t => true;

			this.path = pathToScanForAssemblies;
		}

		public IJasonServerConfiguration AddEndpoint( IJasonServerEndpoint endpoint )
		{
			this.endpoints.Add( endpoint );

			return this;
		}

		public T GetEndpoint<T>() where T : IJasonServerEndpoint 
		{
			return this.endpoints.OfType<T>().SingleOrDefault();
		}

		public Func<String, Boolean> AssemblySelector { get; set; }
		public Func<Type, Boolean> TypeFilter { get; set; }

		public IJasonContainer Container { get; set; }

		Type retryPolicyType = typeof( DefaultCommandExecutionRetryPolicy );
		ICommandExecutionRetryPolicy retryPolicyInstance = null;

		public IJasonServerConfiguration UsingAsRetryPolicy<TPolicy>() where TPolicy : ICommandExecutionRetryPolicy
		{
			this.retryPolicyType = typeof( TPolicy );

			return this;
		}

		public IJasonServerConfiguration UsingAsRetryPolicy<TPolicy>( TPolicy instance ) where TPolicy : ICommandExecutionRetryPolicy
		{
			this.retryPolicyType = typeof( TPolicy );
			this.retryPolicyInstance = instance;

			return this;
		}

		public virtual void Initialize()
		{
			this.Container.RegisterInstance( new[] { typeof( IJasonDependencyResolver ) }, this.Container );
			this.Container.RegisterInstance( new[] { typeof( IJasonServerConfiguration ) }, this );

			if ( this.retryPolicyInstance == null )
			{
				this.Container.RegisterAsTransient( new[] { typeof( ICommandExecutionRetryPolicy ) }, this.retryPolicyType );
			}
			else
			{
				this.Container.RegisterInstance( new[] { typeof( ICommandExecutionRetryPolicy ) }, this.retryPolicyInstance );
			}

			//this.Container.RegisterAsSingleton( new[] { typeof( IJasonService ) }, typeof( DefaultJasonService ) );
			this.Container.RegisterAsSingleton( new[] { typeof( IJobHandlersProvider ) }, typeof( DefaultJobHandlersProvider ) );
			this.Container.RegisterAsSingleton( new[] { typeof( IJobTaskHandlersProvider ) }, typeof( DefaultJobTaskHandlersProvider ) );
			this.Container.RegisterAsSingleton( new[] { typeof( ICommandHandlerProvider ) }, typeof( DefaultCommandHandleProvider ) );
			this.Container.RegisterAsSingleton( new[] { typeof( IInterceptorProvider ) }, typeof( DefaultInterceptorProvider ) );
			this.Container.RegisterAsSingleton( new[] { typeof( IValidatorsProvider ) }, typeof( DefaultValidatorsProvider ) );

			var allMergedTypes = Directory.EnumerateFiles( this.path, "*.dll" )
				.Where( dll =>
				{
					var name = Path.GetFileNameWithoutExtension( dll );
					var include = this.AssemblySelector( name );

					return include;
				} )
				.SelectMany( dll =>
				{
					var name = Path.GetFileNameWithoutExtension( dll );
					var allTypes = Assembly.Load( name ).GetTypes();

					return allTypes;
				} )
				.Where( t => this.TypeFilter( t ) )
				.ToArray();


			RegisterByContract
			(
				allMergedTypes,
				this.Container.RegisterAsTransient,
				t =>
				{
					return !t.IsAbstract &&
						(
							t.Is<IJobRunner>() ||
							t.Is<IJobWorker>()
						);
				}
			);

			RegisterByContract
			(
				allMergedTypes,
				this.Container.RegisterAsTransient,
				t =>
				{
					return !t.IsAbstract &&
						(
							t.Is<IJobTaskRunner>() ||
							t.Is<IJobTaskWorker>()
						);
				}
			);

			RegisterByContract
			(
				allMergedTypes,
				this.Container.RegisterAsTransient,
				t => !t.IsAbstract && t.Is<ICommandHandler>()
			);

			var userInterceptors = allMergedTypes.Where( t => !t.IsAbstract && t.Is<ICommandInterceptor>() && !t.Is<EmptyCommandInterceptor>() ).ToArray();
			RegisterByContract
			(
				userInterceptors.Any() ? userInterceptors : new[] { typeof( EmptyCommandInterceptor ) },
				this.Container.RegisterAsTransient,
				t => true
			);

			var securityInterceptors = allMergedTypes.Where( t => !t.IsAbstract && t.Is<ICommandSecurityInterceptor>() && !t.Is<EmptyCommandSecurityInterceptor>() ).ToArray();
			RegisterByContract
			(
				securityInterceptors.Any() ? securityInterceptors : new[] { typeof( EmptyCommandSecurityInterceptor ) },
				this.Container.RegisterAsTransient,
				t => true
			);

			this.endpoints.ForEach( e => e.Initialize( this, allMergedTypes ) );
		}

		static void RegisterByContract( IEnumerable<Type> types, Action<IEnumerable<Type>, Type> registrar, Predicate<Type> filter )
		{
			types.Where( t => filter( t ) )
				.Select( t => new
				{
					Implementation = t,
					Contracts = t.GetInterfaces().Where( i => i.IsAttributeDefined<ContractAttribute>() ).ToArray()
				} )
				.ForEach( def =>
				{
					registrar( def.Contracts, def.Implementation );
				} );
		}


		public void Teardown()
		{
			this.endpoints.ForEach( e => e.Teardown() );
		}

		Type fallbackCommandHandlerType = null;

		public IJasonServerConfiguration UsingAsFallbackCommandHandler<THandler>() where THandler : ICommandHandler
		{
			this.fallbackCommandHandlerType = typeof( THandler );

			return this;
		}

		Type fallbackCommandValidatorType = null;

		public IJasonServerConfiguration UsingAsFallbackCommandValidator<TValidator>() where TValidator : Topics.Radical.ComponentModel.Validation.IValidator
		{
			this.fallbackCommandValidatorType = typeof( TValidator );

			return this;
		}

		public bool TryGetFallbackCommandHandler( out ICommandHandler handler )
		{
			if ( this.fallbackCommandHandlerType != null )
			{
				if ( !this.Container.IsRegistered( this.fallbackCommandHandlerType ) )
				{
					this.Container.RegisterTypeAsSingleton( this.fallbackCommandHandlerType );
				}

				handler = ( ICommandHandler )this.Container.Resolve( this.fallbackCommandHandlerType );
			}
			else
			{
				handler = null;
			}

			return handler != null;
		}

		public bool TryGetFallbackValidator( out IValidator validator )
		{
			if ( this.fallbackCommandValidatorType != null )
			{
				if ( !this.Container.IsRegistered( this.fallbackCommandValidatorType ) )
				{
					this.Container.RegisterTypeAsSingleton( this.fallbackCommandValidatorType );
				}

				validator = ( IValidator )this.Container.Resolve( this.fallbackCommandValidatorType );
			}
			else
			{
				validator = null;
			}

			return validator != null;
		}
	}
}
