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

namespace NUnit.Util.Tests
{
	using System;
	using System.Collections;
	using Microsoft.Win32;

	using NUnit.Framework;
	
	/// <summary>
	/// This fixture is used to test both RecentProjects and
	/// its base class RecentFiles.  If we add any other derived
	/// classes, the tests should be refactored.
	/// </summary>
	[TestFixture]
	public class RecentFilesTests
	{
		static readonly int MAX = RecentFileSettings.MaxSize;
		static readonly int MIN = RecentFileSettings.MinSize;

		RecentFileSettings recentFiles;

		[SetUp]
		public void SetUp()
		{
			recentFiles = new RecentFileSettings( new MemorySettingsStorage() );
		}

		[TearDown]
		public void TearDown()
		{
			recentFiles.Dispose();
		}

		#region Helper Methods
		// Set RecentFiles to a list of known values up
		// to a maximum. Most recent will be "1", next 
		// "2", and so on...
		private void SetMockValues( int count )
		{
			for( int num = count; num > 0; --num )
				recentFiles.SetMostRecent( num.ToString() );			
		}

		// Check that the list is set right: 1, 2, ...
		private void CheckMockValues( int count )
		{
			RecentFilesCollection files = recentFiles.Entries;
			Assert.AreEqual( count, files.Count, "Count" );
			
			for( int index = 0; index < count; index++ )
				Assert.AreEqual( (index + 1).ToString(), files[index].Path, "Item" ); 
		}

		// Check that we can add count items correctly
		private void CheckAddItems( int count )
		{
			SetMockValues( count );
			Assert.AreEqual( "1", recentFiles.Entries[0].Path, "RecentFile" );

			CheckMockValues( Math.Min( count, recentFiles.MaxFiles ) );
		}

		// Check that the list contains a set of entries
		// in the order given and nothing else.
		private void CheckListContains( params int[] item )
		{
			RecentFilesCollection files = recentFiles.Entries;
			Assert.AreEqual( item.Length, files.Count, "Count" );

			for( int index = 0; index < files.Count; index++ )
				Assert.AreEqual( item[index].ToString(), files[index].Path, "Item" );
		}
		#endregion

		[Test]
		public void CountDefault()
		{
			Assert.AreEqual( RecentFileSettings.DefaultSize, recentFiles.MaxFiles );
		}

		[Test]
		public void CountOverMax()
		{
			recentFiles.MaxFiles = MAX + 1;
			Assert.AreEqual( MAX, recentFiles.MaxFiles );
		}

		[Test]
		public void CountUnderMin()
		{
			recentFiles.MaxFiles = MIN - 1;
			Assert.AreEqual( MIN, recentFiles.MaxFiles );
		}

		[Test]
		public void CountAtMax()
		{
			recentFiles.MaxFiles = MAX;
			Assert.AreEqual( MAX, recentFiles.MaxFiles );
		}

		[Test]
		public void CountAtMin()
		{
			recentFiles.MaxFiles = MIN;
			Assert.AreEqual( MIN, recentFiles.MaxFiles );
		}

		[Test]
		public void EmptyList()
		{
			Assert.IsNotNull(  recentFiles.Entries, "Entries should never be null" );
			Assert.AreEqual( 0, recentFiles.Count );
			Assert.AreEqual( 0, recentFiles.Entries.Count );
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
			recentFiles.MaxFiles = 10;
			CheckAddItems( 10 );
		}

		[Test]
		public void ReduceSize()
		{
			recentFiles.MaxFiles = 3;
			CheckAddItems( 10 );
		}

		[Test]
		public void IncreaseSizeAfterAdd()
		{
			SetMockValues(5);
			recentFiles.MaxFiles = 7;
			recentFiles.SetMostRecent( "30" );
			recentFiles.SetMostRecent( "20" );
			recentFiles.SetMostRecent( "10" );
			CheckListContains( 10, 20, 30, 1, 2, 3, 4 );
		}

		[Test]
		public void ReduceSizeAfterAdd()
		{
			SetMockValues( 5 );
			recentFiles.MaxFiles = 3;
			CheckMockValues( 3 );
		}

		[Test]
		public void ReorderLastProject()
		{
			SetMockValues( 5 );
			recentFiles.SetMostRecent( "5" );
			CheckListContains( 5, 1, 2, 3, 4 );
		}

		[Test]
		public void ReorderSingleProject()
		{
			SetMockValues( 5 );
			recentFiles.SetMostRecent( "3" );
			CheckListContains( 3, 1, 2, 4, 5 );
		}

		[Test]
		public void ReorderMultipleProjects()
		{
			SetMockValues( 5 );
			recentFiles.SetMostRecent( "3" );
			recentFiles.SetMostRecent( "5" );
			recentFiles.SetMostRecent( "2" );
			CheckListContains( 2, 5, 3, 1, 4 );
		}

		[Test]
		public void ReorderSameProject()
		{
			SetMockValues( 5 );
			recentFiles.SetMostRecent( "1" );
			CheckListContains( 1, 2, 3, 4, 5 );
		}

		[Test]
		public void ReorderWithListNotFull()
		{
			SetMockValues( 3 );
			recentFiles.SetMostRecent( "3" );
			CheckListContains( 3, 1, 2 );
		}

		[Test]
		public void RemoveFirstProject()
		{
			SetMockValues( 3 );
			recentFiles.Remove("1");
			CheckListContains( 2, 3 );
		}

		[Test]
		public void RemoveOneProject()
		{
			SetMockValues( 4 );
			recentFiles.Remove("2");
			CheckListContains( 1, 3, 4 );
		}

		[Test]
		public void RemoveMultipleProjects()
		{
			SetMockValues( 5 );
			recentFiles.Remove( "3" );
			recentFiles.Remove( "1" );
			recentFiles.Remove( "4" );
			CheckListContains( 2, 5 );
		}
		
		[Test]
		public void RemoveLastProject()
		{
			SetMockValues( 5 );
			recentFiles.Remove("5");
			CheckListContains( 1, 2, 3, 4 );
		}
	}
}
