using System;
using System.Collections;
#if NET_2_0
using System.Collections.Generic;
#endif

namespace NUnit.Framework.Constraints
{
    #region Operator Stack
    public class OpStack 
	{ 
#if NET_2_0
        private Stack<ConstraintOperator> stack = new Stack<ConstraintOperator>();
#else
		private Stack stack = new Stack();
#endif
        /// <summary>
        /// Gets a value indicating whether this <see cref="T:OpStack"/> is empty.
        /// </summary>
        /// <value><c>true</c> if empty; otherwise, <c>false</c>.</value>
        public bool Empty
        {
            get { return stack.Count == 0; }
        }

        /// <summary>
        /// Gets the topmost operator without modifying the stack.
        /// </summary>
        /// <value>The top.</value>
        public ConstraintOperator Top
        {
            get { return (ConstraintOperator)stack.Peek(); }
        }

        /// <summary>
        /// Pushes the specified operator onto the stack.
        /// </summary>
        /// <param name="op">The op.</param>
		public void Push( ConstraintOperator op )
		{
			stack.Push(op);
		}

        /// <summary>
        /// Pops the topmost operator from the stack.
        /// </summary>
        /// <returns></returns>
		public ConstraintOperator Pop()
		{
			return (ConstraintOperator)stack.Pop();
		}
	}
    #endregion

	#region Constraint Stack
	public class ConstraintStack
	{ 
#if NET_2_0
        private Stack<Constraint> stack = new Stack<Constraint>();
#else
		private Stack stack = new Stack();
#endif

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:ConstraintStack"/> is empty.
        /// </summary>
        /// <value><c>true</c> if empty; otherwise, <c>false</c>.</value>
        public bool Empty
        {
            get { return stack.Count == 0; }
        }

        /// <summary>
        /// Gets the topmost constraint without modifying the stac.
        /// </summary>
        /// <value>The topmost constraint</value>
        public Constraint Top
        {
            get { return (Constraint)stack.Peek(); }
        }

        /// <summary>
        /// Pushes the specified constraint.
        /// </summary>
        /// <param name="constraint">The constraint.</param>
		public void Push( Constraint constraint )
		{
			stack.Push(constraint);
		}

		public Constraint Pop()
		{
			return (Constraint)stack.Pop();
		}
	}
	#endregion

    #region ConstraintOperator
    /// <summary>
    /// The ConstraintOperator class is used internally by a
    /// ConstraintBuilder to represent an operator that 
    /// modifies or combines constraints.
    /// </summary>
    public abstract class ConstraintOperator
    {
        public abstract int LeftPrecedence { get; }
        public abstract int RightPrecedence { get; }
        public abstract void Reduce(ConstraintStack stack);
    }
    #endregion

    #region PrefixOperator
    public abstract class PrefixOperator : ConstraintOperator
    {
        /// <summary>
        /// All PrefixOperators have a precedence of 1
        /// </summary>
        public override int LeftPrecedence
        {
            get { return 1; }
        }

        /// <summary>
        /// All PrefixOperators have a precedence of 1
        /// </summary>
        public override int RightPrecedence
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
        public override int RightPrecedence
        {
            get { return 10; }
        }
        public override Constraint ApplyPrefix(Constraint constraint)
        {
            return new AllItemsConstraint(constraint);
        }
    }
    #endregion

    #region SomeOperator
    public class SomeOperator : PrefixOperator
    {
        public override int RightPrecedence
        {
            get { return 10; }
        }
        public override Constraint ApplyPrefix(Constraint constraint)
        {
            return new SomeItemsConstraint(constraint);
        }
    }
    #endregion

    #region NoneOperator
    public class NoneOperator : PrefixOperator
    {
        public override int RightPrecedence
        {
            get { return 10; }
        }
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

        public string Name
        {
            get { return name; }
        }

        public override int LeftPrecedence
        {
            get { return 1; }
        }

        public override int RightPrecedence
        {
            get { return 1; }
        }

        public PropOperator(string name)
        {
            this.name = name;
        }

        public override void Reduce(ConstraintStack stack)
        {
            stack.Push(new PropertyExistsConstraint(name));
        }
    }
    #endregion

    #region PropValOperator
    public class PropValOperator : PrefixOperator
    {
        private string name;

        public PropValOperator(string name)
        {
            this.name = name;
        }

        public override Constraint ApplyPrefix(Constraint constraint)
        {
            return new PropertyConstraint(name, constraint);
        }
    }
    #endregion

    #region ThrowsOperator
    public class ThrowsOperator : PrefixOperator
    {
        //private Type type;

        public override int RightPrecedence
        {
            get { return 100; }
        }

        //public ThrowsOperator(Type type)
        //{
        //    this.type = type;
        //}

        public override void Reduce(ConstraintStack stack)
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
        public override int LeftPrecedence
        {
            get { return 2; }
        }

        public override int RightPrecedence
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
        public override int LeftPrecedence
        {
            get { return 3; }
        }

        public override int RightPrecedence
        {
            get { return 3; }
        }

        public override Constraint ApplyOperator(Constraint left, Constraint right)
        {
            return new OrConstraint(left, right);
        }
    }
    #endregion

    #region WithOperator
    //public class WithOperator : PrefixOperator
    //{
    //    public override int LeftPrecedence
    //    {
    //        get { return 4; }
    //    }

    //    public override int RightPrecedence
    //    {
    //        get { return 4; }
    //    }

    //    public override Constraint ApplyPrefix(Constraint constraint)
    //    {
    //        return constraint;
    //    }
    //}
    #endregion
}
