#region Copyright (c) 2002-2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
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

		// Set RecentFiles to a list of known values up
		// to a maximum. Most recent will be "1", next 
		// "2", and so on...
		private void SetMockValues( int count )
		{
			for( int num = count; num > 0; --num )
				projects.RecentFile = num.ToString();			
		}

		// Check that the list is set right: 1, 2, ...
		private void CheckMockValues( int count )
		{
			IList files = projects.GetFiles();
			Assert.AreEqual( count, files.Count, "Count" );
			
			for( int index = 0; index < count; index++ )
				Assert.AreEqual( (index + 1).ToString(), files[index], "Item" ); 
		}

		// Check that we can add count items correctly
		private void CheckAddItems( int count )
		{
			SetMockValues( count );
			Assert.AreEqual( "1", projects.RecentFile, "RecentFile" );

			if ( count > UserSettings.RecentProjects.MaxFiles )
				count = UserSettings.RecentProjects.MaxFiles;

			CheckMockValues( count );
		}

		// Check that the list contains a set of entries
		// in the order given and nothing else.
		private void CheckListContains( params int[] item )
		{
			IList files = projects.GetFiles();
			Assert.AreEqual( item.Length, files.Count, "Count" );

			for( int index = 0; index < files.Count; index++ )
				Assert.AreEqual( item[index].ToString(), files[index], "Item" );
		}

		[Test]
		public void RetrieveSubKey()
		{
			Assert.IsNotNull(projects);
		}

		[Test]
		public void StorageName()
		{
			Assert.AreEqual( @"Recent-Projects", projects.Storage.StorageName );
		}

		[Test]
		public void StorageKey()
		{
			Assert.AreEqual( @"HKEY_CURRENT_USER\Software\Nascent Software\Nunit-Test\Recent-Projects", 
				((RegistrySettingsStorage)projects.Storage).StorageKey.Name );
		}

		[Test]
		public void DefaultRecentFilesCount()
		{
			Assert.AreEqual( RecentProjectSettings.DefaultSize, projects.MaxFiles );
		}

		[Test]
		public void RecentFilesCount()
		{
			projects.MaxFiles = 12;
			Assert.AreEqual( 12, projects.MaxFiles );
		}

		[Test]
		public void RecentFilesCountOverMax()
		{
			projects.MaxFiles = RecentProjectSettings.MaxSize + 1;
			Assert.AreEqual( RecentProjectSettings.MaxSize, projects.MaxFiles );
		}

		[Test]
		public void RecentFilesCountUnderMin()
		{
			projects.MaxFiles = RecentProjectSettings.MinSize - 1;
			Assert.AreEqual( RecentProjectSettings.MinSize, projects.MaxFiles );
		}

		[Test]
		public void RecentFilesCountAtMax()
		{
			projects.MaxFiles = RecentProjectSettings.MaxSize;
			Assert.AreEqual( RecentProjectSettings.MaxSize, projects.MaxFiles );
		}

		[Test]
		public void RecentFilesCountAtMin()
		{
			projects.MaxFiles = RecentProjectSettings.MinSize;
			Assert.AreEqual( RecentProjectSettings.MinSize, projects.MaxFiles );
		}

		[Test]
		public void EmptyList()
		{
			Assert.IsNotNull(  projects.GetFiles(), "GetFiles() returned null" );
			Assert.AreEqual( 0, projects.GetFiles().Count );
			Assert.IsNull( projects.RecentFile, "No RecentFile should return null" );
		}

		[Test]
		public void AddSingleItem()
		{
			CheckAddItems( 1 );
		}

		[Test]
		public void AddMaxItems()
		{
			CheckAddItems( 5 );
		}

		[Test]
		public void AddTooManyItems()
		{
			CheckAddItems( 10 );
		}

		[Test]
		public void IncreaseSize()
		{
			projects.MaxFiles = 10;
			CheckAddItems( 10 );
		}

		[Test]
		public void ReduceSize()
		{
			projects.MaxFiles = 3;
			CheckAddItems( 10 );
		}

		[Test]
		public void IncreaseSizeAfterAdd()
		{
			SetMockValues(5);
			projects.MaxFiles = 7;
			projects.RecentFile = "30";
			projects.RecentFile = "20";
			projects.RecentFile = "10";
			CheckListContains( 10, 20, 30, 1, 2, 3, 4 );
		}

		[Test]
		public void ReduceSizeAfterAdd()
		{
			SetMockValues( 5 );
			projects.MaxFiles = 3;
			CheckMockValues( 3 );
		}

		[Test]
		public void ReduceSizeUpdatesRegistry()
		{
			SetMockValues(4);
			projects.MaxFiles = 2;

			using( RegistryKey key = NUnitRegistry.CurrentUser.OpenSubKey( "Recent-Projects" ) )
			{
				Assert.AreEqual( 3, key.ValueCount );
				Assert.AreEqual( 2, key.GetValue( "MaxFiles" ) );
				Assert.AreEqual( "1", key.GetValue( "File1" ) );
				Assert.AreEqual( "2", key.GetValue( "File2" ) );
			}
		}

		[Test]
		public void AddUpdatesRegistry()
		{		
			SetMockValues( 2 );

			using( RegistryKey key = NUnitRegistry.CurrentUser.OpenSubKey( "Recent-Projects" ) )
			{
				Assert.AreEqual( 2, key.ValueCount );
				Assert.AreEqual( "1", key.GetValue( "File1" ) );
				Assert.AreEqual( "2", key.GetValue( "File2" ) );
			}
		}

		[Test]
		public void ReorderLastProject()
		{
			SetMockValues( 5 );
			projects.RecentFile = "5";
			CheckListContains( 5, 1, 2, 3, 4 );
		}

		[Test]
		public void ReorderSingleProject()
		{
			SetMockValues( 5 );
			projects.RecentFile = "3";
			CheckListContains( 3, 1, 2, 4, 5 );
		}

		[Test]
		public void ReorderMultipleProjects()
		{
			SetMockValues( 5 );
			projects.RecentFile = "3";
			projects.RecentFile = "5";
			projects.RecentFile = "2";
			CheckListContains( 2, 5, 3, 1, 4 );
		}

		[Test]
		public void ReorderSameProject()
		{
			SetMockValues( 5 );
			projects.RecentFile = "1";
			CheckListContains( 1, 2, 3, 4, 5 );
		}

		[Test]
		public void ReorderWithListNotFull()
		{
			SetMockValues( 3 );
			projects.RecentFile = "3";
			CheckListContains( 3, 1, 2 );
		}

		[Test]
		public void RemoveFirstProject()
		{
			SetMockValues( 3 );
			projects.Remove("1");
			CheckListContains( 2, 3 );
		}

		[Test]
		public void RemoveOneProject()
		{
			SetMockValues( 4 );
			projects.Remove("2");
			CheckListContains( 1, 3, 4 );
		}

		[Test]
		public void RemoveMultipleProjects()
		{
			SetMockValues( 5 );
			projects.Remove( "3" );
			projects.Remove( "1" );
			projects.Remove( "4" );
			CheckListContains( 2, 5 );
		}
		
		[Test]
		public void RemoveLastProject()
		{
			SetMockValues( 5 );
			projects.Remove("5");
			CheckListContains( 1, 2, 3, 4 );
		}
	}
}
