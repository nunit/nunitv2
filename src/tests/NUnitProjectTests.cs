#region Copyright (c) 2002, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright © 2000-2002 Philip A. Craig
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
' Portions Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright © 2000-2002 Philip A. Craig
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
		NUnitProject project;

		[SetUp]
		public void SetUp()
		{
			File.Delete( xmlfile );
			project = new NUnitProject();
		}

		[TearDown]
		public void EraseFile()
		{
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
			Assert.Null( project.ActiveConfigName );
		}

		[Test]
		public void NewProjectIsNotDirty()
		{
			Assert.False( project.IsDirty );
		}

		[Test]
		public void NewProjectNotLoadable()
		{
			Assert.False( project.IsLoadable );
		}

		[Test]
		public void ProjectDefaultsToNonWrapper()
		{
			Assert.False( project.IsWrapper );
		}

		[Test]
		public void SaveMakesProjectNotDirty()
		{
			project.Save( xmlfile );
			Assert.False( project.IsDirty );
		}

		[Test]
		public void LoadMakesProjectNotDirty()
		{
			project.Save( xmlfile );
			NUnitProject project2 = new NUnitProject();
			project2.Load( xmlfile );
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
			project.ActiveConfigName = "Release";
			Assert.Equals( "Release", project.ActiveConfigName );
		}

		[Test]
		public void CanAddAssemblies()
		{
			project.Configs.Add("Debug");
			project.Configs.Add("Release");

			project.Configs["Debug"].Assemblies.Add( @"bin\debug\assembly1.dll" );
			project.Configs["Debug"].Assemblies.Add( @"bin\debug\assembly2.dll" );
			project.Configs["Release"].Assemblies.Add( @"bin\debug\assembly3.dll" );

			Assertion.AssertEquals( 2, project.Configs.Count );
			Assertion.AssertEquals( 2, project.Configs["Debug"].Assemblies.Count );
			Assertion.AssertEquals( 1, project.Configs["Release"].Assemblies.Count );
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
		public void RemoveConfigMakesProjectDirty()
		{
			project.Configs.Add("Debug");
			project.IsDirty = false;
			project.Configs.Remove("Debug");
			Assert.True( project.IsDirty );
		}

		[Test]
		public void SettingActiveConfigMakesProjectDirty()
		{
			project.Configs.Add("Debug");
			project.Configs.Add("Release");
			project.ActiveConfigName = "Debug";
			project.IsDirty = false;
			project.ActiveConfigName = "Release";
			Assert.True( project.IsDirty );
		}

		[Test]
		public void SaveAndLoadEmptyProject()
		{
			project.Save( xmlfile );
			Assert.True( File.Exists( xmlfile ) );

			NUnitProject project2 = new NUnitProject();
			project2.Load( xmlfile );

			Assert.Equals( 0, project2.Configs.Count );
		}

		[Test]
		public void SaveAndLoadEmptyConfigs()
		{
			project.Configs.Add( "Debug" );
			project.Configs.Add( "Release" );
			project.Save( xmlfile );

			Assert.True( File.Exists( xmlfile ) );

			NUnitProject project2 = new NUnitProject();
			project2.Load( xmlfile );

			Assert.Equals( 2, project2.Configs.Count );
			Assert.True( project2.Configs.Contains( "Debug" ) );
			Assert.True( project2.Configs.Contains( "Release" ) );
		}

		[Test]
		public void SaveAndLoadConfigsWithAssemblies()
		{
			ProjectConfig config1 = new ProjectConfig( "Debug" );
			config1.Assemblies.Add( @"h:\bin\debug\assembly1.dll" );
			config1.Assemblies.Add( @"h:\bin\debug\assembly2.dll" );

			ProjectConfig config2 = new ProjectConfig( "Release" );
			config2.Assemblies.Add( @"h:\bin\release\assembly1.dll" );
			config2.Assemblies.Add( @"h:\bin\release\assembly2.dll" );

			project.Configs.Add( config1 );
			project.Configs.Add( config2 );
			project.Save( xmlfile );

			Assert.True( File.Exists( xmlfile ) );

			NUnitProject project2 = new NUnitProject();
			project2.Load( xmlfile );

			Assert.Equals( 2, project2.Configs.Count );

			config1 = project2.Configs["Debug"];
			Assert.Equals( 2, config1.Assemblies.Count );
			Assert.Equals( @"h:\bin\debug\assembly1.dll", config1.Assemblies[0] );
			Assert.Equals( @"h:\bin\debug\assembly2.dll", config1.Assemblies[1] );

			config2 = project2.Configs["Release"];
			Assert.Equals( 2, config2.Assemblies.Count );
			Assert.Equals( @"h:\bin\release\assembly1.dll", config2.Assemblies[0] );
			Assert.Equals( @"h:\bin\release\assembly2.dll", config2.Assemblies[1] );
		}

		[Test]
		public void FromAssembly()
		{
			NUnitProject project = NUnitProject.FromAssembly( @"h:\bin\debug\assembly1.dll" );
			Assert.Equals( "Default", project.ActiveConfigName );
			Assert.Equals( @"h:\bin\debug\assembly1.dll", project.ActiveConfig.Assemblies[0] );
			Assert.True( project.IsWrapper );
			Assert.True( project.IsLoadable );
			Assert.True( project.IsDirty );
		}

		[Test]
		public void FromCSharpProject()
		{
			NUnitProject project = NUnitProject.FromVSProject( @"..\..\nunit.tests.dll.csproj" );
			Assert.Equals( 2, project.Configs.Count );
			Assert.True( "Missing Debug Config", project.Configs.Contains( "Debug" ) );
			Assert.True( "Missing Release Config", project.Configs.Contains( "Release" ) );
			Assert.Equals( project.Configs[0].Name, project.ActiveConfigName );
			Assert.True( "Missing nunit.tests.dll", ((string)project.Configs["Debug"].Assemblies[0]).EndsWith("nunit.tests.dll") );
			Assert.False( project.IsWrapper );
			Assert.True( project.IsLoadable );
			Assert.True( project.IsDirty );
		}

		[Test]
		public void FromVBProject()
		{
			NUnitProject project = NUnitProject.FromVSProject( @"..\..\..\samples\vb\vb-sample.vbproj" );
			Assert.Equals( 2, project.Configs.Count );
			Assert.True( "Missing Debug config", project.Configs.Contains( "Debug" ) );
			Assert.True( "Missing Release config", project.Configs.Contains( "Release" ) );
			Assert.True( "Missing vb-sample.dll", ((string)project.Configs["Debug"].Assemblies[0]).EndsWith( "vb-sample.dll" ) );
			Assert.Equals( project.Configs[0].Name, project.ActiveConfigName );
			Assert.False( project.IsWrapper );
			Assert.True( project.IsLoadable );
			Assert.True( project.IsDirty );
		}

		[Test]
		public void FromCppProject()
		{
			NUnitProject project = NUnitProject.FromVSProject( @"..\..\..\samples\cpp-sample\cpp-sample.vcproj" );
			Assert.Equals( 2, project.Configs.Count );
			Assert.True( "Missing Debug Config", project.Configs.Contains( "Debug|Win32" ) );
			Assert.True( "Missing Release Config", project.Configs.Contains( "Release|Win32" ) );
			Assert.Equals( project.Configs[0].Name, project.ActiveConfigName );
			Assert.True( "Missing cpp-sample.dll", ((string)project.Configs["Debug|Win32"].Assemblies[0]).EndsWith( "cpp-sample.dll" ) );
			Assert.False( project.IsWrapper );
			Assert.True( project.IsLoadable );
			Assert.True( project.IsDirty );
		}

		[Test]
		public void FromVSSolution()
		{
			NUnitProject project = NUnitProject.FromVSSolution( @"..\..\..\nunit.sln" );
			Assert.Equals( 4, project.Configs.Count );
			Assert.True( "Missing Debug Config", project.Configs.Contains( "Debug" ) );
			Assert.True( "Missing Release Config", project.Configs.Contains( "Release" ) );
			Assert.Equals( project.Configs[0].Name, project.ActiveConfigName );
			Assert.Equals( 14, project.Configs["Debug"].Assemblies.Count );
			Assert.False( project.IsWrapper );
			Assert.True( project.IsLoadable );
			Assert.True( project.IsDirty );
		}
	}
}
