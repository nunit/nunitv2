// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

namespace NUnit.Framework.Constraints
{
    /// <summary>
    /// Represents a constraint that succeeds if none of the 
    /// members of a collection match a base constraint.
    /// </summary>
    public class NoneOperator : CollectionOperator
    {
        /// <summary>
        /// Returns a constraint that will apply the argument
        /// to the members of a collection, succeeding if
        /// none of them succeed.
        /// </summary>
        public override Constraint ApplyPrefix(Constraint constraint)
        {
            return new NoItemConstraint(constraint);
        }
    }
}
