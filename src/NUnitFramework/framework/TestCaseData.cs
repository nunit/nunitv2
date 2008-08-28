using System;

namespace NUnit.Framework
{
    /// <summary>
    /// The TestCaseData class represents a set of arguments
    /// and other parameter info to be used for a parameterized
    /// test case. It provides a number of instance modifiers
    /// for use in initializing the test case.
    /// 
    /// Note: Instance modifiers are getters that return
    /// the same instance after modifying it's state.
    /// </summary>
    public class TestCaseData : ITestCaseData
    {
        #region Instance Fields
        /// <summary>
        /// The argument list to be provided to the test
        /// </summary>
        private object[] arguments;

        /// <summary>
        /// The expected result to be returned
        /// </summary>
        private object result;

        /// <summary>
        ///  The expected exception Type
        /// </summary>
        private Type expectedException;

        /// <summary>
        /// The FullName of the expected exception
        /// </summary>
        private string expectedExceptionName;

        /// <summary>
        /// The name to be used for the test
        /// </summary>
        private string testName;

        /// <summary>
        /// The description of the test
        /// </summary>
        private string description;
        #endregion

        #region Construction and Initialization
        /// <summary>
        /// Initializes a new instance of the <see cref="T:TestCaseData"/> class.
        /// </summary>
        /// <param name="arg1">The first argument</param>
        /// <param name="args">The remaining arguments.</param>
        public TestCaseData(object arg1, params object[] args)
        {
            this.arguments = new object[args.Length + 1];
            arguments[0] = arg1;
            int index = 1;
            foreach (object arg in args)
                arguments[index++] = arg;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TestCaseData"/> class.
        /// </summary>
        /// <param name="arg">The argument.</param>
        public TestCaseData(object arg)
        {
            this.arguments = new object[] { arg };
        }

        #region Instance Modifiers
        /// <summary>
        /// Sets the expected result for the test
        /// </summary>
        /// <param name="result">The expected result</param>
        /// <returns>A modified TestCaseData</returns>
        public TestCaseData Returns( object result )
        {
            this.result = result;
            return this;
        }

        /// <summary>
        /// Sets the expected exception type for the test
        /// </summary>
        /// <param name="exceptionType">Type of the expected exception.</param>
        /// <returns>The modified TestCaseData instance</returns>
        public TestCaseData Throws(Type exceptionType)
        {
            this.expectedException = exceptionType;
            this.expectedExceptionName = exceptionType.FullName;
            return this;
        }

        /// <summary>
        /// Sets the expected exception type for the test
        /// </summary>
        /// <param name="exceptionName">FullName of the expected exception.</param>
        /// <returns>The modified TestCaseData instance</returns>
        public TestCaseData Throws(string exceptionName)
        {
            this.expectedExceptionName = exceptionName;
            return this;
        }

        /// <summary>
        /// Sets the name of the test
        /// </summary>
        /// <returns>The modified TestCaseData instance</returns>
        public TestCaseData WithName(string name)
        {
            this.testName = name;
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
            this.description = description;
            return this;
        }
        #endregion

        #endregion

        #region ITestCaseData Members
        /// <summary>
        /// Gets the argument list to be provided to the test
        /// </summary>
        public object[] Arguments
        {
            get { return arguments; }
        }

        /// <summary>
        /// Gets the expected result
        /// </summary>
        public object Result
        {
            get { return result; }
        }

        /// <summary>
        ///  Gets the expected exception Type
        /// </summary>
        public Type ExpectedException
        {
            get { return expectedException; }
        }

        /// <summary>
        /// Gets the FullName of the expected exception
        /// </summary>
        public string ExpectedExceptionName
        {
            get { return expectedExceptionName; }
        }

        /// <summary>
        /// Gets the name to be used for the test
        /// </summary>
        public string TestName
        {
            get { return testName; }
        }

        /// <summary>
        /// Gets the description of the test
        /// </summary>
        public string Description
        {
            get { return description; }
        }
        #endregion
    }
}
