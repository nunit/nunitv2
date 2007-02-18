// *****************************************************
// Copyright 2007, Charlie Poole
// Licensed under the NUnit License, see license.txt
// *****************************************************

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
