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

			Assert.NotNull( storage );
			Assert.Equals( "Test", storage.StorageName );
			Assert.Null( storage.ParentStorage );
			Assert.NotNull( "Null storage key", storage.StorageKey );
		}

		[Test]
		public void SaveAndLoadSettings()
		{
			RegistrySettingsStorage storage = new RegistrySettingsStorage( "Test", testKey );
			
			Assert.Null( "X is not null", storage.LoadSetting( "X" ) );
			Assert.Null( "NAME is not null", storage.LoadSetting( "NAME" ) );

			storage.SaveSetting("X", 5);
			storage.SaveSetting("NAME", "Charlie");

			Assert.Equals( 5, storage.LoadSetting("X") );
			Assert.Equals( "Charlie", storage.LoadSetting("NAME") );

			using( RegistryKey key = testKey.OpenSubKey( "Test" ) )
			{
				Assert.NotNull( key );
				Assert.Equals( 5, key.GetValue( "X" ) );
				Assert.Equals( "Charlie", key.GetValue( "NAME" ) );
			}
		}

		[Test]
		public void RemoveSettings()
		{
			RegistrySettingsStorage storage = new RegistrySettingsStorage( "Test", testKey );
			
			storage.SaveSetting("X", 5);
			storage.SaveSetting("NAME", "Charlie");

			storage.RemoveSetting( "X" );
			Assert.Null( "X not removed", storage.LoadSetting( "X" ) );
			Assert.Equals( "Charlie", storage.LoadSetting( "NAME" ) );

			storage.RemoveSetting( "NAME" );
			Assert.Null( "NAME not removed", storage.LoadSetting( "NAME" ) );
		}

		[Test]
		public void MakeSubStorages()
		{
			RegistrySettingsStorage storage = new RegistrySettingsStorage( "Test", testKey );
			RegistrySettingsStorage sub1 = new RegistrySettingsStorage( "Sub1", storage );
			RegistrySettingsStorage sub2 = new RegistrySettingsStorage( "Sub2", storage );

			Assert.NotNull( "Sub1 is null", sub1 );
			Assert.NotNull( "Sub2 is null", sub2 );

			Assert.Equals( "Sub1", sub1.StorageName );
			Assert.Equals( "Sub2", sub2.StorageName );
		}

		[Test]
		public void SubstorageSettings()
		{
			RegistrySettingsStorage storage = new RegistrySettingsStorage( "Test", testKey );
			RegistrySettingsStorage sub = new RegistrySettingsStorage( "Sub", storage );

			sub.SaveSetting( "X", 5 );
			sub.SaveSetting( "NAME", "Charlie" );

			Assert.Equals( 5, sub.LoadSetting( "X" ) );
			Assert.Equals( "Charlie", sub.LoadSetting( "NAME" ) );

			sub.RemoveSetting( "X" );
			Assert.Null( "X not removed", sub.LoadSetting( "X" ) );
			
			Assert.Equals( "Charlie", sub.LoadSetting( "NAME" ) );

			sub.RemoveSetting( "NAME" );
			Assert.Null( "NAME not removed", sub.LoadSetting( "NAME" ) );
		}

		[Test]
		public void TypeSafeSettings()
		{
			RegistrySettingsStorage storage = new RegistrySettingsStorage( "Test", testKey );
			
			storage.SaveSetting( "X", 5);
			storage.SaveSetting( "Y", "17" );
			storage.SaveSetting( "NAME", "Charlie");

			Assert.Equals( 5, storage.LoadSetting("X") );
			Assert.Equals( 5, storage.LoadIntSetting( "X" ) );
			Assert.Equals( "5", storage.LoadStringSetting( "X" ) );

			Assert.Equals( "17", storage.LoadSetting( "Y" ) );
			Assert.Equals( 17, storage.LoadIntSetting( "Y" ) );
			Assert.Equals( "17", storage.LoadStringSetting( "Y" ) );

			Assert.Equals( "Charlie", storage.LoadSetting( "NAME" ) );
			Assert.Equals( "Charlie", storage.LoadStringSetting( "NAME" ) );
		}

		[Test]
		public void DefaultSettings()
		{
			RegistrySettingsStorage storage = new RegistrySettingsStorage( "Test", testKey );
			
			Assert.Null( storage.LoadSetting( "X" ) );
			Assert.Null( storage.LoadSetting( "NAME" ) );

			Assert.Equals( 5, storage.LoadSetting( "X", 5 ) );
			Assert.Equals( 6, storage.LoadIntSetting( "X", 6 ) );
			Assert.Equals( "7", storage.LoadStringSetting( "X", "7" ) );
			
			Assert.Equals( "Charlie", storage.LoadSetting( "NAME", "Charlie" ) );
			Assert.Equals( "Fred", storage.LoadStringSetting( "NAME", "Fred" ) );
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
