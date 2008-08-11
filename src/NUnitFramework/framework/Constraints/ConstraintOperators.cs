using System;
using System.Collections;
#if NET_2_0
using System.Collections.Generic;
#endif

namespace NUnit.Framework.Constraints
{
    #region Processing Stacks
#if NET_2_0
        public class OpStack : Stack<ConstraintOperator> { }
        public class ConstraintStack : Stack<Constraint> { }
#else
        public class OpStack : Stack { }
        public class ConstraintStack : Stack { }
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
        public abstract void Reduce(ConstraintStack stack);
    }
    #endregion

    #region PrefixOperator
    public abstract class PrefixOperator : ConstraintOperator
    {
        public override void Reduce(ConstraintStack stack)
        {
            stack.Push( ApplyPrefix( (Constraint)stack.Pop() ) );
        }

        public abstract Constraint ApplyPrefix(Constraint constraint);
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
            stack.Push( ApplyPrefix(stack.Count > 0 ? (Constraint)stack.Pop() : null) );
        }

        public override Constraint ApplyPrefix(Constraint constraint)
        {
            return new PropertyConstraint(name, constraint);
        }
    }
    #endregion
}
