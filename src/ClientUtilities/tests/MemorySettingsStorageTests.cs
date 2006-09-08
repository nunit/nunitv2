using System;
using NUnit.Framework;

namespace NUnit.Util.Tests
{
	[TestFixture]
	public class MemorySettingsStorageTests
	{
		MemorySettingsStorage storage;

		[SetUp]
		public void Init()
		{
			storage = new MemorySettingsStorage();
		}

		[TearDown]
		public void Cleanup()
		{
			storage.Dispose();
		}

		[Test]
		public void MakeStorage()
		{
			Assert.IsNotNull( storage );
		}

		[Test]
		public void SaveAndLoadSettings()
		{
			Assert.IsNull( storage.LoadSetting( "X" ), "X is not null" );
			Assert.IsNull( storage.LoadSetting( "NAME" ), "NAME is not null" );

			storage.SaveSetting("X", 5);
			storage.SaveSetting("NAME", "Charlie");

			Assert.AreEqual( 5, storage.LoadSetting("X") );
			Assert.AreEqual( "Charlie", storage.LoadSetting("NAME") );
		}

		[Test]
		public void RemoveSettings()
		{
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
			ISettingsStorage sub1 = storage.MakeChildStorage( "Sub1" );
			ISettingsStorage sub2 = storage.MakeChildStorage( "Sub2" );

			Assert.IsNotNull( sub1, "Sub1 is null" );
			Assert.IsNotNull( sub2, "Sub2 is null" );
		}

		[Test]
		public void SubstorageSettings()
		{
			ISettingsStorage sub = storage.MakeChildStorage( "Sub" );

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
			storage.SaveSetting( "X", "1y25" );
			storage.LoadIntSetting( "X" );
		}

		[Test, ExpectedException( typeof( FormatException ) )]
		public void BadSetting2()
		{
			storage.SaveSetting( "X", "1y25" );
			storage.LoadIntSetting( "X", 12 );
		}
	}
}
