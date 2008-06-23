using System;
using NUnit.Framework.Constraints;

namespace NUnit.Framework.Syntax.CSharp
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
        public static ThrowsConstraint Exception(Type type)
		{
			return new ThrowsConstraint( type );
		}

        /// <summary>
        /// Creates a constraint specifying the type of exception expected
        /// </summary>
        /// <param name="type">The expected type of exception.</param>
        /// <param name="constraint">A further constraint on the exception</param>
        /// <returns>A ThrowsConstraint</returns>
        public static ThrowsConstraint Exception(Type type, Constraint constraint)
		{
			return new ThrowsConstraint(type, constraint);
		}

#if NET_2_0
        /// <summary>
        /// Creates a constraint specifying the type of exception expected
        /// </summary>
        /// <typeparam name="T">The expected type of exception.</typeparam>
        /// <returns>A ThrowsConstraint</returns>
        public static ThrowsConstraint Exception<T>()
        {
            return new ThrowsConstraint(typeof(T));
        }

        /// <summary>
        /// Creates a constraint specifying the type of exception expected
        /// </summary>
        /// <typeparam name="T">The expected type of exception.</typeparam>
        /// <param name="constraint">A further constraint on the exception</param>
        /// <returns>A ThrowsConstraint</returns>
        public static ThrowsConstraint Exception<T>(Constraint constraint)
        {
            return new ThrowsConstraint(typeof(T), constraint );
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
