#region Copyright (c) 2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
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

using System.IO;
using NUnit.Framework;
using NUnit.TestUtilities;

namespace NUnit.Util.Tests
{
	[TestFixture]
	public class NUnitProjectLoad
	{
		static readonly string xmlfile = "test.nunit";

		private NUnitProject project;

		[SetUp]
		public void SetUp()
		{
			project = NUnitProject.EmptyProject();
		}

		[TearDown]
		public void TearDown()
		{
			if ( File.Exists( xmlfile ) )
				File.Delete( xmlfile );
		}

		// Write a string out to our xml file and then load project from it
		private void LoadProject( string source )
		{
			StreamWriter writer = new StreamWriter( xmlfile );
			writer.Write( source );
			writer.Close();

			project.ProjectPath = Path.GetFullPath( xmlfile );
			project.Load();
		}

		[Test]
		public void LoadEmptyProject()
		{
			LoadProject( NUnitProjectXml.EmptyProject );
			Assert.AreEqual( 0, project.Configs.Count );
		}

		[Test]
		public void LoadEmptyConfigs()
		{
			LoadProject( NUnitProjectXml.EmptyConfigs );
			Assert.AreEqual( 2, project.Configs.Count );
			Assert.IsTrue( project.Configs.Contains( "Debug") );
			Assert.IsTrue( project.Configs.Contains( "Release") );
		}

		[Test]
		public void LoadNormalProject()
		{
			LoadProject( NUnitProjectXml.NormalProject );
			Assert.AreEqual( 2, project.Configs.Count );

			ProjectConfig config1 = project.Configs["Debug"];
			Assert.AreEqual( 2, config1.Assemblies.Count );
			Assert.AreEqual( Path.GetFullPath( @"bin\debug\assembly1.dll" ), config1.Assemblies[0].FullPath );
			Assert.AreEqual( Path.GetFullPath( @"bin\debug\assembly2.dll" ), config1.Assemblies[1].FullPath );

			ProjectConfig config2 = project.Configs["Release"];
			Assert.AreEqual( 2, config2.Assemblies.Count );
			Assert.AreEqual( Path.GetFullPath( @"bin\release\assembly1.dll" ), config2.Assemblies[0].FullPath );
			Assert.AreEqual( Path.GetFullPath( @"bin\release\assembly2.dll" ), config2.Assemblies[1].FullPath );
		}

		[Test]
		public void FromAssembly()
		{
			NUnitProject project = NUnitProject.FromAssembly( "nunit.util.tests.dll" );
			Assert.AreEqual( "Default", project.ActiveConfigName );
			Assert.AreEqual( Path.GetFullPath( "nunit.util.tests.dll" ), project.ActiveConfig.Assemblies[0].FullPath );
			Assert.IsTrue( project.IsLoadable, "Not loadable" );
			Assert.IsTrue( project.IsAssemblyWrapper, "Not wrapper" );
			Assert.IsFalse( project.IsDirty, "Not dirty" );
		}

		[Test]
		public void SaveClearsAssemblyWrapper()
		{
			NUnitProject project = NUnitProject.FromAssembly( "nunit.util.tests.dll" );
			project.Save( xmlfile );
			Assert.IsFalse( project.IsAssemblyWrapper,
				"Changed project should no longer be wrapper");
		}

		[Test]
		public void FromCSharpProject()
		{
			using(TempResourceFile file = new TempResourceFile(this.GetType(), "csharp-sample.csproj"))
			{
				NUnitProject project = NUnitProject.FromVSProject( file.Path );
				Assert.AreEqual( project.Configs[0].Name, project.ActiveConfigName );
				Assert.AreEqual( "csharp-sample.dll", Path.GetFileName( project.Configs["Debug"].Assemblies[0].FullPath.ToLower() ) );
				Assert.IsTrue( project.IsLoadable, "Not loadable" );
				Assert.IsFalse( project.IsDirty, "Project should not be dirty" );
			}
		}

		[Test]
		public void FromVBProject()
		{
			using(TempResourceFile file = new TempResourceFile(this.GetType(), "vb-sample.vbproj"))
			{
				NUnitProject project = NUnitProject.FromVSProject( file.Path );
				Assert.AreEqual( "vb-sample.dll", Path.GetFileName( project.Configs["Debug"].Assemblies[0].FullPath.ToLower() ) );
				Assert.AreEqual( project.Configs[0].Name, project.ActiveConfig.Name );
				Assert.IsTrue( project.IsLoadable, "Not loadable" );
				Assert.IsFalse( project.IsDirty, "Project should not be dirty" );
			}
		}

		[Test]
		public void FromJsharpProject()
		{
			using(TempResourceFile file = new TempResourceFile(this.GetType(), "jsharp.vjsproj"))
			{
				NUnitProject project = NUnitProject.FromVSProject( file.Path );
				Assert.AreEqual( "jsharp.dll", Path.GetFileName( project.Configs["Debug"].Assemblies[0].FullPath.ToLower() ) );
				Assert.AreEqual( project.Configs[0].Name, project.ActiveConfig.Name );
				Assert.IsTrue( project.IsLoadable, "Not loadable" );
				Assert.IsFalse( project.IsDirty, "Project should not be dirty" );
			}
		}

		[Test]
		public void FromCppProject()
		{
			using(TempResourceFile file = new TempResourceFile(this.GetType(), "cpp-sample.vcproj"))
			{
				NUnitProject project = NUnitProject.FromVSProject( file.Path );
				Assert.AreEqual( project.Configs[0].Name, project.ActiveConfig.Name );
				Assert.AreEqual( "cpp-sample.dll", Path.GetFileName( project.Configs["Debug|Win32"].Assemblies[0].FullPath.ToLower() ) );
				Assert.IsTrue( project.IsLoadable, "Not loadable" );
				Assert.IsFalse( project.IsDirty, "Project should not be dirty" );
			}
		}

		[Test]
		public void FromVSSolution2003()
		{
			using(new TempResourceFile(this.GetType(), "csharp-sample.csproj", @"csharp\csharp-sample.csproj"))
			using(new TempResourceFile(this.GetType(), "jsharp.vjsproj", @"jsharp\jsharp.vjsproj"))
			using(new TempResourceFile(this.GetType(), "vb-sample.vbproj", @"vb\vb-sample.vbproj"))
			using(new TempResourceFile(this.GetType(), "cpp-sample.vcproj", @"cpp-sample\cpp-sample.vcproj"))
			using(TempResourceFile file = new TempResourceFile(this.GetType(), "samples.sln"))
			{
				NUnitProject project = NUnitProject.FromVSSolution( file.Path );
				Assert.AreEqual( 4, project.Configs.Count );
				Assert.AreEqual( 3, project.Configs["Debug"].Assemblies.Count );
				Assert.AreEqual( 3, project.Configs["Release"].Assemblies.Count );
				Assert.AreEqual( 1, project.Configs["Debug|Win32"].Assemblies.Count );
				Assert.AreEqual( 1, project.Configs["Release|Win32"].Assemblies.Count );
				Assert.IsTrue( project.IsLoadable, "Not loadable" );
				Assert.IsFalse( project.IsDirty, "Project should not be dirty" );
			}
		}

		[Test]
		public void FromVSSolution2005()
		{
			using(new TempResourceFile(this.GetType(), "csharp-sample_VS2005.csproj", @"csharp\csharp-sample_VS2005.csproj"))
			using(new TempResourceFile(this.GetType(), "jsharp_VS2005.vjsproj", @"jsharp\jsharp_VS2005.vjsproj"))
			using(new TempResourceFile(this.GetType(), "vb-sample_VS2005.vbproj", @"vb\vb-sample_VS2005.vbproj"))
			using(new TempResourceFile(this.GetType(), "cpp-sample_VS2005.vcproj", @"cpp-sample\cpp-sample_VS2005.vcproj"))
			using(TempResourceFile file = new TempResourceFile(this.GetType(), "samples_VS2005.sln"))
			{
				NUnitProject project = NUnitProject.FromVSSolution( file.Path );
				Assert.AreEqual( 4, project.Configs.Count );
				Assert.AreEqual( 3, project.Configs["Debug"].Assemblies.Count );
				Assert.AreEqual( 3, project.Configs["Release"].Assemblies.Count );
				Assert.AreEqual( 1, project.Configs["Debug|Win32"].Assemblies.Count );
				Assert.AreEqual( 1, project.Configs["Release|Win32"].Assemblies.Count );
				Assert.IsTrue( project.IsLoadable, "Not loadable" );
				Assert.IsFalse( project.IsDirty, "Project should not be dirty" );
			}
		}

		[Test]
		public void FromWebApplication()
		{
			using( new TempResourceFile(this.GetType(), "ClassLibrary1.csproj", @"ClassLibrary1\ClassLibrary1.csproj" ) )
			using( TempResourceFile file = new TempResourceFile( this.GetType(), "WebApplication1.sln" ) )
			{
				NUnitProject project = NUnitProject.FromVSSolution( Path.GetFullPath( file.Path ) );
				Assert.AreEqual( 2, project.Configs.Count );
				Assert.AreEqual( 1, project.Configs["Debug"].Assemblies.Count );
				Assert.AreEqual( 1, project.Configs["Release"].Assemblies.Count );
			}
		}

		[Test]
		public void WithUnmanagedCpp()
		{
			using( new TempResourceFile( this.GetType(), "ClassLibrary1.csproj", @"ClassLibrary1\ClassLibrary1.csproj" ) )
			using( new TempResourceFile( this.GetType(), "Unmanaged.vcproj", @"Unmanaged\Unmanaged.vcproj" ) )
			using( TempResourceFile file = new TempResourceFile( this.GetType(), "Solution1.sln" ) ) 
			{
				NUnitProject project = NUnitProject.FromVSSolution( file.Path );
				Assert.AreEqual( 4, project.Configs.Count );
				Assert.AreEqual( 1, project.Configs["Debug"].Assemblies.Count );
				Assert.AreEqual( 1, project.Configs["Release"].Assemblies.Count );
				Assert.AreEqual( 1, project.Configs["Debug|Win32"].Assemblies.Count );
				Assert.AreEqual( 1, project.Configs["Release|Win32"].Assemblies.Count );
				Assert.IsTrue( project.Configs["Debug"].Assemblies[0].HasTests );
				Assert.IsTrue( project.Configs["Release"].Assemblies[0].HasTests );
				Assert.IsFalse( project.Configs["Debug|Win32"].Assemblies[0].HasTests );
				Assert.IsFalse( project.Configs["Release|Win32"].Assemblies[0].HasTests );
			}
		}
	}
}
