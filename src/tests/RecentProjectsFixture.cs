#region Copyright (c) 2002-2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright © 2000-2002 Philip A. Craig
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
' Portions Copyright © 2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright © 2000-2002 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

namespace NUnit.Tests.Util
{
	using System;
	using System.Collections;
	using Microsoft.Win32;

	using NUnit.Framework;
	using NUnit.Util;

	/// <summary>
	/// This fixture is used to test both RecentProjects and
	/// its base class RecentFiles.  If we add any other derived
	/// classes, the tests should be refactored.
	/// </summary>
	[TestFixture]
	public class RecentProjectsFixture
	{
		RecentProjectSettings projects;

		[SetUp]
		public void SetUp()
		{
			NUnitRegistry.TestMode = true;
			NUnitRegistry.ClearTestKeys();
			projects = UserSettings.RecentProjects;
		}

		[TearDown]
		public void TearDown()
		{
			NUnitRegistry.TestMode = false;
		}

		[Test]
		public void RetrieveSubKey()
		{
			Assert.NotNull(projects);
		}

		[Test]
		public void RecentProjectBasicTests()
		{
			Assert.Equals( @"Recent-Projects", projects.Storage.StorageName );
			Assert.Equals( @"HKEY_CURRENT_USER\Software\Nascent Software\Nunit-Test\Recent-Projects", 
				((RegistrySettingsStorage)projects.Storage).StorageKey.Name );
			
			Assert.NotNull(  projects.GetFiles(), "GetFiles() returned null" );
			Assert.Equals( 0, projects.GetFiles().Count );
			Assert.Null( projects.RecentFile, "No RecentFile should return null" );

			projects.RecentFile = "one";
			projects.RecentFile = "two";
			Assert.Equals( 2, projects.GetFiles().Count );
			Assert.Equals( "two", projects.RecentFile );

			using( RegistryKey key = NUnitRegistry.CurrentUser.OpenSubKey( "Recent-Projects" ) )
			{
				Assert.Equals( 2, key.ValueCount );
				Assert.Equals( "two", key.GetValue( "File1" ) );
				Assert.Equals( "one", key.GetValue( "File2" ) );
			}
		}

		[Test]
		public void GetMostRecentProject()
		{
			string projectFileName = "tests.dll";
			Assert.Null(projects.RecentFile, "first time this should be null");
			projects.RecentFile = projectFileName;
			Assert.Equals(projectFileName, projects.RecentFile);
		}

		[Test]
		public void GetProjects()
		{
			projects.RecentFile = "3";
			projects.RecentFile = "2";
			projects.RecentFile = "1";
			IList list = projects.GetFiles();
			Assert.Equals(3, list.Count);
			Assert.Equals("1", list[0]);
		}


		private void SetMockRegistryValues()
		{
			projects.RecentFile = "5";
			projects.RecentFile = "4";
			projects.RecentFile = "3";
			projects.RecentFile = "2";
			projects.RecentFile = "1";  // this is the most recent
		}

		[Test]
		public void ReorderProjects5()
		{
			SetMockRegistryValues();
			projects.RecentFile = "5";

			IList projectList = projects.GetFiles();
			Assert.Equals("5", projectList[0]);
			Assert.Equals("1", projectList[1]);
			Assert.Equals("2", projectList[2]);
			Assert.Equals("3", projectList[3]);
			Assert.Equals("4", projectList[4]);
		}

		[Test]
		public void ReorderProjects4()
		{
			SetMockRegistryValues();
			projects.RecentFile = "4";

			IList projectList = projects.GetFiles();
			Assert.Equals("4", projectList[0]);
			Assert.Equals("1", projectList[1]);
			Assert.Equals("2", projectList[2]);
			Assert.Equals("3", projectList[3]);
			Assert.Equals("5", projectList[4]);
		}

		[Test]
		public void ReorderProjectsNew()
		{
			SetMockRegistryValues();
			projects.RecentFile = "6";

			IList projectList = projects.GetFiles();
			Assert.Equals("6", projectList[0]);
			Assert.Equals("1", projectList[1]);
			Assert.Equals("2", projectList[2]);
			Assert.Equals("3", projectList[3]);
			Assert.Equals("4", projectList[4]);
		}


		[Test]
		public void ReorderProjects3()
		{
			SetMockRegistryValues();
			projects.RecentFile = "3";

			IList projectList = projects.GetFiles();
			Assert.Equals("3", projectList[0]);
			Assert.Equals("1", projectList[1]);
			Assert.Equals("2", projectList[2]);
			Assert.Equals("4", projectList[3]);
			Assert.Equals("5", projectList[4]);
		}

		[Test]
		public void ReorderProjects2()
		{
			SetMockRegistryValues();
			projects.RecentFile = "2";

			IList projectList = projects.GetFiles();
			Assert.Equals("2", projectList[0]);
			Assert.Equals("1", projectList[1]);
			Assert.Equals("3", projectList[2]);
			Assert.Equals("4", projectList[3]);
			Assert.Equals("5", projectList[4]);
		}

		[Test]
		public void ReorderProjects1()
		{
			SetMockRegistryValues();
			projects.RecentFile = "1";

			IList projectList = projects.GetFiles();
			Assert.Equals("1", projectList[0]);
			Assert.Equals("2", projectList[1]);
			Assert.Equals("3", projectList[2]);
			Assert.Equals("4", projectList[3]);
			Assert.Equals("5", projectList[4]);
		}

		[Test]
		public void AddprojectListNotFull()
		{
			projects.RecentFile = "3";
			projects.RecentFile = "2";
			projects.RecentFile = "1";  // this is the most recent

			projects.RecentFile = "3";

			IList projectList = projects.GetFiles();
			Assert.Equals(3, projectList.Count);
			Assert.Equals("3", projectList[0]);
			Assert.Equals("1", projectList[1]);
			Assert.Equals("2", projectList[2]);
		}

		[Test]
		public void AddProjectToList()
		{
			projects.RecentFile = "1";
			projects.RecentFile = "3";

			IList projectList = projects.GetFiles();
			Assert.Equals(2, projectList.Count);
			Assert.Equals("3", projectList[0]);
			Assert.Equals("1", projectList[1]);
		}

		[Test]
		public void RemoveProjectFromList()
		{
			projects.RecentFile = "3";
			projects.RecentFile = "2";
			projects.RecentFile = "1";

			projects.Remove("2");

			IList projectList = projects.GetFiles();
			Assert.Equals(2, projectList.Count);
			Assert.Equals("1", projectList[0]);
			Assert.Equals("3", projectList[1]);
		}
	}
}
