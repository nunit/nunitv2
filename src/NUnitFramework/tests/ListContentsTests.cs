using System;
using System.Collections;
using NUnit.Framework;

namespace NUnit.Framework.Tests
{
	/// <summary>
	/// Summary description for ListContentsTests.
	/// </summary>
	[TestFixture]
	public class ListContentsTests
	{
		private static readonly object[] testArray = { "abc", 123, "xyz" };

		[Test]
		public void ArraySucceeds()
		{
			Assert.Contains( "abc", testArray );
			Assert.Contains( 123, testArray );
			Assert.Contains( "xyz", testArray );
		}

		[Test]
		public void ArrayFails()
		{
			ListContentsAsserter asserter =
				new ListContentsAsserter( "def", testArray, null, null );
			Assert.AreEqual( false, asserter.Test() );
			Assert.AreEqual( 
				"	expected: <\"def\">" + Environment.NewLine + 
				"	 but was: <<\"abc\">,<123>,<\"xyz\">>" + Environment.NewLine,	
				asserter.Message );
		}

		[Test]
		public void EmptyArrayFails()
		{
			ListContentsAsserter asserter =
				new ListContentsAsserter( "def", new object[0], null, null );
			Assert.AreEqual( false, asserter.Test() );
			Assert.AreEqual( 
				"	expected: <\"def\">" + Environment.NewLine + 
				"	 but was: <empty>" + Environment.NewLine,
				asserter.Message );
		}

		[Test]
		public void NullArrayFails()
		{
			ListContentsAsserter asserter =
				new ListContentsAsserter( "def", null, null, null );
			Assert.AreEqual( false, asserter.Test() );
			Assert.AreEqual( 
				"	expected: <\"def\">" + Environment.NewLine + 
				"	 but was: <null>" + Environment.NewLine,
				asserter.Message );
		}

		[Test]
		public void ArrayListSucceeds()
		{
			ArrayList list = new ArrayList( testArray );

			Assert.Contains( "abc", list );
			Assert.Contains( 123, list );
			Assert.Contains( "xyz", list );
		}

		[Test]
		public void ArrayListFails()
		{
			ListContentsAsserter asserter =
				new ListContentsAsserter( "def", new ArrayList(testArray), null, null );
			Assert.AreEqual( false, asserter.Test() );
			Assert.AreEqual( 
				"	expected: <\"def\">" + Environment.NewLine + 
				"	 but was: <<\"abc\">,<123>,<\"xyz\">>" + Environment.NewLine,
				asserter.Message );
		}

		[Test]
		public void DifferentTypesFail()
		{
			ListContentsAsserter asserter =
				new ListContentsAsserter( 123.0, new ArrayList(testArray), null, null );
			Assert.AreEqual( false, asserter.Test() );
			Assert.AreEqual( 
				"	expected: <123>" + Environment.NewLine + 
				"	 but was: <<\"abc\">,<123>,<\"xyz\">>" + Environment.NewLine,
				asserter.Message );
			// TODO: Better message for this case
		}

	}
}
