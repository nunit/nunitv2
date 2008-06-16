using NUnit.Framework;
using NUnit.TestData;
using NUnit.TestUtilities;
using System.Collections;

namespace NUnit.Core.Tests
{
    [TestFixture]
    public class FactoryTests
    {
        [Test, Factory("StaticProperty")]
        public void FactoryCanBeStaticProperty(string factory)
        {
            Assert.AreEqual("StaticProperty", factory);
        }

        static IEnumerable StaticProperty
        {
            get { return new object[] { new object[] { "StaticProperty" } }; }
        }

        [Test, Factory("InstanceProperty")]
        public void FactoryCanBeInstanceProperty(string factory)
        {
            Assert.AreEqual("InstanceProperty", factory);
        }

        IEnumerable InstanceProperty
        {
            get { return new object[] { new object[] { "InstanceProperty" } }; }
        }

        [Test, Factory("StaticMethod")]
        public void FactoryCanBeStaticMethod(string factory)
        {
            Assert.AreEqual("StaticMethod", factory);
        }

        static IEnumerable StaticMethod()
        {
            return new object[] { new object[] { "StaticMethod" } };
        }

        [Test, Factory("StaticField")]
        public void FactoryCanBeStaticField(string factory)
        {
            Assert.AreEqual("StaticField", factory);
        }

        static object[] StaticField =
            { new object[] { "StaticField" } };

        [Test, Factory("CheckCurrentDirectory")]
        public void FactoryIsInvokedWithCorrectCurrentDirectory(bool isOK)
        {
            Assert.That(isOK);
        }

        [Test, Factory("MyData")]
        public void FactoryMayReturnArgumentsAsObjectArray(int n, int d, int q)
        {
            Assert.AreEqual(q, n / d);
        }

        //[DataSource("MyIntData")]
        //public void FactoryMayReturnArgumentsAsIntArray(int n, int d, int q)
        //{
        //    Assert.AreEqual(q, n / d);
        //}

        //[DataSource("EvenNumbers")]
        //public void FactoryMayReturnSingleArgumentAlone(int n)
        //{
        //    Assert.AreEqual(0, n % 2);
        //}

        [Test, Factory("Params")]
        public int FactoryMayReturnArgumentsAsParamSet(int n, int d)
        {
            return n / d;
        }

        [Test]
        [Factory("MyData")]
        [Factory("MoreData")]
        [TestCase(12, 0, 0, ExpectedException = typeof(System.DivideByZeroException))]
        public void TestMayUseMultipleFactories(int n, int d, int q)
        {
            Assert.AreEqual(q, n / d);
        }

        [Test, Factory(typeof(DivideDataProvider), "HereIsTheData")]
        public void FactoryMayBeIdentifiedByType(int n, int d, int q)
        {
            Assert.AreEqual(q, n / d);
        }

        public static IEnumerable MyData
        {
            get
            {
#if NET_2_0
                yield return new object[] { 12, 3, 4 };
                yield return new object[] { 12, 4, 3 };
                yield return new object[] { 12, 6, 2 };
#else
                ArrayList list = new ArrayList();
                list.Add(new object[] { 12, 3, 4 });
                list.Add(new object[] { 12, 4, 3 });
                list.Add(new object[] { 12, 6, 2 });
                return list;
#endif
            }
        }

        public static IEnumerable MyIntData
        {
            get
            {
#if NET_2_0
                yield return new int[] { 12, 3, 4 };
                yield return new int[] { 12, 4, 3 };
                yield return new int[] { 12, 6, 2 };
#else
                ArrayList list = new ArrayList();
                list.Add(new int[] { 12, 3, 4 });
                list.Add(new int[] { 12, 4, 3 });
                list.Add(new int[] { 12, 6, 2 });
                return list;
#endif
            }
        }

        private static IEnumerable EvenNumbers
        {
            get
            {
                ArrayList list = new ArrayList();
                list.Add(new int[] { 2 });
                list.Add(new int[] { 4 });
                list.Add(new int[] { 6 });
                list.Add(new int[] { 8 });
                return list;
            }
        }

        private static IEnumerable CheckCurrentDirectory
        {
            get
            {
                return new object[] { new object[] { System.IO.File.Exists("nunit.core.tests.dll") } };
            }
        }

        private static IEnumerable MoreData
        {
            get
            {
                return new object[] { new object[] { 12, 1, 12 }, new object[] { 12, 2, 6 } };
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
#if NET_2_0
                    yield return new TestCaseData(0, 0, 0)
                        .WithName("ThisOneShouldThow")
                        .WithDescription("Demonstrates use of ExpectedException")
                        .Throws(typeof(System.DivideByZeroException));
                    yield return new object[] { 100, 20, 5 };
                    yield return new object[] { 100, 4, 25 };
#else
                    ArrayList list = new ArrayList();
                    list.Add(
                        new TestCaseData( 0, 0, 0)
                            .WithName("ThisOneShouldThow")
                            .WithDescription("Demonstrates use of ExpectedException")
                            .Throws( typeof (System.DivideByZeroException) ));
                    list.Add(new object[] { 100, 20, 5 });
                    list.Add(new object[] {100, 4, 25});
                    return list;
#endif
                }
            }
        }
    }
}
