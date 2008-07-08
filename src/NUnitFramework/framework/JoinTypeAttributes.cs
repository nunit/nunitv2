using System;

namespace NUnit.Framework
{
    /// <summary>
    /// Marks a test to use a combinatorial join of any argument 
    /// data provided. Since this is the default, the attribute is
    /// not needed.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CombinatorialAttribute : PropertyAttribute
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public CombinatorialAttribute() : base("_JOINTYPE", "Combinatorial") { }
    }

    /// <summary>
    /// Marks a test to use a combinatorial join of any argument 
    /// data provided. Since this is the default, the attribute is
    /// not needed.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class PairwiseAttribute : PropertyAttribute
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public PairwiseAttribute() : base("_JOINTYPE", "Pairwise") { }
    }

    /// <summary>
    /// Marks a test to use a combinatorial join of any argument 
    /// data provided. Since this is the default, the attribute is
    /// not needed.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class SequentialAttribute : PropertyAttribute
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public SequentialAttribute() : base("_JOINTYPE", "Sequential") { }
    }
}
