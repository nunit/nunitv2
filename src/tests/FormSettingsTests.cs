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
using System.Drawing;
using NUnit.Util;
using NUnit.Framework;
using Microsoft.Win32;

namespace NUnit.Tests.Util
{
	[TestFixture]
	public class FormSettingsTests
	{
		static Point DEFAULT_LOCATION = new Point( FormSettings.DEFAULT_XLOCATION, FormSettings.DEFAULT_YLOCATION );
		static Size DEFAULT_SIZE = new Size( FormSettings.DEFAULT_WIDTH, FormSettings.DEFAULT_HEIGHT );
		static Size MIN_SIZE = new Size( FormSettings.MIN_WIDTH, FormSettings.MIN_HEIGHT );
		static int TREE_MIN_POSITION = FormSettings.TREE_MIN_POSITION;
		static int TAB_MIN_POSITION = FormSettings.TAB_MIN_POSITION;

		[SetUp]
		public void Init()
		{
			NUnitRegistry.TestMode = true;
			NUnitRegistry.ClearTestKeys();
		}

		[TearDown]
		public void Cleanup()
		{
			NUnitRegistry.TestMode = false;
		}

		[Test]
		public void StorageName()
		{
			Assert.AreEqual( @"Form", UserSettings.Form.Storage.StorageName );
		}

		[Test]
		public void StorageKey()
		{
			Assert.AreEqual( @"HKEY_CURRENT_USER\Software\Nascent Software\Nunit-Test\Form", 
				((RegistrySettingsStorage)UserSettings.Form.Storage).StorageKey.Name );
		}

		[Test]
		public void FormPosition()
		{
			Point pt = new Point( 100, 200 );
			Size sz = new Size( 300, 200 );
			
			UserSettings.Form.Location = pt;
			UserSettings.Form.Size = sz;

			Assert.AreEqual( pt, UserSettings.Form.Location );
			Assert.AreEqual( sz, UserSettings.Form.Size );
		}

		[Test]
		public void SplitterPosition()
		{
			int position = 383;

			UserSettings.Form.TreeSplitterPosition = position; 
			Assert.AreEqual(position, UserSettings.Form.TreeSplitterPosition);

			UserSettings.Form.TabSplitterPosition = position;
			Assert.AreEqual(position, UserSettings.Form.TabSplitterPosition);
		}

		[Test]
		public void FormPositionDefaults()
		{	
			FormSettings f = UserSettings.Form;
			Point pt = f.Location;
			Size sz = f.Size;

			Assert.AreEqual( DEFAULT_LOCATION, pt );
			Assert.AreEqual( DEFAULT_SIZE, sz );
		}

		[Test]
		public void FormSizeTooSmall()
		{
			Point pt = new Point( 100, 200 );
			Size sz = new Size( 20, 25 );
			
			UserSettings.Form.Location = pt;
			UserSettings.Form.Size = sz;

			Assert.AreEqual( pt, UserSettings.Form.Location );
			Assert.AreEqual( MIN_SIZE, UserSettings.Form.Size );
		}

		[Test]
		public void PositionOutOfBounds()
		{
			Point pt = new Point( -1000, 200 );
			Size sz = new Size( 300, 200 );
			
			UserSettings.Form.Location = pt;
			UserSettings.Form.Size = sz;

			Assert.AreEqual( DEFAULT_LOCATION, UserSettings.Form.Location );
			Assert.AreEqual( sz, UserSettings.Form.Size );
		}

		[Test]
		public void BadTreeSplitterPosition()
		{
			UserSettings.Form.TreeSplitterPosition = 5;
			Assert.AreEqual( TREE_MIN_POSITION, UserSettings.Form.TreeSplitterPosition );
			UserSettings.Form.TreeSplitterPosition = 5000; 
			Assert.AreEqual( TREE_MIN_POSITION, UserSettings.Form.TreeSplitterPosition );
		}
	
		[Test]
		public void BadTabSplitterPosition()
		{
			UserSettings.Form.TabSplitterPosition = 5;
			Assert.AreEqual( TAB_MIN_POSITION, UserSettings.Form.TabSplitterPosition );

			UserSettings.Form.TabSplitterPosition = 5000;
			Assert.AreEqual( TAB_MIN_POSITION, UserSettings.Form.TabSplitterPosition );
		}
	}
}
