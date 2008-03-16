// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org/?p=license&r=2.4.
// ****************************************************************

using System;
using System.Reflection;
using System.Collections;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Diagnostics;

namespace NUnit.Core.Builders
{
	public class NUnitTestCaseBuilder : Extensibility.ITestCaseBuilder
	{
		private bool allowOldStyleTests = NUnitFramework.AllowOldStyleTests;

        #region ITestCaseBuilder Methods
		/// <summary>
		/// Determine if the method is marked as an NUnit test method.
		/// The method must normally be marked with the TestAttribute
		/// for this to be true. If the test config file sets 
		/// AllowOldStyleTests to true, then any method beginning 
		/// "test..." (case-insensitive) is treated as a test unless 
		/// it is also marked as a setup or teardown method.
		/// 
		/// Note that this method does not check that the signature
		/// of the method for validity. If we did that here, any
		/// test methods with invalid signatures would be passed
		/// over in silence in the test run. Since we want such
		/// methods to be reported, the check for validity is made
		/// in BuildFrom rather than here.
		/// </summary>
		/// <param name="method">A MethodInfo for the method being used as a test method</param>
		/// <returns>True if the builder can create a test case from this method</returns>
        public bool CanBuildFrom(MethodInfo method)
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
		/// Build a test case for the provided MethodInfo.
		/// </summary>
		/// <param name="method">The method for which a test case is to be built</param>
		/// <returns>A TestCase or null</returns>
		public Test BuildFrom(System.Reflection.MethodInfo method)
		{
			NUnitTestMethod testCase = new NUnitTestMethod( method );
			
			CheckTestCaseSignature( method, testCase );
			if ( testCase.RunState == RunState.Runnable )
			{
				NUnitFramework.ApplyCommonAttributes( method, testCase );
				ApplyExpectedExceptionAttribute( method, testCase );
			}

			return testCase;
		}
		#endregion
		
		#region Helper Methods
		/// <summary>
		/// Helper method that checks the signature of a potential test case to
		/// determine if it is valid. Currently, NUnit test methods are required
		/// to be public, non-abstract instance methods, taking no parameters and
		/// returning void. Methods not meeting these criteria will be marked by
		/// NUnit as non-runnable.
		/// </summary>
		/// <param name="method">The method to be checked</param>
		/// <returns>True if the method signature is valid, false if not</returns>
		private static void CheckTestCaseSignature( MethodInfo method, TestCase testCase )
		{
			string reason;

			if (method.IsAbstract)
				reason = "it must not be abstract";
			else if (!method.IsPublic)
				reason = "it must be a public method";
			else if (method.GetParameters().Length != 0)
				reason = "it must not have parameters";
			else if (!method.ReturnType.Equals(typeof(void)))
				reason = "it must return void";
			else
				return;

			testCase.RunState = RunState.NotRunnable;
			testCase.IgnoreReason = String.Format("Method {0}'s signature is not correct: {1}.", method.Name, reason);
		}

		private static void ApplyExpectedExceptionAttribute(MethodInfo method, TestMethod testMethod)
		{
			Attribute attribute = Reflect.GetAttribute(
				method, NUnitFramework.ExpectedExceptionAttribute, false );

			if (attribute != null)
			{
				testMethod.ExceptionExpected = true;

				Type expectedExceptionType = GetExceptionType( attribute );
				string expectedExceptionName = GetExceptionName( attribute );
				if ( expectedExceptionType != null )
					testMethod.ExpectedExceptionType = expectedExceptionType;
				else if ( expectedExceptionName != null )
					testMethod.ExpectedExceptionName = expectedExceptionName;
				
				testMethod.ExpectedMessage = GetExpectedMessage( attribute );
				testMethod.MatchType = GetMatchType( attribute );
				testMethod.UserMessage = GetUserMessage( attribute );

				string handlerName = GetHandler( attribute );
				if ( handlerName == null )
					testMethod.ExceptionHandler = GetDefaultExceptionHandler( testMethod.FixtureType );
				else
				{
					MethodInfo handler = GetExceptionHandler( testMethod.FixtureType, handlerName );
					if ( handler != null )
						testMethod.ExceptionHandler = handler;
					else
					{
						testMethod.RunState = RunState.NotRunnable;
						testMethod.IgnoreReason = string.Format( 
							"The specified exception handler {0} was not found", handlerName );
					}
				}
			}
		}

		private static MethodInfo GetDefaultExceptionHandler( Type fixtureType )
		{
			return Reflect.HasInterface( fixtureType, NUnitFramework.ExpectExceptionInterface )
				? GetExceptionHandler( fixtureType, "HandleException" )
				: null;
		}

		private static MethodInfo GetExceptionHandler( Type fixtureType, string name )
		{
			return Reflect.GetNamedMethod( 
				fixtureType, 
				name,
				new string[] { "System.Exception" },
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static );
		}
		
		private static string GetHandler(System.Attribute attribute)
		{
			return Reflect.GetPropertyValue( attribute, "Handler" ) as string;
		}

		private static Type GetExceptionType(System.Attribute attribute)
		{
			return Reflect.GetPropertyValue( attribute, "ExceptionType" ) as Type;
		}

		private static string GetExceptionName(System.Attribute attribute)
		{
			return Reflect.GetPropertyValue( attribute, "ExceptionName" ) as string;
		}

		private static string GetExpectedMessage(System.Attribute attribute)
		{
			return Reflect.GetPropertyValue( attribute, "ExpectedMessage" ) as string;
		}

		private static string GetMatchType(System.Attribute attribute)
		{
			object matchEnum = Reflect.GetPropertyValue( attribute, "MatchType" );
			return matchEnum != null ? matchEnum.ToString() : null;
		}

		private static string GetUserMessage(System.Attribute attribute)
		{
			return Reflect.GetPropertyValue( attribute, "UserMessage" ) as string;
		}
		#endregion
    }
}
