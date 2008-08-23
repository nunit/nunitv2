// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************
using System;

namespace NUnit.Framework.Constraints
{
    public abstract class ConstraintTestBase
    {
        protected Constraint theConstraint;
        protected string expectedDescription;

        [Test, TestCases("GoodData")]
        public void SucceedsWithGoodValues(object value)
        {
            Assert.That(theConstraint.Matches(value));
        }

        [Test, TestCases("BadData")]
        public void FailsWithBadValues(object badValue)
        {
            Assert.IsFalse(theConstraint.Matches(badValue));
        }

        [Test, Sequential]
        public void ProvidesProperFailureMessage(
            [DataSource("BadData")] object badValue,
            [DataSource("FailureMessages")] string message)
        {
            theConstraint.Matches(badValue);
            TextMessageWriter writer = new TextMessageWriter();
            theConstraint.WriteMessageTo(writer);
            Assert.AreEqual(
                TextMessageWriter.Pfx_Expected + expectedDescription + Environment.NewLine +
                TextMessageWriter.Pfx_Actual + message + Environment.NewLine,
                writer.ToString());
        }

		[Test]
        public void ProvidesProperDescription()
        {
            TextMessageWriter writer = new TextMessageWriter();
            theConstraint.WriteDescriptionTo(writer);
            Assert.AreEqual(expectedDescription, writer.ToString());
        }
    }

    public abstract class ConstraintTestBaseWithInvalidDataTest : ConstraintTestBase
    {
        [Test, TestCases("InvalidData")]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidDataGivesArgumentException(object value)
        {
            theConstraint.Matches(value);
        }
    }
}