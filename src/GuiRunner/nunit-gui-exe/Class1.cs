using System;

namespace NUnit.GuiRunner
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class Class1
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		public static int Main(string[] args)
		{
			return NUnit.Gui.AppEntry.Main( args );
		}
	}
}
