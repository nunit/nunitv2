using System;

namespace NUnit.Framework
{
    /// <summary>
    /// DataSourceAttribute indicates the source to be used as a
    /// data provider for a test method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class DataSourceAttribute : Attribute
    {
        private readonly string sourceName;
        private readonly Type sourceType;

        /// <summary>
        /// Construct with the name of a static property
        /// </summary>
        /// <param name="sourceName">The name of the property that will provide data</param>
        public DataSourceAttribute( string sourceName )
        {
            this.sourceName = sourceName;
        }

        /// <summary>
        /// Construct with a Type
        /// </summary>
        /// <param name="sourceType">The Type that will provide data</param>
        public DataSourceAttribute( Type sourceType )
        {
            this.sourceType = sourceType;
        }

        /// <summary>
        /// The name of a the static property to be used as a DataSource
        /// </summary>
        public string SourceName
        {
            get { return sourceName; }   
        }

        /// <summary>
        /// A Type to be used as a DataSource
        /// </summary>
        public Type SourceType
        {
            get { return sourceType;  }
        }
    }
}
