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
		TestSuiteBuilder builder = new TestSuiteBuilder();

		#region Private & Internal Classes Used by Tests

		class AssemblyType
		{
			internal bool called;

			public AssemblyType()
			{
				called = true;
			}
		}

		[TestFixture]
			internal class NoDefaultCtorFixture
		{
			public NoDefaultCtorFixture(int index)
			{}

			[Test] public void OneTest()
			{}
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

		[TestFixture]
			[Ignore("testing ignore a suite")]
			private class IgnoredFixture
		{
			[Test]
			public void Success()
			{}
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

		[TestFixture]
		private abstract class AbstractTestFixture
		{
			[TearDown]
			public void Destroy1()
			{}
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

		[TestFixture]
		private class PrivateSetUp
		{
			[SetUp]
			private void Setup()	{}
		}

		[TestFixture]
		private class ProtectedSetUp
		{
			[SetUp]
			protected void Setup()	{}
		}

		[TestFixture]
		private class SetUpWithReturnValue
		{
			[SetUp]
			public int Setup() { return 0; }
		}

		[TestFixture]
		private class SetUpWithParameters
		{
			[SetUp]
			public void Setup(int j) { }
		}

		[TestFixture]
		private class PrivateTearDown
		{
			[TearDown]
			private void Teardown()	{}
		}

		[TestFixture]
		private class ProtectedTearDown
		{
			[TearDown]
			protected void Teardown()	{}
		}

		[TestFixture]
		private class TearDownWithReturnValue
		{
			[TearDown]
			public int Teardown() { return 0; }
		}

		[TestFixture]
		private class TearDownWithParameters
		{
			[TearDown]
			public void Teardown(int j) { }
		}

		[TestFixture]
		private class PrivateFixtureSetUp
		{
			[TestFixtureSetUp]
			private void Setup()	{}
		}

		[TestFixture]
		private class ProtectedFixtureSetUp
		{
			[TestFixtureSetUp]
			protected void Setup()	{}
		}

		[TestFixture]
		private class FixtureSetUpWithReturnValue
		{
			[TestFixtureSetUp]
			public int Setup() { return 0; }
		}

		[TestFixture]
		private class FixtureSetUpWithParameters
		{
			[SetUp]
			public void Setup(int j) { }
		}

		[TestFixture]
		private class PrivateFixtureTearDown
		{
			[TestFixtureTearDown]
			private void Teardown()	{}
		}

		[TestFixture]
		private class ProtectedFixtureTearDown
		{
			[TestFixtureTearDown]
			protected void Teardown()	{}
		}

		[TestFixture]
		private class FixtureTearDownWithReturnValue
		{
			[TestFixtureTearDown]
			public int Teardown() { return 0; }
		}

		[TestFixture]
		private class FixtureTearDownWithParameters
		{
			[TestFixtureTearDown]
			public void Teardown(int j) { }
		}

		#endregion

		#region Helper Methods

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
			TestSuite suite = builder.Build(testsDll, fixtureName );
			Assert.IsNotNull(suite);

//			TestSuite fixture = (TestSuite)suite.Tests[0];
//			Assert.IsNotNull(fixture);
//			return fixture;
			return suite;
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

		#endregion

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

		[Test] 
		[ExpectedException(typeof(InvalidTestFixtureException))]
		public void BuildTestFixtureWithNoDefaultCtor()
		{
			builder.BuildTestFixture(typeof(NoDefaultCtorFixture));
		}

		[Test]
		public void BuildTestSuiteWithNoDefaultCtorFixture()
		{
			TestSuite suite = builder.MakeSuiteFromTestFixtureType(typeof(NoDefaultCtorFixture));
			Assert.IsFalse(suite.ShouldRun);
		}

		[Test]
		[ExpectedException(typeof(InvalidTestFixtureException))]
		public void BuildFixtureWithBadCtor()
		{
			builder.BuildTestFixture(typeof(BadCtorFixture));
		}

		[Test]
		public void BuildTestSuiteWithBadCtorFixture()
		{
			TestSuite suite = builder.MakeSuiteFromTestFixtureType(typeof(BadCtorFixture));
			Assert.IsFalse(suite.ShouldRun);
		}

		[Test] 
		[ExpectedException(typeof(InvalidTestFixtureException))]
		public void CheckMultipleSetUp()
		{
			builder.BuildTestFixture(typeof(MultipleSetUpAttributes));
		}

		[Test] 
		[ExpectedException(typeof(InvalidTestFixtureException))]
		public void CheckMultipleTearDown()
		{
			builder.BuildTestFixture(typeof(MultipleTearDownAttributes));
		}

		[Test]
		public void TestIgnoredFixture()
		{
			TestSuite suite = builder.Build(testsDll, "NUnit.Tests.Core.TestFixtureBuilderTests+IgnoredFixture" );
			
			//suite = (TestSuite)suite.Tests[0];
			
			Assert.IsNotNull(suite);
			Assert.IsFalse(suite.ShouldRun, "Suite should not be runnable");
			Assert.AreEqual("testing ignore a suite", suite.IgnoreReason);

			NUnit.Core.TestCase testCase = (NUnit.Core.TestCase)suite.Tests[0];
			Assert.IsFalse(testCase.ShouldRun,
				"test case should inherit run state from enclosing suite");
		}

		[Test]
		public void IgnoreStaticTests()
		{
			InvalidSignatureTest("Static", "it must be an instance method" );
		}

		[Test]
		public void IgnoreTestsThatReturnSomething()
		{
			InvalidSignatureTest("NotVoid", "it must return void");
		}

		[Test]
		public void IgnoreTestsWithParameters()
		{
			InvalidSignatureTest("Parameters", "it must not have parameters");
		}

		[Test]
		public void IgnoreProtectedTests()
		{
			InvalidSignatureTest("Protected", "it must be a public method");
		}

		[Test]
		public void IgnorePrivateTests()
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

		[Test]
		public void AbstractFixture()
		{
			TestSuite suite = builder.Build(testsDll, "NUnit.Tests.Core.TestFixtureBuilderTests+AbstractTestFixture" );
			Assert.IsNull(suite);
		}


		[Test] 
		[ExpectedException(typeof(InvalidTestFixtureException))]
		public void CheckMultipleTestFixtureSetUp()
		{
			builder.BuildTestFixture(typeof(MultipleFixtureSetUpAttributes));
		}

		[Test] 
		[ExpectedException(typeof(InvalidTestFixtureException))]
		public void CheckMultipleTestFixtureTearDown()
		{
			builder.BuildTestFixture(typeof(MultipleFixtureTearDownAttributes));
		}

		[Test] 
		[ExpectedException(typeof(InvalidTestFixtureException))]
		public void CheckPrivateSetUp()
		{
			builder.BuildTestFixture(typeof(PrivateSetUp));
		}

		[Test] 
		[ExpectedException(typeof(InvalidTestFixtureException))]
		public void CheckProtectedSetUp()
		{
			builder.BuildTestFixture(typeof(ProtectedSetUp));
		}

		[Test]
		[ExpectedException(typeof(InvalidTestFixtureException))]
		public void CheckSetupWithReturnValue()
		{
			builder.BuildTestFixture(typeof(SetUpWithReturnValue));
		}

		[Test]
		[ExpectedException(typeof(InvalidTestFixtureException))]
		public void CheckSetupWithParameters()
		{
			builder.BuildTestFixture(typeof(SetUpWithParameters));
		}

		[Test] 
		[ExpectedException(typeof(InvalidTestFixtureException))]
		public void CheckPrivateTearDown()
		{
			builder.BuildTestFixture(typeof(PrivateTearDown));
		}

		[Test] 
		[ExpectedException(typeof(InvalidTestFixtureException))]
		public void CheckProtectedTearDown()
		{
			builder.BuildTestFixture(typeof(ProtectedTearDown));
		}

		[Test]
		[ExpectedException(typeof(InvalidTestFixtureException))]
		public void CheckTearDownWithReturnValue()
		{
			builder.BuildTestFixture(typeof(TearDownWithReturnValue));
		}

		[Test]
		[ExpectedException(typeof(InvalidTestFixtureException))]
		public void CheckTearDownWithParameters()
		{
			builder.BuildTestFixture(typeof(TearDownWithParameters));
		}

		[Test] 
		[ExpectedException(typeof(InvalidTestFixtureException))]
		public void CheckPrivateFixtureSetUp()
		{
			builder.BuildTestFixture(typeof(PrivateFixtureSetUp));
		}

		[Test] 
		[ExpectedException(typeof(InvalidTestFixtureException))]
		public void CheckProtectedFixtureSetUp()
		{
			builder.BuildTestFixture(typeof(ProtectedFixtureSetUp));
		}

		[Test]
		[ExpectedException(typeof(InvalidTestFixtureException))]
		public void CheckFixtureSetupWithReturnValue()
		{
			builder.BuildTestFixture(typeof(FixtureSetUpWithReturnValue));
		}

		[Test]
		[ExpectedException(typeof(InvalidTestFixtureException))]
		public void CheckFixtureSetupWithParameters()
		{
			builder.BuildTestFixture(typeof(FixtureSetUpWithParameters));
		}

		[Test] 
		[ExpectedException(typeof(InvalidTestFixtureException))]
		public void CheckPrivateFixtureTearDown()
		{
			builder.BuildTestFixture(typeof(PrivateFixtureTearDown));
		}

		[Test] 
		[ExpectedException(typeof(InvalidTestFixtureException))]
		public void CheckProtectedFixtureTearDown()
		{
			builder.BuildTestFixture(typeof(ProtectedFixtureTearDown));
		}

		[Test]
		[ExpectedException(typeof(InvalidTestFixtureException))]
		public void CheckFixtureTearDownWithReturnValue()
		{
			builder.BuildTestFixture(typeof(FixtureTearDownWithReturnValue));
		}

		[Test]
		[ExpectedException(typeof(InvalidTestFixtureException))]
		public void CheckFixtureTearDownWithParameters()
		{
			builder.BuildTestFixture(typeof(FixtureTearDownWithParameters));
		}

	}
}
