using System;
using System.Windows.Forms;
using NUnit.UiKit;
using NUnit.Framework;

namespace NUnit.Tests.UiKit
{
	[TestFixture]
	public class TextDisplayAreaTests
	{
		private TextDisplayArea textArea;
//		private Panel panel;

		[SetUp]
		public void Init()
		{
			textArea = new TextDisplayArea();

//			foreach( Control control in textView.Controls )
//			{
//				if ( control is Panel )
//					panel = control as Panel;
//			}
		}

		private void WriteTestLines( int count, int start )
		{
			for( int index = start; index < start + count; ++index )
				textArea.WriteLine( string.Format( "This is line {0}", index ) );
		}

		private void WriteTestLines( int count )
		{
			WriteTestLines( count, 1 );
		}

		private int StringWidth( string s )
		{
			return (int)textArea.CreateGraphics().MeasureString( s, textArea.Font ).Width;
		}

		private Control GetControl( Type type )
		{
			return null;
		}

		[Test]
		public void CreateControl()
		{
			Assert.IsNotNull( textArea );
			Assert.AreEqual( 0, textArea.LineCount );
		}

		[Test]
		public void WriteLine()
		{
			WriteTestLines( 5 );
			Assert.AreEqual( 5, textArea.LineCount );
			Assert.AreEqual( "This is line 2", textArea.GetLine( 1 ) );
		}

		[Test]
		public void Clear()
		{
			WriteTestLines( 5 );
			textArea.Clear();
			Assert.AreEqual( 0, textArea.LineCount );
		}

		[Test]
		public void VirtualHeight()
		{
			int expectedHeight = 100 * textArea.Font.Height;
			WriteTestLines( 100 );
			Assert.AreEqual( expectedHeight, textArea.VirtualSize.Height );
//			Assert.AreEqual( expectedHeight, panel.AutoScrollMinSize.Height );
		}

		[Test]
		public void ReallyBigVirtualHeight()
		{
			int expectedHeight = 10000 * textArea.Font.Height;
			WriteTestLines( 10000 );
			Assert.AreEqual( expectedHeight, textArea.VirtualSize.Height );
//			Assert.AreEqual( expectedHeight, panel.AutoScrollMinSize.Height );
		}

		[Test]
		public void VirtualWidth()
		{
			string longLine = "This is line 6, which is longer than the rest";
			int expectedWidth = StringWidth( longLine );

			WriteTestLines( 5 );
			textArea.WriteLine( longLine );
			WriteTestLines( 5, 7 );

			Assert.AreEqual( expectedWidth, textArea.VirtualSize.Width );
//			Assert.AreEqual( expectedWidth, panel.AutoScrollMinSize.Width );
		}

		[Test]
		public void Write()
		{
			int width = StringWidth( "I wrote this in three parts!" );
			int height = textArea.Font.Height;

			textArea.Write( "I wrote this" );
			textArea.Write( " in three parts" );
			textArea.Write( '!' );

			Assert.AreEqual( 1, textArea.LineCount );
			Assert.AreEqual( "I wrote this in three parts!", textArea.GetLine( 0 ) );
			Assert.AreEqual( height, textArea.VirtualSize.Height );
			Assert.AreEqual( width, textArea.VirtualSize.Width );
		}

		[Test]
		public void MixedWrites()
		{
			WriteTestLines( 5 );
			textArea.Write( "This line written" );
			textArea.WriteLine( " in two parts" );
			textArea.WriteLine( "The final line" );

			int width = StringWidth( "This line written in two parts" );
			int height = 7 * textArea.Font.Height;

			Assert.AreEqual( 7, textArea.LineCount );
			Assert.AreEqual( "This line written in two parts", textArea.GetLine( 5 ) );
			Assert.AreEqual( "The final line", textArea.GetLine( 6 ) );
			Assert.AreEqual( width, textArea.VirtualSize.Width );
			Assert.AreEqual( height, textArea.VirtualSize.Height );
		}
	}
}
