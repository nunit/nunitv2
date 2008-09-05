using System;
using System.Collections;
#if NET_2_0
using System.Collections.Generic;
#endif

namespace NUnit.Framework.Constraints
{
    #region ConstraintOperator
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

        public abstract Constraint ApplyPrefix(Constraint constraint);
    }
    #endregion

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

    #region NotOperator
    public class NotOperator : PrefixOperator
    {
        public NotOperator()
        {
            // Not stacks on anything and only allows other
            // prefix ops to stack on top of it.
            this.left_precedence = this.right_precedence = 1;
        }

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

    #region PropOperator
    /// <summary>
    /// PropOperator
    /// </summary>
    public class PropOperator : ConstraintOperator
    {
        private string name;
        public bool HasOperand;

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

    #region ThrowsOperator
    public class ThrowsOperator : PrefixOperator
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
            stack.Push(ApplyPrefix(stack.Empty ? null : stack.Pop()));
        }

        public override Constraint ApplyPrefix(Constraint constraint)
        {
            //Constraint baseConstraint = new ExactTypeConstraint(type);
            //if ( constraint != null )
            //    baseConstraint = new AndConstraint( baseConstraint, constraint);

            //return new ThrowsConstraint(baseConstraint);
            return new ThrowsConstraint( constraint );
        }
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
}
