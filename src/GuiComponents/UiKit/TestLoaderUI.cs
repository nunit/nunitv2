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
using System.IO;
using System.Windows.Forms;
using NUnit.Core;
using NUnit.Util;

namespace NUnit.UiKit
{
	/// <summary>
	/// FileHandler does all file opening and closing that
	/// involves interacting with the user.
	/// </summary>
	public class TestLoaderUI
	{
		#region Instance Variables

		/// <summary>
		/// The owning (main) form
		/// </summary>
		private Form owner;

		/// <summary>
		/// The test loader
		/// </summary>
		private ITestLoader loader;

		#endregion

		#region Public Members

		public TestLoaderUI( Form owner, ITestLoader loader )
		{
			this.owner = owner;
			this.loader = loader;
		}

		public void OpenProject()
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Title = "Open Project";
			
			if ( UserSettings.Options.VisualStudioSupport )
			{
				dlg.Filter =
					"Projects & Assemblies(*.nunit,*.csproj,*.vbproj,*.vjsproj, *.vcproj,*.sln,*.dll,*.exe )|*.nunit;*.csproj;*.vjsproj;*.vbproj;*.vcproj;*.sln;*.dll;*.exe|" +
					"All Project Types (*.nunit,*.csproj,*.vbproj,*.vjsproj,*.vcproj,*.sln)|*.nunit;*.csproj;*.vjsproj;*.vbproj;*.vcproj;*.sln|" +
					"Test Projects (*.nunit)|*.nunit|" +
					"Solutions (*.sln)|*.sln|" +
					"C# Projects (*.csproj)|*.csproj|" +
					"J# Projects (*.vjsproj)|*.vjsproj|" +
					"VB Projects (*.vbproj)|*.vbproj|" +
					"C++ Projects (*.vcproj)|*.vcproj|" +
					"Assemblies (*.dll,*.exe)|*.dll;*.exe|" +
					"All Files (*.*)|*.*";
			}
			else
			{
				dlg.Filter =
					"Projects & Assemblies(*.nunit,*.dll,*.exe)|*.nunit;*.dll;*.exe|" + 
					"Test Projects (*.nunit)|*.nunit|" + 
					"Assemblies (*.dll,*.exe)|*.dll;*.exe|" +
					"All Files (*.*)|*.*";
			}

			dlg.FilterIndex = 1;
			dlg.FileName = "";

			if ( dlg.ShowDialog( owner ) == DialogResult.OK ) 
				OpenProject( dlg.FileName );
		}

		public void OpenProject( string testFileName, string configName, string testName )
		{
			if ( loader.IsProjectLoaded && SaveProjectIfDirty() == DialogResult.Cancel )
				return;

			loader.LoadProject( testFileName, configName );
			if ( loader.IsProjectLoaded )
			{	
				NUnitProject testProject = loader.TestProject;
				if ( testProject.Configs.Count == 0 )
					UserMessage.DisplayInfo( "Loaded project contains no configuration data" );
				else if ( testProject.ActiveConfig == null )
					UserMessage.DisplayInfo( "Loaded project has no active configuration" );
				else if ( testProject.ActiveConfig.Assemblies.Count == 0 )
					UserMessage.DisplayInfo( "Active configuration contains no assemblies" );
				else
					loader.LoadTest( testName );
			}
		}

		public void OpenProject( string testFileName )
		{
			OpenProject( testFileName, null, null );
		}

		public void AddAssembly( )
		{
			AddAssembly( loader.TestProject.ActiveConfigName );
		}

		public void AddAssembly( string configName )
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Title = "Add Assembly";
			
			dlg.Filter =
				"Assemblies (*.dll,*.exe)|*.dll;*.exe|" +
				"All Files (*.*)|*.*";

			dlg.FilterIndex = 1;
			dlg.FileName = "";

			if ( dlg.ShowDialog( owner ) == DialogResult.OK ) 
			{
				loader.TestProject.Configs[configName].Assemblies.Add( dlg.FileName );
			}
		}

		public void AddVSProject()
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Title = "Add Visual Studio Project";

			dlg.Filter =
				"All Project Types (*.csproj,*.vjsproj,*.vbproj,*.vcproj,*.sln)|*.csproj;*.vjsproj;*.vbproj;*.vcproj;*.sln|" +
				"Solutions (*.sln)|*.sln|" +
				"C# Projects (*.csproj)|*.csproj|" +
				"J# Projects (*.vjsproj)|*.vjsproj|" +
				"VB Projects (*.vbproj)|*.vbproj|" +
				"C++ Projects (*.vcproj)|*.vcproj|" +
				"All Files (*.*)|*.*";

			dlg.FilterIndex = 1;
			dlg.FileName = "";

			if ( dlg.ShowDialog( owner ) == DialogResult.OK ) 
			{
				VSProject vsProject = new VSProject( dlg.FileName );
				loader.TestProject.Add( vsProject );
				loader.LoadTest();
			}
		}


		public void SaveProject()
		{
			if ( Path.IsPathRooted( loader.TestProject.ProjectPath ) &&
				 NUnitProject.IsProjectFile( loader.TestProject.ProjectPath ) )
				loader.TestProject.Save();
			else
				SaveProjectAs();
		}

		public void SaveProjectAs()
		{
			SaveFileDialog dlg = new SaveFileDialog();
			dlg.Title = "Save Test Project";
			dlg.Filter = "NUnit Test Project (*.nunit)|*.nunit|All Files (*.*)|*.*";
			dlg.FileName = NUnitProject.ProjectPathFromFile( loader.TestProject.ProjectPath );
			dlg.DefaultExt = "nunit";
			dlg.ValidateNames = true;
			dlg.OverwritePrompt = true;

			if ( dlg.ShowDialog( owner ) == DialogResult.OK )
				loader.TestProject.Save( dlg.FileName );
		}

		public void NewProject()
		{
			SaveFileDialog dlg = new SaveFileDialog();
			dlg.Title = "New Test Project";
			dlg.Filter = "NUnit Test Project (*.nunit)|*.nunit|All Files (*.*)|*.*";
			dlg.FileName = NUnitProject.GenerateProjectName();
			dlg.DefaultExt = "nunit";
			dlg.ValidateNames = true;
			dlg.OverwritePrompt = true;

			if ( dlg.ShowDialog( owner ) == DialogResult.OK )
				loader.NewProject( dlg.FileName );
		}

		public DialogResult CloseProject()
		{
			DialogResult result = SaveProjectIfDirty();

			if( result != DialogResult.Cancel )
				loader.UnloadProject();

			return result;
		}

		private DialogResult SaveProjectIfDirty()
		{
			DialogResult result = DialogResult.OK;

			if( loader.TestProject.IsDirty )
			{
				string msg = "Project has been changed. Do you want to save changes?";

				result = UserMessage.Ask( msg, MessageBoxButtons.YesNoCancel );
				if ( result == DialogResult.Yes )
					SaveProject();
			}

			return result;
		}

		public void SaveLastResult()
		{
			//TODO: Save all results
			TestResult result = loader.Results[0];
			
			SaveFileDialog dlg = new SaveFileDialog();
			dlg.Title = "Save Test Results as XML";
			dlg.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
			dlg.FileName = "TestResult.xml";
			dlg.InitialDirectory = Path.GetDirectoryName( loader.TestFileName );
			dlg.DefaultExt = "xml";
			dlg.ValidateNames = true;
			dlg.OverwritePrompt = true;

			if ( dlg.ShowDialog( owner ) == DialogResult.OK )
			{
				try
				{
					string fileName = dlg.FileName;

					XmlResultVisitor resultVisitor 
						= new XmlResultVisitor( fileName, result);
					result.Accept(resultVisitor);
					resultVisitor.Write();

					string msg = String.Format( "Results saved as {0}", fileName );
					UserMessage.DisplayInfo( msg, "Save Results as XML" );
				}
				catch( Exception exception )
				{
					UserMessage.DisplayFailure( exception, "Unable to Save Results" );
				}
			}
		}

		#endregion
	}
}
