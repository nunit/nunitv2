using System;
using NUnit.Util;
using NUnit.Framework;
using Microsoft.Win32;

namespace NUnit.Tests
{
	/// <summary>
	/// Summary description for SettingsGroupTests.
	/// </summary>
	[TestFixture]
	public class SettingsGroupTests
	{
		private RegistryKey testKey;

		public SettingsGroupTests()
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
		public void TopLevelSettings()
		{
			RegistrySettingsStorage storage = new RegistrySettingsStorage( "Test", testKey );
			SettingsGroup testGroup = new SettingsGroup( "TestGroup", storage );
			Assertion.AssertNotNull( testGroup );
			Assertion.AssertEquals( "TestGroup", testGroup.Name );
			Assertion.AssertEquals( storage, testGroup.Storage );
			
			testGroup.SaveSetting( "X", 5 );
			testGroup.SaveSetting( "NAME", "Charlie" );
			Assertion.AssertEquals( 5, testGroup.LoadSetting( "X" ) );
			Assertion.AssertEquals( "Charlie", testGroup.LoadSetting( "NAME" ) );

			testGroup.RemoveSetting( "X" );
			Assertion.AssertNull( "X not removed", testGroup.LoadSetting( "X" ) );
			Assertion.AssertEquals( "Charlie", testGroup.LoadSetting( "NAME" ) );

			testGroup.RemoveSetting( "NAME" );
			Assertion.AssertNull( "NAME not removed", testGroup.LoadSetting( "NAME" ) );
		}

		[Test]
		public void SubGroupSettings()
		{
			RegistrySettingsStorage storage = new RegistrySettingsStorage( "Test", testKey );
			SettingsGroup testGroup = new SettingsGroup( "TestGroup", storage );
			SettingsGroup subGroup = new SettingsGroup( "SubGroup", testGroup );
			Assertion.AssertNotNull( subGroup );
			Assertion.AssertEquals( "SubGroup", subGroup.Name );
			Assertion.AssertNotNull( subGroup.Storage );
			Assertion.AssertEquals( storage, subGroup.Storage.ParentStorage );

			subGroup.SaveSetting( "X", 5 );
			subGroup.SaveSetting( "NAME", "Charlie" );
			Assertion.AssertEquals( 5, subGroup.LoadSetting( "X" ) );
			Assertion.AssertEquals( "Charlie", subGroup.LoadSetting( "NAME" ) );

			subGroup.RemoveSetting( "X" );
			Assertion.AssertNull( "X not removed", subGroup.LoadSetting( "X" ) );
			Assertion.AssertEquals( "Charlie", subGroup.LoadSetting( "NAME" ) );

			subGroup.RemoveSetting( "NAME" );
			Assertion.AssertNull( "NAME not removed", subGroup.LoadSetting( "NAME" ) );
		}

		[Test]
		public void DefaultSettings()
		{
			RegistrySettingsStorage storage = new RegistrySettingsStorage( "Test", testKey );
			SettingsGroup testGroup = new SettingsGroup( "TestGroup", storage );
			
			Assertion.AssertNull( testGroup.LoadSetting( "X" ) );
			Assertion.AssertNull( testGroup.LoadSetting( "NAME" ) );

			Assertion.AssertEquals( 5, testGroup.LoadSetting( "X", 5 ) );
			Assertion.AssertEquals( "Charlie", testGroup.LoadSetting( "NAME", "Charlie" ) );
		}
	}
}
