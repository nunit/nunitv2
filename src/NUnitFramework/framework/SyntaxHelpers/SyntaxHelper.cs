// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

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
		/// Not returns a ConstraintBuilder that negates
		/// the constraint that follows it.
		/// </summary>
		public static ConstraintBuilder Not
		{
			get { return new ConstraintBuilder().Not; }
		}

		/// <summary>
		/// All returns a ConstraintBuilder, which will apply
		/// the following constraint to all members of a collection,
		/// succeeding if all of them succeed.
		/// </summary>
		public static ConstraintBuilder All
		{
			get { return new ConstraintBuilder().All; }
		}

		/// <summary>
		/// Some returns a ConstraintBuilder, which will apply
		/// the following constraint to all members of a collection,
		/// succeeding if any of them succeed.
		/// </summary>
		public static ConstraintBuilder Some
		{
			get { return new ConstraintBuilder().Some; }
		}

		/// <summary>
		/// None returns a ConstraintBuilder, which will apply
		/// the following constraint to all members of a collection,
		/// succeeding only if none of them succeed.
		/// </summary>
		public static ConstraintBuilder None
		{
			get { return new ConstraintBuilder().None; }
		}
		#endregion
	}
}
