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
	/// This class collects static methods that build test cases.
	public class TestCaseBuilder
	{
		/// <summary>
		/// Makes a test case from a given method if any builders
		/// know how to do it and returns null otherwise.
		/// </summary>
		/// <param name="method">MethodInfo for the particular method</param>
		/// <returns>A test case or null</returns>
		public static TestCase Make( MethodInfo method )
		{
			// First see if any addins are able to make the test case
			TestCase testCase = AddinManager.CurrentManager.BuildFrom( method );

			// If not, try any builtin test case builders
			if ( testCase == null )
				testCase = Builtins.BuildFrom( method );

			if ( testCase != null )
			{
				testCase = Builtins.Decorate( testCase, method );
				testCase = AddinManager.CurrentManager.Decorate( testCase, method );
			}

			return testCase;
		}

		/// <summary>
		/// Old method, still used by tests. We may need to revisit
		/// this approach if the fixtureType for a test case is 
		/// ever different from the method's ReflectedType.
		/// </summary>
		/// <param name="fixtureType">The fixture type</param>
		/// <param name="method">MethodInfo for the particular method</param>
		/// <returns>A test case or null</returns>
		public static TestCase Make(Type fixtureType, MethodInfo method)
		{
			return Make( method );
		}

		/// <summary>
		/// Another method provided for test purposes only. Builds
		/// a test case from a fixture type and the name of a method.
		/// </summary>
		/// <param name="fixtureType">The fixture type</param>
		/// <param name="methodName">the method name to use for the test</param>
		/// <returns>A test case or null</returns>
		public static TestCase Make( Type fixtureType, string methodName )
		{
			return Make( Reflect.GetNamedMethod( 
					fixtureType,
					methodName,
					BindingFlags.Public | BindingFlags.Instance ) );
		}

		/// <summary>
		/// Private constructor to prevent object creation
		/// </summary>
		private TestCaseBuilder() { }
	}
}
