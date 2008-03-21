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

namespace NUnit.Gui
{
	/// <summary>
	/// Class to manage application startup.
	/// </summary>
	public class AppEntry
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		public static int Main(string[] args) 
		{
			NTrace.Info( "Starting NUnit GUI" );

			GuiOptions guiOptions = new GuiOptions(args);

			GuiAttachedConsole attachedConsole = null;
			if ( guiOptions.console )
				attachedConsole = new GuiAttachedConsole();

			if( !guiOptions.Validate() || guiOptions.help) 
			{
				string message = guiOptions.GetHelpText();
				UserMessage.DisplayFailure( message,"Help Syntax" );
				return 2;
			}

			if ( guiOptions.cleanup )
			{
				DomainManager.DeleteShadowCopyPath();
				return 0;
			}

			if(!guiOptions.NoArgs)
			{
				if (guiOptions.lang != null)
					Thread.CurrentThread.CurrentUICulture =
						new CultureInfo( guiOptions.lang );
			}

			// Add Standard Services to ServiceManager
			ServiceManager.Services.AddService( new SettingsService() );
			ServiceManager.Services.AddService( new DomainManager() );
			ServiceManager.Services.AddService( new RecentFilesService() );
			ServiceManager.Services.AddService( new ProjectService() );
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
			NUnitForm form = new NUnitForm( guiOptions );
			c.Add( form );

			try
			{
				Application.Run( form );
			}
			finally
			{
				ServiceManager.Services.StopAllServices();
			}
				
			if ( attachedConsole != null )
			{
				Console.WriteLine( "Press Enter to exit" );
				Console.ReadLine();
				attachedConsole.Close();
			}

			NTrace.Info( "Exiting NUnit GUI" );

			return 0;
		}
	}
}
