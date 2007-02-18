// *****************************************************
// Copyright 2007, Charlie Poole
// Licensed under the NUnit License, see license.txt
// *****************************************************

using System;

namespace NUnit.Framework.Constraints.Tests
{
    [TestFixture]
    public class EmptyTest : ConstraintTestBase
    {
		[SetUp]
        public void SetUp()
        {
            Matcher = new EmptyConstraint();
            GoodValues = new object[] { string.Empty, new object[0], new System.Collections.ArrayList() };
            BadValues = new object[] { "Hello", new object[] { 1, 2, 3 } };
            Description = "<empty>";
        }
    }
}
