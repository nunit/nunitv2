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
		private static string KEY = "Software\\Nascent Software\\Nunit\\";

		/// <summary>
		/// Prevent construction of object
		/// </summary>
		private NUnitRegistry()
		{
		}

		/// <summary>
		/// Registry subkey for the current user
		/// </summary>
		public static RegistryKey CurrentUser
		{
			get { return Registry.CurrentUser.CreateSubKey( KEY ); }
		}

		/// <summary>
		/// Registry subkey for the local machine
		/// </summary>
		public static RegistryKey LocalMachine
		{
			get { return Registry.LocalMachine.CreateSubKey( KEY ); }
		}
	}
}
