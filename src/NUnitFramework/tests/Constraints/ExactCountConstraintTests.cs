// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org.
// ****************************************************************

using System;
using NUnit.Framework.Tests;

namespace NUnit.Framework.Constraints
{
    public class ExactCountConstraintTests : MessageChecker
    {
        private static readonly string[] names = new string[] { "Charlie", "Fred", "Joe", "Charlie" };

        [Test]
        public void ZeroItemsMatch()
        {
            Assert.That(names, new ExactCountConstraint(0, Is.EqualTo("Sam")));
            Assert.That(names, Has.Exactly(0).EqualTo("Sam"));
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void ZeroItemsMatchFails()
        {
            expectedMessage =
                TextMessageWriter.Pfx_Expected + "no item \"Charlie\"" + Environment.NewLine +
                TextMessageWriter.Pfx_Actual + "< \"Charlie\", \"Fred\", \"Joe\", \"Charlie\" >" + Environment.NewLine;
            Assert.That(names, new ExactCountConstraint(0, Is.EqualTo("Charlie")));
        }

        [Test]
        public void ExactlyOneItemMatches()
        {
            Assert.That(names, new ExactCountConstraint(1, Is.EqualTo("Fred")));
            Assert.That(names, Has.Exactly(1).EqualTo("Fred"));
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void ExactlyOneItemMatchFails()
        {
            expectedMessage =
                TextMessageWriter.Pfx_Expected + "exactly one item \"Charlie\"" + Environment.NewLine +
                TextMessageWriter.Pfx_Actual + "< \"Charlie\", \"Fred\", \"Joe\", \"Charlie\" >" + Environment.NewLine;
            Assert.That(names, new ExactCountConstraint(1, Is.EqualTo("Charlie")));
        }

        [Test]
        public void ExactlyTwoItemsMatch()
        {
            Assert.That(names, new ExactCountConstraint(2, Is.EqualTo("Charlie")));
            Assert.That(names, Has.Exactly(2).EqualTo("Charlie"));
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void ExactlyTwoItemsMatchFails()
        {
            expectedMessage =
                TextMessageWriter.Pfx_Expected + "exactly 2 items \"Fred\"" + Environment.NewLine +
                TextMessageWriter.Pfx_Actual + "< \"Charlie\", \"Fred\", \"Joe\", \"Charlie\" >" + Environment.NewLine;
            Assert.That(names, new ExactCountConstraint(2, Is.EqualTo("Fred")));
        }
    }
}
