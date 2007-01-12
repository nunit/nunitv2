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
using NUnit.Framework;
using NUnit.TestUtilities;

namespace NUnit.Util.Tests
{
	[TestFixture]
	public class VSProjectTests
	{
		private string invalidFile = "invalid.csproj";
		private string resourceDir = "resources";

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
		public void NotWebProject()
		{
			Assert.IsFalse(VSProject.IsProjectFile( @"http://localhost/web.csproj") );
			Assert.IsFalse(VSProject.IsProjectFile( @"C:\MyProject\http://localhost/web.csproj") );
		}

		private void AssertCanLoadProject( string resourceName )
		{
			string fileName = Path.GetFileNameWithoutExtension( resourceName );
			using( TempResourceFile file = new TempResourceFile( this.GetType(), resourceDir + "." + resourceName, resourceName ) )
			{
				VSProject project = new VSProject( file.Path );
				Assert.AreEqual( fileName, project.Name );
				Assert.AreEqual( Path.GetFullPath( file.Path ), project.ProjectPath );
				Assert.AreEqual( fileName.ToLower(), Path.GetFileNameWithoutExtension( project.Configs[0].Assemblies[0].ToString().ToLower() ) );
			}
		}

		[Test]
		public void LoadCsharpProject()
		{
			AssertCanLoadProject( "csharp-sample.csproj" );
		}

		[Test]
		public void LoadCsharpProjectVS2005()
		{
			AssertCanLoadProject( "csharp-sample_VS2005.csproj" );
		}

		[Test]
		public void LoadVbProject()
		{
			AssertCanLoadProject( "vb-sample.vbproj" );
		}


		[Test]
		public void LoadVbProjectVS2005()
		{
			AssertCanLoadProject( "vb-sample_VS2005.vbproj" );
		}

		[Test]
		public void LoadJsharpProject()
		{
			AssertCanLoadProject( "jsharp.vjsproj" );
		}

		[Test]
		public void LoadJsharpProjectVS2005()
		{
			AssertCanLoadProject( "jsharp_VS2005.vjsproj" );
		}

		[Test]
		public void LoadCppProject()
		{
			AssertCanLoadProject( "cpp-sample.vcproj" );
		}

		[Test]
		public void LoadCppProjectVS2005()
		{
			AssertCanLoadProject( "cpp-sample_VS2005.vcproj" );
		}

		[Test]
		public void LoadProjectWithHebrewFileIncluded()
		{
			AssertCanLoadProject( "HebrewFileProblem.csproj" );
		}

		[Test]
		public void LoadCppProjectWithMacros()
		{
			using ( TempResourceFile file = new TempResourceFile(this.GetType(), "resources.CPPLibrary.vcproj", "CPPLibrary.vcproj" ))
			{
				VSProject project = new VSProject(file.Path);
				Assert.AreEqual( "CPPLibrary", project.Name );
				Assert.AreEqual( Path.GetFullPath(file.Path), project.ProjectPath);
				Assert.AreEqual( Path.GetFullPath( @"debug\cpplibrary.dll" ).ToLower(), 
					project.Configs["Debug|Win32"].Assemblies[0].ToString().ToLower());
				Assert.AreEqual( Path.GetFullPath( @"release\cpplibrary.dll" ).ToLower(), 
					project.Configs["Release|Win32"].Assemblies[0].ToString().ToLower());
			}
		}

        [Test]
        public void GenerateCorrectExtensionsFromVCProjectVS2005()     
		{
            using (TempResourceFile file = new TempResourceFile(this.GetType(), "resources.cpp-default-library_VS2005.vcproj", "cpp-default-library_VS2005.vcproj"))           
			{
                VSProject project = new VSProject(file.Path);
                Assert.AreEqual("cpp-default-library_VS2005", project.Name);
                Assert.AreEqual(Path.GetFullPath(file.Path), project.ProjectPath);
                Assert.AreEqual(Path.GetFullPath(@"debug\cpp-default-library_VS2005.dll").ToLower(),
                    project.Configs["Debug|Win32"].Assemblies[0].ToString().ToLower());
                Assert.AreEqual(Path.GetFullPath(@"release\cpp-default-library_VS2005.dll").ToLower(),
                    project.Configs["Release|Win32"].Assemblies[0].ToString().ToLower());
            }
        }

		[Test, ExpectedException( typeof ( ArgumentException ) ) ]
		public void LoadInvalidFileType()
		{
			new VSProject( @"\test.junk" );
		}

		[Test, ExpectedException( typeof ( FileNotFoundException ) ) ]
		public void FileNotFoundError()
		{
			new VSProject( @"\junk.csproj" );
		}

		[Test, ExpectedException( typeof( ArgumentException ) )]
		public void InvalidXmlFormat()
		{
			WriteInvalidFile( "<VisualStudioProject><junk></VisualStudioProject>" );
			new VSProject( @"." + System.IO.Path.DirectorySeparatorChar + "invalid.csproj" );
		}

		[Test, ExpectedException( typeof( ArgumentException ) )]
		public void InvalidProjectFormat()
		{
			WriteInvalidFile( "<VisualStudioProject><junk></junk></VisualStudioProject>" );
			new VSProject( @"." + System.IO.Path.DirectorySeparatorChar  + "invalid.csproj" );
		}

		[Test, ExpectedException( typeof( ArgumentException ) )]
		public void MissingAttributes()
		{
			WriteInvalidFile( "<VisualStudioProject><CSharp><Build><Settings></Settings></Build></CSharp></VisualStudioProject>" );
			new VSProject( @"." + System.IO.Path.DirectorySeparatorChar + "invalid.csproj" );
		}

		[Test]
		public void NoConfigurations()
		{
			WriteInvalidFile( "<VisualStudioProject><CSharp><Build><Settings AssemblyName=\"invalid\" OutputType=\"Library\"></Settings></Build></CSharp></VisualStudioProject>" );
			VSProject project = new VSProject( @"." + System.IO.Path.DirectorySeparatorChar  + "invalid.csproj" );
			Assert.AreEqual( 0, project.Configs.Count );
		}
	}
}
