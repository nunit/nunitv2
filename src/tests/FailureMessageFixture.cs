#region Copyright (c) 2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright  2000-2002 Philip A. Craig
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
' Portions Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright  2000-2002 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

namespace NUnit.Tests.Assertions
{
    using System;
    using NUnit.Framework;
	using System.Collections;

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
        public class FailureMessageFixture
        {
            /// <summary>
            /// 
            /// </summary>
            /// 
			[Test]
            public void TestInputsAreStrings()
            {
                Assert.AreEqual( false, InputsAreStrings( null, null ) );
                Assert.AreEqual( false, InputsAreStrings( new Object(), null ) );
                Assert.AreEqual( false, InputsAreStrings( null, new Object() ) );
                Assert.AreEqual( false, InputsAreStrings( new Object(), "" ) );
                Assert.AreEqual( false, InputsAreStrings( "", new Object() ) );
                Assert.AreEqual( true,  InputsAreStrings( "", "" ) );
                Assert.AreEqual( true,  InputsAreStrings( "test1", "test1" ) );
                Assert.AreEqual( true,  InputsAreStrings( "some", "value" ) );
            }

            /// <summary>
            /// 
            /// </summary>
            /// 
			[Test]
            public void TestCreateStringBuilder()
            {
                Assert.AreEqual( "",        CreateStringBuilder( null ).ToString() );
                Assert.AreEqual( "",        CreateStringBuilder( "" ).ToString() );
                Assert.AreEqual( "a",       CreateStringBuilder( "a" ).ToString() );
                Assert.AreEqual( "message", CreateStringBuilder( "message" ).ToString() );
				Assert.AreEqual( "message 5 from me", 
					CreateStringBuilder( "message {0} from {1}", 5, "me" ).ToString() );
            }

            /// <summary>
            /// Tests the string-clipping method to see if it clips at the
            /// expected positions.
            /// </summary>
			[Test]
			public void TestClipAroundPosition()
            {
                // Some spot-checks
                Assert.AreEqual( "", ClipAroundPosition( null, 0 ) );
                Assert.AreEqual( "", ClipAroundPosition( "", 0 ) );
                Assert.AreEqual( "a", ClipAroundPosition( "a", 0 ) );
                Assert.AreEqual( "ab", ClipAroundPosition( "ab", 0 ) );
                Assert.AreEqual( "ab", ClipAroundPosition( "ab", 1 ) );
                Assert.AreEqual( "abc", ClipAroundPosition( "abc", 0 ) );
                Assert.AreEqual( "abc", ClipAroundPosition( "abc", 1 ) );
                Assert.AreEqual( "abc", ClipAroundPosition( "abc", 2 ) );

                Assert.AreEqual( "012345678", ClipAroundPosition( "012345678", 0 ) );
                Assert.AreEqual( "012345678", ClipAroundPosition( "012345678", 1 ) );
                Assert.AreEqual( "012345678", ClipAroundPosition( "012345678", 2 ) );
                Assert.AreEqual( "012345678", ClipAroundPosition( "012345678", 7 ) );
                Assert.AreEqual( "012345678", ClipAroundPosition( "012345678", 8 ) );

                Assert.AreEqual( "0123456789", ClipAroundPosition( "0123456789", 0 ) );
                Assert.AreEqual( "0123456789", ClipAroundPosition( "0123456789", 1 ) );
                Assert.AreEqual( "0123456789", ClipAroundPosition( "0123456789", 2 ) );
                Assert.AreEqual( "0123456789", ClipAroundPosition( "0123456789", 7 ) );
                Assert.AreEqual( "0123456789", ClipAroundPosition( "0123456789", 8 ) );
                Assert.AreEqual( "0123456789", ClipAroundPosition( "0123456789", 9 ) );

                Assert.AreEqual( "01234567890", ClipAroundPosition( "01234567890", 0 ) );
                Assert.AreEqual( "01234567890", ClipAroundPosition( "01234567890", 1 ) );
                Assert.AreEqual( "01234567890", ClipAroundPosition( "01234567890", 2 ) );
                Assert.AreEqual( "01234567890", ClipAroundPosition( "01234567890", 7 ) );
                Assert.AreEqual( "01234567890", ClipAroundPosition( "01234567890", 8 ) );
                Assert.AreEqual( "01234567890", ClipAroundPosition( "01234567890", 9 ) );
                Assert.AreEqual( "01234567890", ClipAroundPosition( "01234567890", 10 ) );
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
                    Assert.AreEqual( sTest.Substring( 0, i+PostClipLength ) + "...",
                        ClipAroundPosition( sTest, iTestPosition ) );
                }

                // Want pre- and post-clipping when position is after
                // first 10 digits and before last 40 digits
                for( int i=PreClipLength+1; i<(sTest.Length - PostClipLength); i++ )
                {
                    iTestPosition = i;
                    Assert.AreEqual( "..." + sTest.Substring( iTestPosition-PreClipLength, PreClipLength+PostClipLength ) + "...",
                        ClipAroundPosition( sTest, iTestPosition ) );
                }

                // Want pre-clipping and no post-clipping when position is within 
                // last 40 digits
                for( int i=(sTest.Length - PostClipLength); i<sTest.Length; i++ )
                {
                    iTestPosition = i;
                    Assert.AreEqual( "..." + sTest.Substring( iTestPosition-PreClipLength ),
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
                Assert.AreEqual( "\\n", ConvertWhitespace("\n") );
                Assert.AreEqual( "\\n\\n", ConvertWhitespace("\n\n") );
                Assert.AreEqual( "\\n\\n\\n", ConvertWhitespace("\n\n\n") );

                Assert.AreEqual( "\\r", ConvertWhitespace("\r") );
                Assert.AreEqual( "\\r\\r", ConvertWhitespace("\r\r") );
                Assert.AreEqual( "\\r\\r\\r", ConvertWhitespace("\r\r\r") );

                Assert.AreEqual( "\\r\\n", ConvertWhitespace("\r\n") );
                Assert.AreEqual( "\\n\\r", ConvertWhitespace("\n\r") );
                Assert.AreEqual( "This is a\\rtest message", ConvertWhitespace("This is a\rtest message") );

                Assert.AreEqual( "", ConvertWhitespace("") );
                Assert.AreEqual( null, ConvertWhitespace(null) );
                
                Assert.AreEqual( "\\t", ConvertWhitespace( "\t" ) );
                Assert.AreEqual( "\\t\\n", ConvertWhitespace( "\t\n" ) );
            }

            /// <summary>
            /// Checks several common failure conditions to ensure the output
            /// strings match the expected output.
            /// </summary>
			[Test]
			public void TestFormatMessageForFailNotEquals()
            {
                Assert.AreEqual( "\r\n\texpected:<(null)>\r\n\t but was:<(null)>", 
                    FormatMessageForFailNotEquals( null, null, null ) );
                Assert.AreEqual( "\r\n\texpected:<(null)>\r\n\t but was:<(null)>", 
                    FormatMessageForFailNotEquals(null, null,  "") );
                Assert.AreEqual( "message \r\n\texpected:<(null)>\r\n\t but was:<(null)>", 
                    FormatMessageForFailNotEquals(null, null, "message" ) );
                Assert.AreEqual( "message \r\n\texpected:<1>\r\n\t but was:<2>", 
                    FormatMessageForFailNotEquals( 1, 2, "message" ) );
                Assert.AreEqual( "message \r\n\texpected:<\"\">\r\n\t but was:<(null)>", 
                    FormatMessageForFailNotEquals( "", null, "message" ) );
				Assert.AreEqual( "message \r\n\texpected:<1>\r\n\t but was:<\"1\">",
					FormatMessageForFailNotEquals( 1, "1", "message" ) );
				Assert.AreEqual( "message 5 \r\n\texpected:<1>\r\n\t but was:<\"1\">",
					FormatMessageForFailNotEquals( 1, "1", "message {0}", 5 ) );
			
				AnalyzeMessageForStrings( "a", "aa", "message" );
				AnalyzeMessageForStrings( "aa", "ab", "message" );
				AnalyzeMessageForStrings( "a", "abc", "message" );
				AnalyzeMessageForStrings( "123", "1x3", "message" );
				AnalyzeMessageForStrings( "12345", "123", "message" );
			}

			private void AnalyzeMessageForStrings( string expected, string actual, string message )
			{
				string[] lines = SplitMessage( 
					FormatMessageForFailNotEquals( expected, actual, message ) );
				string msg = string.Format( "Testing expected={0}, actual={1}", expected, actual );
				
				// First line should contain the user message
				Assert.AreEqual( message, lines[0], msg );

				// Second line compares the lengths
				Assert.AreEqual( 
					expected.Length == actual.Length ? string.Format( "\tString lengths are both {0}.", expected.Length )
					: string.Format( "\tString lengths differ.  Expected length={0}, but was length={1}.", expected.Length, actual.Length ),
					lines[1], msg );

				// Third line indicates the point of difference
				int index = 0;
				while ( index < expected.Length && index < actual.Length
					&& actual[index] == expected[index] )
						index++;
				Assert.AreEqual( string.Format( "\tStrings differ at index {0}.", index ), lines[2], msg );

				// Fourth line is empty
				Assert.AreEqual( string.Empty, lines[3], msg );
				
				// Fifth line gives the expected value
				Assert.AreEqual( string.Format( "\texpected:<\"{0}\">", expected, msg ), lines[4], msg );

				// Sixth line gives the actual value
				Assert.AreEqual( string.Format( "\t but was:<\"{0}\">", actual, msg ), lines[5], msg );

				// Seventh line contains dashes and a caret. The caret should point 
				// at the first nomatching character in the strings. This works
				// even though the lines may contain ellipses. The line should
				// contain a tab, dashes and the caret. We add 11 to match the
				// initial string "expected:" plus the opening quote.
				string caretLine = "\t" + new String( '-', index + 11 ) + "^";
				Assert.AreEqual( caretLine, lines[6] );
			}

			private string[] SplitMessage( string msg )
			{
				int index = 0;
				ArrayList lines = new ArrayList();
				while ( index < msg.Length )
				{
					int next = msg.IndexOf( "\r\n", index );
					
					if ( next < 0 )
						next = msg.Length;
					else
						next = next + 2;

					lines.Add( msg.Substring( index, next - index ).TrimEnd() );
					
					index = next;
				}

				return (string[])lines.ToArray( typeof( string ) );
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
                Assert.AreEqual( "message \r\n\tString lengths differ.  Expected length=2, but was length=3.\r\n\tStrings differ at index 1.\r\n\t\r\n\texpected:<\"a\\r\">\r\n\t but was:<\"aa\\r\">\r\n\t------------^\r\n\t",
                    FormatMessageForFailNotEquals( "a\r", "aa\r", "message" ) );
                Assert.AreEqual( "message \r\n\tString lengths differ.  Expected length=2, but was length=3.\r\n\tStrings differ at index 1.\r\n\t\r\n\texpected:<\"a\\n\">\r\n\t but was:<\"aa\\n\">\r\n\t------------^\r\n\t",
                    FormatMessageForFailNotEquals( "a\n", "aa\n", "message" ) );
                Assert.AreEqual( "message \r\n\tString lengths are both 6.\r\n\tStrings differ at index 5.\r\n\t\r\n\texpected:<\"aa\\r\\naa\">\r\n\t but was:<\"aa\\r\\nab\">\r\n\t------------------^\r\n\t",
                    FormatMessageForFailNotEquals( "aa\r\naa", "aa\r\nab", "message" ) );
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
                Assert.AreEqual( 
					"\r\n\tString lengths differ.  Expected length=164, but was length=166.\r\n\tStrings differ at index 164.\r\n\t\r\n\texpected:<\"...23333333333444444444455555555556666\">\r\n\t but was:<\"...23333333333444444444455555555556666++\">\r\n\t" + (new string('-',ButWasText().Length+"...".Length+PreClipLength+1)) + "^\r\n\t",
                    FormatMessageForFailNotEquals( sFirst, sSecond, null ) );
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
                Assert.AreEqual( "\r\n\tString lengths differ.  Expected length=22, but was length=24.\r\n\tStrings differ at index 22.\r\n\t\r\n\texpected:<\"0000000000111111111122\">\r\n\t but was:<\"0000000000111111111122++\">\r\n\t" + (new string('-',ButWasText().Length+23)) + "^\r\n\t",
                    FormatMessageForFailNotEquals( sFirst, sSecond, null ) );
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
                    Assert.AreEqual( "\r\n\tString lengths differ.  Expected length=" + i + ", but was length=" + (i+sExtra.Length) + ".\r\n\tStrings differ at index "+ i +".\r\n\t\r\n\texpected:<\""+ sFirst +"\">\r\n\t but was:<\""+ sSecond +"\">\r\n\t" + (new string('-',ButWasText().Length+i+1)) + "^\r\n\t",
                        FormatMessageForFailNotEquals( sFirst, sSecond, null ),
						"Failed at index " + i);
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
                    Assert.AreEqual( "\r\n\tString lengths differ.  Expected length=" + i + ", but was length=" + (i+sExtra.Length) + ".\r\n\tStrings differ at index "+ i +".\r\n\t\r\n\texpected:<\""+ sExpected +"\">\r\n\t but was:<\""+ sActual +"\">\r\n\t" + (new string('-',ButWasText().Length+"...".Length+PreClipLength+1)) + "^\r\n\t",
                        FormatMessageForFailNotEquals( sFirst, sSecond, null ),
						"Failed at index " + i );
                }
            }
        }
    }
}
