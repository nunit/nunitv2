using System;

namespace NUnit.Framework.Tests
{
	[TestFixture]
	public class LessFixture
	{
		private readonly int i1 = 5;
		private readonly int i2 = 8;
		private readonly float f1 = 3.543F;
		private readonly float f2 = 8.543F;
		private readonly decimal de1 = 53.4M;
		private readonly decimal de2 = 83.4M;
		private readonly double d1 = 4.85948654;
		private readonly double d2 = 8.0;
		private readonly System.Enum e1 = System.Data.CommandType.StoredProcedure;
		private readonly System.Enum e2 = System.Data.CommandType.TableDirect;

		[Test]
		public void Less()
		{
			Assert.Less(i1,i2);
			Assert.Less(d1,d2, "double");
			Assert.Less(de1,de2, "{0}", "decimal");
			Assert.Less(f1,f2, "float");
		}

		[Test, ExpectedException( typeof( AssertionException ))]
		public void NotLessWhenEqual()
		{
			Assert.Less(i1,i1);
		}

		[Test, ExpectedException( typeof( AssertionException ))]
		public void NotLess()
		{
			Assert.Less(i2,i1);
		}

		[Test, ExpectedException( typeof( AssertionException ))]
		public void NotLessIComparable()
		{
			Assert.Less(e2,e1);
		}
	}
}


