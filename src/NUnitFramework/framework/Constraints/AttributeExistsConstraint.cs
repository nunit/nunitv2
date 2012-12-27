// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

using System;

namespace NUnit.Framework.Constraints
{
    /// <summary>
    /// AttributeExistsConstraint tests for the presence of a
    /// specified attribute on  a Type.
    /// </summary>
    public class AttributeExistsConstraint : Constraint
    {
        private Type expectedType;

        /// <summary>
        /// Constructs an AttributeExistsConstraint for a specific attribute Type
        /// </summary>
        /// <param name="type"></param>
        public AttributeExistsConstraint(Type type)
            : base(type)
        {
            this.expectedType = type;

            if (!typeof(Attribute).IsAssignableFrom(expectedType))
                throw new ArgumentException(string.Format(
                    "Type {0} is not an attribute", expectedType), "type");
        }

        /// <summary>
        /// Tests whether the object provides the expected attribute.
        /// </summary>
        /// <param name="actual">A Type, MethodInfo, or other ICustomAttributeProvider</param>
        /// <returns>True if the expected attribute is present, otherwise false</returns>
        public override bool Matches(object actual)
        {
            this.actual = actual;
            System.Reflection.ICustomAttributeProvider attrProvider =
                actual as System.Reflection.ICustomAttributeProvider;

            if (attrProvider == null)
                throw new ArgumentException(string.Format("Actual value {0} does not implement ICustomAttributeProvider", actual), "actual");

            return attrProvider.GetCustomAttributes(expectedType, true).Length > 0;
        }

        /// <summary>
        /// Writes the description of the constraint to the specified writer
        /// </summary>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.WritePredicate("type with attribute");
            writer.WriteExpectedValue(expectedType);
        }
    }
}
