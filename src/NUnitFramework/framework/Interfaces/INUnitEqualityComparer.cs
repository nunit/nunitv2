using System;

namespace NUnit.Framework
{
    /// <summary>
    /// 
    /// </summary>
    public interface INUnitEqualityComparer
    {
        /// <summary>
        /// Compares two objects for equality within a tolerance
        /// </summary>
        /// <param name="x">The first object to compare</param>
        /// <param name="y">The second object to compare</param>
        /// <param name="tolerance">The tolerance to use in the comparison</param>
        /// <returns></returns>
        bool AreEqual(object x, object y, ref NUnit.Framework.Constraints.Tolerance tolerance);
    }

#if CLR_2_0 || CLR_4_0
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface INUnitEqualityComparer<T>
    {
        /// <summary>
        /// Compares two objects of a given Type for equality within a tolerance
        /// </summary>
        /// <param name="x">The first object to compare</param>
        /// <param name="y">The second object to compare</param>
        /// <param name="tolerance">The tolerance to use in the comparison</param>
        /// <returns></returns>
        bool AreEqual(T x, T y, ref NUnit.Framework.Constraints.Tolerance tolerance);
    }
#endif
}
