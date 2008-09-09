using System;

namespace NUnit.Framework
{
    /// <summary>
    /// DataSourceAttribute indicates the source to be used to
    /// provide data for one parameter of a test method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true, Inherited = false)]
    public class DataSourceAttribute : Attribute
    {
        private readonly string sourceName;
        private readonly Type sourceType;

        /// <summary>
        /// Construct with the name of the factory - for use with languages
        /// that don't support params arrays.
        /// </summary>
        /// <param name="sourceName">The name of the data source to be used</param>
        public DataSourceAttribute(string sourceName)
        {
            this.sourceName = sourceName;
        }

        /// <summary>
        /// Construct with the names of multiple factories
        /// </summary>
        /// <param name="name1">Name of the first data source</param>
        /// <param name="moreNames">An array of additional names</param>
        //public DataSourceAttribute(string name1, params string[] moreNames)
        //{
        //    this.sourceNames = new string[1 + moreNames.Length];
        //    this.sourceNames[0] = name1;
        //    int index = 1;
        //    foreach (string name in moreNames)
        //        this.sourceNames[index++] = name;
        //}

        /// <summary>
        /// Construct with a Type and name - for use with languages
        /// that don't support params arrays.
        /// </summary>
        /// <param name="sourceType">The Type that will provide data</param>
        /// <param name="sourceName">The name of the method, property or field that will provide data</param>
        public DataSourceAttribute(Type sourceType, string sourceName)
        {
            this.sourceType = sourceType;
            this.sourceName = sourceName;
        }

        /// <summary>
        /// Construct with a Type and multiple names - the first name is
        /// given separately because it is required.
        /// </summary>
        /// <param name="sourceType">The Type that will provide data</param>
        /// <param name="name1">The name of the first factory</param>
        /// <param name="moreNames">Additional factory names, if any</param>
        //public DataSourceAttribute(Type sourceType, string name1, params string[] moreNames)
        //{
        //    this.sourceType = sourceType;
        //    this.sourceNames = new string[1 + moreNames.Length];
        //    this.sourceNames[0] = name1;
        //    int index = 1;
        //    foreach (string name in moreNames)
        //        this.sourceNames[index++] = name;
        //}

        /// <summary>
        /// The name of a the method, property or fiend to be used as a source
        /// </summary>
        public string SourceName
        {
            get { return sourceName; }
        }

        /// <summary>
        /// A Type to be used as a source
        /// </summary>
        public Type SourceType
        {
            get { return sourceType; }
        }
    }
}
