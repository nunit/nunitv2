#region Copyright (c) 2002, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov
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
' Portions Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov 
' or Copyright © 2000-2002 Philip A. Craig
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

namespace NUnit.Tests
{
	using NUnit.Util;
	using NUnit.Framework;

	/// <summary>
	/// Summary description for RegistryStorageTests.
	/// </summary>
	[TestFixture]
	public class RegistrySettingsStorageTests
	{
		RegistryKey testKey;

		public RegistrySettingsStorageTests()
		{
		}

		[SetUp]
		public void BeforeEachTest()
		{
			testKey = Registry.CurrentUser.CreateSubKey( "Software\\NunitTest" );
		}

		[TearDown]
		public void AfterEachTest()
		{
			testKey.Close();
			Registry.CurrentUser.DeleteSubKeyTree( "Software\\NunitTest" );
		}

		[Test]
		public void MakeRegistryStorage()
		{
			RegistrySettingsStorage storage = new RegistrySettingsStorage( "Test", testKey );

			Assertion.AssertNotNull( storage );
			Assertion.AssertEquals( "Test", storage.StorageName );
			Assertion.AssertNull( storage.ParentStorage );
			Assertion.AssertNotNull( "Null storage key", storage.StorageKey );
		}

		[Test]
		public void SaveAndLoadSettings()
		{
			RegistrySettingsStorage storage = new RegistrySettingsStorage( "Test", testKey );
			
			Assertion.AssertNull( "X is not null", storage.LoadSetting( "X" ) );
			Assertion.AssertNull( "NAME is not null", storage.LoadSetting( "NAME" ) );

			storage.SaveSetting("X", 5);
			storage.SaveSetting("NAME", "Charlie");

			Assertion.AssertEquals( 5, storage.LoadSetting("X") );
			Assertion.AssertEquals( "Charlie", storage.LoadSetting("NAME") );

			using( RegistryKey key = testKey.OpenSubKey( "Test" ) )
			{
				Assertion.AssertNotNull( key );
				Assertion.AssertEquals( 5, key.GetValue( "X" ) );
				Assertion.AssertEquals( "Charlie", key.GetValue( "NAME" ) );
			}
		}

		[Test]
		public void RemoveSettings()
		{
			RegistrySettingsStorage storage = new RegistrySettingsStorage( "Test", testKey );
			
			storage.SaveSetting("X", 5);
			storage.SaveSetting("NAME", "Charlie");

			storage.RemoveSetting( "X" );
			Assertion.AssertNull( "X not removed", storage.LoadSetting( "X" ) );
			Assertion.AssertEquals( "Charlie", storage.LoadSetting( "NAME" ) );

			storage.RemoveSetting( "NAME" );
			Assertion.AssertNull( "NAME not removed", storage.LoadSetting( "NAME" ) );
		}

		[Test]
		public void MakeSubStorages()
		{
			RegistrySettingsStorage storage = new RegistrySettingsStorage( "Test", testKey );
			RegistrySettingsStorage sub1 = new RegistrySettingsStorage( "Sub1", storage );
			RegistrySettingsStorage sub2 = new RegistrySettingsStorage( "Sub2", storage );

			Assertion.AssertNotNull( "Sub1 is null", sub1 );
			Assertion.AssertNotNull( "Sub2 is null", sub2 );

			Assertion.AssertEquals( "Sub1", sub1.StorageName );
			Assertion.AssertEquals( "Sub2", sub2.StorageName );
		}

		[Test]
		public void SubstorageSettings()
		{
			RegistrySettingsStorage storage = new RegistrySettingsStorage( "Test", testKey );
			RegistrySettingsStorage sub = new RegistrySettingsStorage( "Sub", storage );

			sub.SaveSetting( "X", 5 );
			sub.SaveSetting( "NAME", "Charlie" );

			Assertion.AssertEquals( 5, sub.LoadSetting( "X" ) );
			Assertion.AssertEquals( "Charlie", sub.LoadSetting( "NAME" ) );

			sub.RemoveSetting( "X" );
			Assertion.AssertNull( "X not removed", sub.LoadSetting( "X" ) );
			
			Assertion.AssertEquals( "Charlie", sub.LoadSetting( "NAME" ) );

			sub.RemoveSetting( "NAME" );
			Assertion.AssertNull( "NAME not removed", sub.LoadSetting( "NAME" ) );
		}

		[Test]
		public void TypeSafeSettings()
		{
			RegistrySettingsStorage storage = new RegistrySettingsStorage( "Test", testKey );
			
			storage.SaveSetting( "X", 5);
			storage.SaveSetting( "Y", "17" );
			storage.SaveSetting( "NAME", "Charlie");

			Assertion.AssertEquals( 5, storage.LoadSetting("X") );
			Assertion.AssertEquals( 5, storage.LoadIntSetting( "X" ) );
			Assertion.AssertEquals( "5", storage.LoadStringSetting( "X" ) );

			Assertion.AssertEquals( "17", storage.LoadSetting( "Y" ) );
			Assertion.AssertEquals( 17, storage.LoadIntSetting( "Y" ) );
			Assertion.AssertEquals( "17", storage.LoadStringSetting( "Y" ) );

			Assertion.AssertEquals( "Charlie", storage.LoadSetting( "NAME" ) );
			Assertion.AssertEquals( "Charlie", storage.LoadStringSetting( "NAME" ) );
		}

		[Test]
		public void DefaultSettings()
		{
			RegistrySettingsStorage storage = new RegistrySettingsStorage( "Test", testKey );
			
			Assertion.AssertNull( storage.LoadSetting( "X" ) );
			Assertion.AssertNull( storage.LoadSetting( "NAME" ) );

			Assertion.AssertEquals( 5, storage.LoadSetting( "X", 5 ) );
			Assertion.AssertEquals( 6, storage.LoadIntSetting( "X", 6 ) );
			Assertion.AssertEquals( "7", storage.LoadStringSetting( "X", "7" ) );
			
			Assertion.AssertEquals( "Charlie", storage.LoadSetting( "NAME", "Charlie" ) );
			Assertion.AssertEquals( "Fred", storage.LoadStringSetting( "NAME", "Fred" ) );
		}

		[Test, ExpectedException( typeof( FormatException ) )]
		public void BadSetting1()
		{
			RegistrySettingsStorage storage = new RegistrySettingsStorage( "Test", testKey );
			storage.SaveSetting( "X", "1y25" );

			int x = storage.LoadIntSetting( "X" );
		}

		[Test, ExpectedException( typeof( FormatException ) )]
		public void BadSetting2()
		{
			RegistrySettingsStorage storage = new RegistrySettingsStorage( "Test", testKey );
			storage.SaveSetting( "X", "1y25" );

			int x = storage.LoadIntSetting( "X", 12 );
		}
	}
}
