using System;
using System.Diagnostics;

namespace NUnit.Framework
{
	/// <summary>
	/// Class with static members for use in 
	/// </summary>
	public class TestContext
	{
		private static bool tracing;

		public static bool Tracing
		{
			get { return tracing; }
			set 
			{ 
				if ( tracing != value )
				{
					tracing = value;
					if ( tracing )
						Trace.Listeners.Add( new TextWriterTraceListener( Console.Out, "NUnit" ) );
					else
						Trace.Listeners.Remove( "NUnit" );
				}
			}
		}

		private TestContext()
		{
		}
	}
}
