// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;

namespace NUnit.Framework.Constraints.Tests
{
    [TestFixture]
    public class OrTest : ConstraintTestBase
    {
		[SetUp]
        public void SetUp()
        {
            Matcher = new OrConstraint(new EqualConstraint(42), new EqualConstraint(99));
            GoodValues = new object[] { 99, 42 };
            BadValues = new object[] { 37 };
            Description = "42 or 99";
        }

        [Test]
        public void CanCombineTestsWithOrOperator()
        {
            Assert.That(99, new EqualConstraint(42) | new EqualConstraint(99) );
        }
    }
}
