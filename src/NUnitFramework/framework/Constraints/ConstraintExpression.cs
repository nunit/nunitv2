// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using System.Collections;

namespace NUnit.Framework.Constraints
{
    /// <summary>
    /// ConstraintExpression represents a compound constraint in the 
    /// process of being constructed from a series of syntactic elements.
    /// 
    /// Individual elements are appended to the expression as they are
    /// reognized. Once an actual Constraint is appended, the expression
    /// returns a resolvable Constraint.
    /// </summary>
    public class ConstraintExpression
    {
        #region Instance Fields
        /// <summary>
        /// The ConstraintBuilder holding the elements recognized so far
        /// </summary>
        protected ConstraintBuilder builder;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ConstraintExpression"/> class.
        /// </summary>
        public ConstraintExpression()
        {
            this.builder = new ConstraintBuilder();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ConstraintExpression"/> 
        /// class passing in a ConstraintBuilder, which may be pre-populated.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public ConstraintExpression(ConstraintBuilder builder)
        {
            this.builder = builder;
        }
        #endregion

        #region Prefix Operators

        #region Not
        /// <summary>
        /// Modifies the ConstraintBuilder by pushing a Not operator on the stack.
        /// </summary>
        public ConstraintExpression Not
        {
            get { return this.Append(new NotOperator()); }
        }

        /// <summary>
        /// Modifies the ConstraintBuilder by pushing a Not operator on the stack.
        /// </summary>
        public ConstraintExpression No
        {
            get { return this.Append(new NotOperator()); }
        }
        #endregion

        #region All
        /// <summary>
        /// Modifies the ConstraintBuilder by pushing an All operator on the stack.
        /// </summary>
        public ConstraintExpression All
        {
            get { return this.Append(new AllOperator()); }
        }
        #endregion

        #region Some
        /// <summary>
        /// Modifies the ConstraintBuilder by pushing a Some operator on the stack.
        /// </summary>
        public ConstraintExpression Some
        {
            get { return this.Append(new SomeOperator()); }
        }
        #endregion

        #region None
        /// <summary>
        /// Modifies the constraint builder by pushing a None operator on the stack
        /// </summary>
        public ConstraintExpression None
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
        public ResolvableConstraintExpression Property(string name)
        {
            builder.Append(new PropOperator(name));
            return new ResolvableConstraintExpression(builder);
        }

        /// <summary>
        /// Modifies the ConstraintBuilder by pushing a Prop operator
        /// for the Length property on the operator stack.
        /// </summary>
        /// <returns></returns>
        public ResolvableConstraintExpression Length
        {
            get { return Property("Length"); }
        }

        /// <summary>
        /// Modifies the ConstraintBuilder by pushing a Prop operator
        /// for the Count property on the operator stack.
        /// </summary>
        /// <returns></returns>
        public ResolvableConstraintExpression Count
        {
            get { return Property("Count"); }
        }

        /// <summary>
        /// Modifies the ConstraintBuilder by pushing a Prop operator
        /// for the Message property on the operator stack.
        /// </summary>
        /// <returns></returns>
        public ResolvableConstraintExpression Message
        {
            get { return Property("Message"); }
        }
        #endregion

        #region Attribute
        /// <summary>
        /// Modifies the expression by appending an AttributeOperator.
        /// </summary>
        public ResolvableConstraintExpression Attribute(Type expectedType)
        {
            builder.Append(new AttributeOperator(expectedType));
            return new ResolvableConstraintExpression(builder);
        }

#if NET_2_0
        /// <summary>
        /// Resolves the chain of constraints using an
        /// AttributeConstraint as base.
        /// </summary>
        public ResolvableConstraintExpression Attribute<T>()
        {
            return Attribute(typeof(T));
        }
#endif
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
        public ConstraintExpression With
        {
            get { return this.Append(new WithOperator()); }
        }
        #endregion

        #endregion

        #region Constraints Without Arguments

        /// <summary>
        /// Resolves the chain of constraints using
        /// EqualConstraint(null) as base.
        /// </summary>
        public NullConstraint Null
        {
            get { return this.Append(new NullConstraint()) as NullConstraint; }
        }

        /// <summary>
        /// Resolves the chain of constraints using
        /// EqualConstraint(true) as base.
        /// </summary>
        public TrueConstraint True
        {
            get { return this.Append(new TrueConstraint()) as TrueConstraint; }
        }

        /// <summary>
        /// Resolves the chain of constraints using
        /// EqualConstraint(false) as base.
        /// </summary>
        public FalseConstraint False
        {
            get { return this.Append(new FalseConstraint()) as FalseConstraint; }
        }

        /// <summary>
        /// Resolves the chain of constraints using
        /// Is.NaN as base.
        /// </summary>
        public NaNConstraint NaN
        {
            get { return this.Append(new NaNConstraint()) as NaNConstraint; }
        }

        /// <summary>
        /// Resolves the chain of constraints using
        /// Is.Empty as base.
        /// </summary>
        public EmptyConstraint Empty
        {
            get { return this.Append(new EmptyConstraint()) as EmptyConstraint; }
        }

        /// <summary>
        /// Resolves the chain of constraints using
        /// Is.Unique as base.
        /// </summary>
        public UniqueItemsConstraint Unique
        {
            get { return this.Append(new UniqueItemsConstraint()) as UniqueItemsConstraint; }
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// CollectionOrderedConstraint as base.
        /// </summary>
        public CollectionOrderedConstraint Ordered()
        {
            return this.Append(new CollectionOrderedConstraint()) as CollectionOrderedConstraint;
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// CollectionOrderedConstraint as base.
        /// </summary>
        public CollectionOrderedConstraint Ordered(IComparer comparer)
        {
            return this.Append(new CollectionOrderedConstraint(comparer)) as CollectionOrderedConstraint;
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// CollectionOrderedConstraint as base.
        /// </summary>
        public CollectionOrderedConstraint OrderedBy(string propertyName)
        {
            return this.Append(new CollectionOrderedConstraint(propertyName)) as CollectionOrderedConstraint;
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// CollectionOrderedConstraint as base.
        /// </summary>
        public CollectionOrderedConstraint OrderedBy(string propertyName, IComparer comparer)
        {
            return this.Append(new CollectionOrderedConstraint(propertyName)) as CollectionOrderedConstraint;
        }

        /// <summary>
        /// Resolves the chain of constraints using
        /// BinarySerializableConstraint as base.
        /// </summary>
        public BinarySerializableConstraint BinarySerializable
        {
            get { return Append(new BinarySerializableConstraint()) as BinarySerializableConstraint; }
        }

        /// <summary>
        /// Resolves the chain of constraints using
        /// XmlSerializableConstraint as base.
        /// </summary>
        public XmlSerializableConstraint XmlSerializable
        {
            get { return Append(new XmlSerializableConstraint()) as XmlSerializableConstraint; }
        }
        #endregion

        #region Constraints with an expected value argument

        #region Equality and Identity
        /// <summary>
        /// Resolves the chain of constraints using an
        /// EqualConstraint as base.
        /// </summary>
        public EqualConstraint EqualTo(object expected)
        {
            return this.Append( new EqualConstraint(expected) ) as EqualConstraint;
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// SameAsConstraint as base.
        /// </summary>
        public SameAsConstraint SameAs(object expected)
        {
            return this.Append(new SameAsConstraint(expected)) as SameAsConstraint;
        }
        #endregion

        #region Comparison Constraints
        /// <summary>
        /// Resolves the chain of constraints using a
        /// LessThanConstraint as base.
        /// </summary>
        public LessThanConstraint LessThan(IComparable expected)
        {
            return this.Append(new LessThanConstraint(expected)) as LessThanConstraint;
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// GreaterThanConstraint as base.
        /// </summary>
        public GreaterThanConstraint GreaterThan(IComparable expected)
        {
            return this.Append(new GreaterThanConstraint(expected)) as GreaterThanConstraint;
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// LessThanOrEqualConstraint as base.
        /// </summary>
        public LessThanOrEqualConstraint LessThanOrEqualTo(IComparable expected)
        {
            return this.Append(new LessThanOrEqualConstraint(expected)) as LessThanOrEqualConstraint;
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// LessThanOrEqualConstraint as base.
        /// </summary>
        public LessThanOrEqualConstraint AtMost(IComparable expected)
        {
            return this.Append(new LessThanOrEqualConstraint(expected)) as LessThanOrEqualConstraint;
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// GreaterThanOrEqualConstraint as base.
        /// </summary>
        public GreaterThanOrEqualConstraint GreaterThanOrEqualTo(IComparable expected)
        {
            return this.Append(new GreaterThanOrEqualConstraint(expected)) as GreaterThanOrEqualConstraint;
        }
        /// <summary>
        /// Resolves the chain of constraints using a
        /// GreaterThanOrEqualConstraint as base.
        /// </summary>
        public GreaterThanOrEqualConstraint AtLeast(IComparable expected)
        {
            return this.Append(new GreaterThanOrEqualConstraint(expected)) as GreaterThanOrEqualConstraint;
        }
        #endregion

        #region Type Constraints
        /// <summary>
        /// Resolves the chain of constraints using an
        /// ExactTypeConstraint as base.
        /// </summary>
        public ExactTypeConstraint TypeOf(Type expectedType)
        {
            return this.Append(new ExactTypeConstraint(expectedType)) as ExactTypeConstraint;
        }

        /// <summary>
        /// Resolves the chain of constraints using an
        /// InstanceOfTypeConstraint as base.
        /// </summary>
        public InstanceOfTypeConstraint InstanceOf(Type expectedType)
        {
            return this.Append(new InstanceOfTypeConstraint(expectedType)) as InstanceOfTypeConstraint;
        }

        /// <summary>
        /// Resolves the chain of constraints using an
        /// InstanceOfTypeConstraint as base.
        /// </summary>
        [Obsolete("Use InstanceOf")]
        public InstanceOfTypeConstraint InstanceOfType(Type expectedType)
        {
            return this.Append(new InstanceOfTypeConstraint(expectedType)) as InstanceOfTypeConstraint;
        }

        /// <summary>
        /// Resolves the chain of constraints using an
        /// AssignableFromConstraint as base.
        /// </summary>
        public AssignableFromConstraint AssignableFrom(Type expectedType)
        {
            return this.Append(new AssignableFromConstraint(expectedType)) as AssignableFromConstraint;
        }

        /// <summary>
        /// Resolves the chain of constraints using an
        /// AssignableToConstraint as base.
        /// </summary>
        public AssignableToConstraint AssignableTo(Type expectedType)
        {
            return this.Append(new AssignableToConstraint(expectedType)) as AssignableToConstraint;
        }

#if NET_2_0
        /// <summary>
        /// Resolves the chain of constraints using an
        /// ExactTypeConstraint as base.
        /// </summary>
        public ExactTypeConstraint TypeOf<T>()
        {
            return TypeOf(typeof(T));
        }

        /// <summary>
        /// Resolves the chain of constraints using an
        /// InstanceOfTypeConstraint as base.
        /// </summary>
        [Obsolete("Use InstanceOf")]
        public InstanceOfTypeConstraint InstanceOfType<T>()
        {
            return InstanceOfType(typeof(T));
        }

        /// <summary>
        /// Resolves the chain of constraints using an
        /// InstanceOfTypeConstraint as base.
        /// </summary>
        public InstanceOfTypeConstraint InstanceOf<T>()
        {
            return InstanceOf(typeof(T));
        }

        /// <summary>
        /// Resolves the chain of constraints using an
        /// AssignableFromConstraint as base.
        /// </summary>
        public AssignableFromConstraint AssignableFrom<T>()
        {
            return AssignableFrom(typeof(T));
        }

        /// <summary>
        /// Resolves the chain of constraints using an
        /// AssignableToConstraint as base.
        /// </summary>
        public AssignableToConstraint AssignableTo<T>()
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
        public ContainsConstraint Contains(string expected)
        {
            return this.Append(new ContainsConstraint(expected)) as ContainsConstraint;
        }

        /// <summary>
        /// Resolves the chain of constraints using a 
        /// CollectionContainsConstraint as base.
        /// </summary>
        /// <param name="expected"></param>
        /// <returns></returns>
        public CollectionContainsConstraint Contains(object expected)
        {
            return this.Append(new CollectionContainsConstraint(expected)) as CollectionContainsConstraint;
        }

        /// <summary>
        /// Resolves the chain of constraints using a 
        /// CollectionContainsConstraint as base.
        /// </summary>
        /// <param name="expected">The expected object</param>
        public CollectionContainsConstraint Member(object expected)
        {
            return this.Append(new CollectionContainsConstraint(expected)) as CollectionContainsConstraint;
        }
        #endregion

        #region String Constraints
        /// <summary>
        /// Pushes a SubstringConstraint on the stack and
        /// returns a Modifier for it.
        /// </summary>
        /// <param name="substring"></param>
        /// <returns></returns>
        public SubstringConstraint ContainsSubstring(string substring)
        {
            return this.Append(new SubstringConstraint(substring)) as SubstringConstraint;
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// StartsWithConstraint as base.
        /// </summary>
        public StartsWithConstraint StartsWith(string substring)
        {
            return this.Append(new StartsWithConstraint(substring)) as StartsWithConstraint;
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// StringEndingConstraint as base.
        /// </summary>
        public EndsWithConstraint EndsWith(string substring)
        {
            return this.Append(new EndsWithConstraint(substring)) as EndsWithConstraint;
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// StringMatchingConstraint as base.
        /// </summary>
        public RegexConstraint Matches(string pattern)
        {
            return this.Append(new RegexConstraint(pattern)) as RegexConstraint;
        }
        #endregion

        #region Collection Constraints
        /// <summary>
        /// Resolves the chain of constraints using a
        /// CollectionEquivalentConstraint as base.
        /// </summary>
        public CollectionEquivalentConstraint EquivalentTo(IEnumerable expected)
        {
            return this.Append(new CollectionEquivalentConstraint(expected)) as CollectionEquivalentConstraint;
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// CollectionContainingConstraint as base.
        /// </summary>
        public CollectionContainsConstraint CollectionContaining(object expected)
        {
            return this.Append(new CollectionContainsConstraint(expected)) as CollectionContainsConstraint;
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// CollectionSubsetConstraint as base.
        /// </summary>
        public CollectionSubsetConstraint SubsetOf(IEnumerable expected)
        {
            return this.Append(new CollectionSubsetConstraint(expected)) as CollectionSubsetConstraint;
        }
        #endregion

        #region Path Constraints
        /// <summary>
        /// Resolves the chain of constraints using a
        /// SamePathConstraint as base.
        /// </summary>
        public SamePathConstraint SamePath(string expected)
        {
            return this.Append(new SamePathConstraint(expected)) as SamePathConstraint;
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// SamePathOrUnderConstraint as base.
        /// </summary>
        public SamePathOrUnderConstraint SamePathOrUnder(string expected)
        {
            return this.Append(new SamePathOrUnderConstraint(expected)) as SamePathOrUnderConstraint;
        }
        #endregion

        #endregion

        #region Constraints with two arguments

        #region Range Constraint
        public RangeConstraint InRange(IComparable from, IComparable to)
        {
            return this.Append(new RangeConstraint(from, to)) as RangeConstraint;
        }
        #endregion

        #endregion

        public override string ToString()
        {
            return builder.Resolve().ToString();
        }

        #region Append Methods
        public ConstraintExpression Append(ConstraintOperator op)
        {
            builder.Append(op);
            return this;
        }

        public Constraint Append(Constraint constraint)
        {
            builder.Append(constraint);
            return constraint;
        }
        #endregion
    }

    /// <summary>
    /// ResolvableConstraintExpression is used to represent a compound
    /// constraint being constructed at a point where the last operator
    /// may either terminate the expression or may have additional 
    /// qualifying constraints added to it. 
    /// 
    /// It is used, for example, for a Property element or for
    /// an Exception element, either of which may be optionally
    /// followed by constraints that apply to the property or 
    /// exception.
    /// </summary>
    public class ResolvableConstraintExpression : ConstraintExpression, IResolveConstraint
    {
        public ResolvableConstraintExpression() { }

        public ResolvableConstraintExpression(ConstraintBuilder builder)
            : base(builder) { }

        public ConstraintExpression And
        {
            get { return this.Append(new AndOperator()); }
        }

        public ConstraintExpression Or
        {
            get { return this.Append(new OrOperator()); }
        }

        #region IResolveConstraint Members
        Constraint IResolveConstraint.Resolve()
        {
            return builder.Resolve();
        }
        #endregion
    }

    public class PropertyConstraintExpression : ResolvableConstraintExpression
    {
        public PropertyConstraintExpression(string name)
        {
            builder.Append(new PropOperator(name));
        }
    }

    public class AttributeConstraintExpression : ResolvableConstraintExpression
    {
        public AttributeConstraintExpression(Type type)
        {
            builder.Append(new AttributeOperator(type));
        }

        //public AttributeConstraintExpression(string name)
        //{
        //    builder.Append(new AttributeOperator(name));
        //}
    }

    public class ThrowsConstraintExpression : ResolvableConstraintExpression
    {
        public ThrowsConstraintExpression()
        {
            builder.Append(new ThrowsOperator());
        }
    }
}
