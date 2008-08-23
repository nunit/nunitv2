// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

namespace NUnit.Framework.Constraints.Tests
{
    [TestFixture]
    public class AndTest : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            theConstraint = new AndConstraint(new GreaterThanConstraint(40), new LessThanConstraint(50));
            expectedDescription = "greater than 40 and less than 50";
        }

		object[] GoodData = new object[] { 42 };
	
		object[] BadData = new object[] { 37, 53 };

		object[] FailureMessages = new object[] { "37", "53" };

		[Test]
        public void CanCombineTestsWithAndOperator()
        {
            Assert.That(42, new GreaterThanConstraint(40) & new LessThanConstraint(50));
        }
    }
}