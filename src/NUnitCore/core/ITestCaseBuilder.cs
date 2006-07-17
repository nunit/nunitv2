#region Copyright (c) 2002-2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright © 2000-2003 Philip A. Craig
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
' Portions Copyright © 2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright © 2000-2003 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

using System.Reflection;

namespace NUnit.Core
{
	/// <summary>
	/// The ITestCaseBuilder interface is exposed by a class that knows how to
	/// build a test case from certain methods. 
	/// </summary>
	public interface ITestCaseBuilder
	{
		/// <summary>
		/// Examine the method and determine if it is suitable for
		/// this builder to use in building a TestCase.
		/// 
		/// Note that returning false will cause the method to be ignored 
		/// in loading the tests. If it is desired to load the method
		/// but label it as non-runnable, ignored, etc., then this
		/// method must return true.
		/// 
		/// Derived classes must override this method.
		/// </summary>
		/// <param name="method">The test method to examine</param>
		/// <returns>True is the builder can use this method</returns>
		bool CanBuildFrom( MethodInfo method );

		/// <summary>
		/// Build a TestCase from the provided MethodInfo.
		/// </summary>
		/// <param name="method">The method to be used as a test case</param>
		/// <returns>A TestCase or null</returns>
		TestCase BuildFrom( MethodInfo method );
	}
}
