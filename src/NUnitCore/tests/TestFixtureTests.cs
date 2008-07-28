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

        [Test]
        public void ConstructFromTypeWithoutTestFixtureAttribute()
        {
			TestSuite fixture = TestBuilder.MakeFixture( typeof( FixtureWithoutTestFixtureAttribute ) );
			Assert.AreEqual( "FixtureWithoutTestFixtureAttribute", fixture.TestName.Name );
			Assert.AreEqual( "NUnit.TestData.TestFixtureTests.FixtureWithoutTestFixtureAttribute", fixture.TestName.FullName );
        }

		private void AssertRunnable( Type type )
		{
			TestSuite suite = TestBuilder.MakeFixture( type );
			Assert.AreEqual( RunState.Runnable, suite.RunState );
            TestResult result = suite.Run(NullListener.NULL, TestFilter.Empty);
			Assert.AreEqual( ResultState.Success, result.ResultState );
		}

		private void AssertNotRunnable( Type type )
		{
			TestSuite suite = TestBuilder.MakeFixture( type );
			Assert.AreEqual( RunState.NotRunnable, suite.RunState );
            TestResult result = suite.Run(NullListener.NULL, TestFilter.Empty);
			Assert.AreEqual( ResultState.NotRunnable, result.ResultState );
		}

		private void AssertNotRunnable( Type type, string reason )
		{
			TestSuite suite = TestBuilder.MakeFixture( type );
			Assert.AreEqual( RunState.NotRunnable, suite.RunState );
			Assert.AreEqual( reason, suite.IgnoreReason );
            TestResult result = suite.Run(NullListener.NULL, TestFilter.Empty);
			Assert.AreEqual( ResultState.NotRunnable, result.ResultState );
			Assert.AreEqual( reason, result.Message );
		}

        [Test]
        public void CannotRunConstructorWithArgsNotSupplied()
        {
            AssertNotRunnable(typeof(NoDefaultCtorFixture));
        }

        [Test]
        public void CanRunConstructorWithArgsSupplied()
        {
            AssertRunnable(typeof(FixtureWithArgsSupplied));
        }

        [Test]
		public void CannotRunBadConstructor()
		{
			AssertNotRunnable( typeof( BadCtorFixture ) );
		}

		[Test] 
		public void CanRunMultipleSetUp()
		{
			AssertRunnable(typeof(MultipleSetUpAttributes));
		}

		[Test] 
		public void CanRunMultipleTearDown()
		{
			AssertRunnable(typeof(MultipleTearDownAttributes));
		}

		[Test]
		public void CannotRunIgnoredFixture()
		{
			TestSuite suite = TestBuilder.MakeFixture( typeof( IgnoredFixture ) );
			Assert.AreEqual( RunState.Ignored, suite.RunState );
			Assert.AreEqual( "testing ignore a fixture", suite.IgnoreReason );
		}

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
		public void CanRunMultipleTestFixtureSetUp()
		{
			AssertRunnable(typeof(MultipleFixtureSetUpAttributes));
		}

		[Test] 
		public void CanRunMultipleTestFixtureTearDown()
		{
			AssertRunnable(typeof(MultipleFixtureTearDownAttributes));
		}

		#region SetUp Signature
		[Test] 
		public void CannotRunPrivateSetUp()
		{
			AssertNotRunnable(typeof(PrivateSetUp));
		}

		[Test] 
		public void CanRunProtectedSetUp()
		{
			AssertRunnable(typeof(ProtectedSetUp));
		}

        /// <summary>
        /// Determines whether this instance [can run static set up].
        /// </summary>
		[Test] 
		public void CanRunStaticSetUp()
		{
			AssertRunnable(typeof(StaticSetUp));
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
		#endregion

		#region TearDown Signature
		[Test] 
		public void CannotRunPrivateTearDown()
		{
			AssertNotRunnable(typeof(PrivateTearDown));
		}

		[Test] 
		public void CanRunProtectedTearDown()
		{
			AssertRunnable(typeof(ProtectedTearDown));
		}

		[Test] 
		public void CanRunStaticTearDown()
		{
			AssertRunnable(typeof(StaticTearDown));
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
		#endregion

		#region TestFixtureSetUp Signature
		[Test] 
		public void CannotRunPrivateFixtureSetUp()
		{
			AssertNotRunnable(typeof(PrivateFixtureSetUp));
		}

		[Test] 
		public void CanRunProtectedFixtureSetUp()
		{
			AssertRunnable(typeof(ProtectedFixtureSetUp));
		}

		[Test] 
		public void CanRunStaticFixtureSetUp()
		{
			AssertRunnable(typeof(StaticFixtureSetUp));
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
		#endregion

		#region TestFixtureTearDown Signature
		[Test] 
		public void CannotRunPrivateFixtureTearDown()
		{
			AssertNotRunnable(typeof(PrivateFixtureTearDown));
		}

		[Test] 
		public void CanRunProtectedFixtureTearDown()
		{
			AssertRunnable(typeof(ProtectedFixtureTearDown));
		}

		[Test] 
		public void CanRunStaticFixtureTearDown()
		{
			AssertRunnable(typeof(StaticFixtureTearDown));
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
		#endregion
	}
}
