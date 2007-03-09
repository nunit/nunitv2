using System;
using System.Collections;
using NUnit.Framework.Constraints;

namespace NUnit.Framework.SyntaxHelpers
{
	/// <summary>
	/// The List class is a helper class with properties and methods
	/// that supply a number of constraints used with lists and collections.
	/// </summary>
	public class List : SyntaxHelper
	{
		/// <summary>
		/// Contains returns a constraint that tests
		/// whethner the actual value is a collection containing
		/// the expected value.
		/// </summary>
		/// <param name="expected"></param>
		/// <returns></returns>
		public static Constraint Contains( object expected )
		{
			return new CollectionContainsConstraint( expected );
		}
	}
}
