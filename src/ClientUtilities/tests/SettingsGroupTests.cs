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
using NUnit.Framework;
using Microsoft.Win32;

namespace NUnit.Util.Tests
{
	[TestFixture]
	public class SettingsGroupTests
	{
		private SettingsGroup testGroup;

		[SetUp]
		public void BeforeEachTest()
		{
			MemorySettingsStorage storage = new MemorySettingsStorage();
			testGroup = new SettingsGroup( storage );
		}

		[TearDown]
		public void AfterEachTest()
		{
			testGroup.Dispose();
		}

		[Test]
		public void TopLevelSettings()
		{
			testGroup.SaveSetting( "X", 5 );
			testGroup.SaveSetting( "NAME", "Charlie" );
			Assert.AreEqual( 5, testGroup.GetSetting( "X" ) );
			Assert.AreEqual( "Charlie", testGroup.GetSetting( "NAME" ) );

			testGroup.RemoveSetting( "X" );
			Assert.IsNull( testGroup.GetSetting( "X" ), "X not removed" );
			Assert.AreEqual( "Charlie", testGroup.GetSetting( "NAME" ) );

			testGroup.RemoveSetting( "NAME" );
			Assert.IsNull( testGroup.GetSetting( "NAME" ), "NAME not removed" );
		}

		[Test]
		public void SubGroupSettings()
		{
			SettingsGroup subGroup = new SettingsGroup( testGroup.Storage );
			Assert.IsNotNull( subGroup );
			Assert.IsNotNull( subGroup.Storage );

			subGroup.SaveSetting( "X", 5 );
			subGroup.SaveSetting( "NAME", "Charlie" );
			Assert.AreEqual( 5, subGroup.GetSetting( "X" ) );
			Assert.AreEqual( "Charlie", subGroup.GetSetting( "NAME" ) );

			subGroup.RemoveSetting( "X" );
			Assert.IsNull( subGroup.GetSetting( "X" ), "X not removed" );
			Assert.AreEqual( "Charlie", subGroup.GetSetting( "NAME" ) );

			subGroup.RemoveSetting( "NAME" );
			Assert.IsNull( subGroup.GetSetting( "NAME" ), "NAME not removed" );
		}

		[Test]
		public void TypeSafeSettings()
		{
			testGroup.SaveSetting( "X", 5);
			testGroup.SaveSetting( "Y", "17" );
			testGroup.SaveSetting( "NAME", "Charlie");

			Assert.AreEqual( 5, testGroup.GetSetting("X") );
			Assert.AreEqual( "17", testGroup.GetSetting( "Y" ) );
			Assert.AreEqual( "Charlie", testGroup.GetSetting( "NAME" ) );
		}

		[Test]
		public void DefaultSettings()
		{
			Assert.IsNull( testGroup.GetSetting( "X" ) );
			Assert.IsNull( testGroup.GetSetting( "NAME" ) );

			Assert.AreEqual( 5, testGroup.GetSetting( "X", 5 ) );
			Assert.AreEqual( 6, testGroup.GetSetting( "X", 6 ) );
			Assert.AreEqual( "7", testGroup.GetSetting( "X", "7" ) );

			Assert.AreEqual( "Charlie", testGroup.GetSetting( "NAME", "Charlie" ) );
			Assert.AreEqual( "Fred", testGroup.GetSetting( "NAME", "Fred" ) );
		}

		[Test, ExpectedException( typeof( FormatException ) )]
		public void BadSetting()
		{
			testGroup.SaveSetting( "X", "1y25" );
			testGroup.GetSetting( "X", 12 );
		}
	}
}
