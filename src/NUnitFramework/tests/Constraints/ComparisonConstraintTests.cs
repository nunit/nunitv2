// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;

namespace NUnit.Framework.Constraints.Tests
{
    [TestFixture]
    public class GreaterThanTest : ConstraintTestBaseWithInvalidDataTest
    {
        [SetUp]
        public void SetUp()
        {
            theConstraint = new GreaterThanConstraint(5);
            expectedDescription = "greater than 5";
            stringRepresentation = "<greaterthan 5>";
        }

        object[] GoodData = new object[] { 6, 5.001 };

        object[] BadData = new object[] { 4, 5 };

        object[] FailureMessages = new object[] { "4", "5" };

        object[] InvalidData = new object[] { null, "xxx" };
    }

    [TestFixture]
    public class GreaterThanOrEqualTest : ConstraintTestBaseWithInvalidDataTest
    {
        [SetUp]
        public void SetUp()
        {
            theConstraint = new GreaterThanOrEqualConstraint(5);
            expectedDescription = "greater than or equal to 5";
            stringRepresentation = "<greaterthanorequal 5>";
        }

        object[] GoodData = new object[] { 6, 5 };

        object[] BadData = new object[] { 4 };

        object[] FailureMessages = new object[] { "4" };

        object[] InvalidData = new object[] { null, "xxx" };
    }

    [TestFixture]
    public class LessThanTest : ConstraintTestBaseWithInvalidDataTest
    {
        [SetUp]
        public void SetUp()
        {
            theConstraint = new LessThanConstraint(5);
            expectedDescription = "less than 5";
            stringRepresentation = "<lessthan 5>";
        }

        object[] GoodData = new object[] { 4, 4.999 };

        object[] BadData = new object[] { 6, 5 };

        object[] FailureMessages = new object[] { "6", "5" };

        object[] InvalidData = new object[] { null, "xxx" };
    }

    [TestFixture]
    public class LessThanOrEqualTest : ConstraintTestBaseWithInvalidDataTest
    {
        [SetUp]
        public void SetUp()
        {
            theConstraint = new LessThanOrEqualConstraint(5);
            expectedDescription = "less than or equal to 5";
            stringRepresentation = "<lessthanorequal 5>";
        }

        object[] GoodData = new object[] { 4, 5 };

        object[] BadData = new object[] { 6 };

        object[] FailureMessages = new object[] { "6" };

        object[] InvalidData = new object[] { null, "xxx" };
    }
}