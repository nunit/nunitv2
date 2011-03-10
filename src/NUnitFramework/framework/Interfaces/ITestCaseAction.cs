// ****************************************************************
// Copyright 2011, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

#if NET_2_0 || NET_3_5
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
        /// <param name="fixture">The fixture the test is part of</param>
        /// <param name="method">The method that implements the test case</param>
        void BeforeTestCase(object fixture, MethodInfo method);

        /// <summary>
        /// Executed after each test case is run
        /// </summary>
        /// <param name="fixture">The fixture the test is part of</param>
        /// <param name="method">The method that implements the test case</param>
        void AfterTestCase(object fixture, MethodInfo method);
    }
}
#endif
