namespace NUnit.Core
{
	using System;
	using System.IO;
	using System.Text;

	/// <summary>
	/// This wrapper adds buffering to improve cross-domain performance.
	/// </summary>
	public class EventListenerTextWriter : TextWriter
	{
		private EventListener eventListener;
		private TestOutputType type;
		private const int MAX_BUFFER = 1024;
		private StringBuilder sb = new StringBuilder( MAX_BUFFER );

		public EventListenerTextWriter( EventListener eventListener, TestOutputType type )
		{
			this.eventListener = eventListener;
			this.type = type;
		}

		public override Encoding Encoding
		{
			get
			{
				return Encoding.Default;
			}
		}
	
		override public void Write(char ch)
		{
			lock( sb )
			{
				sb.Append( ch );
				this.CheckBuffer();
			}
		}

		override public void Write(string str)
		{
			lock( sb )
			{
				sb.Append( str );
				this.CheckBuffer();
			}
		}

		override public void WriteLine(string str)
		{
			lock( sb )
			{
				sb.Append( str );
				sb.Append( '\n' );
				this.CheckBuffer();
			}
		}

		override public void Flush()
		{
			if ( sb.Length > 0 )
			{
				lock( sb )
				{
					TestOutput output = new TestOutput(sb.ToString(), this.type);
					this.eventListener.TestOutput( output );
					sb.Length = 0;
				}
			}
		}

		private void CheckBuffer()
		{
			if ( sb.Length >= MAX_BUFFER )
				this.Flush();
		}
	}
}
