using System;
using NUnit.Framework;

namespace NUnit.Framework.Tests
{
	/// <summary>
	/// Summary description for NaNFixture.
	/// </summary>
	[TestFixture]
	public class NaNFixture
	{
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
	}
}
