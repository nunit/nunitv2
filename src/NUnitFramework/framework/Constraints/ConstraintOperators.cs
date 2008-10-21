// ****************************************************************
// Copyright 2008, Charlie Poole
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
    #region ConstraintOperator Base Class
    /// <summary>
    /// The ConstraintOperator class is used internally by a
    /// ConstraintBuilder to represent an operator that 
    /// modifies or combines constraints. 
    /// 
    /// Constraint operators use left and right precedence
    /// values to determine whether the top operator on the
    /// stack should be reduced before pushing a new operator.
    /// </summary>
    public abstract class ConstraintOperator
    {
        private object leftContext;
        private object rightContext;

		/// <summary>
		/// The precedence value used when the operator
		/// is about to be pushed to the stack.
		/// </summary>
		protected int left_precedence;
        
		/// <summary>
		/// The precedence value used when the operator
		/// is on the top of the stack.
		/// </summary>
		protected int right_precedence;

		/// <summary>
		/// The syntax element preceding this operator
		/// </summary>
        public object LeftContext
        {
            get { return leftContext; }
            set { leftContext = value; }
        }

		/// <summary>
		/// The syntax element folowing this operator
		/// </summary>
        public object RightContext
        {
            get { return rightContext; }
            set { rightContext = value; }
        }

		/// <summary>
		/// The precedence value used when the operator
		/// is about to be pushed to the stack.
		/// </summary>
		public virtual int LeftPrecedence
        {
            get { return left_precedence; }
        }

		/// <summary>
		/// The precedence value used when the operator
		/// is on the top of the stack.
		/// </summary>
		public virtual int RightPrecedence
        {
            get { return right_precedence; }
        }

		/// <summary>
		/// Reduce produces a constraint from the operator and 
		/// any arguments. It takes the arguments from the constraint 
		/// stack and pushes the resulting constraint on it.
		/// </summary>
		/// <param name="stack"></param>
		public abstract void Reduce(ConstraintBuilder.ConstraintStack stack);
    }
    #endregion

    #region Prefix Operators

    #region PrefixOperator
    /// <summary>
	/// PrefixOperator takes a single constraint and modifies
	/// it's action in some way.
	/// </summary>
    public abstract class PrefixOperator : ConstraintOperator
    {
		/// <summary>
		/// Reduce produces a constraint from the operator and 
		/// any arguments. It takes the arguments from the constraint 
		/// stack and pushes the resulting constraint on it.
		/// </summary>
		/// <param name="stack"></param>
		public override void Reduce(ConstraintBuilder.ConstraintStack stack)
        {
            stack.Push(ApplyPrefix(stack.Pop()));
        }

        /// <summary>
        /// Returns the constraint created by applying this
        /// prefix to another constraint.
        /// </summary>
        /// <param name="constraint"></param>
        /// <returns></returns>
        public abstract Constraint ApplyPrefix(Constraint constraint);
    }
    #endregion

    #region NotOperator
    /// <summary>
    /// Negates the test of the constraint it wraps.
    /// </summary>
    public class NotOperator : PrefixOperator
    {
        /// <summary>
        /// Constructs a new NotOperator
        /// </summary>
        public NotOperator()
        {
            // Not stacks on anything and only allows other
            // prefix ops to stack on top of it.
            this.left_precedence = this.right_precedence = 1;
        }

        /// <summary>
        /// Returns a NotConstraint applied to its argument.
        /// </summary>
        public override Constraint ApplyPrefix(Constraint constraint)
        {
            return new NotConstraint(constraint);
        }
    }
    #endregion

    #region Collection Operators
    public abstract class CollectionOperator : PrefixOperator
    {
        public CollectionOperator()
        {
            // Collection Operators stack on everything
            // and allow all other ops to stack on them
            this.left_precedence = 1;
            this.right_precedence = 10;
        }
    }

    public class AllOperator : CollectionOperator
    {
        public override Constraint ApplyPrefix(Constraint constraint)
        {
            return new AllItemsConstraint(constraint);
        }
    }

    public class SomeOperator : CollectionOperator
    {
        public override Constraint ApplyPrefix(Constraint constraint)
        {
            return new SomeItemsConstraint(constraint);
        }
    }

    public class NoneOperator : CollectionOperator
    {
        public override Constraint ApplyPrefix(Constraint constraint)
        {
            return new NoItemConstraint(constraint);
        }
    }
    #endregion

    #region WithOperator
    public class WithOperator : PrefixOperator
    {
        public WithOperator()
        {
            this.left_precedence = 1;
            this.right_precedence = 4;
        }

        public override Constraint ApplyPrefix(Constraint constraint)
        {
            return constraint;
        }
    }
    #endregion

    #region SelfResolving Operators

    #region SelfResolvingOperator
    /// <summary>
    /// SelfResolvingOperator is an abstract class used as a marker
    /// for derived operators that are able to reduce to a constraint
    /// whether or not another syntactic element follows.
    /// </summary>
    public abstract class SelfResolvingOperator : ConstraintOperator
    {
    }
    #endregion

    #region PropOperator
    /// <summary>
    /// PropOperator
    /// </summary>
    public class PropOperator : SelfResolvingOperator
    {
        private string name;
        public string Name
        {
            get { return name; }
        }

        public PropOperator(string name)
        {
            this.name = name;

            // Prop stacks on anything and allows only 
            // prefix operators to stack on it.
            this.left_precedence = this.right_precedence = 1;
        }

        /// <summary>
        /// Reduce produces a constraint from the operator and 
        /// any arguments. It takes the arguments from the constraint 
        /// stack and pushes the resulting constraint on it.
        /// </summary>
        /// <param name="stack"></param>
        public override void Reduce(ConstraintBuilder.ConstraintStack stack)
        {
            if (RightContext == null || RightContext is BinaryOperator)
                stack.Push(new PropertyExistsConstraint(name));
            else
                stack.Push(new PropertyConstraint(name, stack.Pop()));
        }
    }
    #endregion

    #region AttributeOperator
    /// <summary>
    /// AttributeOperator
    /// </summary>
    public class AttributeOperator : SelfResolvingOperator
    {
        private Type type;

        public AttributeOperator(Type type)
        {
            this.type = type;

            // Attribute stacks on anything and allows only 
            // prefix operators to stack on it.
            this.left_precedence = this.right_precedence = 1;
        }

        /// <summary>
        /// Reduce produces a constraint from the operator and 
        /// any arguments. It takes the arguments from the constraint 
        /// stack and pushes the resulting constraint on it.
        /// </summary>
        /// <param name="stack"></param>
        public override void Reduce(ConstraintBuilder.ConstraintStack stack)
        {
            if (RightContext == null || RightContext is BinaryOperator)
                stack.Push(new AttributeExistsConstraint(type));
            else
                stack.Push(new AttributeConstraint(type, stack.Pop()));
        }
    }
    #endregion

    #region ThrowsOperator
    public class ThrowsOperator : SelfResolvingOperator
    {
        public ThrowsOperator()
        {
            // ThrowsOperator stacks on everything but
            // it's always the first item on the stack
            // anyway. It is evaluated last of all ops.
            this.left_precedence = 1;
            this.right_precedence = 100;
        }

        public override void Reduce(ConstraintBuilder.ConstraintStack stack)
        {
            if (RightContext == null || RightContext is BinaryOperator)
                stack.Push(new ThrowsConstraint(null));
            else
                stack.Push(new ThrowsConstraint(stack.Pop()));
        }
    }
    #endregion

    #endregion

    #endregion

    #region Binary Operators

    #region BinaryOperator
    public abstract class BinaryOperator : ConstraintOperator
    {
        /// <summary>
        /// Reduce produces a constraint from the operator and 
        /// any arguments. It takes the arguments from the constraint 
        /// stack and pushes the resulting constraint on it.
        /// </summary>
        /// <param name="stack"></param>
        public override void Reduce(ConstraintBuilder.ConstraintStack stack)
        {
            Constraint right = stack.Pop();
            Constraint left = stack.Pop();
            stack.Push(ApplyOperator(left, right));
        }

        public override int LeftPrecedence
        {
            get
            {
                return RightContext is CollectionOperator
                    ? base.LeftPrecedence + 10
                    : base.LeftPrecedence;
            }
        }

        public override int RightPrecedence
        {
            get
            {
                return RightContext is CollectionOperator
                    ? base.RightPrecedence + 10
                    : base.RightPrecedence;
            }
        }

        public abstract Constraint ApplyOperator(Constraint left, Constraint right);
    }
    #endregion

    #region AndOperator
    public class AndOperator : BinaryOperator
    {
        public AndOperator()
        {
            this.left_precedence = this.right_precedence = 2;
        }

        public override Constraint ApplyOperator(Constraint left, Constraint right)
        {
            return new AndConstraint(left, right);
        }
    }
    #endregion

    #region OrOperator
    public class OrOperator : BinaryOperator
    {
        public OrOperator()
        {
            this.left_precedence = this.right_precedence = 3;
        }

        public override Constraint ApplyOperator(Constraint left, Constraint right)
        {
            return new OrConstraint(left, right);
        }
    }
    #endregion

    #endregion
}
