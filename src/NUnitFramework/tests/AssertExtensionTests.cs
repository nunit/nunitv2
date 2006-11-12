using System;
using System.Text;

namespace NUnit.Framework.Tests
{
	[TestFixture]
	public class AssertExtensionTests
	{
		[Test]
		public void FormattedMessageTests()
		{
			new MessageTester( null, null ).Test( string.Empty );
			new MessageTester( string.Empty, null ).Test( string.Empty );
			new MessageTester( "xyz", null ).Test( "xyz" + Environment.NewLine );
			new MessageTester( "xyz", new object[0] ).Test( "xyz" + Environment.NewLine );
			new MessageTester( "one: {0} two: {1}", new object[] {1,2} ).Test( "one: 1 two: 2" + Environment.NewLine );
			new MessageTester( "xyz", new object[] {1,2} ).Test( "xyz" + Environment.NewLine );
		}

		private class MessageTester : AbstractAsserter
		{
			public MessageTester( string message, params object[] args )
				: base( message, args ) { }

			public override bool Test()
			{
				return false;
			}


			public void Test( string expected )
			{
				NUnit.Framework.Assert.AreEqual( expected, Message );
			}
		}

		[Test]
		public void OddNumber()
		{
			MyAssert.IsOdd( 27 );
		}

		[Test]
		public void OddNumberFails()
		{
			try
			{
				MyAssert.IsOdd( 28 );
				Assert.Fail("An AssertionException was expected but not thrown");
			}
			catch(AssertionException ae)
			{
				Assert.AreEqual(ae.Message,
					"\texpected: odd number" 
					+ System.Environment.NewLine 
					+ "\tactual:  <28>",
					"AssertionException thrown with incorrect message");
			}
			catch(Exception ex)
			{
				Assert.Fail("Expected AssertionException but caught: " + ex.ToString());
			}
		}

		private class MyAssert
		{
			static public void IsOdd( int num )
			{
				NUnit.Framework.Assert.DoAssert( new OddAsserter( num, null, null ) );
			}
		}

		private class OddAsserter : AbstractAsserter
		{
			private int num;

			public OddAsserter( int num, string message, params object[] args )
			   : base( message, args )
			{
				this.num = num;
			}

            public override bool Test()
            {
                return (this.num & 1)== 1;
            }
			public override string Message
			{
				get
				{	
					if ( FailureMessage.GetStringBuilder().Length > 0 )
						FailureMessage.WriteLine();
					FailureMessage.WriteLine( "\texpected: odd number" );
					FailureMessage.Write( "\tactual:  <{0}>", this.num );
					return FailureMessage.ToString();
				}
			}

		}
	}
}
