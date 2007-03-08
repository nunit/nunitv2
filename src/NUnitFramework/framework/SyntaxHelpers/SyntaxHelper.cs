using System;
using NUnit.Framework.Constraints;

namespace NUnit.Framework.SyntaxHelpers
{
	/// <summary>
	/// SyntaxHelper is the abstract base class for all
	/// syntax helpers.
	/// </summary>
	public abstract class SyntaxHelper
	{
		#region Prefix Operators
		/// <summary>
		/// Is.Not returns a ConstraintBuilder, which will negate
		/// the constraint that follows it.
		/// </summary>
		public static ConstraintBuilder Not
		{
			get { return new ConstraintBuilder().Not; }
		}

		/// <summary>
		/// Is.All returns a ConstraintBuilder, which will apply
		/// the following constraint to all members of a collection.
		/// </summary>
		public static ConstraintBuilder All
		{
			get { return new ConstraintBuilder().All; }
		}
		#endregion
	}
}
