// ****************************************************************
// Copyright 2012, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

using System;

namespace NUnit.Framework.Constraints
{
    /// <summary>
    /// ReusableConstraint wraps a constraint expression after 
    /// resolving it so that it can be reused consistently.
    /// </summary>
    public class ReusableConstraint : IResolveConstraint
    {
        private readonly Constraint constraint;

        /// <summary>
        /// Construct a ReusableConstraint from a constraint expression
        /// </summary>
        /// <param name="c">The expression to be resolved and reused</param>
        public ReusableConstraint(IResolveConstraint c)
        {
            this.constraint = c.Resolve();
        }

        /// <summary>
        /// Converts a constraint to a ReusableConstraint
        /// </summary>
        /// <param name="c">The constraint to be converted</param>
        /// <returns>A ReusableConstraint</returns>
        public static implicit operator ReusableConstraint(Constraint c)
        {
            return new ReusableConstraint(c);
        }

        /// <summary>
        /// Returns the string representation of the constraint.
        /// </summary>
        /// <returns>A string representing the constraint</returns>
        public override string ToString()
        {
            return constraint.ToString();
        }

        #region IResolveConstraint Members

        /// <summary>
        /// Resolves the ReusableConstraint by returning the constraint
        /// that it originally wrapped.
        /// </summary>
        /// <returns>A resolved constraint</returns>
        public Constraint Resolve()
        {
            return constraint;
        }

        #endregion
    }
}
