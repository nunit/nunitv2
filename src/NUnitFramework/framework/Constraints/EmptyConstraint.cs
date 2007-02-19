// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using System.Collections;

namespace NUnit.Framework.Constraints
{
	public class EmptyConstraint : Constraint
	{
		public override bool Matches(object actual)
		{
			this.actual = actual;

			return actual is string && (string)actual == string.Empty
				|| actual is ICollection && ((ICollection)actual).Count == 0;
		}

		public override void WriteDescriptionTo(MessageWriter writer)
		{
			writer.Write( "<empty>" );
		}
	}
}
