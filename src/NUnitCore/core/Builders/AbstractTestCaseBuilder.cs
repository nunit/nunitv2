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

namespace NUnit.Core.Builders
{
	/// <summary>
	/// AbstractTestCaseBuilder may serve as a base class for 
	/// implementing a test case builder. It provides a templated
	/// implementation of the BuildFrom method.
	/// 
	/// Developers of extended test cases may choose to inherit
	/// from this class, although NUnitTestCaseBuilder will 
	/// probably be more useful if the extension is intended
	/// to work like an NUnit test case. 
	/// </summary>
	public abstract class AbstractTestCaseBuilder : ITestCaseBuilder
	{
		#region Instance Fields
		protected int runnerID;
		protected TestCase testCase;
		#endregion

		#region Abstract Methods
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
		public abstract bool CanBuildFrom(System.Reflection.MethodInfo method);

		/// <summary>
		/// Method that actually creates a new test case object.
		/// 
		/// Derived classes must override this method.
		/// </summary>
		/// <param name="method">The test method to examine</param>
		/// <returns>An object derived from TestCase</returns>
		protected abstract TestCase MakeTestCase( MethodInfo method );

		/// <summary>
		/// Method that sets properties of the test case based on the
		/// information in the provided MethodInfo.
		/// 
		/// Derived classes must override this method.
		/// </summary>
		/// <param name="method">The test method to examine</param>
		/// <param name="testCase">The test case being constructed</param>
		protected abstract void SetTestProperties( MethodInfo method, TestCase testCase );
		#endregion

		#region Virtual Methods
		/// <summary>
		/// Templated implementaton of ITestCaseBuilder.BuildFrom. 
		/// 
		/// Any derived builder may choose to override this method in
		/// it's entirety or to let it stand and override some of
		/// the virtual methods that it calls.
		/// </summary>
		/// <param name="method">The method for which a test case is to be built</param>
		/// <returns>A TestCase or null</returns>
		public virtual TestCase BuildFrom(System.Reflection.MethodInfo method)
		{
			if ( !HasValidTestCaseSignature( method ) )
				return new NotRunnableTestCase( method );

			TestCase testCase = MakeTestCase( method );
			if ( testCase != null )
				SetTestProperties( method , testCase );

			return testCase;
		}
		
		/// <summary>
		/// Virtual method that checks the signature of a potential test case to
		/// determine if it is valid. The default implementation requires methods
		/// to be public, non-abstract instance methods, taking no parameters and
		/// returning void. Methods not meeting these criteria will be marked by
		/// NUnit as non-runnable.
		/// </summary>
		/// <param name="method">The method to be checked</param>
		/// <returns>True if the method signature is valid, false if not</returns>
		protected virtual bool HasValidTestCaseSignature( MethodInfo method )
		{
			return !method.IsStatic
				&& !method.IsAbstract
				&& method.IsPublic
				&& method.GetParameters().Length == 0
				&& method.ReturnType.Equals(typeof(void) );
		}
		#endregion
	}
}
