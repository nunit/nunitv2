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
	public class VSProjectTests
	{
		[Test]
		public void SolutionExtension()
		{
			Assert.True( VSProject.IsSolutionFile( @"\x\y\project.sln" ) );
			Assert.False( VSProject.IsSolutionFile( @"\x\y\project.sol" ) );
		}

		[Test]
		public void ProjectExtensions()
		{
			Assert.True( VSProject.IsProjectFile( @"\x\y\project.csproj" ) );
			Assert.True( VSProject.IsProjectFile( @"\x\y\project.vbproj" ) );
			Assert.True( VSProject.IsProjectFile( @"\x\y\project.vcproj" ) );
			Assert.False( VSProject.IsProjectFile( @"\x\y\project.xyproj" ) );
		}

		[Test]
		public void LoadCsharpProject()
		{
			string fileName = @"..\..\nunit.tests.dll.csproj";
			#if NANTBUILD
			fileName = @"..\tests\nunit.tests.dll.csproj";
			#endif
			VSProject project = new VSProject( fileName );

			Assert.Equals( "nunit.tests.dll", project.Name );
			Assert.Equals( Path.GetFullPath( fileName ), project.ProjectPath );
			Assert.Equals( 2, project.Configs.Count );
			Assert.True( "Missing Debug config", project.Configs.Contains( "Debug" ) );
			Assert.True( "Missing Release config", project.Configs.Contains( "Release" ) );
			Assert.True( "Missing dll", project.Configs["Debug"].Assemblies[0].FullPath.ToLower().EndsWith( @"\bin\debug\nunit.tests.dll" ) );
		}

		[Test]
		public void LoadVbProject()
		{
			string fileName = @"..\..\..\samples\vb\vb-sample.vbproj";
			#if NANTBUILD
			fileName = @"..\samples\vb\vb-sample.vbproj";
			#endif
			VSProject project = new VSProject( fileName );

			Assert.Equals( "vb-sample", project.Name );
			Assert.Equals( Path.GetFullPath( fileName ), project.ProjectPath );
			Assert.Equals( 2, project.Configs.Count );
			Assert.True( "Missing Debug config", project.Configs.Contains( "Debug" ) );
			Assert.True( "Missing Release config", project.Configs.Contains( "Release" ) );
			Assert.True( "Missing dll", project.Configs["Debug"].Assemblies[0].FullPath.ToLower().EndsWith( @"samples\vb\bin\vb-sample.dll" ) );
		}
		[Test]
		public void LoadCppProject()
		{
			string fileName = @"..\..\..\samples\cpp-sample\cpp-sample.vcproj";
			#if NANTBUILD
			fileName = @"..\samples\cpp-sample\cpp-sample.vcproj";
			#endif
			VSProject project = new VSProject( fileName );

			Assert.Equals( "cpp-sample", project.Name );
			Assert.Equals( Path.GetFullPath( fileName ), project.ProjectPath );
			Assert.Equals( 2, project.Configs.Count );
			Assert.True( "Missing Debug config", project.Configs.Contains( "Debug|Win32" ) );
			Assert.True( "Missing Release config", project.Configs.Contains( "Release|Win32" ) );
			Assert.True( "Missing dll", project.Configs["Debug|Win32"].Assemblies[0].FullPath.ToLower().EndsWith( @"samples\cpp-sample\debug\cpp-sample.dll" ) );
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
			Assert.Equals( 0, project.Configs.Count );
		}
	}
}
