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
        /// Default constructor
        /// </summary>
        public FactoryAttribute()
        {
        }

        /// <summary>
        /// Construct with the name of the factory
        /// </summary>
        /// <param name="factoryName">The name of the property that will provide data</param>
        public FactoryAttribute(string factoryName)
        {
            this.factoryName = factoryName;
        }

        /// <summary>
        /// Construct with a Type that contains the factory
        /// </summary>
        /// <param name="factoryType">The Type that will provide data</param>
        public FactoryAttribute(Type factoryType)
        {
            this.factoryType = factoryType;
        }

        /// <summary>
        /// Construct with a Type and name
        /// </summary>
        /// <param name="factoryType">The Type that will provide data</param>
        /// <param name="factoryName">The name of the method, property or field that will provide data</param>
        public FactoryAttribute(Type factoryType, string factoryName)
        {
            this.factoryType = factoryType;
            this.factoryName = factoryName;
        }

        /// <summary>
        /// The name of a the method, property or fiend to be used as a source
        /// </summary>
        public string FactoryName
        {
            get { return factoryName; }   
        }

        /// <summary>
        /// A Type to be used as a source
        /// </summary>
        public Type FactoryType
        {
            get { return factoryType;  }
        }
    }
}
