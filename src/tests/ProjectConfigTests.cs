#region Copyright (c) 2002, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright  2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
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
' Portions Copyright  2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
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
using NUnit.Util;

namespace NUnit.Tests
{
	/// <summary>
	/// Summary description for ProjectConfigTests.
	/// </summary>
	[TestFixture]
	public class ProjectConfigTests
	{
		private ProjectConfig config;
		private Project project;

		[SetUp]
		public void SetUp()
		{
			config = new ProjectConfig( "Debug" );
			project = new Project( @"c:\test\myproject.nunit" );
			config.Project = project;
		}

		[Test]
		public void EmptyConfig()
		{
			Assert.Equals( "Debug", config.Name );
			Assert.Equals( 0, config.Assemblies.Count );
		}

		[Test]
		public void CanAddAssemblies()
		{
			config.Assemblies.Add( @"C:\assembly1.dll" );
			config.Assemblies.Add( "assembly2.dll" );
			Assertion.AssertEquals( 2, config.Assemblies.Count );
			Assertion.AssertEquals( @"C:\assembly1.dll", config.Assemblies[0] );
			Assertion.AssertEquals( "assembly2.dll", config.Assemblies[1] );
		}

		[Test]
		public void AddMarksProjectDirty()
		{
			config.Assemblies.Add( @"bin\debug\assembly1.dll" );
			Assert.True( project.IsDirty );
		}

		[Test]
		public void RenameMarksProjectDirty()
		{
			config.Name = "Renamed";
			Assert.True( project.IsDirty );
		}

		[Test]
		public void RemoveMarksProjectDirty()
		{
			config.Assemblies.Add( @"bin\debug\assembly1.dll" );
			project.IsDirty = false;
			config.Assemblies.Remove( @"bin\debug\assembly1.dll" );
			Assert.True( project.IsDirty );			
		}

		[Test]
		public void SettingApplicationBaseMarksProjectDirty()
		{
			config.BasePath = @"c:\junk";
			Assert.True( project.IsDirty );
		}

		[Test]
		public void AbsoluteBasePath()
		{
			config.BasePath = @"c:\junk";
			config.Assemblies.Add( @"bin\debug\assembly1.dll" );
			Assert.Equals( @"c:\junk\bin\debug\assembly1.dll", config.Assemblies.FullNames[0] );
		}

		[Test]
		public void RelativeBasePath()
		{
			config.BasePath = @"junk";
			config.Assemblies.Add( @"bin\debug\assembly1.dll" );
			Assert.Equals( @"c:\test\junk\bin\debug\assembly1.dll", config.Assemblies.FullNames[0] );
		}

		[Test]
		public void NoBasePathSet()
		{
			config.Assemblies.Add( @"bin\debug\assembly1.dll" );
			Assert.Equals( @"c:\test\bin\debug\assembly1.dll", config.Assemblies.FullNames[0] );
		}

		[Test]
		public void SettingConfigurationFileMarksProjectDirty()
		{
			config.ConfigurationFile = "MyProject.config";
			Assert.True( project.IsDirty );
		}

		[Test]
		public void DefaultConfigurationFile()
		{
		//	Assert.Equals( "myproject.config", config.ConfigurationFile );
			Assert.Equals( @"c:\test\myproject.config", config.ConfigurationFilePath );
		}

		[Test]
		public void AbsoluteConfigurationFile()
		{
			config.ConfigurationFile = @"c:\configs\myconfig.config";
			Assert.Equals( @"c:\configs\myconfig.config", config.ConfigurationFilePath );
		}

		[Test]
		public void RelativeConfigurationFile()
		{
			config.ConfigurationFile = "myconfig.config";
			Assert.Equals( @"c:\test\myconfig.config", config.ConfigurationFilePath );
		}

		[Test]
		public void SettingPrivateBinPathMarksProjectDirty()
		{
			config.BinPath = @"c:\junk;c:\bin";
			Assert.True( project.IsDirty );
		}

		[Test]
		public void SettingAutoBinPathMarksProjectDirty()
		{
			config.AutoBinPath = !config.AutoBinPath;
			Assert.True( project.IsDirty );
		}

		[Test]
		public void NoPrivateBinPath()
		{
			config.Assemblies.Add( @"C:\bin\assembly1.dll" );
			config.Assemblies.Add( @"C:\bin\assembly2.dll" );
			config.AutoBinPath = false;
			Assert.Null( config.FullBinPath );
		}

		[Test]
		public void ManualPrivateBinPath()
		{
			config.Assemblies.Add( @"C:\bin\assembly1.dll" );
			config.Assemblies.Add( @"C:\bin\assembly2.dll" );
			config.AutoBinPath = false;
			config.BinPath = @"C:\test";
			Assert.Equals( @"C:\test", config.FullBinPath );
		}

		[Test]
		public void AutoPrivateBinPath()
		{
			config.Assemblies.Add( @"C:\bin\assembly1.dll" );
			config.Assemblies.Add( @"C:\bin\assembly2.dll" );
			config.AutoBinPath = true;
			Assert.Equals( @"C:\bin", config.FullBinPath );
		}

		[Test]
		public void CombinedPrivateBinPath()
		{
			config.Assemblies.Add( @"C:\bin\assembly1.dll" );
			config.Assemblies.Add( @"C:\bin\assembly2.dll" );
			config.AutoBinPath = true;
			config.BinPath = @"C:\test";
			Assert.Equals( @"C:\bin;C:\test", config.FullBinPath );
		}
	}
}
