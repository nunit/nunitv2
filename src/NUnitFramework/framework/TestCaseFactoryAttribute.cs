using System;

namespace NUnit.Framework
{
    /// <summary>
    /// TestFactoryAttribute is used to mark methods, properties and fields 
    /// that provide test cases and to indicate the types of the parameters
    /// that the factory provides. It's use is only necessary when factories
    /// are located automatically rather than by name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class TestCaseFactoryAttribute : Attribute
    {
        private Type[] argTypes;

        /// <summary>
        /// Construct a TestCaseFactoryAttribute with an array of types
        /// </summary>
        /// <param name="argTypes"></param>
        public TestCaseFactoryAttribute(params Type[] argTypes)
        {
            this.argTypes = argTypes;
        }

        /// <summary>
        /// Construct a TestCaseFactoryAttribute with a single type
        /// </summary>
        /// <param name="argType1"></param>
        public TestCaseFactoryAttribute(Type argType1)
        {
            this.argTypes = new Type[] { argType1 };
        }

        /// <summary>
        /// Construct a TestCaseFactoryAttribute with two types
        /// </summary>
        /// <param name="argType1"></param>
        /// <param name="argType2"></param>
        public TestCaseFactoryAttribute(Type argType1, Type argType2)
        {
            this.argTypes = new Type[] { argType1, argType2 };
        }

        /// <summary>
        /// Construct a TestCaseFactoryAttribute with two types
        /// </summary>
        /// <param name="argType1"></param>
        /// <param name="argType2"></param>
        /// <param name="argType3"></param>
        public TestCaseFactoryAttribute(Type argType1, Type argType2, Type argType3)
        {
            this.argTypes = new Type[] { argType1, argType2, argType3 };
        }

        /// <summary>
        /// Get the argument types provided by this factory
        /// </summary>
        public Type[] ArgTypes
        {
            get { return argTypes; }
        }
    }
}
