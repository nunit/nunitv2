using System;

namespace NUnit.Framework
{
    /// <summary>
    /// FactoryAttribute indicates the source to be used to
    /// provide test cases for a test method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class FactoryAttribute : Attribute
    {
        private readonly string factoryName;
        private readonly Type factoryType;

        /// <summary>
        /// Construct with a name
        /// </summary>
        /// <param name="factoryName">The name of the property that will provide data</param>
        public FactoryAttribute(string factoryName)
        {
            this.factoryName = factoryName;
        }

        /// <summary>
        /// Construct with a Type and name
        /// </summary>
        /// <param name="factoryType">The Type that will provide data</param>
        /// <param name="factoryName">The name of the property that will provide data</param>
        public FactoryAttribute(Type factoryType, string factoryName)
        {
            this.factoryType = factoryType;
            this.factoryName = factoryName;
        }

        /// <summary>
        /// The name of a the static property to be used as a DataSource
        /// </summary>
        public string FactoryName
        {
            get { return factoryName; }   
        }

        /// <summary>
        /// A Type to be used as a DataSource
        /// </summary>
        public Type FactoryType
        {
            get { return factoryType;  }
        }
    }
}
