namespace NUnit.Framework.Extensions.Tests
{
	[TestFixture]
	public class StringAssertTests
	{
		[Test]
		public void Contains()
		{
			StringAssert.Contains( "abc", "abc" );
			StringAssert.Contains( "abc", "***abc" );
			StringAssert.Contains( "abc", "**abc**" );
		}

		[Test, ExpectedException( typeof( AssertionException ) )]
		public void ContainsFails()
		{
			StringAssert.Contains( "abc", "abxcdxbc" );
		}

		[Test]
		public void ContainsAny()
		{
			StringAssert.ContainsAny( "xX", "abxcd" );
			StringAssert.ContainsAny( "xX", "abXcd" );
			StringAssert.ContainsAny( "xX", "axbxcxd" );		
		}

		[Test, ExpectedException( typeof( AssertionException ) )]
		public void ContainsAnyFails()
		{
			StringAssert.Contains( "XYZ", "abxcdxbc" );
		}

		[Test]
		public void StartsWith()
		{
			StringAssert.StartsWith( "abc", "abcdef" );
			StringAssert.StartsWith( "abc", "abc" );
		}

		[Test, ExpectedException( typeof( AssertionException ) )]
		public void StartsWithFails()
		{
			StringAssert.StartsWith( "xyz", "abcxyz" );
		}
	
		[Test]
		public void EndsWith()
		{
			StringAssert.EndsWith( "abc", "abc" );
			StringAssert.EndsWith( "abc", "123abc" );
		}

		[Test, ExpectedException( typeof( AssertionException ) )]
		public void EndsWithFails()
		{
			StringAssert.EndsWith( "xyz", "abcdef" );
		}

		[Test]
		public void CaseInsensitiveCompare()
		{
			StringAssert.AreEqualIgnoringCase( "name", "NAME" );
		}

		[Test, ExpectedException( typeof( AssertionException ) )]
		public void CaseInsensitiveCompareFails()
		{
			StringAssert.AreEqualIgnoringCase( "Name", "NAMES" );
		}
	}
}
