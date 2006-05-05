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
using System.Text.RegularExpressions;

namespace NUnit.Core.Builders
{
	public class CSUnitTestCaseBuilder : AbstractTestCaseBuilder
	{
        #region AbstractTestCaseBuilder Overrides
        public override bool CanBuildFrom(MethodInfo method)
        {
            if (Reflect.HasAttribute(method, "csUnit.TestAttribute", true))
                    return true;

            Regex regex = new Regex("^(?i)test");
            return regex.Match(method.Name).Success && !IsSpecialMethod(method);
        }


        protected override TestCase MakeTestCase(MethodInfo method)
        {
            Type expectedException = null;
            string expectedExceptionName = null;
            string expectedMessage = null;
            string matchType = null;

            Attribute attribute = Reflect.GetAttribute(method, "csUnit.ExpectedExceptionAttribute", false);
            if (attribute != null)
            {
                expectedException = Reflect.GetPropertyValue(
                    attribute, "ExceptionType",
                    BindingFlags.Public | BindingFlags.Instance) as Type;
                expectedExceptionName = (string)Reflect.GetPropertyValue(
                    attribute, "ExceptionName",
                    BindingFlags.Public | BindingFlags.Instance) as String;
                expectedMessage = (string)Reflect.GetPropertyValue(
                    attribute, "ExpectedMessage",
                    BindingFlags.Public | BindingFlags.Instance) as String;
                object matchEnum = Reflect.GetPropertyValue(
                    attribute, "MatchType",
                    BindingFlags.Public | BindingFlags.Instance);
                if (matchEnum != null)
                    matchType = matchEnum.ToString();
            }

            if (expectedException != null)
                return new CSUnitTestMethod(method, expectedException, expectedMessage, matchType);
            else if (expectedExceptionName != null)
                return new CSUnitTestMethod(method, expectedExceptionName, expectedMessage, matchType);
            else
                return new CSUnitTestMethod(method);
        }

        /// <summary>
        /// Checks to see if the test is runnable by looking for the ignore 
        /// attribute specified in the parameters.
        /// </summary>
        /// <param name="method">The method to be checked</param>
        /// <param name="reason">A message indicating why the fixture is not runnable</param>
        /// <returns>True if runnable, false if not</returns>
        protected override bool IsRunnable(MethodInfo method, ref string reason)
        {
            Attribute ignoreAttribute =
                Reflect.GetAttribute(method, "csUnit.IgnoreAttribute", false);
            if (ignoreAttribute != null)
            {
                reason = (string)Reflect.GetPropertyValue(
                    ignoreAttribute,
                    "Reason",
                    BindingFlags.Public | BindingFlags.Instance);
                return false;
            }

            return true;
        }

        protected override string GetTestCaseDescription(MethodInfo method)
        {
            Attribute testAttribute = Reflect.GetAttribute(method, "csUnit.TestAttribute", false);
            if (testAttribute != null)
                return (string)Reflect.GetPropertyValue(
                    testAttribute,
                    "Description",
                    BindingFlags.Public | BindingFlags.Instance);

            return null;
        }
        #endregion

        #region Virtual Methods
        protected virtual bool IsSpecialMethod(MethodInfo method)
        {
            return IsSetUpMethod(method)
                || IsTearDownMethod(method)
                || IsFixtureSetUpMethod(method)
                || IsFixtureTearDownMethod(method);
        }

        protected virtual bool IsSetUpMethod(MethodInfo method)
        {
            return Reflect.HasAttribute(method, "csUnit.SetUpAttribute", false);
        }
        protected virtual bool IsTearDownMethod(MethodInfo method)
        {
            return Reflect.HasAttribute(method, "csUnit.TearDownAttribute", false);
        }
        protected virtual bool IsFixtureSetUpMethod(MethodInfo method)
        {
            return Reflect.HasAttribute(method, "csUnit.FixtureSetUpAttribute", false);
        }
        protected virtual bool IsFixtureTearDownMethod(MethodInfo method)
        {
            return Reflect.HasAttribute(method, "csUnit.FixtureTearDownAttribute", false);
        }
        #endregion
    }
}
