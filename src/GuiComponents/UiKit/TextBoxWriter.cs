#region Copyright (c) 2002, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov
' Copyright © 2000-2002 Philip A. Craig
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
' Portions Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov 
' or Copyright © 2000-2002 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

namespace NUnit.UiKit
{
	using System;
	using System.IO;
	using System.Windows.Forms;
	using System.Text;

	/// <summary>
	/// TextBoxWriter translates a series of writes to a TextBox display.
	/// Note that no locking is performed because this class is only written 
	/// to by BufferedStringTextWriter which does locking. However the 
	/// class does do an extra level of buffering in the case where the 
	/// control has not yet been created at the time of the Write().
	/// </summary>
	public class TextBoxWriter : TextWriter
	{
		private TextBoxBase textBox;
		private StringBuilder sb;
    			
		public TextBoxWriter(TextBox textBox)
		{
			this.textBox = textBox;
			textBox.HandleCreated += new EventHandler( OnHandleCreated );
		}
    			
		public TextBoxWriter(RichTextBox textBox)
		{
			this.textBox = textBox;
			textBox.HandleCreated += new EventHandler( OnHandleCreated );
		}

		public override void Write(char c)
		{
			Write( c.ToString() );
		}

		//		private delegate void TextAppender( string s );
		//
		//		private void AppendText( string s )
		//		{
		//			textBox.AppendText( s );
		//		}
		//
		//		public override void Write(String s)
		//		{
		//			textBox.Invoke( new TextAppender( AppendText ) );
		//		}

		public override void Write(String s)
		{
			if ( textBox.IsHandleCreated )
				AppendText( s );
			else
				BufferText( s );
		}

		private void BufferText( string s )
		{
			if ( sb == null )
				sb = new StringBuilder();

			sb.Append( s );
		}

		private void AppendText( string s )
		{
			if ( sb != null )
			{
				textBox.AppendText( sb.ToString() );
				sb = null;
			}
			
			textBox.AppendText( s );
		}

		private void OnHandleCreated( object sender, EventArgs e )
		{
			if ( sb != null )
			{
				textBox.AppendText( sb.ToString() );
				sb = null;
			}
		}

		public override void WriteLine(string s)
		{
			Write( s + "\r\n" );
		}

		public override Encoding Encoding
		{
			get { return Encoding.Default; }
		}

		public override Object InitializeLifetimeService()
		{
			return null;
		}
	}
}
