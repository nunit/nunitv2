#region Copyright (c) 2002-2004, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002-2004 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
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
' Portions Copyright © 2002-2004 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright © 2000-2003 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

using System;
using System.Reflection;

namespace NUnit.Core
{
	/// <summary>
	/// This class originally did all the work of making test
	/// cases. With the new extensibility mechanism, it's reduced
	/// to a single static method and the AddinManager along with
	/// any installed test builders do the job.
	/// </summary>
	public abstract class TestCaseBuilder
	{
		/// <summary>
		/// Determine whether a test case can be created from
		/// a given method.
		/// </summary>
		/// <param name="method">MethodInfo for the method to use</param>
		/// <returns>True if a method can be created, false if not</returns>
		public static bool CanBuildFrom( MethodInfo method )
		{
			return AddinManager.Addins.CanBuildFrom( method )
				|| AddinManager.Builtins.CanBuildFrom( method );
		}

		/// <summary>
		/// Makes a test case from a given method if any builders
		/// know how to do it and returns null otherwise.
		/// </summary>
		/// <param name="method">MethodInfo for the particular method</param>
		/// <returns>A test case or null</returns>
		public static TestCase BuildFrom( MethodInfo method )
		{
			// First see if any addins are able to make the test case
			TestCase testCase = AddinManager.Addins.BuildFrom( method );

			// If not, try any builtin test case builders
			if ( testCase == null )
				testCase = AddinManager.Builtins.BuildFrom( method );

			return testCase;
		}

		/// <summary>
		/// Make a test case from a given fixture type and method.
		/// This method is retained for test compatibility.
		/// </summary>
		/// <param name="fixtureType">The fixture type</param>
		/// <param name="method">MethodInfo for the particular method</param>
		/// <returns>A test case or null</returns>
		public static TestCase Make(Type fixtureType, MethodInfo method)
		{
			return BuildFrom( method );
		}

		protected bool HasValidTestCaseSignature( MethodInfo method )
		{
			return !method.IsStatic
				&& !method.IsAbstract
				&& method.IsPublic
				&& method.GetParameters().Length == 0
				&& method.ReturnType.Equals(typeof(void) );
		}
		
		/// <summary>
		/// Private constructor to prevent object creation
		/// </summary>
		private TestCaseBuilder() { }
	}
}
