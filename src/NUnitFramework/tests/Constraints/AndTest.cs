// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;

namespace NUnit.Framework.Constraints.Tests
{
    [TestFixture]
    public class AndTest : ConstraintTestBase
    {
		[SetUp]
        public void SetUp()
        {
            Matcher = new AndConstraint(new GreaterThanConstraint(40), new LessThanConstraint(50));
            GoodValues = new object[] { 42 };
            BadValues = new object[] { 37, 53 };
            Description = "greater than 40 and less than 50";
        }

        [Test]
        public void CanCombineTestsWithAndOperator()
        {
            Assert.That(42, new GreaterThanConstraint(40) & new LessThanConstraint(50));
        }
    }
}
