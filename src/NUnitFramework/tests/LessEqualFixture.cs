using System;

namespace NUnit.Framework.Tests
{
    [TestFixture]
    public class LessEqualFixture
    {
        private readonly int i1 = 5;
        private readonly int i2 = 8;
        private readonly uint u1 = 12345678;
        private readonly uint u2 = 12345879;
        private readonly float f1 = 3.543F;
        private readonly float f2 = 8.543F;
        private readonly decimal de1 = 53.4M;
        private readonly decimal de2 = 83.4M;
        private readonly double d1 = 4.85948654;
        private readonly double d2 = 8.0;
        private readonly System.Enum e1 = System.Data.CommandType.StoredProcedure;
        private readonly System.Enum e2 = System.Data.CommandType.TableDirect;

        [Test]
        public void LessEqual()
        {
            // Test equality check for all forms
            Assert.LessEqual(i1, i1);
            Assert.LessEqual(i1, i1, "int");
            Assert.LessEqual(i1, i1, "{0}", "int");
            Assert.LessEqual(u1, u1);
            Assert.LessEqual(u1, u1, "uint");
            Assert.LessEqual(u1, u1, "{0}", "uint");
            Assert.LessEqual(d1, d1);
            Assert.LessEqual(d1, d1, "double");
            Assert.LessEqual(d1, d1, "{0}", "double");
            Assert.LessEqual(de1, de1);
            Assert.LessEqual(de1, de1, "decimal");
            Assert.LessEqual(de1, de1, "{0}", "decimal");
            Assert.LessEqual(f1, f1);
            Assert.LessEqual(f1, f1, "float");
            Assert.LessEqual(f1, f1, "{0}", "float");

            // Testing all forms after seeing some bugs. CFP
            Assert.LessEqual(i1, i2);
            Assert.LessEqual(i1, i2, "int");
            Assert.LessEqual(i1, i2, "{0}", "int");
            Assert.LessEqual(u1, u2);
            Assert.LessEqual(u1, u2, "uint");
            Assert.LessEqual(u1, u2, "{0}", "uint");
            Assert.LessEqual(d1, d2);
            Assert.LessEqual(d1, d2, "double");
            Assert.LessEqual(d1, d2, "{0}", "double");
            Assert.LessEqual(de1, de2);
            Assert.LessEqual(de1, de2, "decimal");
            Assert.LessEqual(de1, de2, "{0}", "decimal");
            Assert.LessEqual(f1, f2);
            Assert.LessEqual(f1, f2, "float");
            Assert.LessEqual(f1, f2, "{0}", "float");
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void NotLessEqual()
        {
            Assert.LessEqual(i2, i1);
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void NotLessEqualIComparable()
        {
            Assert.LessEqual(e2, e1);
        }

        [Test]
        public void FailureMessage()
        {
            string msg = null;

            try
            {
                Assert.LessEqual(9, 4);
            }
            catch (AssertionException ex)
            {
                msg = ex.Message;
            }

            StringAssert.Contains("expected: Value less than or equal to 4", msg);
            StringAssert.Contains("but was: 9", msg);
        }
    }
}


