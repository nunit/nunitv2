using System;
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
			string message = exception.Message;
			if(exception.InnerException != null)
				message = exception.InnerException.Message;

			return DisplayFailure( message, caption );
		}

		public static DialogResult DisplayInfo( string message )
		{
			return DisplayInfo( message, nunitCaption );
		}

		public static DialogResult DisplayInfo( string message, string caption )
		{
			return Display( message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information );
		}

		public static DialogResult Ask( string message )
		{
			return Ask( message, nunitCaption );
		}

		public static DialogResult Ask( string message, string caption )
		{
			return Display( message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question );
		}

	}
}
