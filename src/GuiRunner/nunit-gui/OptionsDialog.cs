using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using NUnit.Util;
using NUnit.UiKit;

namespace NUnit.Gui
{
	/// <summary>
	/// Summary description for OptionsDialog.
	/// </summary>
	public class OptionsDialog : System.Windows.Forms.Form
	{
		private UIActions actions;
		private OptionSettings options;

		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.HelpProvider helpProvider1;
		private System.Windows.Forms.CheckBox loadLastAssemblyCheckBox;
		private System.Windows.Forms.CheckBox clearResultsCheckBox;
		private System.Windows.Forms.CheckBox enableWatcherCheckBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox initialDisplayComboBox;
		private System.ComponentModel.IContainer components;

		public OptionsDialog( OptionSettings options, UIActions actions )
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			this.actions = actions;
			this.options = options;
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
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.helpProvider1 = new System.Windows.Forms.HelpProvider();
			this.loadLastAssemblyCheckBox = new System.Windows.Forms.CheckBox();
			this.clearResultsCheckBox = new System.Windows.Forms.CheckBox();
			this.enableWatcherCheckBox = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.initialDisplayComboBox = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// okButton
			// 
			this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.okButton.Location = new System.Drawing.Point(65, 224);
			this.okButton.Name = "okButton";
			this.okButton.TabIndex = 0;
			this.okButton.Text = "OK";
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(161, 224);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(67, 23);
			this.cancelButton.TabIndex = 1;
			this.cancelButton.Text = "Cancel";
			// 
			// loadLastAssemblyCheckBox
			// 
			this.helpProvider1.SetHelpString(this.loadLastAssemblyCheckBox, "If checked, most recent assembly is loaded at startup.");
			this.loadLastAssemblyCheckBox.Location = new System.Drawing.Point(24, 16);
			this.loadLastAssemblyCheckBox.Name = "loadLastAssemblyCheckBox";
			this.helpProvider1.SetShowHelp(this.loadLastAssemblyCheckBox, true);
			this.loadLastAssemblyCheckBox.Size = new System.Drawing.Size(256, 24);
			this.loadLastAssemblyCheckBox.TabIndex = 15;
			this.loadLastAssemblyCheckBox.Text = "Load most recent assembly at startup.";
			// 
			// clearResultsCheckBox
			// 
			this.helpProvider1.SetHelpString(this.clearResultsCheckBox, "If checked, any prior results are cleared if tests are added or removed.");
			this.clearResultsCheckBox.Location = new System.Drawing.Point(56, 80);
			this.clearResultsCheckBox.Name = "clearResultsCheckBox";
			this.helpProvider1.SetShowHelp(this.clearResultsCheckBox, true);
			this.clearResultsCheckBox.Size = new System.Drawing.Size(232, 24);
			this.clearResultsCheckBox.TabIndex = 12;
			this.clearResultsCheckBox.Text = "Clear results when tests change.";
			// 
			// enableWatcherCheckBox
			// 
			this.helpProvider1.SetHelpString(this.enableWatcherCheckBox, "If checked, the assembly is reloaded whenever it changes. Changes to this setting" +
				" do not take effect until the next time an assembly is loaded.");
			this.enableWatcherCheckBox.Location = new System.Drawing.Point(24, 56);
			this.enableWatcherCheckBox.Name = "enableWatcherCheckBox";
			this.helpProvider1.SetShowHelp(this.enableWatcherCheckBox, true);
			this.enableWatcherCheckBox.Size = new System.Drawing.Size(256, 24);
			this.enableWatcherCheckBox.TabIndex = 11;
			this.enableWatcherCheckBox.Text = "Watch for changes to the assembly.";
			this.enableWatcherCheckBox.CheckedChanged += new System.EventHandler(this.enableWatcherCheckBox_CheckedChanged);
			// 
			// label1
			// 
			this.helpProvider1.SetHelpString(this.label1, "");
			this.label1.Location = new System.Drawing.Point(32, 120);
			this.label1.Name = "label1";
			this.helpProvider1.SetShowHelp(this.label1, true);
			this.label1.Size = new System.Drawing.Size(104, 24);
			this.label1.TabIndex = 16;
			this.label1.Text = "Initial Display:";
			// 
			// initialDisplayComboBox
			// 
			this.initialDisplayComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.helpProvider1.SetHelpString(this.initialDisplayComboBox, "Selects the initial display style of the tree when an assembly is loaded");
			this.initialDisplayComboBox.Items.AddRange(new object[] {
																		"Auto",
																		"Expand",
																		"Collapse",
																		"HideTests"});
			this.initialDisplayComboBox.Location = new System.Drawing.Point(144, 120);
			this.initialDisplayComboBox.Name = "initialDisplayComboBox";
			this.helpProvider1.SetShowHelp(this.initialDisplayComboBox, true);
			this.initialDisplayComboBox.Size = new System.Drawing.Size(120, 24);
			this.initialDisplayComboBox.TabIndex = 17;
			// 
			// OptionsDialog
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(298, 250);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.initialDisplayComboBox,
																		  this.label1,
																		  this.loadLastAssemblyCheckBox,
																		  this.clearResultsCheckBox,
																		  this.enableWatcherCheckBox,
																		  this.cancelButton,
																		  this.okButton});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.HelpButton = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "OptionsDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "NUnit Options";
			this.Load += new System.EventHandler(this.OptionsDialog_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void OptionsDialog_Load(object sender, System.EventArgs e)
		{
			loadLastAssemblyCheckBox.Checked = options.LoadLastAssembly;

			enableWatcherCheckBox.Checked = options.EnableWatcher;
			clearResultsCheckBox.Enabled = enableWatcherCheckBox.Checked;
			clearResultsCheckBox.Checked = options.ClearResults;

			initialDisplayComboBox.SelectedIndex = options.InitialTreeDisplay;
		}

		private void okButton_Click(object sender, System.EventArgs e)
		{
			if ( actions.IsAssemblyLoaded && 
				options.EnableWatcher != enableWatcherCheckBox.Checked )
			{
				string msg = String.Format(
					"Watching for file changes will be {0} the next time you load an assembly.",
					enableWatcherCheckBox.Checked ? "enabled" : "disabled" );

				MessageBox.Show( msg, "NUnit Options" );
			}

			options.LoadLastAssembly = loadLastAssemblyCheckBox.Checked;
			
			options.EnableWatcher = enableWatcherCheckBox.Checked;
			options.ClearResults = clearResultsCheckBox.Checked;

			options.InitialTreeDisplay = initialDisplayComboBox.SelectedIndex;

			DialogResult = DialogResult.OK;

			Close();
		}

		private void enableWatcherCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			clearResultsCheckBox.Enabled = enableWatcherCheckBox.Checked;
		}
	}
}
