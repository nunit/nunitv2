using NUnit.Framework;
using NUnit.TestData;
using NUnit.TestUtilities;
using System.Collections;

namespace NUnit.Core.Tests
{
    [TestFixture]
    public class ParameterizedTestCaseTests
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

        [TestCase(2, 2, Result=4.0)]
        public double ConversionOfIntToDoubleIsOK(double x, double y)
        {
            return x + y;
        }

        [TestCase(2.0, 2.0, ExpectedException=typeof(System.ArgumentException))]
        public int ConversionOfDoubleToIntFails(int x, int y)
        {
            return x + y;
        }


        [TestCase(42, ExpectedException = typeof(System.Exception),
                   ExpectedExceptionMessage = "Test Exception")]
        public void ExceptionWithExceptionMessage(int a)
        {
        	throw new System.Exception("Test Exception");
        }
         
        [TestCase(null)]
        public void NullCanBeUsedAsFirstArgument(object a)
        {
        	Assert.IsNull(a);
        }
  
        [Test]
        public void ParameterizedTestCaseMayHaveDescription()
        {
            Test test = (Test)TestBuilder.MakeTestCase(
                typeof(ParameterizedTestMethodFixture), "MethodHasDescriptionSpecified").Tests[0];
            Assert.AreEqual("My Description", test.Description);
        }

        [Test]
        public void ParameterizedTestCaseMayHaveNameSpecified()
        {
            Test test = (Test)TestBuilder.MakeTestCase(
                typeof(ParameterizedTestMethodFixture), "MethodHasTestNameSpecified").Tests[0];
            Assert.AreEqual("XYZ", test.TestName.Name);
            Assert.AreEqual("NUnit.TestData.ParameterizedTestMethodFixture.XYZ", test.TestName.FullName);
        }
    }
}
