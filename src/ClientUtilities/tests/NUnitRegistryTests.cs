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
using System.IO;
using System.Text;
//using System.Windows.Forms;
using Microsoft.Win32;
using NUnit.Framework;

namespace NUnit.Util.Tests
{
	/// <summary>
	/// Summary description for NUnitRegistryTests.
	/// </summary>
	[TestFixture]
	public class NUnitRegistryTests
	{
		[TearDown]
		public void RestoreRegistry()
		{
			NUnitRegistry.TestMode = false;
		}

		[Test]
		public void CurrentUser()
		{
			NUnitRegistry.TestMode = false;
			using( RegistryKey key = NUnitRegistry.CurrentUser )
			{
				Assert.IsNotNull( key );
				Assert.AreEqual( @"HKEY_CURRENT_USER\Software\nunit.org\Nunit\2.4".ToLower(), key.Name.ToLower() );
			}
		}

		[Test]
		public void LocalMachine()
		{
			NUnitRegistry.TestMode = false;
			using( RegistryKey key = NUnitRegistry.LocalMachine )
			{
				Assert.IsNotNull( key );
				StringAssert.EndsWith( @"Software\nunit.org\Nunit\2.4".ToLower(), key.Name.ToLower() );
			}
		}

		[Test]
		public void CurrentUserTestMode()
		{

			NUnitRegistry.TestMode = true;
			using( RegistryKey key = NUnitRegistry.CurrentUser )
			{
				Assert.IsNotNull( key );
				Assert.AreEqual( @"HKEY_CURRENT_USER\Software\nunit.org\Nunit-Test".ToLower(), key.Name.ToLower() );
			}
		}

		[Test]
		public void LocalMachineTestMode()
		{
			NUnitRegistry.TestMode = true;
			using( RegistryKey key = NUnitRegistry.LocalMachine )
			{
				Assert.IsNotNull( key );
				StringAssert.EndsWith( @"Software\nunit.org\Nunit-Test".ToLower(), key.Name.ToLower() );
			}
		}

		// Not sure why this fails on Mono under windows, but it does
		[Test, Platform(Exclude="Mono")]
		public void TestClearRoutines()
		{
			NUnitRegistry.TestMode = true;

			using( RegistryKey key = NUnitRegistry.CurrentUser )
			using( RegistryKey foo = key.CreateSubKey( "foo" ) )
			using( RegistryKey bar = key.CreateSubKey( "bar" ) )
			using( RegistryKey footoo = foo.CreateSubKey( "foo" ) )
			{
				key.SetValue("X", 5);
				key.SetValue("NAME", "Joe");
				foo.SetValue("Y", 17);
				bar.SetValue("NAME", "Jennifer");
				footoo.SetValue( "X", 5 );
				footoo.SetValue("NAME", "Charlie" );
				
				NUnitRegistry.ClearTestKeys();

				Assert.AreEqual( 0, key.ValueCount );
				Assert.AreEqual( 0, key.SubKeyCount );
			}
		}
	}
}
