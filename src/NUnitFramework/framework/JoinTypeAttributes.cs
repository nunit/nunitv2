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
        public SequentialAttribute() : base("_JOINTYPE", "Sequential") { }
    }
}
