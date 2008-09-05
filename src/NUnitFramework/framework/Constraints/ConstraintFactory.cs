using System;
using System.Collections;
#if NET_2_0
using System.Collections.Generic;
#endif

namespace NUnit.Framework.Constraints
{
    /// <summary>
    /// ConstraintFactory knows how to map key words like "Null"
    /// or "EqualTo" to the appropriate constraints.
    /// </summary>
    public class ConstraintFactory
    {
        #region Prefix Operators
        /// <summary>
        /// Not returns a ConstraintBuilder that negates
        /// the constraint that follows it.
        /// </summary>
        public PartialConstraintExpression Not
        {
            get { return Is.Not; }
        }

        /// <summary>
        /// All returns a ConstraintBuilder, which will apply
        /// the following constraint to all members of a collection,
        /// succeeding if all of them succeed.
        /// </summary>
        public PartialConstraintExpression All
        {
            get { return Is.All; }
        }

        /// <summary>
        /// Some returns a ConstraintBuilder, which will apply
        /// the following constraint to all members of a collection,
        /// succeeding if any of them succeed.
        /// </summary>
        public PartialConstraintExpression Some
        {
            get { return Has.Some; }
        }

        /// <summary>
        /// None returns a ConstraintBuilder, which will apply
        /// the following constraint to all members of a collection,
        /// succeeding only if none of them succeed.
        /// </summary>
        public PartialConstraintExpression None
        {
            get { return Has.None; }
        }

        /// <summary>
        /// Returns a new ConstraintBuilder, which will apply the
        /// following constraint to a named property of the object
        /// being tested.
        /// </summary>
        /// <param name="name">The name of the property</param>
        public PendingConstraintExpression Property(string name)
        {
            return Has.Property(name);
        }

        /// <summary>
        /// Returns a new ConstraintBuilder, which will apply the
        /// following constraint to the Length property of the object
        /// being tested.
        /// </summary>
        public PendingConstraintExpression Length
        {
            get { return Has.Length; }
        }

        /// <summary>
        /// Returns a new ConstraintBuilder, which will apply the
        /// following constraint to the Count property of the object
        /// being tested.
        /// </summary>
        public PendingConstraintExpression Count
        {
            get { return Has.Count; }
        }

        /// <summary>
        /// Returns a new ConstraintBuilder, which will apply the
        /// following constraint to the Message property of the object
        /// being tested.
        /// </summary>
        public PartialConstraintExpression Message
        {
            get { return Has.Message; }
        }

        #endregion

        #region Constraints Without Arguments
        /// <summary>
        /// Null returns a constraint that tests for null
        /// </summary>
        public ConstraintExpression Null
        {
            get { return Is.Null; }
        }

        /// <summary>
        /// True returns a constraint that tests whether a value is true
        /// </summary>
        public ConstraintExpression True
        {
            get { return Is.True; }
        }

        /// <summary>
        /// False returns a constraint that tests whether a value is false
        /// </summary>
        public ConstraintExpression False
        {
            get { return Is.False; }
        }

        /// <summary>
        /// NaN returns a constraint that tests whether a value is an NaN
        /// </summary>
        public ConstraintExpression NaN
        {
            get { return Is.NaN; }
        }

        /// <summary>
        /// Empty returns a constraint that tests whether a string or collection is empty
        /// </summary>
        public ConstraintExpression Empty
        {
            get { return Is.Empty; }
        }

        /// <summary>
        /// Unique returns a constraint that tests whether a collection contains all unque items.
        /// </summary>
        public ConstraintExpression Unique
        {
            get { return Is.Unique; }
        }
        #endregion

        #region Equality and Identity
        /// <summary>
        /// EqualTo returns a constraint that tests whether the
        /// actual value equals the supplied argument
        /// </summary>
        /// <param name="expected"></param>
        /// <returns></returns>
        public EqualConstraint.Modifier EqualTo(object expected)
        {
            return Is.EqualTo(expected);
        }
        /// <summary>
        /// SameAs returns a constraint that tests whether the
        /// actual value is the same object as the supplied argument.
        /// </summary>
        /// <param name="expected"></param>
        /// <returns></returns>
        public ConstraintExpression SameAs(object expected)
        {
            return Is.SameAs(expected);
        }
        #endregion

        #region Comparison Constraints
        /// <summary>
        /// GreaterThan returns a constraint that tests whether the
        /// actual value is greater than the suppled argument
        /// </summary>
        public ConstraintExpression GreaterThan(IComparable expected)
        {
            return Is.GreaterThan(expected);
        }
        /// <summary>
        /// GreaterThanOrEqualTo returns a constraint that tests whether the
        /// actual value is greater than or equal to the suppled argument
        /// </summary>
        public ConstraintExpression GreaterThanOrEqualTo(IComparable expected)
        {
            return Is.GreaterThanOrEqualTo(expected);
        }

        /// <summary>
        /// AtLeast is a synonym for GreaterThanOrEqualTo
        /// </summary>
        public ConstraintExpression AtLeast(IComparable expected)
        {
            return Is.AtLeast(expected);
        }

        /// <summary>
        /// LessThan returns a constraint that tests whether the
        /// actual value is less than the suppled argument
        /// </summary>
        public ConstraintExpression LessThan(IComparable expected)
        {
            return Is.LessThan(expected);
        }

        /// <summary>
        /// LessThanOrEqualTo returns a constraint that tests whether the
        /// actual value is less than or equal to the suppled argument
        /// </summary>
        public ConstraintExpression LessThanOrEqualTo(IComparable expected)
        {
            return Is.LessThanOrEqualTo(expected);
        }

        /// <summary>
        /// AtMost is a synonym for LessThanOrEqualTo
        /// </summary>
        public ConstraintExpression AtMost(IComparable expected)
        {
            return Is.AtMost(expected);
        }
        #endregion

        #region Type Constraints
        /// <summary>
        /// TypeOf returns a constraint that tests whether the actual
        /// value is of the exact type supplied as an argument.
        /// </summary>
        /// <param name="expectedType">The type to be tested for</param>
        public ConstraintExpression TypeOf(Type expectedType)
        {
            return Is.TypeOf(expectedType);
        }

#if NET_2_0
        /// <summary>
        /// TypeOf returns a constraint that tests whether the actual
        /// value is of the exact type supplied as an argument.
        /// </summary>
        /// <typeparam name="T">Type to be tested for</typeparam>
        public ConstraintExpression TypeOf<T>()
        {
            return TypeOf(typeof(T));
        }
#endif

        /// <summary>
        /// InstanceOfType returns a constraint that tests whether 
        /// the actual value is of the type supplied as an argument
        /// or a derived type.
        /// </summary>
        /// <param name="expectedType">The type to be tested for</param>
        public ConstraintExpression InstanceOfType(Type expectedType)
        {
            return Is.InstanceOfType(expectedType);
        }

#if NET_2_0
        /// <summary>
        /// InstanceOfType returns a constraint that tests whether 
        /// the actual value is of the type supplied as an argument
        /// or a derived type.
        /// </summary>
        /// <typeparam name="T">The type to be tested for</typeparam>
        public ConstraintExpression InstanceOfType<T>()
        {
            return InstanceOfType(typeof(T));
        }
#endif

        /// <summary>
        /// AssignableFrom returns a constraint that tests whether
        /// the actual value is assignable from the type supplied as
        /// an argument.
        /// </summary>
        /// <param name="expectedType"></param>
        /// <returns></returns>
        public ConstraintExpression AssignableFrom(Type expectedType)
        {
            return Is.AssignableFrom(expectedType);
        }

#if NET_2_0
        /// <summary>
        /// AssignableFrom returns a constraint that tests whether
        /// the actual value is assignable from the type supplied as
        /// an argument.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ConstraintExpression AssignableFrom<T>()
        {
            return AssignableFrom(typeof(T));
        }
#endif

        /// <summary>
        /// AssignableTo returns a constraint that tests whether
        /// the actual value is assignable to the type supplied as
        /// an argument.
        /// </summary>
        /// <param name="expectedType"></param>
        /// <returns></returns>
        public ConstraintExpression AssignableTo(Type expectedType)
        {
            return Is.AssignableTo(expectedType);
        }

#if NET_2_0
        /// <summary>
        /// AssignableTo returns a constraint that tests whether
        /// the actual value is assignable to the type supplied as
        /// an argument.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ConstraintExpression AssignableTo<T>()
        {
            return AssignableTo(typeof(T));
        }
#endif
        #endregion

        #region Collection Constraints
        /// <summary>
        /// EquivalentTo returns a constraint that tests whether
        /// the actual value is a collection containing the same
        /// elements as the collection supplied as an arument
        /// </summary>
        public ConstraintExpression EquivalentTo(ICollection expected)
        {
            return Is.EquivalentTo(expected);
        }

        /// <summary>
        /// SubsetOf returns a constraint that tests whether
        /// the actual value is a subset of the collection 
        /// supplied as an arument
        /// </summary>
        public ConstraintExpression SubsetOf(ICollection expected)
        {
            return Is.SubsetOf(expected);
        }
        #endregion

        #region Path Constraints
        /// <summary>
        /// SamePath returns a constraint that tests whether
        /// the path provided is the same as an expected path
        /// after canonicalization.
        /// </summary>
        /// <param name="expected">The expected path</param>
        /// <returns>True if the paths are the same, otherwise false</returns>
        public SamePathConstraint.Modifier SamePath(string expected)
        {
            return Is.SamePath(expected);
        }

        /// <summary>
        /// SamePathOrUnder returns a constraint that tests whether
        /// the path provided is the same as or under an expected path
        /// after canonicalization.
        /// </summary>
        /// <param name="expected">The expected path</param>
        /// <returns>True if the path is the same as or a subpath of the expected path, otherwise false</returns>
        public SamePathOrUnderConstraint.Modifier SamePathOrUnder(string expected)
        {
            return Is.SamePathOrUnder(expected);
        }

        /// <summary>
        /// Ordered returns a constraint that tests whether
        /// a collection is ordered
        /// </summary>
        public ConstraintExpression Ordered()
        {
            return Is.Ordered();
        }

        /// <summary>
        /// Ordered returns a constraint that tests whether
        /// a collection is ordered
        /// </summary>
        /// <param name="comparer">A custom comparer to be used to comparison</param>
        public ConstraintExpression Ordered(IComparer comparer)
        {
            return Is.Ordered(comparer);
        }
        #endregion

        #region Range Constraints
        public ConstraintExpression InRange(IComparable from, IComparable to)
        {
            return Is.InRange(from, to);
        }
        #endregion

        #region Property Constraints
        /// <summary>
        /// Returns a new PropertyConstraint checking for the
        /// existence of a particular property value.
        /// </summary>
        /// <param name="name">The name of the property to look for</param>
        /// <param name="expected">The expected value of the property</param>
        //public ConstraintExpression Property(string name, object expected)
        //{
        //    return Has.Property(name, expected);
        //}

        /// <summary>
        /// Returns a new PropertyConstraint for the Length property
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        //public ConstraintExpression Length(int length)
        //{
        //    return Has.Length(length);
        //}

        /// <summary>
        /// Returns a new PropertyConstraint for the Count property
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        //public ConstraintExpression Count(int count)
        //{
        //    return Has.Count(count);
        //}

        /// <summary>
        /// Returns a new PropertyConstraint for the Message property
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        //public ConstraintExpression Message(string message)
        //{
        //    return Has.Message(message);
        //}
        #endregion

        #region String Constraints
        /// <summary>
        /// Contains returns a constraint that succeeds if the actual
        /// value contains the substring supplied as an argument.
        /// </summary>
        public StringConstraint.Modifier ContainsSubstring(string substring)
        {
            return Text.Contains(substring);
        }

        /// <summary>
        /// DoesNotContain returns a constraint that fails if the actual
        /// value contains the substring supplied as an argument.
        /// </summary>
        public StringConstraint.Modifier DoesNotContain(string substring)
        {
            return Text.DoesNotContain(substring);
        }

        /// <summary>
        /// StartsWith returns a constraint that succeeds if the actual
        /// value starts with the substring supplied as an argument.
        /// </summary>
        public StringConstraint.Modifier StartsWith(string substring)
        {
            return Text.StartsWith(substring);
        }

        /// <summary>
        /// DoesNotStartWith returns a constraint that fails if the actual
        /// value starts with the substring supplied as an argument.
        /// </summary>
        public StringConstraint.Modifier DoesNotStartWith(string substring)
        {
            return Text.DoesNotStartWith(substring);
        }

        /// <summary>
        /// EndsWith returns a constraint that succeeds if the actual
        /// value ends with the substring supplied as an argument.
        /// </summary>
        public StringConstraint.Modifier EndsWith(string substring)
        {
            return Text.EndsWith(substring);
        }

        /// <summary>
        /// DoesNotEndWith returns a constraint that fails if the actual
        /// value ends with the substring supplied as an argument.
        /// </summary>
        public StringConstraint.Modifier DoesNotEndWith(string substring)
        {
            return Text.DoesNotEndWith(substring);
        }

        /// <summary>
        /// Matches returns a constraint that succeeds if the actual
        /// value matches the pattern supplied as an argument.
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public StringConstraint.Modifier Matches(string pattern)
        {
            return Text.Matches(pattern);
        }

        /// <summary>
        /// DoesNotMatch returns a constraint that failss if the actual
        /// value matches the pattern supplied as an argument.
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public StringConstraint.Modifier DoesNotMatch(string pattern)
        {
            return Text.DoesNotMatch(pattern);
        }
        #endregion

        #region Throws Constraints
//        /// <summary>
//        /// Creates a constraint specifying the type of exception expected
//        /// </summary>
//        /// <param name="type">The expected type of exception.</param>
//        /// <returns>A ThrowsConstraint</returns>
//        public ThrowsConstraint Throws(Type type)
//        {
//            return new ThrowsConstraint(type);
//        }

//        /// <summary>
//        /// Creates a constraint specifying the type of exception expected
//        /// </summary>
//        /// <param name="type">The expected type of exception.</param>
//        /// <param name="constraint">A further constraint on the exception</param>
//        /// <returns>A ThrowsConstraint</returns>
//        public ThrowsConstraint Throws(Type type, IConstraint constraint)
//        {
//            return new ThrowsConstraint(type, constraint);
//        }

//#if NET_2_0
//        /// <summary>
//        /// Creates a constraint specifying the type of exception expected
//        /// </summary>
//        /// <typeparam name="T">The expected type of exception.</typeparam>
//        /// <returns>A ThrowsConstraint</returns>
//        public ThrowsConstraint Throws<T>()
//        {
//            return new ThrowsConstraint(typeof(T));
//        }

//        /// <summary>
//        /// Creates a constraint specifying the type of exception expected
//        /// </summary>
//        /// <typeparam name="T">The expected type of exception.</typeparam>
//        /// <param name="constraint">A further constraint on the exception</param>
//        /// <returns>A ThrowsConstraint</returns>
//        public ThrowsConstraint Throws<T>(IConstraint constraint)
//        {
//            return new ThrowsConstraint(typeof(T), constraint);
//        }
//#endif

//        /// <summary>
//        /// Creates a constraint to test that no exception is thrown.
//        /// </summary>
//        public static ThrowsNothingConstraint Nothing
//        {
//            get { return new ThrowsNothingConstraint(); }
//        }
        #endregion

        #region Contains Constraint
        /// <summary>
        /// Resolves the chain of constraints using a
        /// ContainsConstraint as base. This constraint
        /// will, in turn, make use of the appropriate
        /// second-level constraint, depending on the
        /// type of the actual argument. This overload
        /// is only used if the item sought is a string,
        /// since any other type implies that we are
        /// looking in a collection for the item.
        /// </summary>
        public ContainsConstraint Contains(string expected)
        {
            return new ContainsConstraint(expected);
        }

        /// <summary>
        /// Resolves the chain of constraints using a 
        /// CollectionContainsConstraint as base.
        /// </summary>
        /// <param name="expected"></param>
        /// <returns></returns>
        public CollectionContainsConstraint Contains(object expected)
        {
            return new CollectionContainsConstraint(expected);
        }
        #endregion
    }
}
