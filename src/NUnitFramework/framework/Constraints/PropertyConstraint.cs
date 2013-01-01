// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

using System;
using System.Reflection;

namespace NUnit.Framework.Constraints
{
	/// <summary>
	/// PropertyConstraint extracts a named property and uses
    /// its value as the actual value for a chained constraint.
	/// </summary>
	public class PropertyConstraint : PrefixConstraint
	{
		private readonly string name;
		private object propValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:PropertyConstraint"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="baseConstraint">The constraint to apply to the property.</param>
        public PropertyConstraint(string name, Constraint baseConstraint)
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
            Guard.ArgumentNotNull(actual, "actual");

            Type actualType = actual as Type;
            if ( actualType == null )
                actualType = actual.GetType();

            PropertyInfo property = actualType.GetProperty(name,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty);

            if (property == null)
                throw new ArgumentException(string.Format("Property {0} was not found",name), "name");

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
            writer.WriteActualValue(propValue);
		}

        /// <summary>
        /// Returns the string representation of the constraint.
        /// </summary>
        /// <returns></returns>
        protected override string GetStringRepresentation()
        {
            return string.Format("<property {0} {1}>", name, baseConstraint);
        }
	}
}
