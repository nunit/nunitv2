using System;

namespace NUnit.Core
{
	public struct TestFixtureParameters
	{
		public string FrameworkName;
		public string TestFixtureType;
		public string TestType;
		public string ExpectedExceptionType;
		public string SetUpType;
		public string TearDownType;
		public string FixtureSetUpType;
		public string FixtureTearDownType;
		public string ExplicitType;
		public string CategoryType;
		public string IgnoreType;
		public string PlatformType;

		public TestFixtureParameters(
			string FrameworkName,
			string TestFixtureType,
			string TestType,
			string ExpectedExceptionType,
			string SetUpType,
			string TearDownType,
			string FixtureSetUpType,
			string FixtureTearDownType,
			string ExplicitType,
			string CategoryType,
			string IgnoreType,
			string PlatformType )
		{
			this.FrameworkName = FrameworkName;
			this.TestFixtureType = TestFixtureType;
			this.TestType = TestType;
			this.ExpectedExceptionType = ExpectedExceptionType;
			this.SetUpType = SetUpType;
			this.TearDownType = TearDownType;
			this.FixtureSetUpType = FixtureSetUpType;
			this.FixtureTearDownType = FixtureTearDownType;
			this.ExplicitType = ExplicitType;
			this.CategoryType = CategoryType;
			this.IgnoreType = IgnoreType;
			this.PlatformType = PlatformType;
		}
	}
}
