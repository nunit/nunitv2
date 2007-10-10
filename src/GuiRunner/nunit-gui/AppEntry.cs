// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org/?p=license&r=2.4.
// ****************************************************************

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using NUnit.UiKit;
using NUnit.Util;
using NUnit.Core;
using NUnit.Core.Extensibility;

[assembly: log4net.Config.XmlConfigurator(Watch=true)]

namespace NUnit.Gui
{
	/// <summary>
	/// Class to manage application startup.
	/// </summary>
	public class AppEntry
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
			System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		public static int Main(string[] args) 
		{
			log4net.GlobalContext.Properties["PID"] = System.Diagnostics.Process.GetCurrentProcess().Id;
			log.Info( "Starting NUnit GUI" );

			NUnitForm.CommandLineOptions command =
				new NUnitForm.CommandLineOptions();

			GuiOptions parser = new GuiOptions(args);

			GuiAttachedConsole attachedConsole = null;
			if ( parser.console )
				attachedConsole = new GuiAttachedConsole();

			if(parser.Validate() && !parser.help) 
			{
				if ( parser.cleanup )
				{
					DomainManager.DeleteShadowCopyPath();
					return 0;
				}

				if(!parser.NoArgs)
				{
					if (parser.IsAssembly)
						command.testFileName = parser.Assembly;
					command.configName = parser.config;
					command.testName = parser.fixture;
					command.noload = parser.noload;
					command.autorun = parser.run;
					if (parser.lang != null)
						Thread.CurrentThread.CurrentUICulture =
							new CultureInfo( parser.lang );

					if ( parser.HasInclude )
					{
						command.categories = parser.include;
						command.exclude = false;
					}
					else if ( parser.HasExclude )
					{
						command.categories = parser.exclude;
						command.exclude = true;
					}
				}

				// Add Standard Services to ServiceManager
				ServiceManager.Services.AddService( new SettingsService() );
				ServiceManager.Services.AddService( new DomainManager() );
				ServiceManager.Services.AddService( new RecentFilesService() );
				ServiceManager.Services.AddService( new TestLoader( new GuiTestEventDispatcher() ) );
				ServiceManager.Services.AddService( new AddinRegistry() );
				ServiceManager.Services.AddService( new AddinManager() );
				ServiceManager.Services.AddService( new TestAgency() );

				// Initialize Services
				ServiceManager.Services.InitializeServices();

				// Create container in order to allow ambient properties
				// to be shared across all top-level forms.
				AppContainer c = new AppContainer();
				AmbientProperties ambient = new AmbientProperties();
				c.Services.AddService( typeof( AmbientProperties ), ambient );
				NUnitForm form = new NUnitForm( command );
				c.Add( form );

				try
				{
					Application.Run( form );
				}
				finally
				{
					ServiceManager.Services.StopAllServices();
				}
			}
			else
			{
				string message = parser.GetHelpText();
				UserMessage.DisplayFailure( message,"Help Syntax" );
				return 2;
			}	
				
			if ( attachedConsole != null )
			{
				Console.WriteLine( "Press Enter to exit" );
				Console.ReadLine();
				attachedConsole.Close();
			}

			log.Info( "Exiting NUnit GUI" );

			return 0;
		}
	}
}
