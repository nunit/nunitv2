// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************
using System;
using System.Windows.Forms;
using NUnit.Core;
using NUnit.Util;

namespace NUnit.UiKit
{
	/// <summary>
	/// TextBoxDisplay is an adapter that allows accessing a 
	/// System.Windows.Forms.TextBox using the TextDisplay interface.
	/// </summary>
	public class TextBoxDisplay : System.Windows.Forms.RichTextBox, TextDisplay, TestObserver
	{
		private ContextMenu contextMenu = new ContextMenu();
		private MenuItem copyMenuItem;
		private MenuItem selectAllMenuItem;
		private MenuItem wordWrapMenuItem;

		private TextDisplayContent content;

		public TextBoxDisplay()
		{
			this.Multiline = true;
			this.ReadOnly = true;
			this.WordWrap = false;

			this.ContextMenu = new ContextMenu();
			this.copyMenuItem = new MenuItem( "&Copy", new EventHandler( copyMenuItem_Click ) );
			this.selectAllMenuItem = new MenuItem( "Select &All", new EventHandler( selectAllMenuItem_Click ) );
			this.wordWrapMenuItem = new MenuItem( "&Word Wrap", new EventHandler( wordWrapMenuItem_Click ) );
			this.ContextMenu.MenuItems.AddRange( new MenuItem[] { copyMenuItem, selectAllMenuItem, wordWrapMenuItem } );
			this.ContextMenu.Popup += new EventHandler(ContextMenu_Popup);
		}

		private void copyMenuItem_Click(object sender, EventArgs e)
		{
			this.Copy();
		}

		private void selectAllMenuItem_Click(object sender, EventArgs e)
		{
			this.SelectAll();
		}

		private void wordWrapMenuItem_Click(object sender, EventArgs e)
		{
			this.WordWrap = this.wordWrapMenuItem.Checked = !this.wordWrapMenuItem.Checked;
		}

		private void ContextMenu_Popup(object sender, EventArgs e)
		{
			this.copyMenuItem.Enabled = this.SelectedText != "";
			this.selectAllMenuItem.Enabled = this.TextLength > 0;
		}

		private string pendingTestCaseLabel = null;
		private void OnTestOutput( object sender, TestEventArgs e )
		{
			if ( WantOutputType( e.TestOutput.Type ) )
			{
				if ( pendingTestCaseLabel != null )
				{
					WriteLine( pendingTestCaseLabel );
					pendingTestCaseLabel = null;
				}

				Write( e.TestOutput.Text );
			}
		}

		private bool WantOutputType( TestOutputType type )
		{
			TextDisplayContent mask = TextDisplayContent.Empty;
			switch( type )
			{
				case TestOutputType.Out:
					mask = TextDisplayContent.Out;
					break;
				case TestOutputType.Error:
					mask = TextDisplayContent.Error;
					break;
				case TestOutputType.Trace:
					mask = TextDisplayContent.Trace;
					break;
				case TestOutputType.Log:
					mask = TextDisplayContent.Log;
					break;
			}

			return ((int)mask & (int)this.content) != 0;
		}

		private void OnTestStarting(object sender, TestEventArgs args)
		{
			if ( (this.content & TextDisplayContent.Labels) != 0 )
			{
				string label = string.Format( "***** {0}", args.TestName.FullName );

				if ( (this.content & TextDisplayContent.LabelOnlyOnOutput) != 0 )
					this.pendingTestCaseLabel = label;
				else
					WriteLine(label);
			}
		}

		#region TextDisplay Members
		public TextDisplayContent Content
		{
			get { return content; }
			set { content = value; }
		}

		public void Write( string text )
		{
			this.AppendText( text );
		}

		public void Write( NUnit.Core.TestOutput output )
		{
			Write( output.Text );
		}

		public void WriteLine( string text )
		{
			Write( text + Environment.NewLine );
		}

		public string GetText()
		{
			return this.Text;
		}
		#endregion

		#region TestObserver Members
		public void Subscribe(ITestEvents events)
		{
			events.TestOutput += new TestEventHandler(OnTestOutput);
			events.TestStarting += new TestEventHandler(OnTestStarting);
		}
		#endregion
	}
}
