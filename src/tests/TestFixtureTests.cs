using System;
using NUnit.Framework;
using NUnit.Core;
using NUnit.Tests.TestClasses;

namespace NUnit.Tests.Core
{
	/// <summary>
	/// Tests of the TestFixture class
	/// </summary>
	[TestFixture]
	public class TestFixtureTests
	{
		[Test]
		public void ConstructFromType()
		{
			TestFixture fixture = new TestFixture( typeof( NUnit.Tests.Assemblies.MockTestFixture ) );
			Assert.AreEqual( "MockTestFixture", fixture.Name );
			Assert.AreEqual( "NUnit.Tests.Assemblies.MockTestFixture", fixture.FullName );
		}

		[Test]
		public void ConstructFromTypeWithoutNamespace()
		{
			TestFixture fixture = new TestFixture( typeof( NoNamespaceTestFixture ) );
			Assert.AreEqual( "NoNamespaceTestFixture", fixture.Name );
			Assert.AreEqual( "NoNamespaceTestFixture", fixture.FullName );
		}

		[Test]
		public void ConstructFromNestedType()
		{
			TestFixture fixture = new TestFixture( typeof( OuterClass.NestedTestFixture ) );
			Assert.AreEqual( "OuterClass+NestedTestFixture", fixture.Name );
			Assert.AreEqual( "NUnit.Tests.TestClasses.OuterClass+NestedTestFixture", fixture.FullName );
		}

		[Test]
		public void ConstructFromDoublyNestedType()
		{
			TestFixture fixture = new TestFixture( typeof( OuterClass.NestedTestFixture.DoublyNestedTestFixture ) );
			Assert.AreEqual( "OuterClass+NestedTestFixture+DoublyNestedTestFixture", fixture.Name );
			Assert.AreEqual( "NUnit.Tests.TestClasses.OuterClass+NestedTestFixture+DoublyNestedTestFixture", fixture.FullName );
		}

		private void AssertNotRunnable( Type type )
		{
			TestSuite suite = new TestFixture( type );
			Assert.IsFalse( suite.ShouldRun );
		}

		private void AssertNotRunnable( Type type, string reason )
		{
			TestSuite suite = new TestFixture( type );
			Assert.IsFalse( suite.ShouldRun );
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
			AssertNotRunnable( typeof( IgnoredFixture ), "testing ignore a suite" );
//			TestSuite suite = builder.Build(testsDll, "NUnit.Tests.Core.TestFixtureBuilderTests+IgnoredFixture" );
//			
//			//suite = (TestSuite)suite.Tests[0];
//			
//			Assert.IsNotNull(suite);
//			Assert.IsFalse(suite.ShouldRun, "Suite should not be runnable");
//			Assert.AreEqual("testing ignore a suite", suite.IgnoreReason);
//
//			NUnit.Core.TestCase testCase = (NUnit.Core.TestCase)suite.Tests[0];
//			Assert.IsFalse(testCase.ShouldRun,
//				"test case should inherit run state from enclosing suite");
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
//			TestSuite fixture = LoadFixture("NUnit.Tests.Core.TestFixtureBuilderTests+SignatureTestFixture");
//			NUnit.Core.TestCase foundTest = FindTestByName(fixture, methodName);
//			Assert.IsNotNull(foundTest);
//			Assert.IsTrue(foundTest.ShouldRun);
//		}

		[Test]
		public void CannotRunAbstractFixture()
		{
			AssertNotRunnable( typeof( AbstractTestFixture ) );
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
//			TestSuite fixture = LoadFixture("NUnit.Tests.Core.TestFixtureBuilderTests+HasCategories");
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
