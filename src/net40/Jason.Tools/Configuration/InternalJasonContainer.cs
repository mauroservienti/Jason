using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Topics.Radical;
using Topics.Radical.ComponentModel;

namespace Jason.Configuration
{
	class InternalJasonContainer : IJasonContainer
	{
		PuzzleContainer defaultContainer = new PuzzleContainer();

		public void RegisterAsTransient( IEnumerable<Type> contracts, Type implementation )
		{
			this.defaultContainer.Register(
				EntryBuilder.For( contracts.First() )
					.ImplementedBy( implementation )
					.WithLifestyle( Lifestyle.Transient )
			);
		}

		public void RegisterAsSingleton( IEnumerable<Type> contracts, Type implementation )
		{
			this.defaultContainer.Register(
				EntryBuilder.For( contracts.First() )
					.ImplementedBy( implementation )
					.WithLifestyle( Lifestyle.Singleton )
			);
		}

		public void RegisterInstance( IEnumerable<Type> contracts, object instance )
		{
			this.defaultContainer.Register(
				EntryBuilder.For( contracts.First() )
					.UsingInstance( instance )
					.WithLifestyle( Lifestyle.Singleton )
			);
		}

		public void RegisterTypeAsTransient( Type implementation )
		{
			this.defaultContainer.Register(
				EntryBuilder.For( implementation )
					.WithLifestyle( Lifestyle.Transient )
			);
		}

		public void RegisterTypeAsSingleton( Type implementation )
		{
			this.defaultContainer.Register(
				EntryBuilder.For( implementation )
					.WithLifestyle( Lifestyle.Singleton )
			);
		}

		public IEnumerable<T> ResolveAll<T>()
		{
			return this.defaultContainer.ResolveAll<T>();
		}

		public T Resolve<T>()
		{
			return this.defaultContainer.Resolve<T>();
		}

		public IEnumerable<object> ResolveAll( Type t )
		{
			return this.defaultContainer.ResolveAll( t );
		}

		public object Resolve( Type t )
		{
			return this.defaultContainer.Resolve( t );
		}

		public void Release( object instance )
		{
			//NOP
		}

		public object GetService( Type serviceType )
		{
			if ( this.defaultContainer.IsRegistered( serviceType ) ) 
			{
				return this.defaultContainer.Resolve( serviceType );
			}

			return null;
		}


		public bool IsRegistered( Type type )
		{
			return this.defaultContainer.IsRegistered( type );
		}

		public bool IsRegistered<TType>()
		{
			return this.IsRegistered( typeof( TType ) );
		}
	}
}
