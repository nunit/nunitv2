// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;

namespace NUnit.Framework.Constraints
{
    public class RangeConstraint : Constraint
    {
        private IComparable from;
        private IComparable to;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:RangeConstraint"/> class.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        public RangeConstraint(IComparable from, IComparable to) : base( from, to )
        {
            this.from = from;
            this.to = to;
        }

        /// <summary>
        /// Test whether the constraint is satisfied by a given value
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>True for success, false for failure</returns>
        public override bool Matches(object actual)
        {
            this.actual = actual;

            return Numerics.Compare(from, actual) <= 0 &&
                   Numerics.Compare(to, actual) >= 0;
        }

        /// <summary>
        /// Write the constraint description to a MessageWriter
        /// </summary>
        /// <param name="writer">The writer on which the description is displayed</param>
        public override void WriteDescriptionTo(MessageWriter writer)
        {

            writer.Write("in range ({0},{1})", from, to);
        }
    }
}
