/********************************************************************************************************************
'
' Copyright (c) 2002, James Newkirk, Michael C. Two, Alexei Vorontsov, Philip Craig
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
' THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
'
'*******************************************************************************************************************/
namespace NUnit.Util
{
	using System;
	using Microsoft.Win32;

	/// <summary>
	/// NUnitRegistry provides static properties for NUnit's
	/// CurrentUser and LocalMachine subkeys.
	/// </summary>
	public class NUnitRegistry
	{
		private static readonly string KEY = "Software\\Nascent Software\\Nunit\\";

		private static bool testMode = false;
		private static string testKey = "Software\\Nascent Software\\Nunit-Test";

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
				// Todo: Code can go here to migrate the registry
				// if we change our location.
				//	Try to open new key
				//	if ( key doesn't exist )
				//		create it
				//		open old key
				//		if ( it was opened )
				//			copy entries to new key
				//	return new key
				return Registry.CurrentUser.CreateSubKey( testMode ? testKey : KEY ); 
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
			ClearSubKey( Registry.LocalMachine, testKey );
		}

		/// <summary>
		/// Static function that clears out the contents of a subkey
		/// </summary>
		/// <param name="baseKey">Base key for the subkey</param>
		/// <param name="subKey">Name of the subkey</param>
		public static void ClearSubKey( RegistryKey baseKey, string subKey )
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

			foreach( string name in key.GetSubKeyNames() )
				key.DeleteSubKeyTree( name );
		}
	}
}
