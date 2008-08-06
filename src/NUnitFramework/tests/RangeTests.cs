using System;
using NUnit.Framework.Syntax.CSharp;

namespace NUnit.Framework.Tests
{
	[TestFixture]
	public class RangeTests
	{
		[Test]
		public void InRangeSucceeds()
		{
			Assert.That( 7, Is.InRange(5, 10) );
			Assert.That(0.23, Is.InRange(-1.0, 1.0));
			Assert.That(DateTime.Parse("12-December-2008"),
				Is.InRange(DateTime.Parse("1-October-2008"), DateTime.Parse("31-December-2008")));
		}


		[Test]
		public void InRangeFails()
		{
			string expectedMessage =
@"  Expected: in range (5,10)
  But was:  12
";
			Assert.That(
				new TestDelegate( FailingInRangeMethod ),
				Throws.Exception(typeof(AssertionException),
				Has.Property("Message").EqualTo(expectedMessage) ) );
		}

		private void FailingInRangeMethod()
		{
			Assert.That(12, Is.InRange(5, 10));
		}

		[Test]
		public void NotInRangeSucceeds()
		{
			Assert.That(12, Is.Not.InRange(5, 10));
			Assert.That(2.57, Is.Not.InRange(-1.0, 1.0));
		}

		[Test]
		public void NotInRangeFails()
		{
			string expectedMessage =
@"  Expected: not in range (5,10)
  But was:  7
";
			Assert.That(
				new TestDelegate(FailingNotInRangeMethod),
				Throws.Exception(typeof(AssertionException),
				Has.Property("Message").EqualTo(expectedMessage)));
		}

		private void FailingNotInRangeMethod()
		{
			Assert.That(7, Is.Not.InRange(5, 10));
		}
	}
}
