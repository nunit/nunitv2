// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

namespace NUnit.Framework.Constraints
{
    /// <summary>
    /// BasicConstraint is the abstract base for constraints that
    /// perform a simple comparison to a constant value.
    /// </summary>
    public abstract class BasicConstraint : Constraint
    {
        private readonly object expected;
        private readonly string description;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:BasicConstraint"/> class.
        /// </summary>
        /// <param name="expected">The expected.</param>
        /// <param name="description">The description.</param>
        protected BasicConstraint(object expected, string description)
        {
            this.expected = expected;
            this.description = description;
        }

        /// <summary>
        /// Test whether the constraint is satisfied by a given value
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>True for success, false for failure</returns>
        public override bool Matches(object actual)
        {
            this.actual = actual;

            if (actual == null && expected == null)
                return true;

            if (actual == null || expected == null)
                return false;
            
            return expected.Equals(actual);
        }

        /// <summary>
        /// Write the constraint description to a MessageWriter
        /// </summary>
        /// <param name="writer">The writer on which the description is displayed</param>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.Write(description);
        }
    }
}
