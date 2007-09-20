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

        [Test]
        public void TestClipString()
        {
            Assert.AreEqual(s52, MsgUtils.ClipString2(s52, 52, 0));

            Assert.AreEqual("abcdefghijklmnopqrstuvwxyz...",
                MsgUtils.ClipString2(s52, 29, 0));
            Assert.AreEqual("...ABCDEFGHIJKLMNOPQRSTUVWXYZ",
                MsgUtils.ClipString2(s52, 29, 26));
            Assert.AreEqual("...ABCDEFGHIJKLMNOPQRSTUV...",
                MsgUtils.ClipString2(s52, 28, 26));
        }

        private static readonly string s52 = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        [Test]
        public void ClipExpectedAndActual_StringsFitInLine()
        {
            string eClip = s52;
            string aClip = "abcde";
            MsgUtils.ClipExpectedAndActual(ref eClip, ref aClip, 52, 5);
            Assert.AreEqual(s52, eClip);
            Assert.AreEqual("abcde", aClip);

            eClip = s52;
            aClip = "abcdefghijklmno?qrstuvwxyz";
            MsgUtils.ClipExpectedAndActual(ref eClip, ref aClip, 52, 15);
            Assert.AreEqual(s52, eClip);
            Assert.AreEqual("abcdefghijklmno?qrstuvwxyz", aClip);
        }

        [Test]
        public void ClipExpectedAndActual_StringTailsFitInLine()
        {
            string s1 = s52;
            string s2 = s52.Replace('Z', '?');
            MsgUtils.ClipExpectedAndActual(ref s1, ref s2, 29, 51);
            Assert.AreEqual("...ABCDEFGHIJKLMNOPQRSTUVWXYZ", s1);
        }

        [Test]
        public void ClipExpectedAndActual_StringsDoNotFitInLine()
        {
            string s1 = s52;
            string s2 = "abcdefghij";
            MsgUtils.ClipExpectedAndActual(ref s1, ref s2, 29, 10);
            Assert.AreEqual("abcdefghijklmnopqrstuvwxyz...", s1);
            Assert.AreEqual("abcdefghij", s2);

            s1 = s52;
            s2 = "abcdefghijklmno?qrstuvwxyz";
            MsgUtils.ClipExpectedAndActual(ref s1, ref s2, 25, 15);
            Assert.AreEqual("...efghijklmnopqrstuvw...", s1);
            Assert.AreEqual("...efghijklmno?qrstuvwxyz", s2);
        }
	}
}
