// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org.
// ****************************************************************

namespace NUnit.Framework.Constraints
{
    [TestFixture]
    public class ExactTypeConstraintTests : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            theConstraint = new ExactTypeConstraint(typeof(D1));
            expectedDescription = string.Format("<{0}>", typeof(D1));
            stringRepresentation = string.Format("<typeof {0}>", typeof(D1));
        }

        internal object[] SuccessData = new object[] { new D1() };

        internal object[] FailureData = new object[] { new B(), new D2() };

        internal string[] ActualValues = new string[]
            {
                "<NUnit.Framework.Constraints.ExactTypeConstraintTests+B>",
                "<NUnit.Framework.Constraints.ExactTypeConstraintTests+D2>"
            };

        class B { }

        class D1 : B { }

        class D2 : D1 { }
    }
}