//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov. All Rights Reserved.
//
namespace NUnit.Gui
{
	using System;
	using System.IO;
	using System.Windows.Forms;
	using System.Text;

	/// <summary>
	/// Summary description for TextBoxWriter.
	/// </summary>
	public class TextBoxWriter : TextWriter
	{
		private TextBox fTextBox;
    			
		public TextBoxWriter(TextBox textBox)
		{
			fTextBox=textBox;
		}
    			
		public override void Write(char c)
		{
			fTextBox.Text = fTextBox.Text + c.ToString();
		}

		public override void Write(String s)
		{
			fTextBox.Text = fTextBox.Text + s;
		}

		public override Encoding Encoding
		{
			get { return Encoding.Default; }
		}
	}
}
