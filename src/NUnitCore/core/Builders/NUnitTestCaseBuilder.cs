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
		static bool allowOldStyleTests;

		static NUnitTestCaseBuilder()
		{
			try
			{
				NameValueCollection settings = (NameValueCollection)
					ConfigurationSettings.GetConfig("NUnit/TestCaseBuilder");
				if (settings != null)
				{
					string oldStyle = settings["OldStyleTestCases"];
					if (oldStyle != null)
						allowOldStyleTests = Boolean.Parse(oldStyle);
				}
			}
			catch(Exception e)
			{
				Debug.WriteLine(e);
			}
		}

		public override TestCase BuildFrom( MethodInfo method )
		{
			TestCase testCase = base.BuildFrom( method );
		
			if ( testCase != null )
			{
				PlatformHelper helper = new PlatformHelper();
				if ( !helper.IsPlatformSupported( method ) )
				{
					testCase.RunState = RunState.Skipped;
					testCase.IgnoreReason = helper.Reason;
				}

				testCase.Categories = GetCategories( method );

				testCase.IsExplicit = Reflect.HasAttribute( method, "NUnit.Framework.ExplicitAttribute", false );
				if ( testCase.IsExplicit )
					testCase.RunState = RunState.Explicit;
				
				System.Attribute[] attributes = 
					Reflect.GetAttributes( method, "NUnit.Framework.PropertyAttribute", false );

				foreach( Attribute propertyAttribute in attributes ) 
				{
					string name = (string)Reflect.GetPropertyValue( propertyAttribute, "Name", BindingFlags.Public | BindingFlags.Instance );
					if ( name != null && name != string.Empty )
					{
						object value = Reflect.GetPropertyValue( propertyAttribute, "Value", BindingFlags.Public | BindingFlags.Instance );
						testCase.Properties[name] = value;
					}
				}
			}

			return testCase;
		}

		protected virtual IList GetCategories( MethodInfo method )
		{
			System.Attribute[] attributes = 
				Reflect.GetAttributes( method, "NUnit.Framework.CategoryAttribute", false );
			IList categories = new ArrayList();

			foreach( Attribute categoryAttribute in attributes ) 
				categories.Add( 
					Reflect.GetPropertyValue( 
					categoryAttribute, 
					"Name", 
					BindingFlags.Public | BindingFlags.Instance ) );

			return categories;
		}

        #region AbstractTestCaseBuilder Overrides
        public override bool CanBuildFrom(MethodInfo method)
        {
            if (Reflect.HasAttribute(method, "NUnit.Framework.TestAttribute", false))
                return true;

            if (allowOldStyleTests)
            {
                Regex regex = new Regex("^(?i:test)");
                if (regex.Match(method.Name).Success && !IsSpecialMethod(method))
                    return true;
            }

            return false;
        }


        protected override TestCase MakeTestCase(MethodInfo method)
        {
            Type expectedException = null;
            string expectedExceptionName = null;
            string expectedMessage = null;
            string matchType = null;

            Attribute attribute = Reflect.GetAttribute(method, "NUnit.Framework.ExpectedExceptionAttribute", false);
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
                return new NUnitTestMethod(method, expectedException, expectedMessage, matchType);
            else if (expectedExceptionName != null)
                return new NUnitTestMethod(method, expectedExceptionName, expectedMessage, matchType);
            else
                return new NUnitTestMethod(method);
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
                Reflect.GetAttribute(method, "NUnit.Framework.IgnoreAttribute", false);
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
            Attribute testAttribute = Reflect.GetAttribute(method, "NUnit.Framework.TestAttribute", false);
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
            return Reflect.HasAttribute(method, "NUnit.Framework.SetUpAttribute", false);
        }
        protected virtual bool IsTearDownMethod(MethodInfo method)
        {
            return Reflect.HasAttribute(method, "NUnit.Framework.TearDownAttribute", false);
        }
        protected virtual bool IsFixtureSetUpMethod(MethodInfo method)
        {
            return Reflect.HasAttribute(method, "NUnit.Framework.FixtureSetUpAttribute", false);
        }
        protected virtual bool IsFixtureTearDownMethod(MethodInfo method)
        {
            return Reflect.HasAttribute(method, "NUnit.Framework.FixtureTearDownAttribute", false);
        }
        #endregion
    }
}
