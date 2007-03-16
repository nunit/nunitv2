// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using System.Windows.Forms;
using NUnit.Framework;

namespace NUnit.UiKit.Tests
{
	[TestFixture]
	public class TextBoxWriterTests
	{
		private TextBoxWriter textBoxWriter;
		private RichTextBox textBox;
		
		[SetUp]
		public void Init()
		{
			textBox = new RichTextBox();
			textBox.Multiline = true;
			textBoxWriter = new TextBoxWriter( textBox );
			textBox.CreateControl();
		}

		public void CleanUp()
		{
			textBox.Dispose();
		}

		[Test]
		public void CreateWriter()
		{
			Assert.IsNotNull( textBoxWriter );
			Assert.AreEqual( "", textBox.Text );
			Assert.AreEqual( 0, textBox.Lines.Length );
		}

		private void WriteTestLines( int count, int start )
		{
			for( int index = start; index < start + count; ++index )
				textBoxWriter.WriteLine( string.Format( "This is line {0}", index ) );
		}

		private void WriteTestLines( int count )
		{
			WriteTestLines( count, 1 );
		}

		[Test]
		public void WriteLines()
		{
			WriteTestLines( 5 );
			Assert.AreEqual( "This is line 3", textBox.Lines[2] );
		}

		[Test]
		public void Write()
		{
			textBoxWriter.Write( "I wrote this" );
			textBoxWriter.Write( " in three parts" );
			textBoxWriter.Write( '!' );
			
			Assert.AreEqual( "I wrote this in three parts!", textBox.Text );
		}

		[Test, Platform(Exclude="Mono", Reason="Mono 1.2.2 mixes up lines")]
		public void MixedWrites()
		{
			WriteTestLines( 5 );
			textBoxWriter.Write( "This line written" );
			textBoxWriter.WriteLine( " in two parts" );
			textBoxWriter.WriteLine( "The final line" );

			Assert.AreEqual( "This line written in two parts", textBox.Lines[5] );
			Assert.AreEqual( "The final line", textBox.Lines[6] );
		}
	}
}
