#region Copyright (c) 2002-2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright  2000-2002 Philip A. Craig
'
' This software is provided 'as-is', without any express or implied warranty. In no 
' event will the authors be held liable for any damages arising from the use of this 
' software.
' 
' Permission is granted to anyone to use this software for any purpose, including 
' commercial applications, and to alter it and redistribute it freely, subject to the 
' following restrictions:
'
' 1. The origin of this software must not be misrepresented; you must not claim that 
' you wrote the original software. If you use this software in a product, an 
' acknowledgment (see the following) in the product documentation is required.
'
' Portions Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright  2000-2002 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

using System;
using System.Text;
using System.Windows.Forms;

namespace NUnit.UiKit
{
	/// <summary>
	/// Summary description for UserMessage.
	/// </summary>
	public class UserMessage
	{
		private static readonly string nunitCaption = "NUnit";

		public static DialogResult Display( string message )
		{
			return Display( message, nunitCaption, MessageBoxButtons.OK, MessageBoxIcon.None );
		}

		public static DialogResult Display( string message, string caption )
		{
			return Display( message, caption, MessageBoxButtons.OK, MessageBoxIcon.None );
		}

		public static DialogResult Display( string message, MessageBoxButtons buttons )
		{
			return Display( message, nunitCaption, buttons, MessageBoxIcon.None );
		}

		public static DialogResult Display( string message, MessageBoxButtons buttons, MessageBoxIcon icon )
		{
			return Display( message, nunitCaption, buttons, icon );
		}

		public static DialogResult Display( string message, string caption, MessageBoxButtons buttons, MessageBoxIcon icon )
		{
			return MessageBox.Show( message, caption, buttons, icon );
		}

		public static DialogResult DisplayFailure( string message )
		{
			return DisplayFailure( message, nunitCaption );
		}

		public static DialogResult DisplayFailure( string message, string caption )
		{
			return Display( message, caption, MessageBoxButtons.OK, MessageBoxIcon.Stop );
		}

		public static DialogResult DisplayFailure( Exception exception, string caption )
		{
			Exception ex = exception;
			StringBuilder sb = new StringBuilder();		
			sb.AppendFormat( "{0} : {1}", ex.GetType().ToString(), ex.Message );

			while( ex.InnerException != null )
			{
				ex = ex.InnerException;
				sb.AppendFormat( "\r----> {0} : {1}", ex.GetType().ToString(), ex.Message );
			}

			sb.Append( "\r\rFor further information, use the Exception Details menu item." );

			return DisplayFailure( sb.ToString(), caption );
		}

		public static DialogResult DisplayInfo( string message )
		{
			return DisplayInfo( message, nunitCaption );
		}

		public static DialogResult DisplayInfo( string message, string caption )
		{
			return Display( message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information );
		}

		public static DialogResult Ask( string message, MessageBoxButtons buttons )
		{
			return Ask( message, nunitCaption, buttons );
		}

		public static DialogResult Ask( string message )
		{
			return Ask( message, nunitCaption, MessageBoxButtons.YesNo );
		}

		public static DialogResult Ask( string message, string caption )
		{
			return Display( message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question );
		}

		public static DialogResult Ask( string message, string caption, MessageBoxButtons buttons )
		{
			return Display( message, caption, buttons, MessageBoxIcon.Question );
		}

	}
}
