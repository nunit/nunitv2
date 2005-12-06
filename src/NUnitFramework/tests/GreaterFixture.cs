using System;

namespace NUnit.Framework.Tests
{
	[TestFixture]
	public class GreaterFixture
	{
		private readonly int i1 = 5;
		private readonly int i2 = 4;
		private readonly float f1 = 3.543F;
		private readonly float f2 = 2.543F;
		private readonly decimal de1 = 53.4M;
		private readonly decimal de2 = 33.4M;
		private readonly double d1 = 4.85948654;
		private readonly double d2 = 1.0;
		private readonly System.Enum e1 = System.Data.CommandType.TableDirect;
		private readonly System.Enum e2 = System.Data.CommandType.StoredProcedure;

		[Test]
		public void Greater()
		{
			Assert.Greater(i1,i2);
			Assert.Greater(d1,d2, "double");
			Assert.Greater(de1,de2, "{0}", "decimal");
			Assert.Greater(f1,f2, "float");
		}

		[Test, ExpectedException( typeof( AssertionException ))]
		public void NotGreaterWhenEqual()
		{
			Assert.Greater(i1,i1);
		}

		[Test, ExpectedException( typeof( AssertionException ))]
		public void NotGreater()
		{
			Assert.Greater(i2,i1);
		}

		[Test, ExpectedException( typeof( AssertionException ))]
		public void NotGreaterIComparable()
		{
			Assert.Greater(e2,e1);
		}
	}
}


