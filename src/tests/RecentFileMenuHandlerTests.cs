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
using System.Windows.Forms;
using NUnit.Framework;
using NUnit.Util;
using NUnit.UiKit;

namespace NUnit.Tests.UiKit
{
	[TestFixture]
	public class RecentFileMenuHandlerTests
	{
		private MenuItem menu;
		private RecentProjectSettings projects;
		private RecentFileMenuHandler handler;
		
		[SetUp]
		public void SetUp()
		{
			NUnitRegistry.TestMode = true;
			NUnitRegistry.ClearTestKeys();

			menu = new MenuItem();
			projects = UserSettings.RecentProjects;
			handler = new RecentFileMenuHandler( menu, projects );
		}

		[TearDown]
		public void TearDown()
		{
			NUnitRegistry.TestMode = false;
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
			projects.RecentFile = "Test";
			handler.Load();
			Assert.IsTrue( menu.Enabled );
		}
		[Test]
		public void LoadMenuItems()
		{
			projects.RecentFile = "Third";
			projects.RecentFile = "Second";
			projects.RecentFile = "First";
			handler.Load();
			Assert.AreEqual( 3, menu.MenuItems.Count );
			Assert.AreEqual( "1 First", menu.MenuItems[0].Text );
		}

		
		// TODO: Need mock loader to test clicking
	}
}
