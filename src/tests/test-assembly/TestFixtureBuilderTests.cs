using System;
using NUnit.Framework;

namespace NUnit.TestData.TestFixtureBuilderTests
{
	[TestFixture]
	[Category("fixture category")]
	[Category("second")]
	public class HasCategories 
	{
		[Test] public void OneTest()
		{}
	}

	[TestFixture]
	public class SignatureTestFixture
	{
		[Test]
		public static void Static()
		{
		}

		[Test]
		public int NotVoid() 
		{
			return 1;
		}

		[Test]
		public void Parameters(string test) 
		{}
		
		[Test]
		protected void Protected() 
		{}

		[Test]
		private void Private() 
		{}


		[Test]
		public void TestVoid() 
		{}
	}
}
