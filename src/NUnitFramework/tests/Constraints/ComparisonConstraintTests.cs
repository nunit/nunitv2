// *****************************************************
// Copyright 2007, Charlie Poole
// Licensed under the NUnit License, see license.txt
// *****************************************************

using System;

namespace NUnit.Framework.Constraints.Tests
{
    [TestFixture]
    public class GreaterThanTest : ConstraintTestBase
    {
		[SetUp]
        public void SetUp()
        {
            Matcher = new GreaterThanConstraint(5);
            GoodValues = new object[] { 6 };
            BadValues = new object[] { 4, 5, "big", null };
            Description = "greater than 5";
        }
    }

    [TestFixture]
    public class GreaterThanOrEqualTest : ConstraintTestBase
    {
		[SetUp]
        public void SetUp()
        {
            Matcher = new GreaterThanOrEqualConstraint(5);
            GoodValues = new object[] { 6, 5 };
            BadValues = new object[] { 4, "big", null };
            Description = "greater than or equal to 5";
        }
    }

    [TestFixture]
    public class LessThanTest : ConstraintTestBase
    {
		[SetUp]
        public void SetUp()
        {
            Matcher = new LessThanConstraint(5);
            GoodValues = new object[] { 4 };
            BadValues = new object[] { 6, 5, "big", null };
            Description = "less than 5";
        }
    }

    [TestFixture]
    public class LessThanOrEqualTest : ConstraintTestBase
    {
		[SetUp]
        public void SetUp()
        {
            Matcher = new LessThanOrEqualConstraint(5);
            GoodValues = new object[] { 4 , 5 };
            BadValues = new object[] { 6, "big", null };
            Description = "less than or equal to 5";
        }
    }
}
