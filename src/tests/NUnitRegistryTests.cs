using System;
using Microsoft.Win32;
using NUnit.Framework;
using NUnit.Util;

namespace NUnit.Tests
{
	/// <summary>
	/// Summary description for NUnitRegistryTests.
	/// </summary>
	[TestFixture]
	public class NUnitRegistryTests
	{
		public NUnitRegistryTests()
		{
		}

		[Test]
		public void CurrentUser()
		{
			using( RegistryKey key = NUnitRegistry.CurrentUser )
			{
				Assertion.AssertNotNull( key );
				Assertion.AssertEquals( @"HKEY_CURRENT_USER\Software\Nascent Software\Nunit", key.Name );
			}
		}

		[Test]
		public void LocalMachine()
		{
			using( RegistryKey key = NUnitRegistry.LocalMachine )
			{
				Assertion.AssertNotNull( key );
				Assertion.AssertEquals( @"HKEY_LOCAL_MACHINE\Software\Nascent Software\Nunit", key.Name );
			}
		}

		[Test]
		public void CurrentUserTestMode()
		{
			try
			{
				NUnitRegistry.TestMode = true;
				using( RegistryKey key = NUnitRegistry.CurrentUser )
				{
					Assertion.AssertNotNull( key );
					Assertion.AssertEquals( @"HKEY_CURRENT_USER\Software\Nascent Software\Nunit-Test", key.Name );
				}
			}
			finally
			{
				NUnitRegistry.TestMode = false;
			}
		}

		[Test]
		public void LocalMachineTestMode()
		{
			try
			{
				NUnitRegistry.TestMode = true;
				using( RegistryKey key = NUnitRegistry.LocalMachine )
				{
					Assertion.AssertNotNull( key );
					Assertion.AssertEquals( @"HKEY_LOCAL_MACHINE\Software\Nascent Software\Nunit-Test", key.Name );
				}
			}
			finally
			{
				NUnitRegistry.TestMode = false;
			}
		}

		[Test]
		public void TestClearRoutines()
		{
			try
			{
				NUnitRegistry.TestMode = true;
				using( RegistryKey key = NUnitRegistry.LocalMachine )
				{
					using( RegistryKey foo = key.CreateSubKey( "foo" ) )
					{
						using( RegistryKey bar = key.CreateSubKey( "bar" ) )
						{
							using( RegistryKey footoo = foo.CreateSubKey( "foo" ) )
							{
								key.SetValue("X", 5);
								key.SetValue("NAME", "Joe");
								foo.SetValue("Y", 17);
								bar.SetValue("NAME", "Jennifer");
								footoo.SetValue( "X", 5 );
								footoo.SetValue("NAME", "Charlie" );

								NUnitRegistry.ClearTestKeys();

								Assertion.AssertEquals( 0, key.ValueCount );
								Assertion.AssertEquals( 0, key.SubKeyCount );
							}
						}
					}
				}
			}
			finally
			{
				NUnitRegistry.TestMode = false;
			}
		}
	}
}
