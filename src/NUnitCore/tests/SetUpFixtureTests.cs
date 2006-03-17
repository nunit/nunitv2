using System;
using NUnit.Framework;

namespace NUnit.Core.Tests.SetupFixture
{
    [TestFixture]
    public class Test
    {
        #region SetUp
        [SetUp]
        public void SetUp()
        {
            TestUtilities.SimpleEventRecorder.Clear();
        }
        #endregion SetUp

        private TestResult runTests(string nameSpace)
        {
            return runTests(nameSpace, TestFilter.Empty);
        }
        private TestResult runTests(string nameSpace,TestFilter filter)
        {
            TestSuiteBuilder builder = new TestSuiteBuilder();

            TestSuite suite;
            if(nameSpace == null)
                suite = builder.Build(typeof(NUnit.TestData.SetupFixture.Namespace1.SomeTestFixture).Assembly.FullName);
            else
                suite = builder.Build(typeof(NUnit.TestData.SetupFixture.Namespace1.SomeTestFixture).Assembly.FullName, nameSpace);
            return suite.Run(new NullListener(),filter);
        }

        #region Builder
        /// <summary>
        /// Tests that the TestSuiteBuilder correctly interperets a SetupFixture class as a 'virtual namespace' into which 
        /// all it's sibling classes are inserted.
        /// </summary>
        [NUnit.Framework.Test]
        public void Builder()
        {
            string nameSpace = "NUnit.TestData.SetupFixture.Namespace1";
            TestSuiteBuilder builder = new TestSuiteBuilder();
            TestSuite suite = builder.Build(typeof(NUnit.TestData.SetupFixture.Namespace1.SomeTestFixture).Assembly.FullName, nameSpace);

            Assert.IsNotNull(suite);

            Assert.AreEqual(typeof(NUnit.TestData.SetupFixture.Namespace1.SomeTestFixture).Assembly.FullName, suite.Name);
            Assert.AreEqual(1, suite.Tests.Count);

            string[] nameSpaceBits = nameSpace.Split('.');
            for (int i = 0; i < nameSpaceBits.Length; i++)
            {
                suite = suite.Tests[0] as TestSuite;
                Assert.AreEqual(nameSpaceBits[i], suite.Name);
                Assert.AreEqual(1, suite.Tests.Count);
            }

            Assert.IsInstanceOfType(typeof(SetUpFixture), suite);

            suite = suite.Tests[0] as TestSuite;
            Assert.AreEqual("SomeTestFixture", suite.Name);
            Assert.AreEqual(1, suite.Tests.Count);
        }
        #endregion Builder

        #region NoNamespaceBuilder
        /// <summary>
        /// Tests that the TestSuiteBuilder correctly interperets a SetupFixture class with no parent namespace 
        /// as a 'virtual assembly' into which all it's sibling fixtures are inserted.
        /// </summary>
        //[NUnit.Framework.Test]
        public void NoNamespaceBuilder()
        {
            TestSuiteBuilder builder = new TestSuiteBuilder();
            TestSuite suite = builder.Build(typeof(NUnit.TestData.SetupFixture.Namespace1.SomeTestFixture).Assembly.FullName);

            Assert.IsNotNull(suite);

            Assert.AreEqual(typeof(NUnit.TestData.SetupFixture.Namespace1.SomeTestFixture).Assembly.FullName, suite.Name);
            Assert.AreEqual(1, suite.Tests.Count);
            suite = suite.Tests[0] as TestSuite;
            Assert.AreEqual("[default namespace]", suite.Name);
            Assert.IsInstanceOfType(typeof(SetUpFixture), suite);

            //suite = suite.Tests[0] as TestSuite;
            //Assert.AreEqual("SomeTestFixture", suite.Name);
            //Assert.AreEqual(1, suite.TestCount);
        }
        #endregion NoNamespaceBuilder


        #region Simple
        [NUnit.Framework.Test]
        public void Simple()
        {
            Assert.IsTrue(runTests("NUnit.TestData.SetupFixture.Namespace1").IsSuccess);
            TestUtilities.SimpleEventRecorder.Verify("NamespaceSetup",
                                    "FixtureSetup", "Setup", "Test", "TearDown", "FixtureTearDown",
                                  "NamespaceTearDown");
        }
        #endregion Simple

        #region TwoTestFixtures
        [NUnit.Framework.Test]
        public void TwoTestFixtures()
        {
            Assert.IsTrue(runTests("NUnit.TestData.SetupFixture.Namespace2").IsSuccess);
            TestUtilities.SimpleEventRecorder.Verify("NamespaceSetup",
                                    "FixtureSetup", "Setup", "Test", "TearDown", "FixtureTearDown",
                                    "FixtureSetup", "Setup", "Test", "TearDown", "FixtureTearDown",
                                  "NamespaceTearDown");
        }
        #endregion TwoTestFixtures

        #region SubNamespace
        [NUnit.Framework.Test]
        public void SubNamespace()
        {
            Assert.IsTrue(runTests("NUnit.TestData.SetupFixture.Namespace3").IsSuccess);
            TestUtilities.SimpleEventRecorder.Verify("NamespaceSetup",
                                    "FixtureSetup", "Setup", "Test", "TearDown", "FixtureTearDown",
                                    "SubNamespaceSetup",
                                        "FixtureSetup", "Setup", "Test", "TearDown", "FixtureTearDown",
                                    "SubNamespaceTearDown",
                                  "NamespaceTearDown");
        }
        #endregion SubNamespace

        #region TwoSetUpFixtures
        [NUnit.Framework.Test]
        public void TwoSetUpFixtures()
        {
            Assert.IsTrue(runTests("NUnit.TestData.SetupFixture.Namespace4").IsSuccess);
            TestUtilities.SimpleEventRecorder.Verify("NamespaceSetup2",
                                    "FixtureSetup", "Setup", "Test", "TearDown", "FixtureTearDown",
                                  "NamespaceTearDown2");
        }
        #endregion TwoSetUpFixtures

        #region NoNamespaceSetupFixture
        //[NUnit.Framework.Test]
        public void NoNamespaceSetupFixture()
        {
            //This line should run only the SomeTestFixture fixture at the root of the namespace hierarchy.
            //For some reason though, it runs all the tests
            TestResult result = runTests(null, new Filters.NameFilter(TestName.Parse("SomeTestFixture")));
            Assert.AreEqual(1, result.Test.TestCount);
            Assert.IsTrue(result.IsSuccess);
            TestUtilities.SimpleEventRecorder.Verify("RootNamespaceSetup",
                                    "Test",
                                  "RootNamespaceTearDown");
        }
        #endregion NoNamespaceSetupFixture
    }
}












