// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using NUnit.Framework.Constraints;

namespace NUnit.Framework.Syntax.CSharp
{
	/// <summary>
	/// The Text class is a helper class with properties and methods
	/// that supply a number of constraints used with strings.
	/// </summary>
	public class Text
	{
		/// <summary>
		/// Text.All returns a ConstraintBuilder, which will apply
		/// the following constraint to all members of a collection,
		/// succeeding if all of them succeed.
		/// </summary>
		public static ConstraintBuilder All
		{
			get { return new ConstraintBuilder().All; }
		}

		/// <summary>
		/// Contains returns a constraint that succeeds if the actual
		/// value contains the substring supplied as an argument.
		/// </summary>
		public static SubstringConstraint Contains(string substring)
		{
			return new SubstringConstraint(substring);
		}

		/// <summary>
		/// DoesNotContain returns a constraint that fails if the actual
		/// value contains the substring supplied as an argument.
		/// </summary>
		public static StringConstraint.Modifier DoesNotContain(string substring)
		{
            return new ConstraintBuilder().Not.ContainsSubstring(substring);
		}

		/// <summary>
		/// StartsWith returns a constraint that succeeds if the actual
		/// value starts with the substring supplied as an argument.
		/// </summary>
		public static StartsWithConstraint StartsWith(string substring)
		{
			return new StartsWithConstraint(substring);
		}

		/// <summary>
		/// DoesNotStartWith returns a constraint that fails if the actual
		/// value starts with the substring supplied as an argument.
		/// </summary>
		public static StringConstraint.Modifier DoesNotStartWith(string substring)
		{
            return new ConstraintBuilder().Not.StartsWith(substring);
		}

		/// <summary>
		/// EndsWith returns a constraint that succeeds if the actual
		/// value ends with the substring supplied as an argument.
		/// </summary>
		public static EndsWithConstraint EndsWith(string substring)
		{
			return new EndsWithConstraint(substring);
		}

		/// <summary>
		/// DoesNotEndWith returns a constraint that fails if the actual
		/// value ends with the substring supplied as an argument.
		/// </summary>
		public static StringConstraint.Modifier DoesNotEndWith(string substring)
		{
            return new ConstraintBuilder().Not.EndsWith(substring);
		}

		/// <summary>
		/// Matches returns a constraint that succeeds if the actual
		/// value matches the pattern supplied as an argument.
		/// </summary>
		/// <param name="pattern"></param>
		/// <returns></returns>
		public static RegexConstraint Matches(string pattern)
		{
			return new RegexConstraint(pattern);
		}

		/// <summary>
		/// DoesNotMatch returns a constraint that failss if the actual
		/// value matches the pattern supplied as an argument.
		/// </summary>
		/// <param name="pattern"></param>
		/// <returns></returns>
		public static StringConstraint.Modifier DoesNotMatch(string pattern)
		{
            return new ConstraintBuilder().Not.Matches(pattern);
		}
	}
}
