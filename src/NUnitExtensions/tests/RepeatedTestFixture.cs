using System;
using System.Reflection;
using NUnit.Core;
using NUnit.Framework;
using NUnit.Framework.Extensions;
using NUnit.Core.Tests;
using NUnit.Core.Builders;

namespace NUnit.Core.Extensions.Tests
{
	[TestFixture]
	public class RepeatedTestFixture
	{
		private MethodInfo successMethod;
		private MethodInfo failOnFirstMethod;
		private MethodInfo failOnThirdMethod;
		private RecordingListener listener;

		[SetUp]
		public void SetUp()
		{
			Type testType = typeof(RepeatSuccessFixture);
			successMethod = testType.GetMethod ("RepeatSuccess");
			testType = typeof(RepeatFailOnFirstFixture);
			failOnFirstMethod = testType.GetMethod("RepeatFailOnFirst");
			testType = typeof(RepeatFailOnThirdFixture);
			failOnThirdMethod = testType.GetMethod("RepeatFailOnThird");
			listener = new RecordingListener();
		}

		private TestResult RunTestOnFixture( object fixture )
		{
			TestSuite suite = TestFixtureBuilder.Make( fixture );
			Assert.AreEqual( 1, suite.Tests.Count, "Test case count" );
			TypeAssert.IsType( typeof( RepeatedTestCase ), suite.Tests[0] );
			return suite.Run( listener );
		}

		[Test]
		public void RepeatSuccess()
		{
			Assert.IsNotNull (successMethod);
			RepeatSuccessFixture fixture = new RepeatSuccessFixture();
			TestResult result = RunTestOnFixture( fixture );

			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual(3, fixture.SetupCount);
			Assert.AreEqual(3, fixture.TeardownCount);
			Assert.AreEqual(3, fixture.Count);
		}

		[Test]
		public void RepeatFailOnFirst()
		{
			Assert.IsNotNull (failOnFirstMethod);
			RepeatFailOnFirstFixture fixture = new RepeatFailOnFirstFixture();
			TestResult result = RunTestOnFixture( fixture );

			Assert.IsFalse(result.IsSuccess);
			Assert.AreEqual(1, fixture.SetupCount);
			Assert.AreEqual(1, fixture.TeardownCount);
			Assert.AreEqual(1, fixture.Count);
		}

		[Test]
		public void RepeatFailOnThird()
		{
			Assert.IsNotNull (failOnThirdMethod);
			RepeatFailOnThirdFixture fixture = new RepeatFailOnThirdFixture();
			TestResult result = RunTestOnFixture( fixture );

			Assert.IsFalse(result.IsSuccess);
			Assert.AreEqual(3, fixture.SetupCount);
			Assert.AreEqual(3, fixture.TeardownCount);
			Assert.AreEqual(3, fixture.Count);
		}

		[Test]
		public void IgnoreWorksWithRepeatedTest()
		{
			RepeatedTestWithIgnore fixture = new RepeatedTestWithIgnore();
			TestResult result = RunTestOnFixture( fixture );

			Assert.AreEqual( 0, fixture.SetupCount );
			Assert.AreEqual( 0, fixture.TeardownCount );
			Assert.AreEqual( 0, fixture.Count );
		}


	}

	[TestFixture]
	internal class RepeatingTestsBase
	{
		private int setupCount;
		private int teardownCount;
		protected int count;

		[SetUp]
		public void SetUp()
		{
			setupCount++;
		}

		[TearDown]
		public void TearDown()
		{
			teardownCount++;
		}

		public int SetupCount
		{
			get { return setupCount; }
		}
		public int TeardownCount
		{
			get { return teardownCount; }
		}
		public int Count
		{
			get { return count; }
		}
	}

	internal class RepeatSuccessFixture : RepeatingTestsBase
	{
		//[Test]
		[RepeatedTest(3)]
		public void RepeatSuccess()
		{
			count++;
			Assert.IsTrue (true);
		}
	}

	internal class RepeatFailOnFirstFixture : RepeatingTestsBase
	{
		//[Test]
		[RepeatedTest(3)]
		public void RepeatFailOnFirst()
		{
			count++;
			Assert.IsFalse (true);
		}
	}

	internal class RepeatFailOnThirdFixture : RepeatingTestsBase
	{
		//[Test]
		[RepeatedTest(3)]
		public void RepeatFailOnThird()
		{
			count++;

			if (count == 3)
				Assert.IsTrue (false);
		}
	}

	internal class RepeatedTestWithIgnore : RepeatingTestsBase
	{
		//[Test]
		[RepeatedTest(3), Ignore( "Ignore this test" )]
		public void RepeatShouldIgnore()
		{
			Assert.Fail( "Ignored test executed" );
		}
	}
}
