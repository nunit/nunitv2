// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org.
// ****************************************************************

namespace NUnit.Framework.Constraints
{
    [TestFixture]
    public class AssignableFromConstraintTests : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            theConstraint = new AssignableFromConstraint(typeof(D1));
            expectedDescription = string.Format("assignable from <{0}>", typeof(D1));
            stringRepresentation = string.Format("<assignablefrom {0}>", typeof(D1));
        }

        internal object[] SuccessData = new object[] { new D1(), new B() };

        internal object[] FailureData = new object[] { new D2() };

        internal string[] ActualValues = new string[]
            {
                "<NUnit.Framework.Constraints.AssignableFromConstraintTests+D2>"
            };

        class B { }

        class D1 : B { }

        class D2 : D1 { }
    }
}