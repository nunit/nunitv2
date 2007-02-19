// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org/?p=license&r=2.4.
// ****************************************************************

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

				Assert.AreEqual( "\\\\r\\\\n", ConvertWhitespace( "\\r\\n" ) );
			}

			/// <summary>
			/// Checks several common failure conditions to ensure the output
			/// strings match the expected output.
			/// </summary>
			[Test]
			public void TestFormatMessageForFailNotEquals()
			{
				Assert.AreEqual( "	expected: <(null)>" + Environment.NewLine + 
					"	 but was: <(null)>" + Environment.NewLine, 
					GetMsg( null, null, null ) );

				Assert.AreEqual( 
					"	expected: <(null)>" + Environment.NewLine + 
					"	 but was: <(null)>" + Environment.NewLine, 
					GetMsg(null, null,  "") );

				Assert.AreEqual( "message" +
					Environment.NewLine + "	expected: <(null)>" +
					Environment.NewLine + "	 but was: <(null)>" + Environment.NewLine, 
					GetMsg(null, null, "message" ) );

				Assert.AreEqual( "message" +
					Environment.NewLine + "	expected: <1>" +
					Environment.NewLine + "	 but was: <2>" + Environment.NewLine, 
					GetMsg( 1, 2, "message" ) );

				Assert.AreEqual( "message" +
					Environment.NewLine + "	expected: <\"\">" +
					Environment.NewLine + "	 but was: <(null)>" + Environment.NewLine, 
					GetMsg( "", null, "message" ) );

				Assert.AreEqual( "message" +
					Environment.NewLine + "	expected: <1>" +
					Environment.NewLine + "	 but was: <\"1\">" + Environment.NewLine,
					GetMsg( 1, "1", "message" ) );

				Assert.AreEqual( "message 5" +
					Environment.NewLine + "	expected: <1>" +
					Environment.NewLine + "	 but was: <\"1\">" + Environment.NewLine,
					GetMsg( 1, "1", "message {0}", 5 ) );
			
				Assert.AreEqual( "message" +
					Environment.NewLine + "	String lengths differ.  Expected length=1, but was length=2." +
					Environment.NewLine + "	Strings differ at index 1." +
					Environment.NewLine + "	expected: <\"a\">" +
					Environment.NewLine + "	 but was: <\"aa\">" +
					Environment.NewLine + "	-------------^" + Environment.NewLine,
					GetMsg( "a", "aa", "message" ) );

				Assert.AreEqual( "message" +
					Environment.NewLine + "	String lengths are both 2." +
					Environment.NewLine + "	Strings differ at index 1." +
					Environment.NewLine + "	expected: <\"aa\">" +
					Environment.NewLine + "	 but was: <\"ab\">" +
					Environment.NewLine + "	-------------^" + Environment.NewLine,
					GetMsg( "aa", "ab", "message" ) );

				Assert.AreEqual( "message" +
					Environment.NewLine + "	String lengths differ.  Expected length=1, but was length=3." +
					Environment.NewLine + "	Strings differ at index 1." +
					Environment.NewLine + "	expected: <\"a\">" +
					Environment.NewLine + "	 but was: <\"abc\">" +
					Environment.NewLine + "	-------------^" + Environment.NewLine,
					GetMsg( "a", "abc", "message" ) );

				Assert.AreEqual( "message" +
					Environment.NewLine + "	String lengths are both 3." +
					Environment.NewLine + "	Strings differ at index 1." +
					Environment.NewLine + "	expected: <\"123\">" +
					Environment.NewLine + "	 but was: <\"1x3\">" +
					Environment.NewLine + "	-------------^" + Environment.NewLine,
					GetMsg( "123", "1x3", "message" ) );

				Assert.AreEqual( "message" +
					Environment.NewLine + "	String lengths differ.  Expected length=5, but was length=3." +
					Environment.NewLine + "	Strings differ at index 3." +
					Environment.NewLine + "	expected: <\"12345\">" +
					Environment.NewLine + "	 but was: <\"123\">" +
					Environment.NewLine + "	---------------^" + Environment.NewLine,
					GetMsg( "12345", "123", "message" ) );

				Assert.AreEqual( "message" +
					Environment.NewLine + "	expected: <" + new DateTime(2005, 6, 1, 0, 0, 0) + ">" +
					Environment.NewLine + "	 but was: <" + new DateTime(2005, 6, 7, 0, 0, 0) + ">" + Environment.NewLine,
					GetMsg( new DateTime( 2005, 6, 1 ), new DateTime( 2005, 6, 7 ), "message" ) );
			}

			[Test]
			public void TestFormatMessageForFailNotEqualsIgnoringCase()
			{
				AssertionFailureMessage msg = new AssertionFailureMessage( "message" );
				msg.DisplayDifferences( "Name", "NAMES", true );

				Assert.AreEqual( "message" +
					Environment.NewLine + "	String lengths differ.  Expected length=4, but was length=5." +
					Environment.NewLine + "	Strings differ at index 4." +
					Environment.NewLine + "	expected: <\"Name\">" +
					Environment.NewLine + "	 but was: <\"NAMES\">" +
					Environment.NewLine + "	----------------^" + Environment.NewLine,
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
			public void TestFormatMessageForFailNotEqualsNewlines2()
			{
				Assert.AreEqual( "message" +
					Environment.NewLine + "	String lengths differ.  Expected length=2, but was length=3." +
					Environment.NewLine + "	Strings differ at index 1." +
					Environment.NewLine + "	expected: <\"a\\r\">" +
					Environment.NewLine + "	 but was: <\"aa\\r\">" +
					Environment.NewLine + "	-------------^" + Environment.NewLine,
					GetMsg( "a\r", "aa\r", "message" ) );

				Assert.AreEqual( "message" +
					Environment.NewLine + "	String lengths differ.  Expected length=2, but was length=3." +
					Environment.NewLine + "	Strings differ at index 1." +
					Environment.NewLine + "	expected: <\"a\\n\">" +
					Environment.NewLine + "	 but was: <\"aa\\n\">" +
					Environment.NewLine + "	-------------^" + Environment.NewLine,
					GetMsg( "a\n", "aa\n", "message" ) );

				Assert.AreEqual( "message" +
					Environment.NewLine + "	String lengths are both 6." +
					Environment.NewLine + "	Strings differ at index 5." +
					Environment.NewLine + "	expected: <\"aa\\r\\naa\">" +
					Environment.NewLine + "	 but was: <\"aa\\r\\nab\">" +
					Environment.NewLine + "	-------------------^" + Environment.NewLine,
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
				Assert.AreEqual(
"	String lengths differ.  Expected length=164, but was length=166." + System.Environment.NewLine +
"	Strings differ at index 164." + System.Environment.NewLine + 
"	expected: <\"...23333333333444444444455555555556666\">" + System.Environment.NewLine + 
"	 but was: <\"...23333333333444444444455555555556666++\">" + System.Environment.NewLine + 
"	--------------------------------------------------^" + Environment.NewLine,
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
				Assert.AreEqual( 
					"	String lengths differ.  Expected length=22, but was length=24." + 
					Environment.NewLine + "	Strings differ at index 22." + 
					Environment.NewLine + "	expected: <\"0000000000111111111122\">" + 
					Environment.NewLine + "	 but was: <\"0000000000111111111122++\">" + 
					Environment.NewLine + "	----------------------------------^" + Environment.NewLine,
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
					Assert.AreEqual( 
						"\tString lengths differ.  Expected length=" + i + ", but was length=" + (i+sExtra.Length) + "." + System.Environment.NewLine + 
						"\tStrings differ at index "+ i +"." + System.Environment.NewLine + 
						"\texpected: <\""+ sFirst +"\">" + System.Environment.NewLine + 
						"\t but was: <\""+ sSecond +"\">" + System.Environment.NewLine + 
						"\t" + (new string('-',actualPrefix.Length+i+3)) + "^" + Environment.NewLine,
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
					Assert.AreEqual( 
						"\tString lengths differ.  Expected length=" + i + ", but was length=" + (i+sExtra.Length) + "." + System.Environment.NewLine + 
						"\tStrings differ at index "+ i +"." + System.Environment.NewLine + 
						"\texpected: <\""+ sExpected +"\">" + System.Environment.NewLine + 
						"\t but was: <\""+ sActual +"\">" + System.Environment.NewLine + 
						"\t" + (new string('-',actualPrefix.Length+"...".Length+PreClipLength+3)) + "^" + Environment.NewLine,
						GetMsg( sFirst, sSecond, null ),
						"Failed at index " + i );
				}
			}

			[Test]
			public void DisplayListElements()
			{
				AssertionFailureMessage msg = null;

				msg = new AssertionFailureMessage( "message");
				msg.DisplayListElements( "label:", new object[] { "a", "b", "c" }, 0, 3 );
				Assert.AreEqual( 
					"message" + System.Environment.NewLine + 
					"label:<<\"a\">,<\"b\">,<\"c\">>" + Environment.NewLine,
					msg.ToString() );

				msg = new AssertionFailureMessage( "message");
				msg.DisplayListElements( "label:", new object[] { "a", "b", "c" }, 0, 5 );
				Assert.AreEqual( 
					"message" + System.Environment.NewLine + 
					"label:<<\"a\">,<\"b\">,<\"c\">>" + Environment.NewLine,
					msg.ToString() );

				msg = new AssertionFailureMessage( "message");
				msg.DisplayListElements( "label:", new object[] { "a", "b", "c" }, 1, 1 );
				Assert.AreEqual( 
					"message" + System.Environment.NewLine + 
					"label:<<\"b\">,...>" + Environment.NewLine,
					msg.ToString() );

				msg = new AssertionFailureMessage( "message");
				msg.DisplayListElements( "label:", new object[] { "a", "b", "c" }, 1, 5 );
				Assert.AreEqual( 
					"message" + System.Environment.NewLine + 
					"label:<<\"b\">,<\"c\">>" + Environment.NewLine,
					msg.ToString() );

				msg = new AssertionFailureMessage( "message");
				msg.DisplayListElements( "label:", new object[0], 0, 5 );
				Assert.AreEqual( 
					"message" + System.Environment.NewLine + 
					"label:<empty>" + Environment.NewLine,
					msg.ToString() );


				msg = new AssertionFailureMessage( "message");
				msg.DisplayListElements( "label:", null, 0, 5 );
				Assert.AreEqual( 
					"message" + System.Environment.NewLine + 
					"label:<null>" + Environment.NewLine,
					msg.ToString() );
			}

			[Test]
			public void TestFormatMessageForArraysNotEqual()
			{
				AssertionFailureMessage msg = null;
				
				msg = new AssertionFailureMessage( "message" );
				msg.DisplayArrayDifferences( 
					new object[] { "one", "two", "three" },
					new object[] { "one", "two", "three", "four", "five" },
					3 );

				Assert.AreEqual( "message" +
					Environment.NewLine + "Array lengths differ.  Expected length=3, but was length=5." +
					Environment.NewLine + "Arrays differ at index 3." +
					Environment.NewLine + "   extra:<<\"four\">,<\"five\">>" + Environment.NewLine,
					msg.ToString() );

				msg = new AssertionFailureMessage( "message" );
				msg.DisplayArrayDifferences( 
					new object[] { "one", "two", "three", "four", "five" },
					new object[] { "one", "two", "three" },
					3 );

				Assert.AreEqual( "message" +
					Environment.NewLine + "Array lengths differ.  Expected length=5, but was length=3." +
					Environment.NewLine + "Arrays differ at index 3." +
					Environment.NewLine + " missing:<<\"four\">,<\"five\">>" + Environment.NewLine,
					msg.ToString() );

				msg = new AssertionFailureMessage( "message" );
				msg.DisplayArrayDifferences( 
					new object[] { "one", "two", "three" },
					new object[] { "one", "two", "ten" },
					2 );

				Assert.AreEqual( "message" +
					Environment.NewLine + "Array lengths are both 3." +
					Environment.NewLine + "Arrays differ at index 2." +
					Environment.NewLine + "	String lengths differ.  Expected length=5, but was length=3." +
					Environment.NewLine + "	Strings differ at index 1." +
					Environment.NewLine + "	expected: <\"three\">" +
					Environment.NewLine + "	 but was: <\"ten\">" +
					Environment.NewLine + "	-------------^" + Environment.NewLine,
					msg.ToString() );

				msg = new AssertionFailureMessage( "message" );
				msg.DisplayArrayDifferences( 
					new object[] { 1, 2, 3 },
					new object[] { 1, 2, 10 },
					2 );

				Assert.AreEqual( "message" +
					Environment.NewLine + "Array lengths are both 3." +
					Environment.NewLine + "Arrays differ at index 2." +
					Environment.NewLine + "	expected: <3>" +
					Environment.NewLine + "	 but was: <10>" + Environment.NewLine,
					msg.ToString() );

				msg = new AssertionFailureMessage( "message" );
				msg.DisplayArrayDifferences( 
					new object[,] { { 1, 2, 3 }, { 4, 5, 6 } },
					new object[,] { { 1, 2, 9 }, { 4, 5, 6 } },
					2 );

				Assert.AreEqual( "message" +
					Environment.NewLine + "Array lengths are both 6." +
					Environment.NewLine + "Arrays differ at index 2." +
					Environment.NewLine + "	expected: <3>" +
					Environment.NewLine + "	 but was: <9>" + Environment.NewLine,
					msg.ToString() );

			}
		}
    }
}
