// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using System.Text.RegularExpressions;

namespace NUnit.Framework.Constraints
{
	/// <summary>
	/// StringConstraint is the abstract base for constraints
	/// that operate on strings.
	/// </summary>
    public abstract class StringConstraint : Constraint
    {
        protected string expected;
        protected abstract bool IsMatch(string expected, string actual );

        /// <summary>
        /// Construct a string constraint, passing the expected string value
        /// to be used in applying the constraint.
        /// </summary>
        /// <param name="expected">The expected string</param>
		public StringConstraint(string expected)
        {
            this.expected = expected;
        }

		/// <summary>
		/// Test whether the constraint is matched by the actual value.
		/// This is a template method, which calls the IsMatch method
		/// of the derived class.
		/// </summary>
		/// <param name="actual"></param>
		/// <returns></returns>
        public override bool Matches(object actual)
        {
            this.actual = actual;

            if ( !(actual is string) )
                return false;

            if (caseInsensitive)
                return IsMatch(expected.ToLower(), ((string)actual).ToLower());
            else
                return IsMatch(expected, (string)actual );
        }
    }

	/// <summary>
	/// SubstringConstraint can test whether a string contains
	/// the expected substring.
	/// </summary>
    public class SubstringConstraint : StringConstraint
    {
		public SubstringConstraint(string expected) : base(expected) { }

        protected override bool IsMatch(string expected, string actual)
        {
            return actual.IndexOf(expected) >= 0;
        }

        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.WritePredicate("String containing");
            writer.WriteExpectedValue(expected);
        }
    }

	/// <summary>
	/// StartsWithConstraint can test whether a string starts
	/// with an expected substring.
	/// </summary>
    public class StartsWithConstraint : StringConstraint
    {
        public StartsWithConstraint(string expected) : base(expected) { }

        protected override bool IsMatch(string expected, string actual)
        {
            return actual.StartsWith( expected );
        }

        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.WritePredicate("String starting with");
            writer.WriteExpectedValue( MsgUtils.ClipString(expected, writer.MaxLineLength - 40, 0) );
        }
    }

    /// <summary>
    /// EndsWithConstraint can test whether a string ends
    /// with an expected substring.
    /// </summary>
    public class EndsWithConstraint : StringConstraint
    {
        public EndsWithConstraint(string expected) : base(expected) { }

        protected override bool IsMatch(string expected, string actual)
        {
            return actual.EndsWith(expected);
        }

        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.WritePredicate("String ending with");
            writer.WriteExpectedValue(expected);
        }
    }

    /// <summary>
    /// RegexConstraint can test whether a string matches
    /// the pattern provided.
    /// </summary>
    public class RegexConstraint : StringConstraint
    {
        public RegexConstraint(string pattern) : base(pattern) { }

        protected override bool IsMatch(string pattern, string actual)
        {
            return Regex.IsMatch( actual, pattern );
        }

        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.WritePredicate("String matching");
            writer.WriteExpectedValue(expected);
        }
    }
}
