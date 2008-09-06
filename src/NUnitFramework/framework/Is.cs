// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using System.Collections;
using NUnit.Framework.Constraints;

namespace NUnit.Framework
{
	/// <summary>
	/// The Is class is a helper class with properties and methods
	/// that supply a number of constraints used in Asserts.
    /// All constraints are actually supplied as ConstraintBuilders,
    /// since the constraint keyword may end up being followed by
    /// another constraint or operator.
	/// </summary>
	public class Is
	{
		#region Prefix Operators
		/// <summary>
		/// Is.Not returns a ConstraintBuilder that negates
		/// the constraint that follows it.
		/// </summary>
		public static PartialConstraintExpression Not
		{
            get { return new PartialConstraintExpression().Not; }
		}

		/// <summary>
		/// Is.All returns a ConstraintBuilder, which will apply
		/// the following constraint to all members of a collection,
		/// succeeding if all of them succeed.
		/// </summary>
		public static PartialConstraintExpression All
		{
			get { return new PartialConstraintExpression().All; }
		}
		#endregion

		#region Constraints Without Arguments
		/// <summary>
		/// Is.Null returns a constraint that tests for null
		/// </summary>
        public static ConstraintExpression Null
        {
            get { return new PartialConstraintExpression().Null; }
        }

		/// <summary>
		/// Is.True returns a constraint that tests whether a value is true
		/// </summary>
        public static ConstraintExpression True
        {
            get { return new PartialConstraintExpression().True; }
        }

		/// <summary>
		/// Is.False returns a constraint that tests whether a value is false
		/// </summary>
        public static ConstraintExpression False
        {
            get { return new PartialConstraintExpression().False; }
        }

		/// <summary>
		/// Is.NaN returns a static constraint that tests whether a value is an NaN
		/// </summary>
        public static ConstraintExpression NaN
        {
            get { return new PartialConstraintExpression().NaN; }
        }

		/// <summary>
		/// Is.Empty returns a static constraint that tests whether a string or collection is empty
		/// </summary>
        public static ConstraintExpression Empty
        {
            get { return new PartialConstraintExpression().Empty; }
        }

        /// <summary>
        /// Is.Unique returns a static constraint that tests whether a collection contains all unque items.
        /// </summary>
        public static ConstraintExpression Unique
        {
            get { return new PartialConstraintExpression().Unique; }
        }

        #endregion

        #region Constraints with an expected value

        #region Equality and Identity
        /// <summary>
        /// Is.EqualTo returns a constraint that tests whether the
        /// actual value equals the supplied argument
        /// </summary>
        /// <param name="expected"></param>
        /// <returns></returns>
        public static EqualConstraint.Modifier EqualTo(object expected)
        {
            return new PartialConstraintExpression().EqualTo(expected);
        }
		/// <summary>
		/// Is.SameAs returns a constraint that tests whether the
		/// actual value is the same object as the supplied argument.
		/// </summary>
		/// <param name="expected"></param>
		/// <returns></returns>
        public static ConstraintExpression SameAs(object expected)
        {
            return new PartialConstraintExpression().SameAs(expected);
        }
        #endregion

        #region Comparison Constraints
		/// <summary>
		/// Is.GreaterThan returns a constraint that tests whether the
		/// actual value is greater than the suppled argument
		/// </summary>
		public static ConstraintExpression GreaterThan(IComparable expected)
        {
            return new PartialConstraintExpression().GreaterThan(expected);
        }
		/// <summary>
		/// Is.GreaterThanOrEqualTo returns a constraint that tests whether the
		/// actual value is greater than or equal to the suppled argument
		/// </summary>
        public static ConstraintExpression GreaterThanOrEqualTo(IComparable expected)
        {
            return new PartialConstraintExpression().GreaterThanOrEqualTo(expected);
        }

		/// <summary>
		/// Is.AtLeast is a synonym for Is.GreaterThanOrEqualTo
		/// </summary>
        public static ConstraintExpression AtLeast(IComparable expected)
        {
            return GreaterThanOrEqualTo(expected);
        }

		/// <summary>
		/// Is.LessThan returns a constraint that tests whether the
		/// actual value is less than the suppled argument
		/// </summary>
        public static ConstraintExpression LessThan(IComparable expected)
        {
            return new PartialConstraintExpression().LessThan(expected);
        }

		/// <summary>
		/// Is.LessThanOrEqualTo returns a constraint that tests whether the
		/// actual value is less than or equal to the suppled argument
		/// </summary>
        public static ConstraintExpression LessThanOrEqualTo(IComparable expected)
        {
            return new PartialConstraintExpression().LessThanOrEqualTo(expected);
        }

		/// <summary>
		/// Is.AtMost is a synonym for Is.LessThanOrEqualTo
		/// </summary>
        public static ConstraintExpression AtMost(IComparable expected)
        {
            return LessThanOrEqualTo(expected);
        }
        #endregion

		#region Type Constraints
        /// <summary>
        /// Is.TypeOf returns a constraint that tests whether the actual
        /// value is of the exact type supplied as an argument.
        /// </summary>
        /// <param name="expectedType">The type to be tested for</param>
        public static ConstraintExpression TypeOf(Type expectedType)
        {
            return new PartialConstraintExpression().TypeOf(expectedType);
        }

#if NET_2_0
        /// <summary>
        /// Is.TypeOf returns a constraint that tests whether the actual
        /// value is of the exact type supplied as an argument.
        /// </summary>
        /// <typeparam name="T">Type to be tested for</typeparam>
        public static ConstraintExpression TypeOf<T>()
        {
            return TypeOf( typeof(T) );
        }
#endif

        /// <summary>
        /// Is.InstanceOfType returns a constraint that tests whether 
        /// the actual value is of the type supplied as an argument
        /// or a derived type.
        /// </summary>
        /// <param name="expectedType">The type to be tested for</param>
        [Obsolete("Use InstanceOf")]
        public static ConstraintExpression InstanceOfType(Type expectedType)
        {
            return new PartialConstraintExpression().InstanceOfType(expectedType);
        }

        /// <summary>
        /// Is.InstanceOf returns a constraint that tests whether 
        /// the actual value is of the type supplied as an argument
        /// or a derived type.
        /// </summary>
        /// <param name="expectedType">The type to be tested for</param>
        public static ConstraintExpression InstanceOf(Type expectedType)
        {
            return new PartialConstraintExpression().InstanceOf(expectedType);
        }

#if NET_2_0
        /// <summary>
        /// Is.InstanceOfType returns a constraint that tests whether 
        /// the actual value is of the type supplied as an argument
        /// or a derived type.
        /// </summary>
        /// <typeparam name="T">The type to be tested for</typeparam>
        [Obsolete("Use InstanceOf")]
        public static ConstraintExpression InstanceOfType<T>()
        {
            return InstanceOfType(typeof(T));
        }

        /// <summary>
        /// Is.InstanceOf returns a constraint that tests whether 
        /// the actual value is of the type supplied as an argument
        /// or a derived type.
        /// </summary>
        /// <typeparam name="T">The type to be tested for</typeparam>
        public static ConstraintExpression InstanceOf<T>()
        {
            return InstanceOf(typeof(T));
        }
#endif

        /// <summary>
        /// Is.AssignableFrom returns a constraint that tests whether
        /// the actual value is assignable from the type supplied as
        /// an argument.
        /// </summary>
        /// <param name="expectedType"></param>
        /// <returns></returns>
        public static ConstraintExpression AssignableFrom(Type expectedType)
        {
            return new PartialConstraintExpression().AssignableFrom(expectedType);
        }

#if NET_2_0
        /// <summary>
        /// Is.AssignableFrom returns a constraint that tests whether
        /// the actual value is assignable from the type supplied as
        /// an argument.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ConstraintExpression AssignableFrom<T>()
        {
            return AssignableFrom(typeof(T));
        }
#endif

        /// <summary>
        /// Is.AssignableTo returns a constraint that tests whether
        /// the actual value is assignable to the type supplied as
        /// an argument.
        /// </summary>
        /// <param name="expectedType"></param>
        /// <returns></returns>
        public static ConstraintExpression AssignableTo(Type expectedType)
        {
            return new PartialConstraintExpression().AssignableTo(expectedType);
        }

#if NET_2_0
        /// <summary>
        /// Is.AssignableTo returns a constraint that tests whether
        /// the actual value is assignable to the type supplied as
        /// an argument.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ConstraintExpression AssignableTo<T>()
        {
            return AssignableTo(typeof(T));
        }
#endif
        #endregion

		#region Collection Constraints
		/// <summary>
		/// Is.EquivalentTo returns a constraint that tests whether
		/// the actual value is a collection containing the same
		/// elements as the collection supplied as an arument
		/// </summary>
		public static ConstraintExpression EquivalentTo(ICollection expected)
        {
            return new PartialConstraintExpression().EquivalentTo(expected);
        }

		/// <summary>
		/// Is.SubsetOf returns a constraint that tests whether
		/// the actual value is a subset of the collection 
		/// supplied as an arument
		/// </summary>
		public static ConstraintExpression SubsetOf(ICollection expected)
        {
            return new PartialConstraintExpression().SubsetOf(expected);
        }
        #endregion

		#region Path Constraints
        /// <summary>
        /// Is.SamePath returns a constraint that tests whether
        /// the path provided is the same as an expected path
        /// after canonicalization.
        /// </summary>
        /// <param name="expected">The expected path</param>
        /// <returns>True if the paths are the same, otherwise false</returns>
        public static SamePathConstraint.Modifier SamePath(string expected)
		{
			return new PartialConstraintExpression().SamePath( expected );
		}

        /// <summary>
        /// Is.SamePathOrUnder returns a constraint that tests whether
        /// the path provided is the same as or under an expected path
        /// after canonicalization.
        /// </summary>
        /// <param name="expected">The expected path</param>
        /// <returns>True if the path is the same as or a subpath of the expected path, otherwise false</returns>
        public static SamePathOrUnderConstraint.Modifier SamePathOrUnder(string expected)
		{
			return new PartialConstraintExpression().SamePathOrUnder( expected );
		}

        /// <summary>
        /// Is.Ordered returns a constraint that tests whether
        /// a collection is ordered
        /// </summary>
        public static ConstraintExpression Ordered()
        {
            return new PartialConstraintExpression().Ordered();
        }

        /// <summary>
        /// Is.Ordered returns a constraint that tests whether
        /// a collection is ordered
        /// </summary>
        /// <param name="comparer">A custom comparer to be used to comparison</param>
        public static ConstraintExpression Ordered(IComparer comparer)
        {
            return new PartialConstraintExpression().Ordered(comparer);
        }
        #endregion

        #region Range Constraints
        public static ConstraintExpression InRange(IComparable from, IComparable to)
        {
            return new PartialConstraintExpression().InRange(from, to);
        }
        #endregion

        #endregion
    }


	/// <summary>
	/// The Iz class is a synonym for Is intended for use in VB,
	/// which regards Is as a keyword.
	/// </summary>
	public class Iz : Is
	{
	}
}
