using System;

namespace NUnit.Framework.Tests
{
	[TestFixture]
	public class NotEqualFixture
	{
		[Test]
		public void NotEqual()
		{
			Assert.AreNotEqual( 5, 3 );
		}

		[Test, ExpectedException( typeof( AssertionException ) )]
		public void NotEqualFails()
		{
			Assert.AreNotEqual( 5, 5 );
		}

		[Test]
		public void NullNotEqualToNonNull()
		{
			Assert.AreNotEqual( null, 3 );
		}

		[Test, ExpectedException( typeof( AssertionException ) )]
		public void NullEqualsNull()
		{
			Assert.AreNotEqual( null, null );
		}

		[Test]
		public void ArraysNotEqual()
		{
			Assert.AreNotEqual( new object[] { 1, 2, 3 }, new object[] { 1, 3, 2 } );
		}

		[Test, ExpectedException( typeof( AssertionException ) )]
		public void ArraysNotEqualFails()
		{
			Assert.AreNotEqual( new object[] { 1, 2, 3 }, new object[] { 1, 2, 3 } );
		}
	}
}