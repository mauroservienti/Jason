using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Jason.ComponentModel;
using Jason.Factories;
using Jason.Runtime;
using Topics.Radical.Reflection;
using Topics.Radical.Linq;
using System.Reflection;
using Jason.Configuration;
using Jason.Handlers.Jobs;
using Jason.Handlers.Tasks;
using Topics.Radical.ComponentModel;
using Jason.Server.WcfServices;
using Jason.Handlers.Commands;
using System.Runtime.Serialization;
using SampleJasonCommon;

namespace SampleJasonServer.Web
{
    public class Global : System.Web.HttpApplication, IServiceProvider
    {
        IWindsorContainer windsor;

        public static IServiceProvider Container
        {
            get;
            private set;
        }

        public object GetService( Type serviceType )
        {
            return this.windsor.Resolve( serviceType );
        }

        protected void Application_Start( object sender, EventArgs e )
        {
            Container = this;

            this.windsor = new WindsorContainer();
            windsor.Register( Component.For<IServiceProvider>().Instance( Global.Container ) );

			var jasonConfig = new DefaultJasonServerConfiguration( Path.Combine( AppDomain.CurrentDomain.BaseDirectory, "bin" ) )
			{
				Container = new WindsorJasonContainerProxy( this.windsor )
			};

			jasonConfig.AddEndpoint( new Jason.Server.JasonWcfEndpoint()
			{
				CommandsSelector = t =>
				{
					return t.IsAttributeDefined<DataContractAttribute>()
						&& ( t.Name.EndsWith( "Command" ) || t.Name.EndsWith( "CommandResponse" ) );
				}
			} );

            jasonConfig.Initialize();
        }
    }
}