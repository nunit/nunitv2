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
			Assertion.Assert( NUnitProject.IsProjectFile( @"\x\y\test.nunit" ) );
			Assertion.Assert( !NUnitProject.IsProjectFile( @"\x\y\test.junit" ) );
		}

		[Test]
		public void EmptyProject()
		{
			Assertion.AssertEquals( 0, project.Configs.Count );
		}

		[Test]
		public void AddOneConfig()
		{
			project.Configs.Add("Debug");
			project.Configs["Debug"].Assemblies.Add( @"bin\debug\assembly1.dll" );
			project.Configs["Debug"].Assemblies.Add( @"bin\debug\assembly2.dll" );

			Assertion.AssertEquals( 2, project.Configs["Debug"].Assemblies.Count );
		}

		[Test]
		public void AddMakesProjectDirty()
		{
			project.Configs.Add("Debug");
			Assert.True( project.IsDirty );
		}

		[Test]
		public void RenameMakesProjectDirty()
		{
			project.Configs.Add("Old");
			project.IsDirty = false;
			project.Configs[0].Name = "New";
			Assert.True( project.IsDirty );
		}

		[Test]
		public void RemoveMakesProjectDirty()
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
			project.ActiveConfig = "Debug";
			project.IsDirty = false;
			project.ActiveConfig = "Release";
			Assert.True( project.IsDirty );
		}

		[Test]
		public void AddTwoConfigs()
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
		public void SaveAndLoadEmptyProject()
		{
			project.Save( xmlfile );

			Assertion.Assert( File.Exists( xmlfile ) );

			NUnitProject project2 = new NUnitProject();
			project2.Load( xmlfile );

			Assertion.AssertEquals( 0, project2.Configs.Count );
		}

		[Test]
		public void SaveAndLoadEmptyConfigs()
		{
			project.Configs.Add( "Debug" );
			project.Configs.Add( "Release" );
			project.Save( xmlfile );

			Assertion.Assert( File.Exists( xmlfile ) );

			NUnitProject project2 = new NUnitProject();
			project2.Load( xmlfile );

			Assertion.AssertEquals( 2, project2.Configs.Count );
			Assertion.Assert( project2.Configs.Contains( "Debug" ) );
			Assertion.Assert( project2.Configs.Contains( "Release" ) );
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

			Assertion.Assert( File.Exists( xmlfile ) );

			NUnitProject project2 = new NUnitProject();
			project2.Load( xmlfile );

			Assertion.AssertEquals( 2, project2.Configs.Count );

			config1 = project2.Configs["Debug"];
			Assertion.AssertEquals( 2, config1.Assemblies.Count );
			Assertion.AssertEquals( @"h:\bin\debug\assembly1.dll", config1.Assemblies[0] );
			Assertion.AssertEquals( @"h:\bin\debug\assembly2.dll", config1.Assemblies[1] );

			config2 = project2.Configs["Release"];
			Assertion.AssertEquals( 2, config2.Assemblies.Count );
			Assertion.AssertEquals( @"h:\bin\release\assembly1.dll", config2.Assemblies[0] );
			Assertion.AssertEquals( @"h:\bin\release\assembly2.dll", config2.Assemblies[1] );
		}

		[Test]
		public void FromAssembly()
		{
			NUnitProject project = NUnitProject.FromAssembly( @"h:\bin\debug\assembly1.dll" );
			Assertion.AssertEquals( "Default", project.ActiveConfig );
			Assertion.AssertEquals( @"h:\bin\debug\assembly1.dll", project.ActiveAssemblies[0] );
		}

		[Test]
		public void FromCSharpProject()
		{
			NUnitProject project = NUnitProject.FromVSProject( @"..\..\nunit.tests.dll.csproj" );
			Assertion.AssertEquals( 2, project.Configs.Count );
			Assertion.Assert( "Missing Debug Config", project.Configs.Contains( "Debug" ) );
			Assertion.Assert( "Missing Release Config", project.Configs.Contains( "Release" ) );
			Assertion.Assert( "Missing nunit.tests.dll", project.Configs["Debug"].Assemblies[0].EndsWith("nunit.tests.dll") );
		}

		[Test]
		public void FromVBProject()
		{
			NUnitProject project = NUnitProject.FromVSProject( @"..\..\..\samples\vb\vb-sample.vbproj" );
			Assertion.AssertEquals( 2, project.Configs.Count );
			Assertion.Assert( "Missing Debug config", project.Configs.Contains( "Debug" ) );
			Assertion.Assert( "Missing Release config", project.Configs.Contains( "Release" ) );
			Assertion.Assert( "Missing vb-sample.dll", project.Configs["Debug"].Assemblies[0].EndsWith( "vb-sample.dll" ) );
		}

		[Test]
		public void FromCppProject()
		{
			NUnitProject project = NUnitProject.FromVSProject( @"..\..\..\samples\cpp-sample\cpp-sample.vcproj" );
			Assertion.AssertEquals( 2, project.Configs.Count );
			Assertion.Assert( "Missing Debug Config", project.Configs.Contains( "Debug|Win32" ) );
			Assertion.Assert( "Missing Release Config", project.Configs.Contains( "Release|Win32" ) );
			Assertion.Assert( "Missing cpp-sample.dll", project.Configs["Debug|Win32"].Assemblies[0].EndsWith( "cpp-sample.dll" ) );
		}

		[Test]
		public void FromVSSolution()
		{
			NUnitProject project = NUnitProject.FromVSSolution( @"..\..\..\nunit.sln" );
			Assertion.AssertEquals( 4, project.Configs.Count );
			Assertion.Assert( project.Configs.Contains( "Debug" ) );
			Assertion.Assert( project.Configs.Contains( "Release" ) );
			Assertion.AssertEquals( 14, project.Configs["Debug"].Assemblies.Count );
		}
	}
}
