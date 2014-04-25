using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Jason.Client.ComponentModel;
using Jason.ComponentModel;
using Jason.Configuration;
using Topics.Radical.ComponentModel;
using Topics.Radical.Linq;
using Topics.Radical.Reflection;
using Topics.Radical;

namespace Jason.Client.Runtime
{
    public class DefaultJasonClientConfiguration : IJasonClientConfiguration
    {
        String path;
        readonly IWorkerServiceClientFactory clientFactory;

        public DefaultJasonClientConfiguration( String pathToScanForAssemblies, IWorkerServiceClientFactory clientFactory, String assemblySelectPattern = "*" )
        {
            this.CommandsSelector = t => false;

            this.path = pathToScanForAssemblies;
            this.clientFactory = clientFactory;
            this.AssemblySelector = name => name.IsLike( "jason.*", assemblySelectPattern );
        }

        public Func<Type, Boolean> CommandsSelector { get; set; }
        public Func<String, Boolean> AssemblySelector { get; set; }

        public Task Initialize()
        {
            return Task.Factory.StartNew( () =>
            {
                Directory.EnumerateFiles( this.path, "*.dll" )
                    .ForEach( dll =>
                    {
                        var name = Path.GetFileNameWithoutExtension( dll );
                        if ( this.AssemblySelector( name ) )
                        {
                            var allTypes = Assembly.Load( name ).GetTypes();

                            allTypes.Where( t =>
                            {
                                return ( !t.IsGenericType && t.IsAttributeDefined<ServiceKnownTypeAttribute>( true ) )
                                    || this.CommandsSelector( t );
                            } )
                            .ForEach( t => ServiceKnownTypesProvider.RegisterKnownType( t ) );
                        }
                    } );

                this.clientFactory.QueueChannelFactoryReset();
            } );
        }
    }
}
