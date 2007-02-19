// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;

namespace NUnit.Framework.Tests
{
	/// <summary>
	/// Summary description for MsgUtilTests.
	/// </summary>
	[TestFixture]
	public class MsgUtilTests
	{
		[Test]
		public void TestConvertWhitespace()
		{
			Assert.AreEqual( "\\n", MsgUtils.ConvertWhitespace("\n") );
			Assert.AreEqual( "\\n\\n", MsgUtils.ConvertWhitespace("\n\n") );
			Assert.AreEqual( "\\n\\n\\n", MsgUtils.ConvertWhitespace("\n\n\n") );

			Assert.AreEqual( "\\r", MsgUtils.ConvertWhitespace("\r") );
			Assert.AreEqual( "\\r\\r", MsgUtils.ConvertWhitespace("\r\r") );
			Assert.AreEqual( "\\r\\r\\r", MsgUtils.ConvertWhitespace("\r\r\r") );

			Assert.AreEqual( "\\r\\n", MsgUtils.ConvertWhitespace("\r\n") );
			Assert.AreEqual( "\\n\\r", MsgUtils.ConvertWhitespace("\n\r") );
			Assert.AreEqual( "This is a\\rtest message", MsgUtils.ConvertWhitespace("This is a\rtest message") );

			Assert.AreEqual( "", MsgUtils.ConvertWhitespace("") );
			Assert.AreEqual( null, MsgUtils.ConvertWhitespace(null) );
                
			Assert.AreEqual( "\\t", MsgUtils.ConvertWhitespace( "\t" ) );
			Assert.AreEqual( "\\t\\n", MsgUtils.ConvertWhitespace( "\t\n" ) );

			Assert.AreEqual( "\\\\r\\\\n", MsgUtils.ConvertWhitespace( "\\r\\n" ) );
		}
	}
}
