using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Castle.Windsor;
using Jason.Client.Runtime;
using Topics.Radical.Reflection;
using Topics.Radical.Linq;
using Jason.Configuration;
using Jason.ComponentModel;
using jason = Jason.Model;
using SampleTasks;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SampleJasonServer.Desktop
{
    class Program
    {
        static void Main( string[] args )
        {
            var factory = new DefaultWorkerServiceClientFactory();
            var config = new Jason.Client.Runtime.DefaultJasonClientConfiguration( AppDomain.CurrentDomain.BaseDirectory, factory )
            {
                CommandsSelector = t =>
                {
                    return t.IsAttributeDefined<DataContractAttribute>()
                        && ( t.Name.EndsWith( "Command" ) || t.Name.EndsWith( "CommandResponse" ) );
                }
            };

            config.Initialize().Wait();

            Console.WriteLine( "=========== Sync ============" );

            using ( var client = factory.CreateClient() )
            {
                var result = client.WorkOn( new jason.Job( new jason.EchoJobTask() { Message = "Hi there" } ) );
                Console.WriteLine( result.GetTaskResult<jason.EchoJobTaskResult>().EchoedMessage );
            }

            using ( var client = factory.CreateClient() )
            {
                var result = client.WorkOn( new jason.Job( new SampleJobTask() ) );
            }

            Console.WriteLine( "=========== Async ===========" );

            using ( var client = factory.CreateAsyncClient() )
            {
                var task = client.WorkOnAsync( new jason.Job( new jason.EchoJobTask() { Message = "Hi there" } ) )
                    .ContinueWith( r =>
                    {
                        Console.WriteLine( r.Result.GetTaskResult<jason.EchoJobTaskResult>().EchoedMessage );
                    } );

                task.Wait();
            }

            using ( var client = factory.CreateAsyncClient() )
            {
                var task = client.ExecuteAsync( new SampleCommand() )
                    .ContinueWith( r =>
                    {
                        var e = r.Result as jason.EmptyCommandExecutionResult;
                        Console.WriteLine( e == null ? "something wrong..." : "command executed" );
                    } );

                task.Wait();
            }

            Console.WriteLine( "=========== Async with 'ignorance' ===========" );

            using ( var client = factory.CreateAsyncClient() )
            {
                var task = client.ExecuteAsync( new JasonIgnorantCommand() )
                    .ContinueWith( r =>
                    {
                        var e = r.Result as JasonIgnorantCommandResponse;
                        Console.WriteLine( e == null ? "something wrong..." : "ignorant command executed" );
                    } );

                task.Wait();
            }

            Console.Read();
        }

        //static void SetupJsonClient()
        //{
        //	Directory.EnumerateFiles( AppDomain.CurrentDomain.BaseDirectory, "*.dll" )
        //		.ForEach( dll => Assembly.Load( Path.GetFileNameWithoutExtension( dll ) )
        //			.GetTypes()
        //			.Where( t => !t.IsGenericType && t.IsAttributeDefined<ServiceKnownTypeAttribute>( true ) )
        //			.ForEach( t => ServiceKnownTypesProvider.RegisterKnownType( t ) )
        //		);
        //}
    }
}
