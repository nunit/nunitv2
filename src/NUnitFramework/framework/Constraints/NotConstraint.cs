// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;

namespace NUnit.Framework.Constraints
{
    public class NotConstraint : Constraint
    {
        Constraint baseConstraint;

        public NotConstraint(Constraint baseConstraint)
        {
            this.baseConstraint = baseConstraint;
        }

        public override bool Matches(object actual)
        {
            this.actual = actual;
            return !baseConstraint.Matches(actual);
        }

        public override void WriteDescriptionTo( MessageWriter writer )
        {
            writer.WritePredicate( "not" );
            baseConstraint.WriteDescriptionTo( writer );
        }

		public override void WriteActualValueTo(MessageWriter writer)
		{
			baseConstraint.WriteActualValueTo (writer);
		}

    }
}
