// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using System.Collections;
using NUnit.Framework.Constraints;

namespace NUnit.Framework.SyntaxHelpers
{
	/// <summary>
	/// The List class is a helper class with properties and methods
	/// that supply a number of constraints used with lists and collections.
	/// </summary>
	public class List
	{
		/// <summary>
		/// List.Contains returns a constraint that succeeds
		/// if the actual value is a collection containing
		/// the expected value.
		/// </summary>
		/// <param name="expected"></param>
		/// <returns></returns>
		public static Constraint Contains( object expected )
		{
			return new CollectionContainsConstraint( expected );
		}

		/// <summary>
		/// List.DoesNotContain returns a constraint that succeeds
		/// if the actual value is a collection not containing
		/// the expected value.
		/// </summary>
		/// <param name="expected"></param>
		/// <returns></returns>
		public static Constraint DoesNotContain( object expected )
		{
			return new NotConstraint( new CollectionContainsConstraint( expected ) );
		}
	}
}
