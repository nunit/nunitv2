using System;
using System.Collections;
using NUnit.Framework;

namespace NUnit.Framework.Tests
{
	[TestFixture]
	public class ConditionAsserts
	{
		[Test]
		public void AssertTrue()
		{
			Assert.IsTrue(true);
		}

		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void AssertTrueFails()
		{
			Assert.IsTrue(false);
		}

		[Test]
		public void AssertFalse()
		{
			Assert.IsFalse(false);
		}

		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void AssertFalseFails()
		{
			Assert.IsFalse(true);
		}
	
		[Test]
		public void Null()
		{
			Assert.IsNull(null);
		}

		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void NullFails()
		{
			String s1 = "S1";
			Assert.IsNull(s1);
		}
	
		[Test]
		public void NotNull()
		{
			String s1 = "S1";
			Assert.IsNotNull(s1);
		}

		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void NotNullFails()
		{
			Assert.IsNotNull(null);
		}
	
		[Test]
		public void NaN()
		{
			Assert.IsNaN(double.NaN);
		}

		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void NaNFails()
		{
			Assert.IsNaN(10.0);
		}

		[Test]
		public void EmptyString()
		{
			Assert.IsEmpty( "" );
		}

		[Test, ExpectedException(typeof(AssertionException))]
		public void EmptyStringFails()
		{
			Assert.IsEmpty( "Hi!" );
		}

		[Test, ExpectedException(typeof(AssertionException))]
		public void EmptyStringFailsOnNull()
		{
			Assert.IsEmpty( (string)null );
		}

		[Test]
		public void EmptyArray()
		{
			Assert.IsEmpty( new int[0] );
		}

		[Test]
		public void EmptyArrayList()
		{
			Assert.IsEmpty( new ArrayList() );
		}

		[Test]
		public void EmptyHashtable()
		{
			Assert.IsEmpty( new Hashtable() );
		}

		[Test, ExpectedException(typeof(AssertionException))]
		public void EmptyFails()
		{
			Assert.IsEmpty( new int[] { 1, 2, 3 } );
		}
	}
}
