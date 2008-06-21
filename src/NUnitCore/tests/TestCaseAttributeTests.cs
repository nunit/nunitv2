using System;
using NUnit.Framework;
using NUnit.TestData;
using NUnit.TestUtilities;
using System.Collections;

namespace NUnit.Core.Tests
{
    [TestFixture]
    public class TestCaseAttributeTests
    {
        [TestCase(12, 3, 4)]
        [TestCase(12, 2, 6)]
        [TestCase(12, 4, 3)]
        [TestCase(12, 0, 0, ExpectedException = typeof(System.DivideByZeroException))]
        [TestCase(12, 0, 0, ExpectedExceptionName = "System.DivideByZeroException")]
        public void IntegerDivisionWithResultPassedToTest(int n, int d, int q)
        {
            Assert.AreEqual(q, n / d);
        }

        [TestCase(12, 3, Result = 4)]
        [TestCase(12, 2, Result = 6)]
        [TestCase(12, 4, Result = 3)]
        [TestCase(12, 0, ExpectedException = typeof(System.DivideByZeroException))]
        [TestCase(12, 0, ExpectedExceptionName = "System.DivideByZeroException",
            TestName = "DivisionByZeroThrowsException")]
        public int IntegerDivisionWithResultCheckedByNUnit(int n, int d)
        {
            return n / d;
        }

        [TestCase(2, 2, Result=4)]
        public double CanConvertIntToDouble(double x, double y)
        {
            return x + y;
        }

		[TestCase(2.0, 2.0, Result=4.0)]
		public int CanConvertDoubleToInt(int x, int y)
		{
			return x + y;
		}

        [TestCase(2, 2, Result = 4)]
        public decimal CanConvertIntToDecimal(decimal x, decimal y)
        {
            return x + y;
        }

        [TestCase("2.2", "3.3", Result = 5.5)]
        public decimal CanConvertStringToDecimal(decimal x, decimal y)
        {
            return x + y;
        }

        [TestCase(2.2, 3.3, Result = 5.5)]
        public decimal CanConvertDoubleToDecimal(decimal x, decimal y)
        {
            return x + y;
        }

        [Test]
		public void ConversionOverflowGivesNonRunnableTest()
		{
			Test test = (Test)TestBuilder.MakeTestCase(
				typeof(TestCaseAttributeFixture), "MethodCausesConversionOverflow").Tests[0];
			Assert.AreEqual(RunState.NotRunnable, test.RunState);
		}

        [TestCase("12-October-1942")]
        public void CanConvertStringToDateTime(DateTime dt)
        {
            Assert.AreEqual(1942, dt.Year);
        }

        [TestCase(42, ExpectedException = typeof(System.Exception),
                   ExpectedExceptionMessage = "Test Exception")]
        public void CanSpecifyExceptionMessage(int a)
        {
        	throw new System.Exception("Test Exception");
        }
         
        [TestCase(null)]
        public void CanPassNullAsFirstArgument(object a)
        {
        	Assert.IsNull(a);
        }
  
        [Test]
        public void CanSpecifyDescription()
        {
			Test test = (Test)TestBuilder.MakeTestCase(
				typeof(TestCaseAttributeFixture), "MethodHasDescriptionSpecified").Tests[0];
			Assert.AreEqual("My Description", test.Description);
		}

        [Test]
        public void CanSpecifyTestName()
        {
            Test test = (Test)TestBuilder.MakeTestCase(
                typeof(TestCaseAttributeFixture), "MethodHasTestNameSpecified").Tests[0];
            Assert.AreEqual("XYZ", test.TestName.Name);
            Assert.AreEqual("NUnit.TestData.TestCaseAttributeFixture.XYZ", test.TestName.FullName);
        }
    }
}
