#region Copyright (c) 2002, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright  2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
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
' Portions Copyright  2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
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
using System.Xml;
using System.Text;

namespace NUnit.Tests
{
	using NUnit.Util;
	using NUnit.Framework;

	/// <summary>
	/// Summary description for NUnitProjectTests.
	/// </summary>
	[TestFixture]
	public class NUnitProjectTests
	{
		static readonly string xmlfile = "test.nunit";

		private NUnitProject project;

		[SetUp]
		public void SetUp()
		{
			NUnitProject.ProjectSeed = 0;
			project = NUnitProject.EmptyProject();
		}

		[TearDown]
		public void EraseFile()
		{
			if ( File.Exists( xmlfile ) )
				File.Delete( xmlfile );
		}

		[Test]
		public void IsProjectFile()
		{
			Assert.True( NUnitProject.IsProjectFile( @"\x\y\test.nunit" ) );
			Assert.False( NUnitProject.IsProjectFile( @"\x\y\test.junit" ) );
		}

		[Test]
		public void NewProjectIsEmpty()
		{
			Assert.Equals( 0, project.Configs.Count );
			Assert.Null( project.ActiveConfig );
		}

		[Test]
		public void NewProjectIsNotDirty()
		{
			Assert.False( project.IsDirty );
		}

		[Test] 
		public void NewProjectDefaultPath()
		{
			Assert.Equals( Path.GetFullPath( "Project1" ), project.ProjectPath );
			Assert.Equals( "Project1", project.Name );
			NUnitProject another = NUnitProject.EmptyProject();
			Assert.Equals( Path.GetFullPath( "Project2" ), another.ProjectPath );
		}

		[Test]
		public void NewProjectNotLoadable()
		{
			Assert.False( project.IsLoadable );
		}

		[Test]
		public void SaveMakesProjectNotDirty()
		{
			project.Save( xmlfile );
			Assert.False( project.IsDirty );
		}

		[Test]
		public void SaveSetsProjectPath()
		{
			project.Save( xmlfile );
			Assert.Equals( Path.GetFullPath( xmlfile ), project.ProjectPath );
			Assert.Equals( "test", project.Name );
		}

		[Test]
		public void DefaultApplicationBase()
		{
			project.Save( xmlfile );
			Assert.Equals( Path.GetDirectoryName( project.ProjectPath ), project.BasePath );
		}

		public void DefaultConfigurationFile()
		{
			project.Save( xmlfile );
			Assert.Equals( "test.config", project.ConfigurationFile );
		}

		[Test]
		public void DefaultProjectName()
		{
			project.Save( xmlfile );
			Assert.Equals( "test", project.Name );
		}

		[Test]
		public void LoadMakesProjectNotDirty()
		{
			project.Save( xmlfile );
			NUnitProject project2 = NUnitProject.LoadProject( xmlfile );
			Assert.False( project2.IsDirty );
		}

		[Test]
		public void CanAddConfigs()
		{
			project.Configs.Add("Debug");
			project.Configs.Add("Release");
			Assert.Equals( 2, project.Configs.Count );
		}

		[Test]
		public void CanSetActiveConfig()
		{
			project.Configs.Add("Debug");
			project.Configs.Add("Release");
			project.SetActiveConfig( "Release" );
			Assert.Equals( "Release", project.ActiveConfig.Name );
		}

		[Test]
		public void CanAddAssemblies()
		{
			project.Configs.Add("Debug");
			project.Configs.Add("Release");

			project.Configs["Debug"].Assemblies.Add( Path.GetFullPath( @"bin\debug\assembly1.dll" ) );
			project.Configs["Debug"].Assemblies.Add( Path.GetFullPath( @"bin\debug\assembly2.dll" ) );
			project.Configs["Release"].Assemblies.Add( Path.GetFullPath( @"bin\debug\assembly3.dll" ) );

			Assert.Equals( 2, project.Configs.Count );
			Assert.Equals( 2, project.Configs["Debug"].Assemblies.Count );
			Assert.Equals( 1, project.Configs["Release"].Assemblies.Count );
		}

		[Test]
		public void AddConfigMakesProjectDirty()
		{
			project.Configs.Add("Debug");
			Assert.True( project.IsDirty );
		}

		[Test]
		public void RenameConfigMakesProjectDirty()
		{
			project.Configs.Add("Old");
			project.IsDirty = false;
			project.Configs[0].Name = "New";
			Assert.True( project.IsDirty );
		}

		[Test]
		public void DefaultActiveConfig()
		{
			project.Configs.Add("Debug");
			Assert.Equals( "Debug", project.ActiveConfig.Name );
		}

		[Test]
		public void RenameActiveConfig()
		{
			project.Configs.Add( "Old" );
			project.SetActiveConfig( "Old" );
			project.Configs[0].Name = "New";
			Assert.Equals( "New", project.ActiveConfig.Name );
		}

		[Test]
		public void RemoveConfigMakesProjectDirty()
		{
			project.Configs.Add("Debug");
			project.IsDirty = false;
			project.Configs.Remove("Debug");
			Assert.True( project.IsDirty );
		}

		[Test]
		public void RemoveActiveConfig()
		{
			project.Configs.Add("Debug");
			project.Configs.Add("Release");
			project.SetActiveConfig("Debug");
			project.Configs.Remove("Debug");
			Assert.Equals( "Release", project.ActiveConfig.Name );
		}

		[Test]
		public void SettingActiveConfigMakesProjectDirty()
		{
			project.Configs.Add("Debug");
			project.Configs.Add("Release");
			project.SetActiveConfig( "Debug" );
			project.IsDirty = false;
			project.SetActiveConfig( "Release" );
			Assert.True( project.IsDirty );
		}

		[Test]
		public void SaveAndLoadEmptyProject()
		{
			project.Save( xmlfile );
			Assert.True( File.Exists( xmlfile ) );

			NUnitProject project2 = NUnitProject.LoadProject( xmlfile );

			Assert.Equals( 0, project2.Configs.Count );
		}

		[Test]
		public void SaveAndLoadEmptyConfigs()
		{
			project.Configs.Add( "Debug" );
			project.Configs.Add( "Release" );
			project.Save( xmlfile );

			Assert.True( File.Exists( xmlfile ) );

			NUnitProject project2 = NUnitProject.LoadProject( xmlfile );

			Assert.Equals( 2, project2.Configs.Count );
			Assert.True( project2.Configs.Contains( "Debug" ) );
			Assert.True( project2.Configs.Contains( "Release" ) );
		}

		[Test]
		public void SaveAndLoadConfigsWithAssemblies()
		{
			ProjectConfig config1 = new ProjectConfig( "Debug" );
			config1.Assemblies.Add( Path.GetFullPath( @"bin\debug\assembly1.dll" ) );
			config1.Assemblies.Add( Path.GetFullPath( @"bin\debug\assembly2.dll" ) );

			ProjectConfig config2 = new ProjectConfig( "Release" );
			config2.Assemblies.Add( Path.GetFullPath( @"bin\release\assembly1.dll" ) );
			config2.Assemblies.Add( Path.GetFullPath( @"bin\release\assembly2.dll" ) );

			project.Configs.Add( config1 );
			project.Configs.Add( config2 );
			project.Save( xmlfile );

			Assert.True( File.Exists( xmlfile ) );

			NUnitProject project2 = NUnitProject.LoadProject( xmlfile );

			Assert.Equals( 2, project2.Configs.Count );

			config1 = project2.Configs["Debug"];
			Assert.Equals( 2, config1.Assemblies.Count );
			Assert.Equals( Path.GetFullPath( @"bin\debug\assembly1.dll" ), config1.Assemblies[0].FullPath );
			Assert.Equals( Path.GetFullPath( @"bin\debug\assembly2.dll" ), config1.Assemblies[1].FullPath );

			config2 = project2.Configs["Release"];
			Assert.Equals( 2, config2.Assemblies.Count );
			Assert.Equals( Path.GetFullPath( @"bin\release\assembly1.dll" ), config2.Assemblies[0].FullPath );
			Assert.Equals( Path.GetFullPath( @"bin\release\assembly2.dll" ), config2.Assemblies[1].FullPath );
		}
	}
}
