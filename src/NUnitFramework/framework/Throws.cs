using System;
using NUnit.Framework.Constraints;

namespace NUnit.Framework
{
    /// <summary>
    /// Throws is a syntax helper that allows capturing and testing
    /// an exception at the point where it is thrown.
    /// </summary>
	public class Throws
	{
        /// <summary>
        /// Creates a constraint specifying the type of exception expected
        /// </summary>
        /// <param name="type">The expected type of exception.</param>
        /// <returns>A ThrowsConstraint</returns>
        public static ResolvableConstraintBuilder Exception(Type type)
		{
			return new ConstraintBuilder(new ThrowsOperator()).TypeOf( type );
		}

#if NET_2_0
        /// <summary>
        /// Creates a constraint specifying the type of exception expected
        /// </summary>
        /// <typeparam name="T">The expected type of exception.</typeparam>
        /// <returns>A ResolvableConstraintBuilder</returns>
        public static ResolvableConstraintBuilder Exception<T>()
        {
            return Exception(typeof(T));
        }
#endif

        /// <summary>
        /// Creates a constraint to test that no exception is thrown.
        /// </summary>
		public static ThrowsNothingConstraint Nothing
		{
			get { return new ThrowsNothingConstraint(); }
		}
    }
}
