using System;
using System.Windows.Forms;

namespace NUnit.UiKit
{
	/// <summary>
	/// Utility class used to display a wait cursor
	/// while a long operation takes place and
	/// guarantee that it will be removed on exit.
	/// 
	/// Use as follows:
	/// 
	///		using ( new WaitCursor() )
	///		{
	///			// Long running operation goes here
	///		}
	///		
	/// </summary>
	public class WaitCursor : IDisposable
	{
		private Cursor cursor;

		public WaitCursor()
		{
			cursor = Cursor.Current;
			Cursor.Current = Cursors.WaitCursor;
		}

		public void Dispose()
		{
			Cursor.Current = cursor;
		}
	}

}
