using System;
using NUnit.Core;
using NUnit.Framework;

namespace NUnit.TestUtilities
{
    public class TestAssert
    {
        #region IsRunnable
        public static void IsRunnable(Type type)
        {
            TestSuite suite = TestBuilder.MakeFixture(type);
            Assert.AreEqual(RunState.Runnable, suite.RunState);
            TestResult result = suite.Run(NullListener.NULL, TestFilter.Empty);
            Assert.AreEqual(ResultState.Success, result.ResultState);
        }

        public static void IsRunnable(Type type, string name)
        {
            IsRunnable(type, name, ResultState.Success);
        }

        public static void IsRunnable(Type type, string name, ResultState resultState)
        {
            Test fixture = TestBuilder.MakeFixture(type);
            Test test = TestFinder.RequiredChildTest(name, fixture);
            Assert.That(test.RunState, Is.EqualTo(RunState.Runnable));
            Assert.That(test.TestCount, Is.EqualTo(1));
            TestResult result = test.Run(NullListener.NULL, TestFilter.Empty);
            if (result.HasResults)
                result = (TestResult)result.Results[0];
            Assert.That(result.ResultState, Is.EqualTo(resultState));
        }
        #endregion

        #region IsNotRunnable
        public static void IsNotRunnable(Type type)
        {
            TestSuite suite = TestBuilder.MakeFixture(type);
            Assert.AreEqual(RunState.NotRunnable, suite.RunState);
            TestResult result = suite.Run(NullListener.NULL, TestFilter.Empty);
            Assert.AreEqual(ResultState.NotRunnable, result.ResultState);
        }

        public static void IsNotRunnable(Type type, string name)
        {
            Test fixture = TestBuilder.MakeFixture(type);
            Test test = TestFinder.RequiredChildTest(name, fixture);
            Assert.That(test.RunState, Is.EqualTo(RunState.NotRunnable));
            Assert.That(test.TestCount, Is.EqualTo(1));
            TestResult result = test.Run(NullListener.NULL, TestFilter.Empty);
            Assert.That(result.ResultState, Is.EqualTo(ResultState.NotRunnable));
        }

        public static void ChildNotRunnable(Type type, string name)
        {
            Test fixture = TestBuilder.MakeFixture(type);
            Test test = (Test)TestFinder.RequiredChildTest(name, fixture).Tests[0];
            Assert.That(test.RunState, Is.EqualTo(RunState.NotRunnable));
            Assert.That(test.TestCount, Is.EqualTo(1));
            TestResult result = test.Run(NullListener.NULL, TestFilter.Empty);
            Assert.That(result.ResultState, Is.EqualTo(ResultState.NotRunnable));
        }
        #endregion
        
        private TestAssert() { }
    }
}
