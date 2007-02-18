// *****************************************************
// Copyright 2007, Charlie Poole
// Licensed under the NUnit License, see license.txt
// *****************************************************

using System;

namespace NUnit.Framework.Constraints.Tests
{
    [TestFixture]
    public class SameAsTest : ConstraintTestBase
    {
		[SetUp]
        public void SetUp()
        {
            object obj1 = new object();
            object obj2 = new object();

            Matcher = new SameAsConstraint(obj1);
            GoodValues = new object[] { obj1 };
            BadValues = new object[] { obj2, 3, "Hello" };
            Description = "same as <System.Object>";
        }
    }
}
