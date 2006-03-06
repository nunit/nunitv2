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
            EventRegistrar.Clear();
        }
        #endregion SetUp

        private TestResult runTests(string nameSpace)
        {
            return runTests(nameSpace, null);
        }
        private TestResult runTests(string nameSpace,ITestFilter filter)
        {
            TestSuiteBuilder builder = new TestSuiteBuilder();

            TestSuite suite;
            if(nameSpace == null)
                suite = builder.Build(this.GetType().Assembly.FullName);
            else
                suite = builder.Build(this.GetType().Assembly.FullName, nameSpace);
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
            string nameSpace = "NUnit.Core.Tests.SetupFixture.Mocks.Namespace1";
            TestSuiteBuilder builder = new TestSuiteBuilder();
            TestSuite suite = builder.Build(this.GetType().Assembly.FullName, nameSpace);

            Assert.IsNotNull(suite);

            Assert.AreEqual(this.GetType().Assembly.FullName, suite.Name);
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
            TestSuite suite = builder.Build(this.GetType().Assembly.FullName);
            
            Assert.IsNotNull(suite);

            Assert.AreEqual(this.GetType().Assembly.FullName, suite.Name);
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
            Assert.IsTrue(runTests("NUnit.Core.Tests.SetupFixture.Mocks.Namespace1").IsSuccess);
            EventRegistrar.Verify("NamespaceSetup",
                                    "FixtureSetup","Setup","Test","TearDown","FixtureTearDown",
                                  "NamespaceTearDown");
        }
        #endregion Simple

        #region TwoTestFixtures
        [NUnit.Framework.Test]
        public void TwoTestFixtures()
        {
            Assert.IsTrue(runTests("NUnit.Core.Tests.SetupFixture.Mocks.Namespace2").IsSuccess);
            EventRegistrar.Verify("NamespaceSetup", 
                                    "FixtureSetup", "Setup", "Test", "TearDown", "FixtureTearDown",
                                    "FixtureSetup", "Setup", "Test", "TearDown", "FixtureTearDown", 
                                  "NamespaceTearDown");
        }
        #endregion TwoTestFixtures

        #region SubNamespace
        [NUnit.Framework.Test]
        public void SubNamespace()
        {
            Assert.IsTrue(runTests("NUnit.Core.Tests.SetupFixture.Mocks.Namespace3").IsSuccess);
            EventRegistrar.Verify("NamespaceSetup",
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
            Assert.IsTrue(runTests("NUnit.Core.Tests.SetupFixture.Mocks.Namespace4").IsSuccess);
            EventRegistrar.Verify("NamespaceSetup2",
                                    "FixtureSetup", "Setup", "Test", "TearDown", "FixtureTearDown",
                                  "NamespaceTearDown2");
        }
        #endregion TwoSetUpFixtures

        #region NoNamespaceSetupFixture
       // [NUnit.Framework.Test]
        public void NoNamespaceSetupFixture()
        {
            //This line should run only the SomeTestFixture fixture at the root of the namespace hierarchy.
            //For some reason though, it runs all the tests
            TestResult result = runTests(null, new Filters.NameFilter(TestName.Parse("SomeSetupFixture")));
            Assert.AreEqual(1, result.Test.TestCount);
            Assert.IsTrue(result.IsSuccess);
            EventRegistrar.Verify("RootNamespaceSetup",
                                    "Test",
                                  "RootNamespaceTearDown");
        }
        #endregion NoNamespaceSetupFixture
    }

    #region EventRegistrar
    /// <summary>
    /// A helper to Verify that Setup/Teardown 'events' occur, and that they are in the correct order...
    /// </summary>
    public class EventRegistrar
    {
        private static System.Collections.Queue _events;

        /// <summary>
        /// Initializes the <see cref="T:EventRegistrar"/> 'static' class.
        /// </summary>
        static EventRegistrar()
        {
            _events = new System.Collections.Queue();
        }

        /// <summary>
        /// Registers an event.
        /// </summary>
        /// <param name="evnt">The event to register.</param>
        public static void RegisterEvent(string evnt)
        {
            System.Console.WriteLine(evnt);
            _events.Enqueue(evnt);
        }


        /// <summary>
        /// Verifies the specified expected events occurred and that they occurred in the specified order.
        /// </summary>
        /// <param name="expectedEvents">The expected events.</param>
        public static void Verify(params string[] expectedEvents)
        {
            foreach (string expected in expectedEvents)
            {
                if (_events.Count == 0)
                    throw new AssertionException(string.Format("Not enough events occurred.\n\tThe next expected event was: \"{0}\"", expected));
                string actual = _events.Dequeue() as string;
                if (expected != actual)
                    throw new AssertionException(string.Format("Actual event doesn't match expected event.\n\texpected:{0}\n\tactual:{1}", expected, actual));
            }
        }

        /// <summary>
        /// Clears any unverified events.
        /// </summary>
        public static void Clear()
        {
            _events.Clear();
        }
    }
    #endregion
}

namespace NUnit.Core.Tests.SetupFixture.Mocks.Namespace1
{
    #region SomeTestFixture
    [TestFixture]
	public class SomeTestFixture
	{
        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            EventRegistrar.RegisterEvent("FixtureSetup");
        }

        [SetUp]
        public void Setup()
        {
            EventRegistrar.RegisterEvent("Setup");
        }

        [Test]
        public void Test()
        {
            EventRegistrar.RegisterEvent("Test");
        }

        [TearDown]
        public void TearDown()
        {
            EventRegistrar.RegisterEvent("TearDown");
        }
        
        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            EventRegistrar.RegisterEvent("FixtureTearDown");
        }
	}
#endregion SomeTestFixture

    [SetUpFixture]
    public class NUnitNamespaceSetUpFixture
    {
        [TestFixtureSetUp]
        public void DoNamespaceSetUp()
        {
            EventRegistrar.RegisterEvent("NamespaceSetup");
        }

        [TestFixtureTearDown]
        public void DoNamespaceTearDown()
        {
            EventRegistrar.RegisterEvent("NamespaceTearDown");
        }
    }
}

namespace NUnit.Core.Tests.SetupFixture.Mocks.Namespace2
{

    #region SomeTestFixture
    /// <summary>
    /// Summary description for SetUpFixtureTests.
    /// </summary>
    [TestFixture]
    public class SomeTestFixture
    {


        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            EventRegistrar.RegisterEvent("FixtureSetup");
        }

        [SetUp]
        public void Setup()
        {
            EventRegistrar.RegisterEvent("Setup");
        }

        [Test]
        public void Test()
        {
            EventRegistrar.RegisterEvent("Test");
        }

        [TearDown]
        public void TearDown()
        {
            EventRegistrar.RegisterEvent("TearDown");
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            EventRegistrar.RegisterEvent("FixtureTearDown");
        }
    }
    #endregion SomeTestFixture

    #region SomeTestFixture2
    [TestFixture]
    public class SomeTestFixture2
    {


        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            EventRegistrar.RegisterEvent("FixtureSetup");
        }

        [SetUp]
        public void Setup()
        {
            EventRegistrar.RegisterEvent("Setup");
        }

        [Test]
        public void Test()
        {
            EventRegistrar.RegisterEvent("Test");
        }

        [TearDown]
        public void TearDown()
        {
            EventRegistrar.RegisterEvent("TearDown");
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            EventRegistrar.RegisterEvent("FixtureTearDown");
        }
    }
    #endregion SomeTestFixture2

    [SetUpFixture]
    public class NUnitNamespaceSetUpFixture
    {
        [TestFixtureSetUp]
        public void DoNamespaceSetUp()
        {
            EventRegistrar.RegisterEvent("NamespaceSetup");
        }

        [TestFixtureTearDown]
        public void DoNamespaceTearDown()
        {
            EventRegistrar.RegisterEvent("NamespaceTearDown");
        }
    }
}

namespace NUnit.Core.Tests.SetupFixture.Mocks.Namespace3
{
    namespace SubNamespace
    {


        #region SomeTestFixture
        [TestFixture]
        public class SomeTestFixture
        {
            [TestFixtureSetUp]
            public void FixtureSetup()
            {
                EventRegistrar.RegisterEvent("FixtureSetup");
            }

            [SetUp]
            public void Setup()
            {
                EventRegistrar.RegisterEvent("Setup");
            }

            [Test]
            public void Test()
            {
                EventRegistrar.RegisterEvent("Test");
            }

            [TearDown]
            public void TearDown()
            {
                EventRegistrar.RegisterEvent("TearDown");
            }

            [TestFixtureTearDown]
            public void FixtureTearDown()
            {
                EventRegistrar.RegisterEvent("FixtureTearDown");
            }
        }
        #endregion SomeTestFixture

        [SetUpFixture]
        public class NUnitNamespaceSetUpFixture
        {
            [TestFixtureSetUp]
            public void DoNamespaceSetUp()
            {
                EventRegistrar.RegisterEvent("SubNamespaceSetup");
            }

            [TestFixtureTearDown]
            public void DoNamespaceTearDown()
            {
                EventRegistrar.RegisterEvent("SubNamespaceTearDown");
            }
        }

    }


    #region SomeTestFixture
    [TestFixture]
    public class SomeTestFixture
    {
        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            EventRegistrar.RegisterEvent("FixtureSetup");
        }

        [SetUp]
        public void Setup()
        {
            EventRegistrar.RegisterEvent("Setup");
        }

        [Test]
        public void Test()
        {
            EventRegistrar.RegisterEvent("Test");
        }

        [TearDown]
        public void TearDown()
        {
            EventRegistrar.RegisterEvent("TearDown");
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            EventRegistrar.RegisterEvent("FixtureTearDown");
        }
    }
    #endregion SomeTestFixture

    [SetUpFixture]
    public class NUnitNamespaceSetUpFixture
    {
        [TestFixtureSetUp]
        public void DoNamespaceSetUp()
        {
            EventRegistrar.RegisterEvent("NamespaceSetup");
        }

        [TestFixtureTearDown]
        public void DoNamespaceTearDown()
        {
            EventRegistrar.RegisterEvent("NamespaceTearDown");
        }
    }
}

namespace NUnit.Core.Tests.SetupFixture.Mocks.Namespace4
{
    #region SomeTestFixture
    [TestFixture]
    public class SomeTestFixture
    {
        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            EventRegistrar.RegisterEvent("FixtureSetup");
        }

        [SetUp]
        public void Setup()
        {
            EventRegistrar.RegisterEvent("Setup");
        }

        [Test]
        public void Test()
        {
            EventRegistrar.RegisterEvent("Test");
        }

        [TearDown]
        public void TearDown()
        {
            EventRegistrar.RegisterEvent("TearDown");
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            EventRegistrar.RegisterEvent("FixtureTearDown");
        }
    }
    #endregion SomeTestFixture

    [SetUpFixture]
    public class NUnitNamespaceSetUpFixture
    {
        [TestFixtureSetUp]
        public void DoNamespaceSetUp()
        {
            EventRegistrar.RegisterEvent("NamespaceSetup");
        }

        [TestFixtureTearDown]
        public void DoNamespaceTearDown()
        {
            EventRegistrar.RegisterEvent("NamespaceTearDown");
        }
    }

    [SetUpFixture]
    public class NUnitNamespaceSetUpFixture2
    {
        [TestFixtureSetUp]
        public void DoNamespaceSetUp()
        {
            EventRegistrar.RegisterEvent("NamespaceSetup2");
        }

        [TestFixtureTearDown]
        public void DoNamespaceTearDown()
        {
            EventRegistrar.RegisterEvent("NamespaceTearDown2");
        }
    }
}

#region NoNamespaceSetupFixture
//[SetUpFixture]
public class NoNamespaceSetupFixture
{
    [TestFixtureSetUp]
    public void DoNamespaceSetUp()
    {
        NUnit.Core.Tests.SetupFixture.EventRegistrar.RegisterEvent("RootNamespaceSetup");
    }

    [TestFixtureTearDown]
    public void DoNamespaceTearDown()
    {
        NUnit.Core.Tests.SetupFixture.EventRegistrar.RegisterEvent("RootNamespaceTearDown");
    }
}

//[TestFixture]
public class SomeTestFixture
{
    [Test]
    public void Test()
    {
        NUnit.Core.Tests.SetupFixture.EventRegistrar.RegisterEvent("Test");
    }
}
#endregion NoNamespaceSetupFixture


