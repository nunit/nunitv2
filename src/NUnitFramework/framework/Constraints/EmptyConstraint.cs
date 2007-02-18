// *****************************************************
// Copyright 2007, Charlie Poole
// Licensed under the NUnit License, see license.txt
// *****************************************************

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
