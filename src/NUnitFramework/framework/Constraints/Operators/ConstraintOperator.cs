// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

namespace NUnit.Framework.Constraints
{
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
}
