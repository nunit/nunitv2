using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using NUnit.Util;
using NUnit.UiKit;

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
		static int Main(string[] args) 
		{
			NUnitRegistry.InitializeAddReferenceDialog();

			NUnitForm.CommandLineOptions command =
				new NUnitForm.CommandLineOptions();

			GuiOptions parser = new GuiOptions(args);
			if(parser.Validate() && !parser.help) 
			{
				if(!parser.NoArgs)
					command.testFileName = (string)parser.Assembly;

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

				NUnitForm form = new NUnitForm( command );
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
