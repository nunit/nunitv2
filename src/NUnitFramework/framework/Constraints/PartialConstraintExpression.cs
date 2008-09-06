using System;
using System.Collections;

namespace NUnit.Framework.Constraints
{
    /// <summary>
    /// PartialConstraintExpression is one of the classes used to 
    /// represent a constraint in the process of being constructed 
    /// from a series of syntactic elements. 
    /// 
    /// Specifically, PartialConstraintExpression represents an 
    /// incomplete expression, which cannot yet be resolved to 
    /// an actual constraint.
    /// </summary>
    public class PartialConstraintExpression
    {
        protected ConstraintBuilder builder;

        public PartialConstraintExpression()
        {
            this.builder = new ConstraintBuilder();
        }

        public PartialConstraintExpression(ConstraintBuilder builder)
        {
            this.builder = builder;
        }

        #region Constraints Without Arguments
        /// <summary>
        /// Resolves the chain of constraints using
        /// EqualConstraint(null) as base.
        /// </summary>
        public ConstraintExpression Null
        {
            get { return this.Append(new EqualConstraint(null)); }
        }

        /// <summary>
        /// Resolves the chain of constraints using
        /// EqualConstraint(true) as base.
        /// </summary>
        public ConstraintExpression True
        {
            get { return this.Append(new EqualConstraint(true)); }
        }

        /// <summary>
        /// Resolves the chain of constraints using
        /// EqualConstraint(false) as base.
        /// </summary>
        public ConstraintExpression False
        {
            get { return this.Append(new EqualConstraint(false)); }
        }

        /// <summary>
        /// Resolves the chain of constraints using
        /// Is.NaN as base.
        /// </summary>
        public ConstraintExpression NaN
        {
            get { return this.Append(new EqualConstraint(double.NaN)); }
        }

        /// <summary>
        /// Resolves the chain of constraints using
        /// Is.Empty as base.
        /// </summary>
        public ConstraintExpression Empty
        {
            get { return this.Append(new EmptyConstraint()); }
        }

        /// <summary>
        /// Resolves the chain of constraints using
        /// Is.Unique as base.
        /// </summary>
        public ConstraintExpression Unique
        {
            get { return this.Append(new UniqueItemsConstraint()); }
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// CollectionOrderedConstraint as base.
        /// </summary>
        public CollectionOrderedConstraint.Modifier Ordered()
        {
            CollectionOrderedConstraint constraint = new CollectionOrderedConstraint();
            return new CollectionOrderedConstraint.Modifier(constraint, this.Append(constraint));
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// CollectionOrderedConstraint as base.
        /// </summary>
        public CollectionOrderedConstraint.Modifier Ordered(IComparer comparer)
        {
            CollectionOrderedConstraint constraint = new CollectionOrderedConstraint(comparer);
            return new CollectionOrderedConstraint.Modifier(constraint, this.Append(constraint));
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// CollectionOrderedConstraint as base.
        /// </summary>
        public CollectionOrderedConstraint.Modifier OrderedBy(string propertyName)
        {
            CollectionOrderedConstraint constraint = new CollectionOrderedConstraint(propertyName);
            return new CollectionOrderedConstraint.Modifier(constraint, this.Append(constraint));
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// CollectionOrderedConstraint as base.
        /// </summary>
        public CollectionOrderedConstraint.Modifier OrderedBy(string propertyName, IComparer comparer)
        {
            CollectionOrderedConstraint constraint = new CollectionOrderedConstraint(propertyName, comparer);
            return new CollectionOrderedConstraint.Modifier(constraint, this.Append(constraint));
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
            return new EqualConstraint.Modifier(constraint, this.Append(constraint));
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// SameAsConstraint as base.
        /// </summary>
        public ConstraintExpression SameAs(object expected)
        {
            return this.Append(new SameAsConstraint(expected));
        }
        #endregion

        #region Comparison Constraints
        /// <summary>
        /// Resolves the chain of constraints using a
        /// LessThanConstraint as base.
        /// </summary>
        public ConstraintExpression LessThan(IComparable expected)
        {
            return this.Append(new LessThanConstraint(expected));
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// GreaterThanConstraint as base.
        /// </summary>
        public ConstraintExpression GreaterThan(IComparable expected)
        {
            return this.Append(new GreaterThanConstraint(expected));
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// LessThanOrEqualConstraint as base.
        /// </summary>
        public ConstraintExpression LessThanOrEqualTo(IComparable expected)
        {
            return this.Append(new LessThanOrEqualConstraint(expected));
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// LessThanOrEqualConstraint as base.
        /// </summary>
        public ConstraintExpression AtMost(IComparable expected)
        {
            return this.Append(new LessThanOrEqualConstraint(expected));
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// GreaterThanOrEqualConstraint as base.
        /// </summary>
        public ConstraintExpression GreaterThanOrEqualTo(IComparable expected)
        {
            return this.Append(new GreaterThanOrEqualConstraint(expected));
        }
        /// <summary>
        /// Resolves the chain of constraints using a
        /// GreaterThanOrEqualConstraint as base.
        /// </summary>
        public ConstraintExpression AtLeast(IComparable expected)
        {
            return this.Append(new GreaterThanOrEqualConstraint(expected));
        }
        #endregion

        #region Type Constraints
        /// <summary>
        /// Resolves the chain of constraints using an
        /// ExactTypeConstraint as base.
        /// </summary>
        public ConstraintExpression TypeOf(Type expectedType)
        {
            return this.Append(new ExactTypeConstraint(expectedType));
        }

        /// <summary>
        /// Resolves the chain of constraints using an
        /// InstanceOfTypeConstraint as base.
        /// </summary>
        public ConstraintExpression InstanceOf(Type expectedType)
        {
            return this.Append(new InstanceOfTypeConstraint(expectedType));
        }

        /// <summary>
        /// Resolves the chain of constraints using an
        /// InstanceOfTypeConstraint as base.
        /// </summary>
        [Obsolete("Use InstanceOf")]
        public ConstraintExpression InstanceOfType(Type expectedType)
        {
            return this.Append(new InstanceOfTypeConstraint(expectedType));
        }

        /// <summary>
        /// Resolves the chain of constraints using an
        /// AssignableFromConstraint as base.
        /// </summary>
        public ConstraintExpression AssignableFrom(Type expectedType)
        {
            return this.Append(new AssignableFromConstraint(expectedType));
        }

        /// <summary>
        /// Resolves the chain of constraints using an
        /// AssignableToConstraint as base.
        /// </summary>
        public ConstraintExpression AssignableTo(Type expectedType)
        {
            return this.Append(new AssignableToConstraint(expectedType));
        }

        /// <summary>
        /// Resolves the chain of constraints using an
        /// AttributeConstraint as base.
        /// </summary>
        public ConstraintExpression Attribute(Type expectedType)
        {
            return this.Append(new AttributeConstraint(expectedType));
        }

#if NET_2_0
        /// <summary>
        /// Resolves the chain of constraints using an
        /// ExactTypeConstraint as base.
        /// </summary>
        public ConstraintExpression TypeOf<T>()
        {
            return TypeOf(typeof(T));
        }

        /// <summary>
        /// Resolves the chain of constraints using an
        /// InstanceOfTypeConstraint as base.
        /// </summary>
        [Obsolete("Use InstanceOf")]
        public ConstraintExpression InstanceOfType<T>()
        {
            return InstanceOfType(typeof(T));
        }

        /// <summary>
        /// Resolves the chain of constraints using an
        /// InstanceOfTypeConstraint as base.
        /// </summary>
        public ConstraintExpression InstanceOf<T>()
        {
            return InstanceOf(typeof(T));
        }

        /// <summary>
        /// Resolves the chain of constraints using an
        /// AssignableFromConstraint as base.
        /// </summary>
        public ConstraintExpression AssignableFrom<T>()
        {
            return AssignableFrom(typeof(T));
        }

        /// <summary>
        /// Resolves the chain of constraints using an
        /// AssignableToConstraint as base.
        /// </summary>
        public ConstraintExpression AssignableTo<T>()
        {
            return AssignableTo(typeof(T));
        }

        /// <summary>
        /// Resolves the chain of constraints using an
        /// AttributeConstraint as base.
        /// </summary>
        public ConstraintExpression Attribute<T>()
        {
            return Attribute(typeof(T));
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
            return new ContainsConstraint.Modifier(constraint, this.Append(constraint));
        }

        /// <summary>
        /// Resolves the chain of constraints using a 
        /// CollectionContainsConstraint as base.
        /// </summary>
        /// <param name="expected"></param>
        /// <returns></returns>
        public ConstraintExpression Contains(object expected)
        {
            return this.Append(new CollectionContainsConstraint(expected));
        }

        /// <summary>
        /// Resolves the chain of constraints using a 
        /// CollectionContainsConstraint as base.
        /// </summary>
        /// <param name="expected">The expected object</param>
        public ConstraintExpression Member(object expected)
        {
            return this.Append(new CollectionContainsConstraint(expected));
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
            return new StringConstraint.Modifier(constraint, this.Append(constraint));
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// StartsWithConstraint as base.
        /// </summary>
        public StringConstraint.Modifier StartsWith(string substring)
        {
            StartsWithConstraint constraint = new StartsWithConstraint(substring);
            return new StringConstraint.Modifier(constraint, this.Append(constraint));
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// StringEndingConstraint as base.
        /// </summary>
        public StringConstraint.Modifier EndsWith(string substring)
        {
            EndsWithConstraint constraint = new EndsWithConstraint(substring);
            return new StringConstraint.Modifier(constraint, this.Append(constraint));
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// StringMatchingConstraint as base.
        /// </summary>
        public StringConstraint.Modifier Matches(string pattern)
        {
            RegexConstraint constraint = new RegexConstraint(pattern);
            return new StringConstraint.Modifier(constraint, this.Append(constraint));
        }
        #endregion

        #region Collection Constraints
        /// <summary>
        /// Resolves the chain of constraints using a
        /// CollectionEquivalentConstraint as base.
        /// </summary>
        public ConstraintExpression EquivalentTo(ICollection expected)
        {
            return this.Append(new CollectionEquivalentConstraint(expected));
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// CollectionContainingConstraint as base.
        /// </summary>
        public ConstraintExpression CollectionContaining(object expected)
        {
            return this.Append(new CollectionContainsConstraint(expected));
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// CollectionSubsetConstraint as base.
        /// </summary>
        public ConstraintExpression SubsetOf(ICollection expected)
        {
            return this.Append(new CollectionSubsetConstraint(expected));
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
            return new PathConstraint.Modifier(constraint, this.Append(constraint));
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// SamePathOrUnderConstraint as base.
        /// </summary>
        public PathConstraint.Modifier SamePathOrUnder(string expected)
        {
            SamePathOrUnderConstraint constraint = new SamePathOrUnderConstraint(expected);
            return new PathConstraint.Modifier(constraint, this.Append(constraint));
        }
        #endregion

        #region Range Constraint
        public ConstraintExpression InRange(IComparable from, IComparable to)
        {
            return this.Append(new RangeConstraint(from, to));
        }
        #endregion

        #region Operators

        #region Not
        /// <summary>
        /// Modifies the ConstraintBuilder by pushing a Not operator on the stack.
        /// </summary>
        public PartialConstraintExpression Not
        {
            get { return this.Append(new NotOperator()); }
        }

        /// <summary>
        /// Modifies the ConstraintBuilder by pushing a Not operator on the stack.
        /// </summary>
        public PartialConstraintExpression No
        {
            get { return this.Append(new NotOperator()); }
        }
        #endregion

        #region All
        /// <summary>
        /// Modifies the ConstraintBuilder by pushing an All operator on the stack.
        /// </summary>
        public PartialConstraintExpression All
        {
            get { return this.Append(new AllOperator()); }
        }
        #endregion

        #region Some
        /// <summary>
        /// Modifies the ConstraintBuilder by pushing a Some operator on the stack.
        /// </summary>
        public PartialConstraintExpression Some
        {
            get { return this.Append(new SomeOperator()); }
        }
        #endregion

        #region None
        /// <summary>
        /// Modifies the constraint builder by pushing a None operator on the stack
        /// </summary>
        public PartialConstraintExpression None
        {
            get { return this.Append(new NoneOperator()); }
        }
        #endregion

        #region Prop
        /// <summary>
        /// Modifies the ConstraintBuilder by pushing a Prop operator
        /// with the specified name on the operator stack
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public PendingConstraintExpression Property(string name)
        {
            builder.Append(new PropOperator(name));
            return new PendingConstraintExpression(builder);
        }

        /// <summary>
        /// Modifies the ConstraintBuilder by pushing a Prop operator
        /// for the Length property on the operator stack.
        /// </summary>
        /// <returns></returns>
        public PendingConstraintExpression Length
        {
            get { return Property("Length"); }
        }

        /// <summary>
        /// Modifies the ConstraintBuilder by pushing a Prop operator
        /// for the Count property on the operator stack.
        /// </summary>
        /// <returns></returns>
        public PendingConstraintExpression Count
        {
            get { return Property("Count"); }
        }

        /// <summary>
        /// Modifies the ConstraintBuilder by pushing a Prop operator
        /// for the Message property on the operator stack.
        /// </summary>
        /// <returns></returns>
        public PendingConstraintExpression Message
        {
            get { return Property("Message"); }
        }
        #endregion

        #region Throws
        //        public ResolvableConstraintBuilder Throws(Type type)
        //        {
        //            builder.Push(new ThrowsOperator());
        //            builder.Push(new ExactTypeConstraint(type));
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
        public PartialConstraintExpression With
        {
            get { return this.Append(new WithOperator()); }
        }
        #endregion

        #endregion

        public override string ToString()
        {
            return builder.Resolve().ToString();
        }

        #region Append Methods
        public PartialConstraintExpression Append(ConstraintOperator op)
        {
            builder.Append(op);
            return this;
        }

        public ConstraintExpression Append(Constraint constraint)
        {
            builder.Append(constraint);
            return new ConstraintExpression(this.builder);
        }
        #endregion
    }

    /// <summary>
    /// PendingConstraintExpression is one of the classes used to 
    /// represent a constraint in the process of being constructed 
    /// from a series of syntactic elements. 
    /// 
    /// Specifically, PendingConstraintExpression represents an 
    /// expression that may be complete, or not, depending on the
    /// interpretation of the last operator applied. Currently,
    /// this type is only used after recognizing a PropOperator, 
    /// which can either stand alone or be followed by a constraint.
    /// </summary>
    public class PendingConstraintExpression : PartialConstraintExpression
    {
        public PendingConstraintExpression() { }

        public PendingConstraintExpression(ConstraintBuilder builder)
            : base(builder) { }

        public Constraint Resolve()
        {
            return builder.Resolve();
        }

        public PartialConstraintExpression And
        {
            get { return this.Append(new AndOperator()); }
        }

        public PartialConstraintExpression Or
        {
            get { return this.Append(new OrOperator()); }
        }
    }

    public class ThrowsConstraintExpression : PendingConstraintExpression
    {
        public ThrowsConstraintExpression()
        {
            builder.Append(new ThrowsOperator());
        }
    }
}
