// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

namespace NUnit.Framework.Constraints.Tests
{
    [TestFixture]
    public class OrTest : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            theConstraint = new OrConstraint(new EqualConstraint(42), new EqualConstraint(99));
            expectedDescription = "42 or 99";
        }

		object[] GoodData = new object[] { 99, 42 };

		object[] BadData = new object[] { 37 };

		object[] FailureMessages = new object[] { "37" };

		[Test]
        public void CanCombineTestsWithOrOperator()
        {
            Assert.That(99, new EqualConstraint(42) | new EqualConstraint(99) );
        }
    }
}