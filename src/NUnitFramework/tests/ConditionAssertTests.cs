using System;
using System.Collections;
using NUnit.Framework;

namespace NUnit.Framework.Tests
{
	[TestFixture]
	public class ConditionAssertTests
	{
		[Test]
		public void IsTrue()
		{
			Assert.IsTrue(true);
		}

		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void IsTrueFails()
		{
			Assert.IsTrue(false);
		}

		[Test]
		public void IsFalse()
		{
			Assert.IsFalse(false);
		}

		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void IsFalseFails()
		{
			Assert.IsFalse(true);
		}
	
		[Test]
		public void IsNull()
		{
			Assert.IsNull(null);
		}

		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void IsNullFails()
		{
			String s1 = "S1";
			Assert.IsNull(s1);
		}
	
		[Test]
		public void IsNotNull()
		{
			String s1 = "S1";
			Assert.IsNotNull(s1);
		}

		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void IsNotNullFails()
		{
			Assert.IsNotNull(null);
		}
	
		[Test]
		public void IsNaN()
		{
			Assert.IsNaN(double.NaN);
		}

		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void IsNaNFails()
		{
			Assert.IsNaN(10.0);
		}

		[Test]
		public void IsEmpty()
		{
			Assert.IsEmpty( "", "Failed on empty String" );
			Assert.IsEmpty( new int[0], "Failed on empty Array" );
			Assert.IsEmpty( new ArrayList(), "Failed on empty ArrayList" );
			Assert.IsEmpty( new Hashtable(), "Failed on empty Hashtable" );
		}

		[Test, ExpectedException(typeof(AssertionException))]
		public void IsEmptyFailsOnString()
		{
			Assert.IsEmpty( "Hi!" );
		}

		[Test, ExpectedException(typeof(AssertionException))]
		public void IsEmptyFailsOnNullString()
		{
			Assert.IsEmpty( (string)null );
		}

		[Test, ExpectedException(typeof(AssertionException))]
		public void IsEmptyFailsOnNonEmptyArray()
		{
			Assert.IsEmpty( new int[] { 1, 2, 3 } );
		}

		[Test]
		public void IsNotEmpty()
		{
			int[] array = new int[] { 1, 2, 3 };
			ArrayList list = new ArrayList( array );
			Hashtable hash = new Hashtable();
			hash.Add( "array", array );

			Assert.IsNotEmpty( "Hi!", "Failed on String" );
			Assert.IsNotEmpty( array, "Failed on Array" );
			Assert.IsNotEmpty( list, "Failed on ArrayList" );
			Assert.IsNotEmpty( hash, "Failed on Hashtable" );
		}

		[Test, ExpectedException(typeof(AssertionException))]
		public void IsNotEmptyFailsOnEmptyString()
		{
			Assert.IsNotEmpty( "" );
		}

		[Test, ExpectedException(typeof(AssertionException))]
		public void IsNotEmptyFailsOnEmptyArray()
		{
			Assert.IsNotEmpty( new int[0] );
		}

		[Test, ExpectedException(typeof(AssertionException))]
		public void IsNotEmptyFailsOnEmptyArrayList()
		{
			Assert.IsNotEmpty( new ArrayList() );
		}

		[Test, ExpectedException(typeof(AssertionException))]
		public void IsNotEmptyFailsOnEmptyHashTable()
		{
			Assert.IsNotEmpty( new Hashtable() );
		}
	}
}
