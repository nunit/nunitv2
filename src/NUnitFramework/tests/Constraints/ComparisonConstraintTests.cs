// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;

namespace NUnit.Framework.Constraints.Tests
{
	public class ComparisonConstraintTestBase : ConstraintTestBase
	{
		[Test,ExpectedException(typeof(ArgumentException))]
		public void NullGivesError()
		{
			Assert.That( null, Matcher );
		}

		[Test,ExpectedException]
		public void BadTypeGivesError()
		{
			Assert.That( "big", Matcher );
		}
	}

    [TestFixture]
    public class GreaterThanTest : ComparisonConstraintTestBase
    {
		[SetUp]
        public void SetUp()
        {
            Matcher = new GreaterThanConstraint(5);
            GoodValues = new object[] { 6 };
            BadValues = new object[] { 4, 5 };
            Description = "greater than 5";
        }
    }

    [TestFixture]
    public class GreaterThanOrEqualTest : ComparisonConstraintTestBase
    {
		[SetUp]
        public void SetUp()
        {
            Matcher = new GreaterThanOrEqualConstraint(5);
            GoodValues = new object[] { 6, 5 };
            BadValues = new object[] { 4 };
            Description = "greater than or equal to 5";
        }
    }

    [TestFixture]
    public class LessThanTest : ComparisonConstraintTestBase
    {
		[SetUp]
        public void SetUp()
        {
            Matcher = new LessThanConstraint(5);
            GoodValues = new object[] { 4 };
            BadValues = new object[] { 6, 5 };
            Description = "less than 5";
        }
    }

    [TestFixture]
    public class LessThanOrEqualTest : ComparisonConstraintTestBase
    {
		[SetUp]
        public void SetUp()
        {
            Matcher = new LessThanOrEqualConstraint(5);
            GoodValues = new object[] { 4 , 5 };
            BadValues = new object[] { 6 };
            Description = "less than or equal to 5";
        }
    }
}
