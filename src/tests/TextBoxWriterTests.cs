using System;
using System.Windows.Forms;
using NUnit.Framework;
using NUnit.UiKit;

namespace NUnit.Tests.UiKit
{
	[TestFixture]
	public class TextBoxWriterTests
	{
		private TextBoxWriter textBoxWriter;
		private RichTextBox textBox;
		private static readonly string testLine = "This is a test";
		
		[SetUp]
		public void Init()
		{
			textBox = new RichTextBox();
			textBoxWriter = new TextBoxWriter( textBox );
		}

		[Test]
		public void CreateWriter()
		{
			Assert.NotNull( textBoxWriter );
			Assert.Equals( 0, textBox.Lines.Length );
			Assert.Equals( "", textBox.Text );
			Assert.Equals( 0, textBox.Lines.Length );
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
			Assert.Equals( "This is line 3", textBox.Lines[2] );
		}

		[Test]
		public void Write()
		{
			textBoxWriter.Write( "I wrote this" );
			textBoxWriter.Write( " in three parts" );
			textBoxWriter.Write( '!' );

			Assert.Equals( "I wrote this in three parts!", textBox.Lines[0] );
		}

		[Test]
		public void MixedWrites()
		{
			WriteTestLines( 5 );
			textBoxWriter.Write( "This line written" );
			textBoxWriter.WriteLine( " in two parts" );
			textBoxWriter.WriteLine( "The final line" );

			Assert.Equals( "This line written in two parts", textBox.Lines[5] );
			Assert.Equals( "The final line", textBox.Lines[6] );
		}
	}
}
