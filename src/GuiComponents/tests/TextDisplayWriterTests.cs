// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using NUnit.Framework;

namespace NUnit.UiKit.Tests
{
	[TestFixture]
	public class TextDisplayWriterTestsUsingSimpleTextDisplay : TextDisplayWriterTests
	{
		protected override object createDisplayObject()
		{
			return new SimpleTextDisplay();
		}
	}

	[TestFixture]
	public class TextDisplayWriterTestsUsingTextBoxDisplay : TextDisplayWriterTests
	{
		protected override object createDisplayObject()
		{
			return new TextBoxDisplay();
		}
	}

	public abstract class TextDisplayWriterTests
	{
		protected TextDisplay textDisplay;
		protected TextDisplayWriter textDisplayWriter;

		[SetUp]
		public void Init()
		{
			textDisplay = (TextDisplay)createDisplayObject();
			textDisplayWriter = new TextDisplayWriter( textDisplay );
		}

		[TearDown]
		public void CleanUp()
		{
			textDisplayWriter.Close();
			IDisposable display = textDisplay as IDisposable;
			if ( display != null )
				display.Dispose();
		}

		protected abstract object createDisplayObject();

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
				textDisplay.Text.Replace("\r\n", "\n") );
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
				textDisplay.Text.Replace("\r\n","\n" ) );
		}
	}
}
