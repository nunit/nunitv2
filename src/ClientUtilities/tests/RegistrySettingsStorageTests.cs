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
using Microsoft.Win32;
using NUnit.Framework;

namespace NUnit.Util.Tests
{
	[TestFixture]
	public class RegistrySettingsStorageTests
	{
		private static readonly string testKeyName = "Software\\NUnitTest";

		RegistryKey testKey;
		RegistrySettingsStorage storage;

		[SetUp]
		public void BeforeEachTest()
		{
			testKey = Registry.CurrentUser.CreateSubKey( testKeyName );
			storage = new RegistrySettingsStorage( testKey );
		}

		[TearDown]
		public void AfterEachTest()
		{
			NUnitRegistry.ClearKey( testKey );
			storage.Dispose();
		}

		[Test]
		public void StorageHasCorrectKey()
		{
			StringAssert.AreEqualIgnoringCase( "HKEY_CURRENT_USER\\" + testKeyName, storage.StorageKey.Name );
		}

		[Test]
		public void SaveAndLoadSettings()
		{
			Assert.IsNull( storage.GetSetting( "X" ), "X is not null" );
			Assert.IsNull( storage.GetSetting( "NAME" ), "NAME is not null" );

			storage.SaveSetting("X", 5);
			storage.SaveSetting("NAME", "Charlie");

			Assert.AreEqual( 5, storage.GetSetting("X") );
			Assert.AreEqual( "Charlie", storage.GetSetting("NAME") );

			Assert.AreEqual( 5, testKey.GetValue( "X" ) );
			Assert.AreEqual( "Charlie", testKey.GetValue( "NAME" ) );
		}

		[Test]
		public void RemoveSettings()
		{
			storage.SaveSetting("X", 5);
			storage.SaveSetting("NAME", "Charlie");

			storage.RemoveSetting( "X" );
			Assert.IsNull( storage.GetSetting( "X" ), "X not removed" );
			Assert.AreEqual( "Charlie", storage.GetSetting( "NAME" ) );

			storage.RemoveSetting( "NAME" );
			Assert.IsNull( storage.GetSetting( "NAME" ), "NAME not removed" );
		}

		[Test]
		public void MakeSubStorages()
		{
			RegistrySettingsStorage sub1 = (RegistrySettingsStorage)storage.MakeChildStorage( "Sub1" );
			RegistrySettingsStorage sub2 = (RegistrySettingsStorage)storage.MakeChildStorage( "Sub2" );

			Assert.IsNotNull( sub1, "Sub1 is null" );
			Assert.IsNotNull( sub2, "Sub2 is null" );

			StringAssert.AreEqualIgnoringCase( "HKEY_CURRENT_USER\\" + testKeyName + "\\Sub1", sub1.StorageKey.Name);
			StringAssert.AreEqualIgnoringCase( "HKEY_CURRENT_USER\\" + testKeyName + "\\Sub2", sub2.StorageKey.Name );
		}

		[Test]
		public void SubstorageSettings()
		{
			ISettingsStorage sub = storage.MakeChildStorage( "Sub" );

			sub.SaveSetting( "X", 5 );
			sub.SaveSetting( "NAME", "Charlie" );

			Assert.AreEqual( 5, sub.GetSetting( "X" ) );
			Assert.AreEqual( "Charlie", sub.GetSetting( "NAME" ) );

			sub.RemoveSetting( "X" );
			Assert.IsNull( sub.GetSetting( "X" ), "X not removed" );
			
			Assert.AreEqual( "Charlie", sub.GetSetting( "NAME" ) );

			sub.RemoveSetting( "NAME" );
			Assert.IsNull( sub.GetSetting( "NAME" ), "NAME not removed" );
		}

		[Test]
		public void TypeSafeSettings()
		{
			storage.SaveSetting( "X", 5);
			storage.SaveSetting( "Y", "17" );
			storage.SaveSetting( "NAME", "Charlie");

			Assert.AreEqual( 5, storage.GetSetting("X") );
			Assert.AreEqual( "17", storage.GetSetting( "Y" ) );
			Assert.AreEqual( "Charlie", storage.GetSetting( "NAME" ) );
		}
	}
}
