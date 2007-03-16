// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using System.Text;

namespace NUnit.Framework.Tests
{
	[TestFixture]
	public class AssertExtensionTests
	{
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

		[Obsolete]
		private class MyAssert
		{
			static public void IsOdd( int num )
			{
				NUnit.Framework.Assert.DoAssert( new OddAsserter( num, null, null ) );
			}
		}

		[Obsolete]
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
