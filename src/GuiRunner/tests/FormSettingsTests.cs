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
using System.Windows.Forms;
using NUnit.Framework;
using NUnit.Util;
using Microsoft.Win32;

namespace NUnit.Gui.Tests
{
	[TestFixture]
	public class FormSettingsTests
	{
		static Point DEFAULT_LOCATION = new Point( FormSettings.DEFAULT_XLOCATION, FormSettings.DEFAULT_YLOCATION );
		static Size DEFAULT_SIZE = new Size( FormSettings.DEFAULT_WIDTH, FormSettings.DEFAULT_HEIGHT );
		static Size MIN_SIZE = new Size( FormSettings.MIN_WIDTH, FormSettings.MIN_HEIGHT );
		static int TREE_MIN_POSITION = FormSettings.TREE_MIN_POSITION;
		static int TAB_MIN_POSITION = FormSettings.TAB_MIN_POSITION;
		
		FormSettings formSettings;

		[SetUp]
		public void Init()
		{
			MemorySettingsStorage storage = new MemorySettingsStorage();
			formSettings = new UserSettings( storage ).Form;
		}

		[TearDown]
		public void Cleanup()
		{
			formSettings.Dispose();
		}

		[Test]
		public void FormPosition()
		{
			Point pt = new Point( 100, 200 );
			Size sz = new Size( 300, 200 );
			
			formSettings.Location = pt;
			formSettings.Size = sz;

			Assert.AreEqual( pt, formSettings.Location );
			Assert.AreEqual( sz, formSettings.Size );
		}

		[Test]
		public void SplitterPosition()
		{
			int position = 383;

			formSettings.TreeSplitterPosition = position; 
			Assert.AreEqual(position, formSettings.TreeSplitterPosition);

			formSettings.TabSplitterPosition = position;
			Assert.AreEqual(position, formSettings.TabSplitterPosition);
		}

		[Test]
		public void FormPositionDefaults()
		{	
			Point pt = formSettings.Location;
			Size sz = formSettings.Size;

			Assert.AreEqual( DEFAULT_LOCATION, pt );
			Assert.AreEqual( DEFAULT_SIZE, sz );
		}

		[Test]
		public void FormSizeTooSmall()
		{
			Point pt = new Point( 100, 200 );
			Size sz = new Size( 20, 25 );
			
			formSettings.Location = pt;
			formSettings.Size = sz;

			Assert.AreEqual( pt, formSettings.Location );
			Assert.AreEqual( MIN_SIZE, formSettings.Size );
		}

		[Test]
		public void PositionOutOfBounds()
		{
			int minX = 0;
			foreach( Screen screen in Screen.AllScreens )
				if ( screen.Bounds.Left < minX )
					minX = screen.Bounds.Left;

			Point pt = new Point( minX-1000, 200 );
			Size sz = new Size( 300, 200 );

			formSettings.Location = pt;
			formSettings.Size = sz;

			Assert.AreEqual( DEFAULT_LOCATION, formSettings.Location );
			Assert.AreEqual( sz, formSettings.Size );
		}

		[Test]
		public void BadTreeSplitterPosition()
		{
			formSettings.TreeSplitterPosition = 5;
			Assert.AreEqual( TREE_MIN_POSITION, formSettings.TreeSplitterPosition );
			formSettings.TreeSplitterPosition = 5000; 
			Assert.AreEqual( TREE_MIN_POSITION, formSettings.TreeSplitterPosition );
		}
	
		[Test]
		public void BadTabSplitterPosition()
		{
			formSettings.TabSplitterPosition = 5;
			Assert.AreEqual( TAB_MIN_POSITION, formSettings.TabSplitterPosition );

			formSettings.TabSplitterPosition = 5000;
			Assert.AreEqual( TAB_MIN_POSITION, formSettings.TabSplitterPosition );
		}
	}
}
