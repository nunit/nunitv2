using System;
using System.IO;
using NUnit.Util;
using NUnit.Framework;

namespace NUnit.Tests
{
	/// <summary>
	/// Summary description for VSProjectTests.
	/// </summary>
	[TestFixture]
	public class VSProjectTests
	{
		[Test]
		public void CreateEmptyProject()
		{
			VSProject project = new VSProject();
			Assertion.AssertEquals( 0, project.Configs.Count );
		}

		[Test]
		public void SolutionExtension()
		{
			Assertion.Assert( VSProject.IsSolutionFile( @"\x\y\project.sln" ) );
			Assertion.Assert( !VSProject.IsSolutionFile( @"\x\y\project.sol" ) );
		}

		[Test]
		public void ProjectExtensions()
		{
			Assertion.Assert( VSProject.IsProjectFile( @"\x\y\project.csproj" ) );
			Assertion.Assert( VSProject.IsProjectFile( @"\x\y\project.vbproj" ) );
			Assertion.Assert( VSProject.IsProjectFile( @"\x\y\project.vcproj" ) );
			Assertion.Assert( !VSProject.IsProjectFile( @"\x\y\project.xyproj" ) );
		}

		[Test]
		public void LoadCsharpProject()
		{
			string fileName = @"..\..\nunit.tests.dll.csproj";
			VSProject project = new VSProject( fileName );

			Assertion.AssertEquals( "nunit.tests.dll", project.Name );
			Assertion.AssertEquals( fileName, project.ProjectPath );
			Assertion.AssertEquals( 2, project.Configs.Count );
			Assertion.Assert( "Missing Debug config", project.Configs.Contains( "Debug" ) );
			Assertion.Assert( "Missing Release config", project.Configs.Contains( "Release" ) );
			Assertion.Assert( "Missing dll", project.Configs["Debug"].Assemblies[0].EndsWith( @"\bin\debug\nunit.tests.dll" ) );
		}

		[Test]
		public void LoadVbProject()
		{
			string fileName = @"..\..\..\samples\vb\vb-sample.vbproj";
			VSProject project = new VSProject( fileName );

			Assertion.AssertEquals( "vb-sample", project.Name );
			Assertion.AssertEquals( fileName, project.ProjectPath );
			Assertion.AssertEquals( 2, project.Configs.Count );
			Assertion.Assert( "Missing Debug config", project.Configs.Contains( "Debug" ) );
			Assertion.Assert( "Missing Release config", project.Configs.Contains( "Release" ) );
			Assertion.Assert( "Missing dll", project.Configs["Debug"].Assemblies[0].EndsWith( @"samples\vb\bin\vb-sample.dll" ) );
		}
		[Test]
		public void LoadCppProject()
		{
			string fileName = @"..\..\..\samples\cpp-sample\cpp-sample.vcproj";
			VSProject project = new VSProject( fileName );

			Assertion.AssertEquals( "cpp-sample", project.Name );
			Assertion.AssertEquals( fileName, project.ProjectPath );
			Assertion.AssertEquals( 2, project.Configs.Count );
			Assertion.Assert( "Missing Debug config", project.Configs.Contains( "Debug|Win32" ) );
			Assertion.Assert( "Missing Release config", project.Configs.Contains( "Release|Win32" ) );
			Assertion.Assert( "Missing dll", project.Configs["Debug|Win32"].Assemblies[0].EndsWith( @"samples\cpp-sample\debug\cpp-sample.dll" ) );
		}

		[Test, ExpectedException( typeof ( ArgumentException ) ) ]
		public void LoadInvalidFileType()
		{
			VSProject project = new VSProject( @"\test.junk" );
		}

		[Test, ExpectedException( typeof ( FileNotFoundException ) ) ]
		public void FileNotFoundError()
		{
			VSProject project = new VSProject( @"\junk.csproj" );
		}

		private string invalidFile = "invalid.csproj";
		private void WriteInvalidFile( string text )
		{
			StreamWriter writer = new StreamWriter( invalidFile );
			writer.WriteLine( text );
			writer.Close();
		}

		[Test, ExpectedException( typeof( ArgumentException ) )]
		public void InvalidXmlFormat()
		{
			WriteInvalidFile( "<VisualStudioProject><junk></VisualStudioProject>" );
			VSProject project = new VSProject( @".\invalid.csproj" );
		}

		[Test, ExpectedException( typeof( ArgumentException ) )]
		public void InvalidProjectFormat()
		{
			WriteInvalidFile( "<VisualStudioProject><junk></junk></VisualStudioProject>" );
			VSProject project = new VSProject( @".\invalid.csproj" );
		}

		[Test, ExpectedException( typeof( ArgumentException ) )]
		public void MissingAttributes()
		{
			WriteInvalidFile( "<VisualStudioProject><CSharp><Build><Settings></Settings></Build></CSharp></VisualStudioProject>" );
			VSProject project = new VSProject( @".\invalid.csproj" );
		}

		[Test]
		public void NoConfigurations()
		{
			WriteInvalidFile( "<VisualStudioProject><CSharp><Build><Settings AssemblyName=\"invalid\" OutputType=\"Library\"></Settings></Build></CSharp></VisualStudioProject>" );
			VSProject project = new VSProject( @".\invalid.csproj" );
			Assertion.AssertEquals( 0, project.Configs.Count );
		}
	}
}
