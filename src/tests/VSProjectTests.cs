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

using System;
using System.IO;
using NUnit.Util;
using NUnit.Framework;

namespace NUnit.Tests.Util
{
	/// <summary>
	/// Summary description for VSProjectTests.
	/// </summary>
	[TestFixture]
	public class VSProjectTests : FixtureBase
	{
		private string invalidFile = "invalid.csproj";

		private void WriteInvalidFile( string text )
		{
			StreamWriter writer = new StreamWriter( invalidFile );
			writer.WriteLine( text );
			writer.Close();
		}

		[TearDown]
		public void EraseInvalidFile()
		{
			if ( File.Exists( invalidFile ) )
				File.Delete( invalidFile );
		}

		[Test]
		public void SolutionExtension()
		{
			Assert.IsTrue( VSProject.IsSolutionFile( @"\x\y\project.sln" ) );
			Assert.IsFalse( VSProject.IsSolutionFile( @"\x\y\project.sol" ) );
		}

		[Test]
		public void ProjectExtensions()
		{
			Assert.IsTrue( VSProject.IsProjectFile( @"\x\y\project.csproj" ) );
			Assert.IsTrue( VSProject.IsProjectFile( @"\x\y\project.vbproj" ) );
			Assert.IsTrue( VSProject.IsProjectFile( @"\x\y\project.vcproj" ) );
			Assert.IsFalse( VSProject.IsProjectFile( @"\x\y\project.xyproj" ) );
		}

		[Test]
		public void LoadCsharpProject()
		{
			string fileName = GetSamplesPath( @"csharp\csharp-sample.csproj" );
			VSProject project = new VSProject( fileName );

			Assert.AreEqual( "csharp-sample", project.Name );
			Assert.AreEqual( Path.GetFullPath( fileName ), project.ProjectPath );
			Assert.AreEqual( "csharp-sample.dll", Path.GetFileName( project.Configs["Debug"].Assemblies[0].ToString().ToLower() ) );
		}

		[Test]
		public void LoadVbProject()
		{
			string fileName = GetSamplesPath( @"vb\vb-sample.vbproj" );
			VSProject project = new VSProject( fileName );

			Assert.AreEqual( "vb-sample", project.Name );
			Assert.AreEqual( Path.GetFullPath( fileName ), project.ProjectPath );
			Assert.AreEqual( "vb-sample.dll", Path.GetFileName( project.Configs["Debug"].Assemblies[0].ToString().ToLower() ) );
		}
		
		[Test]
		public void LoadJsharpProject()
		{
			string fileName = GetSamplesPath( @"jsharp\jsharp.vjsproj" );
			VSProject project = new VSProject( fileName );

			Assert.AreEqual( "jsharp", project.Name );
			Assert.AreEqual( Path.GetFullPath( fileName ), project.ProjectPath );
			Assert.AreEqual( "jsharp.dll", Path.GetFileName( project.Configs["Debug"].Assemblies[0].ToString().ToLower() ) );
		}

		[Test]
		public void LoadCppProject()
		{
			string fileName = GetSamplesPath( @"cpp-sample\cpp-sample.vcproj" );
			VSProject project = new VSProject( fileName );

			Assert.AreEqual( "cpp-sample", project.Name );
			Assert.AreEqual( Path.GetFullPath( fileName ), project.ProjectPath );
			Assert.AreEqual( "cpp-sample.dll", Path.GetFileName( project.Configs["Debug|Win32"].Assemblies[0].ToString().ToLower() ) );
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
			Assert.AreEqual( 0, project.Configs.Count );
		}
	}
}
