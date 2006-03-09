using System;
using NUnit.Framework;
using NUnit.Framework.Extensions;

namespace NUnit.TestData.RepeatedTestFixture
{
	[TestFixture]
	public class RepeatingTestsBase
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

	public class RepeatSuccessFixture : RepeatingTestsBase
	{
		//[Test]
		[RepeatedTest(3)]
		public void RepeatSuccess()
		{
			count++;
			Assert.IsTrue (true);
		}
	}

	public class RepeatFailOnFirstFixture : RepeatingTestsBase
	{
		//[Test]
		[RepeatedTest(3)]
		public void RepeatFailOnFirst()
		{
			count++;
			Assert.IsFalse (true);
		}
	}

	public class RepeatFailOnThirdFixture : RepeatingTestsBase
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

	public class RepeatedTestWithIgnore : RepeatingTestsBase
	{
		//[Test]
		[RepeatedTest(3), Ignore( "Ignore this test" )]
		public void RepeatShouldIgnore()
		{
			Assert.Fail( "Ignored test executed" );
		}
	}
}
