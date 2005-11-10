namespace NUnit.Framework.Tests
{
	[TestFixture]
	public class StringAssertTests
	{
		[Test]
		public void Contains()
		{
			Assert.Contains( "abc", "abc" );
			Assert.Contains( "abc", "***abc" );
			Assert.Contains( "abc", "**abc**" );
		}

		[Test]
		public void ContainsFails()
		{
			SubstringAsserter asserter = 
				new SubstringAsserter( "abc", "abxcdxbc", null, null );
			Assert.AreEqual( false, asserter.Test() );
			Assert.AreEqual( @"
	expected: String containing ""abc""
	 but was: <""abxcdxbc"">",
				asserter.Message );
		}

//		[Test]
//		public void ContainsAny()
//		{
//			Assert.ContainsAny( "xX", "abxcd" );
//			Assert.ContainsAny( "xX", "abXcd" );
//			Assert.ContainsAny( "xX", "axbxcxd" );		
//		}
//
//		[Test]
//		public void ContainsAnyFails()
//		{
//			ContainsAnyAsserter asserter = 
//				new ContainsAnyAsserter( "XYZ", "abxcdxbc", null, null );
//			Assert.AreEqual( false, asserter.Test() );
//			Assert.AreEqual( @"
//	expected: String containing any of ""XYZ""
//	 but was: <""abxcdxbc"">",
//				asserter.Message );
//		}

		[Test]
		public void StartsWith()
		{
			Assert.StartsWith( "abc", "abcdef" );
			Assert.StartsWith( "abc", "abc" );
		}

		[Test]
		public void StartsWithFails()
		{
			StartsWithAsserter asserter = 
				new StartsWithAsserter( "xyz", "abcxyz", null, null );
			Assert.AreEqual( false, asserter.Test() );
			Assert.AreEqual( @"
	expected: String starting with ""xyz""
	 but was: <""abcxyz"">",
				asserter.Message );
		}
	
		[Test]
		public void EndsWith()
		{
			Assert.EndsWith( "abc", "abc" );
			Assert.EndsWith( "abc", "123abc" );
		}

		[Test]
		public void EndsWithFails()
		{
			EndsWithAsserter asserter =
				new EndsWithAsserter( "xyz", "abcdef", null, null );
			Assert.AreEqual( false, asserter.Test() );
			Assert.AreEqual( @"
	expected: String ending with ""xyz""
	 but was: <""abcdef"">",
				asserter.Message );
		}

		[Test]
		public void CaseInsensitiveCompare()
		{
			Assert.AreEqualIgnoringCase( "name", "NAME" );
		}

		[Test]
		public void CaseInsensitiveCompareFails()
		{
			EqualIgnoringCaseAsserter asserter =
				new EqualIgnoringCaseAsserter( "Name", "NAMES", null, null );
			Assert.AreEqual( false, asserter.Test() );
			Assert.AreEqual( @"
	String lengths differ.  Expected length=4, but was length=5.
	Strings differ at index 4.
	expected: <""Name"">
	 but was: <""NAMES"">
	----------------^",
				asserter.Message );
		}
	}
}
