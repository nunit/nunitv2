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
using System.Collections;
using NUnit.Framework;

namespace NUnit.Util.Tests
{
	/// <summary>
	/// Summary description for ProjectConfigTests.
	/// </summary>
	[TestFixture]
	public class ProjectConfigTests
	{
		private ProjectConfig config;
		private NUnitProject project;

		[SetUp]
		public void SetUp()
		{
			config = new ProjectConfig( "Debug" );
			project = new NUnitProject( TestPath( "/test/myproject.nunit" ) );
			project.Configs.Add( config );
		}

        /// <summary>
        /// Take a valid Linux path and make a valid windows path out of it
        /// if we are on Windows. Change slashes to backslashes and, if the
        /// path starts with a slash, add C: in front of it.
        /// </summary>
        private string TestPath(string path)
        {
            if (Path.DirectorySeparatorChar != '/')
            {
                path = path.Replace('/', Path.DirectorySeparatorChar);
                if (path[0] == Path.DirectorySeparatorChar)
                    path = "C:" + path;
            }

            return path;
        }

		[Test]
		public void EmptyConfig()
		{
			Assert.AreEqual( "Debug", config.Name );
			Assert.AreEqual( 0, config.Assemblies.Count );
		}

		[Test]
		public void CanAddAssemblies()
		{
            string path1 = TestPath("/test/assembly1.dll");
            string path2 = TestPath("/test/assembly2.dll");
            config.Assemblies.Add(path1);
			config.Assemblies.Add( path2 );
			Assert.AreEqual( 2, config.Assemblies.Count );
			Assert.AreEqual( path1, config.Assemblies[0].FullPath );
			Assert.AreEqual( path2, config.Assemblies[1].FullPath );
		}

		[Test]
		public void GetAbsolutePaths()
		{
            string path1 = TestPath("/test/assembly1.dll");
            string path2 = TestPath("/test/assembly2.dll");
            config.Assemblies.Add( path1 );
			config.Assemblies.Add( path2 );

			string[] files = config.AbsolutePaths;
			Assert.AreEqual( path1, files[0] );
			Assert.AreEqual( path2, files[1] );
		}

		[Test]
		public void GetRelativePaths()
		{
            string path1 = TestPath("/test/bin/debug/assembly1.dll");
            string path2 = TestPath("/test/bin/debug/assembly2.dll");
            config.Assemblies.Add( path1 );
			config.Assemblies.Add( path2 );

			string[] files = config.RelativePaths;
			Assert.AreEqual( TestPath( "bin/debug/assembly1.dll" ), files[0] );
			Assert.AreEqual( TestPath( "bin/debug/assembly2.dll" ), files[1] );
		}

		[Test]
		public void AddMarksProjectDirty()
		{
			config.Assemblies.Add( TestPath( "/test/bin/debug/assembly1.dll" ) );
			Assert.IsTrue( project.IsDirty );
		}

		[Test]
		public void RenameMarksProjectDirty()
		{
			config.Name = "Renamed";
			Assert.IsTrue( project.IsDirty );
		}

		[Test]
		public void RemoveMarksProjectDirty()
		{
            string path1 = TestPath("/test/bin/debug/assembly1.dll");
			config.Assemblies.Add( path1 );
			project.IsDirty = false;
			config.Assemblies.Remove( path1 );
			Assert.IsTrue( project.IsDirty );			
		}

		[Test]
		public void SettingApplicationBaseMarksProjectDirty()
		{
			config.BasePath = TestPath( "/junk" );
			Assert.IsTrue( project.IsDirty );
		}

		[Test]
		public void AbsoluteBasePath()
		{
            config.BasePath = TestPath("/junk");
            string path1 = TestPath( "/junk/bin/debug/assembly1.dll" );
			config.Assemblies.Add( path1 );
			Assert.AreEqual( path1, config.Assemblies[0].FullPath );
			Assert.AreEqual( TestPath( "bin/debug/assembly1.dll" ), config.RelativePaths[0] );
		}

		[Test]
		public void RelativeBasePath()
		{
			config.BasePath = @"junk";
            string path1 = TestPath("/test/junk/bin/debug/assembly1.dll");
            config.Assemblies.Add( path1 );
			Assert.AreEqual( path1, config.Assemblies[0].FullPath );
			Assert.AreEqual( TestPath( "bin/debug/assembly1.dll" ), config.RelativePaths[0] );
		}

		[Test]
		public void NoBasePathSet()
		{
            string path1 = TestPath( "/test/bin/debug/assembly1.dll" );
			config.Assemblies.Add( path1 );
			Assert.AreEqual( path1, config.Assemblies[0].FullPath );
			Assert.AreEqual( TestPath ("bin/debug/assembly1.dll"), config.RelativePaths[0] );
		}

		[Test]
		public void SettingConfigurationFileMarksProjectDirty()
		{
			config.ConfigurationFile = "MyProject.config";
			Assert.IsTrue( project.IsDirty );
		}

		[Test]
		public void DefaultConfigurationFile()
		{
			Assert.AreEqual( "myproject.config", config.ConfigurationFile );
			Assert.AreEqual( TestPath( "/test/myproject.config" ), config.ConfigurationFilePath );
		}

		[Test]
		public void AbsoluteConfigurationFile()
		{
            string path1 = TestPath("/configs/myconfig.config");
			config.ConfigurationFile = path1;
			Assert.AreEqual( path1, config.ConfigurationFilePath );
		}

		[Test]
		public void RelativeConfigurationFile()
		{
			config.ConfigurationFile = "myconfig.config";
			Assert.AreEqual( TestPath( "/test/myconfig.config" ), config.ConfigurationFilePath );
		}

		[Test]
		public void SettingPrivateBinPathMarksProjectDirty()
		{
			config.PrivateBinPath = TestPath( "/junk" ) + Path.PathSeparator + TestPath( "/bin" );
			Assert.IsTrue( project.IsDirty );
		}

		[Test]
		public void SettingBinPathTypeMarksProjectDirty()
		{
			config.BinPathType = BinPathType.Manual;
			Assert.IsTrue( project.IsDirty );
		}

		[Test]
		public void GetPrivateBinPath()
		{
            string path1 = TestPath("/test/bin/debug/test1.dll");
            string path2 = TestPath("/test/bin/debug/test2.dll");
            string path3 = TestPath("/test/utils/test3.dll");
            config.Assemblies.Add( path1 );
            config.Assemblies.Add(path2);
            config.Assemblies.Add(path3);

			Assert.AreEqual( TestPath( "bin/debug" ) + Path.PathSeparator + TestPath( "utils" ), config.PrivateBinPath ); 
		}

		[Test]
		public void NoPrivateBinPath()
		{
			config.Assemblies.Add( TestPath( "/bin/assembly1.dll" ) );
			config.Assemblies.Add( TestPath( "/bin/assembly2.dll" ) );
			config.BinPathType = BinPathType.None;
			Assert.IsNull( config.PrivateBinPath );
		}

		[Test]
		public void ManualPrivateBinPath()
		{
			config.Assemblies.Add( TestPath( "/test/bin/assembly1.dll" ) );
			config.Assemblies.Add( TestPath( "/test/bin/assembly2.dll" ) );
			config.BinPathType = BinPathType.Manual;
			config.PrivateBinPath = TestPath( "/test" );
			Assert.AreEqual( TestPath( "/test" ), config.PrivateBinPath );
		}

		[Test]
		public void AutoPrivateBinPath()
		{
			config.Assemblies.Add( TestPath( "/test/bin/assembly1.dll" ) );
			config.Assemblies.Add( TestPath( "/test/bin/assembly2.dll" ) );
			config.BinPathType = BinPathType.Auto;
			Assert.AreEqual( "bin", config.PrivateBinPath );
		}
	}
}
