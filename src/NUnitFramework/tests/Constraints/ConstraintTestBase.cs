// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;

namespace NUnit.Framework.Constraints.Tests
{
    public abstract class ConstraintTestBase
    {
        protected Constraint Matcher;
        protected object[] GoodValues;
        protected object[] BadValues;
        protected string Description;

        [Test]
		public void SucceedsOnGoodValues()
        {
            foreach (object value in GoodValues)
                Assert.That(value, Matcher, "Test should succeed with {0}", value);
        }

        [Test]
		public void FailsOnBadValues()
        {
            foreach (object value in BadValues)
            {
                Assert.That(Matcher.Matches(value), new EqualConstraint(false), "Test should fail with value {0}", value);
            }
        }

		[Test]
        public void ProvidesProperDescription()
        {
            TextMessageWriter writer = new TextMessageWriter();
            Matcher.WriteDescriptionTo(writer);
            Assert.That(writer.ToString(), new EqualConstraint(Description), null);
        }

		[Test]
        public void ProvidesProperFailureMessage()
        {
			object badValue = BadValues[0];
			string badString = badValue == null ? "null" : badValue.ToString();

			TextMessageWriter writer = new TextMessageWriter();
            Matcher.Matches(badValue);
            Matcher.WriteMessageTo(writer);
            Assert.That(writer.ToString(), new SubstringConstraint(Description));
			Assert.That(writer.ToString(), new SubstringConstraint(badString));
            Assert.That(writer.ToString(), new NotConstraint(new SubstringConstraint("<UNSET>")));
        }
    }
}
