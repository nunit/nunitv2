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
		private OptionSettings options;

		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.HelpProvider helpProvider1;
		private System.Windows.Forms.CheckBox clearResultsCheckBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox initialDisplayComboBox;
		private System.Windows.Forms.CheckBox reloadOnChangeCheckBox;
		private System.Windows.Forms.CheckBox reloadOnRunCheckBox;
		private System.Windows.Forms.CheckBox visualStudioSupportCheckBox;
		private System.Windows.Forms.CheckBox loadLastProjectCheckBox;
		private System.ComponentModel.IContainer components;

		public static void EditOptions( )
		{
			OptionsDialog dialog = new OptionsDialog( UserSettings.Options );
			dialog.ShowDialog();
		}

		public OptionsDialog( OptionSettings options )
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(OptionsDialog));
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.helpProvider1 = new System.Windows.Forms.HelpProvider();
			this.loadLastProjectCheckBox = new System.Windows.Forms.CheckBox();
			this.clearResultsCheckBox = new System.Windows.Forms.CheckBox();
			this.reloadOnChangeCheckBox = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.initialDisplayComboBox = new System.Windows.Forms.ComboBox();
			this.reloadOnRunCheckBox = new System.Windows.Forms.CheckBox();
			this.visualStudioSupportCheckBox = new System.Windows.Forms.CheckBox();
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
			// loadLastProjectCheckBox
			// 
			this.helpProvider1.SetHelpString(this.loadLastProjectCheckBox, "If checked, most recent project is loaded at startup.");
			this.loadLastProjectCheckBox.Location = new System.Drawing.Point(24, 16);
			this.loadLastProjectCheckBox.Name = "loadLastProjectCheckBox";
			this.helpProvider1.SetShowHelp(this.loadLastProjectCheckBox, true);
			this.loadLastProjectCheckBox.Size = new System.Drawing.Size(256, 24);
			this.loadLastProjectCheckBox.TabIndex = 15;
			this.loadLastProjectCheckBox.Text = "Load most recent project at startup.";
			// 
			// clearResultsCheckBox
			// 
			this.helpProvider1.SetHelpString(this.clearResultsCheckBox, "If checked, any prior results are cleared if tests are added or removed.");
			this.clearResultsCheckBox.Location = new System.Drawing.Point(48, 136);
			this.clearResultsCheckBox.Name = "clearResultsCheckBox";
			this.helpProvider1.SetShowHelp(this.clearResultsCheckBox, true);
			this.clearResultsCheckBox.Size = new System.Drawing.Size(232, 24);
			this.clearResultsCheckBox.TabIndex = 12;
			this.clearResultsCheckBox.Text = "Clear results when tests change.";
			// 
			// reloadOnChangeCheckBox
			// 
			this.helpProvider1.SetHelpString(this.reloadOnChangeCheckBox, "If checked, the assembly is reloaded whenever it changes. Changes to this setting" +
				" do not take effect until the next time an assembly is loaded.");
			this.reloadOnChangeCheckBox.Location = new System.Drawing.Point(24, 112);
			this.reloadOnChangeCheckBox.Name = "reloadOnChangeCheckBox";
			this.helpProvider1.SetShowHelp(this.reloadOnChangeCheckBox, true);
			this.reloadOnChangeCheckBox.Size = new System.Drawing.Size(256, 24);
			this.reloadOnChangeCheckBox.TabIndex = 11;
			this.reloadOnChangeCheckBox.Text = "Reload when test assembly changes";
			this.reloadOnChangeCheckBox.CheckedChanged += new System.EventHandler(this.enableWatcherCheckBox_CheckedChanged);
			// 
			// label1
			// 
			this.helpProvider1.SetHelpString(this.label1, "");
			this.label1.Location = new System.Drawing.Point(40, 48);
			this.label1.Name = "label1";
			this.helpProvider1.SetShowHelp(this.label1, true);
			this.label1.Size = new System.Drawing.Size(96, 24);
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
			this.initialDisplayComboBox.Location = new System.Drawing.Point(144, 48);
			this.initialDisplayComboBox.Name = "initialDisplayComboBox";
			this.helpProvider1.SetShowHelp(this.initialDisplayComboBox, true);
			this.initialDisplayComboBox.Size = new System.Drawing.Size(120, 24);
			this.initialDisplayComboBox.TabIndex = 17;
			// 
			// reloadOnRunCheckBox
			// 
			this.helpProvider1.SetHelpString(this.reloadOnRunCheckBox, "If checked, the assembly is reloaded before each run.");
			this.reloadOnRunCheckBox.Location = new System.Drawing.Point(24, 80);
			this.reloadOnRunCheckBox.Name = "reloadOnRunCheckBox";
			this.helpProvider1.SetShowHelp(this.reloadOnRunCheckBox, true);
			this.reloadOnRunCheckBox.Size = new System.Drawing.Size(264, 24);
			this.reloadOnRunCheckBox.TabIndex = 18;
			this.reloadOnRunCheckBox.Text = "Reload before each test run";
			// 
			// visualStudioSupportCheckBox
			// 
			this.helpProvider1.SetHelpString(this.visualStudioSupportCheckBox, "If checked, Visual Studio projects and solutions may be opened or added to existi" +
				"ng test projects.");
			this.visualStudioSupportCheckBox.Location = new System.Drawing.Point(24, 176);
			this.visualStudioSupportCheckBox.Name = "visualStudioSupportCheckBox";
			this.helpProvider1.SetShowHelp(this.visualStudioSupportCheckBox, true);
			this.visualStudioSupportCheckBox.Size = new System.Drawing.Size(264, 32);
			this.visualStudioSupportCheckBox.TabIndex = 19;
			this.visualStudioSupportCheckBox.Text = "Enable Visual Studio Support";
			// 
			// OptionsDialog
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(298, 250);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.visualStudioSupportCheckBox,
																		  this.reloadOnRunCheckBox,
																		  this.initialDisplayComboBox,
																		  this.label1,
																		  this.loadLastProjectCheckBox,
																		  this.clearResultsCheckBox,
																		  this.reloadOnChangeCheckBox,
																		  this.cancelButton,
																		  this.okButton});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.HelpButton = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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
			loadLastProjectCheckBox.Checked = options.LoadLastProject;

			reloadOnChangeCheckBox.Checked = options.ReloadOnChange;
			reloadOnRunCheckBox.Checked = options.ReloadOnRun;
			clearResultsCheckBox.Enabled = reloadOnChangeCheckBox.Checked;
			clearResultsCheckBox.Checked = options.ClearResults;
			visualStudioSupportCheckBox.Checked = options.VisualStudioSupport;

			initialDisplayComboBox.SelectedIndex = options.InitialTreeDisplay;
		}

		private void okButton_Click(object sender, System.EventArgs e)
		{
			if ( options.ReloadOnChange != reloadOnChangeCheckBox.Checked )
			{
				string msg = String.Format(
					"Watching for file changes will be {0} the next time you load an assembly.",
					reloadOnChangeCheckBox.Checked ? "enabled" : "disabled" );

				UserMessage.DisplayInfo( msg, "NUnit Options" );
			}

			options.LoadLastProject = loadLastProjectCheckBox.Checked;
			
			options.ReloadOnChange = reloadOnChangeCheckBox.Checked;
			options.ReloadOnRun = reloadOnRunCheckBox.Checked;
			options.ClearResults = clearResultsCheckBox.Checked;
			options.VisualStudioSupport = visualStudioSupportCheckBox.Checked;

			options.InitialTreeDisplay = initialDisplayComboBox.SelectedIndex;

			DialogResult = DialogResult.OK;

			Close();
		}

		private void enableWatcherCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			clearResultsCheckBox.Enabled = reloadOnChangeCheckBox.Checked;
		}
	}
}
