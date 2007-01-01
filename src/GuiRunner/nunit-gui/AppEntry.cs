#region Copyright (c) 2002-2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright  2000-2002 Philip A. Craig
'
' This software is provided 'as-is', without any express or implied warranty. In no 
' event will the authors be held liable for any damages arising from the use of this 
' software.
' 
' Permission is granted to anyone to use this software for any purpose, including 
' commercial applications, and to alter it and redistribute it freely, subject to the 
' following restrictions:
'
' 1. The origin of this software must not be misrepresented; you must not claim that 
' you wrote the original software. If you use this software in a product, an 
' acknowledgment (see the following) in the product documentation is required.
'
' Portions Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright  2000-2002 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
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
			NUnitForm.CommandLineOptions command =
				new NUnitForm.CommandLineOptions();

			GuiOptions parser = new GuiOptions(args);
			if(parser.Validate() && !parser.help) 
			{
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

				if(command.testFileName != null)
				{
					FileInfo fileInfo = new FileInfo(command.testFileName);
					if(!fileInfo.Exists)
					{
						string message = String.Format("{0} does not exist", fileInfo.FullName);
						UserMessage.DisplayFailure( message,"Specified test file does not exist" );
						return 1;
					}
					else
					{
						command.testFileName = fileInfo.FullName;
					}
				}

				// Add Standard Services to ServiceManager
				ServiceManager.Services.AddService( new SettingsService() );
				ServiceManager.Services.AddService( new DomainManager() );
				ServiceManager.Services.AddService( new RecentFilesService() );
				ServiceManager.Services.AddService( new TestLoader( new GuiTestEventDispatcher() ) );
				ServiceManager.Services.AddService( new AddinRegistry() );
				ServiceManager.Services.AddService( new AddinManager() );

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
				
			return 0;
		}
	}
}
