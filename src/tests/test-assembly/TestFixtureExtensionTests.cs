using System;
using NUnit.Framework;

namespace NUnit.TestData.TestFixtureExtension
{
	[TestFixture]
	public abstract class BaseTestFixture : NUnit.Framework.TestCase
	{
		public bool baseSetup = false;
		public bool baseTeardown = false;

		protected override void SetUp()
		{ baseSetup = true; }

		protected override void TearDown()
		{ baseTeardown = true; }
	}

	public class DerivedTestFixture : BaseTestFixture
	{
		[Test]
		public void Success()
		{
			Assert(true);
		}
	}

	public class SetUpDerivedTestFixture : BaseTestFixture
	{
		[SetUp]
		public void Init()
		{
			base.SetUp();
		}

		[Test]
		public void Success()
		{
			Assert(true);
		}
	}
}
