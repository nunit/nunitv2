using System;

namespace NUnit.ConsoleRunner
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class Class1
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		public static int Main(string[] args)
		{
			return ConsoleUi.Main( args );
		}
	}
}
