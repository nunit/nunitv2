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
using System;
using System.Drawing;
using Microsoft.Win32;

namespace NUnit.Util
{
	/// <summary>
	/// Summary description for NunitRegistry.
	/// </summary>
	public class RegistryHelper
	{
		private static string KEY = "Software\\Nascent Software\\Nunit\\";

		public RegistryHelper()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static string ApplicationKey
		{
			get { return KEY; }
		}

		private static string FullKey( string subkey )
		{
			return String.Format( "{0}{1}", KEY, subkey );
		}

		public static RegistryKey CurrentUser
		{
			get { return Registry.CurrentUser.CreateSubKey( KEY ); }
		}

		public static RegistryKey LocalMachine
		{
			get { return Registry.LocalMachine.CreateSubKey( KEY ); }
		}
	}
}
