// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org/?p=license&r=2.4.
// ****************************************************************

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
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.HelpProvider helpProvider1;
		private System.Windows.Forms.CheckBox clearResultsCheckBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox initialDisplayComboBox;
		private System.Windows.Forms.CheckBox reloadOnChangeCheckBox;
		private System.Windows.Forms.CheckBox reloadOnRunCheckBox;
		private System.Windows.Forms.CheckBox loadLastProjectCheckBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox recentFilesCountTextBox;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.GroupBox groupBox6;
		private System.Windows.Forms.RadioButton multiDomainRadioButton;
		private System.Windows.Forms.RadioButton singleDomainRadioButton;
		private System.Windows.Forms.CheckBox mergeAssembliesCheckBox;
		private System.Windows.Forms.GroupBox groupBox7;
		private System.Windows.Forms.RadioButton autoNamespaceSuites;
		private System.Windows.Forms.RadioButton flatTestList;
		private System.Windows.Forms.CheckBox rerunOnChangeCheckBox;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.GroupBox groupBox8;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.CheckBox visualStudioSupportCheckBox;
		private System.Windows.Forms.CheckBox labelTestOutputCheckBox;
		private System.Windows.Forms.CheckBox enableWordWrap;
		private System.Windows.Forms.CheckBox failureToolTips;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.CheckBox errorsTabCheckBox;
		private System.Windows.Forms.CheckBox notRunTabCheckBox;
		private System.Windows.Forms.CheckBox consoleOutputCheckBox;
		private System.Windows.Forms.CheckBox traceOutputCheckBox;
		private System.Windows.Forms.CheckBox consoleErrrorCheckBox;
		private System.Windows.Forms.RadioButton separateErrors;
		private System.Windows.Forms.RadioButton mergeErrors;
		private System.Windows.Forms.RadioButton mergeTrace;
		private System.Windows.Forms.RadioButton separateTrace;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.GroupBox groupBox9;
		private System.Windows.Forms.CheckBox shadowCopyCheckBox;

		private ISettings settings;

		public OptionsDialog()
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
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.recentFilesCountTextBox = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.rerunOnChangeCheckBox = new System.Windows.Forms.CheckBox();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.groupBox9 = new System.Windows.Forms.GroupBox();
			this.shadowCopyCheckBox = new System.Windows.Forms.CheckBox();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.visualStudioSupportCheckBox = new System.Windows.Forms.CheckBox();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.groupBox7 = new System.Windows.Forms.GroupBox();
			this.flatTestList = new System.Windows.Forms.RadioButton();
			this.autoNamespaceSuites = new System.Windows.Forms.RadioButton();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.mergeAssembliesCheckBox = new System.Windows.Forms.CheckBox();
			this.singleDomainRadioButton = new System.Windows.Forms.RadioButton();
			this.multiDomainRadioButton = new System.Windows.Forms.RadioButton();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.panel2 = new System.Windows.Forms.Panel();
			this.separateTrace = new System.Windows.Forms.RadioButton();
			this.traceOutputCheckBox = new System.Windows.Forms.CheckBox();
			this.mergeTrace = new System.Windows.Forms.RadioButton();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.errorsTabCheckBox = new System.Windows.Forms.CheckBox();
			this.failureToolTips = new System.Windows.Forms.CheckBox();
			this.enableWordWrap = new System.Windows.Forms.CheckBox();
			this.notRunTabCheckBox = new System.Windows.Forms.CheckBox();
			this.groupBox8 = new System.Windows.Forms.GroupBox();
			this.consoleOutputCheckBox = new System.Windows.Forms.CheckBox();
			this.labelTestOutputCheckBox = new System.Windows.Forms.CheckBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.consoleErrrorCheckBox = new System.Windows.Forms.CheckBox();
			this.mergeErrors = new System.Windows.Forms.RadioButton();
			this.separateErrors = new System.Windows.Forms.RadioButton();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.groupBox9.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.groupBox7.SuspendLayout();
			this.groupBox6.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.panel2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox8.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// okButton
			// 
			this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.okButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.okButton.Location = new System.Drawing.Point(77, 434);
			this.okButton.Name = "okButton";
			this.helpProvider1.SetShowHelp(this.okButton, false);
			this.okButton.Size = new System.Drawing.Size(76, 23);
			this.okButton.TabIndex = 15;
			this.okButton.Text = "OK";
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.cancelButton.CausesValidation = false;
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.cancelButton.Location = new System.Drawing.Point(173, 434);
			this.cancelButton.Name = "cancelButton";
			this.helpProvider1.SetShowHelp(this.cancelButton, false);
			this.cancelButton.Size = new System.Drawing.Size(68, 23);
			this.cancelButton.TabIndex = 16;
			this.cancelButton.Text = "Cancel";
			// 
			// loadLastProjectCheckBox
			// 
			this.helpProvider1.SetHelpString(this.loadLastProjectCheckBox, "If checked, most recent project is loaded at startup.");
			this.loadLastProjectCheckBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.loadLastProjectCheckBox.Location = new System.Drawing.Point(19, 65);
			this.loadLastProjectCheckBox.Name = "loadLastProjectCheckBox";
			this.helpProvider1.SetShowHelp(this.loadLastProjectCheckBox, true);
			this.loadLastProjectCheckBox.Size = new System.Drawing.Size(250, 24);
			this.loadLastProjectCheckBox.TabIndex = 4;
			this.loadLastProjectCheckBox.Text = "Load most recent project at startup.";
			// 
			// clearResultsCheckBox
			// 
			this.helpProvider1.SetHelpString(this.clearResultsCheckBox, "If checked, any prior results are cleared when reloading.");
			this.clearResultsCheckBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.clearResultsCheckBox.Location = new System.Drawing.Point(19, 65);
			this.clearResultsCheckBox.Name = "clearResultsCheckBox";
			this.helpProvider1.SetShowHelp(this.clearResultsCheckBox, true);
			this.clearResultsCheckBox.Size = new System.Drawing.Size(232, 24);
			this.clearResultsCheckBox.TabIndex = 10;
			this.clearResultsCheckBox.Text = "Clear results when reloading.";
			// 
			// reloadOnChangeCheckBox
			// 
			this.helpProvider1.SetHelpString(this.reloadOnChangeCheckBox, "If checked, the assembly is reloaded whenever it changes. Changes to this setting" +
				" do not take effect until the next time an assembly is loaded.");
			this.reloadOnChangeCheckBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.reloadOnChangeCheckBox.Location = new System.Drawing.Point(19, 55);
			this.reloadOnChangeCheckBox.Name = "reloadOnChangeCheckBox";
			this.helpProvider1.SetShowHelp(this.reloadOnChangeCheckBox, true);
			this.reloadOnChangeCheckBox.Size = new System.Drawing.Size(245, 25);
			this.reloadOnChangeCheckBox.TabIndex = 9;
			this.reloadOnChangeCheckBox.Text = "Reload when test assembly changes";
			this.reloadOnChangeCheckBox.CheckedChanged += new System.EventHandler(this.reloadOnChangeCheckBox_CheckedChanged);
			// 
			// label1
			// 
			this.helpProvider1.SetHelpString(this.label1, "");
			this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label1.Location = new System.Drawing.Point(19, 28);
			this.label1.Name = "label1";
			this.helpProvider1.SetShowHelp(this.label1, true);
			this.label1.Size = new System.Drawing.Size(144, 24);
			this.label1.TabIndex = 5;
			this.label1.Text = "Initial display on load:";
			// 
			// initialDisplayComboBox
			// 
			this.initialDisplayComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.helpProvider1.SetHelpString(this.initialDisplayComboBox, "Selects the initial display style of the tree when an assembly is loaded");
			this.initialDisplayComboBox.ItemHeight = 16;
			this.initialDisplayComboBox.Items.AddRange(new object[] {
																		"Auto",
																		"Expand",
																		"Collapse",
																		"HideTests"});
			this.initialDisplayComboBox.Location = new System.Drawing.Point(173, 28);
			this.initialDisplayComboBox.Name = "initialDisplayComboBox";
			this.helpProvider1.SetShowHelp(this.initialDisplayComboBox, true);
			this.initialDisplayComboBox.Size = new System.Drawing.Size(87, 24);
			this.initialDisplayComboBox.TabIndex = 6;
			// 
			// reloadOnRunCheckBox
			// 
			this.helpProvider1.SetHelpString(this.reloadOnRunCheckBox, "If checked, the assembly is reloaded before each run.");
			this.reloadOnRunCheckBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.reloadOnRunCheckBox.Location = new System.Drawing.Point(19, 28);
			this.reloadOnRunCheckBox.Name = "reloadOnRunCheckBox";
			this.helpProvider1.SetShowHelp(this.reloadOnRunCheckBox, true);
			this.reloadOnRunCheckBox.Size = new System.Drawing.Size(237, 23);
			this.reloadOnRunCheckBox.TabIndex = 8;
			this.reloadOnRunCheckBox.Text = "Reload before each test run";
			// 
			// label2
			// 
			this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label2.Location = new System.Drawing.Point(19, 28);
			this.label2.Name = "label2";
			this.helpProvider1.SetShowHelp(this.label2, false);
			this.label2.Size = new System.Drawing.Size(55, 16);
			this.label2.TabIndex = 1;
			this.label2.Text = "Display";
			// 
			// label3
			// 
			this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label3.Location = new System.Drawing.Point(144, 28);
			this.label3.Name = "label3";
			this.helpProvider1.SetShowHelp(this.label3, false);
			this.label3.Size = new System.Drawing.Size(96, 24);
			this.label3.TabIndex = 3;
			this.label3.Text = "files in list";
			// 
			// recentFilesCountTextBox
			// 
			this.recentFilesCountTextBox.Location = new System.Drawing.Point(86, 28);
			this.recentFilesCountTextBox.Name = "recentFilesCountTextBox";
			this.helpProvider1.SetShowHelp(this.recentFilesCountTextBox, false);
			this.recentFilesCountTextBox.Size = new System.Drawing.Size(40, 22);
			this.recentFilesCountTextBox.TabIndex = 2;
			this.recentFilesCountTextBox.Text = "";
			this.recentFilesCountTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.recentFilesCountTextBox_Validating);
			this.recentFilesCountTextBox.Validated += new System.EventHandler(this.recentFilesCountTextBox_Validated);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.recentFilesCountTextBox);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.loadLastProjectCheckBox);
			this.groupBox1.Location = new System.Drawing.Point(10, 9);
			this.groupBox1.Name = "groupBox1";
			this.helpProvider1.SetShowHelp(this.groupBox1, false);
			this.groupBox1.Size = new System.Drawing.Size(283, 102);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Recent Files";
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.rerunOnChangeCheckBox);
			this.groupBox2.Controls.Add(this.reloadOnRunCheckBox);
			this.groupBox2.Controls.Add(this.reloadOnChangeCheckBox);
			this.groupBox2.Location = new System.Drawing.Point(10, 208);
			this.groupBox2.Name = "groupBox2";
			this.helpProvider1.SetShowHelp(this.groupBox2, false);
			this.groupBox2.Size = new System.Drawing.Size(283, 120);
			this.groupBox2.TabIndex = 7;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Assembly Reload";
			// 
			// rerunOnChangeCheckBox
			// 
			this.rerunOnChangeCheckBox.Enabled = false;
			this.rerunOnChangeCheckBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.rerunOnChangeCheckBox.Location = new System.Drawing.Point(40, 80);
			this.rerunOnChangeCheckBox.Name = "rerunOnChangeCheckBox";
			this.helpProvider1.SetShowHelp(this.rerunOnChangeCheckBox, false);
			this.rerunOnChangeCheckBox.Size = new System.Drawing.Size(200, 24);
			this.rerunOnChangeCheckBox.TabIndex = 10;
			this.rerunOnChangeCheckBox.Text = "Re-run last tests run";
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.ItemSize = new System.Drawing.Size(46, 18);
			this.tabControl1.Location = new System.Drawing.Point(10, 9);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.helpProvider1.SetShowHelp(this.tabControl1, false);
			this.tabControl1.Size = new System.Drawing.Size(310, 420);
			this.tabControl1.TabIndex = 2;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.groupBox9);
			this.tabPage1.Controls.Add(this.groupBox4);
			this.tabPage1.Controls.Add(this.groupBox5);
			this.tabPage1.Controls.Add(this.groupBox1);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.helpProvider1.SetShowHelp(this.tabPage1, false);
			this.tabPage1.Size = new System.Drawing.Size(302, 394);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "General";
			// 
			// groupBox9
			// 
			this.groupBox9.Controls.Add(this.shadowCopyCheckBox);
			this.groupBox9.Location = new System.Drawing.Point(8, 296);
			this.groupBox9.Name = "groupBox9";
			this.groupBox9.Size = new System.Drawing.Size(280, 48);
			this.groupBox9.TabIndex = 15;
			this.groupBox9.TabStop = false;
			this.groupBox9.Text = "Shadow Copy";
			// 
			// shadowCopyCheckBox
			// 
			this.shadowCopyCheckBox.Location = new System.Drawing.Point(19, 18);
			this.shadowCopyCheckBox.Name = "shadowCopyCheckBox";
			this.shadowCopyCheckBox.Size = new System.Drawing.Size(240, 22);
			this.shadowCopyCheckBox.TabIndex = 0;
			this.shadowCopyCheckBox.Text = "Enable Shadow Copy";
			// 
			// groupBox4
			// 
			this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox4.Controls.Add(this.visualStudioSupportCheckBox);
			this.groupBox4.Location = new System.Drawing.Point(10, 240);
			this.groupBox4.Name = "groupBox4";
			this.helpProvider1.SetShowHelp(this.groupBox4, false);
			this.groupBox4.Size = new System.Drawing.Size(283, 47);
			this.groupBox4.TabIndex = 14;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Visual Studio";
			// 
			// visualStudioSupportCheckBox
			// 
			this.helpProvider1.SetHelpString(this.visualStudioSupportCheckBox, "If checked, Visual Studio projects and solutions may be opened or added to existi" +
				"ng test projects.");
			this.visualStudioSupportCheckBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.visualStudioSupportCheckBox.Location = new System.Drawing.Point(19, 18);
			this.visualStudioSupportCheckBox.Name = "visualStudioSupportCheckBox";
			this.helpProvider1.SetShowHelp(this.visualStudioSupportCheckBox, true);
			this.visualStudioSupportCheckBox.Size = new System.Drawing.Size(264, 25);
			this.visualStudioSupportCheckBox.TabIndex = 14;
			this.visualStudioSupportCheckBox.Text = "Enable Visual Studio Support";
			// 
			// groupBox5
			// 
			this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox5.Controls.Add(this.label1);
			this.groupBox5.Controls.Add(this.initialDisplayComboBox);
			this.groupBox5.Controls.Add(this.clearResultsCheckBox);
			this.groupBox5.Location = new System.Drawing.Point(10, 120);
			this.groupBox5.Name = "groupBox5";
			this.helpProvider1.SetShowHelp(this.groupBox5, false);
			this.groupBox5.Size = new System.Drawing.Size(283, 102);
			this.groupBox5.TabIndex = 1;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Tree View";
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.groupBox7);
			this.tabPage2.Controls.Add(this.groupBox6);
			this.tabPage2.Controls.Add(this.groupBox2);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.helpProvider1.SetShowHelp(this.tabPage2, false);
			this.tabPage2.Size = new System.Drawing.Size(302, 394);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Test Load";
			// 
			// groupBox7
			// 
			this.groupBox7.Controls.Add(this.flatTestList);
			this.groupBox7.Controls.Add(this.autoNamespaceSuites);
			this.groupBox7.Location = new System.Drawing.Point(8, 8);
			this.groupBox7.Name = "groupBox7";
			this.helpProvider1.SetShowHelp(this.groupBox7, false);
			this.groupBox7.Size = new System.Drawing.Size(288, 80);
			this.groupBox7.TabIndex = 16;
			this.groupBox7.TabStop = false;
			this.groupBox7.Text = "Test Structure";
			// 
			// flatTestList
			// 
			this.flatTestList.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.flatTestList.Location = new System.Drawing.Point(24, 48);
			this.flatTestList.Name = "flatTestList";
			this.helpProvider1.SetShowHelp(this.flatTestList, false);
			this.flatTestList.Size = new System.Drawing.Size(216, 24);
			this.flatTestList.TabIndex = 1;
			this.flatTestList.Text = "Flat list of TestFixtures";
			// 
			// autoNamespaceSuites
			// 
			this.autoNamespaceSuites.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.autoNamespaceSuites.Location = new System.Drawing.Point(24, 16);
			this.autoNamespaceSuites.Name = "autoNamespaceSuites";
			this.helpProvider1.SetShowHelp(this.autoNamespaceSuites, false);
			this.autoNamespaceSuites.Size = new System.Drawing.Size(224, 24);
			this.autoNamespaceSuites.TabIndex = 0;
			this.autoNamespaceSuites.Text = "Automatic Namespace suites";
			// 
			// groupBox6
			// 
			this.groupBox6.Controls.Add(this.mergeAssembliesCheckBox);
			this.groupBox6.Controls.Add(this.singleDomainRadioButton);
			this.groupBox6.Controls.Add(this.multiDomainRadioButton);
			this.groupBox6.Location = new System.Drawing.Point(8, 96);
			this.groupBox6.Name = "groupBox6";
			this.helpProvider1.SetShowHelp(this.groupBox6, false);
			this.groupBox6.Size = new System.Drawing.Size(288, 104);
			this.groupBox6.TabIndex = 14;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "Multiple Assemblies";
			// 
			// mergeAssembliesCheckBox
			// 
			this.mergeAssembliesCheckBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.mergeAssembliesCheckBox.Location = new System.Drawing.Point(40, 72);
			this.mergeAssembliesCheckBox.Name = "mergeAssembliesCheckBox";
			this.helpProvider1.SetShowHelp(this.mergeAssembliesCheckBox, false);
			this.mergeAssembliesCheckBox.Size = new System.Drawing.Size(224, 24);
			this.mergeAssembliesCheckBox.TabIndex = 2;
			this.mergeAssembliesCheckBox.Text = "Merge tests across assemblies";
			// 
			// singleDomainRadioButton
			// 
			this.singleDomainRadioButton.Checked = true;
			this.singleDomainRadioButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.singleDomainRadioButton.Location = new System.Drawing.Point(24, 48);
			this.singleDomainRadioButton.Name = "singleDomainRadioButton";
			this.helpProvider1.SetShowHelp(this.singleDomainRadioButton, false);
			this.singleDomainRadioButton.Size = new System.Drawing.Size(240, 24);
			this.singleDomainRadioButton.TabIndex = 1;
			this.singleDomainRadioButton.TabStop = true;
			this.singleDomainRadioButton.Text = "Load in a single AppDomain";
			this.singleDomainRadioButton.CheckedChanged += new System.EventHandler(this.singleDomainRadioButton_CheckedChanged);
			// 
			// multiDomainRadioButton
			// 
			this.multiDomainRadioButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.multiDomainRadioButton.Location = new System.Drawing.Point(24, 24);
			this.multiDomainRadioButton.Name = "multiDomainRadioButton";
			this.helpProvider1.SetShowHelp(this.multiDomainRadioButton, false);
			this.multiDomainRadioButton.Size = new System.Drawing.Size(240, 24);
			this.multiDomainRadioButton.TabIndex = 0;
			this.multiDomainRadioButton.Text = "Load in separate AppDomains";
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.panel2);
			this.tabPage3.Controls.Add(this.groupBox3);
			this.tabPage3.Controls.Add(this.groupBox8);
			this.tabPage3.Location = new System.Drawing.Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.helpProvider1.SetShowHelp(this.tabPage3, false);
			this.tabPage3.Size = new System.Drawing.Size(302, 394);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "Test Output";
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.separateTrace);
			this.panel2.Controls.Add(this.traceOutputCheckBox);
			this.panel2.Controls.Add(this.mergeTrace);
			this.panel2.Location = new System.Drawing.Point(8, 296);
			this.panel2.Name = "panel2";
			this.helpProvider1.SetShowHelp(this.panel2, false);
			this.panel2.Size = new System.Drawing.Size(248, 80);
			this.panel2.TabIndex = 14;
			// 
			// separateTrace
			// 
			this.separateTrace.Checked = true;
			this.separateTrace.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.separateTrace.Location = new System.Drawing.Point(32, 32);
			this.separateTrace.Name = "separateTrace";
			this.helpProvider1.SetShowHelp(this.separateTrace, false);
			this.separateTrace.Size = new System.Drawing.Size(224, 16);
			this.separateTrace.TabIndex = 18;
			this.separateTrace.TabStop = true;
			this.separateTrace.Text = "In Separate Tab";
			// 
			// traceOutputCheckBox
			// 
			this.traceOutputCheckBox.Checked = true;
			this.traceOutputCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.traceOutputCheckBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.traceOutputCheckBox.Location = new System.Drawing.Point(8, 8);
			this.traceOutputCheckBox.Name = "traceOutputCheckBox";
			this.helpProvider1.SetShowHelp(this.traceOutputCheckBox, false);
			this.traceOutputCheckBox.Size = new System.Drawing.Size(208, 16);
			this.traceOutputCheckBox.TabIndex = 14;
			this.traceOutputCheckBox.Text = "Display Trace Output";
			this.traceOutputCheckBox.CheckedChanged += new System.EventHandler(this.traceOutputCheckBox_CheckedChanged);
			// 
			// mergeTrace
			// 
			this.mergeTrace.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.mergeTrace.Location = new System.Drawing.Point(32, 56);
			this.mergeTrace.Name = "mergeTrace";
			this.helpProvider1.SetShowHelp(this.mergeTrace, false);
			this.mergeTrace.Size = new System.Drawing.Size(192, 16);
			this.mergeTrace.TabIndex = 19;
			this.mergeTrace.Text = "Merge with Console Output";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.errorsTabCheckBox);
			this.groupBox3.Controls.Add(this.failureToolTips);
			this.groupBox3.Controls.Add(this.enableWordWrap);
			this.groupBox3.Controls.Add(this.notRunTabCheckBox);
			this.groupBox3.Location = new System.Drawing.Point(0, 8);
			this.groupBox3.Name = "groupBox3";
			this.helpProvider1.SetShowHelp(this.groupBox3, false);
			this.groupBox3.Size = new System.Drawing.Size(296, 128);
			this.groupBox3.TabIndex = 13;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Test Results";
			// 
			// errorsTabCheckBox
			// 
			this.errorsTabCheckBox.Checked = true;
			this.errorsTabCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.errorsTabCheckBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.errorsTabCheckBox.Location = new System.Drawing.Point(16, 16);
			this.errorsTabCheckBox.Name = "errorsTabCheckBox";
			this.helpProvider1.SetShowHelp(this.errorsTabCheckBox, false);
			this.errorsTabCheckBox.Size = new System.Drawing.Size(248, 24);
			this.errorsTabCheckBox.TabIndex = 15;
			this.errorsTabCheckBox.Text = "Display Errors and Failures";
			this.errorsTabCheckBox.CheckedChanged += new System.EventHandler(this.errorsTabCheckBox_CheckedChanged);
			// 
			// failureToolTips
			// 
			this.failureToolTips.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.failureToolTips.Location = new System.Drawing.Point(32, 40);
			this.failureToolTips.Name = "failureToolTips";
			this.helpProvider1.SetShowHelp(this.failureToolTips, false);
			this.failureToolTips.Size = new System.Drawing.Size(202, 19);
			this.failureToolTips.TabIndex = 13;
			this.failureToolTips.Text = "Display Failure ToolTips";
			// 
			// enableWordWrap
			// 
			this.enableWordWrap.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.enableWordWrap.Location = new System.Drawing.Point(32, 64);
			this.enableWordWrap.Name = "enableWordWrap";
			this.helpProvider1.SetShowHelp(this.enableWordWrap, false);
			this.enableWordWrap.Size = new System.Drawing.Size(248, 19);
			this.enableWordWrap.TabIndex = 14;
			this.enableWordWrap.Text = "Enable Word Wrap";
			// 
			// notRunTabCheckBox
			// 
			this.notRunTabCheckBox.Checked = true;
			this.notRunTabCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.notRunTabCheckBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.notRunTabCheckBox.Location = new System.Drawing.Point(16, 96);
			this.notRunTabCheckBox.Name = "notRunTabCheckBox";
			this.helpProvider1.SetShowHelp(this.notRunTabCheckBox, false);
			this.notRunTabCheckBox.Size = new System.Drawing.Size(264, 16);
			this.notRunTabCheckBox.TabIndex = 0;
			this.notRunTabCheckBox.Text = "Display Tests Not Run";
			// 
			// groupBox8
			// 
			this.groupBox8.Controls.Add(this.consoleOutputCheckBox);
			this.groupBox8.Controls.Add(this.labelTestOutputCheckBox);
			this.groupBox8.Controls.Add(this.panel1);
			this.groupBox8.Location = new System.Drawing.Point(0, 152);
			this.groupBox8.Name = "groupBox8";
			this.helpProvider1.SetShowHelp(this.groupBox8, false);
			this.groupBox8.Size = new System.Drawing.Size(298, 232);
			this.groupBox8.TabIndex = 12;
			this.groupBox8.TabStop = false;
			this.groupBox8.Text = "Text Output";
			// 
			// consoleOutputCheckBox
			// 
			this.consoleOutputCheckBox.Checked = true;
			this.consoleOutputCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.consoleOutputCheckBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.consoleOutputCheckBox.Location = new System.Drawing.Point(16, 18);
			this.consoleOutputCheckBox.Name = "consoleOutputCheckBox";
			this.helpProvider1.SetShowHelp(this.consoleOutputCheckBox, false);
			this.consoleOutputCheckBox.Size = new System.Drawing.Size(240, 16);
			this.consoleOutputCheckBox.TabIndex = 13;
			this.consoleOutputCheckBox.Text = "Display Console Standard Output";
			this.consoleOutputCheckBox.CheckedChanged += new System.EventHandler(this.consoleOutputCheckBox_CheckedChanged);
			// 
			// labelTestOutputCheckBox
			// 
			this.labelTestOutputCheckBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.labelTestOutputCheckBox.Location = new System.Drawing.Point(32, 40);
			this.labelTestOutputCheckBox.Name = "labelTestOutputCheckBox";
			this.helpProvider1.SetShowHelp(this.labelTestOutputCheckBox, false);
			this.labelTestOutputCheckBox.Size = new System.Drawing.Size(237, 19);
			this.labelTestOutputCheckBox.TabIndex = 12;
			this.labelTestOutputCheckBox.Text = "Label Test Cases";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.consoleErrrorCheckBox);
			this.panel1.Controls.Add(this.mergeErrors);
			this.panel1.Controls.Add(this.separateErrors);
			this.panel1.Location = new System.Drawing.Point(8, 64);
			this.panel1.Name = "panel1";
			this.helpProvider1.SetShowHelp(this.panel1, false);
			this.panel1.Size = new System.Drawing.Size(248, 80);
			this.panel1.TabIndex = 14;
			// 
			// consoleErrrorCheckBox
			// 
			this.consoleErrrorCheckBox.Checked = true;
			this.consoleErrrorCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.consoleErrrorCheckBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.consoleErrrorCheckBox.Location = new System.Drawing.Point(8, 8);
			this.consoleErrrorCheckBox.Name = "consoleErrrorCheckBox";
			this.helpProvider1.SetShowHelp(this.consoleErrrorCheckBox, false);
			this.consoleErrrorCheckBox.Size = new System.Drawing.Size(232, 24);
			this.consoleErrrorCheckBox.TabIndex = 15;
			this.consoleErrrorCheckBox.Text = "Display Console Error Output";
			this.consoleErrrorCheckBox.CheckedChanged += new System.EventHandler(this.consoleErrrorCheckBox_CheckedChanged);
			// 
			// mergeErrors
			// 
			this.mergeErrors.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.mergeErrors.Location = new System.Drawing.Point(32, 56);
			this.mergeErrors.Name = "mergeErrors";
			this.helpProvider1.SetShowHelp(this.mergeErrors, false);
			this.mergeErrors.Size = new System.Drawing.Size(192, 16);
			this.mergeErrors.TabIndex = 17;
			this.mergeErrors.Text = "Merge with Console Output";
			// 
			// separateErrors
			// 
			this.separateErrors.Checked = true;
			this.separateErrors.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.separateErrors.Location = new System.Drawing.Point(32, 32);
			this.separateErrors.Name = "separateErrors";
			this.helpProvider1.SetShowHelp(this.separateErrors, false);
			this.separateErrors.Size = new System.Drawing.Size(224, 16);
			this.separateErrors.TabIndex = 16;
			this.separateErrors.TabStop = true;
			this.separateErrors.Text = "In Separate Tab";
			// 
			// OptionsDialog
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.ClientSize = new System.Drawing.Size(322, 458);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.HelpButton = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "OptionsDialog";
			this.helpProvider1.SetShowHelp(this, false);
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "NUnit Options";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.OptionsDialog_Closing);
			this.Load += new System.EventHandler(this.OptionsDialog_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.groupBox9.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.groupBox5.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.groupBox7.ResumeLayout(false);
			this.groupBox6.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox8.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void OptionsDialog_Load(object sender, System.EventArgs e)
		{
			this.settings = Services.UserSettings;

			recentFilesCountTextBox.Text = Services.RecentFiles.MaxFiles.ToString();

			loadLastProjectCheckBox.Checked = settings.GetSetting( "Options.LoadLastProject", true );
			initialDisplayComboBox.SelectedIndex = (int)(TestSuiteTreeView.DisplayStyle)settings.GetSetting( "Gui.TestTree.InitialTreeDisplay", TestSuiteTreeView.DisplayStyle.Auto );

			visualStudioSupportCheckBox.Checked = settings.GetSetting( "Options.TestLoader.VisualStudioSupport", false );

			shadowCopyCheckBox.Checked = settings.GetSetting( "Options.TestLoader.ShadowCopyFiles", true );

			reloadOnChangeCheckBox.Enabled = Environment.OSVersion.Platform == System.PlatformID.Win32NT;
			reloadOnChangeCheckBox.Checked = settings.GetSetting( "Options.TestLoader.ReloadOnChange", true );
			rerunOnChangeCheckBox.Checked = settings.GetSetting( "Options.TestLoader.RerunOnChange", false );
			reloadOnRunCheckBox.Checked = settings.GetSetting( "Options.TestLoader.ReloadOnRun", true );
			clearResultsCheckBox.Checked = settings.GetSetting( "Options.TestLoader.ClearResultsOnReload", true );

			bool multiDomain = settings.GetSetting( "Options.TestLoader.MultiDomain", false );
			multiDomainRadioButton.Checked = multiDomain;
			singleDomainRadioButton.Checked = !multiDomain;
			mergeAssembliesCheckBox.Enabled = !multiDomain;
			mergeAssembliesCheckBox.Checked = settings.GetSetting( "Options.TestLoader.MergeAssemblies", false );
			autoNamespaceSuites.Checked = settings.GetSetting( "Options.TestLoader.AutoNamespaceSuites", true );
			flatTestList.Checked = !autoNamespaceSuites.Checked;

			errorsTabCheckBox.Checked = settings.GetSetting( "Gui.ResultTabs.DisplayErrorsTab", true );
			notRunTabCheckBox.Checked = settings.GetSetting( "Gui.ResultTabs.DisplayNotRunTab", true );
			consoleOutputCheckBox.Checked = settings.GetSetting( "Gui.ResultTabs.DisplayConsoleOutputTab", true );
			consoleErrrorCheckBox.Checked = settings.GetSetting( "Gui.ResultTabs.DisplayConsoleErrorTab", true )
				|| settings.GetSetting( "Gui.ResultTabs.MergeErrorOutput", false );
			traceOutputCheckBox.Checked = settings.GetSetting( "Gui.ResultTabs.DisplayTraceTab", true )
				|| settings.GetSetting( "Gui.ResultTabs.MergeTraceOutput", false );

			labelTestOutputCheckBox.Checked = settings.GetSetting( "Gui.ResultTabs.DisplayTestLabels", false );
			failureToolTips.Checked = settings.GetSetting( "Gui.ResultTabs.ErrorsTab.ToolTipsEnabled", true );
			enableWordWrap.Checked = settings.GetSetting( "Gui.ResultTabs.ErrorsTab.WordWrapEnabled", true );

			mergeErrors.Checked = settings.GetSetting( "Gui.ResultTabs.MergeErrorOutput", false );
			mergeTrace.Checked = settings.GetSetting( "Gui.ResultTabs.MergeTraceOutput", false );
			

		}

		private void okButton_Click(object sender, System.EventArgs e)
		{
			if ( settings.GetSetting( "Options.TestLoader.ReloadOnChange", true ) != reloadOnChangeCheckBox.Checked )
			{
				string msg = String.Format(
					"Watching for file changes will be {0} the next time you load an assembly.",
					reloadOnChangeCheckBox.Checked ? "enabled" : "disabled" );

				UserMessage.DisplayInfo( msg, "NUnit Options" );
			}

			settings.SaveSetting( "Options.LoadLastProject", loadLastProjectCheckBox.Checked );
			
			TestLoader loader = Services.TestLoader;
			settings.SaveSetting( "Options.TestLoader.ReloadOnChange", loader.ReloadOnChange = reloadOnChangeCheckBox.Checked );
			settings.SaveSetting( "Options.TestLoader.RerunOnChange", loader.RerunOnChange = rerunOnChangeCheckBox.Checked );
			settings.SaveSetting( "Options.TestLoader.ReloadOnRun", loader.ReloadOnRun = reloadOnRunCheckBox.Checked );
			settings.SaveSetting( "Options.TestLoader.ClearResultsOnReload", clearResultsCheckBox.Checked );

			settings.SaveSetting( "Options.TestLoader.MultiDomain", loader.MultiDomain = multiDomainRadioButton.Checked );
			settings.SaveSetting( "Options.TestLoader.MergeAssemblies", loader.MergeAssemblies = mergeAssembliesCheckBox.Checked );
			settings.SaveSetting( "Options.TestLoader.AutoNamespaceSuites", loader.AutoNamespaceSuites = autoNamespaceSuites.Checked );

			settings.SaveSetting( "Gui.ResultTabs.DisplayTestLabels", labelTestOutputCheckBox.Checked );
			settings.SaveSetting( "Gui.ResultTabs.ErrorsTab.ToolTipsEnabled", failureToolTips.Checked );
			settings.SaveSetting( "Gui.ResultTabs.ErrorsTab.WordWrapEnabled", enableWordWrap.Checked );
			
			settings.SaveSetting( "Options.TestLoader.VisualStudioSupport", visualStudioSupportCheckBox.Checked );

			settings.SaveSetting( "Options.TestLoader.ShadowCopyFiles", loader.ShadowCopyFiles = shadowCopyCheckBox.Checked );

			settings.SaveSetting( "Gui.TestTree.InitialTreeDisplay", (TestSuiteTreeView.DisplayStyle)initialDisplayComboBox.SelectedIndex );

			settings.SaveSetting( "Gui.ResultTabs.DisplayErrorsTab", errorsTabCheckBox.Checked );
			settings.SaveSetting( "Gui.ResultTabs.DisplayNotRunTab", notRunTabCheckBox.Checked );
			settings.SaveSetting( "Gui.ResultTabs.DisplayConsoleOutputTab", consoleOutputCheckBox.Checked );
			settings.SaveSetting( "Gui.ResultTabs.DisplayConsoleErrorTab", consoleErrrorCheckBox.Checked && separateErrors.Checked );
			settings.SaveSetting( "Gui.ResultTabs.DisplayTraceTab", traceOutputCheckBox.Checked && separateTrace.Checked );

			settings.SaveSetting( "Gui.ResultTabs.MergeErrorOutput", mergeErrors.Checked );
			settings.SaveSetting( "Gui.ResultTabs.MergeTraceOutput", mergeTrace.Checked );

			DialogResult = DialogResult.OK;

			Close();
		}

		private void recentFilesCountTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if ( recentFilesCountTextBox.Text.Length == 0 )
			{
				recentFilesCountTextBox.Text = Services.RecentFiles.MaxFiles.ToString();
				recentFilesCountTextBox.SelectAll();
				e.Cancel = true;
			}
			else
			{
				string errmsg = null;

				try
				{
					int count = int.Parse( recentFilesCountTextBox.Text );

					if ( count < RecentFilesService.MinSize ||
						count > RecentFilesService.MaxSize )
					{
						errmsg = string.Format( "Number of files must be from {0} to {1}", 
							RecentFilesService.MinSize, RecentFilesService.MaxSize );
					}
				}
				catch
				{
					errmsg = "Number of files must be numeric";
				}

				if ( errmsg != null )
				{
					recentFilesCountTextBox.SelectAll();
					UserMessage.DisplayFailure( errmsg );
					e.Cancel = true;
				}
			}
		}

		private void recentFilesCountTextBox_Validated(object sender, System.EventArgs e)
		{
			int count = int.Parse( recentFilesCountTextBox.Text );
			Services.RecentFiles.MaxFiles = count;
		}

		private void OptionsDialog_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			e.Cancel = false;
		}

		private void singleDomainRadioButton_CheckedChanged(object sender, System.EventArgs e)
		{
			mergeAssembliesCheckBox.Enabled = singleDomainRadioButton.Checked;
		}

		private void reloadOnChangeCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			rerunOnChangeCheckBox.Enabled = reloadOnChangeCheckBox.Checked;
			rerunOnChangeCheckBox.Checked = false;
		}

		private void errorsTabCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			this.failureToolTips.Enabled = this.enableWordWrap.Enabled = this.errorsTabCheckBox.Checked;
		}

		private void consoleOutputCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			this.labelTestOutputCheckBox.Enabled = this.consoleOutputCheckBox.Checked;
		}

		private void consoleErrrorCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			this.separateErrors.Enabled = this.mergeErrors.Enabled = this.consoleErrrorCheckBox.Checked;
		}

		private void traceOutputCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			this.separateTrace.Enabled = this.mergeTrace.Enabled = this.traceOutputCheckBox.Checked;
		}
	}
}
