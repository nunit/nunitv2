using System;
using System.Drawing;
using NUnit.Util;
using NUnit.Framework;
using Microsoft.Win32;

namespace NUnit.Tests
{
	/// <summary>
	/// Summary description for UserSettingsTests.
	/// </summary>
	[TestFixture]
	public class UserSettingsTests
	{
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
		public void FormPosition()
		{
			UserSettings.Form.Location = new Point( 100, 200 );
			UserSettings.Form.Size = new Size( 20, 25 );

			Assertion.AssertEquals( 100, UserSettings.Form.Location.X );
			Assertion.AssertEquals( 200, UserSettings.Form.Location.Y );
			Assertion.AssertEquals( 20, UserSettings.Form.Size.Width );
			Assertion.AssertEquals( 25, UserSettings.Form.Size.Height );	
		}

		public void FormPositionDefaults()
		{	
			FormSettings f = UserSettings.Form;
			Point pt = f.Location;
			Size sz = f.Size;

			Assertion.AssertEquals( 10, pt.X );
			Assertion.AssertEquals( 10, pt.Y );
			Assertion.AssertEquals( 632, sz.Width );
			Assertion.AssertEquals( 432, sz.Height );	
		}

		[Test]
		public void RecentAssemblyBasicTests()
		{
			RecentAssemblySettings ras = UserSettings.RecentAssemblies;
			Assertion.AssertEquals( @"Recent-Assemblies", ras.Storage.StorageName );
			Assertion.AssertEquals( @"HKEY_CURRENT_USER\Software\Nascent Software\Nunit-Test\Recent-Assemblies", 
				((RegistrySettingsStorage)ras.Storage).StorageKey.Name );
			Assertion.AssertNotNull( "GetAssemblies() returned null", ras.GetAssemblies() );
			Assertion.AssertEquals( 0, ras.GetAssemblies().Count );
			Assertion.AssertNull( "No RecentAssembly should return null", ras.RecentAssembly );

			ras.RecentAssembly = "one";
			ras.RecentAssembly = "two";
			Assertion.AssertEquals( 2, ras.GetAssemblies().Count );
			Assertion.AssertEquals( "two", ras.RecentAssembly );

			using( RegistryKey key = NUnitRegistry.CurrentUser.OpenSubKey( "Recent-Assemblies" ) )
			{
				Assertion.AssertEquals( 2, key.ValueCount );
				Assertion.AssertEquals( "two", key.GetValue( "RecentAssembly1" ) );
				Assertion.AssertEquals( "one", key.GetValue( "RecentAssembly2" ) );
			}
		}
	}
}
