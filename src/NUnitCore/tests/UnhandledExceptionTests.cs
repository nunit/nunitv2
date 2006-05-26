using System;
using System.Collections;
using System.Text;
using NUnit.Framework;

namespace NUnit.Core.Tests
{
    [TestFixture, Explicit( "NYI" )]
    public class UnhandledExceptionTests
    {
        #region Normal
        [NUnit.Framework.Test]
        public void Normal()
        {
            testDummy("Normal");
        }

        private static void testDummy(string dummyName)
        {
            Type fixtureType = typeof(NUnit.TestData.UnhandledExceptions);
            Test test = TestCaseBuilder.Make(fixtureType, dummyName);
            TestResult result = test.Run(NullListener.NULL);
            Assert.IsTrue(result.IsFailure, "{0} test should have failed", dummyName);
            Assert.AreEqual("System.ApplicationException : Test exception", result.Message);
        }
        #endregion Normal

        #region Threaded
        [NUnit.Framework.Test]
        public void Threaded()
        {
            testDummy("Threaded");
        }
        #endregion Threaded

        #region ThreadedAndWait
        [NUnit.Framework.Test]
        public void ThreadedAndWait()
        {
            testDummy("ThreadedAndWait");
        }
        #endregion ThreadedAndWait

        #region ThreadedAndForget
        [NUnit.Framework.Test]
        public void ThreadedAndForget()
        {
            testDummy("ThreadedAndForget");
        }
        #endregion ThreadedAndForget
    }

}
