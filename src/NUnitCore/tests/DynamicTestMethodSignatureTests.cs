using System;
using NUnit.Framework;
using NUnit.Framework.Syntax.CSharp;
using NUnit.TestUtilities;
using NUnit.TestData;

namespace NUnit.Core.Tests
{
    [TestFixture]
    public class DynamicTestMethodSignatureTests
    {
        private Test fixture;

        [SetUp]
        public void CreateFixture()
        {
            fixture = TestBuilder.MakeFixture(typeof(DynamicTestMethodSignatureFixture));
        }

        private void AssertRunnable(string name)
        {
            AssertRunnable(name, ResultState.Success);
        }

        private void AssertRunnable(string name, ResultState resultState)
        {
            Test test = TestFinder.RequiredChildTest(name, fixture);
            Assert.That(test.RunState, Is.EqualTo(RunState.Runnable));
            Assert.That(test.TestCount, Is.EqualTo(1));
            TestResult result = test.Run(NullListener.NULL, TestFilter.Empty);
            if (result.HasResults)
                result = (TestResult)result.Results[0];
            Assert.That(result.ResultState, Is.EqualTo(resultState));
        }

        private void AssertNotRunnable(string name)
        {
            Test test = TestFinder.RequiredChildTest(name, fixture);
            Assert.That(test.RunState, Is.EqualTo(RunState.NotRunnable));
            Assert.That(test.TestCount, Is.EqualTo(1));
            TestResult result = test.Run(NullListener.NULL, TestFilter.Empty);
            Assert.That(result.ResultState, Is.EqualTo(ResultState.NotRunnable));
        }

        private void AssertChildNotRunnable(string name)
        {
            Test test = (Test)TestFinder.RequiredChildTest(name, fixture).Tests[0];
            Assert.That(test.RunState, Is.EqualTo(RunState.NotRunnable));
            Assert.That(test.TestCount, Is.EqualTo(1));
            TestResult result = test.Run(NullListener.NULL, TestFilter.Empty);
            Assert.That(result.ResultState, Is.EqualTo(ResultState.NotRunnable));
        }

        [Test]
        public void DynamicTestWithNoArgumentsIsNotRunnable()
        {
            AssertNotRunnable("TestMethodWithNoArguments");
        }

        [Test]
        public void TestMethodWithArgumentsNotProvidedIsNotRunnable()
        {
            AssertNotRunnable("TestMethodWithArgumentsNotProvided");
        }

        [Test]
        public void TestMethodWithArgumentsProvidedIsRunnable()
        {
            AssertRunnable("TestMethodWithArgumentsProvided");
        }

    }
}
