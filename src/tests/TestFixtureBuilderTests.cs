#region Copyright (c) 2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright  2000-2002 Philip A. Craig
'
' This software is provided 'as-is', without any express or implied warranty. In no 
' event will the authors be held liable for any damages arising from the use of this 
' software.
' 
' Permission is granted to anyone to use this software for any purpose, including 
' commercial applications, and to alter it and redistribute it freely, subject to the 
' following restrictions:
'
' 1. The origin of this software must not be misrepresented; you must not claim that 
' you wrote the original software. If you use this software in a product, an 
' acknowledgment (see the following) in the product documentation is required.
'
' Portions Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright  2000-2002 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

using System;
using System.Reflection;
using NUnit.Framework;
using NUnit.Core;

namespace NUnit.Tests.Core
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
			ConstructorInfo ctor = typeof(NUnit.Tests.Core.TestFixtureBuilderTests.AssemblyType).GetConstructor(Type.EmptyTypes);
			Assert.IsNotNull(ctor);

			object testFixture = ctor.Invoke(Type.EmptyTypes);
			Assert.IsNotNull(testFixture);

			AssemblyType assemblyType = (AssemblyType)testFixture;
			Assert.IsTrue(assemblyType.called, "AssemblyType constructor should be called");
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
		public void BuildTestFixtureWithNoDefaultCtor()
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			object fixture = builder.BuildTestFixture(typeof(NoDefaultCtorFixture));
		}

		[Test]
		public void BuildTestSuiteWithNoDefaultCtorFixture()
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			TestSuite suite = builder.MakeSuiteFromTestFixtureType(typeof(NoDefaultCtorFixture));
			Assert.IsFalse(suite.ShouldRun);
		}

		[TestFixture]
			internal class BadCtorFixture
		{
			BadCtorFixture()
			{
				throw new Exception();
			}

			[Test] public void OneTest()
			{}
		}

		[Test]
		[ExpectedException(typeof(InvalidTestFixtureException))]
		public void BuildFixtureWithBadCtor()
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			object fixture = builder.BuildTestFixture(typeof(BadCtorFixture));
		}

		[Test]
		public void BuildTestSuiteWithBadCtorFixture()
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			TestSuite suite = builder.MakeSuiteFromTestFixtureType(typeof(BadCtorFixture));
			Assert.IsFalse(suite.ShouldRun);
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
			TestSuite suite = builder.Build(testsDll, "NUnit.Tests.Core.TestFixtureBuilderTests+IgnoredFixture" );
			
			suite = (TestSuite)suite.Tests[0];
			
			Assert.IsNotNull(suite);
			Assert.IsFalse(suite.ShouldRun, "Suite should not be runnable");
			Assert.AreEqual("testing ignore a suite", suite.IgnoreReason);

			NUnit.Core.TestCase testCase = (NUnit.Core.TestCase)suite.Tests[0];
			Assert.IsFalse(testCase.ShouldRun,
				"test case should inherit run state from enclosing suite");
		}

		[TestFixture]
		internal class SignatureTestFixture
		{
			[Test]
			public static void Static()
			{
			}

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

		private class NestedTestFixture
		{
			[TestFixture]
			internal class DoubleNestedTestFixture
			{
				[Test]
				public void Test()
				{
				}
			}
		}

		[Test]
		public void TestStatic()
		{
			InvalidSignatureTest("Static", "it must be an instance method" );
		}

		[Test]
		public void TestNonVoidReturn()
		{
			InvalidSignatureTest("NotVoid", "it must return void");
		}

		[Test]
		public void TestNonEmptyParameters()
		{
			InvalidSignatureTest("Parameters", "it must not have parameters");
		}

		[Test]
		public void TestProtected()
		{
			InvalidSignatureTest("Protected", "it must be a public method");
		}

		[Test]
		public void TestPrivate()
		{
			InvalidSignatureTest("Private", "it must be a public method");
		}

		[Test]
		public void GoodSignature()
		{
			string methodName = "TestVoid";
			TestSuite fixture = LoadFixture("NUnit.Tests.Core.TestFixtureBuilderTests+SignatureTestFixture");
			NUnit.Core.TestCase foundTest = FindTestByName(fixture, methodName);
			Assert.IsNotNull(foundTest);
			Assert.IsTrue(foundTest.ShouldRun);
		}

		[Test]
		public void FixtureName()
		{
			TestSuite fixture = LoadFixture("NUnit.Tests.Core.TestFixtureBuilderTests");
			Assert.AreEqual( "TestFixtureBuilderTests", fixture.Name );
		}

		[Test]
		public void FixtureName_Nested()
		{
			TestSuite fixture = LoadFixture("NUnit.Tests.Core.TestFixtureBuilderTests+SignatureTestFixture");
			Assert.AreEqual( "TestFixtureBuilderTests+SignatureTestFixture", fixture.Name );
		}

		[Test]
		public void FixtureName_NestedTwice()
		{
			TestSuite fixture = LoadFixture("NUnit.Tests.Core.TestFixtureBuilderTests+NestedTestFixture+DoubleNestedTestFixture");
			Assert.AreEqual( "TestFixtureBuilderTests+NestedTestFixture+DoubleNestedTestFixture", fixture.Name );
		}

		private void InvalidSignatureTest(string methodName, string reason)
		{
			TestSuite fixture = LoadFixture("NUnit.Tests.Core.TestFixtureBuilderTests+SignatureTestFixture");
			NUnit.Core.TestCase foundTest = FindTestByName(fixture, methodName);
			Assert.IsNotNull(foundTest);
			Assert.IsFalse(foundTest.ShouldRun);
			string expected = String.Format("Method {0}'s signature is not correct: {1}.", methodName, reason);
			Assert.AreEqual(expected, foundTest.IgnoreReason);
		}

		private TestSuite LoadFixture(string fixtureName)
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			TestSuite suite = builder.Build(testsDll, fixtureName );
			Assert.IsNotNull(suite);

			TestSuite fixture = (TestSuite)suite.Tests[0];
			Assert.IsNotNull(fixture);
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
			TestSuite suite = builder.Build(testsDll, "NUnit.Tests.Core.TestFixtureBuilderTests+AbstractTestFixture" );
			Assert.IsNull(suite);
		}


		[TestFixture]
		private class MultipleFixtureSetUpAttributes
		{
			[TestFixtureSetUp]
			public void Init1()
			{}

			[TestFixtureSetUp]
			public void Init2()
			{}

			[Test] public void OneTest()
			{}
		}

		[TestFixture]
		private class MultipleFixtureTearDownAttributes
		{
			[TestFixtureTearDown]
			public void Destroy1()
			{}

			[TestFixtureTearDown]
			public void Destroy2()
			{}

			[Test] public void OneTest()
			{}
		}

		[Test] 
		[ExpectedException(typeof(InvalidTestFixtureException))]
		public void CheckMultipleTestFixtureSetUp()
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			object fixture = builder.BuildTestFixture(typeof(MultipleFixtureSetUpAttributes));
		}

		[Test] 
		[ExpectedException(typeof(InvalidTestFixtureException))]
		public void CheckMultipleTestFixtureTearDown()
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			object fixture = builder.BuildTestFixture(typeof(MultipleFixtureTearDownAttributes));
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
