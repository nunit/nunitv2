using System;
using System.Xml;
using NUnit.Util;
using NUnit.Framework;

namespace NUnit.Tests
{
	[TestFixture]
	public class NUnitProjectLoad
	{
		private NUnitProject project;

		private void LoadProject( string source )
		{
			XmlTextReader reader = new XmlTextReader( source, XmlNodeType.Element, null );
			project.Load( reader );
		}

		[SetUp]
		public void SetUp()
		{
			project = NUnitProject.EmptyProject();
		}

		[Test]
		public void LoadEmptyProject()
		{
			LoadProject( NUnitProjectXml.EmptyProject );
			Assert.Equals( 0, project.Configs.Count );
		}

		[Test]
		public void LoadEmptyConfigs()
		{
			LoadProject( NUnitProjectXml.EmptyConfigs );
			Assert.Equals( 2, project.Configs.Count );
			Assert.True( project.Configs.Contains( "Debug") );
			Assert.True( project.Configs.Contains( "Release") );
		}

		[Test]
		public void LoadNormalProject()
		{
			LoadProject( NUnitProjectXml.NormalProject );
			Assert.Equals( 2, project.Configs.Count );

			ProjectConfig config1 = project.Configs["Debug"];
			Assert.Equals( 2, config1.Assemblies.Count );
			Assert.Equals( @"h:\bin\debug\assembly1.dll", config1.Assemblies[0] );
			Assert.Equals( @"h:\bin\debug\assembly2.dll", config1.Assemblies[1] );

			ProjectConfig config2 = project.Configs["Release"];
			Assert.Equals( 2, config2.Assemblies.Count );
			Assert.Equals( @"h:\bin\release\assembly1.dll", config2.Assemblies[0] );
			Assert.Equals( @"h:\bin\release\assembly2.dll", config2.Assemblies[1] );
		}

		[Test]
		public void FromAssembly()
		{
			NUnitProject project = NUnitProject.FromAssembly( @"h:\bin\debug\assembly1.dll" );
			Assert.Equals( "Default", project.ActiveConfig.Name );
			Assert.Equals( @"h:\bin\debug\assembly1.dll", project.ActiveConfig.Assemblies[0] );
			Assert.True( "Not loadable", project.IsLoadable );
			Assert.True( "Not wrapper", project.IsAssemblyWrapper );
			Assert.False( "Not dirty", project.IsDirty );
		}

		[Test]
		public void ChangeAssemblyWrapper()
		{
			NUnitProject project = NUnitProject.FromAssembly( @"h:\bin\debug\assembly1.dll" );
			project.ActiveConfig.Name = "Debug";
			Assert.False( "Changed project should no longer be wrapper", project.IsAssemblyWrapper );
		}

		[Test]
		public void FromCSharpProject()
		{
			string projectPath = @"..\..\nunit.tests.dll.csproj";
			#if NANTBUILD
			projectPath = @"..\tests\nunit.tests.dll.csproj";
			#endif
			NUnitProject project = NUnitProject.FromVSProject( projectPath );
			Assert.Equals( 2, project.Configs.Count );
			Assert.True( "Missing Debug Config", project.Configs.Contains( "Debug" ) );
			Assert.True( "Missing Release Config", project.Configs.Contains( "Release" ) );
			Assert.Equals( project.Configs[0].Name, project.ActiveConfig.Name );
			Assert.True( "Missing nunit.tests.dll", ((string)project.Configs["Debug"].Assemblies[0]).EndsWith("nunit.tests.dll") );
			Assert.True( "Not loadable", project.IsLoadable );
			Assert.True( "Not dirty", project.IsDirty );
		}

		[Test]
		public void FromVBProject()
		{
			string projectPath = @"..\..\..\samples\vb\vb-sample.vbproj";
			#if NANTBUILD
			projectPath = @"..\samples\vb\vb-sample.vbproj";
			#endif
			NUnitProject project = NUnitProject.FromVSProject( projectPath );
			Assert.Equals( 2, project.Configs.Count );
			Assert.True( "Missing Debug config", project.Configs.Contains( "Debug" ) );
			Assert.True( "Missing Release config", project.Configs.Contains( "Release" ) );
			Assert.True( "Missing vb-sample.dll", ((string)project.Configs["Debug"].Assemblies[0]).EndsWith( "vb-sample.dll" ) );
			Assert.Equals( project.Configs[0].Name, project.ActiveConfig.Name );
			Assert.True( "Not loadable", project.IsLoadable );
			Assert.True( "Not dirty", project.IsDirty );
		}

		[Test]
		public void FromCppProject()
		{
			string projectPath = @"..\..\..\samples\cpp-sample\cpp-sample.vcproj";
			#if NANTBUILD
			projectPath = @"..\samples\cpp-sample\cpp-sample.vcproj";
			#endif
			NUnitProject project = NUnitProject.FromVSProject( projectPath );
			Assert.Equals( 2, project.Configs.Count );
			Assert.True( "Missing Debug Config", project.Configs.Contains( "Debug|Win32" ) );
			Assert.True( "Missing Release Config", project.Configs.Contains( "Release|Win32" ) );
			Assert.Equals( project.Configs[0].Name, project.ActiveConfig.Name );
			Assert.True( "Missing cpp-sample.dll", ((string)project.Configs["Debug|Win32"].Assemblies[0]).EndsWith( "cpp-sample.dll" ) );
			Assert.True( "Not loadable", project.IsLoadable );
			Assert.True( "Not dirty", project.IsDirty );
		}

		[Test]
		public void FromVSSolution()
		{
			string projectPath = @"..\..\..\nunit.sln";
			#if NANTBUILD
			projectPath = @"..\nunit.sln";
			#endif
			NUnitProject project = NUnitProject.FromVSSolution( projectPath );
			Assert.Equals( 4, project.Configs.Count );
			Assert.True( "Missing Debug Config", project.Configs.Contains( "Debug" ) );
			Assert.True( "Missing Release Config", project.Configs.Contains( "Release" ) );
			Assert.Equals( project.Configs[0].Name, project.ActiveConfig.Name );
			Assert.Equals( 14, project.Configs["Debug"].Assemblies.Count );
			Assert.True( "Not loadable", project.IsLoadable );
			Assert.True( "Not dirty", project.IsDirty );
		}
	}
}
