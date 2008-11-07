using NUnit.Framework;
using NUnit.TestData;
using NUnit.TestUtilities;
using System.Collections;

namespace NUnit.Core.Tests
{
    [TestFixture]
    public class ValueSourceTests
    {
        [Test]
        public void ValueSourceCanBeStaticProperty(
            [ValueSource("StaticProperty")] string source)
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
        public void ValueSourceCanBeInstanceProperty(
            [ValueSource("InstanceProperty")] string source)
        {
            Assert.AreEqual("InstanceProperty", source);
        }

        IEnumerable InstanceProperty
        {
            get { return new object[] { "InstanceProperty" }; }
        }

        [Test]
        public void ValueSourceCanBeStaticMethod(
            [ValueSource("StaticMethod")] string source)
        {
            Assert.AreEqual("StaticMethod", source);
        }

        static IEnumerable StaticMethod()
        {
            return new object[] { "StaticMethod" };
        }

        [Test]
        public void ValueSourceCanBeInstanceMethod(
            [ValueSource("InstanceMethod")] string source)
        {
            Assert.AreEqual("InstanceMethod", source);
        }

        IEnumerable InstanceMethod()
        {
            return new object[] { "InstanceMethod" };
        }

        [Test]
        public void ValueSourceCanBeStaticField(
            [ValueSource("StaticField")] string source)
        {
            Assert.AreEqual("StaticField", source);
        }

        static object[] StaticField = { "StaticField" };

        [Test]
        public void ValueSourceCanBeInstanceField(
            [ValueSource("InstanceField")] string source)
        {
            Assert.AreEqual("InstanceField", source);
        }

        static object[] InstanceField = { "InstanceField" };

        [Test]
        public void ValueSourceIsInvokedWithCorrectCurrentDirectory(
            [ValueSource("CheckCurrentDirectory")] bool isOK)
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
            [ValueSource("Numerators")] int n, 
            [ValueSource("Denominators")] int d, 
            [ValueSource("Quotients")] int q)
        {
            Assert.AreEqual(q, n / d);
        }

        static int[] Numerators = new int[] { 12, 12, 12 };
        static int[] Denominators = new int[] { 3, 4, 6 };
        static int[] Quotients = new int[] { 4, 3, 2 };

        [Test, Sequential]
        public void ValueSourceMayBeInAnotherClass(
            [ValueSource(typeof(DivideDataProvider), "Numerators")] int n,
            [ValueSource(typeof(DivideDataProvider), "Denominators")] int d,
            [ValueSource(typeof(DivideDataProvider), "Quotients")] int q)
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
