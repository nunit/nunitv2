/********************************************************************************************************************
'
' Copyright (c) 2002, James Newkirk, Michael C. Two, Alexei Vorontsov, Philip Craig
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
' THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
'
'*******************************************************************************************************************/
using System;
using System.Reflection;
using NUnit.Framework;
using NUnit.Core;

namespace NUnit.Tests
{
	/// <summary>
	/// Summary description for AttributeTests.
	/// </summary>
	/// 
	[TestFixture]
	public class TestFixtureBuilderTests
	{
		private string testsDll = "NUnit.Tests.dll";

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
			ConstructorInfo ctor = typeof(NUnit.Tests.TestFixtureBuilderTests.AssemblyType).GetConstructor(Type.EmptyTypes);
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
			TestSuiteBuilder builder = new TestSuiteBuilder();
			object fixture = builder.BuildTestFixture(typeof(NoDefaultCtorFixture));
		}

		[Test]
		public void BuildTestSuiteWithBadFixture()
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			TestSuite suite = builder.MakeSuiteFromTestFixtureType(typeof(NoDefaultCtorFixture));
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
			TestSuiteBuilder builder = new TestSuiteBuilder();
			object fixture = builder.BuildTestFixture(typeof(MultipleSetUpAttributes));
		}

		[Test] 
		[ExpectedException(typeof(InvalidTestFixtureException))]
		public void CheckMultipleTearDown()
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			object fixture = builder.BuildTestFixture(typeof(MultipleTearDownAttributes));
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
			TestSuiteBuilder builder = new TestSuiteBuilder();
			TestSuite suite = builder.Build("NUnit.Tests.TestFixtureBuilderTests+IgnoredFixture", testsDll);
			
			suite = (TestSuite)suite.Tests[0];
			
			Assertion.AssertNotNull(suite);
			Assertion.Assert("Suite should not be runnable", !suite.ShouldRun);
			Assertion.AssertEquals("testing ignore a suite", suite.IgnoreReason);

			NUnit.Core.TestCase testCase = (NUnit.Core.TestCase)suite.Tests[0];
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
			TestSuite fixture = LoadFixture("NUnit.Tests.TestFixtureBuilderTests+SignatureTestFixture");
			NUnit.Core.TestCase foundTest = FindTestByName(fixture, methodName);
			Assertion.AssertNotNull(foundTest);
			Assertion.Assert(foundTest.ShouldRun);
		}

		private void InvalidSignatureTest(string methodName)
		{
			TestSuite fixture = LoadFixture("NUnit.Tests.TestFixtureBuilderTests+SignatureTestFixture");
			NUnit.Core.TestCase foundTest = FindTestByName(fixture, methodName);
			Assertion.AssertNotNull(foundTest);
			Assertion.Assert(!foundTest.ShouldRun);
			string expected = String.Format("Method: {0}'s signature is not correct", methodName);
			Assertion.AssertEquals(expected, foundTest.IgnoreReason);
		}

		private TestSuite LoadFixture(string fixtureName)
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			TestSuite suite = builder.Build(fixtureName, testsDll);
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
			TestSuiteBuilder builder = new TestSuiteBuilder();
			TestSuite suite = builder.Build("NUnit.Tests.TestFixtureBuilderTests+AbstractTestFixture", testsDll);
			Assertion.AssertNull(suite);
		}

		private NUnit.Core.TestCase FindTestByName(TestSuite fixture, string methodName)
		{
			NUnit.Core.TestCase foundTest = null;
			foreach(Test test in fixture.Tests)
			{
				NUnit.Core.TestCase testCase = test as NUnit.Core.TestCase;
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
