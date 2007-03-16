// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using NUnit.Core;
using NUnit.Core.Extensibility;

namespace NUnit.Gui
{
	/// <summary>
	/// Summary description for AddinDialog.
	/// </summary>
	public class AddinDialog : System.Windows.Forms.Form
	{
		private IList addins;

		private System.Windows.Forms.TextBox descriptionTextBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ListView addinListView;
		private System.Windows.Forms.ColumnHeader addinNameColumn;
		private System.Windows.Forms.ColumnHeader extensionTypeColumn;
		private System.Windows.Forms.ColumnHeader addinStatusColumn;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public AddinDialog()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.addinListView = new System.Windows.Forms.ListView();
			this.addinNameColumn = new System.Windows.Forms.ColumnHeader();
			this.extensionTypeColumn = new System.Windows.Forms.ColumnHeader();
			this.addinStatusColumn = new System.Windows.Forms.ColumnHeader();
			this.descriptionTextBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// addinListView
			// 
			this.addinListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.addinListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							this.addinNameColumn,
																							this.extensionTypeColumn,
																							this.addinStatusColumn});
			this.addinListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.addinListView.Location = new System.Drawing.Point(8, 8);
			this.addinListView.MultiSelect = false;
			this.addinListView.Name = "addinListView";
			this.addinListView.Size = new System.Drawing.Size(448, 208);
			this.addinListView.TabIndex = 0;
			this.addinListView.View = System.Windows.Forms.View.Details;
			this.addinListView.Resize += new System.EventHandler(this.addinListView_Resize);
			this.addinListView.SelectedIndexChanged += new System.EventHandler(this.addinListView_SelectedIndexChanged);
			// 
			// addinNameColumn
			// 
			this.addinNameColumn.Text = "Addin";
			this.addinNameColumn.Width = 200;
			// 
			// extensionTypeColumn
			// 
			this.extensionTypeColumn.Text = "Type";
			this.extensionTypeColumn.Width = 100;
			// 
			// addinStatusColumn
			// 
			this.addinStatusColumn.Text = "Status";
			this.addinStatusColumn.Width = 100;
			// 
			// descriptionTextBox
			// 
			this.descriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.descriptionTextBox.Location = new System.Drawing.Point(8, 240);
			this.descriptionTextBox.Multiline = true;
			this.descriptionTextBox.Name = "descriptionTextBox";
			this.descriptionTextBox.Size = new System.Drawing.Size(448, 72);
			this.descriptionTextBox.TabIndex = 1;
			this.descriptionTextBox.Text = "";
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.Location = new System.Drawing.Point(8, 224);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(304, 16);
			this.label1.TabIndex = 2;
			this.label1.Text = "Description:";
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Location = new System.Drawing.Point(368, 320);
			this.button1.Name = "button1";
			this.button1.TabIndex = 3;
			this.button1.Text = "Close";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// AddinDialog
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.ClientSize = new System.Drawing.Size(464, 352);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.descriptionTextBox);
			this.Controls.Add(this.addinListView);
			this.Name = "AddinDialog";
			this.Text = "Registered Addins";
			this.Load += new System.EventHandler(this.AddinDialog_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void AddinDialog_Load(object sender, System.EventArgs e)
		{
			this.addins = NUnit.Util.Services.AddinRegistry.Addins;

			foreach( Addin addin in addins )
			{
				ListViewItem item = new ListViewItem( 
					new string[] { addin.Name, addin.ExtensionType.ToString(), addin.Status.ToString() } );
				addinListView.Items.Add( item );
			}

			if ( addinListView.Items.Count > 0 )
				addinListView.Items[0].Selected = true;

			AutoSizeFirstColumnOfListView();
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void addinListView_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if ( addinListView.SelectedIndices.Count > 0 )
			{
				int index = addinListView.SelectedIndices[0];
				this.descriptionTextBox.Text = ((Addin)addins[index]).Description;
			}
		}

		private void addinListView_Resize(object sender, System.EventArgs e)
		{
			AutoSizeFirstColumnOfListView();
		}

		private void AutoSizeFirstColumnOfListView()
		{
			int width = addinListView.ClientSize.Width;
			for( int i = 1; i < addinListView.Columns.Count; i++ )
				width -= addinListView.Columns[i].Width;
			addinListView.Columns[0].Width = width;
		}
	}
}
