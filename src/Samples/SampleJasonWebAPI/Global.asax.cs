using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dependencies;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Jason.Configuration;
using Jason.Factories;
using Jason.Handlers.Commands;
using Jason.Runtime;
using Jason.WebAPI;
using Newtonsoft.Json;
using SampleJasonCommon;
using Topics.Radical.Reflection;

namespace SampleJasonWebAPI
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class WebApiApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			GlobalConfiguration.Configure( cfg =>
			{
				WebApiConfig.Register( cfg );
			} );

			FilterConfig.RegisterGlobalFilters( GlobalFilters.Filters );
			RouteConfig.RegisterRoutes( RouteTable.Routes );
			BundleConfig.RegisterBundles( BundleTable.Bundles );

			var windsor = new WindsorContainer();

			windsor.Register( Component.For<IServiceProvider>().Instance( new ServiceProviderWrapper( windsor ) ) );

			var jasonConfig = new DefaultJasonServerConfiguration( Path.Combine( AppDomain.CurrentDomain.BaseDirectory, "bin" ) )
			{
				Container = new WindsorJasonContainerProxy( windsor ),
				TypeFilter = t => !t.Is<MissingHandler>()
			};

			jasonConfig.AddEndpoint( new Jason.WebAPI.JasonWebAPIEndpoint( GlobalConfiguration.Configuration )
			{
				//TypeNameHandling = TypeNameHandling.Objects,
				IsCommandConvention = t =>
				{
					return t.Namespace != null && t.Namespace == "SampleTasks";
				}
			} );

			jasonConfig.AddEndpoint( new Jason.Client.JasonInProcessEndpoint() );
			jasonConfig.UsingAsFallbackCommandHandler<MissingHandler>();
			jasonConfig.Initialize();

			GlobalConfiguration.Configuration.DependencyResolver = new DelegateDependencyResolver()
			{
				OnGetService = t =>
				{
					if( windsor.Kernel.HasComponent( t ) )
					{
						return windsor.Resolve( t );
					}

					return null;
				},
				OnGetServices = t =>
				{
					if( windsor.Kernel.HasComponent( t ) )
					{
						return windsor.ResolveAll( t ).OfType<Object>();
					}

					return new List<Object>();
				}
			};
		}
	}

	class MissingHandler : AbstractCommandHandler<Object>
	{
		protected override object OnExecute( object command )
		{
			return null;
		}
	}

	class ServiceProviderWrapper : IServiceProvider
	{
		IWindsorContainer container;

		public ServiceProviderWrapper( IWindsorContainer container )
		{
			this.container = container;
		}

		public object GetService( Type serviceType )
		{
			if( this.container.Kernel.HasComponent( serviceType ) )
			{
				return this.container.Resolve( serviceType );
			}

			return null;
		}
	}


	class DelegateDependencyResolver : System.Web.Http.Dependencies.IDependencyResolver
	{
		public DelegateDependencyResolver()
		{
			this.OnBeginScope = () => this;
			this.OnGetService = t => null;
			this.OnGetServices = t => new List<Object>();
			this.OnDispose = () => { };
		}

		public Func<IDependencyScope> OnBeginScope { get; set; }

		public IDependencyScope BeginScope()
		{
			return this.OnBeginScope();
		}

		public Func<Type, Object> OnGetService { get; set; }

		public object GetService( Type serviceType )
		{
			return this.OnGetService( serviceType );
		}

		public Func<Type, IEnumerable<Object>> OnGetServices { get; set; }

		public IEnumerable<object> GetServices( Type serviceType )
		{
			return this.OnGetServices( serviceType );
		}

		public Action OnDispose { get; set; }

		public void Dispose()
		{
			this.OnDispose();
		}
	}

}