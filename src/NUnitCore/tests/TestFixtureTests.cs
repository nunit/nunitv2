using System;
using NUnit.Framework;
using NUnit.Core;
using NUnit.TestUtilities;
using NUnit.TestData.TestFixtureTests;

namespace NUnit.Core.Tests
{
	/// <summary>
	/// Tests of the NUnitTestFixture class
	/// </summary>
	[TestFixture]
	public class TestFixtureTests
	{
		[Test]
		public void ConstructFromType()
		{
			TestSuite fixture = TestBuilder.MakeFixture( typeof( NUnit.Tests.Assemblies.MockTestFixture ) );
			Assert.AreEqual( "MockTestFixture", fixture.TestName.Name );
			Assert.AreEqual( "NUnit.Tests.Assemblies.MockTestFixture", fixture.TestName.FullName );
		}

		[Test]
		public void ConstructFromTypeWithoutNamespace()
		{
			TestSuite fixture = TestBuilder.MakeFixture( typeof( NoNamespaceTestFixture ) );
			Assert.AreEqual( "NoNamespaceTestFixture", fixture.TestName.Name );
			Assert.AreEqual( "NoNamespaceTestFixture", fixture.TestName.FullName );
		}

		[Test]
		public void ConstructFromNestedType()
		{
			TestSuite fixture = TestBuilder.MakeFixture( typeof( OuterClass.NestedTestFixture ) );
			Assert.AreEqual( "OuterClass+NestedTestFixture", fixture.TestName.Name );
			Assert.AreEqual( "NUnit.TestData.TestFixtureTests.OuterClass+NestedTestFixture", fixture.TestName.FullName );
		}

		[Test]
		public void ConstructFromDoublyNestedType()
		{
			TestSuite fixture = TestBuilder.MakeFixture( typeof( OuterClass.NestedTestFixture.DoublyNestedTestFixture ) );
			Assert.AreEqual( "OuterClass+NestedTestFixture+DoublyNestedTestFixture", fixture.TestName.Name );
			Assert.AreEqual( "NUnit.TestData.TestFixtureTests.OuterClass+NestedTestFixture+DoublyNestedTestFixture", fixture.TestName.FullName );
		}

		private void AssertNotRunnable( Type type )
		{
			TestSuite suite = TestBuilder.MakeFixture( type );
			Assert.AreEqual( RunState.NotRunnable, suite.RunState );
		}

		private void AssertNotRunnable( Type type, string reason )
		{
			TestSuite suite = TestBuilder.MakeFixture( type );
			Assert.AreEqual( RunState.NotRunnable, suite.RunState );
			Assert.AreEqual( reason, suite.IgnoreReason );
		}

		[Test]
		public void CannotRunNoDefaultConstructor()
		{
			AssertNotRunnable( typeof( NoDefaultCtorFixture ) );
		}

		[Test]
		public void CannotRunBadConstructor()
		{
			AssertNotRunnable( typeof( BadCtorFixture ) );
		}

		[Test] 
		public void CannotRunMultipleSetUp()
		{
			AssertNotRunnable(typeof(MultipleSetUpAttributes));
		}

		[Test] 
		public void CannotRunMultipleTearDown()
		{
			AssertNotRunnable(typeof(MultipleTearDownAttributes));
		}

		[Test]
		public void CannotRunIgnoredFixture()
		{
			TestSuite suite = TestBuilder.MakeFixture( typeof( IgnoredFixture ) );
			Assert.AreEqual( RunState.Ignored, suite.RunState );
			Assert.AreEqual( "testing ignore a suite", suite.IgnoreReason );
		}

//		[Test]
//		public void IgnoreStaticTests()
//		{
//			InvalidSignatureTest("Static", "it must be an instance method" );
//		}
//
//		[Test]
//		public void IgnoreTestsThatReturnSomething()
//		{
//			InvalidSignatureTest("NotVoid", "it must return void");
//		}
//
//		[Test]
//		public void IgnoreTestsWithParameters()
//		{
//			InvalidSignatureTest("Parameters", "it must not have parameters");
//		}
//
//		[Test]
//		public void IgnoreProtectedTests()
//		{
//			InvalidSignatureTest("Protected", "it must be a public method");
//		}
//
//		[Test]
//		public void IgnorePrivateTests()
//		{
//			InvalidSignatureTest("Private", "it must be a public method");
//		}
//
//		[Test]
//		public void GoodSignature()
//		{
//			string methodName = "TestVoid";
//			TestSuite fixture = LoadFixture("NUnit.Core.Tests.TestFixtureBuilderTests+SignatureTestFixture");
//			NUnit.Core.TestCase foundTest = FindTestByName(fixture, methodName);
//			Assert.IsNotNull(foundTest);
//			Assert.AreEqual( RunState.Runnable, foundTest.RunState);
//		}

		[Test]
		public void CannotRunAbstractFixture()
		{
			AssertNotRunnable( typeof( AbstractTestFixture ) );
		}

		[Test]
		public void CannotRunAbstractDerivedFixture()
		{
			AssertNotRunnable( typeof( AbstractDerivedTestFixture ) );
		}

		[Test] 
		public void CannotRunMultipleTestFixtureSetUp()
		{
			AssertNotRunnable(typeof(MultipleFixtureSetUpAttributes));
		}

		[Test] 
		public void CannotRunMultipleTestFixtureTearDown()
		{
			AssertNotRunnable(typeof(MultipleFixtureTearDownAttributes));
		}

		[Test] 
		public void CannotRunPrivateSetUp()
		{
			AssertNotRunnable(typeof(PrivateSetUp));
		}

		[Test] 
		public void CannotRunProtectedSetUp()
		{
			AssertNotRunnable(typeof(ProtectedSetUp));
		}

		[Test] 
		public void CannotRunStaticSetUp()
		{
			AssertNotRunnable(typeof(StaticSetUp));
		}

		[Test]
		public void CannotRunSetupWithReturnValue()
		{
			AssertNotRunnable(typeof(SetUpWithReturnValue));
		}

		[Test]
		public void CannotRunSetupWithParameters()
		{
			AssertNotRunnable(typeof(SetUpWithParameters));
		}

		[Test] 
		public void CannotRunPrivateTearDown()
		{
			AssertNotRunnable(typeof(PrivateTearDown));
		}

		[Test] 
		public void CannotRunProtectedTearDown()
		{
			AssertNotRunnable(typeof(ProtectedTearDown));
		}

		[Test] 
		public void CannotRunStaticTearDown()
		{
			AssertNotRunnable(typeof(StaticTearDown));
		}

		[Test]
		public void CannotRunTearDownWithReturnValue()
		{
			AssertNotRunnable(typeof(TearDownWithReturnValue));
		}

		[Test]
		public void CannotRunTearDownWithParameters()
		{
			AssertNotRunnable(typeof(TearDownWithParameters));
		}

		[Test] 
		public void CannotRunPrivateFixtureSetUp()
		{
			AssertNotRunnable(typeof(PrivateFixtureSetUp));
		}

		[Test] 
		public void CannotRunProtectedFixtureSetUp()
		{
			AssertNotRunnable(typeof(ProtectedFixtureSetUp));
		}

		[Test] 
		public void CannotRunStaticFixtureSetUp()
		{
			AssertNotRunnable(typeof(StaticFixtureSetUp));
		}

		[Test]
		public void CannotRunFixtureSetupWithReturnValue()
		{
			AssertNotRunnable(typeof(FixtureSetUpWithReturnValue));
		}

		[Test]
		public void CannotRunFixtureSetupWithParameters()
		{
			AssertNotRunnable(typeof(FixtureSetUpWithParameters));
		}

		[Test] 
		public void CannotRunPrivateFixtureTearDown()
		{
			AssertNotRunnable(typeof(PrivateFixtureTearDown));
		}

		[Test] 
		public void CannotRunProtectedFixtureTearDown()
		{
			AssertNotRunnable(typeof(ProtectedFixtureTearDown));
		}

		[Test] 
		public void CannotRunStaticFixtureTearDown()
		{
			AssertNotRunnable(typeof(StaticFixtureTearDown));
		}

//		[TestFixture]
//			[Category("fixture category")]
//			[Category("second")]
//			private class HasCategories 
//		{
//			[Test] public void OneTest()
//			{}
//		}
//
//		[Test]
//		public void LoadCategories() 
//		{
//			TestSuite fixture = LoadFixture("NUnit.Core.Tests.TestFixtureBuilderTests+HasCategories");
//			Assert.IsNotNull(fixture);
//			Assert.AreEqual(2, fixture.Categories.Count);
//		}

		[Test]
		public void CannotRunFixtureTearDownWithReturnValue()
		{
			AssertNotRunnable(typeof(FixtureTearDownWithReturnValue));
		}

		[Test]
		public void CannotRunFixtureTearDownWithParameters()
		{
			AssertNotRunnable(typeof(FixtureTearDownWithParameters));
		}

	}
}
