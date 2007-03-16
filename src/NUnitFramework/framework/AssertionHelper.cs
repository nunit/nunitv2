// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using NUnit.Framework.SyntaxHelpers;
using NUnit.Framework.Constraints;

namespace NUnit.Framework
{
	/// <summary>
	/// AssertionHelper is an optional base class for user tests,
	/// allowing the use of shorter names for constraints and
	/// asserts and avoiding conflict with the definition of 
	/// <see cref="Is"/>, from which it inherits much of its
	/// behavior, in certain mock object frameworks.
	/// </summary>
	public class AssertionHelper : Is
	{
		#region Expect
		/// <summary>
		/// Apply a constraint to an actual value, succeeding if the constraint
		/// is satisfied and throwing an assertion exception on failure. Works
		/// identically to <see cref="NUnit.Framework.Assert.That(object, IConstraint)"/>
		/// </summary>
		/// <param name="constraint">An IConstraint to be applied</param>
		/// <param name="actual">The actual value to test</param>
		static public void Expect( object actual, IConstraint constraint )
		{
			Assert.That( actual, constraint, null, null );
		}

		/// <summary>
		/// Apply a constraint to an actual value, succeeding if the constraint
		/// is satisfied and throwing an assertion exception on failure. Works
		/// identically to <see cref="NUnit.Framework.Assert.That(object, IConstraint, string)"/>
		/// </summary>
		/// <param name="constraint">An IConstraint to be applied</param>
		/// <param name="actual">The actual value to test</param>
		/// <param name="message">The message that will be displayed on failure</param>
		static public void Expect( object actual, IConstraint constraint, string message )
		{
			Assert.That( actual, constraint, message, null );
		}

		/// <summary>
		/// Apply a constraint to an actual value, succeeding if the constraint
		/// is satisfied and throwing an assertion exception on failure. Works
		/// identically to <see cref="NUnit.Framework.Assert.That(object, IConstraint, string, object[])"/>
		/// </summary>
		/// <param name="constraint">An IConstraint to be applied</param>
		/// <param name="actual">The actual value to test</param>
		/// <param name="message">The message that will be displayed on failure</param>
		/// <param name="args">Arguments to be used in formatting the message</param>
		static public void Expect( object actual, IConstraint constraint, string message, params object[] args )
		{
			Assert.That( actual, constraint, message, args );
		}

		/// <summary>
		/// Asserts that a condition is true. If the condition is false the method throws
		/// an <see cref="AssertionException"/>. Works Identically to 
        /// <see cref="Assert.That(bool, string, object[])"/>.
		/// </summary> 
		/// <param name="condition">The evaluated condition</param>
		/// <param name="message">The message to display if the condition is false</param>
		/// <param name="args">Arguments to be used in formatting the message</param>
		static public void Expect(bool condition, string message, params object[] args)
		{
			Assert.That(condition, Is.True, message, args);
		}

		/// <summary>
		/// Asserts that a condition is true. If the condition is false the method throws
		/// an <see cref="AssertionException"/>. Works Identically to 
        /// <see cref="Assert.That(bool, string)"/>.
		/// </summary>
		/// <param name="condition">The evaluated condition</param>
		/// <param name="message">The message to display if the condition is false</param>
		static public void Expect(bool condition, string message)
		{
			Assert.That(condition, Is.True, message, null);
		}

		/// <summary>
		/// Asserts that a condition is true. If the condition is false the method throws
		/// an <see cref="AssertionException"/>. Works Identically to <see cref="Assert.That(bool)"/>.
		/// </summary>
		/// <param name="condition">The evaluated condition</param>
		static public void Expect(bool condition)
		{
			Assert.That(condition, Is.True, null, null);
		}
		#endregion

		#region ContainingConstraint
        /// <summary>
        /// Contains returns a ContainsConstraint
        /// </summary>
		public Constraint Contains(object expected)
		{
			return new ContainsConstraint( expected );
		}
		#endregion

		#region String Constraints
		/// <summary>
		/// StartsWith is a synonym for Text.StartsWith
		/// </summary>
		public Constraint StartsWith(string expected)
		{
			return Text.StartsWith( expected );
		}

		/// <summary>
		/// Ending is a synonym for Is.StringEnding
		/// </summary>
		public Constraint EndsWith(string expected)
		{
			return Text.EndsWith( expected );
		}

		/// <summary>
		/// Matches is a synonym for Text.Matches
		/// </summary>
		public Constraint Matches(string pattern)
		{
			return Text.Matches(pattern);
		}
		#endregion

		#region PropertyConstraint
        /// <summary>
        /// Property is a synonym for Has.Property
        /// </summary>
		public static Constraint Property( string name, object expected )
		{
			return new PropertyConstraint( name, expected );
		}

        /// <summary>
        /// Length is a synonym for Has.Length
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
		public static Constraint Length( int length )
		{
			return Property( "Length", length );
		}
		#endregion
	}
}
