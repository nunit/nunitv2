// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

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
