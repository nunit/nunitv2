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
		public void CurrentUserTest()
		{
			RegistryKey key = NUnitRegistry.CurrentUser;
			
			Assertion.AssertNotNull( key );
			Assertion.AssertEquals( @"HKEY_CURRENT_USER\Software\Nascent Software\Nunit", key.Name );
		}

		[Test]
		public void LocalMachineTest()
		{
			RegistryKey key = NUnitRegistry.LocalMachine;
			
			Assertion.AssertNotNull( key );
			Assertion.AssertEquals( @"HKEY_LOCAL_MACHINE\Software\Nascent Software\Nunit", key.Name );
		}
	}
}
