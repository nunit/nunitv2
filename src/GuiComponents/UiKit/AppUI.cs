using System;
using System.IO;
using System.Windows.Forms;
using NUnit.Util;

namespace NUnit.UiKit
{
	/// <summary>
	/// AppUI keeps track of the main components
	/// for an NUnit-based application so that
	/// UI elements can find them.
	/// </summary>
	public class AppUI
	{
		private static TextWriter outWriter;
		private static TextWriter errWriter;

		private static TestLoader loader;
		private static TestLoaderUI loaderUI;

		public static void Init( Form ownerForm, TextWriter outWriter, TextWriter errWriter )
		{
			AppUI.outWriter = outWriter;
			Console.SetOut( outWriter );

			AppUI.errWriter = errWriter;
			Console.SetError( errWriter );

			loader = new TestLoader( outWriter, errWriter );
			loaderUI = new TestLoaderUI( ownerForm, loader );
		}

		public static TextWriter Out
		{
			get { return outWriter; }
		}

		public static TextWriter Error
		{
			get { return errWriter; }
		}

		public static TestLoader TestLoader
		{
			get { return loader; }
		}

		public static TestLoaderUI TestLoaderUI
		{
			get { return loaderUI; }
		}
	}
}
