using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace NUnit.Framework
{
    /// <summary>
    /// The interface implemented to provide actions to execute before and after tests.
    /// </summary>
    public interface ITestAction : IAction
    {
        /// <summary>
        /// Executed before each test is run
        /// </summary>
        /// <param name="fixture">The fixture the test is part of</param>
        /// <param name="method">The method that implements the test</param>
        void BeforeTest(object fixture, MethodInfo method);

        /// <summary>
        /// Executed after each test is run
        /// </summary>
        /// <param name="fixture">The fixture the test is part of</param>
        /// <param name="method">The method that implements the test</param>
        void AfterTest(object fixture, MethodInfo method);
    }
}
