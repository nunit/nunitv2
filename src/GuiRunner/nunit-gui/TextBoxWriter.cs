/********************************************************************************************************************
'
' Copyright (c) 2002, James Newkirk, Michael C. Two, Alexei Vorontsov, Philip Craig
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
' THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
'
'*******************************************************************************************************************/
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
