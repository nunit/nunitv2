// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org.
// ****************************************************************

using System;

namespace NUnit.Framework.Constraints
{
    [TestFixture]
    public class StartsWithTest : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            theConstraint = new StartsWithConstraint("hello");
            expectedDescription = "String starting with \"hello\"";
            stringRepresentation = "<startswith \"hello\">";
        }

        internal object[] SuccessData = new object[] { "hello", "hello there" };

        internal object[] FailureData = new object[] { "goodbye", "HELLO THERE", "I said hello", "say hello to fred", string.Empty, null };

        internal string[] ActualValues = new string[] { "\"goodbye\"", "\"HELLO THERE\"", "\"I said hello\"", "\"say hello to fred\"", "<string.Empty>", "null" };
    }

    [TestFixture]
    public class StartsWithTestIgnoringCase : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            theConstraint = new StartsWithConstraint("hello").IgnoreCase;
            expectedDescription = "String starting with \"hello\", ignoring case";
            stringRepresentation = "<startswith \"hello\">";
        }

        internal object[] SuccessData = new object[] { "Hello", "HELLO there" };

        internal object[] FailureData = new object[] { "goodbye", "What the hell?", "I said hello", "say Hello to fred", string.Empty, null };

        internal string[] ActualValues = new string[] { "\"goodbye\"", "\"What the hell?\"", "\"I said hello\"", "\"say Hello to fred\"", "<string.Empty>", "null" };
    }
}
