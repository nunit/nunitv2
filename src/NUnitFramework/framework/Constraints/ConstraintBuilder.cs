// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using System.Collections;
#if NET_2_0
using System.Collections.Generic;
#endif

namespace NUnit.Framework.Constraints
{
    /// <summary>
    /// ConstraintBuilder is used to resolve the Not and All properties,
    /// which serve as prefix operators for constraints. With the addition
    /// of an operand stack, And and Or could be supported, but we have
    /// left them out in favor of a simpler, more type-safe implementation.
    /// Use the &amp; and | operator overloads to combine constraints.
    /// </summary>
    public class ConstraintBuilder : IConstraint
    {
        #region Constructors
        public ConstraintBuilder() { }

        public ConstraintBuilder(Constraint constraint)
        {
            Push(constraint);
        }
        #endregion

        #region Processing Stacks
        OpStack ops = new OpStack();
        ConstraintStack constraints = new ConstraintStack();
        #endregion

        #region Properties
        private Constraint resolvedConstraint;
        public IConstraint Constraint
        {
            get
            {
                if (resolvedConstraint == null)
                    resolvedConstraint = Resolve();

                return resolvedConstraint;
            }
        }
        #endregion

        #region Constraints Without Arguments
        /// <summary>
        /// Resolves the chain of constraints using
        /// EqualConstraint(null) as base.
        /// </summary>
        public ConstraintBuilder Null
        {
            get { return PushAndReturnSelf(new EqualConstraint(null)); }
        }

        /// <summary>
        /// Resolves the chain of constraints using
        /// EqualConstraint(true) as base.
        /// </summary>
        public ConstraintBuilder True
        {
            get { return PushAndReturnSelf(new EqualConstraint(true)); }
        }

        /// <summary>
        /// Resolves the chain of constraints using
        /// EqualConstraint(false) as base.
        /// </summary>
        public ConstraintBuilder False
        {
            get { return PushAndReturnSelf(new EqualConstraint(false)); }
        }

        /// <summary>
        /// Resolves the chain of constraints using
        /// Is.NaN as base.
        /// </summary>
        public ConstraintBuilder NaN
        {
            get { return PushAndReturnSelf(new EqualConstraint(double.NaN)); }
        }

        /// <summary>
        /// Resolves the chain of constraints using
        /// Is.Empty as base.
        /// </summary>
        public ConstraintBuilder Empty
        {
            get { return PushAndReturnSelf(new EmptyConstraint()); }
        }

        /// <summary>
        /// Resolves the chain of constraints using
        /// Is.Unique as base.
        /// </summary>
        public ConstraintBuilder Unique
        {
            get { return PushAndReturnSelf(new UniqueItemsConstraint()); }
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// CollectionOrderedConstraint as base.
        /// </summary>
        public ConstraintBuilder Ordered()
        {
            return PushAndReturnSelf(new CollectionOrderedConstraint());
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// CollectionOrderedConstraint as base.
        /// </summary>
        public ConstraintBuilder Ordered(IComparer comparer)
        {
            return PushAndReturnSelf(new CollectionOrderedConstraint(comparer));
        }
        #endregion

        #region Equality and Identity
        /// <summary>
        /// Resolves the chain of constraints using an
        /// EqualConstraint as base.
        /// </summary>
        public EqualConstraint.Modifier EqualTo(object expected)
        {
            EqualConstraint constraint = new EqualConstraint(expected);
            Push(constraint);
            return new EqualConstraint.Modifier(constraint);
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// SameAsConstraint as base.
        /// </summary>
        public ConstraintBuilder SameAs(object expected)
        {
            return PushAndReturnSelf(new SameAsConstraint(expected));
        }
        #endregion

        #region Comparison Constraints
        /// <summary>
        /// Resolves the chain of constraints using a
        /// LessThanConstraint as base.
        /// </summary>
        public ConstraintBuilder LessThan(IComparable expected)
        {
            return PushAndReturnSelf(new LessThanConstraint(expected));
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// GreaterThanConstraint as base.
        /// </summary>
        public ConstraintBuilder GreaterThan(IComparable expected)
        {
            return PushAndReturnSelf(new GreaterThanConstraint(expected));
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// LessThanOrEqualConstraint as base.
        /// </summary>
        public ConstraintBuilder LessThanOrEqualTo(IComparable expected)
        {
            return PushAndReturnSelf(new LessThanOrEqualConstraint(expected));
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// LessThanOrEqualConstraint as base.
        /// </summary>
        public ConstraintBuilder AtMost(IComparable expected)
        {
            return PushAndReturnSelf(new LessThanOrEqualConstraint(expected));
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// GreaterThanOrEqualConstraint as base.
        /// </summary>
        public ConstraintBuilder GreaterThanOrEqualTo(IComparable expected)
        {
            return PushAndReturnSelf(new GreaterThanOrEqualConstraint(expected));
        }
        /// <summary>
        /// Resolves the chain of constraints using a
        /// GreaterThanOrEqualConstraint as base.
        /// </summary>
        public ConstraintBuilder AtLeast(IComparable expected)
        {
            return PushAndReturnSelf(new GreaterThanOrEqualConstraint(expected));
        }
        #endregion

        #region Type Constraints
        /// <summary>
        /// Resolves the chain of constraints using an
        /// ExactTypeConstraint as base.
        /// </summary>
        public ConstraintBuilder TypeOf(Type expectedType)
        {
            return PushAndReturnSelf(new ExactTypeConstraint(expectedType));
        }

        /// <summary>
        /// Resolves the chain of constraints using an
        /// InstanceOfTypeConstraint as base.
        /// </summary>
        public ConstraintBuilder InstanceOfType(Type expectedType)
        {
            return PushAndReturnSelf(new InstanceOfTypeConstraint(expectedType));
        }

        /// <summary>
        /// Resolves the chain of constraints using an
        /// AssignableFromConstraint as base.
        /// </summary>
        public ConstraintBuilder AssignableFrom(Type expectedType)
        {
            return PushAndReturnSelf(new AssignableFromConstraint(expectedType));
        }
        #endregion

        #region Containing Constraint
        /// <summary>
        /// Resolves the chain of constraints using a
        /// ContainsConstraint as base. This constraint
        /// will, in turn, make use of the appropriate
        /// second-level constraint, depending on the
        /// type of the actual argument.
        /// </summary>
        public ContainsConstraint.Modifier Contains(string expected)
        {
            ContainsConstraint constraint = new ContainsConstraint(expected);
            Push(constraint);
            return new ContainsConstraint.Modifier(constraint);
        }

        /// <summary>
        /// Resolves the chain of constraints using a 
        /// CollectionContainsConstraint as base.
        /// </summary>
        /// <param name="expected"></param>
        /// <returns></returns>
        public ConstraintBuilder Contains(object expected)
        {
            return PushAndReturnSelf(new CollectionContainsConstraint(expected));
        }

        /// <summary>
        /// Resolves the chain of constraints using a 
        /// CollectionContainsConstraint as base.
        /// </summary>
        /// <param name="expected">The expected object</param>
        public ConstraintBuilder Member(object expected)
        {
            return PushAndReturnSelf(new CollectionContainsConstraint(expected));
        }
        #endregion

        #region String Constraints
        /// <summary>
        /// Pushes a SubstringConstraint on the stack and
        /// returns a Modifier for it.
        /// </summary>
        /// <param name="substring"></param>
        /// <returns></returns>
        public StringConstraint.Modifier ContainsSubstring(string substring)
        {
            SubstringConstraint constraint = new SubstringConstraint(substring);
            Push(constraint);
            return new StringConstraint.Modifier(constraint);
        }

		/// <summary>
		/// Resolves the chain of constraints using a
		/// StartsWithConstraint as base.
		/// </summary>
		public StringConstraint.Modifier StartsWith(string substring)
        {
            StartsWithConstraint constraint = new StartsWithConstraint(substring);
            Push(constraint);
            return new StringConstraint.Modifier( constraint );
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// StringEndingConstraint as base.
        /// </summary>
        public StringConstraint.Modifier EndsWith(string substring)
        {
            EndsWithConstraint constraint = new EndsWithConstraint(substring);
            Push(constraint);
            return new StringConstraint.Modifier(constraint);
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// StringMatchingConstraint as base.
        /// </summary>
        public StringConstraint.Modifier Matches(string pattern)
        {
            RegexConstraint constraint = new RegexConstraint(pattern);
            Push(constraint);
            return new StringConstraint.Modifier(constraint);
        }
        #endregion

        #region Collection Constraints
        /// <summary>
        /// Resolves the chain of constraints using a
        /// CollectionEquivalentConstraint as base.
        /// </summary>
        public ConstraintBuilder EquivalentTo(ICollection expected)
        {
            return PushAndReturnSelf( new CollectionEquivalentConstraint(expected) );
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// CollectionContainingConstraint as base.
        /// </summary>
        public ConstraintBuilder CollectionContaining(object expected)
		{
			return PushAndReturnSelf( new CollectionContainsConstraint(expected) );
		}

        /// <summary>
        /// Resolves the chain of constraints using a
        /// CollectionSubsetConstraint as base.
        /// </summary>
        public ConstraintBuilder SubsetOf(ICollection expected)
        {
            return PushAndReturnSelf(new CollectionSubsetConstraint(expected));
        }
        #endregion

		#region Path Constraints
		/// <summary>
		/// Resolves the chain of constraints using a
		/// SamePathConstraint as base.
		/// </summary>
		public PathConstraint.Modifier SamePath(string expected)
		{
            SamePathConstraint constraint = new SamePathConstraint(expected);
            Push(constraint);
            return constraint.GetModifier();
        }

		/// <summary>
		/// Resolves the chain of constraints using a
		/// SamePathOrUnderConstraint as base.
		/// </summary>
		public PathConstraint.Modifier SamePathOrUnder( string expected )
		{
            SamePathOrUnderConstraint constraint = new SamePathOrUnderConstraint( expected );
            Push(constraint);
            return constraint.GetModifier();
		}
		#endregion

		#region Property Constraints
        /// <summary>
        /// Resolves the chain of constraints using a 
        /// PropertyConstraint as base
        /// </summary>
		public ConstraintBuilder Property( string name, object expected )
		{
			return PushAndReturnSelf( new PropertyConstraint( name, new EqualConstraint( expected ) ) );
		}

        /// <summary>
        /// Resolves the chain of constraints using a
        /// PropertyCOnstraint on Length as base
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public ConstraintBuilder Length(int length)
        {
            return Property("Length", length);
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// PropertyConstraint on Length as base
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public ConstraintBuilder Count(int count)
        {
            return Property("Count", count);
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// PropertyConstraint on Message as base
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public ConstraintBuilder Message(string message)
        {
            return Property("Message", message);
        }
        #endregion

        #region Range Constraint
        public ConstraintBuilder InRange(IComparable from, IComparable to)
        {
            return PushAndReturnSelf( new RangeConstraint(from, to) );
        }
        #endregion

        #region Operators

        #region Not
        /// <summary>
		/// Modifies the ConstraintBuilder by pushing a Not operator on the stack.
		/// </summary>
		public ConstraintBuilder Not
		{
			get { return PushAndReturnSelf( new NotOperator() ); }
		}

		/// <summary>
		/// Modifies the ConstraintBuilder by pushing a Not operator on the stack.
		/// </summary>
		public ConstraintBuilder No
		{
            get { return PushAndReturnSelf(new NotOperator()); }
        }
        #endregion

        #region All
        /// <summary>
        /// Modifies the ConstraintBuilder by pushing an All operator on the stack.
        /// </summary>
        public ConstraintBuilder All
        {
            get { return PushAndReturnSelf(new AllOperator()); }
        }
        #endregion

        #region Some
        /// <summary>
		/// Modifies the ConstraintBuilder by pushing a Some operator on the stack.
		/// </summary>
		public ConstraintBuilder Some
		{
            get { return PushAndReturnSelf(new SomeOperator()); }
        }
        #endregion

        #region None
        /// <summary>
        /// Modifies the constraint builder by pushing a None operator on the stack
        /// </summary>
		public ConstraintBuilder None
		{
            get { return PushAndReturnSelf(new NoneOperator()); }
        }
        #endregion

        #region Prop
        /// <summary>
        /// Modifies the ConstraintBuilder by pushing a Prop operator on the
        /// ops stack and the name of the property on the constraints stack.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ConstraintBuilder Property(string name)
		{
			return PushAndReturnSelf( new PropOperator(name) );
        }
        #endregion

        #region And
        public ConstraintBuilder And
        {
            get { return PushAndReturnSelf(new AndOperator()); }
        }
        #endregion

        #region Or
        public ConstraintBuilder Or
        {
            get { return PushAndReturnSelf(new OrOperator()); }
        }
        #endregion

        #endregion

        #region Operator Overloads
        /// <summary>
        /// This operator creates a constraint that is satisfied only if both 
        /// argument constraints are satisfied.
        /// </summary>
        public static ConstraintBuilder operator &(ConstraintBuilder left, ConstraintBuilder right)
        {
            left.Push(new AndOperator());
            left.Push(right.Resolve());
            return left;
        }

        /// <summary>
        /// This operator creates a constraint that is satisfied if either 
        /// of the argument constraints is satisfied.
        /// </summary>
        public static ConstraintBuilder operator |(ConstraintBuilder left, ConstraintBuilder right)
        {
            left.Push(new OrOperator());
            left.Push(right.Resolve());
            return left;
        }

        /// <summary>
        /// This operator creates a constraint that is satisfied if the 
        /// argument constraint is not satisfied.
        /// </summary>
        public static Constraint operator !(ConstraintBuilder m)
        {
            return new NotConstraint(m == null ? new EqualConstraint(null) : m.Resolve());
        }
        #endregion

        #region Helper Methods
        private void Push(ConstraintOperator op)
        {
            while (ops.Count > 0 && op.Precedence > ops.Peek().Precedence)
                ops.Pop().Reduce(constraints);

            ops.Push(op);
        }

        private void Push(Constraint constraint)
        {
            constraint.Builder = this;
            constraints.Push(constraint);
        }

        private ConstraintBuilder PushAndReturnSelf(ConstraintOperator op)
        {
            Push(op);
            return this;
        }

        private ConstraintBuilder PushAndReturnSelf(Constraint constraint)
        {
            Push(constraint);
            return this;
        }

        /// <summary>
        /// Resolve a constraint that has been recognized by pushing it
        /// to the operand stack and calling Resolve().
        /// </summary>
        /// <returns>A constraint that incorporates all pending operators</returns>
        private Constraint Resolve(Constraint constraint)
        {
            constraints.Push(constraint);
            return Resolve();
        }

        /// <summary>
        /// Resolves the builder to a constraint by applying
        /// all pending operators and operands from the stack.
        /// </summary>
        /// <returns>A constraint that incorporates all pending operators</returns>
        private Constraint Resolve()
        {
            while (ops.Count > 0)
            {
                ConstraintOperator op = ops.Pop();
                op.Reduce(constraints);
            }

            return constraints.Pop(); ;
        }
        #endregion

        #region IConstraint Members
        public bool Matches(object actual)
        {
            return Constraint.Matches(actual);
        }

        public void WriteMessageTo(MessageWriter writer)
        {
            Constraint.WriteMessageTo(writer);
        }

        public void WriteDescriptionTo(MessageWriter writer)
        {
            Constraint.WriteDescriptionTo(writer);
        }

        public void WriteActualValueTo(MessageWriter writer)
        {
            Constraint.WriteActualValueTo(writer);
        }
        #endregion
    }
}
