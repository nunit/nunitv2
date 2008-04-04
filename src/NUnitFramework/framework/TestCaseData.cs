using System;

namespace NUnit.Framework
{
    /// <summary>
    /// The TestCaseData class represents a set of arguments
    /// and other parameter info to be used for a parameterized
    /// test case.
    /// </summary>
    public class TestCaseData
    {
            public object[] Arguments;
            public object Result;
            public Type ExpectedException;
            public string ExpectedExceptionName;
            public string TestName;
            public string Description;


            /// <summary>
            /// Initializes a new instance of the <see cref="T:TestCaseData"/> class.
            /// </summary>
            /// <param name="args">The args.</param>
            public TestCaseData(params object[] args)
            {
                this.Arguments = args;
            }

            /// <summary>
            /// Sets the expected result for the test
            /// </summary>
            /// <param name="result">The expected result</param>
            /// <returns>A modified TestCaseData</returns>
            public TestCaseData Returns( object result )
            {
                this.Result = result;
                return this;
            }

            /// <summary>
            /// Sets the expected exception type for the test
            /// </summary>
            /// <param name="exceptionType">Type of the expected exception.</param>
            /// <returns>The modified TestCaseData instance</returns>
            public TestCaseData Throws(Type exceptionType)
            {
                this.ExpectedException = exceptionType;
                this.ExpectedExceptionName = exceptionType.FullName;
                return this;
            }

            /// <summary>
            /// Sets the expected exception type for the test
            /// </summary>
            /// <param name="exceptionName">FullName of the expected exception.</param>
            /// <returns>The modified TestCaseData instance</returns>
            public TestCaseData Throws(string exceptionName)
            {
                this.ExpectedExceptionName = exceptionName;
                return this;
            }

            /// <summary>
            /// Gets a TestCaseDataHelper to set additional properties
            /// on this instance of TestCaseData
            /// </summary>
            /// <value>A TestCaseDataHelper instance.</value>
            public TestCaseDataHelper With
            {
                get { return new TestCaseDataHelper(this); }
            }

            /// <summary>
            /// TestCaseDataHelper helps construct a TestCaseData
            /// by exposing member names that would otherwise
            /// conflict with those defined on TestCaseData. 
            /// </summary>
            public class TestCaseDataHelper
            {
                private readonly TestCaseData parms;

                /// <summary>
                /// Initializes a new instance of the <see cref="T:TestCaseDataHelper"/> class.
                /// </summary>
                /// <param name="parms">The TestCaseData being constructed.</param>
                public TestCaseDataHelper(TestCaseData parms)
                {
                    this.parms = parms;
                }

                /// <summary>
                /// Provides a non-default name for the TestCaseData
                /// instance being constructe.
                /// </summary>
                /// <param name="testName">Name of the test.</param>
                /// <returns>The modified TestCaseData instance.</returns>
                public TestCaseData Name( string testName )
                {
                    parms.TestName = testName;
                    return parms;
                }

                /// <summary>
                /// Provides a description for the TestCaseData
                /// being constructed.
                /// </summary>
                /// <param name="description">The description.</param>
                /// <returns>The modified TestCaseData instance.</returns>
                public TestCaseData Description( string description )
                {
                    parms.Description = description;
                    return parms;
                }
            }
    }
}
