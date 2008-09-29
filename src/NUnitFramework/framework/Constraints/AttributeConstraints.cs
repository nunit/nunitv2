// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;

namespace NUnit.Framework.Constraints
{
    public class AttributeExistsConstraint : Constraint
    {
        private Type expectedType;

        public AttributeExistsConstraint(Type type)
            : base(type)
        {
            this.expectedType = type;

            if (!typeof(Attribute).IsAssignableFrom(expectedType))
                throw new ArgumentException(string.Format(
                    "Type {0} is not an attribute", expectedType), "type");
        }

        public override bool Matches(object actual)
        {
            this.actual = actual;
            System.Reflection.ICustomAttributeProvider attrProvider =
                actual as System.Reflection.ICustomAttributeProvider;

            if (attrProvider == null)
                throw new ArgumentException(string.Format("Actual value {0} does not implement ICustomAttributeProvider", actual), "actual");

            return attrProvider.GetCustomAttributes(expectedType, true).Length > 0;
        }

        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.WritePredicate("type with attribute");
            writer.WriteExpectedValue(expectedType);
        }
    }

    public class AttributeConstraint : PrefixConstraint
    {
        private Type expectedType;
        private Attribute attrFound;

        public AttributeConstraint(Type type, Constraint baseConstraint)
            : base( baseConstraint )
        {
            this.expectedType = type;

            if (!typeof(Attribute).IsAssignableFrom(expectedType))
                throw new ArgumentException(string.Format(
                    "Type {0} is not an attribute", expectedType), "type");
        }

        public override bool Matches(object actual)
        {
            this.actual = actual;
            System.Reflection.ICustomAttributeProvider attrProvider =
                actual as System.Reflection.ICustomAttributeProvider;

            if (attrProvider == null)
                throw new ArgumentException(string.Format("Actual value {0} does not implement ICustomAttributeProvider", actual), "actual");

            Attribute[] attrs = (Attribute[])attrProvider.GetCustomAttributes(expectedType, true);
            if (attrs.Length == 0)
                throw new ArgumentException(string.Format("Attribute {0} was not found", expectedType), "actual");

            this.attrFound = attrs[0];
            return baseConstraint.Matches(attrFound);
        }

        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.WritePredicate("attribute " + expectedType.FullName);
            if (baseConstraint != null)
            {
                if (baseConstraint is EqualConstraint)
                    writer.WritePredicate("equal to");
                baseConstraint.WriteDescriptionTo(writer);
            }
        }

        public override void WriteActualValueTo(MessageWriter writer)
        {
            writer.WriteActualValue(attrFound);
        }

        public override string ToString()
        {
            return string.Format("<attribute {0} {1}>", expectedType, baseConstraint); 
        }
    }
}
