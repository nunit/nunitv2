#region Copyright (c) 2002, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright © 2000-2002 Philip A. Craig
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
' Portions Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright © 2000-2002 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using NUnit.Util;
using NUnit.UiKit;

namespace NUnit.Gui
{
	/// <summary>
	/// Summary description for ProjectEditor.
	/// </summary>
	public class ProjectEditor : System.Windows.Forms.Form
	{
		#region Instance Variables

		private NUnitProject project;
		private string selectedConfig;
		private string selectedAssembly;
		private System.Windows.Forms.Button editAssemblyButton;
		private System.Windows.Forms.Button addAssemblyButton;
		private System.Windows.Forms.Button deleteAssemblyButton;
		private System.Windows.Forms.Button editConfigsButton;
		private System.Windows.Forms.ListView assemblyListView;
		private System.Windows.Forms.ColumnHeader fileNameHeader;
		private System.Windows.Forms.ColumnHeader fullPathHeader;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox configComboBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button closeButton;
		private System.Windows.Forms.Button addVSProjectButton;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#endregion

		#region Construction and Disposal

		public ProjectEditor( NUnitProject project )
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			this.project = project;
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

		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ProjectEditor));
			this.closeButton = new System.Windows.Forms.Button();
			this.editAssemblyButton = new System.Windows.Forms.Button();
			this.addAssemblyButton = new System.Windows.Forms.Button();
			this.deleteAssemblyButton = new System.Windows.Forms.Button();
			this.editConfigsButton = new System.Windows.Forms.Button();
			this.assemblyListView = new System.Windows.Forms.ListView();
			this.fileNameHeader = new System.Windows.Forms.ColumnHeader();
			this.fullPathHeader = new System.Windows.Forms.ColumnHeader();
			this.label2 = new System.Windows.Forms.Label();
			this.configComboBox = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.addVSProjectButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// closeButton
			// 
			this.closeButton.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.closeButton.Location = new System.Drawing.Point(392, 296);
			this.closeButton.Name = "closeButton";
			this.closeButton.Size = new System.Drawing.Size(128, 23);
			this.closeButton.TabIndex = 21;
			this.closeButton.Text = "Close";
			this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
			// 
			// editAssemblyButton
			// 
			this.editAssemblyButton.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.editAssemblyButton.Location = new System.Drawing.Point(392, 192);
			this.editAssemblyButton.Name = "editAssemblyButton";
			this.editAssemblyButton.Size = new System.Drawing.Size(128, 23);
			this.editAssemblyButton.TabIndex = 20;
			this.editAssemblyButton.Text = "&Edit Path...";
			this.editAssemblyButton.Click += new System.EventHandler(this.editAssemblyButton_Click);
			// 
			// addAssemblyButton
			// 
			this.addAssemblyButton.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.addAssemblyButton.Location = new System.Drawing.Point(392, 128);
			this.addAssemblyButton.Name = "addAssemblyButton";
			this.addAssemblyButton.Size = new System.Drawing.Size(128, 23);
			this.addAssemblyButton.TabIndex = 19;
			this.addAssemblyButton.Text = "&Add Assembly...";
			this.addAssemblyButton.Click += new System.EventHandler(this.addAssemblyButton_Click);
			// 
			// deleteAssemblyButton
			// 
			this.deleteAssemblyButton.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.deleteAssemblyButton.Location = new System.Drawing.Point(392, 224);
			this.deleteAssemblyButton.Name = "deleteAssemblyButton";
			this.deleteAssemblyButton.Size = new System.Drawing.Size(128, 23);
			this.deleteAssemblyButton.TabIndex = 18;
			this.deleteAssemblyButton.Text = "&Remove Assembly";
			this.deleteAssemblyButton.Click += new System.EventHandler(this.deleteAssemblyButton_Click);
			// 
			// editConfigsButton
			// 
			this.editConfigsButton.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.editConfigsButton.Location = new System.Drawing.Point(392, 32);
			this.editConfigsButton.Name = "editConfigsButton";
			this.editConfigsButton.Size = new System.Drawing.Size(128, 23);
			this.editConfigsButton.TabIndex = 17;
			this.editConfigsButton.Text = "&Edit Configs...";
			this.editConfigsButton.Click += new System.EventHandler(this.editConfigsButton_Click);
			// 
			// assemblyListView
			// 
			this.assemblyListView.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.assemblyListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							   this.fileNameHeader,
																							   this.fullPathHeader});
			this.assemblyListView.FullRowSelect = true;
			this.assemblyListView.HideSelection = false;
			this.assemblyListView.Location = new System.Drawing.Point(23, 104);
			this.assemblyListView.MultiSelect = false;
			this.assemblyListView.Name = "assemblyListView";
			this.assemblyListView.Size = new System.Drawing.Size(360, 216);
			this.assemblyListView.TabIndex = 16;
			this.assemblyListView.View = System.Windows.Forms.View.Details;
			this.assemblyListView.Resize += new System.EventHandler(this.assemblyListView_Resize);
			this.assemblyListView.SelectedIndexChanged += new System.EventHandler(this.assemblyListView_SelectedIndexChanged);
			// 
			// fileNameHeader
			// 
			this.fileNameHeader.Text = "File Name";
			this.fileNameHeader.Width = 100;
			// 
			// fullPathHeader
			// 
			this.fullPathHeader.Text = "Full Path";
			this.fullPathHeader.Width = 256;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(23, 72);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(304, 16);
			this.label2.TabIndex = 15;
			this.label2.Text = "Assemblies Included:";
			// 
			// configComboBox
			// 
			this.configComboBox.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.configComboBox.Location = new System.Drawing.Point(23, 32);
			this.configComboBox.Name = "configComboBox";
			this.configComboBox.Size = new System.Drawing.Size(360, 24);
			this.configComboBox.TabIndex = 14;
			this.configComboBox.SelectedIndexChanged += new System.EventHandler(this.configComboBox_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(23, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(128, 16);
			this.label1.TabIndex = 13;
			this.label1.Text = "Select Configuration";
			// 
			// addVSProjectButton
			// 
			this.addVSProjectButton.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.addVSProjectButton.Location = new System.Drawing.Point(392, 160);
			this.addVSProjectButton.Name = "addVSProjectButton";
			this.addVSProjectButton.Size = new System.Drawing.Size(128, 23);
			this.addVSProjectButton.TabIndex = 22;
			this.addVSProjectButton.Text = "Add &VS Project...";
			this.addVSProjectButton.Click += new System.EventHandler(this.addVSProjectButton_Click);
			// 
			// ProjectEditor
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.CancelButton = this.closeButton;
			this.ClientSize = new System.Drawing.Size(528, 336);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.addVSProjectButton,
																		  this.closeButton,
																		  this.editAssemblyButton,
																		  this.addAssemblyButton,
																		  this.deleteAssemblyButton,
																		  this.editConfigsButton,
																		  this.assemblyListView,
																		  this.label2,
																		  this.configComboBox,
																		  this.label1});
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(448, 328);
			this.Name = "ProjectEditor";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "NUnit Test Project Editor";
			this.Load += new System.EventHandler(this.ProjectEditor_Load);
			this.ResumeLayout(false);

		}
		#endregion

		#region Static Edit Method

		public static DialogResult Edit( NUnitProject project )
		{
			ProjectEditor editor = new ProjectEditor( project );
			return editor.ShowDialog();
		}

		#endregion

		#region ComboBoxes

		private void UpdateLists()
		{
			UpdateConfigs();
		}

		private void UpdateConfigs()
		{
			configComboBox.Items.Clear();

			if ( selectedConfig == null )
				selectedConfig = project.ActiveConfigName;

			int selectedIndex = -1; 
			foreach( string name in project.Configs.Names )
			{
				int index = configComboBox.Items.Add( name );
				if ( name == selectedConfig )
					selectedIndex = index;
			}

			if ( selectedIndex == -1 && configComboBox.Items.Count > 0 )
			{
				selectedIndex = 0;
				selectedConfig = project.Configs[0].Name;
			}

			if ( selectedIndex == -1 )
				selectedConfig = null;
			else
				configComboBox.SelectedIndex = selectedIndex;
		}

		private void configComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			selectedConfig = (string)configComboBox.SelectedItem;
			UpdateAssembliesForConfig();
		}

		#endregion

		#region ListViews

		private void UpdateAssembliesForConfig()
		{
			assemblyListView.Items.Clear();
			int selectedIndex = -1;

			foreach( string assemblyPath in project.Configs[selectedConfig].Assemblies )
			{
				string assemblyName = Path.GetFileName( assemblyPath );

				ListViewItem item = new ListViewItem(
					new string[] { 
									 Path.GetFileName( assemblyPath ),
									 assemblyPath } );

				int index = assemblyListView.Items.Add( item ).Index;			

				if ( assemblyName == selectedAssembly )
					selectedIndex = index;
			}

			if ( assemblyListView.Items.Count > 0 && selectedIndex == -1)
				selectedIndex = 0;
				
			if ( selectedIndex != -1 )
				assemblyListView.Items[selectedIndex].Selected = true;
		}

		private void assemblyListView_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if ( assemblyListView.SelectedItems.Count == 0 )
			{
				selectedAssembly = null;

				editAssemblyButton.Enabled = false;
				deleteAssemblyButton.Enabled = false;
			}
			else 
			{
				selectedAssembly = assemblyListView.SelectedItems[0].Text;

				editAssemblyButton.Enabled = true;
				deleteAssemblyButton.Enabled = true;
			}
		}

		private void assemblyListView_Resize(object sender, System.EventArgs e)
		{
			assemblyListView.Columns[1].Width =
				assemblyListView.ClientSize.Width - assemblyListView.Columns[0].Width;
		}

		#endregion

		#region Other UI Events

		private void ProjectEditor_Load(object sender, System.EventArgs e)
		{
			UpdateLists();
		}

		private void editConfigsButton_Click(object sender, System.EventArgs e)
		{
			ConfigurationEditor.Edit( project );
			UpdateLists();
		}

		private void addAssemblyButton_Click(object sender, System.EventArgs e)
		{
			AppUI.TestLoaderUI.AddAssembly( selectedConfig );
		}

		private void addVSProjectButton_Click(object sender, System.EventArgs e)
		{
			AppUI.TestLoaderUI.AddVSProject( );
		}
		private void editAssemblyButton_Click(object sender, System.EventArgs e)
		{
			AssemblyPathDialog dlg = new AssemblyPathDialog( (string)project.Configs[selectedConfig].Assemblies[assemblyListView.SelectedIndices[0]] );
			if ( dlg.ShowDialog() == DialogResult.OK )
			{
				project.Configs[selectedConfig].Assemblies[assemblyListView.SelectedIndices[0]] = dlg.Path;
				UpdateAssembliesForConfig();
				if ( AppUI.TestLoader.IsTestLoaded && selectedConfig == project.ActiveConfigName )
					AppUI.TestLoader.SetActiveConfig( selectedConfig );
			}
		}

		private void deleteAssemblyButton_Click(object sender, System.EventArgs e)
		{
			int index = assemblyListView.SelectedItems[0].Index;
			project.Configs[selectedConfig].Assemblies.RemoveAt( index );
			assemblyListView.Items.RemoveAt( index );
			if ( index >= assemblyListView.Items.Count )
				--index;

			if ( index >= 0 )
			{
				selectedAssembly = assemblyListView.Items[index].Text;
				assemblyListView.Items[index].Selected = true;
			}
			else
				selectedAssembly = null;
		}

		private void closeButton_Click(object sender, System.EventArgs e)
		{
			this.Close();		
		}

		#endregion
	}
}
