using System;
using System.Drawing;
using System.Windows.Forms;
using NUnit.Framework;

namespace NUnit.UiKit.Tests
{
	/// <summary>
	/// Summary description for SimpleTextDisplayTests.
	/// </summary>
	[TestFixture]
	public class SimpleTextDisplayTests
	{
		SimpleTextDisplay textDisplay;

		[SetUp]
		public void Init()
		{
			textDisplay = new SimpleTextDisplay(); 
		}

		[TearDown]
		public void CleanUp()
		{
			textDisplay.Dispose();
		}

		private void AppendLines( int count )
		{
			for( int index = 1; index <= count; ++index )
				textDisplay.AppendText( string.Format( "This is line {0}\n", index ) );
		}

		private Size getTextSize( string text )
		{
			return Graphics.FromHwnd( textDisplay.Handle ).MeasureString(text, textDisplay.Font ).ToSize();
		}

		[Test]
		public void SetText_BeforeCreation()
		{
			string myText = "Here is one line\nHere is another\n";
			textDisplay.Text = myText;
			Assert.AreEqual( myText, textDisplay.Text );
			Assert.AreEqual( Size.Empty, textDisplay.AutoScrollMinSize );
			textDisplay.CreateControl();
			Assert.AreEqual( myText, textDisplay.Text );
			Assert.AreEqual( getTextSize( myText ), textDisplay.AutoScrollMinSize );
		}

		[Test]
		public void SetText_AfterCreation()
		{
			textDisplay.CreateControl();
			string myText = "Here is one line\nHere is another\n";
			textDisplay.Text = myText;
			Assert.AreEqual( myText, textDisplay.Text );
			Assert.AreEqual( getTextSize( myText ), textDisplay.AutoScrollMinSize );
		}

		[Test]
		public void ClearText_BeforeCreation()
		{
			textDisplay.Text = "text that should be cleared";
			textDisplay.Clear();

			Assert.AreEqual( "", textDisplay.Text );
			Assert.AreEqual( Size.Empty, textDisplay.AutoScrollMinSize );
		}

		[Test]
		public void ClearText_AfterCreation()
		{
			textDisplay.Text = "text that should be cleared";
			textDisplay.CreateControl();
			textDisplay.Clear();

			Assert.AreEqual( "", textDisplay.Text );
			Assert.AreEqual( Size.Empty, textDisplay.AutoScrollMinSize );
		}

		[Test]
		public void AppendText_BeforeCreation()
		{
			AppendLines( 5 );
			textDisplay.AppendText( "This line written" );
			textDisplay.AppendText( " in two parts\n" );
			textDisplay.AppendText( "The final line\n" );

			string expected =
				"This is line 1\nThis is line 2\nThis is line 3\nThis is line 4\nThis is line 5\n" +
				"This line written in two parts\nThe final line\n";
			Assert.AreEqual( expected, textDisplay.Text );
			Assert.AreEqual( Size.Empty, textDisplay.AutoScrollMinSize );
		}

		[Test]
		public void AppendText_AfterCreation()
		{
			textDisplay.CreateControl();
			AppendLines( 5 );
			Assert.AreEqual( getTextSize( textDisplay.Text ), textDisplay.AutoScrollMinSize );
			textDisplay.AppendText( "This line written" );
			Assert.AreEqual( getTextSize( textDisplay.Text ), textDisplay.AutoScrollMinSize );
			textDisplay.AppendText( " in three" );
			Assert.AreEqual( getTextSize( textDisplay.Text ), textDisplay.AutoScrollMinSize );
			textDisplay.AppendText( " parts\n" );
			Assert.AreEqual( getTextSize( textDisplay.Text ), textDisplay.AutoScrollMinSize );
			textDisplay.AppendText( "The final line\n" );
			Assert.AreEqual( getTextSize( textDisplay.Text ), textDisplay.AutoScrollMinSize );

			string expected =
				"This is line 1\nThis is line 2\nThis is line 3\nThis is line 4\nThis is line 5\n" +
				"This line written in three parts\nThe final line\n";
			Assert.AreEqual( expected, textDisplay.Text );
			Assert.AreEqual( getTextSize( expected ), textDisplay.AutoScrollMinSize );
		}

		[Test]
		public void AppendText_BeforeAndAfterCreation()
		{
			AppendLines( 5 );
			textDisplay.AppendText( "This line written" );
			textDisplay.CreateControl();
			textDisplay.AppendText( " in two parts\n" );
			textDisplay.AppendText( "The final line\n" );

			string expected =
				"This is line 1\nThis is line 2\nThis is line 3\nThis is line 4\nThis is line 5\n" +
				"This line written in two parts\nThe final line\n";
			Assert.AreEqual( expected, textDisplay.Text );
			Assert.AreEqual( getTextSize( expected ), textDisplay.AutoScrollMinSize );
		}

		[Test]
		public void StressTest()
		{
			textDisplay.CreateControl();
			DateTime startTime = DateTime.Now;
			AppendLines( 1000 );
			DateTime endTime = DateTime.Now;
			Assert.AreEqual( 9*15 + 90*16 + 900*17 + 18, textDisplay.Text.Length );
			Assert.AreEqual( getTextSize( textDisplay.Text ), textDisplay.AutoScrollMinSize );
		}
	}
}
