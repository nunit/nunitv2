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
		}

		//CCF 3/11/06
		//Modified to try/catch the exception since .NET won't let us use
		//System.Environment.NewLines in Custom Properties
		[Test]
		public void NotSameFails()
		{
			try
			{
				Assert.AreNotSame( s1, s1 );
			} catch(AssertionException ae) {
				Assert.AreEqual(
					"Objects should be different" + System.Environment.NewLine + 
					"\tboth are: <\"S1\">" + Environment.NewLine, 
					ae.Message );
				return;
			}
			Assert.Fail("Expected AssertionException to be thrown");
		}
	}
}
