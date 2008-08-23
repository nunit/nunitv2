// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using System.Collections;
using System.Reflection;

namespace NUnit.Framework.Constraints
{
	/// <summary>
	/// Summary description for PropertyConstraint.
	/// </summary>
	public class PropertyConstraint : PrefixConstraint
	{
		private string name;
		private object propValue;

		private bool propertyExists;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:PropertyConstraint"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="baseConstraint">The constraint to apply to the property.</param>
		public PropertyConstraint( string name, Constraint baseConstraint )
			: base( baseConstraint ) 
		{ 
			this.name = name;
		}

		/// <summary>
		/// Test whether the constraint is satisfied by a given value
		/// </summary>
		/// <param name="actual">The value to be tested</param>
		/// <returns>True for success, false for failure</returns>
		public override bool Matches(object actual)
		{
			this.actual = actual;
            Type actualType = actual.GetType();

			// TODO: Should be argument exception?
			if ( actual == null ) return false;

			PropertyInfo property = actualType.GetProperty( name, 
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty );
			this.propertyExists = property != null;
			if ( !propertyExists ) return false;

			if ( baseConstraint == null ) return true;

			propValue = property.GetValue( actual, null );
			return baseConstraint.Matches( propValue );
		}

		/// <summary>
		/// Write the constraint description to a MessageWriter
		/// </summary>
		/// <param name="writer">The writer on which the description is displayed</param>
		public override void WriteDescriptionTo(MessageWriter writer)
		{
			writer.WritePredicate( "property " + name );
            if (baseConstraint != null)
            {
                if (baseConstraint is EqualConstraint)
                    writer.WritePredicate("equal to");
                baseConstraint.WriteDescriptionTo(writer);
            }
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
            if (!propertyExists)
                writer.Write("no property " + name + " found");
            else
            {
                writer.WritePredicate("property " + name);
                if (baseConstraint != null)
                    baseConstraint.WriteActualValueTo(writer);
                else
                    writer.WriteActualValue(actual);
            }
		}
	}
}
