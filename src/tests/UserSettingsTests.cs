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
			Point pt = new Point( 100, 200 );
			Size sz = new Size( 20, 25 );
			
			UserSettings.Form.Location = pt;
			UserSettings.Form.Size = sz;

			Assert.Equals( pt, UserSettings.Form.Location );
			Assert.Equals( sz, UserSettings.Form.Size );
		}

		[Test]
		public void FormPositionDefaults()
		{	
			FormSettings f = UserSettings.Form;
			Point pt = f.Location;
			Size sz = f.Size;

			Assert.Equals( new Point( 10, 10 ), pt );
			Assert.Equals( new Size( 632, 432 ), sz );
		}

		[Test]
		public void RecentAssemblyBasicTests()
		{
			RecentAssemblySettings assemblies = UserSettings.RecentAssemblies;
			Assert.Equals( @"Recent-Assemblies", assemblies.Storage.StorageName );
			Assert.Equals( @"HKEY_CURRENT_USER\Software\Nascent Software\Nunit-Test\Recent-Assemblies", 
				((RegistrySettingsStorage)assemblies.Storage).StorageKey.Name );
			Assertion.AssertNotNull( "GetFiles() returned null", assemblies.GetFiles() );
			Assert.Equals( 0, assemblies.GetFiles().Count );
			Assertion.AssertNull( "No RecentAssembly should return null", assemblies.RecentFile );

			assemblies.RecentFile = "one";
			assemblies.RecentFile = "two";
			Assert.Equals( 2, assemblies.GetFiles().Count );
			Assert.Equals( "two", assemblies.RecentFile );

			using( RegistryKey key = NUnitRegistry.CurrentUser.OpenSubKey( "Recent-Assemblies" ) )
			{
				Assert.Equals( 2, key.ValueCount );
				Assert.Equals( "two", key.GetValue( "File1" ) );
				Assert.Equals( "one", key.GetValue( "File2" ) );
			}
		}

		[Test]
		public void RecentProjectBasicTests()
		{
			RecentProjectSettings projects = UserSettings.RecentProjects;
			Assert.Equals( @"Recent-Projects", projects.Storage.StorageName );
			Assert.Equals( @"HKEY_CURRENT_USER\Software\Nascent Software\Nunit-Test\Recent-Projects", 
				((RegistrySettingsStorage)projects.Storage).StorageKey.Name );
			Assertion.AssertNotNull( "GetFiles() returned null", projects.GetFiles() );
			Assert.Equals( 0, projects.GetFiles().Count );
			Assertion.AssertNull( "No RecentFile should return null", projects.RecentFile );

			projects.RecentFile = "one";
			projects.RecentFile = "two";
			Assert.Equals( 2, projects.GetFiles().Count );
			Assert.Equals( "two", projects.RecentFile );

			using( RegistryKey key = NUnitRegistry.CurrentUser.OpenSubKey( "Recent-Projects" ) )
			{
				Assert.Equals( 2, key.ValueCount );
				Assert.Equals( "two", key.GetValue( "File1" ) );
				Assert.Equals( "one", key.GetValue( "File2" ) );
			}
		}
	}
}
