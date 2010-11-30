using System;
using System.Collections.Generic;
using System.Text;

namespace NUnit.Framework
{
    ///<summary>
    /// Base class used to introduce reusable behaviors into tests
    ///</summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public abstract class BehaviorAttribute : Attribute
    {
        private int priority = 0;

        /// <summary>
        /// Called once before any tests within a fixture are run
        /// </summary>
        public virtual void BeforeTestFixture(object fixture) { }

        /// <summary>
        /// Called once after all tests within a fixture are run
        /// </summary>
        public virtual void AfterTestFixture(object fixture) { }

        /// <summary>
        /// Called once before each test is run
        /// </summary>
        public virtual void BeforeTest(object fixture) { }

        /// <summary>
        /// Called after a test is run
        /// </summary>
        public virtual void AfterTest(object fixture) { }

        /// <summary>
        /// Indicates the priority of this behavior. Used to order execution of behaviors - higher number priorities execute first. Default is zero (0).
        /// </summary>
        public int Priority { get { return priority; } set { priority = value; } }
    }
}
