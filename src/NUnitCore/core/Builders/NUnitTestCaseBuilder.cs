// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org/?p=license&r=2.4.
// ****************************************************************
#define ALLOW_PROVIDERS_ON_ANY_METHOD

using System;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Text.RegularExpressions;
using NUnit.Core.Extensibility;

namespace NUnit.Core.Builders
{
	public class NUnitTestCaseBuilder : Extensibility.ITestCaseBuilder
	{
		private readonly bool allowOldStyleTests = NUnitFramework.AllowOldStyleTests;

	    private readonly IParameterProvider provider = CoreExtensions.Host.ParameterProviders;

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
		/// <param name="suite">The test suite being built, to which the new test would be added</param>
		/// <returns>True if the builder can create a test case from this method</returns>
        public bool CanBuildFrom(MethodInfo method, Test suite)
        {
            if( Reflect.HasAttribute( method, NUnitFramework.TestAttribute, false ) ||
#if ALLOW_PROVIDERS_ON_ANY_METHOD
                provider.HasParametersFor( method ) )
#else
                Reflect.HasAttribute(method, NUnitFramework.TestCaseAttribute, false) ||
                Reflect.HasAttribute(method, NUnitFramework.DataSourceAttribute, false))
#endif
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
		/// Build a Test from the provided MethodInfo. Depending on
		/// whether the method takes arguments and on the availability
		/// of test case data, this method may return a single test
		/// or a group of tests contained in a ParameterizedMethodSuite.
		/// </summary>
		/// <param name="method">The MethodInfo for which a test is to be built</param>
		/// <param name="suite">The test fixture being populated, or null</param>
		/// <returns>A Test representing one or more method invocations</returns>
		public Test BuildFrom(System.Reflection.MethodInfo method, Test suite)
		{
   
            // If we need no parameters and none are provided, take a shortcut
            if ( method.GetParameters().Length == 0 && !provider.HasParametersFor(method))
                return BuildTestMethod(method);

		    IList parmList = provider.GetParametersFor(method);

            switch( parmList.Count )
            {
                case 0:     // Allow for provider returning none when called 
                    return BuildTestMethod(method);

                case 1:     // If there is only one, don't bother with a suite
                    ParameterSet parms = parmList[0] as ParameterSet;
                    return BuildTestMethod(method, parms);

                default:    // Retuern a suite of tests using the parameters
                    return BuildMultipleTestMethods(method, parmList);
            }
        }
		#endregion

        #region Helper Methods
        /// <summary>
        /// Build a standalone non-parameterized NUnitTestMethod
        /// </summary>
        /// <param name="method">The MethodInfo for which a test is to be built</param>
        /// <returns>A single NUnitTestMethod</returns>
        private static NUnitTestMethod BuildTestMethod(MethodInfo method)
        {
            return BuildSingleTestMethod(method, null, null);
        }

        /// <summary>
        /// Builds a standalone parameterized NUnitTestMethod for use in
        /// situations where only one ParameterSet is provided.
        /// </summary>
        /// <param name="method">The MethodInfo for which a test is to be built</param>
        /// <param name="parms">The ParameterSet to be used in building the test</param>
        /// <returns>A single NUnitTestMethod</returns>
        private static NUnitTestMethod BuildTestMethod(MethodInfo method, ParameterSet parms)
        {
            return BuildSingleTestMethod(method, parms, null);
        }

        /// <summary>
        /// Builds a ParameterizedMethodSuite containing multiple invocations
        /// of the same test method with dif
        /// </summary>
        /// <param name="method"></param>
        /// <param name="parmList"></param>
        /// <returns></returns>
        private static Test BuildMultipleTestMethods(MethodInfo method, IList parmList)
        {
            ParameterizedMethodSuite suite = new ParameterizedMethodSuite(method);
            NUnitFramework.ApplyCommonAttributes(method, suite);

            foreach (ParameterSet parms in parmList)
            {
                TestMethod test = BuildSingleTestMethod(method, parms, suite);

                suite.Add(test);
            }

            return suite;
        }

        /// <summary>
        /// Builds a single NUnitTestMethod, either as a child of the fixture 
        /// or as one of a set of test cases under a ParameterizedTestMethodSuite.
        /// </summary>
        /// <param name="method">The MethodInfo from which to construct the TestMethod</param>
        /// <param name="parms">The ParameterSet to be used, or null</param>
        /// <param name="suite">The ParameterizedMethodSuite being constructed, or null</param>
        /// <returns></returns>
        private static NUnitTestMethod BuildSingleTestMethod(MethodInfo method, ParameterSet parms, TestSuite suite)
        {
            NUnitTestMethod testMethod = new NUnitTestMethod(method);

            if (CheckTestMethodSignature(testMethod, parms))
            {
                NUnitFramework.ApplyCommonAttributes(method, testMethod);
                ApplyExpectedExceptionAttribute(method, testMethod);
            }

            if (parms != null)
            {
                if (parms.TestName != null)
                {
                    testMethod.TestName.Name = parms.TestName;
                    testMethod.TestName.FullName = method.DeclaringType.FullName + "." + parms.TestName;
                }
                else if (parms.Arguments != null && suite != null)
                {
                    string suffix = GetArgumentString(parms.Arguments);
                    testMethod.TestName.Name += suffix;
                    testMethod.TestName.FullName += suffix;
                }

                if (parms.ExpectedExceptionName != null)
                {
                    testMethod.ExceptionExpected = true;
                    testMethod.ExpectedExceptionType = parms.ExpectedExceptionType;
                    testMethod.ExpectedExceptionName = parms.ExpectedExceptionName;
                    testMethod.ExpectedMessage = parms.ExpectedExceptionMessage;
                }

                if (parms.Description != null)
                    testMethod.Description = parms.Description;
            }

            return testMethod;
        }

	    /// <summary>
        /// Helper method that checks the signature of a TestMethod and
        /// any supplied parameters to determine if the test is valid.
        /// 
        /// Currently, NUnitTestMethods are required to be public, 
        /// non-abstract methods, either static or instance,
        /// returning void. They may take arguments but the values must
        /// be provided or the TestMethod is not considered runnable.
        /// 
        /// Methods not meeting these criteria will be marked as
        /// non-runnable and the method will return false in that case.
        /// </summary>
        /// <param name="testMethod">The TestMethod to be checked. If it
        /// is found to be non-runnable, it will be modified.</param>
        /// <param name="parms">Parameters to be used for this test, or null</param>
        /// <returns>True if the method signature is valid, false if not</returns>
        private static bool CheckTestMethodSignature(TestMethod testMethod, ParameterSet parms)
		{
            MethodInfo method = testMethod.Method;

            if (method.IsAbstract)
            {
                testMethod.RunState = RunState.NotRunnable;
                testMethod.IgnoreReason = "Method is abstract";
                return false;
            }

            if (!method.IsPublic)
            {
                testMethod.RunState = RunState.NotRunnable;
                testMethod.IgnoreReason = "Method is not public";
                return false;
            }

            if ( parms == null )
            {
                if (method.GetParameters().Length > 0)
                {
                    testMethod.RunState = RunState.NotRunnable;
                    testMethod.IgnoreReason = "No arguments provided";
                    return false;
                }
                else if (!method.ReturnType.Equals(typeof(void)))
                {
                    testMethod.RunState = RunState.NotRunnable;
                    testMethod.IgnoreReason = "Method has non-void return value";
                    return false;
                }

                return true;
            }

            testMethod.arguments = parms.Arguments;
            testMethod.expectedResult = parms.Result;
            testMethod.RunState = parms.RunState;
            testMethod.IgnoreReason = parms.NotRunReason;

            if( testMethod.RunState != RunState.Runnable)
                return false;

            object[] arglist = parms.Arguments;
            int argsProvided = 0;

            if (arglist != null)
                argsProvided = arglist.Length;
            int argsNeeded = method.GetParameters().Length;


            if (!method.ReturnType.Equals(typeof(void)) && parms.Result == null && parms.ExpectedExceptionName == null)
            {
                testMethod.RunState = RunState.NotRunnable;
                testMethod.IgnoreReason = "Method has non-void return value";
                return false;
            }

            if (argsProvided > 0 && argsNeeded == 0)
            {
                testMethod.RunState = RunState.NotRunnable;
                testMethod.IgnoreReason = "Arguments provided for method not taking any";
                return false;
            }

            if (argsProvided == 0 && argsNeeded > 0)
            {
                testMethod.RunState = RunState.NotRunnable;
                testMethod.IgnoreReason = "No arguments were provided";
                return false;
            }

            if (argsProvided != argsNeeded)
            {
                testMethod.RunState = RunState.NotRunnable;
                testMethod.IgnoreReason = "Wrong number of arguments provided";
                return false;
            }

	        // TODO: Check type compatibility and possibly convert here

            return true;
        }

		private static bool CanConvertTypes( Type fromType, Type toType )
		{
			return fromType == toType;
		}

        private static string GetArgumentString(object[] arglist)
        {
            StringBuilder sb = new StringBuilder("(");

            if (arglist != null)
                for (int i = 0; i < arglist.Length; i++)
                {
                    if (i > 0) sb.Append(",");

                    object arg = arglist[i];
                    if (arg == null)
                        sb.Append("null");
                    else
                        sb.Append(arg.ToString());
                }

            sb.Append(")");

            return sb.ToString();
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
