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
			new MessageTester( "xyz", null ).Test( "xyz" );
			new MessageTester( "xyz", new object[0] ).Test( "xyz" );
			new MessageTester( "one: {0} two: {1}", new object[] {1,2} ).Test( "one: 1 two: 2" );
			new MessageTester( "xyz", new object[] {1,2} ).Test( "xyz" );
		}

		private class MessageTester : AbstractAsserter
		{
			public MessageTester( string message, params object[] args )
				: base( message, args ) { }

			public void Test( string expected )
			{
				NUnit.Framework.Assert.AreEqual( expected, FormattedMessage );
			}

			public override void Assert() { }
		}

		[Test]
		public void OddNumber()
		{
			MyAssert.IsOdd( 27 );
		}

		[Test]
		[ExpectedException( typeof( AssertionException ), 
			"\texpected: odd number\r\n\tactual:  <28>")]
		public void OddNumberFails()
		{
			MyAssert.IsOdd( 28 );
		}

		private class MyAssert
		{
			static public void IsOdd( int num )
			{
				NUnit.Framework.Assert.DoAssert( new OddAsserter( num, null, null ) );
			}
		}

		private class OddAsserter : ConditionAsserter
		{
			private int num;

			public OddAsserter( int num, string message, params object[] args )
			   : base( (num & 1) == 1, message, args )
			{
				this.num = num;
			}

			public override void Assert()
			{
				if ( !condition )
				{	
					StringBuilder sb = new StringBuilder( FormattedMessage );
					if ( sb.Length > 0 ) sb.Append( Environment.NewLine );
					sb.Append( "\texpected: odd number" );
					sb.Append( Environment.NewLine );
					sb.AppendFormat( "\tactual:  <{0}>", this.num );
					throw new AssertionException( sb.ToString() );
				}
			}

		}
	}
}
