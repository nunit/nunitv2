// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;

namespace NUnit.Framework.Constraints.Tests
{
    [TestFixture]
    public class NotTest : ConstraintTestBase
    {
		[SetUp]
        public void SetUp()
        {
            Matcher = new NotConstraint( new EqualConstraint(null) );
            GoodValues = new object[] { 42, "Hello" };
            BadValues = new object [] { null };
            Description = "not null";
        }

        [Test]
        public void CanUseNotOperator()
        {
            Assert.That(42, !new EqualConstraint(99));
        }
    }
}
