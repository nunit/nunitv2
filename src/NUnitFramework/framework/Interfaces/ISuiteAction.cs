using System;
using System.Collections.Generic;
using System.Text;

namespace NUnit.Framework
{
    /// <summary>
    /// The interface implemented to provide actions to execute before and after suites.
    /// </summary>
    public interface ISuiteAction : IAction
    {
        /// <summary>
        /// Executed before each suite is run
        /// </summary>
        /// <param name="fixture">The fixture that defines the suite</param>
        void BeforeSuite(object fixture);

        /// <summary>
        /// Executed after each suite is run
        /// </summary>
        /// <param name="fixture">The fixture that defines the suite</param>
        void AfterSuite(object fixture);
    }
}
