using System;
using NUnit.Framework;

namespace NUnit.TestData.TestCaseTest
{
	[TestFixture]
	public class HasCategories 
	{
		[Test] 
		[Category("A category")]
		[Category("Another Category")]
		public void ATest()
		{}
	}
}
