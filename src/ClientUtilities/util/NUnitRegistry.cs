#region Copyright (c) 2002-2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
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

namespace NUnit.Util
{
	using System;
	using System.IO;
	using System.Text;
	using Microsoft.Win32;

	/// <summary>
	/// NUnitRegistry provides static properties for NUnit's
	/// CurrentUser and LocalMachine subkeys.
	/// </summary>
	public class NUnitRegistry
	{
		private static readonly string KEY = 
			@"Software\nunit.org\Nunit\2.4";

		private static readonly string LEGACY_KEY = 
			@"Software\Nascent Software\Nunit\";

		private static bool testMode = false;
		private static string testKey = 
			@"Software\nunit.org\Nunit-Test";


		/// <summary>
		/// Prevent construction of object
		/// </summary>
		private NUnitRegistry() { }

		public static bool TestMode
		{
			get { return testMode; }
			set { testMode = value; }
		}

		public static string TestKey
		{
			get { return testKey; }
			set { testKey = value; }
		}

		/// <summary>
		/// Registry subkey for the current user
		/// </summary>
		public static RegistryKey CurrentUser
		{
			get 
			{
				if ( testMode )
					return Registry.CurrentUser.CreateSubKey( testKey );
				
				RegistryKey newKey = Registry.CurrentUser.OpenSubKey( KEY, true );
				if (newKey == null)
				{
					newKey = Registry.CurrentUser.CreateSubKey( KEY );
					RegistryKey oldKey = Registry.CurrentUser.OpenSubKey( LEGACY_KEY );
					if ( oldKey != null )
					{
						CopyKey( oldKey, newKey );
						oldKey.Close();
					}
				}

				return newKey; 
			}
		}

		/// <summary>
		/// Registry subkey for the local machine
		/// </summary>
		public static RegistryKey LocalMachine
		{
			get { return Registry.LocalMachine.CreateSubKey( testMode ? testKey : KEY ); }
		}

		public static void ClearTestKeys()
		{
			ClearSubKey( Registry.CurrentUser, testKey );
			//ClearSubKey( Registry.LocalMachine, testKey );	
		}

		/// <summary>
		/// Static helper method that clears out the contents of a subkey
		/// </summary>
		/// <param name="baseKey">Base key for the subkey</param>
		/// <param name="subKey">Name of the subkey</param>
		private static void ClearSubKey( RegistryKey baseKey, string subKey )
		{
			using( RegistryKey key = baseKey.OpenSubKey( subKey, true ) )
			{
				if ( key != null ) ClearKey( key );
			}
		}

		/// <summary>
		/// Static function that clears out the contents of a key
		/// </summary>
		/// <param name="key">Key to be cleared</param>
		public static void ClearKey( RegistryKey key )
		{
			foreach( string name in key.GetValueNames() )
				key.DeleteValue( name );

			// TODO: This throws under Mono - Restore when bug is fixed
			//foreach( string name in key.GetSubKeyNames() )
			//    key.DeleteSubKeyTree( name );

			foreach (string name in key.GetSubKeyNames())
			{
				ClearSubKey(key, name);
				// TODO: Remove this test when Mono bug is fixed
				if ( NUnit.Core.RuntimeFramework.CurrentFramework.Runtime == NUnit.Core.RuntimeType.Net ) 
					key.DeleteSubKey( name );
			}
		}

		/// <summary>
		/// Static method that copies the contents of one key to another
		/// </summary>
		/// <param name="fromKey">The source key for the copy</param>
		/// <param name="toKey">The target key for the copy</param>
		public static void CopyKey( RegistryKey fromKey, RegistryKey toKey )
		{
			foreach( string name in fromKey.GetValueNames() )
				toKey.SetValue( name, fromKey.GetValue( name ) );

			foreach( string name in fromKey.GetSubKeyNames() )
				using( RegistryKey fromSubKey = fromKey.OpenSubKey( name ) )
				using( RegistryKey toSubKey = toKey.CreateSubKey( name ) )
				{
					CopyKey( fromSubKey, toSubKey );
				}
		}
	}
}
