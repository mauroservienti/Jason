using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jason.Configuration
{
	/// <summary>
	/// Represents a facade to abstract the interface of a dependency container.
	/// </summary>
	public interface IJasonDependencyResolver : IServiceProvider
	{
		/// <summary>
		/// Resolves all the dependencies identified by the given type.
		/// </summary>
		/// <typeparam name="T">The type of the dependency.</typeparam>
		/// <returns></returns>
		IEnumerable<T> ResolveAll<T>();

		/// <summary>
		/// Resolves the dependency identified by the given type.
		/// </summary>
		/// <typeparam name="T">The type of the dependency.</typeparam>
		/// <returns></returns>
		T Resolve<T>();

		/// <summary>
		/// Resolves all the dependencies identified by the given type.
		/// </summary>
		/// <param name="t">The type of the dependency.</param>
		/// <returns></returns>
		IEnumerable<Object> ResolveAll( Type t );

		/// <summary>
		/// Resolves the dependency identified by the given type.
		/// </summary>
		/// <param name="t">The type of the dependency.</param>
		/// <returns></returns>
		Object Resolve( Type t );

		/// <summary>
		/// Releases the specified instance.
		/// </summary>
		/// <param name="instance">The instance.</param>
		void Release( Object instance );

		/// <summary>
		/// Determines whether the specified type is registered in the container.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		Boolean IsRegistered( Type type );

		/// <summary>
		/// Determines whether the specified type is registered in the container.
		/// </summary>
		/// <typeparam name="TType">The type of the component.</typeparam>
		/// <returns></returns>
		Boolean IsRegistered<TType>();
	}
}
