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
using System.Collections;
using System.Windows.Forms;
using NUnit.Framework;
using NUnit.Util;

namespace NUnit.UiKit.Tests
{
	[TestFixture]
	public class RecentFileMenuHandlerTests
	{
		private MenuItem menu;
		private RecentFiles files;
		private RecentFileMenuHandler handler;
		
		[SetUp]
		public void SetUp()
		{
			menu = new MenuItem();
			files = new FakeRecentFiles();
			handler = new RecentFileMenuHandler( menu, files );
            handler.ShowMissingFiles = true;
        }

		[Test]
		public void DisableOnLoadWhenEmpty()
		{
			handler.Load();
			Assert.IsFalse( menu.Enabled );
		}

		[Test]
		public void EnableOnLoadWhenNotEmpty()
		{
			files.SetMostRecent( "Test" );
			handler.Load();
			Assert.IsTrue( menu.Enabled );
		}
		[Test]
		public void LoadMenuItems()
		{
			files.SetMostRecent( "Third" );
			files.SetMostRecent( "Second" );
			files.SetMostRecent( "First" );
			handler.Load();
			Assert.AreEqual( 3, menu.MenuItems.Count );
			Assert.AreEqual( "1 First", menu.MenuItems[0].Text );
		}

		private class FakeRecentFiles : RecentFiles
		{
			private RecentFilesCollection files = new RecentFilesCollection();
			private int maxFiles = 24;

			public int Count
			{
				get { return files.Count; }
			}

			public int MaxFiles
			{
				get { return maxFiles; }
				set { maxFiles = value; }
			}

			public void SetMostRecent( string fileName )
			{
				SetMostRecent( new RecentFileEntry( fileName ) );
			}

			public void SetMostRecent( RecentFileEntry entry )
			{
				files.Insert( 0, entry );
			}

			public RecentFilesCollection Entries
			{
				get { return files; }
			}

			public void Clear()
			{
				files.Clear();
			}

			public void Remove( string fileName )
			{
				files.Remove( fileName );
			}
		}
	
		// TODO: Need mock loader to test clicking
	}
}
