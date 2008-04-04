using NUnit.Framework;
using NUnit.Framework.Syntax.CSharp;
using NUnit.Util;
using NUnit.TestUtilities;
using NUnit.TestData;

namespace NUnit.Core.Tests
{
	[TestFixture]
	public class TestMethodSignatureTests
	{
		private Test fixture;

		[SetUp]
		public void CreateFixture()
		{
			fixture = TestBuilder.MakeFixture( typeof( TestMethodSignatureFixture ) );
		}

        private void AssertRunnable( string name )
        {
            AssertRunnable( name, ResultState.Success );
        }

	    private void AssertRunnable( string name, ResultState resultState )
		{
			Test test = TestFinder.RequiredChildTest( name, fixture );
			Assert.That( test.RunState, Is.EqualTo( RunState.Runnable ) );
			Assert.That( test.TestCount, Is.EqualTo( 1 ) );
			TestResult result = test.Run( NullListener.NULL );
			Assert.That( result.RunState, Is.EqualTo( RunState.Executed ) );
			Assert.That( result.ResultState, Is.EqualTo( resultState ) );
		}

		private void AssertNotRunnable( string name )
		{
			Test test = TestFinder.RequiredChildTest( name, fixture );
			Assert.That( test.RunState, Is.EqualTo( RunState.NotRunnable ) );
			Assert.That( test.TestCount, Is.EqualTo( 1 ) );
			TestResult result = test.Run( NullListener.NULL );
			Assert.That( result.RunState, Is.EqualTo( RunState.NotRunnable ) );
		}

		[Test]
		public void InstanceTestMethodIsRunnable()
		{
			AssertRunnable( "InstanceTestMethod" );
		}

		[Test]
		public void StaticTestMethodIsRunnable()
		{
			AssertRunnable( "StaticTestMethod" );
		}

		[Test]
		public void TestMethodWithoutParametersWithArgumentsProvidedIsNotRunnable()
		{
			AssertNotRunnable("TestMethodWithoutParametersWithArgumentsProvided");
		}

        [Test]
        public void TestMethodWithArgumentsNotProvidedIsNotRunnable()
        {
            AssertNotRunnable("TestMethodWithArgumentsNotProvided");
        }

        [Test]
        public void TestMethodWithArgumentsProvidedIsRunnable()
        {
            AssertRunnable("TestMethodWithArgumentsProvided");
        }

        [Test]
        public void TestMethodWithWrongNumberOfArgumentsProvidedIsNotRunnable()
        {
            AssertNotRunnable("TestMethodWithWrongNumberOfArgumentsProvided");
        }

        [Test]
        public void TestMethodWithWrongArgumentTypesProvidedGivesError()
        {
            AssertRunnable("TestMethodWithWrongArgumentTypesProvided", ResultState.Error);
        }

        [Test]
        public void StaticTestMethodWithArgumentsNotProvidedIsNotRunnable()
        {
            AssertNotRunnable("StaticTestMethodWithArgumentsNotProvided");
        }

        [Test]
        public void StaticTestMethodWithArgumentsProvidedIsRunnable()
        {
            AssertRunnable("StaticTestMethodWithArgumentsProvided");
        }

        [Test]
        public void StaticTestMethodWithWrongNumberOfArgumentsProvidedIsNotRunnable()
        {
            AssertNotRunnable("StaticTestMethodWithWrongNumberOfArgumentsProvided");
        }

        [Test]
        public void StaticTestMethodWithWrongArgumentTypesProvidedGivesError()
        {
            AssertRunnable("StaticTestMethodWithWrongArgumentTypesProvided", ResultState.Error);
        }

        [Test]
        public void TestMethodWithConvertibleArgumentsIsRunnable()
        {
            AssertRunnable("TestMethodWithConvertibleArguments");
        }

	    [Test]
		public void ProtectedTestMethodIsNotRunnable()
		{
			AssertNotRunnable( "ProtectedTestMethod" );
		}

		[Test]
		public void PrivateTestMethodIsNotRunnable()
		{
			AssertNotRunnable( "PrivateTestMethod" );
		}

		[Test]
		public void TestMethodWithReturnTypeIsNotRunnable()
		{
			AssertNotRunnable( "TestMethodWithReturnType" );
		}

		[Test]
		public void TestMethodWithMultipleTestCasesExecutesMultipleTimes()
		{
			Test test = TestFinder.FindChildTest( "TestMethodWithMultipleTestCases", fixture );
			Assert.That( test.RunState, Is.EqualTo( RunState.Runnable ) );
			TestResult result = test.Run( NullListener.NULL );
			Assert.That( result.ResultState, Is.EqualTo(ResultState.Success) );
            ResultSummarizer summary = new ResultSummarizer(result);
		    Assert.That(summary.TestsRun, Is.EqualTo(3));
		}

        [Test]
        public void TestMethodWithMultipleTestCasesUsesCorrectNames()
        {
            string name = "TestMethodWithMultipleTestCases";
            string fullName = typeof (TestMethodSignatureFixture).FullName + "." + name;
            TestSuite suite = (TestSuite)TestFinder.FindChildTest(name, fixture);
            Assert.That(suite.TestCount, Is.EqualTo(3));
            suite.Sort();
            
            Test test = (Test)suite.Tests[0];
            Assert.That(test.TestName.Name, Is.EqualTo(name + "(12,2,6)"));
            Assert.That(test.TestName.FullName, Is.EqualTo(fullName + "(12,2,6)"));

            test = (Test)suite.Tests[1];
            Assert.That(test.TestName.Name, Is.EqualTo(name + "(12,3,4)"));
            Assert.That(test.TestName.FullName, Is.EqualTo(fullName + "(12,3,4)"));

            test = (Test)suite.Tests[2];
            Assert.That(test.TestName.Name, Is.EqualTo(name + "(12,4,3)"));
            Assert.That(test.TestName.FullName, Is.EqualTo(fullName + "(12,4,3)"));
        }

        [Test]
        public void RunningTestsThroughFixtureGivesCorrectResults()
		{
			TestResult result = fixture.Run(NullListener.NULL);
			ResultSummarizer summary = new ResultSummarizer( result );

			Assert.That( 
				summary.ResultCount, 
				Is.EqualTo( TestMethodSignatureFixture.Tests ) );
			Assert.That( 
				summary.TestsRun, 
				Is.EqualTo( TestMethodSignatureFixture.Runnable ) );
			Assert.That( 
				summary.NotRunnable, 
				Is.EqualTo( TestMethodSignatureFixture.NotRunnable ) );
            Assert.That(
                summary.Errors,
                Is.EqualTo(TestMethodSignatureFixture.Errors));
            Assert.That(
                summary.Failures,
                Is.EqualTo(TestMethodSignatureFixture.Failures));
            Assert.That( 
				summary.TestsNotRun, 
				Is.EqualTo( TestMethodSignatureFixture.NotRunnable ) );
		}
	}
}
