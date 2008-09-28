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
		public static ConstraintExpression Not
		{
            get { return new ConstraintExpression().Not; }
		}

		/// <summary>
		/// Is.All returns a ConstraintBuilder, which will apply
		/// the following constraint to all members of a collection,
		/// succeeding if all of them succeed.
		/// </summary>
		public static ConstraintExpression All
		{
			get { return new ConstraintExpression().All; }
		}

		#endregion

		#region Constraints Without Arguments

		/// <summary>
		/// Is.Null returns a constraint that tests for null
		/// </summary>
        public static NullConstraint Null
        {
            get { return new NullConstraint(); }
        }

		/// <summary>
		/// Is.True returns a constraint that tests whether a value is true
		/// </summary>
        public static TrueConstraint True
        {
            get { return new TrueConstraint(); }
        }

		/// <summary>
		/// Is.False returns a constraint that tests whether a value is false
		/// </summary>
        public static FalseConstraint False
        {
            get { return new FalseConstraint(); }
        }

		/// <summary>
		/// Is.NaN returns a static constraint that tests whether a value is an NaN
		/// </summary>
        public static NaNConstraint NaN
        {
            get { return new NaNConstraint(); }
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

        /// <summary>
        /// Is.BinarySerializable returns a constraint that tests whether an object graph is serializable in binary format.
        /// </summary>
        public static BinarySerializableConstraint BinarySerializable
        {
            get { return new BinarySerializableConstraint(); }
        }

        /// <summary>
        /// Is.XmlSerializable returns a constraint that tests whether an object graph is serializable in xml format.
        /// </summary>
        public static XmlSerializableConstraint XmlSerializable
        {
            get { return new XmlSerializableConstraint(); }
        }

        #endregion

        #region Constraints with an expected value argument

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
        public static SameAsConstraint SameAs(object expected)
        {
            return new SameAsConstraint(expected);
        }
        #endregion

        #region Comparison Constraints
		/// <summary>
		/// Is.GreaterThan returns a constraint that tests whether the
		/// actual value is greater than the suppled argument
		/// </summary>
		public static GreaterThanConstraint GreaterThan(IComparable expected)
        {
            return new GreaterThanConstraint(expected);
        }
		/// <summary>
		/// Is.GreaterThanOrEqualTo returns a constraint that tests whether the
		/// actual value is greater than or equal to the suppled argument
		/// </summary>
        public static GreaterThanOrEqualConstraint GreaterThanOrEqualTo(IComparable expected)
        {
            return new GreaterThanOrEqualConstraint(expected);
        }

		/// <summary>
		/// Is.AtLeast is a synonym for Is.GreaterThanOrEqualTo
		/// </summary>
        public static GreaterThanOrEqualConstraint AtLeast(IComparable expected)
        {
            return GreaterThanOrEqualTo(expected);
        }

		/// <summary>
		/// Is.LessThan returns a constraint that tests whether the
		/// actual value is less than the suppled argument
		/// </summary>
        public static LessThanConstraint LessThan(IComparable expected)
        {
            return new LessThanConstraint(expected);
        }

		/// <summary>
		/// Is.LessThanOrEqualTo returns a constraint that tests whether the
		/// actual value is less than or equal to the suppled argument
		/// </summary>
        public static LessThanOrEqualConstraint LessThanOrEqualTo(IComparable expected)
        {
            return new LessThanOrEqualConstraint(expected);
        }

		/// <summary>
		/// Is.AtMost is a synonym for Is.LessThanOrEqualTo
		/// </summary>
        public static LessThanOrEqualConstraint AtMost(IComparable expected)
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
        public static ExactTypeConstraint TypeOf(Type expectedType)
        {
            return new ExactTypeConstraint(expectedType);
        }

#if NET_2_0
        /// <summary>
        /// Is.TypeOf returns a constraint that tests whether the actual
        /// value is of the exact type supplied as an argument.
        /// </summary>
        /// <typeparam name="T">Type to be tested for</typeparam>
        public static ExactTypeConstraint TypeOf<T>()
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
        public static InstanceOfTypeConstraint InstanceOfType(Type expectedType)
        {
            return new InstanceOfTypeConstraint(expectedType);
        }

        /// <summary>
        /// Is.InstanceOf returns a constraint that tests whether 
        /// the actual value is of the type supplied as an argument
        /// or a derived type.
        /// </summary>
        /// <param name="expectedType">The type to be tested for</param>
        public static InstanceOfTypeConstraint InstanceOf(Type expectedType)
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
        [Obsolete("Use InstanceOf")]
        public static InstanceOfTypeConstraint InstanceOfType<T>()
        {
            return InstanceOfType(typeof(T));
        }

        /// <summary>
        /// Is.InstanceOf returns a constraint that tests whether 
        /// the actual value is of the type supplied as an argument
        /// or a derived type.
        /// </summary>
        /// <typeparam name="T">The type to be tested for</typeparam>
        public static InstanceOfTypeConstraint InstanceOf<T>()
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
        public static AssignableFromConstraint AssignableFrom(Type expectedType)
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
        public static AssignableFromConstraint AssignableFrom<T>()
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
        public static AssignableToConstraint AssignableTo(Type expectedType)
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
        public static AssignableToConstraint AssignableTo<T>()
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
		public static CollectionEquivalentConstraint EquivalentTo(IEnumerable expected)
        {
            return new CollectionEquivalentConstraint(expected);
        }

		/// <summary>
		/// Is.SubsetOf returns a constraint that tests whether
		/// the actual value is a subset of the collection 
		/// supplied as an arument
		/// </summary>
		public static CollectionSubsetConstraint SubsetOf(IEnumerable expected)
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
			return new ConstraintExpression().SamePath( expected );
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
			return new ConstraintExpression().SamePathOrUnder( expected );
		}

        /// <summary>
        /// Is.Ordered returns a constraint that tests whether
        /// a collection is ordered
        /// </summary>
        public static CollectionOrderedConstraint Ordered()
        {
            return new ConstraintExpression().Ordered();
        }

        /// <summary>
        /// Is.Ordered returns a constraint that tests whether
        /// a collection is ordered
        /// </summary>
        /// <param name="comparer">A custom comparer to be used to comparison</param>
        public static CollectionOrderedConstraint Ordered(IComparer comparer)
        {
            return new ConstraintExpression().Ordered(comparer);
        }

        /// <summary>
        /// Is.OrderedBy returns a constraint that tests whether
        /// a collection is ordered by a property
        /// </summary>
        public static CollectionOrderedConstraint OrderedBy(string propertyName)
        {
            return new ConstraintExpression().OrderedBy(propertyName);
        }

        /// <summary>
        /// Is.OrderedBy returns a constraint that tests whether
        /// a collection is ordered by a property
        /// </summary>
        /// <param name="comparer">A custom comparer to be used to comparison</param>
        public static CollectionOrderedConstraint OrderedBy(string propertyName, IComparer comparer)
        {
            return new ConstraintExpression().OrderedBy(propertyName, comparer);
        }
        #endregion
        #endregion

        #region Constraints with two arguments
        #region Range Constraint
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
