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
    /// The interface implemented to provide actions to execute before and after tests.
    /// </summary>
    public interface ITestCaseAction : ITestAction
    {
        /// <summary>
        /// Executed before each test case is run
        /// </summary>
        /// <param name="fixture">The fixture the test is part of, if available.</param>
        /// <param name="method">The method that implements the test case, if available.</param>
        void BeforeTestCase(object fixture, MethodInfo method);

        /// <summary>
        /// Executed after each test case is run
        /// </summary>
        /// <param name="fixture">The fixture the test is part of, if available.</param>
        /// <param name="method">The method that implements the test case, if available.</param>
        void AfterTestCase(object fixture, MethodInfo method);
    }
}
#endif
