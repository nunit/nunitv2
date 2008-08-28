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
        protected string expectedDescription = "<NOT SET>";
        protected string stringRepresentation = "<NOT SET>";

        [Test, TestCases("SuccessData")]
        public void SucceedsWithGoodValues(object value)
        {
            Assert.That(theConstraint.Matches(value));
        }

        [Test, TestCases("FailureData")]
        public void FailsWithBadValues(object badValue)
        {
            Assert.IsFalse(theConstraint.Matches(badValue));
        }

        [Test, Sequential]
        public void ProvidesProperFailureMessage(
            [DataSource("FailureData")] object badValue,
            [DataSource("ActualValues")] string message)
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

        [Test]
        public void ProvidesProperStringRepresentation()
        {
            Assert.AreEqual(stringRepresentation, theConstraint.ToString());
        }
    }

    /// <summary>
    /// Base class for testing constraints that can throw an ArgumentException
    /// </summary>
    public abstract class ConstraintTestBaseWithArgumentException : ConstraintTestBase
    {
        [Test, TestCases("InvalidData")]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidDataThrowsArgumentException(object value)
        {
            theConstraint.Matches(value);
        }
    }

    /// <summary>
    /// Base class for tests that can throw multiple exceptions. Use
    /// TestCaseData class to specify the expected exception type.
    /// </summary>
    public abstract class ConstraintTestBaseWithExceptionTests : ConstraintTestBase
    {
        [Test, TestCases("InvalidData")]
        public void InvalidDataThrowsException(object value)
        {
            theConstraint.Matches(value);
        }
    }
}