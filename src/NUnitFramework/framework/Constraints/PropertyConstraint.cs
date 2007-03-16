// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using System.Reflection;

namespace NUnit.Framework.Constraints
{
	/// <summary>
	/// Summary description for PropertyConstraint.
	/// </summary>
	public class PropertyConstraint : Constraint
	{
		private string name;
		private object expected;
		private object propValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:PropertyConstraint"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="expected">The expected.</param>
		public PropertyConstraint( string name, object expected )
		{
			this.name = name;
			this.expected = expected;
		}

        /// <summary>
        /// Test whether the constraint is satisfied by a given value
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>True for success, false for failure</returns>
		public override bool Matches(object actual)
		{
			this.actual = actual;
			if ( actual == null ) return false;

			PropertyInfo property = actual.GetType().GetProperty( name, 
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance );

			propValue = property.GetValue( actual, null );

			if ( propValue == null && expected == null ) return true;
			if ( propValue == null || expected == null ) return false;
			return propValue.Equals( expected );
		}

        /// <summary>
        /// Write the constraint description to a MessageWriter
        /// </summary>
        /// <param name="writer">The writer on which the description is displayed</param>
		public override void WriteDescriptionTo(MessageWriter writer)
		{
			writer.WritePredicate( "Property \"" + name + "\" equal to" );
			writer.WriteExpectedValue( expected );
		}

        /// <summary>
        /// Write the actual value for a failing constraint test to a
        /// MessageWriter. The default implementation simply writes
        /// the raw value of actual, leaving it to the writer to
        /// perform any formatting.
        /// </summary>
        /// <param name="writer">The writer on which the actual value is displayed</param>
		public override void WriteActualValueTo(MessageWriter writer)
		{
			writer.WriteActualValue( propValue );
		}


	}
}
