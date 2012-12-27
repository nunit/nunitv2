// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

namespace NUnit.Framework.Constraints
{
    /// <summary>
    /// StringConstraint is the abstract base for constraints
    /// that operate on strings. It supports the IgnoreCase
    /// modifier for string operations.
    /// </summary>
    public abstract class StringConstraint : Constraint
    {
        /// <summary>
        /// The expected value
        /// </summary>
        protected string expected;

        /// <summary>
        /// Indicates whether tests should be case-insensitive
        /// </summary>
        protected bool caseInsensitive;

        /// <summary>
        /// Constructs a StringConstraint given an expected value
        /// </summary>
        /// <param name="expected">The expected value</param>
        public StringConstraint(string expected)
            : base(expected)
        {
            this.expected = expected;
        }

        /// <summary>
        /// Modify the constraint to ignore case in matching.
        /// </summary>
        public StringConstraint IgnoreCase
        {
            get { caseInsensitive = true; return this; }
        }
    }
}
