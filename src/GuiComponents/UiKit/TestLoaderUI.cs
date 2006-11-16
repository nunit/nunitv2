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
		public static void OpenProject( Form owner, bool vsSupport )
		{
			OpenFileDialog dlg = new OpenFileDialog();
			System.ComponentModel.ISite site = owner == null ? null : owner.Site;
			if ( site != null ) dlg.Site = site;
			dlg.Title = "Open Project";
			
			if ( vsSupport )
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
                    "Assemblies (*.dll,*.exe)|*.dll;*.exe";
					//"All Files (*.*)|*.*";
			}
			else
			{
                dlg.Filter =
                    "Projects & Assemblies(*.nunit,*.dll,*.exe)|*.nunit;*.dll;*.exe|" +
                    "Test Projects (*.nunit)|*.nunit|" +
                    "Assemblies (*.dll,*.exe)|*.dll;*.exe";
					//"All Files (*.*)|*.*";
			}

			dlg.FilterIndex = 1;
			dlg.FileName = "";

			if ( dlg.ShowDialog( owner ) == DialogResult.OK ) 
				OpenProject( owner, dlg.FileName );
		}

		public static void OpenProject( Form owner, string testFileName, string configName, string testName )
		{
			TestLoader loader = GetTestLoader( owner );

			if ( loader.IsProjectLoaded && SaveProjectIfDirty( owner ) == DialogResult.Cancel )
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

		public static void OpenProject( Form owner, string testFileName )
		{
			OpenProject( owner, testFileName, null, null );
		}

		public static void AddAssembly( Form owner )
		{
			AddAssembly( owner, null );
		}

		public static void AddAssembly( Form owner, string configName )
		{
			TestLoader loader = GetTestLoader( owner );

			if ( configName == null )
				configName = loader.TestProject.ActiveConfigName;

			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Title = "Add Assembly";
			dlg.InitialDirectory = configName == null
				? loader.TestProject.ActiveConfig.BasePath
				: loader.TestProject.Configs[configName].BasePath;
            dlg.Filter =
                "Assemblies (*.dll,*.exe)|*.dll;*.exe";
				//"All Files (*.*)|*.*";
			dlg.FilterIndex = 1;
			dlg.FileName = "";

			if ( dlg.ShowDialog( owner ) == DialogResult.OK )
			{
				loader.TestProject.Configs[configName].Assemblies.Add( dlg.FileName );
			}
		}

		public static void AddVSProject( Form owner )
		{
			TestLoader loader = GetTestLoader( owner );
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Title = "Add Visual Studio Project";

			dlg.Filter =
				"All Project Types (*.csproj,*.vjsproj,*.vbproj,*.vcproj)|*.csproj;*.vjsproj;*.vbproj;*.vcproj|" +
				"C# Projects (*.csproj)|*.csproj|" +
				"J# Projects (*.vjsproj)|*.vjsproj|" +
				"VB Projects (*.vbproj)|*.vbproj|" +
				"C++ Projects (*.vcproj)|*.vcproj|" +
				"All Files (*.*)|*.*";

			dlg.FilterIndex = 1;
			dlg.FileName = "";

			if ( dlg.ShowDialog( owner ) == DialogResult.OK ) 
			{
				try
				{
					VSProject vsProject = new VSProject( dlg.FileName );
					loader.TestProject.Add( vsProject );
					loader.LoadTest();
				}
				catch( Exception ex )
				{
					UserMessage.DisplayFailure( ex.Message, "Invalid VS Project" );
				}
			}
		}

		private static bool CanWriteProjectFile( string path )
		{
			return !File.Exists( path ) || 
				( File.GetAttributes( path ) & FileAttributes.ReadOnly ) == 0;
		}

		public static void SaveProject( Form owner )
		{
			TestLoader loader = GetTestLoader( owner );

			if ( Path.IsPathRooted( loader.TestProject.ProjectPath ) &&
				 NUnitProject.IsProjectFile( loader.TestProject.ProjectPath ) &&
				 CanWriteProjectFile( loader.TestProject.ProjectPath ) )
				loader.TestProject.Save();
			else
				SaveProjectAs( owner );
		}

		public static void SaveProjectAs( Form owner )
		{
			TestLoader loader = GetTestLoader( owner );

			SaveFileDialog dlg = new SaveFileDialog();
			dlg.Title = "Save Test Project";
			dlg.Filter = "NUnit Test Project (*.nunit)|*.nunit|All Files (*.*)|*.*";
			string path = NUnitProject.ProjectPathFromFile( loader.TestProject.ProjectPath );
			if ( CanWriteProjectFile( path ) )
				dlg.FileName = path;
			dlg.DefaultExt = "nunit";
			dlg.ValidateNames = true;
			dlg.OverwritePrompt = true;

			while( dlg.ShowDialog( owner ) == DialogResult.OK )
			{
				if ( !CanWriteProjectFile( dlg.FileName ) )
					UserMessage.DisplayInfo( string.Format( "File {0} is write-protected. Select another file name.", dlg.FileName ) );
				else
				{
					loader.TestProject.Save( dlg.FileName );
					return;
				}
			}
		}

		public static void NewProject( Form owner )
		{
			TestLoader loader = GetTestLoader( owner );
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

		public static DialogResult CloseProject( Form owner )
		{
			DialogResult result = SaveProjectIfDirty( owner );

			if( result != DialogResult.Cancel )
				GetTestLoader( owner ).UnloadProject();

			return result;
		}

		private static DialogResult SaveProjectIfDirty( Form owner )
		{
			DialogResult result = DialogResult.OK;
			TestLoader loader = GetTestLoader( owner );

			if( loader.TestProject.IsDirty )
			{
				string msg = "Project has been changed. Do you want to save changes?";

				result = UserMessage.Ask( msg, MessageBoxButtons.YesNoCancel );
				if ( result == DialogResult.Yes )
					SaveProject( owner );
			}

			return result;
		}

		public static void SaveLastResult( Form owner )
		{
			//TODO: Save all results
			TestLoader loader = GetTestLoader( owner );
			TestResult result = loader.TestResult;
			
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

		private static TestLoader GetTestLoader( Form owner )
		{
			return (TestLoader)owner.Site.GetService( typeof( TestLoader ) );
		}
	}
}
