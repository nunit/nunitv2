// ***************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

namespace NUnit.Framework.Constraints
{
    /// <summary>
    /// Summary description for SamePathConstraint.
    /// </summary>
    public class SamePathConstraint : PathConstraint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SamePathConstraint"/> class.
        /// </summary>
        /// <param name="expected">The expected path</param>
        public SamePathConstraint(string expected) : base(expected) { }

        /// <summary>
        /// Test whether the constraint is satisfied by a given value
        /// </summary>
        /// <param name="expectedPath">The expected path</param>
        /// <param name="actualPath">The actual path</param>
        /// <returns>True for success, false for failure</returns>
        protected override bool IsMatch(string expectedPath, string actualPath)
        {
            return string.Compare(Canonicalize(expectedPath), Canonicalize(actualPath), caseInsensitive) == 0;
        }

        /// <summary>
        /// Write the constraint description to a MessageWriter
        /// </summary>
        /// <param name="writer">The writer on which the description is displayed</param>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.WritePredicate("Path matching");
            writer.WriteExpectedValue(expectedPath);
        }
    }
}
