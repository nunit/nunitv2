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

        [TestCase(2, 2, Result = 4.0)]
        public double ConversionOfIntToDoubleIsOK(double x, double y)
        {
            return x + y;
        }

        [TestCase(2.0, 2.0, ExpectedException = typeof(System.ArgumentException))]
        public int ConversionOfDoubleToIntFails(int x, int y)
        {
            return x + y;
        }

        [Test]
        public void ParameterizedTestCaseMayHaveDescription()
        {
            Test test = TestBuilder.MakeTestCase(
                typeof(ParameterizedTestMethodFixture), "MethodHasDescriptionSpecified");
            Assert.AreEqual("My Description", test.Description);
        }

        [Test]
        public void ParameterizedTestCaseMayHaveNameSpecified()
        {
            Test test = TestBuilder.MakeTestCase(
                typeof(ParameterizedTestMethodFixture), "MethodHasTestNameSpecified");
            Assert.AreEqual("XYZ", test.TestName.Name);
            Assert.AreEqual("NUnit.TestData.ParameterizedTestMethodFixture.XYZ", test.TestName.FullName);
        }

        [DataSource("MyData")]
        public void DataSourcePropertyReturnsArguments(int n, int d, int q)
        {
            Assert.AreEqual(q, n / d);
        }

        [DataSource("Params")]
        public int DataSourcePropertyReturnsParamSet(int n, int d)
        {
            return n/d;
        }

        [DataSource("MyData")]
        [DataSource("MoreData")]
        [TestCase(12,0, 0, ExpectedException = typeof(System.DivideByZeroException))]
        public void MultipleDataSources(int n, int d, int q)
        {
            Assert.AreEqual(q, n / d);
        }

        [DataSource(typeof(DivideDataProvider))]
        public void DataSourceTypeHasProperty(int n, int d, int q)
        {
            Assert.AreEqual(q, n/d);   
        }

        public static ICollection MyData
        {
            get
            {          
                ArrayList list = new ArrayList();
                list.Add(new object[] { 12, 3, 4 });
                list.Add(new object[] { 12, 4, 3 });
                list.Add(new object[] { 12, 6, 2 });
                return list;
            }
        }

        private static IList MoreData
        {
            get
            {
                return new object[] {new object[] {12, 1, 12}, new object[] {12, 2, 6}};
            }
        }

        static IEnumerable Params
        {
            get
            {
                ArrayList list = new ArrayList();
                list.Add(new TestCaseData(24, 3).Returns(8));
                list.Add(new TestCaseData(24, 2).Returns(12));
                return list;
            }
        }

        private class DivideDataProvider
        {
            public static IEnumerable HereIsTheData
            {
                get
                {
                    ArrayList list = new ArrayList();
                    list.Add(
                        new TestCaseData( 0, 0, 0)
                            .With.Name("ThisOneShouldThow")
                            .With.Description("Demonstrates use of ExpectedException")
                            .Throws( typeof (System.DivideByZeroException) ));
                    list.Add(new object[] { 100, 20, 5 });
                    list.Add(new object[] {100, 4, 25});
                    return list;
                }
            }
        }
    }
}
