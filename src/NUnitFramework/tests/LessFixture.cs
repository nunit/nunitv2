// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org/?p=license&r=2.4.
// ****************************************************************

using System;

namespace NUnit.Framework.Tests
{
	[TestFixture]
	public class LessFixture : MessageChecker
	{
		private readonly int i1 = 5;
		private readonly int i2 = 8;
		private readonly uint u1 = 12345678;
		private readonly uint u2 = 12345879;
		private readonly long l1 = 12345678;
		private readonly long l2 = 12345879;
		private readonly ulong ul1 = 12345678;
		private readonly ulong ul2 = 12345879;
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
			// Testing all forms after seeing some bugs. CFP
			Assert.Less(i1,i2);
			Assert.Less(i1,i2,"int");
			Assert.Less(i1,i2,"{0}","int");
			Assert.Less(u1,u2,"uint");
			Assert.Less(u1,u2,"{0}","uint");
			Assert.Less(l1,l2,"long");
			Assert.Less(l1,l2,"{0}","long");
			Assert.Less(ul1,ul2,"ulong");
			Assert.Less(ul1,ul2,"{0}","ulong");
			Assert.Less(d1,d2);
			Assert.Less(d1,d2, "double");
			Assert.Less(d1,d2, "{0}", "double");
			Assert.Less(de1,de2);
			Assert.Less(de1,de2, "decimal");
			Assert.Less(de1,de2, "{0}", "decimal");
			Assert.Less(f1,f2);
			Assert.Less(f1,f2, "float");
			Assert.Less(f1,f2, "{0}", "float");
		}

		[Test, ExpectedException( typeof( AssertionException ))]
		public void NotLessWhenEqual()
		{
			expectedMessage =
				"  Expected: less than 5" + Environment.NewLine +
				"  But was:  5" + Environment.NewLine;
			Assert.Less(i1,i1);
		}

		[Test, ExpectedException( typeof( AssertionException ))]
		public void NotLess()
		{
			expectedMessage =
				"  Expected: less than 5" + Environment.NewLine +
				"  But was:  8" + Environment.NewLine;
			Assert.Less(i2,i1);
		}

		[Test, ExpectedException( typeof( AssertionException ))]
		public void NotLessIComparable()
		{
			expectedMessage =
				"  Expected: less than StoredProcedure" + Environment.NewLine +
				"  But was:  TableDirect" + Environment.NewLine;
			Assert.Less(e2,e1);
		}

		[Test]
		public void FailureMessage()
		{
			string msg = null;

			try
			{
				Assert.Less( 9, 4 );
			}
			catch( AssertionException ex )
			{
				msg = ex.Message;
			}

			StringAssert.Contains( TextMessageWriter.Pfx_Expected + "less than 4", msg );
			StringAssert.Contains( TextMessageWriter.Pfx_Actual + "9", msg );
		}
	}
}


