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
	/// </summary>
	public class Is
	{
		#region Prefix Operators
		/// <summary>
		/// Is.Not returns a ConstraintBuilder that negates
		/// the constraint that follows it.
		/// </summary>
		public static ConstraintBuilder Not
		{
			get { return new ConstraintBuilder().Not; }
		}

		/// <summary>
		/// Is.All returns a ConstraintBuilder, which will apply
		/// the following constraint to all members of a collection,
		/// succeeding if all of them succeed.
		/// </summary>
		public static ConstraintBuilder All
		{
			get { return new ConstraintBuilder().All; }
		}
		#endregion

		#region Constraints Without Arguments
		/// <summary>
		/// Is.Null returns a constraint that tests for null
		/// </summary>
        public static Constraint Null
        {
            get { return new EqualConstraint(null); }
        }

		/// <summary>
		/// Is.True returns a constraint that tests whether a value is true
		/// </summary>
        public static Constraint True
        {
            get { return new EqualConstraint(true); }
        }

		/// <summary>
		/// Is.False returns a constraint that tests whether a value is false
		/// </summary>
        public static Constraint False
        {
            get { return new EqualConstraint(false); }
        }

		/// <summary>
		/// Is.NaN returns a static constraint that tests whether a value is an NaN
		/// </summary>
        public static Constraint NaN
        {
            get { return new EqualConstraint(double.NaN); }
        }

		/// <summary>
		/// Is.Empty returns a static constraint that tests whether a string or collection is empty
		/// </summary>
        public static EmptyConstraint Empty
        {
            get { return new EmptyConstraint(); }
        }

        /// <summary>
        /// Is.Unique returns a static constraint that tests whether a collection contains all unque items.
        /// </summary>
        public static UniqueItemsConstraint Unique
        {
            get { return new UniqueItemsConstraint(); }
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
        public static EqualConstraint EqualTo(object expected)
        {
            return new EqualConstraint(expected);
        }
		/// <summary>
		/// Is.SameAs returns a constraint that tests whether the
		/// actual value is the same object as the supplied argument.
		/// </summary>
		/// <param name="expected"></param>
		/// <returns></returns>
        public static Constraint SameAs(object expected)
        {
            return new SameAsConstraint(expected);
        }
        #endregion

        #region Comparison Constraints
		/// <summary>
		/// Is.GreaterThan returns a constraint that tests whether the
		/// actual value is greater than the suppled argument
		/// </summary>
		public static Constraint GreaterThan(IComparable expected)
        {
            return new GreaterThanConstraint(expected);
        }
		/// <summary>
		/// Is.GreaterThanOrEqualTo returns a constraint that tests whether the
		/// actual value is greater than or equal to the suppled argument
		/// </summary>
        public static Constraint GreaterThanOrEqualTo(IComparable expected)
        {
            return new GreaterThanOrEqualConstraint(expected);
        }

		/// <summary>
		/// Is.AtLeast is a synonym for Is.GreaterThanOrEqualTo
		/// </summary>
        public static Constraint AtLeast(IComparable expected)
        {
            return GreaterThanOrEqualTo(expected);
        }

		/// <summary>
		/// Is.LessThan returns a constraint that tests whether the
		/// actual value is less than the suppled argument
		/// </summary>
        public static Constraint LessThan(IComparable expected)
        {
            return new LessThanConstraint(expected);
        }

		/// <summary>
		/// Is.LessThanOrEqualTo returns a constraint that tests whether the
		/// actual value is less than or equal to the suppled argument
		/// </summary>
        public static Constraint LessThanOrEqualTo(IComparable expected)
        {
            return new LessThanOrEqualConstraint(expected);
        }

		/// <summary>
		/// Is.AtMost is a synonym for Is.LessThanOrEqualTo
		/// </summary>
        public static Constraint AtMost(IComparable expected)
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
        public static Constraint TypeOf(Type expectedType)
        {
            return new ExactTypeConstraint(expectedType);
        }

#if NET_2_0
        /// <summary>
        /// Is.TypeOf returns a constraint that tests whether the actual
        /// value is of the exact type supplied as an argument.
        /// </summary>
        /// <typeparam name="T">Type to be tested for</typeparam>
        public static Constraint TypeOf<T>()
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
        public static Constraint InstanceOfType(Type expectedType)
        {
            return new InstanceOfTypeConstraint(expectedType);
        }

#if NET_2_0
        /// <summary>
        /// Is.InstanceOfType returns a constraint that tests whether 
        /// the actual value is of the type supplied as an argument
        /// or a derived type.
        /// </summary>
        /// <typeparam name="T">The type to be tested for</typeparam>
        public static Constraint InstanceOfType<T>()
        {
            return InstanceOfType(typeof(T));
        }
#endif

        /// <summary>
        /// Is.AssignableFrom returns a constraint that tests whether
        /// the actual value is assignable from the type supplied as
        /// an argument.
        /// </summary>
        /// <param name="expectedType"></param>
        /// <returns></returns>
        public static Constraint AssignableFrom(Type expectedType)
        {
            return new AssignableFromConstraint(expectedType);
        }

#if NET_2_0
        /// <summary>
        /// Is.AssignableFrom returns a constraint that tests whether
        /// the actual value is assignable from the type supplied as
        /// an argument.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Constraint AssignableFrom<T>()
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
        public static Constraint AssignableTo(Type expectedType)
        {
            return new AssignableToConstraint(expectedType);
        }

#if NET_2_0
        /// <summary>
        /// Is.AssignableTo returns a constraint that tests whether
        /// the actual value is assignable to the type supplied as
        /// an argument.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Constraint AssignableTo<T>()
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
		public static Constraint EquivalentTo(ICollection expected)
        {
            return new CollectionEquivalentConstraint(expected);
        }

		/// <summary>
		/// Is.SubsetOf returns a constraint that tests whether
		/// the actual value is a subset of the collection 
		/// supplied as an arument
		/// </summary>
		public static Constraint SubsetOf(ICollection expected)
        {
            return new CollectionSubsetConstraint(expected);
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
        public static SamePathConstraint SamePath(string expected)
		{
			return new SamePathConstraint( expected );
		}

        /// <summary>
        /// Is.SamePathOrUnder returns a constraint that tests whether
        /// the path provided is the same as or under an expected path
        /// after canonicalization.
        /// </summary>
        /// <param name="expected">The expected path</param>
        /// <returns>True if the path is the same as or a subpath of the expected path, otherwise false</returns>
        public static SamePathOrUnderConstraint SamePathOrUnder(string expected)
		{
			return new SamePathOrUnderConstraint( expected );
		}

        /// <summary>
        /// Is.Ordered returns a constraint that tests whether
        /// a collection is ordered
        /// </summary>
        public static Constraint Ordered()
        {
            return new CollectionOrderedConstraint();
        }

        /// <summary>
        /// Is.Ordered returns a constraint that tests whether
        /// a collection is ordered
        /// </summary>
        /// <param name="comparer">A custom comparer to be used to comparison</param>
        public static Constraint Ordered(IComparer comparer)
        {
            return new CollectionOrderedConstraint(comparer);
        }
        #endregion

        #region Range Constraints
        public static RangeConstraint InRange(IComparable from, IComparable to)
        {
            return new RangeConstraint(from, to);
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
