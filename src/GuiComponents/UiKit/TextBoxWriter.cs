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
		#region Private Fields
		
		/// <summary>
		/// The TextBoxBase-derived control we write to
		/// </summary>
		private TextBoxBase textBox;
		
		/// <summary>
		/// StringBuilder to hold text until the control is actually  created
		/// </summary>
		private StringBuilder sb;

		#endregion
    			
		#region Constructors

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

		#endregion

		#region TextBoxWriterMethods

		/// <summary>
		/// Clear both the TextBox and the buffer.
		/// </summary>
		public void Clear()
		{
			textBox.Clear();
			sb = null;
		}

		#endregion

		#region TextWriter Overrides

		/// <summary>
		/// Flush the buffer to the TextBox, if it has been 
		/// created, and clear the buffer.
		/// </summary>
		public override void Flush()
		{
			if ( textBox.IsHandleCreated && sb != null )
				AppendToTextBox( sb.ToString() );
			sb = null;
		}

		/// <summary>
		/// Write a single char
		/// </summary>
		/// <param name="c">The char to write</param>
		public override void Write(char c)
		{
			Write( c.ToString() );
		}

		/// <summary>
		/// Write a string
		/// </summary>
		/// <param name="s">The string to write</param>
		public override void Write(String s)
		{
			if ( textBox.IsHandleCreated )
			{
				if ( sb != null )
				{
					sb.Append( s );
					Flush();
				}
				else
					AppendToTextBox( s );
			}
			else
			{
				if ( sb == null )
					sb = new StringBuilder();
				sb.Append( s );
			}
		}

		/// <summary>
		/// Write a string followed by a newline.
		/// </summary>
		/// <param name="s">The string to write</param>
		public override void WriteLine(string s)
		{
			Write( s + "\r\n" );
		}

		/// <summary>
		/// The encoding in use for this TextWriter.
		/// </summary>
		public override Encoding Encoding
		{
			get { return Encoding.Default; }
		}

		#endregion

		#region MarshalByRefObject Overrides

		public override Object InitializeLifetimeService()
		{
			return null;
		}

		#endregion

		#region Internal Helpers

		/// <summary>
		/// Delegate used to append text to the text box
		/// </summary>
		private delegate void AppendTextHandler( string s );

		/// <summary>
		/// Append to the text box using the proper thread
		/// </summary>
		/// <param name="s">The string to append</param>
		private void AppendToTextBox( string s )
		{
			textBox.Invoke( new AppendTextHandler( textBox.AppendText ),
				new object[] { s } );
		}

		/// <summary>
		/// Respond to creation of the text box handle by flushing
		/// any buffered text so that it is displayed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnHandleCreated( object sender, EventArgs e )
		{
			Flush();
		}

		#endregion
	}
}
