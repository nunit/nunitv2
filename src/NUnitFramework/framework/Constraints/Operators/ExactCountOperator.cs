// ****************************************************************
// Copyright 2011, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

namespace NUnit.Framework.Constraints
{
    /// <summary>
    /// Represents a constraint that succeeds if the specified 
    /// count of members of a collection match a base constraint.
    /// </summary>
    public class ExactCountOperator : CollectionOperator
    {
        private int expectedCount;

        /// <summary>
        /// Construct an ExactCountOperator for a specified count
        /// </summary>
        /// <param name="expectedCount">The expected count</param>
        public ExactCountOperator(int expectedCount)
        {
            this.expectedCount = expectedCount;
        }

        /// <summary>
        /// Returns a constraint that will apply the argument
        /// to the members of a collection, succeeding if
        /// none of them succeed.
        /// </summary>
        public override Constraint ApplyPrefix(Constraint constraint)
        {
            return new ExactCountConstraint(expectedCount, constraint);
        }
    }
}
