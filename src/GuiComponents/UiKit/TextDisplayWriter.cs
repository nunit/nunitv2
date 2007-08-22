// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using System.IO;
using System.Text;

namespace NUnit.UiKit
{
	/// <summary>
	/// Summary description for TextDisplayWriter.
	/// </summary>
	public class TextDisplayWriter : TextWriter
	{
		private TextDisplay textDisplay;

		public TextDisplayWriter( TextDisplay textDisplay )
		{
			this.textDisplay = textDisplay;
		}

		public void Clear()
		{
			textDisplay.Clear();
		}
	
		#region TextWriter Overrides

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
			textDisplay.AppendText( s );
		}

		/// <summary>
		/// Write a string followed by a newline.
		/// </summary>
		/// <param name="s">The string to write</param>
		public override void WriteLine(string s)
		{
			Write( s + Environment.NewLine );
		}

		/// <summary>
		/// The encoding in use for this TextWriter.
		/// </summary>
		public override Encoding Encoding
		{
			get { return Encoding.Default; }
		}
		#endregion
	}
}
