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
using NUnit.Util;

namespace NUnit.Tests.Util
{
	/// <summary>
	/// Summary description for ProjectConfigTests.
	/// </summary>
	[TestFixture]
	public class ProjectConfigTests
	{
		private ProjectConfig config;
		private NUnitProject project;
		private bool gotChangedEvent;

		[SetUp]
		public void SetUp()
		{
			config = new ProjectConfig( "Debug" );
			project = new NUnitProject( @"C:\test\myproject.nunit" );
			project.Configs.Add( config );
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
			config.Assemblies.Add( @"C:\test\assembly1.dll" );
			config.Assemblies.Add( @"C:\test\assembly2.dll" );
			Assert.AreEqual( 2, config.Assemblies.Count );
			Assert.AreEqual( @"C:\test\assembly1.dll", config.Assemblies[0].FullPath );
			Assert.AreEqual( @"C:\test\assembly2.dll", config.Assemblies[1].FullPath );
		}

		[Test]
		public void GetAbsolutePaths()
		{
			config.Assemblies.Add( @"C:\test\assembly1.dll" );
			config.Assemblies.Add( @"C:\test\assembly2.dll" );

			string[] files = config.AbsolutePaths;
			Assertion.AssertEquals( @"C:\test\assembly1.dll", files[0] );
			Assertion.AssertEquals( @"C:\test\assembly2.dll", files[1] );
		}

		[Test]
		public void GetRelativePaths()
		{
			config.Assemblies.Add( @"C:\test\bin\debug\assembly1.dll" );
			config.Assemblies.Add( @"C:\test\bin\debug\assembly2.dll" );

			string[] files = config.RelativePaths;
			Assert.AreEqual( @"bin\debug\assembly1.dll", files[0] );
			Assert.AreEqual( @"bin\debug\assembly2.dll", files[1] );
		}

		public void GetTestAssemblies()
		{
			config.Assemblies.Add( @"C:\test\assembly1.dll" );
			config.Assemblies.Add( @"C:\test\assembly2.dll", false );
			config.Assemblies.Add( @"C:\test\assembly3.dll", true );

			string[] files = config.TestAssemblies;
			Assert.AreEqual( 2, files.Length );
			Assert.AreEqual( @"C:\test\assembly1.dll", files[0] );
			Assert.AreEqual( @"C:\test\assembly2.dll", files[1] );
		}

		[Test]
		public void AddMarksProjectDirty()
		{
			config.Assemblies.Add( @"C:\test\bin\debug\assembly1.dll" );
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
			config.Assemblies.Add( @"C:\test\bin\debug\assembly1.dll" );
			project.IsDirty = false;
			config.Assemblies.Remove( @"C:\test\bin\debug\assembly1.dll" );
			Assert.IsTrue( project.IsDirty );			
		}

		[Test]
		public void SettingApplicationBaseMarksProjectDirty()
		{
			config.BasePath = @"C:\junk";
			Assert.IsTrue( project.IsDirty );
		}

		[Test]
		public void AbsoluteBasePath()
		{
			config.BasePath = @"C:\junk";
			config.Assemblies.Add( @"C:\junk\bin\debug\assembly1.dll" );
			Assert.AreEqual( @"C:\junk\bin\debug\assembly1.dll", config.Assemblies[0].FullPath );
			Assert.AreEqual( @"bin\debug\assembly1.dll", config.RelativePaths[0] );
		}

		[Test]
		public void RelativeBasePath()
		{
			config.BasePath = @"junk";
			config.Assemblies.Add( @"C:\test\junk\bin\debug\assembly1.dll" );
			Assert.AreEqual( @"C:\test\junk\bin\debug\assembly1.dll", config.Assemblies[0].FullPath );
			Assert.AreEqual( @"bin\debug\assembly1.dll", config.RelativePaths[0] );
		}

		[Test]
		public void NoBasePathSet()
		{
			config.Assemblies.Add( @"C:\test\bin\debug\assembly1.dll" );
			Assert.AreEqual( @"C:\test\bin\debug\assembly1.dll", config.Assemblies[0].FullPath );
			Assert.AreEqual( @"bin\debug\assembly1.dll", config.RelativePaths[0] );
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
		//	Assert.Equals( "myproject.config", config.ConfigurationFile );
			Assert.AreEqual( @"C:\test\myproject.config", config.ConfigurationFilePath );
		}

		[Test]
		public void AbsoluteConfigurationFile()
		{
			config.ConfigurationFile = @"C:\configs\myconfig.config";
			Assert.AreEqual( @"C:\configs\myconfig.config", config.ConfigurationFilePath );
		}

		[Test]
		public void RelativeConfigurationFile()
		{
			config.ConfigurationFile = "myconfig.config";
			Assert.AreEqual( @"C:\test\myconfig.config", config.ConfigurationFilePath );
		}

		[Test]
		public void SettingPrivateBinPathMarksProjectDirty()
		{
			config.PrivateBinPath = @"C:\junk;C:\bin";
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
			config.Assemblies.Add( @"C:\test\bin\debug\test1.dll" );
			config.Assemblies.Add( @"C:\test\bin\debug\test2.dll" );
			config.Assemblies.Add( @"C:\test\utils\test3.dll" );

			Assert.AreEqual( @"bin\debug;utils", config.PrivateBinPath ); 
		}

		[Test]
		public void NoPrivateBinPath()
		{
			config.Assemblies.Add( @"C:\bin\assembly1.dll" );
			config.Assemblies.Add( @"C:\bin\assembly2.dll" );
			config.BinPathType = BinPathType.None;
			Assert.IsNull( config.PrivateBinPath );
		}

		[Test]
		public void ManualPrivateBinPath()
		{
			config.Assemblies.Add( @"C:\test\bin\assembly1.dll" );
			config.Assemblies.Add( @"C:\test\bin\assembly2.dll" );
			config.BinPathType = BinPathType.Manual;
			config.PrivateBinPath = @"C:\test";
			Assert.AreEqual( @"C:\test", config.PrivateBinPath );
		}

		[Test]
		public void AutoPrivateBinPath()
		{
			config.Assemblies.Add( @"C:\test\bin\assembly1.dll" );
			config.Assemblies.Add( @"C:\test\bin\assembly2.dll" );
			config.BinPathType = BinPathType.Auto;
			Assert.AreEqual( @"bin", config.PrivateBinPath );
		}

		[Test]
		public void GeneratesChangedEvent()
		{
			gotChangedEvent = false;
			config.Changed += new EventHandler( OnConfigChanged );
			config.IsDirty = true;
			Assert.IsTrue( gotChangedEvent, "Event not generated" );

			gotChangedEvent = false;
			config.IsDirty = false;
			Assert.IsFalse( gotChangedEvent, "Event generated in error" );
		}

		private void OnConfigChanged( object sender, EventArgs e )
		{
			gotChangedEvent = true;
		}
	}
}
