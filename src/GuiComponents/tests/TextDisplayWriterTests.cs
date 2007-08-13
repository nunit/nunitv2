using System;
using NUnit.Framework;

namespace NUnit.UiKit.Tests
{
	/// <summary>
	/// Summary description for TextDisplayWriterTests.
	/// </summary>
	[TestFixture]
	public class TextDisplayWriterTests
	{
		private TextDisplayWriter textDisplayWriter;
		private SimpleTextDisplay textDisplay;
		
		[SetUp]
		public void Init()
		{
			textDisplay = new SimpleTextDisplay();
			textDisplayWriter = new TextDisplayWriter( textDisplay );
		}

		[TearDown]
		public void CleanUp()
		{
			textDisplayWriter.Close();
			textDisplay.Dispose();
		}

		private void WriteTestLines( int count )
		{
			for( int index = 1; index <= count; ++index )
				textDisplayWriter.WriteLine( string.Format( "This is line {0}", index ) );
		}

		[Test]
		public void WriteLines()
		{
			WriteTestLines( 5 );
			Assert.AreEqual(
				"This is line 1\nThis is line 2\nThis is line 3\nThis is line 4\nThis is line 5\n",
				textDisplay.Text );
		}

		[Test]
		public void Write()
		{
			textDisplayWriter.Write( "I wrote this" );
			textDisplayWriter.Write( " in three parts" );
			textDisplayWriter.Write( '!' );
			
			Assert.AreEqual( "I wrote this in three parts!", textDisplay.Text );
		}

		[Test]
		public void MixedWrites()
		{
			WriteTestLines( 5 );
			textDisplayWriter.Write( "This line written" );
			textDisplayWriter.WriteLine( " in two parts" );
			textDisplayWriter.WriteLine( "The final line" );

			Assert.AreEqual(
				"This is line 1\nThis is line 2\nThis is line 3\nThis is line 4\nThis is line 5\n" +
				"This line written in two parts\nThe final line\n",
				textDisplay.Text );
		}
	}
}
