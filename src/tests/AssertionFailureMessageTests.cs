#region Copyright (c) 2002, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Philip A. Craig, Douglas de la Torre
/************************************************************************************
'
' Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov
' Copyright © 2000-2002 Philip A. Craig
' Copyright © 2001 Douglas de la Torre
'
' This software is provided 'as-is', without any express or implied warranty. In no 
' event will the authors be held liable for any damages arising from the use of this 
' software.
' 
' Permission is granted to anyone to use this software for any purpose, including 
' commercial applications, and to alter it and redistribute it freely, subject to the 
' following restrictions:
'
' 1. The origin of this software must not be misrepresented; you must not claim that 
' you wrote the original software. If you use this software in a product, an 
' acknowledgment (see the following) in the product documentation is required.
'
' Portions Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov 
' Copyright © 2000-2002 Philip A. Craig, or Copyright © 2001 Douglas de la Torre
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

namespace NUnit.Tests
{
    using System;
    using NUnit.Framework;

    /// <summary>
    /// Wrapper class to give us 'inner-class' access to the methods of this
    /// class, without exposing them publicly.  We need to be able to test
    /// these methods, but because this is a framework, don't want others
    /// calling the methods.
    /// </summary>
    public class MyAssertionFailureMessage : NUnit.Framework.AssertionFailureMessage
    {
        /// <summary>
        /// Protected constructor, used since this class is only used via
        /// static methods
        /// </summary>
        protected MyAssertionFailureMessage() : base()	{}

        /// <summary>
        /// Summary description for AssertionFailureMessageTests.
        /// </summary>
        /// 
		[TestFixture]
        public class AssertionFailureMessageTests 
        {
            /// <summary>
            /// 
            /// </summary>
            /// 
			[Test]
            public void TestInputsAreStrings()
            {
                Assertion.AssertEquals( false, InputsAreStrings( null, null ) );
                Assertion.AssertEquals( false, InputsAreStrings( new Object(), null ) );
                Assertion.AssertEquals( false, InputsAreStrings( null, new Object() ) );
                Assertion.AssertEquals( false, InputsAreStrings( new Object(), "" ) );
                Assertion.AssertEquals( false, InputsAreStrings( "", new Object() ) );
                Assertion.AssertEquals( true,  InputsAreStrings( "", "" ) );
                Assertion.AssertEquals( true,  InputsAreStrings( "test1", "test1" ) );
                Assertion.AssertEquals( true,  InputsAreStrings( "some", "value" ) );
            }

            /// <summary>
            /// 
            /// </summary>
            /// 
			[Test]
            public void TestCreateStringBuilder()
            {
                Assertion.AssertEquals( "",        CreateStringBuilder( null ).ToString() );
                Assertion.AssertEquals( "",        CreateStringBuilder( "" ).ToString() );
                Assertion.AssertEquals( "a",       CreateStringBuilder( "a" ).ToString() );
                Assertion.AssertEquals( "message", CreateStringBuilder( "message" ).ToString() );
            }

            /// <summary>
            /// Tests the string-clipping method to see if it clips at the
            /// expected positions.
            /// </summary>
			[Test]
			public void TestClipAroundPosition()
            {
                // Some spot-checks
                Assertion.AssertEquals( "", ClipAroundPosition( null, 0 ) );
                Assertion.AssertEquals( "", ClipAroundPosition( "", 0 ) );
                Assertion.AssertEquals( "a", ClipAroundPosition( "a", 0 ) );
                Assertion.AssertEquals( "ab", ClipAroundPosition( "ab", 0 ) );
                Assertion.AssertEquals( "ab", ClipAroundPosition( "ab", 1 ) );
                Assertion.AssertEquals( "abc", ClipAroundPosition( "abc", 0 ) );
                Assertion.AssertEquals( "abc", ClipAroundPosition( "abc", 1 ) );
                Assertion.AssertEquals( "abc", ClipAroundPosition( "abc", 2 ) );

                Assertion.AssertEquals( "012345678", ClipAroundPosition( "012345678", 0 ) );
                Assertion.AssertEquals( "012345678", ClipAroundPosition( "012345678", 1 ) );
                Assertion.AssertEquals( "012345678", ClipAroundPosition( "012345678", 2 ) );
                Assertion.AssertEquals( "012345678", ClipAroundPosition( "012345678", 7 ) );
                Assertion.AssertEquals( "012345678", ClipAroundPosition( "012345678", 8 ) );

                Assertion.AssertEquals( "0123456789", ClipAroundPosition( "0123456789", 0 ) );
                Assertion.AssertEquals( "0123456789", ClipAroundPosition( "0123456789", 1 ) );
                Assertion.AssertEquals( "0123456789", ClipAroundPosition( "0123456789", 2 ) );
                Assertion.AssertEquals( "0123456789", ClipAroundPosition( "0123456789", 7 ) );
                Assertion.AssertEquals( "0123456789", ClipAroundPosition( "0123456789", 8 ) );
                Assertion.AssertEquals( "0123456789", ClipAroundPosition( "0123456789", 9 ) );

                Assertion.AssertEquals( "01234567890", ClipAroundPosition( "01234567890", 0 ) );
                Assertion.AssertEquals( "01234567890", ClipAroundPosition( "01234567890", 1 ) );
                Assertion.AssertEquals( "01234567890", ClipAroundPosition( "01234567890", 2 ) );
                Assertion.AssertEquals( "01234567890", ClipAroundPosition( "01234567890", 7 ) );
                Assertion.AssertEquals( "01234567890", ClipAroundPosition( "01234567890", 8 ) );
                Assertion.AssertEquals( "01234567890", ClipAroundPosition( "01234567890", 9 ) );
                Assertion.AssertEquals( "01234567890", ClipAroundPosition( "01234567890", 10 ) );
            }

            /// <summary>
            /// Tests the string-clipping method to see if it clips at the
            /// expected positions.
            /// </summary>
			[Test]
			public void TestClipAroundPosition2()
            {
                const string sTest = "a0a1a2a3a4a5a6a7a8a9b0b1b2b3b4b5b6b7b8b9c0c1c2c3c4c5c6c7c8c9d0d1d2d3d4d5d6d7d8d9";
                int iTestPosition;

                // Want no pre-clipping and post-clipping when position is before 
                // first 10 digits
                for( int i=0; i<=PreClipLength; i++ )
                {
                    iTestPosition = i;
                    Assertion.AssertEquals( sTest.Substring( 0, i+PostClipLength ) + "...",
                        ClipAroundPosition( sTest, iTestPosition ) );
                }

                // Want pre- and post-clipping when position is after
                // first 10 digits and before last 40 digits
                for( int i=PreClipLength+1; i<(sTest.Length - PostClipLength); i++ )
                {
                    iTestPosition = i;
                    Assertion.AssertEquals( "..." + sTest.Substring( iTestPosition-PreClipLength, PreClipLength+PostClipLength ) + "...",
                        ClipAroundPosition( sTest, iTestPosition ) );
                }

                // Want pre-clipping and no post-clipping when position is within 
                // last 40 digits
                for( int i=(sTest.Length - PostClipLength); i<sTest.Length; i++ )
                {
                    iTestPosition = i;
                    Assertion.AssertEquals( "..." + sTest.Substring( iTestPosition-PreClipLength ),
                        ClipAroundPosition( sTest, iTestPosition ) );
                }
            }

            /// <summary>
            /// Makes sure that whitespace conversion for CR, LF, or TAB
            ///  characters into 2 characters, one slash '\' and 
            ///  one 'r' or 'n' or 't' for display
            /// </summary>
			[Test]
            public void TestConvertWhitespace()
            {
                Assertion.AssertEquals( "\\n", ConvertWhitespace("\n") );
                Assertion.AssertEquals( "\\n\\n", ConvertWhitespace("\n\n") );
                Assertion.AssertEquals( "\\n\\n\\n", ConvertWhitespace("\n\n\n") );

                Assertion.AssertEquals( "\\r", ConvertWhitespace("\r") );
                Assertion.AssertEquals( "\\r\\r", ConvertWhitespace("\r\r") );
                Assertion.AssertEquals( "\\r\\r\\r", ConvertWhitespace("\r\r\r") );

                Assertion.AssertEquals( "\\r\\n", ConvertWhitespace("\r\n") );
                Assertion.AssertEquals( "\\n\\r", ConvertWhitespace("\n\r") );
                Assertion.AssertEquals( "This is a\\rtest message", ConvertWhitespace("This is a\rtest message") );

                Assertion.AssertEquals( "", ConvertWhitespace("") );
                Assertion.AssertEquals( null, ConvertWhitespace(null) );
                
                Assertion.AssertEquals( "\\t", ConvertWhitespace( "\t" ) );
                Assertion.AssertEquals( "\\t\\n", ConvertWhitespace( "\t\n" ) );
            }

            /// <summary>
            /// Checks several common failure conditions to ensure the output
            /// strings match the expected output.
            /// </summary>
			[Test]
			public void TestFormatMessageForFailNotEquals()
            {
                Assertion.AssertEquals( "\r\nexpected:<(null)>\r\n but was:<(null)>", 
                    FormatMessageForFailNotEquals( null, null, null ) );
                Assertion.AssertEquals( "\r\nexpected:<(null)>\r\n but was:<(null)>", 
                    FormatMessageForFailNotEquals( "", null, null ) );
                Assertion.AssertEquals( "message \r\nexpected:<(null)>\r\n but was:<(null)>", 
                    FormatMessageForFailNotEquals( "message", null, null ) );
                Assertion.AssertEquals( "message \r\nexpected:<1>\r\n but was:<2>", 
                    FormatMessageForFailNotEquals( "message", 1, 2 ) );
                Assertion.AssertEquals( "message \r\nexpected:<>\r\n but was:<(null)>", 
                    FormatMessageForFailNotEquals( "message", "", null ) );
                Assertion.AssertEquals( "message \r\nString lengths differ.  Expected length=1, but was length=2.\r\nStrings differ at index 1.\r\n\r\nexpected:<a>\r\n but was:<aa>\r\n-----------^\r\n",
                    FormatMessageForFailNotEquals( "message", "a", "aa" ) );
                Assertion.AssertEquals( "message \r\nString lengths are both 2.\r\nStrings differ at index 1.\r\n\r\nexpected:<aa>\r\n but was:<ab>\r\n-----------^\r\n",
                    FormatMessageForFailNotEquals( "message", "aa", "ab" ) );
                Assertion.AssertEquals( "message \r\nString lengths differ.  Expected length=1, but was length=3.\r\nStrings differ at index 1.\r\n\r\nexpected:<a>\r\n but was:<abc>\r\n-----------^\r\n",
                    FormatMessageForFailNotEquals( "message", "a", "abc" ) );
            }

            /// <summary>
            /// Checks several common failure conditions to ensure the output
            /// strings match the expected output when newlines are in the
            /// string.  Newlines (CR or LF) characters are rendered in the
            /// output string as "\"+"r" or "\"+"n", so that they can be
            /// seen, and also to preserve the alignment of the output
            /// position differences.  Without this, the lines will wrap
            /// when displayed, and the differences marker will not align
            /// where the differences actually are.
            /// </summary>
			[Test]
			public void TestFormatMessageForFailNotEqualsNewlines()
            {
                Assertion.AssertEquals( "message \r\nString lengths differ.  Expected length=2, but was length=3.\r\nStrings differ at index 1.\r\n\r\nexpected:<a\\r>\r\n but was:<aa\\r>\r\n-----------^\r\n",
                    FormatMessageForFailNotEquals( "message", "a\r", "aa\r" ) );
                Assertion.AssertEquals( "message \r\nString lengths differ.  Expected length=2, but was length=3.\r\nStrings differ at index 1.\r\n\r\nexpected:<a\\n>\r\n but was:<aa\\n>\r\n-----------^\r\n",
                    FormatMessageForFailNotEquals( "message", "a\n", "aa\n" ) );
                Assertion.AssertEquals( "message \r\nString lengths are both 6.\r\nStrings differ at index 5.\r\n\r\nexpected:<aa\\r\\naa>\r\n but was:<aa\\r\\nab>\r\n-----------------^\r\n",
                    FormatMessageForFailNotEquals( "message", "aa\r\naa", "aa\r\nab" ) );
            }

            /// <summary>
            /// Test to track down a bug with string length differences
            /// One string has length 164, the other length 166.  Content is the
            /// same for both strings, but the longer string has 2 extra chars.
            /// </summary>
			[Test]
			public void TestStringLengthsDiffer()
            {
                // 2 strings, one length 166, the other length 164.  Indexes for each:
                // length 166: index=0...165
                // length 164: index=0...163

                // Assume content is same for all 164 common characters, and only the length
                // is different, so we want to show the length marker starting at position
                // 164

                // 111111111111111111111111111111
                // 444444444455555555556666666666
                // 012345678901234567890123456789
                // ...same content to here!++       length 166 has 2 extra chars
                // ...same content to here!         length 164
                // ------------------------^

                // On entry, the iPosition will be 164 since we start mismatching after
                // the first string ends (since all content is the same)

                // Need to clip appropriately when iPosition=164, and build characters
                // for the shorter string appropriately.  Longer string will be able
                // to be clipped up to position 164, but 164 will be past the end of
                // the short string.

                string sFirst  = "00000000001111111111222222222233333333334444444444555555555566666666667777777777888888888899999999990000000000111111111122222222223333333333444444444455555555556666";
                string sSecond = "00000000001111111111222222222233333333334444444444555555555566666666667777777777888888888899999999990000000000111111111122222222223333333333444444444455555555556666++";
                Assertion.AssertEquals( "\r\nString lengths differ.  Expected length=164, but was length=166.\r\nStrings differ at index 164.\r\n\r\nexpected:<...23333333333444444444455555555556666>\r\n but was:<...23333333333444444444455555555556666++>\r\n" + (new string('-',ButWasText().Length+"...".Length+PreClipLength)) + "^\r\n",
                    FormatMessageForFailNotEquals( null, sFirst, sSecond ) );
            }

            /// <summary>
            /// Test to verify conditions found by a bug, where strings have same
            /// content bug lengths are different.  Checking lengths shorter than
            /// clipping range, with 2 extra chars on longer string.
            /// </summary>
			[Test]
			public void TestStringLengthsDiffer2()
            {
                string sFirst  = "0000000000111111111122";
                string sSecond = "0000000000111111111122++";
                Assertion.AssertEquals( "\r\nString lengths differ.  Expected length=22, but was length=24.\r\nStrings differ at index 22.\r\n\r\nexpected:<0000000000111111111122>\r\n but was:<0000000000111111111122++>\r\n" + (new string('-',ButWasText().Length+22)) + "^\r\n",
                    FormatMessageForFailNotEquals( null, sFirst, sSecond ) );
            }

            /// <summary>
            /// Exhaustive test, all strings up to 200 characters in length,
            /// with one string 1, 2, 3, 4, or 5 characters longer than the other.
            /// 
            /// Verifies conditions found by a bug, that appears when strings have same
            /// content bug lengths are different.
            /// </summary>
			[Test]
			public void TestStringLengthsDiffer3()
            {
                VerifySameContentDifferentLengths( "+" );
                VerifySameContentDifferentLengths( "++" );
                VerifySameContentDifferentLengths( "+++" );
                VerifySameContentDifferentLengths( "++++" );
                VerifySameContentDifferentLengths( "+++++" );
            }

            private void VerifySameContentDifferentLengths( string sExtra )
            {
                //
                // Makes sure N-longer is ok just up to the point where
                // clipping would occur
                //
                for( int i=1; i<=PreClipLength; i++ )
                {
                    string sFirst  = new string( '=', i );
                    string sSecond = new string( '=', i ) + sExtra;
                    Assertion.AssertEquals( "Failed at index " + i, "\r\nString lengths differ.  Expected length=" + i + ", but was length=" + (i+sExtra.Length) + ".\r\nStrings differ at index "+ i +".\r\n\r\nexpected:<"+ sFirst +">\r\n but was:<"+ sSecond +">\r\n" + (new string('-',ButWasText().Length+i)) + "^\r\n",
                        FormatMessageForFailNotEquals( null, sFirst, sSecond ) );
                }

                //
                // Makes sure N-longer is ok from for strings equal in length
                // to the start of clipping and longer
                //
                string sExpected = "..." + new string( '=', PreClipLength );
                string sActual   = "..." + new string( '=', PreClipLength ) + sExtra;

                for( int i=PreClipLength+1; i<200; i++ )
                {
                    string sFirst  = new string( '=', i );
                    string sSecond = new string( '=', i ) + sExtra;
                    Assertion.AssertEquals( "Failed at index " + i, "\r\nString lengths differ.  Expected length=" + i + ", but was length=" + (i+sExtra.Length) + ".\r\nStrings differ at index "+ i +".\r\n\r\nexpected:<"+ sExpected +">\r\n but was:<"+ sActual +">\r\n" + (new string('-',ButWasText().Length+"...".Length+PreClipLength)) + "^\r\n",
                        FormatMessageForFailNotEquals( null, sFirst, sSecond ) );
                }
            }
        }
    }
}
