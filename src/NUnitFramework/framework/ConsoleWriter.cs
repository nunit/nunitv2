namespace NUnit.Core
{
	using System;
	using System.IO;
	using System.Text;

	/// <summary>
	/// Summary description for ConsoleWriter.
	/// </summary>
	public class ConsoleWriter : TextWriter
	{
		private TextWriter console;
    			
		public ConsoleWriter(TextWriter console)
		{
			this.console = console;
		}
    			
		public override void Write(char c)
		{
			console.Write(c);
		}

		public override void Write(String s)
		{
			console.Write(s);
		}

		public override void WriteLine(string s)
		{
			console.WriteLine(s);
		}

		public override Encoding Encoding
		{
			get { return Encoding.Default; }
		}
	}
}
