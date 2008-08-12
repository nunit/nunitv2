using System;
using System.Collections;
#if NET_2_0
using System.Collections.Generic;
#endif

namespace NUnit.Framework.Constraints
{
    #region Operator Stack
#if NET_2_0
        public class OpStack : Stack<ConstraintOperator> { }
#else
    public class OpStack 
	{ 
		private Stack stack = new Stack();

		public int Count
		{
			get { return stack.Count; }
		}

		public void Push( ConstraintOperator op )
		{
			stack.Push(op);
		}

		public ConstraintOperator Pop()
		{
			return (ConstraintOperator)stack.Pop();
		}

		public ConstraintOperator Peek()
		{
			return (ConstraintOperator)stack.Peek();
		}
	}
#endif
    #endregion

	#region Constraint Stack
#if NET_2_0
    public class ConstraintStack : Stack<Constraint> { }
#else
	public class ConstraintStack
	{ 
		private Stack stack = new Stack();

		public int Count
		{
			get { return stack.Count; }
		}

		public void Push( Constraint constraint )
		{
			stack.Push(constraint);
		}

		public Constraint Pop()
		{
			return (Constraint)stack.Pop();
		}

		public Constraint Peek()
		{
			return (Constraint)stack.Peek();
		}
	}
#endif
	#endregion

    #region ConstraintOperator
    /// <summary>
    /// The ConstraintOperator class is used internally by a
    /// ConstraintBuilder to represent an operator that 
    /// modifies or combines constraints.
    /// </summary>
    public abstract class ConstraintOperator
    {
        public abstract int Precedence { get; }
        public abstract void Reduce(ConstraintStack stack);
    }
    #endregion

    #region PrefixOperator
    public abstract class PrefixOperator : ConstraintOperator
    {
        /// <summary>
        /// All PrefixOperators have a precedence of 1
        /// </summary>
        public override int Precedence
        {
            get { return 1; }
        }
    
        public override void Reduce(ConstraintStack stack)
        {
            stack.Push( ApplyPrefix( stack.Pop() ) );
        }

        public abstract Constraint ApplyPrefix(Constraint constraint);
    }
    #endregion

    #region BinaryOperator
    public abstract class BinaryOperator : ConstraintOperator
    {
        public override void Reduce(ConstraintStack stack)
        {
            Constraint right = stack.Pop();
            Constraint left = stack.Pop();
            stack.Push(ApplyOperator(left, right));
        }

        public abstract Constraint ApplyOperator(Constraint left, Constraint right);
    }
    #endregion

    #region NotOperator
    public class NotOperator : PrefixOperator
    {
        public override Constraint ApplyPrefix(Constraint constraint)
        {
            return new NotConstraint(constraint);
        }
    }
    #endregion

    #region AllOperator
    public class AllOperator : PrefixOperator
    {
        public override Constraint ApplyPrefix(Constraint constraint)
        {
            return new AllItemsConstraint(constraint);
        }
    }
    #endregion

    #region SomeOperator
    public class SomeOperator : PrefixOperator
    {
        public override Constraint ApplyPrefix(Constraint constraint)
        {
            return new SomeItemsConstraint(constraint);
        }
    }
    #endregion

    #region NoneOperator
    public class NoneOperator : PrefixOperator
    {
        public override Constraint ApplyPrefix(Constraint constraint)
        {
            return new NoItemConstraint(constraint);
        }
    }
    #endregion

    #region PropOperator
    public class PropOperator : PrefixOperator
    {
        private string name;

        public PropOperator(string name)
        {
            this.name = name;
        }

        public override void Reduce(ConstraintStack stack)
        {
            stack.Push(ApplyPrefix(stack.Count > 0 ? stack.Pop() : null));
        }

        public override Constraint ApplyPrefix(Constraint constraint)
        {
            return new PropertyConstraint(name, constraint);
        }
    }
    #endregion

    public class ThrowsOperator : PrefixOperator
    {
        private Type type;

        public ThrowsOperator(Type type)
        {
            this.type = type;
        }

        public override void Reduce(ConstraintStack stack)
        {
            stack.Push(ApplyPrefix(stack.Count > 0 ? stack.Pop() : null));
        }

        public override Constraint ApplyPrefix(Constraint constraint)
        {
            return new ThrowsConstraint( type, constraint );
        }
    }

    #region AndOperator
    public class AndOperator : BinaryOperator
    {
        public override int Precedence
        {
            get { return 2; }
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
        public override int Precedence
        {
            get { return 3; }
        }
        public override Constraint ApplyOperator(Constraint left, Constraint right)
        {
            return new OrConstraint(left, right);
        }
    }
    #endregion
}
