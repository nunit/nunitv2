using System;
using NUnit.Framework;

namespace NUnit.Tests.Assertions
{
	/// <summary>
	/// Summary description for ArrayEqualTests.
	/// </summary>
	[TestFixture]
	public class ArrayEqualsFixture
	{
		[Test]
		public void SameArraysAreEqual()
		{
			string[] array = { "one", "two", "three" };
			Assert.AreSame( array, array );
			Assert.AreEqual( array, array );
		}

		[Test]
		public void DifferentArraysAreEqual()
		{
			string[] array1 = { "one", "two", "three" };
			string[] array2 = { "one", "two", "three" };
			Assert.IsFalse( array1 == array2 );
			Assert.AreEqual( array1, array2 );
		}

		[Test, ExpectedException( typeof( AssertionException ) )]
		public void DifferentLengthArrays()
		{
			string[] array1 = { "one", "two", "three" };
			string[] array2 = { "one", "two", "three", "four", "five" };

			Assert.AreEqual( array1, array2 );
		}

		[Test, ExpectedException( typeof( AssertionException ) )]
		public void SameLengthDifferentContent()
		{
			string[] array1 = { "one", "two", "three" };
			string[] array2 = { "one", "two", "ten" };
			Assert.AreEqual( array1, array2 );
		}

		[Test]
		public void DifferentArrayTypesButEqual()
		{
			string[] array1 = { "one", "two", "three" };
			object[] array2 = { "one", "two", "three" };
			Assert.AreEqual( array1, array2, "String[] not equal to Object[]" );
			Assert.AreEqual( array2, array1, "Object[] not equal to String[]" );	
		}

		[Test]
		public void MixedTypesAreEqual()
		{
			object[] array1 = { "one", 2, 3.0 };
			object[] array2 = { "one", 2.0, 3 };
			Assert.AreEqual( array1, array2 );
		}

		[Test, ExpectedException( typeof( AssertionException ) )]
		public void DifferentArrayTypesNotEqual()
		{
			string[] array1 = { "one", "two", "three" };
			object[] array2 = { "one", "three", "two" };
			Assert.AreEqual( array1, array2 );
		}
	}
}
