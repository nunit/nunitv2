// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************
using System;
using System.Windows.Forms;

namespace NUnit.UiKit
{
	/// <summary>
	/// TextBoxDisplay is an adapter that allows accessing a 
	/// System.Windows.Forms.TextBox using the TextDisplay interface.
	/// </summary>
	public class TextBoxDisplay : System.Windows.Forms.RichTextBox, TextDisplay
	{
		private ContextMenu contextMenu = new ContextMenu();
		private MenuItem copyMenuItem;
		private MenuItem selectAllMenuItem;
		private MenuItem wordWrapMenuItem;

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
	}
}
