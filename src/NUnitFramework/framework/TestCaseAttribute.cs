// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org/?p=license&r=2.4.
// ****************************************************************
using System;

namespace NUnit.Framework
{
    /// <summary>
    /// TestCaseAttribute is used to mark parameterized test cases
    /// and provide them with their arguments.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class TestCaseAttribute : Attribute
    {
        private object[] arguments;
        private object result;
        private Type expectedExceptionType;
        private string expectedExceptionName;
        private string description;
        private string testName;

        /// <summary>
        /// Construct a TestCaseAttribute with a list of arguments.
        /// This constructor is not CLS-Compliant
        /// </summary>
        /// <param name="arguments"></param>
        public TestCaseAttribute(params object[] arguments)
        {
            this.arguments = arguments;
        }

        /// <summary>
        /// Construct a TestCaseAttribute with a single argument
        /// </summary>
        /// <param name="arg"></param>
        public TestCaseAttribute(object arg)
        {
            this.arguments = new object[] { arg };
        }

        /// <summary>
        /// Construct a TestCaseAttribute with a two arguments
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        public TestCaseAttribute(object arg1, object arg2)
        {
            this.arguments = new object[] { arg1, arg2 };
        }

        /// <summary>
        /// Construct a TestCaseAttribute with a three arguments
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        public TestCaseAttribute(object arg1, object arg2, object arg3)
        {
            this.arguments = new object[] { arg1, arg2, arg3 };
        }

        /// <summary>
        /// Gets the list of arguments to a test case
        /// </summary>
        public object[] Arguments
        {
            get { return arguments; }
        }

        public object Result
        {
            get { return result; }
            set { result = value; }
        }

        public Type ExpectedException
        {
            get { return expectedExceptionType;  }
            set
            {
                expectedExceptionType = value;
                expectedExceptionName = expectedExceptionType.FullName;
            }
        }

        public string ExpectedExceptionName
        {
            get { return expectedExceptionName; }
            set
            {
                expectedExceptionName = value;
                expectedExceptionType = null;
            }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string TestName
        {
            get { return testName; }
            set { testName = value; }
        }
    }
}
