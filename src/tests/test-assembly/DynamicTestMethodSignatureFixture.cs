using System;
using NUnit.Framework;

namespace NUnit.TestData
{
    [TestFixture]
    public class DynamicTestMethodSignatureFixture
    {
        public static int Tests = 18;
        public static int Runnable = 8;
        public static int NotRunnable = 10;
        public static int Errors = 0;
        public static int Failures = 0;

        [DynamicTest]
        public void TestMethodWithNoArguments() { }

        [DynamicTest]
        public void TestMethodWithArgumentsNotProvided(int x, int y, string label) { }

        [DynamicTest]
        public static void StaticTestMethodWithArgumentsNotProvided(int x, int y, string label) { }

        [DynamicTest]
        [TestCase(5, 2, "ABC")]
        public void TestMethodWithArgumentsProvided(int x, int y, string label)
        {
            Assert.AreEqual(5, x);
            Assert.AreEqual(2, y);
            Assert.AreEqual("ABC", label);
        }

        [DynamicTest]
        [TestCase(5, 2, "ABC")]
        public static void StaticTestMethodWithArgumentsProvided(int x, int y, string label)
        {
            Assert.AreEqual(5, x);
            Assert.AreEqual(2, y);
            Assert.AreEqual("ABC", label);
        }

        [DynamicTest]
        [TestCase(2, 2)]
        public void TestMethodWithWrongNumberOfArgumentsProvided(int x, int y, string label)
        {
        }

        [DynamicTest]
        [TestCase(2, 2, 3.5)]
        public void TestMethodWithWrongArgumentTypesProvided(int x, int y, string label)
        {
        }

        [DynamicTest]
        [TestCase(2, 2)]
        public static void StaticTestMethodWithWrongNumberOfArgumentsProvided(int x, int y, string label)
        {
        }

        [DynamicTest]
        [TestCase(2, 2, 3.5)]
        public static void StaticTestMethodWithWrongArgumentTypesProvided(int x, int y, string label)
        {
        }

        [DynamicTest]
        [TestCase(3.7, 2, 5.7)]
        public void TestMethodWithConvertibleArguments(double x, double y, double sum)
        {
            Assert.AreEqual(sum, x + y, 0.0001);
        }

        [DynamicTest]
        [TestCase(12, 3, 4)]
        [TestCase(12, 2, 6)]
        [TestCase(12, 4, 3)]
        public void TestMethodWithMultipleTestCases(int n, int d, int q)
        {
            Assert.AreEqual(q, n / d);
        }

        //		[DynamicTest]
        //		public abstract void AbstractTestMethod() { }

        [DynamicTest]
        protected void ProtectedTestMethod() { }

        [DynamicTest]
        private void PrivateTestMethod() { }

        [DynamicTest]
        public bool TestMethodWithReturnType()
        {
            return true;
        }
    }
}
