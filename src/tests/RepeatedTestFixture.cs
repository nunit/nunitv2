using System;
using System.Reflection;
using NUnit.Core;
using NUnit.Framework;
using NUnit.Tests.Core;

namespace NUnit.Tests
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

		[Test]
		public void RepeatSuccess()
		{
			Assert.IsNotNull (successMethod);
			RepeatSuccessFixture fixture = new RepeatSuccessFixture();
			TestSuite suite = new TestSuite("RepeatSuccess");
			suite.Add(fixture);
			TestResult result = suite.Run(listener);
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
			TestSuite suite = new TestSuite("RepeatSuccess");
			suite.Add(fixture);
			TestResult result = suite.Run(listener);
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
			TestSuite suite = new TestSuite("RepeatSuccess");
			suite.Add(fixture);
			TestResult result = suite.Run(listener);
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
