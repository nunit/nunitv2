// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org.
// ****************************************************************

using System;
using System.Collections;
#if CLR_2_0 || CLR_4_0
using System.Collections.Generic;
#endif

namespace NUnit.Framework.Constraints
{
    [TestFixture]
    public class BinarySerializableTest : ConstraintTestBaseWithArgumentException
    {
        [SetUp]
        public void SetUp()
        {
            theConstraint = new BinarySerializableConstraint();
            expectedDescription = "binary serializable";
            stringRepresentation = "<binaryserializable>";
        }

        internal object[] SuccessData = new object[] { 1, "a", new ArrayList(), new InternalWithSerializableAttributeClass() };

        internal object[] FailureData = new object[] { new InternalClass() };

        internal string[] ActualValues = new string[] { "<InternalClass>" };

        internal object[] InvalidData = new object[] { null };

        internal class InternalClass
        { }

        [Serializable]
        internal class InternalWithSerializableAttributeClass
        { }
    }
}
