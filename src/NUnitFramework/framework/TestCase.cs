namespace NUnit.Framework
{
	using System;

	/// <summary>
	/// TestFixture
	/// </summary>
	/// 
	[TestFixture]
	[Obsolete("use TestFixture attribute instead of inheritance",false)]
	public class TestCase : Assertion
	{
		[SetUp]
		[Obsolete("use SetUp attribute instead of naming convention",false)]
		protected virtual void SetUp()
		{}

		[TearDown]
		[Obsolete("use TearDown attribute instead of naming convention",false)]
		protected virtual void TearDown()
		{}
	}
}
