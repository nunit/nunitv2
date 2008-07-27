using NUnit.Framework;
using NUnit.TestData;
using NUnit.TestUtilities;
using System.Collections;

namespace NUnit.Core.Tests
{
    [TestFixture]
    public class DataSourceTests
    {
        [Test]
        public void DataSourceCanBeStaticProperty(
            [DataSource("StaticProperty")] string source)
        {
            Assert.AreEqual("StaticProperty", source);
        }

        static IEnumerable StaticProperty
        {
            get 
            {
#if NET_2_0
                yield return "StaticProperty";
#else
                return new object[] { "StaticProperty" }; 
#endif
            }
        }

        [Test]
        public void DataSourceCanBeInstanceProperty(
            [DataSource("InstanceProperty")] string source)
        {
            Assert.AreEqual("InstanceProperty", source);
        }

        IEnumerable InstanceProperty
        {
            get { return new object[] { "InstanceProperty" }; }
        }

        [Test]
        public void DataSourceCanBeStaticMethod(
            [DataSource("StaticMethod")] string source)
        {
            Assert.AreEqual("StaticMethod", source);
        }

        static IEnumerable StaticMethod()
        {
            return new object[] { "StaticMethod" };
        }

        [Test]
        public void DataSourceCanBeInstanceMethod(
            [DataSource("InstanceMethod")] string source)
        {
            Assert.AreEqual("InstanceMethod", source);
        }

        IEnumerable InstanceMethod()
        {
            return new object[] { "InstanceMethod" };
        }

        [Test]
        public void DataSourceCanBeStaticField(
            [DataSource("StaticField")] string source)
        {
            Assert.AreEqual("StaticField", source);
        }

        static object[] StaticField = { "StaticField" };

        [Test]
        public void DataSourceCanBeInstanceField(
            [DataSource("InstanceField")] string source)
        {
            Assert.AreEqual("InstanceField", source);
        }

        static object[] InstanceField = { "InstanceField" };

        [Test]
        public void DataSourceIsInvokedWithCorrectCurrentDirectory(
            [DataSource("CheckCurrentDirectory")] bool isOK)
        {
            Assert.That(isOK);
        }

        private static IEnumerable CheckCurrentDirectory
        {
            get
            {
                return new object[] { System.IO.File.Exists("nunit.core.tests.dll") };
            }
        }

        [Test, Sequential]
        public void MultipleArguments(
            [DataSource("Numerators")] int n, 
            [DataSource("Denominators")] int d, 
            [DataSource("Quotients")] int q)
        {
            Assert.AreEqual(q, n / d);
        }

        static int[] Numerators = new int[] { 12, 12, 12 };
        static int[] Denominators = new int[] { 3, 4, 6 };
        static int[] Quotients = new int[] { 4, 3, 2 };

        [Test, Sequential]
        public void DataSourceMayBeInAnotherClass(
            [DataSource(typeof(DivideDataProvider), "Numerators")] int n,
            [DataSource(typeof(DivideDataProvider), "Denominators")] int d,
            [DataSource(typeof(DivideDataProvider), "Quotients")] int q)
        {
            Assert.AreEqual(q, n / d);
        }

        private class DivideDataProvider
        {
            static int[] Numerators = new int[] { 12, 12, 12 };
            static int[] Denominators = new int[] { 3, 4, 6 };
            static int[] Quotients = new int[] { 4, 3, 2 };
        }
    }
}
