using System;
using System.Reflection;
using Nunit.Framework;
using Nunit.Core;

namespace Nunit.Tests
{
	/// <summary>
	/// Summary description for AttributeTests.
	/// </summary>
	/// 
	[TestFixture]
	public class TestFixtureBuilderTests
	{
		private string testsDll = "Nunit.Tests.dll";

		class AssemblyType
		{
			internal bool called;

			public AssemblyType()
			{
				called = true;
			}
		}

		[Test]
		public void CallTestFixtureConstructor()
		{
			ConstructorInfo ctor = typeof(Nunit.Tests.TestFixtureBuilderTests.AssemblyType).GetConstructor(Type.EmptyTypes);
			Assertion.Assert(ctor != null);

			object testFixture = ctor.Invoke(Type.EmptyTypes);
			Assertion.Assert(testFixture != null);

			AssemblyType assemblyType = (AssemblyType)testFixture;
			Assertion.Assert("AssemblyType constructor should be called", assemblyType.called);
		}

		[TestFixture]
		internal class NoDefaultCtorFixture
		{
			public NoDefaultCtorFixture(int index)
			{}

			[Test] public void OneTest()
			{}
		}

		[Test] 
		[ExpectedException(typeof(InvalidTestFixtureException))]
		public void BuildTestFixture()
		{
			object fixture = TestSuiteBuilder.BuildTestFixture(typeof(NoDefaultCtorFixture));
		}

		[Test]
		public void BuildTestSuiteWithBadFixture()
		{
			TestSuite suite = TestSuiteBuilder.MakeSuiteFromTestFixtureType(typeof(NoDefaultCtorFixture));
			Assertion.Assert(!suite.ShouldRun);
		}

		[TestFixture]
		private class MultipleSetUpAttributes
		{
			[SetUp]
			public void Init1()
			{}

			[SetUp]
			public void Init2()
			{}

			[Test] public void OneTest()
			{}
		}

		[TestFixture]
		private class MultipleTearDownAttributes
		{
			[TearDown]
			public void Destroy1()
			{}

			[TearDown]
			public void Destroy2()
			{}

			[Test] public void OneTest()
			{}
		}

		[Test] 
		[ExpectedException(typeof(InvalidTestFixtureException))]
		public void CheckMultipleSetUp()
		{
			object fixture = TestSuiteBuilder.BuildTestFixture(typeof(MultipleSetUpAttributes));
		}

		[Test] 
		[ExpectedException(typeof(InvalidTestFixtureException))]
		public void CheckMultipleTearDown()
		{
			object fixture = TestSuiteBuilder.BuildTestFixture(typeof(MultipleTearDownAttributes));
		}

		[TestFixture]
		[Ignore("testing ignore a suite")]
		private class IgnoredFixture
		{
			[Test]
			public void Success()
			{}
		}

		[Test]
		public void TestIgnoredFixture()
		{
			TestSuite suite = TestSuiteBuilder.Build("Nunit.Tests.TestFixtureBuilderTests+IgnoredFixture", testsDll);
			
			suite = (TestSuite)suite.Tests[0];
			
			Assertion.AssertNotNull(suite);
			Assertion.Assert("Suite should not be runnable", !suite.ShouldRun);
			Assertion.AssertEquals("testing ignore a suite", suite.IgnoreReason);

			TestCase testCase = (TestCase)suite.Tests[0];
			Assertion.Assert("test case should inherit run state from enclosing suite", !testCase.ShouldRun);
		}

		[TestFixture]
		internal class SignatureTestFixture
		{
			[Test]
			public int NotVoid() 
			{
				return 1;
			}

			[Test]
			public void Parameters(string test) 
			{}
		
			[Test]
			protected void Protected() 
			{}

			[Test]
			private void Private() 
			{}


			[Test]
			public void TestVoid() 
			{}
		}

		[Test]
		public void TestNonVoidReturn()
		{
			InvalidSignatureTest("NotVoid");
		}

		[Test]
		public void TestNonEmptyParameters()
		{
			InvalidSignatureTest("Parameters");
		}

		[Test]
		public void TestProtected()
		{
			InvalidSignatureTest("Protected");
		}

		[Test]
		public void TestPrivate()
		{
			InvalidSignatureTest("Private");
		}

		[Test]
		public void GoodSignature()
		{
			string methodName = "TestVoid";
			TestSuite fixture = LoadFixture("Nunit.Tests.TestFixtureBuilderTests+SignatureTestFixture");
			TestCase foundTest = FindTestByName(fixture, methodName);
			Assertion.AssertNotNull(foundTest);
			Assertion.Assert(foundTest.ShouldRun);
		}

		private void InvalidSignatureTest(string methodName)
		{
			TestSuite fixture = LoadFixture("Nunit.Tests.TestFixtureBuilderTests+SignatureTestFixture");
			TestCase foundTest = FindTestByName(fixture, methodName);
			Assertion.AssertNotNull(foundTest);
			Assertion.Assert(!foundTest.ShouldRun);
			string expected = String.Format("Method: {0}'s signature is not correct", methodName);
			Assertion.AssertEquals(expected, foundTest.IgnoreReason);
		}

		private TestSuite LoadFixture(string fixtureName)
		{
			TestSuite suite = TestSuiteBuilder.Build(fixtureName, testsDll);
			Assertion.AssertNotNull(suite);

			TestSuite fixture = (TestSuite)suite.Tests[0];
			Assertion.AssertNotNull(fixture);
			return fixture;
		}

		[TestFixture]
		private abstract class AbstractTestFixture
		{
			[TearDown]
			public void Destroy1()
			{}
		}

		[Test]
		public void AbstractFixture()
		{
			TestSuite suite = TestSuiteBuilder.Build("Nunit.Tests.TestFixtureBuilderTests+AbstractTestFixture", testsDll);
			Assertion.AssertNull(suite);
		}

		private TestCase FindTestByName(TestSuite fixture, string methodName)
		{
			TestCase foundTest = null;
			foreach(Test test in fixture.Tests)
			{
				TestCase testCase = test as TestCase;
				if(testCase != null)
				{
					if(testCase.Name.Equals(methodName))
						foundTest = testCase;
				}

				if(foundTest != null)
					break;
			}

			return foundTest;
		}


	}
}
