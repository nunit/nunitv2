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
    /// ConstraintBuilder is used to resolve constraint expressions, which
    /// are constructed from a series of syntactic elements joined by the
    /// C# dot operator. Elements in an expression may be:
    ///   Individual constraints, like EqualTo or Null
    ///   Prefix operators, which precede a constraint
    ///   Constraint modifiers, which follow a constraint.
    ///   Binary operators And and Or, which join two constraints.
    /// </summary>
    public class ConstraintBuilder
    {
        #region Constructors
        public ConstraintBuilder() { }

        public ConstraintBuilder(Constraint constraint)
        {
            Push(constraint);
        }

        public ConstraintBuilder(ConstraintOperator op)
        {
            Push(op);
        }

        public ConstraintBuilder(ConstraintBuilder other)
        {
            this.ops = other.ops;
            this.constraints = other.constraints;
            this.lastPushed = other.lastPushed;
        }
        #endregion

        #region Processing Stacks
        protected OpStack ops = new OpStack();
        protected ConstraintStack constraints = new ConstraintStack();
        #endregion

        #region Constraints Without Arguments
        /// <summary>
        /// Resolves the chain of constraints using
        /// EqualConstraint(null) as base.
        /// </summary>
        public ResolvableConstraintBuilder Null
        {
            get { Push(new EqualConstraint(null)); return this.AsResolvable(); }
        }

        /// <summary>
        /// Resolves the chain of constraints using
        /// EqualConstraint(true) as base.
        /// </summary>
        public ResolvableConstraintBuilder True
        {
            get { Push(new EqualConstraint(true)); return this.AsResolvable(); }
        }

        /// <summary>
        /// Resolves the chain of constraints using
        /// EqualConstraint(false) as base.
        /// </summary>
        public ResolvableConstraintBuilder False
        {
            get { Push(new EqualConstraint(false)); return this.AsResolvable(); }
        }

        /// <summary>
        /// Resolves the chain of constraints using
        /// Is.NaN as base.
        /// </summary>
        public ResolvableConstraintBuilder NaN
        {
            get { Push(new EqualConstraint(double.NaN)); return this.AsResolvable(); }
        }

        /// <summary>
        /// Resolves the chain of constraints using
        /// Is.Empty as base.
        /// </summary>
        public ResolvableConstraintBuilder Empty
        {
            get { Push(new EmptyConstraint()); return this.AsResolvable(); }
        }

        /// <summary>
        /// Resolves the chain of constraints using
        /// Is.Unique as base.
        /// </summary>
        public ResolvableConstraintBuilder Unique
        {
            get { Push(new UniqueItemsConstraint()); return this.AsResolvable(); }
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// CollectionOrderedConstraint as base.
        /// </summary>
        public ResolvableConstraintBuilder Ordered()
        {
            Push(new CollectionOrderedConstraint()); return this.AsResolvable();
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// CollectionOrderedConstraint as base.
        /// </summary>
        public ResolvableConstraintBuilder Ordered(IComparer comparer)
        {
            Push(new CollectionOrderedConstraint(comparer)); return this.AsResolvable();
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
        public ResolvableConstraintBuilder SameAs(object expected)
        {
            Push(new SameAsConstraint(expected)); return this.AsResolvable();
        }
        #endregion

        #region Comparison Constraints
        /// <summary>
        /// Resolves the chain of constraints using a
        /// LessThanConstraint as base.
        /// </summary>
        public ResolvableConstraintBuilder LessThan(IComparable expected)
        {
            Push(new LessThanConstraint(expected)); return this.AsResolvable();
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// GreaterThanConstraint as base.
        /// </summary>
        public ResolvableConstraintBuilder GreaterThan(IComparable expected)
        {
            Push(new GreaterThanConstraint(expected)); return this.AsResolvable();
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// LessThanOrEqualConstraint as base.
        /// </summary>
        public ResolvableConstraintBuilder LessThanOrEqualTo(IComparable expected)
        {
            Push(new LessThanOrEqualConstraint(expected)); return this.AsResolvable();
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// LessThanOrEqualConstraint as base.
        /// </summary>
        public ResolvableConstraintBuilder AtMost(IComparable expected)
        {
            Push(new LessThanOrEqualConstraint(expected)); return this.AsResolvable();
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// GreaterThanOrEqualConstraint as base.
        /// </summary>
        public ResolvableConstraintBuilder GreaterThanOrEqualTo(IComparable expected)
        {
            Push(new GreaterThanOrEqualConstraint(expected)); return this.AsResolvable();
        }
        /// <summary>
        /// Resolves the chain of constraints using a
        /// GreaterThanOrEqualConstraint as base.
        /// </summary>
        public ResolvableConstraintBuilder AtLeast(IComparable expected)
        {
            Push(new GreaterThanOrEqualConstraint(expected)); return this.AsResolvable();
        }
        #endregion

        #region Type Constraints
        /// <summary>
        /// Resolves the chain of constraints using an
        /// ExactTypeConstraint as base.
        /// </summary>
        public ResolvableConstraintBuilder TypeOf(Type expectedType)
        {
            Push(new ExactTypeConstraint(expectedType)); return this.AsResolvable();
        }

        /// <summary>
        /// Resolves the chain of constraints using an
        /// InstanceOfTypeConstraint as base.
        /// </summary>
        public ResolvableConstraintBuilder InstanceOfType(Type expectedType)
        {
            Push(new InstanceOfTypeConstraint(expectedType)); return this.AsResolvable();
        }

        /// <summary>
        /// Resolves the chain of constraints using an
        /// AssignableFromConstraint as base.
        /// </summary>
        public ResolvableConstraintBuilder AssignableFrom(Type expectedType)
        {
            Push(new AssignableFromConstraint(expectedType)); return this.AsResolvable();
        }

        /// <summary>
        /// Resolves the chain of constraints using an
        /// AssignableToConstraint as base.
        /// </summary>
        public ResolvableConstraintBuilder AssignableTo(Type expectedType)
        {
            Push(new AssignableToConstraint(expectedType)); return this.AsResolvable();
        }

#if NET_2_0
        /// <summary>
        /// Resolves the chain of constraints using an
        /// ExactTypeConstraint as base.
        /// </summary>
        public ResolvableConstraintBuilder TypeOf<T>()
        {
            return TypeOf(typeof(T));
        }

        /// <summary>
        /// Resolves the chain of constraints using an
        /// InstanceOfTypeConstraint as base.
        /// </summary>
        public ResolvableConstraintBuilder InstanceOfType<T>()
        {
            return InstanceOfType(typeof(T));
        }

        /// <summary>
        /// Resolves the chain of constraints using an
        /// AssignableFromConstraint as base.
        /// </summary>
        public ResolvableConstraintBuilder AssignableFrom<T>()
        {
            return AssignableFrom(typeof(T));
        }

        /// <summary>
        /// Resolves the chain of constraints using an
        /// AssignableToConstraint as base.
        /// </summary>
        public ResolvableConstraintBuilder AssignableTo<T>()
        {
            return AssignableTo(typeof(T));
        }
#endif
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
        public ResolvableConstraintBuilder Contains(object expected)
        {
            Push(new CollectionContainsConstraint(expected)); return this.AsResolvable();
        }

        /// <summary>
        /// Resolves the chain of constraints using a 
        /// CollectionContainsConstraint as base.
        /// </summary>
        /// <param name="expected">The expected object</param>
        public ResolvableConstraintBuilder Member(object expected)
        {
            Push(new CollectionContainsConstraint(expected)); return this.AsResolvable();
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
        public ResolvableConstraintBuilder EquivalentTo(ICollection expected)
        {
            Push( new CollectionEquivalentConstraint(expected) ); return this.AsResolvable();
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// CollectionContainingConstraint as base.
        /// </summary>
        public ResolvableConstraintBuilder CollectionContaining(object expected)
		{
			Push( new CollectionContainsConstraint(expected) ); return this.AsResolvable();
		}

        /// <summary>
        /// Resolves the chain of constraints using a
        /// CollectionSubsetConstraint as base.
        /// </summary>
        public ResolvableConstraintBuilder SubsetOf(ICollection expected)
        {
            Push(new CollectionSubsetConstraint(expected)); return this.AsResolvable();
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
		public ResolvableConstraintBuilder Property( string name, object expected )
		{
			Push( new PropertyConstraint( name, new EqualConstraint( expected ) ) ); return this.AsResolvable();
		}

        /// <summary>
        /// Resolves the chain of constraints using a
        /// PropertyCOnstraint on Length as base
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public ResolvableConstraintBuilder Length(int length)
        {
            return Property("Length", length);
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// PropertyConstraint on Length as base
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public ResolvableConstraintBuilder Count(int count)
        {
            return Property("Count", count);
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// PropertyConstraint on Message as base
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public ResolvableConstraintBuilder Message(string message)
        {
            return Property("Message", message);
        }
        #endregion

        #region Range Constraint
        public ResolvableConstraintBuilder InRange(IComparable from, IComparable to)
        {
            Push( new RangeConstraint(from, to) ); return this.AsResolvable();
        }
        #endregion

        #region Operators

        #region Not
        /// <summary>
		/// Modifies the ConstraintBuilder by pushing a Not operator on the stack.
		/// </summary>
		public ConstraintBuilder Not
		{
            get { Push(new NotOperator()); return this; }
        }

		/// <summary>
		/// Modifies the ConstraintBuilder by pushing a Not operator on the stack.
		/// </summary>
		public ConstraintBuilder No
		{
            get { Push(new NotOperator()); return this; }
        }
        #endregion

        #region All
        /// <summary>
        /// Modifies the ConstraintBuilder by pushing an All operator on the stack.
        /// </summary>
        public ConstraintBuilder All
        {
            get { Push(new AllOperator()); return this; }
        }
        #endregion

        #region Some
        /// <summary>
		/// Modifies the ConstraintBuilder by pushing a Some operator on the stack.
		/// </summary>
		public ConstraintBuilder Some
		{
            get { Push(new SomeOperator()); return this;  }
        }
        #endregion

        #region None
        /// <summary>
        /// Modifies the constraint builder by pushing a None operator on the stack
        /// </summary>
		public ConstraintBuilder None
		{
            get { Push(new NoneOperator()); return this;  }
        }
        #endregion

        #region Prop
        /// <summary>
        /// Modifies the ConstraintBuilder by pushing a Prop operator on the
        /// ops stack and the name of the property on the constraints stack.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ResolvableConstraintBuilder Property(string name)
		{
            Push(new PropOperator(name));
            return new ResolvableConstraintBuilder(this);
        }
        #endregion

        #region Throws
//        public ResolvableConstraintBuilder Throws(Type type)
//        {
//            Push(new ThrowsOperator());
//            Push(new ExactTypeConstraint(type));
//            return new ResolvableConstraintBuilder(this);
//        }

//#if NET_2_0
//        public ResolvableConstraintBuilder Throws<T>()
//        {
//            return Throws(typeof(T));
//        }
//#endif
        #endregion

        #region With
        /// <summary>
        /// With is currently a NOP - reserved for future use.
        /// </summary>
        public ConstraintBuilder With
        {
            get { return this; }
            //get { Push(new WithOperator()); return this.AsResolvable(); }
        }
        #endregion

        #endregion

        #region Helper Methods
        // A single token of left context used only
        // by operators that depend on it.
        private object lastPushed;

        /// <summary>
        /// Pushes the specified operator onto the operator stack.
        /// </summary>
        /// <param name="op">The operator to push.</param>
        protected void Push(ConstraintOperator op)
        {
            ConstraintOperator prior;

            if (lastPushed is PropOperator)
                ops.Push(new PropValOperator(((PropOperator)ops.Pop()).Name));

            while (!ops.Empty && op.LeftPrecedence > ops.Top.RightPrecedence)
                ops.Pop().Reduce(constraints);

            ops.Push(op);
            lastPushed = op;
        }

        /// <summary>
        /// Pushes the specified constraint on the constraint stack.
        /// </summary>
        /// <param name="constraint">The constraint to push.</param>
        protected void Push(Constraint constraint)
        {
            if (lastPushed is PropOperator)
                ops.Push(new PropValOperator(((PropOperator)ops.Pop()).Name));
            else if (lastPushed is Constraint)
                Push(new AndOperator());

            constraint.Builder = this;
            constraints.Push(constraint);
            lastPushed = constraint;
        }

        protected virtual ResolvableConstraintBuilder AsResolvable()
        {
            return new ResolvableConstraintBuilder(this);
        }
        #endregion
    }

    public class ResolvableConstraintBuilder : ConstraintBuilder, IConstraint
    {
        public ResolvableConstraintBuilder() { }

        public ResolvableConstraintBuilder(Constraint constraint)
            : base(constraint) { }

        public ResolvableConstraintBuilder(ConstraintBuilder other)
            : base( other ) { }

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

        /// <summary>
        /// Resolves the builder to a constraint by applying
        /// all pending operators and operands from the stack.
        /// </summary>
        /// <returns>A constraint that incorporates all pending operators</returns>
        public Constraint Resolve()
        {
            while (!ops.Empty)
            {
                ConstraintOperator op = ops.Pop();
                op.Reduce(constraints);
            }

            Constraint constraint = constraints.Pop();
            constraint.Builder = null;
            return constraint;
        }
        
        #region And
        public ConstraintBuilder And
        {
            get { Push(new AndOperator()); return this; }
        }
        #endregion

        #region Or
        public ConstraintBuilder Or
        {
            get { Push(new OrOperator()); return this; }
        }
        #endregion

        #region Operator Overloads
        /// <summary>
        /// This operator creates a constraint that is satisfied only if both 
        /// argument constraints are satisfied.
        /// </summary>
        public static Constraint operator &(ResolvableConstraintBuilder left, ResolvableConstraintBuilder right)
        {
            return new AndConstraint(left.Resolve(), right.Resolve());
        }

        /// <summary>
        /// This operator creates a constraint that is satisfied only if both 
        /// argument constraints are satisfied.
        /// </summary>
        public static Constraint operator &(ResolvableConstraintBuilder left, Constraint right)
        {
            return new AndConstraint(left.Resolve(), right);
        }

        /// <summary>
        /// This operator creates a constraint that is satisfied only if both 
        /// argument constraints are satisfied.
        /// </summary>
        public static Constraint operator &(Constraint left, ResolvableConstraintBuilder right)
        {
            return new AndConstraint(left, right.Resolve());
        }

        /// <summary>
        /// This operator creates a constraint that is satisfied if either 
        /// of the argument constraints is satisfied.
        /// </summary>
        public static Constraint operator |(ResolvableConstraintBuilder left, ResolvableConstraintBuilder right)
        {
            return new OrConstraint(left.Resolve(), right.Resolve());
            //left.Push(new OrOperator());
            //left.Push(right.Resolve());
            //return left;
        }

        /// <summary>
        /// This operator creates a constraint that is satisfied if the 
        /// argument constraint is not satisfied.
        /// </summary>
        public static Constraint operator !(ResolvableConstraintBuilder m)
        {
            return new NotConstraint(m == null ? new EqualConstraint(null) : m.Resolve());
        }
        #endregion

        protected override ResolvableConstraintBuilder AsResolvable()
        {
            return this;
        }

        public override string ToString()
        {
            return Constraint.ToString();
        }

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
