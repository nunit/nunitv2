// ****************************************************************
// Copyright 2011, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

#if CLR_2_0 || CLR_4_0
using System;
using System.Reflection;

namespace NUnit.Framework
{
    /// <summary>
    /// The interface implemented to provide actions to execute before and after suites.
    /// </summary>
    public interface ITestSuiteAction : ITestAction
    {
        /// <summary>
        /// Executed before each suite is run
        /// </summary>
        /// <param name="fixture">The fixture that defines the test suite, if available.</param>
        /// <param name="method">The method that defines the test suite, if available.</param>
        void BeforeTestSuite(object fixture, MethodInfo method);

        /// <summary>
        /// Executed after each suite is run
        /// </summary>
        /// <param name="fixture">The fixture that defines the test suite, if available.</param>
        /// <param name="method">The method that defines the test suite, if available.</param>
        void AfterTestSuite(object fixture, MethodInfo method);
    }
}
#endif
