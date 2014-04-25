using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Jason.Client.ComponentModel;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Topics.Radical;
using Jason.Configuration;
using Jason.ComponentModel;
using Topics.Radical.Reflection;
using Topics.Radical.Linq;
using System.IO;

namespace Jason.Client.Runtime
{
    public class WinRTJasonClientConfiguration : IJasonClientConfiguration
    {
        readonly IWorkerServiceClientFactory clientFactory;

        public WinRTJasonClientConfiguration( IWorkerServiceClientFactory clientFactory )
        {
            this.clientFactory = clientFactory;
            this.CommandsSelector = t => false;
            this.AssembliesProvider = () => this.GetCompositionAssemblies();
        }

        public Func<TypeInfo, bool> CommandsSelector { get; set; }

        public Func<Task<IEnumerable<Assembly>>> AssembliesProvider { get; set; }

        async Task<IEnumerable<Assembly>> GetCompositionAssemblies()
        {
            var temp = new Dictionary<String, Assembly>();

            var appAssembly = Application.Current.GetType().GetTypeInfo().Assembly;
            temp.Add( appAssembly.GetName().Name, appAssembly );

            var thisAssembly = this.GetType().GetTypeInfo().Assembly;
            if ( !temp.ContainsKey( thisAssembly.GetName().Name ) )
            {
                temp.Add( thisAssembly.GetName().Name, thisAssembly );
            }

            var pattern = String.Format( "{0}*.dll", appAssembly.GetName().Name );
            var radical = "radical*.dll";

            var all = await Package.Current.InstalledLocation.GetFilesAsync();
            var allAssemblies = all.Where( f => f.Name.IsLike( pattern ) || f.Name.IsLike( radical ) );

            foreach ( var assembly in allAssemblies )
            {
                var name = new AssemblyName( Path.GetFileNameWithoutExtension( assembly.Name ) );
                if ( !temp.ContainsKey( name.Name ) )
                {
                    temp.Add( name.Name, Assembly.Load( name ) );
                }
            }

            return temp.Values;
        }

        public async Task Initialize()
        {
            var assemblies = await this.GetCompositionAssemblies();
            foreach ( var dll in assemblies )
            {
                dll.ExportedTypes
                    .Select( t => t.GetTypeInfo() )
                    .Where( t =>
                    {
                        return ( !t.IsGenericType && t.GetCustomAttribute<ServiceKnownTypeAttribute>( true ) != null )
                            || this.CommandsSelector( t );
                    } )
                    .ForEach( t => ServiceKnownTypesProvider.RegisterKnownType( t.AsType() ) );
            }

            this.clientFactory.QueueChannelFactoryReset();
        }
    }
}
