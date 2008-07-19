using System;

namespace NUnit.Framework
{
    /// <summary>
    /// FactoryAttribute indicates the source to be used to
    /// provide test cases for a test method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class FactoriesAttribute : Attribute
    {
        private readonly string[] factoryNames;
        private readonly Type factoryType;

        /// <summary>
        /// Construct with the name of the factory - for use with languages
        /// that don't support params arrays.
        /// </summary>
        /// <param name="factoryName">An array of the names of the factories that will provide data</param>
        public FactoriesAttribute(string factoryName)
        {
            this.factoryNames = new string[] { factoryName };
        }

        /// <summary>
        /// Construct with the names of multiple factories
        /// </summary>
        /// <param name="name1">Name of the first factory</param>
        /// <param name="moreNames">An array of additional names</param>
        public FactoriesAttribute(string name1, params string[] moreNames)
        {
            this.factoryNames = new string[1 + moreNames.Length];
            this.factoryNames[0] = name1;
            int index = 1;
            foreach (string name in moreNames)
                this.factoryNames[index++] = name;
        }

        /// <summary>
        /// Construct with a Type and name - for use with languages
        /// that don't support params arrays.
        /// </summary>
        /// <param name="factoryType">The Type that will provide data</param>
        /// <param name="factoryName">The name of the method, property or field that will provide data</param>
        public FactoriesAttribute(Type factoryType, string factoryName)
        {
            this.factoryType = factoryType;
            this.factoryNames = new string[] { factoryName };
        }

        /// <summary>
        /// Construct with a Type and multiple names - the first name is
        /// given separately because it is required.
        /// </summary>
        /// <param name="factoryType">The Type that will provide data</param>
        /// <param name="name1">The name of the first factory</param>
        /// <param name="moreNames">Additional factory names, if any</param>
        public FactoriesAttribute(Type factoryType, string name1, params string[] moreNames)
        {
            this.factoryType = factoryType;
            this.factoryNames = new string[1 + moreNames.Length];
            this.factoryNames[0] = name1;
            int index = 1;
            foreach (string name in moreNames)
                this.factoryNames[index++] = name;
        }

        #region Protected Constructors
        /// <summary>
        /// Default constructor - provided for use by derived class 
        /// UseCompatibleFactoriesAttribute.
        /// </summary>
        protected FactoriesAttribute() { }

        /// <summary>
        /// Construct with a Type that contains the factory - provided
        /// for use by derived class UseCompatibleFactoriesAttribute.
        /// </summary>
        /// <param name="factoryType">The Type that will provide data</param>
        protected FactoriesAttribute(Type factoryType)
        {
            this.factoryType = factoryType;
        }
        #endregion

        /// <summary>
        /// The name of a the method, property or fiend to be used as a source
        /// </summary>
        public string[] FactoryNames
        {
            get { return factoryNames; }   
        }

        /// <summary>
        /// A Type to be used as a source
        /// </summary>
        public Type FactoryType
        {
            get { return factoryType;  }
        }
    }
    
    /// <summary>
    /// The UseCompatibleFactoriesAttribute is derived from FactoriesAttribute
    /// and provides constructors that do not specify the factory names to be
    /// used. This gives a clearer indication that NUnit will use any factories
    /// it is able to find as compared to using Factories without any arguments
    /// as was done in the first version of this feature.
    /// </summary>
    public class UseCompatibleFactoriesAttribute : FactoriesAttribute
    {
		/// <summary>
		/// Default Constructor
		/// </summary>
        public UseCompatibleFactoriesAttribute() { }

		/// <summary>
		///  Construct with the type to be examined to locate factories
		/// </summary>
		/// <param name="type"></param>
        public UseCompatibleFactoriesAttribute(Type type) : base(type) { }
    }
}
