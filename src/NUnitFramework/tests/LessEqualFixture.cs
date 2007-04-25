// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org/?p=license&r=2.4.
// ****************************************************************

using System;

namespace NUnit.Framework.Tests
{
    [TestFixture]
    public class LessEqualFixture : MessageChecker
    {
        private readonly int i1 = 5;
        private readonly int i2 = 8;
		private readonly uint u1 = 12345678;
		private readonly uint u2 = 12345879;
		private readonly long l1 = 12345678;
		private readonly long l2 = 12345879;
		private readonly ulong ul1 = 12345678;
		private readonly ulong ul2 = 12345879;
		private readonly float f1 = 3.543F;
        private readonly float f2 = 8.543F;
        private readonly decimal de1 = 53.4M;
        private readonly decimal de2 = 83.4M;
        private readonly double d1 = 4.85948654;
        private readonly double d2 = 8.0;
        private readonly System.Enum e1 = System.Data.CommandType.StoredProcedure;
        private readonly System.Enum e2 = System.Data.CommandType.TableDirect;

        [Test]
        public void LessOrEqual()
        {
            // Test equality check for all forms
            Assert.LessOrEqual(i1, i1);
            Assert.LessOrEqual(i1, i1, "int");
            Assert.LessOrEqual(i1, i1, "{0}", "int");
			Assert.LessOrEqual(u1, u1);
			Assert.LessOrEqual(u1, u1, "uint");
			Assert.LessOrEqual(u1, u1, "{0}", "uint");
			Assert.LessOrEqual(l1, l1);
			Assert.LessOrEqual(l1, l1, "long");
			Assert.LessOrEqual(l1, l1, "{0}", "long");
			Assert.LessOrEqual(ul1, ul1);
			Assert.LessOrEqual(ul1, ul1, "ulong");
			Assert.LessOrEqual(ul1, ul1, "{0}", "ulong");
			Assert.LessOrEqual(d1, d1);
            Assert.LessOrEqual(d1, d1, "double");
            Assert.LessOrEqual(d1, d1, "{0}", "double");
            Assert.LessOrEqual(de1, de1);
            Assert.LessOrEqual(de1, de1, "decimal");
            Assert.LessOrEqual(de1, de1, "{0}", "decimal");
            Assert.LessOrEqual(f1, f1);
            Assert.LessOrEqual(f1, f1, "float");
            Assert.LessOrEqual(f1, f1, "{0}", "float");

            // Testing all forms after seeing some bugs. CFP
            Assert.LessOrEqual(i1, i2);
            Assert.LessOrEqual(i1, i2, "int");
            Assert.LessOrEqual(i1, i2, "{0}", "int");
            Assert.LessOrEqual(u1, u2);
            Assert.LessOrEqual(u1, u2, "uint");
            Assert.LessOrEqual(u1, u2, "{0}", "uint");
			Assert.LessOrEqual(l1, l2);
			Assert.LessOrEqual(l1, l2, "long");
			Assert.LessOrEqual(l1, l2, "{0}", "long");
			Assert.LessOrEqual(ul1, ul2);
			Assert.LessOrEqual(ul1, ul2, "ulong");
			Assert.LessOrEqual(ul1, ul2, "{0}", "ulong");
			Assert.LessOrEqual(d1, d2);
            Assert.LessOrEqual(d1, d2, "double");
            Assert.LessOrEqual(d1, d2, "{0}", "double");
            Assert.LessOrEqual(de1, de2);
            Assert.LessOrEqual(de1, de2, "decimal");
            Assert.LessOrEqual(de1, de2, "{0}", "decimal");
            Assert.LessOrEqual(f1, f2);
            Assert.LessOrEqual(f1, f2, "float");
            Assert.LessOrEqual(f1, f2, "{0}", "float");
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void NotLessOrEqual()
        {
			expectedMessage =
				"  Expected: less than or equal to 5" + Environment.NewLine +
				"  But was:  8" + Environment.NewLine;
			Assert.LessOrEqual(i2, i1);
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void NotLessEqualIComparable()
        {
			expectedMessage =
				"  Expected: less than or equal to StoredProcedure" + Environment.NewLine +
				"  But was:  TableDirect" + Environment.NewLine;
			Assert.LessOrEqual(e2, e1);
        }

        [Test,ExpectedException(typeof(AssertionException))]
        public void FailureMessage()
        {
			expectedMessage =
				"  Expected: less than or equal to 4" + Environment.NewLine +
				"  But was:  9" + Environment.NewLine;
            Assert.LessOrEqual(9, 4);
        }
    }
}


