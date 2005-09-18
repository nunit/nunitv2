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

namespace NUnit.Framework.Tests
{
    using System;
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
				Assert.AreEqual( @"
	expected: <(null)>
	 but was: <(null)>", 
					GetMsg( null, null, null ) );

				Assert.AreEqual( @"
	expected: <(null)>
	 but was: <(null)>", 
					GetMsg(null, null,  "") );

				Assert.AreEqual( @"message
	expected: <(null)>
	 but was: <(null)>", 
					GetMsg(null, null, "message" ) );

				Assert.AreEqual( @"message
	expected: <1>
	 but was: <2>", 
					GetMsg( 1, 2, "message" ) );

				Assert.AreEqual( @"message
	expected: <"""">
	 but was: <(null)>", 
					GetMsg( "", null, "message" ) );

				Assert.AreEqual( @"message
	expected: <1>
	 but was: <""1"">",
					GetMsg( 1, "1", "message" ) );

				Assert.AreEqual( @"message 5
	expected: <1>
	 but was: <""1"">",
					GetMsg( 1, "1", "message {0}", 5 ) );
			
				Assert.AreEqual( @"message
	String lengths differ.  Expected length=1, but was length=2.
	Strings differ at index 1.
	expected: <""a"">
	 but was: <""aa"">
	-------------^",
					GetMsg( "a", "aa", "message" ) );

				Assert.AreEqual( @"message
	String lengths are both 2.
	Strings differ at index 1.
	expected: <""aa"">
	 but was: <""ab"">
	-------------^",
					GetMsg( "aa", "ab", "message" ) );

				Assert.AreEqual( @"message
	String lengths differ.  Expected length=1, but was length=3.
	Strings differ at index 1.
	expected: <""a"">
	 but was: <""abc"">
	-------------^",
					GetMsg( "a", "abc", "message" ) );

				Assert.AreEqual( @"message
	String lengths are both 3.
	Strings differ at index 1.
	expected: <""123"">
	 but was: <""1x3"">
	-------------^",
					GetMsg( "123", "1x3", "message" ) );

				Assert.AreEqual( @"message
	String lengths differ.  Expected length=5, but was length=3.
	Strings differ at index 3.
	expected: <""12345"">
	 but was: <""123"">
	---------------^",
					GetMsg( "12345", "123", "message" ) );

				Assert.AreEqual( @"message
	expected: <" + new DateTime(2005, 6, 1, 0, 0, 0) + @">
	 but was: <" + new DateTime(2005, 6, 7, 0, 0, 0) + ">",
					GetMsg( new DateTime( 2005, 6, 1 ), new DateTime( 2005, 6, 7 ), "message" ) );
			}

			[Test]
			public void TestFormatMessageForFailNotEqualsIgnoringCase()
			{
				AssertionFailureMessage msg = new AssertionFailureMessage( "message" );
				msg.DisplayDifferences( "Name", "NAMES", true );

				Assert.AreEqual( @"message
	String lengths differ.  Expected length=4, but was length=5.
	Strings differ at index 4.
	expected: <""Name"">
	 but was: <""NAMES"">
	----------------^",
					msg.ToString() );
			}

			private string GetMsg( object expected, object actual, string message, params object[] args )
			{
				AssertionFailureMessage msg = new AssertionFailureMessage( message, args );
				msg.DisplayDifferences( expected, actual, false );
				return msg.ToString();
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
				Assert.AreEqual( @"message
	String lengths differ.  Expected length=2, but was length=3.
	Strings differ at index 1.
	expected: <""a\r"">
	 but was: <""aa\r"">
	-------------^",
					GetMsg( "a\r", "aa\r", "message" ) );

				Assert.AreEqual( @"message
	String lengths differ.  Expected length=2, but was length=3.
	Strings differ at index 1.
	expected: <""a\n"">
	 but was: <""aa\n"">
	-------------^",
					GetMsg( "a\n", "aa\n", "message" ) );

				Assert.AreEqual( @"message
	String lengths are both 6.
	Strings differ at index 5.
	expected: <""aa\r\naa"">
	 but was: <""aa\r\nab"">
	-------------------^",
					GetMsg( "aa\r\naa", "aa\r\nab", "message" ) );
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
				Assert.AreEqual( @"
	String lengths differ.  Expected length=164, but was length=166.
	Strings differ at index 164.
	expected: <""...23333333333444444444455555555556666"">
	 but was: <""...23333333333444444444455555555556666++"">
	--------------------------------------------------^",
					GetMsg( sFirst, sSecond, null ) );
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
				Assert.AreEqual( @"
	String lengths differ.  Expected length=22, but was length=24.
	Strings differ at index 22.
	expected: <""0000000000111111111122"">
	 but was: <""0000000000111111111122++"">
	----------------------------------^",
					GetMsg( sFirst, sSecond, null ) );
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
					Assert.AreEqual( "\r\n\tString lengths differ.  Expected length=" + i + ", but was length=" + (i+sExtra.Length) + ".\r\n\tStrings differ at index "+ i +".\r\n\texpected: <\""+ sFirst +"\">\r\n\t but was: <\""+ sSecond +"\">\r\n\t" + (new string('-',actualPrefix.Length+i+3)) + "^",
						GetMsg( sFirst, sSecond, null ),
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
					Assert.AreEqual( "\r\n\tString lengths differ.  Expected length=" + i + ", but was length=" + (i+sExtra.Length) + ".\r\n\tStrings differ at index "+ i +".\r\n\texpected: <\""+ sExpected +"\">\r\n\t but was: <\""+ sActual +"\">\r\n\t" + (new string('-',actualPrefix.Length+"...".Length+PreClipLength+3)) + "^",
						GetMsg( sFirst, sSecond, null ),
						"Failed at index " + i );
				}
			}

			[Test]
			public void TestFormatMessageForArraysNotEqual()
			{
				AssertionFailureMessage msg = new AssertionFailureMessage( "message" );
				msg.DisplayArrayDifferences( 
					new object[] { "one", "two", "three" },
					new object[] { "one", "two", "three", "four", "five" },
					3 );

				Assert.AreEqual( @"message
Array lengths differ.  Expected length=3, but was length=5.
Arrays differ at index 3.
   extra:<<""four"">,<""five"">>",
					msg.ToString() );

				msg = new AssertionFailureMessage( "message" );
				msg.DisplayArrayDifferences( 
					new object[] { "one", "two", "three", "four", "five" },
					new object[] { "one", "two", "three" },
					3 );

				Assert.AreEqual( @"message
Array lengths differ.  Expected length=5, but was length=3.
Arrays differ at index 3.
 missing:<<""four"">,<""five"">>",
					msg.ToString() );

				msg = new AssertionFailureMessage( "message" );
				msg.DisplayArrayDifferences( 
					new object[] { "one", "two", "three" },
					new object[] { "one", "two", "ten" },
					2 );

				Assert.AreEqual( @"message
Array lengths are both 3.
Arrays differ at index 2.
	String lengths differ.  Expected length=5, but was length=3.
	Strings differ at index 1.
	expected: <""three"">
	 but was: <""ten"">
	-------------^",
					msg.ToString() );

				msg = new AssertionFailureMessage( "message" );
				msg.DisplayArrayDifferences( 
					new object[] { 1, 2, 3 },
					new object[] { 1, 2, 10 },
					2 );

				Assert.AreEqual( @"message
Array lengths are both 3.
Arrays differ at index 2.
	expected: <3>
	 but was: <10>",
					msg.ToString() );

			}
		}
    }
}
