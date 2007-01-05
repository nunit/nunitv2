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
			textBoxWriter = new TextBoxWriter( textBox );
			textBox.CreateControl();
		}

		[Test]
		public void CreateWriter()
		{
			Assert.IsNotNull( textBoxWriter );
			Assert.AreEqual( 0, textBox.Lines.Length );
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

		[Test]//, Platform(Exclude="Mono")]
		public void WriteLines()
		{
			WriteTestLines( 5 );
			Assert.AreEqual( "This is line 1\nThis is line 2\nThis is line 3\nThis is line 4\nThis is line 5\n", textBox.Text );
			Assert.AreEqual( "This is line 3", textBox.Lines[2] );
		}

		[Test]//, Platform(Exclude="Mono")]
		public void Write()
		{
			textBoxWriter.Write( "I wrote this" );
			textBoxWriter.Write( " in three parts" );
			textBoxWriter.Write( '!' );

			Assert.AreEqual( "I wrote this in three parts!", textBox.Lines[0] );
		}

		[Test]//, Platform(Exclude="Mono")]
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
