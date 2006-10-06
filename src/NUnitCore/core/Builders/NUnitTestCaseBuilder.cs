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

using System;
using System.Reflection;
using System.Collections;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Diagnostics;

namespace NUnit.Core.Builders
{
	public class NUnitTestCaseBuilder : AbstractTestCaseBuilder
	{
		private bool allowOldStyleTests = NUnitFramework.AllowOldStyleTests;

        #region AbstractTestCaseBuilder Overrides
		/// <summary>
		/// Determine if the method is an NUnit test method.
		/// The method must normally be marked with the test
		/// attribute for this to be true. If the test config
		/// file sets AllowOldStyleTests to true, then any 
		/// method beginning "test..." (case-insensitive)
		/// is treated as a test unless it is also marked
		/// as a setup or teardown method.
		/// </summary>
		/// <param name="method">A MethodInfo for the method being used as a test method</param>
		/// <returns>True if the builder can create a test case from this method</returns>
        public override bool CanBuildFrom(MethodInfo method)
        {
            if ( Reflect.HasAttribute( method, NUnitFramework.TestAttribute, false ) )
                return true;

            if (allowOldStyleTests)
            {
                Regex regex = new Regex("^(?i:test)");
                if ( regex.Match(method.Name).Success 
					&& !NUnitFramework.IsSetUpMethod( method )
					&& !NUnitFramework.IsTearDownMethod( method )
					&& !NUnitFramework.IsFixtureSetUpMethod( method )
					&& !NUnitFramework.IsFixtureTearDownMethod( method ) )
						return true;
            }

            return false;
        }

		/// <summary>
		/// Create an NUnitTestMethod with arguments determined by 
		/// examining any ExpectedException attribute.
		/// TODO: Move the analysis of ExpectedExceptionAttribute
		/// to SetTestProperties.
		/// </summary>
		/// <param name="method">A MethodInfo for the method being used as a test method</param>
		/// <returns>A new NUnitTestMethod</returns>
        protected override TestCase MakeTestCase(MethodInfo method)
        {
			return new NUnitTestMethod( method );
        }

		/// <summary>
		/// Set additional properties of the newly created test case based
		/// on its attributes. As implemented, the method sets the test's
		/// RunState,  Description, Categories and Properties.
		/// </summary>
		/// <param name="method">A MethodInfo for the method being used as a test method</param>
		/// <param name="testCase">The test case being constructed</param>
		protected override void SetTestProperties( MethodInfo method, TestCase testCase )
		{
            NUnitFramework.ApplyCommonAttributes( method, testCase );
			NUnitFramework.ApplyExpectedExceptionAttribute( method, (TestMethod)testCase );
		}
		#endregion
    }
}
