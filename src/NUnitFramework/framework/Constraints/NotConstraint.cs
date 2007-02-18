// *****************************************************
// Copyright 2007, Charlie Poole
// Licensed under the NUnit License, see license.txt
// *****************************************************

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
