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

				// Create container for top-level forms
				AppContainer c = new AppContainer();

				// Add standard services to the container's ServiceContainer
				AmbientProperties ambient = new AmbientProperties();
				// Todo: Get font from UserSettings
				c.Services.AddService( typeof( AmbientProperties ), ambient );

				UserSettings settings = new UserSettings();
				c.Services.AddService( typeof( UserSettings ), settings );

				TestLoader loader = new TestLoader( new GuiTestEventDispatcher() );
				loader.ReloadOnRun = settings.Options.ReloadOnRun;
				loader.ReloadOnChange = settings.Options.ReloadOnChange;
				loader.RerunOnChange = settings.Options.RerunOnChange;
				loader.MultiDomain = settings.Options.MultiDomain;
				loader.MergeAssemblies = settings.Options.MergeAssemblies;
				loader.AutoNamespaceSuites = settings.Options.AutoNamespaceSuites;
				c.Services.AddService( typeof( TestLoader ), loader );

				// Create top-level form
				NUnitForm form = new NUnitForm( command );
				c.Add( form );
				Application.Run( form );
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
