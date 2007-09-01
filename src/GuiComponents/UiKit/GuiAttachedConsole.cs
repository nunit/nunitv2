using System;
using System.Runtime.InteropServices;

namespace NUnit.UiKit
{
	/// <summary>
	/// Summary description for GuiAttachedConsole.
	/// </summary>
	public class GuiAttachedConsole
	{
		public GuiAttachedConsole()
		{
			AllocConsole();
		}

		public void Close()
		{
			FreeConsole();
		}

		[DllImport("Kernel32.dll")]
		static extern bool AllocConsole();

		[DllImport("Kernel32.dll")]
		static extern bool FreeConsole();
	}
}
