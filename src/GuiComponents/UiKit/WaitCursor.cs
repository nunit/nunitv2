#region Copyright (c) 2002-2003 Charlie Poole
/************************************************************************************
'
' Copyright (c) 2002-2003 Charlie Poole
'
' Later versions may be available at http://charliepoole.org.
'
' This software is provided 'as-is', without any express or implied warranty. In no 
' event will the author be held liable for any damages arising from the use of this 
' software.
' 
' Permission is granted to anyone to use this software for any purpose, including 
' commercial applications, and to alter it and redistribute it freely, subject to the 
' following restrictions:
'
' 1. The origin of this software must not be misrepresented; you must not claim that
' you wrote the original software. If you use this software in a product, you must
' include the following notice in the product documentation and/or other materials
' provided with the distribution.
'
' Portions Copyright (c) 2002-2003 Charlie Poole
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed from or altered in any source distribution.
'
'***********************************************************************************/
#endregion

using System;
using System.Windows.Forms;

namespace CP.Windows.Forms
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
		private Control control;

		public WaitCursor()
		{
			this.control = null;
			this.cursor = Cursor.Current;
			Cursor.Current = Cursors.WaitCursor;
		}

		public WaitCursor( Control control )
		{
			this.control = control;
			this.cursor = control.Cursor;
			control.Cursor = Cursors.WaitCursor;
		}

		public void Dispose()
		{
			if ( control != null )
				control.Cursor = this.cursor;
			else
				Cursor.Current = this.cursor;
		}
	}
}
