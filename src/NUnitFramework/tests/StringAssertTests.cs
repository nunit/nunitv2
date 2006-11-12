namespace NUnit.Framework.Tests
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

		[Test]
		public void ContainsFails()
		{
			ContainsAsserter asserter = 
				new ContainsAsserter( "abc", "abxcdxbc", null, null );
			Assert.AreEqual( false, asserter.Test() );
			Assert.AreEqual(
				"\t" + @"expected: String containing ""abc""" + System.Environment.NewLine +
				"\t" + @" but was: <""abxcdxbc"">" + System.Environment.NewLine,
				asserter.Message );
		}

		[Test]
		public void StartsWith()
		{
			StringAssert.StartsWith( "abc", "abcdef" );
			StringAssert.StartsWith( "abc", "abc" );
		}

		[Test]
		public void StartsWithFails()
		{
			StartsWithAsserter asserter = 
				new StartsWithAsserter( "xyz", "abcxyz", null, null );
			Assert.AreEqual( false, asserter.Test() );
			Assert.AreEqual(
				"\t" + @"expected: String starting with ""xyz""" + System.Environment.NewLine +
				"\t" + @" but was: <""abcxyz"">" + System.Environment.NewLine,
				asserter.Message );
		}
	
		[Test]
		public void EndsWith()
		{
			StringAssert.EndsWith( "abc", "abc" );
			StringAssert.EndsWith( "abc", "123abc" );
		}

		[Test]
		public void EndsWithFails()
		{
			EndsWithAsserter asserter =
				new EndsWithAsserter( "xyz", "abcdef", null, null );
			Assert.AreEqual( false, asserter.Test() );
			Assert.AreEqual(
				"\t" + @"expected: String ending with ""xyz""" + System.Environment.NewLine +
				"\t" + @" but was: <""abcdef"">" + System.Environment.NewLine,
				asserter.Message );
		}

		[Test]
		public void CaseInsensitiveCompare()
		{
			StringAssert.AreEqualIgnoringCase( "name", "NAME" );
		}

		[Test]
		public void CaseInsensitiveCompareFails()
		{
			EqualIgnoringCaseAsserter asserter =
				new EqualIgnoringCaseAsserter( "Name", "NAMES", null, null );
			Assert.AreEqual( false, asserter.Test() );
			Assert.AreEqual(
	"\tString lengths differ.  Expected length=4, but was length=5." + System.Environment.NewLine
	+ "\tStrings differ at index 4." + System.Environment.NewLine
	+ "\t" + @"expected: <""Name"">" + System.Environment.NewLine
	+ "\t" + @" but was: <""NAMES"">" + System.Environment.NewLine
	+ "\t----------------^" + System.Environment.NewLine,
				asserter.Message );
		}

		[Test]		
		public void IsMatch()
		{
			StringAssert.IsMatch( "a?bc", "12a3bc45" );
		}

		[Test]
		public void IsMatchFails()
		{
			RegexAsserter asserter =
				new RegexAsserter( "a?b*c", "12ab456", null, null );
			Assert.IsFalse( asserter.Test() );
			Assert.AreEqual(
				"\t" + @"expected: String matching ""a?b*c""" + System.Environment.NewLine
				+ "\t" + @" but was: <""12ab456"">"  + System.Environment.NewLine,
				asserter.Message );
		}
	}
}
