namespace Nunit.Framework
{
	using System;

	/// <summary>
	/// TestFixture
	/// </summary>
	/// 
	[TestFixture]
	public class TestFixture : Assertion
	{
		[SetUp]
		protected virtual void SetUp()
		{}

		[TearDown]
		protected virtual void TearDown()
		{}
	}
}
