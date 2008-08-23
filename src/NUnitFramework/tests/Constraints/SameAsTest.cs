// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

namespace NUnit.Framework.Constraints.Tests
{
    [TestFixture]
    public class SameAsTest : ConstraintTestBase
    {
        private static readonly object obj1 = new object();
        private static readonly object obj2 = new object();

        [SetUp]
        public void SetUp()
        {
            theConstraint = new SameAsConstraint(obj1);
            expectedDescription = "same as <System.Object>";
        }

        static object[] GoodData = new object[] { obj1 };

        static object[] BadData = new object[] { obj2, 3, "Hello" };

        static object[] FailureMessages = new object[] { "<System.Object>", "3", "\"Hello\"" };
    }
}