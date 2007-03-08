// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;

namespace NUnit.Framework.Tests
{
    public class MessageWriterTests : AssertionHelper
    {
        protected TextMessageWriter writer;

		[SetUp]
		public void SetUp()
        {
            writer = new TextMessageWriter();
        }
    }

    [TestFixture]
    public class TestMessageWriterTests : MessageWriterTests
    {
        [Test]
        public void ConnectorIsWrittenWithSurroundingSpaces()
        {
            writer.WriteConnector("and");
            Expect(writer.ToString(), EqualTo(" and "));
        }

        [Test]
        public void PredicateIsWrittenWithTrailingSpace()
        {
            writer.WritePredicate("contains");
            Expect(writer.ToString(), EqualTo("contains "));
        }

        [TestFixture]
        public class ExpectedValueTests : ValueTests
        {
            protected override void WriteValue(object obj)
            {
                writer.WriteExpectedValue(obj);
            }
        }

        [TestFixture]
        public class ActualValueTests : ValueTests
        {
            protected override void WriteValue(object obj)
            {
                writer.WriteActualValue( obj );
            }
        }

        public abstract class ValueTests : MessageWriterTests
        {
            protected abstract void WriteValue( object obj);

            [Test]
            public void IntegerIsWrittenAsIs()
            {
                WriteValue(42);
                Expect(writer.ToString(), EqualTo("42"));
            }

            [Test]
            public void StringIsWrittenWithQuotes()
            {
                WriteValue("Hello");
                Expect(writer.ToString(), EqualTo("\"Hello\""));
            }

			// This test currently fails because control character replacement is
			// done at a higher level...
			// TODO: See if we should do it at a lower level
//            [Test]
//            public void ControlCharactersInStringsAreEscaped()
//            {
//                WriteValue("Best Wishes,\r\n\tCharlie\r\n");
//                Assert.That(writer.ToString(), Is.EqualTo("\"Best Wishes,\\r\\n\\tCharlie\\r\\n\""));
//            }

            [Test]
            public void FloatIsWrittenWithTrailingF()
            {
                WriteValue(0.5f);
                Expect(writer.ToString(), EqualTo("0.5f"));
            }

            [Test]
            public void FloatIsWrittenToNineDigits()
            {
                WriteValue(0.33333333333333f);
                int digits = writer.ToString().Length - 3;   // 0.dddddddddf
                Expect(digits, EqualTo(9));
				Expect(writer.ToString().Length, EqualTo(12));
            }

            [Test]
            public void DoubleIsWrittenWithTrailingD()
            {
                WriteValue(0.5d);
                Expect(writer.ToString(), EqualTo("0.5d"));
            }

            [Test]
            public void DoubleIsWrittenToSeventeenDigits()
            {
                WriteValue(0.33333333333333333333333333333333333333333333d);
                Expect(writer.ToString().Length, EqualTo(20)); // add 3 for leading 0, decimal and trailing d
            }

            [Test]
            public void DecimalIsWrittenWithTrailingM()
            {
                WriteValue(0.5m);
                Expect(writer.ToString(), EqualTo("0.5m"));
            }

            [Test]
            public void DecimalIsWrittenToTwentyNineDigits()
            {
                WriteValue(12345678901234567890123456789m);
                Expect(writer.ToString(), EqualTo("12345678901234567890123456789m"));
            }
        }
    }
}
