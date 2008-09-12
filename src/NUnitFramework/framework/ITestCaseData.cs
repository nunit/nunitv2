// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;

namespace NUnit.Framework
{
    /// <summary>
    /// The ITestCaseData interface is implemented by a class
    /// that is able to return complete testcases for use by
    /// a parameterized test method.
    /// </summary>
    public interface ITestCaseData
    {
        /// <summary>
        /// Gets the argument list to be provided to the test
        /// </summary>
        object[] Arguments { get; }

        /// <summary>
        /// Gets the expected result
        /// </summary>
        object Result { get; }

        /// <summary>
        ///  Gets the expected exception Type
        /// </summary>
        Type ExpectedException { get; }

        /// <summary>
        /// Gets the FullName of the expected exception
        /// </summary>
        string ExpectedExceptionName { get; }

        /// <summary>
        /// Gets the name to be used for the test
        /// </summary>
        string TestName { get; }

        /// <summary>
        /// Gets the description of the test
        /// </summary>
        string Description { get; }
    }
}
