using System;
using System.Collections.Generic;
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Jason.Configuration;

namespace Jason.Windsor
{
	public class WindsorJasonContainerProxy : IJasonContainer
	{
		readonly IWindsorContainer _container;

		public WindsorJasonContainerProxy( IWindsorContainer container )
		{
			this._container = container;
		}

		public void RegisterAsTransient( IEnumerable<Type> contracts, Type implementation )
		{
			this._container.Register( Component.For( contracts ).ImplementedBy( implementation ).LifestyleTransient().IsFallback() );
		}

		public void RegisterAsSingleton( IEnumerable<Type> contracts, Type implementation )
		{
			this._container.Register( Component.For( contracts ).ImplementedBy( implementation ).LifestyleSingleton().IsFallback() );
		}

		public void RegisterInstance( IEnumerable<Type> contracts, object instance )
		{
			this._container.Register( Component.For( contracts ).Instance( instance ).LifestyleSingleton().IsFallback() );
		}

		public void RegisterTypeAsTransient( Type implementation )
		{
			this._container.Register( Component.For( implementation ).LifestyleTransient().IsFallback() );
		}

		public void RegisterTypeAsSingleton( Type implementation )
		{
			this._container.Register( Component.For( implementation ).LifestyleSingleton().IsFallback() );
		}

		public IEnumerable<T> ResolveAll<T>()
		{
			return this._container.ResolveAll<T>();
		}

		public T Resolve<T>()
		{
			return this._container.Resolve<T>();
		}

		public IEnumerable<object> ResolveAll( Type t )
		{
			return this._container.ResolveAll( t ).Cast<Object>();
		}

		public object Resolve( Type t )
		{
			return this._container.Resolve( t );
		}

		public void Release( object instance )
		{
			this._container.Release( instance );
		}

		public object GetService( Type serviceType )
		{
			if ( this._container.Kernel.HasComponent( serviceType ) )
			{
				return this._container.Resolve( serviceType );
			}

			return null;
		}

		public bool IsRegistered( Type type )
		{
			return this._container.Kernel.HasComponent( type );
		}

		public bool IsRegistered<TType>()
		{
			return this.IsRegistered( typeof( TType ) );
		}
	}
}
