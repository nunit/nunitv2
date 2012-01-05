// ****************************************************************
// Copyright 2011, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

#if CLR_2_0 || CLR_4_0
using System;
using System.Reflection;

namespace NUnit.Framework.Interfaces
{
    /// <summary>
    /// When implemented by an attribute, this interface implemented to provide actions to execute before and after tests.
    /// </summary>
    public interface ITestAction
    {
        /// <summary>
        /// Executed before each test is run
        /// </summary>
        /// <param name="testDetails">Provides details about the test that is going to be run.</param>
        void BeforeTest(TestDetails testDetails);

        /// <summary>
        /// Executed after each test is run
        /// </summary>
        /// <param name="testDetails">Provides details about the test that has just been run.</param>
        void AfterTest(TestDetails testDetails);

    }
}
#endif
