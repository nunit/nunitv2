// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

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
        /// Creates a constraint specifying an expected exception
        /// </summary>
        /// <returns>A ThrowsConstraintExpression</returns>
        public static ThrowsConstraintExpression Exception
        {
            get { return new ThrowsConstraintExpression(); }
        }

        /// <summary>
        /// Creates a constraint specifying the type of exception expected
        /// </summary>
        /// <param name="type">The expected type of exception.</param>
        /// <returns>A ThrowsConstraint</returns>
        public static ExactTypeConstraint TypeOf(Type type)
        {
            return new ConstraintExpression().Append(new ThrowsOperator()).TypeOf(type);
        }

#if NET_2_0
        /// <summary>
        /// Creates a constraint specifying the type of exception expected
        /// </summary>
        /// <typeparam name="T">The expected type of exception.</typeparam>
        /// <returns>A ResolvableConstraintBuilder</returns>
        public static ExactTypeConstraint TypeOf<T>()
        {
            return TypeOf(typeof(T));
        }
#endif

        /// <summary>
        /// Creates a constraint specifying the type of exception expected
        /// </summary>
        /// <param name="type">The expected type of exception.</param>
        /// <returns>A ThrowsConstraint</returns>
        public static InstanceOfTypeConstraint InstanceOf(Type type)
        {
            return new ConstraintExpression().Append(new ThrowsOperator()).InstanceOfType(type);
        }

#if NET_2_0
        /// <summary>
        /// Creates a constraint specifying the type of exception expected
        /// </summary>
        /// <typeparam name="T">The expected type of exception.</typeparam>
        /// <returns>A ResolvableConstraintBuilder</returns>
        public static InstanceOfTypeConstraint InstanceOf<T>()
        {
            return InstanceOf(typeof(T));
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
