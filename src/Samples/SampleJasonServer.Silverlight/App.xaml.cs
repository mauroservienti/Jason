using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Xml;
using Jason.ComponentModel;
using Jason.Configuration;
using Topics.Radical.Reflection;
using Topics.Radical.Linq;

namespace SampleJasonServer.Silverlight
{
	public partial class App : Application
	{
		public App()
		{
			this.Startup += this.Application_Startup;
			this.Exit += this.Application_Exit;
			this.UnhandledException += this.Application_UnhandledException;

			InitializeComponent();
		}

		private void Application_Startup( object sender, StartupEventArgs e )
		{
			//setup client
			SetupJasonClient();

			this.RootVisual = new MainPage()
			{
				DataContext = new MainPageViewModel( new Jason.Client.Runtime.DefaultWorkerServiceClientFactory() )
			};
		}

		static void SetupJasonClient()
		{
			var reader = XmlReader.Create( Application.GetResourceStream( new Uri( "AppManifest.xaml", UriKind.Relative ) ).Stream );
			var parts = new AssemblyPartCollection();

			if( reader.Read() )
			{
				reader.ReadStartElement();

				if( reader.ReadToNextSibling( "Deployment.Parts" ) )
				{
					while( reader.ReadToFollowing( "AssemblyPart" ) )
					{
						parts.Add( new AssemblyPart() { Source = reader.GetAttribute( "Source" ) } );
					}
				}
			}

			foreach( var part in parts )
			{
				if( part.Source.ToLower().EndsWith( ".dll" ) )
				{
					var stream = Application.GetResourceStream( new Uri( part.Source, UriKind.Relative ) ).Stream;
					var assembly = part.Load( stream );
					var allTypes = assembly.GetTypes();

					allTypes.Where( t => !t.IsGenericType && t.IsAttributeDefined<ServiceKnownTypeAttribute>( true ) )
						.ForEach( t => ServiceKnownTypesProvider.RegisterKnownType( t ) );
				}
			}

		}

		private void Application_Exit( object sender, EventArgs e )
		{

		}

		private void Application_UnhandledException( object sender, ApplicationUnhandledExceptionEventArgs e )
		{
			// If the app is running outside of the debugger then report the exception using
			// the browser's exception mechanism. On IE this will display it a yellow alert 
			// icon in the status bar and Firefox will display a script error.
			if( !System.Diagnostics.Debugger.IsAttached )
			{

				// NOTE: This will allow the application to continue running after an exception has been thrown
				// but not handled. 
				// For production applications this error handling should be replaced with something that will 
				// report the error to the website and stop the application.
				e.Handled = true;
				Deployment.Current.Dispatcher.BeginInvoke( delegate { ReportErrorToDOM( e ); } );
			}
		}

		private void ReportErrorToDOM( ApplicationUnhandledExceptionEventArgs e )
		{
			try
			{
				string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
				errorMsg = errorMsg.Replace( '"', '\'' ).Replace( "\r\n", @"\n" );

				System.Windows.Browser.HtmlPage.Window.Eval( "throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");" );
			}
			catch( Exception )
			{
			}
		}
	}
}
