// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org.
// ****************************************************************

using System;

namespace NUnit.Framework.Constraints
{
    [TestFixture]
    public class EndsWithTest : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            theConstraint = new EndsWithConstraint("hello");
            expectedDescription = "String ending with \"hello\"";
            stringRepresentation = "<endswith \"hello\">";
        }

        internal object[] SuccessData = new object[] { "hello", "I said hello" };

        internal object[] FailureData = new object[] { "goodbye", "What the hell?", "hello there", "say hello to fred", string.Empty, null };

        internal string[] ActualValues = new string[] { "\"goodbye\"", "\"What the hell?\"", "\"hello there\"", "\"say hello to fred\"", "<string.Empty>", "null" };
    }

    [TestFixture]
    public class EndsWithTestIgnoringCase : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            theConstraint = new EndsWithConstraint("hello").IgnoreCase;
            expectedDescription = "String ending with \"hello\", ignoring case";
            stringRepresentation = "<endswith \"hello\">";
        }

        internal object[] SuccessData = new object[] { "HELLO", "I said Hello" };

        internal object[] FailureData = new object[] { "goodbye", "What the hell?", "hello there", "say hello to fred", string.Empty, null };

        internal string[] ActualValues = new string[] { "\"goodbye\"", "\"What the hell?\"", "\"hello there\"", "\"say hello to fred\"", "<string.Empty>", "null" };
    }
}
