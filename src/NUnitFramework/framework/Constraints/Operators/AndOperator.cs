// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

namespace NUnit.Framework.Constraints
{
    /// <summary>
    /// Operator that requires both it's arguments to succeed
    /// </summary>
    public class AndOperator : BinaryOperator
    {
        /// <summary>
        /// Construct an AndOperator
        /// </summary>
        public AndOperator()
        {
            this.left_precedence = this.right_precedence = 2;
        }

        /// <summary>
        /// Apply the operator to produce an AndConstraint
        /// </summary>
        public override Constraint ApplyOperator(Constraint left, Constraint right)
        {
            return new AndConstraint(left, right);
        }
    }
}
