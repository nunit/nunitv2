// ****************************************************************
// Copyright 2011, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

#if CLR_2_0 || CLR_4_0
using System;

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
        /// <param name="fixture">The fixture that defines the test suite</param>
        void BeforeTestSuite(object fixture);

        /// <summary>
        /// Executed after each suite is run
        /// </summary>
        /// <param name="fixture">The fixture that defines the test suite</param>
        void AfterTestSuite(object fixture);
    }
}
#endif
