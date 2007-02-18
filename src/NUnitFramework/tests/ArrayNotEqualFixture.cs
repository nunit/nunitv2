using System;

namespace NUnit.Framework.Tests
{
	/// <summary>
	/// Summary description for ArrayNotEqualFixture.
	/// </summary>
	[TestFixture]
	public class ArrayNotEqualFixture
	{
		[Test]
		public void DifferentLengthArrays()
		{
			string[] array1 = { "one", "two", "three" };
			string[] array2 = { "one", "two", "three", "four", "five" };

			Assert.AreNotEqual(array1, array2);
			Assert.AreNotEqual(array2, array1);
			Assert.That(array1, Is.Not.EqualTo(array2));
			Assert.That(array2, Is.Not.EqualTo(array1));
		}

		[Test]
		public void SameLengthDifferentContent()
		{
			string[] array1 = { "one", "two", "three" };
			string[] array2 = { "one", "two", "ten" };
			Assert.AreNotEqual(array1, array2);
			Assert.AreNotEqual(array2, array1);
			Assert.That(array1, Is.Not.EqualTo(array2));
			Assert.That(array2, Is.Not.EqualTo(array1));
		}

		[Test]
		public void ArraysDeclaredAsDifferentTypes()
		{
			string[] array1 = { "one", "two", "three" };
			object[] array2 = { "one", "three", "two" };
			Assert.AreNotEqual(array1, array2);
			Assert.That(array1, Is.Not.EqualTo(array2));
			Assert.That(array2, Is.Not.EqualTo(array1));
		}

	}
}
