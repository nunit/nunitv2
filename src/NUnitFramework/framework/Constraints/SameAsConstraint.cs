// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;

namespace NUnit.Framework.Constraints
{
    public class SameAsConstraint : Constraint
    {
        protected object expected;

        public SameAsConstraint(object expected)
        {
            this.expected = expected;
        }

        public override bool Matches(object actual)
        {
            this.actual = actual;

            return Object.ReferenceEquals(expected,actual);
        }

        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.WritePredicate("same as");
            writer.WriteExpectedValue(expected);
        }
    }
}
