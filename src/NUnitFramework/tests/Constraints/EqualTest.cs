// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using System.IO;
using System.Drawing;

namespace NUnit.Framework.Constraints.Tests
{
    [TestFixture]
    public class EqualConstraintTest : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            theConstraint = new EqualConstraint(4);
            expectedDescription = "4";
            stringRepresentation = "<equal 4>";
        }

        object[] SuccessData = new object[] { 4, 4.0f, 4.0d, 4.0000m };
            
        object[] FailureData = new object[] { 5, null, "Hello", double.NaN, double.PositiveInfinity };

        string[] ActualValues = new string[] { "5", "null", "\"Hello\"", "NaN", "Infinity" };

        [TestCase(double.NaN)]
        [TestCase(double.PositiveInfinity)]
        [TestCase(double.NegativeInfinity)]
        [TestCase(float.NaN)]
        [TestCase(float.PositiveInfinity)]
        [TestCase(float.NegativeInfinity)]
        public void CanMatchSpecialFloatingPointValues(object value)
        {
            Assert.That(value, new EqualConstraint(value));
        }

        [Test]
        public void CanMatchDates()
        {
            DateTime expected = new DateTime(2007, 4, 1);
            DateTime actual = new DateTime(2007, 4, 1);
            Assert.That(actual, new EqualConstraint(expected));
        }

        [Test]
        public void CanMatchDatesWithinTolerance()
        {
            DateTime expected = new DateTime(2007, 4, 1, 13, 0, 0);
            DateTime actual = new DateTime(2007, 4, 1, 13, 1, 0);
            TimeSpan tolerance = TimeSpan.FromMinutes(5.0);
            Assert.That(actual, new EqualConstraint(expected).Within(tolerance));
        }
    }

    [TestFixture]
    public class EqualTest : IExpectException
    {

        [Test, ExpectedException(typeof(AssertionException))]
        public void FailedStringMatchShowsFailurePosition()
        {
            Assert.That( "abcdgfe", new EqualConstraint( "abcdefg" ) );
        }

        static readonly string testString = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        [Test, ExpectedException(typeof(AssertionException))]
        public void LongStringsAreTruncated()
        {
            string expected = testString;
            string actual = testString.Replace('k', 'X');

            Assert.That(actual, new EqualConstraint(expected));
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void LongStringsAreTruncatedAtBothEndsIfNecessary()
        {
            string expected = testString;
            string actual = testString.Replace('Z', '?');

            Assert.That(actual, new EqualConstraint(expected));
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void LongStringsAreTruncatedAtFrontEndIfNecessary()
        {
            string expected = testString;
            string actual = testString  + "+++++";

            Assert.That(actual, new EqualConstraint(expected));
        }

//        [Test]
//        public void NamedAndUnnamedColorsCompareAsEqual()
//        {
//            EqualConstraint.SetConstraintForType(typeof(Color), typeof(SameColorAs));
//            Assert.That(System.Drawing.Color.Red,
//                Is.EqualTo(System.Drawing.Color.FromArgb(255, 0, 0)));
//        }

        public void HandleException(Exception ex)
        {
            StringReader rdr = new StringReader(ex.Message);
            /* skip */ rdr.ReadLine();
            string expected = rdr.ReadLine();
            if (expected != null && expected.Length > 11)
                expected = expected.Substring(11);
            string actual = rdr.ReadLine();
            if (actual != null && actual.Length > 11)
                actual = actual.Substring(11);
            string line = rdr.ReadLine();
            Assert.That(line, new NotConstraint(new EqualConstraint(null)), "No caret line displayed");
            int caret = line.Substring(11).IndexOf('^');

            int minLength = Math.Min(expected.Length, actual.Length);
            int minMatch = Math.Min(caret, minLength);

            if (caret != minLength)
            {
                if (caret > minLength ||
                    expected.Substring(0, minMatch) != actual.Substring(0, minMatch) ||
                    expected[caret] == actual[caret])
                    Assert.Fail("Message Error: Caret does not point at first mismatch..." + Environment.NewLine + ex.Message);
            }

            if (expected.Length > 68 || actual.Length > 68 || caret > 68)
                Assert.Fail("Message Error: Strings are not truncated..." + Environment.NewLine + ex.Message);
        }

        public class SameColorAs : Constraint
        {
            private Color expectedColor;

            public SameColorAs(Color expectedColor)
            {
                this.expectedColor = expectedColor;
            }

            public override bool Matches(object actual)
            {
                this.actual = actual;
                return actual is Color && ((Color)actual).ToArgb() == expectedColor.ToArgb();
            }

            public override void WriteDescriptionTo(MessageWriter writer)
            {
                writer.WriteExpectedValue( "same color as " + expectedColor );
            }
        }

    }
}