using System;

namespace NUnit.Framework.Tests
{
	[TestFixture]
	public class NotSameFixture
	{
		private readonly string s1 = "S1";
		private readonly string s2 = "S2";

		[Test]
		public void NotSame()
		{
			Assert.AreNotSame(s1, s2);
			new NotSameAsserter( s1, s2, null, null ).Assert();
		}

		[Test, ExpectedException( typeof( AssertionException ), "expected not same" )]
		public void NotSameFails()
		{
			Assert.AreNotSame( s1, s1 );
		}
	}
}
