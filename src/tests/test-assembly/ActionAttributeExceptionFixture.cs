#if CLR_2_0 || CLR_4_0
using System;
using System.Reflection;
using NUnit.Framework;

namespace NUnit.TestData
{
    [ExceptionThrowingAction]
    [TestFixture]
    public class ActionAttributeExceptionFixture
    {
        public static bool SetUpRun = false;
        public static bool TestRun = false;
        public static bool TearDownRun = false;

        public static void Reset()
        {
            SetUpRun = false;
            TestRun = false;
            TearDownRun = false;
        }

        [SetUp]
        public void SetUp()
        {
            SetUpRun = true;
        }

        [TearDown]
        public void TearDown()
        {
            TearDownRun = false;
        }

        [ExceptionThrowingAction]
        [Test]
        public void SomeTest()
        {
            TestRun = true;
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ExceptionThrowingActionAttribute : Attribute, ITestSuiteAction, ITestCaseAction
    {
        private static bool _ThrowBeforeSuiteException;
        private static bool _ThrowAfterSuiteException;
        private static bool _ThrowBeforeCaseException;
        private static bool _ThrowAfterCaseException;

        public static void Reset()
        {
            ThrowBeforeSuiteException = false;
            ThrowAfterSuiteException = false;
            ThrowBeforeCaseException = false;
            ThrowAfterCaseException = false;
        }

        public void BeforeTestSuite(object fixture)
        {
            if (ThrowBeforeSuiteException)
                throw new InvalidOperationException("Failure in BeforeTestSuite.");
        }

        public void AfterTestSuite(object fixture)
        {
            if (ThrowAfterSuiteException)
                throw new InvalidOperationException("Failure in AfterTestSuite.");
        }

        public void BeforeTestCase(object fixture, MethodInfo method)
        {
            if (ThrowBeforeCaseException)
                throw new InvalidOperationException("Failure in BeforeTestCase.");
        }

        public void AfterTestCase(object fixture, MethodInfo method)
        {
            if (ThrowAfterCaseException)
                throw new InvalidOperationException("Failure in AfterTestCase.");
        }

        public static bool ThrowBeforeSuiteException
        {
            get { return _ThrowBeforeSuiteException; }
            set { _ThrowBeforeSuiteException = value; }
        }

        public static bool ThrowAfterSuiteException
        {
            get { return _ThrowAfterSuiteException; }
            set { _ThrowAfterSuiteException = value; }
        }

        public static bool ThrowBeforeCaseException
        {
            get { return _ThrowBeforeCaseException; }
            set { _ThrowBeforeCaseException = value; }
        }

        public static bool ThrowAfterCaseException
        {
            get { return _ThrowAfterCaseException; }
            set { _ThrowAfterCaseException = value; }
        }
    }
}
#endif