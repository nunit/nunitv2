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

namespace NUnit.Tests.Util
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

			Assert.IsNotNull( storage );
			Assert.AreEqual( "Test", storage.StorageName );
			Assert.IsNull( storage.ParentStorage );
			Assert.IsNotNull( storage.StorageKey, "Null storage key" );
		}

		[Test]
		public void SaveAndLoadSettings()
		{
			RegistrySettingsStorage storage = new RegistrySettingsStorage( "Test", testKey );
			
			Assert.IsNull( storage.LoadSetting( "X" ), "X is not null" );
			Assert.IsNull( storage.LoadSetting( "NAME" ), "NAME is not null" );

			storage.SaveSetting("X", 5);
			storage.SaveSetting("NAME", "Charlie");

			Assert.AreEqual( 5, storage.LoadSetting("X") );
			Assert.AreEqual( "Charlie", storage.LoadSetting("NAME") );

			using( RegistryKey key = testKey.OpenSubKey( "Test" ) )
			{
				Assert.IsNotNull( key );
				Assert.AreEqual( 5, key.GetValue( "X" ) );
				Assert.AreEqual( "Charlie", key.GetValue( "NAME" ) );
			}
		}

		[Test]
		public void RemoveSettings()
		{
			RegistrySettingsStorage storage = new RegistrySettingsStorage( "Test", testKey );
			
			storage.SaveSetting("X", 5);
			storage.SaveSetting("NAME", "Charlie");

			storage.RemoveSetting( "X" );
			Assert.IsNull( storage.LoadSetting( "X" ), "X not removed" );
			Assert.AreEqual( "Charlie", storage.LoadSetting( "NAME" ) );

			storage.RemoveSetting( "NAME" );
			Assert.IsNull( storage.LoadSetting( "NAME" ), "NAME not removed" );
		}

		[Test]
		public void MakeSubStorages()
		{
			RegistrySettingsStorage storage = new RegistrySettingsStorage( "Test", testKey );
			RegistrySettingsStorage sub1 = new RegistrySettingsStorage( "Sub1", storage );
			RegistrySettingsStorage sub2 = new RegistrySettingsStorage( "Sub2", storage );

			Assert.IsNotNull( sub1, "Sub1 is null" );
			Assert.IsNotNull( sub2, "Sub2 is null" );

			Assert.AreEqual( sub1.StorageName, "Sub1" );
			Assert.AreEqual( sub2.StorageName, "Sub2" );
		}

		[Test]
		public void SubstorageSettings()
		{
			RegistrySettingsStorage storage = new RegistrySettingsStorage( "Test", testKey );
			RegistrySettingsStorage sub = new RegistrySettingsStorage( "Sub", storage );

			sub.SaveSetting( "X", 5 );
			sub.SaveSetting( "NAME", "Charlie" );

			Assert.AreEqual( 5, sub.LoadSetting( "X" ) );
			Assert.AreEqual( "Charlie", sub.LoadSetting( "NAME" ) );

			sub.RemoveSetting( "X" );
			Assert.IsNull( sub.LoadSetting( "X" ), "X not removed" );
			
			Assert.AreEqual( "Charlie", sub.LoadSetting( "NAME" ) );

			sub.RemoveSetting( "NAME" );
			Assert.IsNull( sub.LoadSetting( "NAME" ), "NAME not removed" );
		}

		[Test]
		public void TypeSafeSettings()
		{
			RegistrySettingsStorage storage = new RegistrySettingsStorage( "Test", testKey );
			
			storage.SaveSetting( "X", 5);
			storage.SaveSetting( "Y", "17" );
			storage.SaveSetting( "NAME", "Charlie");

			Assert.AreEqual( 5, storage.LoadSetting("X") );
			Assert.AreEqual( 5, storage.LoadIntSetting( "X" ) );
			Assert.AreEqual( "5", storage.LoadStringSetting( "X" ) );

			Assert.AreEqual( "17", storage.LoadSetting( "Y" ) );
			Assert.AreEqual( 17, storage.LoadIntSetting( "Y" ) );
			Assert.AreEqual( "17", storage.LoadStringSetting( "Y" ) );

			Assert.AreEqual( "Charlie", storage.LoadSetting( "NAME" ) );
			Assert.AreEqual( "Charlie", storage.LoadStringSetting( "NAME" ) );
		}

		[Test]
		public void DefaultSettings()
		{
			RegistrySettingsStorage storage = new RegistrySettingsStorage( "Test", testKey );
			
			Assert.IsNull( storage.LoadSetting( "X" ) );
			Assert.IsNull( storage.LoadSetting( "NAME" ) );

			Assert.AreEqual( 5, storage.LoadSetting( "X", 5 ) );
			Assert.AreEqual( 6, storage.LoadIntSetting( "X", 6 ) );
			Assert.AreEqual( "7", storage.LoadStringSetting( "X", "7" ) );
			
			Assert.AreEqual( "Charlie", storage.LoadSetting( "NAME", "Charlie" ) );
			Assert.AreEqual( "Fred", storage.LoadStringSetting( "NAME", "Fred" ) );
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
