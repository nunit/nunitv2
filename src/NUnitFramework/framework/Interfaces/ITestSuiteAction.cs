using System;
using System.Collections.Generic;
using System.Text;

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
