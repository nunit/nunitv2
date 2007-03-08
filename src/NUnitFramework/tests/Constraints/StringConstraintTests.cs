// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;

namespace NUnit.Framework.Constraints.Tests
{
    [TestFixture]
    public class SubstringTest : ConstraintTestBase, IExpectException
    {
		[SetUp]
        public void SetUp()
        {
            Matcher = new SubstringConstraint("hello");
            GoodValues = new object[] { "hello", "hello there", "I said hello", "say hello to fred" };
            BadValues = new object[] { "goodbye", "What the hell?", string.Empty, null };
            Description = "String containing \"hello\"";
        }

        public void HandleException(Exception ex)
        {
            Assert.That(ex.Message, new EqualConstraint(
                TextMessageWriter.Pfx_Expected + "String containing \"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa...\"" + Environment.NewLine +
                TextMessageWriter.Pfx_Actual   + "\"xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx...\"" + Environment.NewLine));
        }
    }

    [TestFixture]
    public class SubstringTestIgnoringCase : ConstraintTestBase
    {
		[SetUp]
        public void SetUp()
        {
            Matcher = new SubstringConstraint("hello").IgnoreCase;
            GoodValues = new object[] { "Hello", "HellO there", "I said HELLO", "say hello to fred" };
            BadValues = new object[] { "goodbye", "What the hell?", string.Empty, null };
            Description = "String containing \"hello\", ignoring case";
        }
    }

    [TestFixture]
    public class StartsWithTest : ConstraintTestBase
    {
		[SetUp]
        public void SetUp()
        {
            Matcher = new StartsWithConstraint("hello");
            GoodValues = new object[] { "hello", "hello there" };
            BadValues = new object[] { "goodbye", "What the hell?", "I said hello", "say hello to fred", string.Empty, null };
            Description = "String starting with \"hello\"";
        }
    }

    [TestFixture]
    public class StartsWithTestIgnoringCase : ConstraintTestBase
    {
		[SetUp]
        public void SetUp()
        {
            Matcher = new StartsWithConstraint("hello").IgnoreCase;
            GoodValues = new object[] { "Hello", "HELLO there" };
            BadValues = new object[] { "goodbye", "What the hell?", "I said hello", "say Hello to fred", string.Empty, null };
            Description = "String starting with \"hello\", ignoring case";
        }
    }

    [TestFixture]
    public class EndsWithTest : ConstraintTestBase
    {
		[SetUp]
        public void SetUp()
        {
            Matcher = new EndsWithConstraint("hello");
            GoodValues = new object[] { "hello", "I said hello" };
            BadValues = new object[] { "goodbye", "What the hell?", "hello there", "say hello to fred", string.Empty, null };
            Description = "String ending with \"hello\"";
        }
    }

    [TestFixture]
    public class EndsWithTestIgnoringCase : ConstraintTestBase//, IExpectException
    {
		[SetUp]
        public void SetUp()
        {
            Matcher = new EndsWithConstraint("hello").IgnoreCase;
            GoodValues = new object[] { "HELLO", "I said Hello" };
            BadValues = new object[] { "goodbye", "What the hell?", "hello there", "say hello to fred", string.Empty, null };
            Description = "String ending with \"hello\", ignoring case";
        }
    }

    [TestFixture]
    public class EqualIgnoringCaseTest : ConstraintTestBase
    {
		[SetUp]
        public void SetUp()
        {
            Matcher = new EqualConstraint("Hello World!").IgnoreCase;
            GoodValues = new object[] { "hello world!", "Hello World!", "HELLO world!" };
            BadValues = new object[] { "goodbye", "Hello Friends!", string.Empty, null };
            Description = "\"Hello World!\"";
        }
    }
}
