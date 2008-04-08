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
        /// <summary>
        /// The argument list to be provided to the test
        /// </summary>
        public object[] Arguments;

        /// <summary>
        /// The expected result to be returned
        /// </summary>
        public object Result;

        /// <summary>
        ///  The expected exception Type
        /// </summary>
        public Type ExpectedException;

        /// <summary>
        /// The FullName of the expected exception
        /// </summary>
        public string ExpectedExceptionName;

        /// <summary>
        /// The name to be used for the test
        /// </summary>
        public string TestName;

        /// <summary>
        /// The description of the test
        /// </summary>
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
        /// Sets the name of the test
        /// </summary>
        /// <returns>The modified TestCaseData instance</returns>
        public TestCaseData WithName(string name)
        {
            this.TestName = name;
            return this;
        }

        /// <summary>
        /// Provides a description for the TestCaseData
        /// being constructed.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns>The modified TestCaseData instance.</returns>
        public TestCaseData WithDescription(string description)
        {
            this.Description = description;
            return this;
        }
    }
}
