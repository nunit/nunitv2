// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;

namespace NUnit.Framework.Constraints.Tests
{
    [TestFixture]
    public class ExactTypeTest : ConstraintTestBase
    {
		[SetUp]
        public void SetUp()
        {
            Matcher = new ExactTypeConstraint(typeof(D1));
            GoodValues = new object[] { new D1() };
            BadValues = new object[] { new B(), new D2() };
            Description = "<NUnit.Framework.Constraints.Tests.D1>";
        }
    }

    [TestFixture]
    public class InstanceOfTypeTest : ConstraintTestBase
    {
		[SetUp]
        public void SetUp()
        {
            Matcher = new InstanceOfTypeConstraint(typeof(D1));
            GoodValues = new object[] { new D1(), new D2() };
            BadValues = new object[] { new B() };
            Description = "instance of <NUnit.Framework.Constraints.Tests.D1>";
        }
    }

    [TestFixture]
    public class AssignableFromTest : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            Matcher = new AssignableFromConstraint(typeof(D1));
            GoodValues = new object[] { new D1(), new B() };
            BadValues = new object[] { new D2() };
            Description = "Type assignable from <NUnit.Framework.Constraints.Tests.D1>";
        }
    }

    class B { }
    class D1 : B { }
    class D2 : D1 { }
}
