using System;
using System.Reflection;
using NUnit.Core;
using NUnit.Framework;
using NUnit.Core.Tests;
using NUnit.Core.Builders;

namespace NUnit.Extensions.Tests
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
			TestSuite suite = new NUnitTestFixtureBuilder().BuildFrom( fixture.GetType() );
			suite.Fixture = fixture;
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
	}

	internal class RepeatingTestsBase
	{
		private int setupCount;
		private int teardownCount;

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

	}

	internal class RepeatSuccessFixture : RepeatingTestsBase
	{
		private int count = 0;

		public int Count
		{
			get { return count; }
		}

		[Test]
		[RepeatedTest(3)]
		public void RepeatSuccess()
		{
			count++;
			Assert.IsTrue (true);
		}
	}

	internal class RepeatFailOnFirstFixture : RepeatingTestsBase
	{
		private int count;

		public int Count
		{
			get { return count; }
		}

		[Test]
		[RepeatedTest(3)]
		public void RepeatFailOnFirst()
		{
			count++;
			Assert.IsFalse (true);
		}
	}

	internal class RepeatFailOnThirdFixture : RepeatingTestsBase
	{
		private int count = 0;

		public int Count
		{
			get { return count; }
		}

		[Test]
		[RepeatedTest(3)]
		public void RepeatFailOnThird()
		{
			count++;

			if (count == 3)
				Assert.IsTrue (false);
		}
	}
}
