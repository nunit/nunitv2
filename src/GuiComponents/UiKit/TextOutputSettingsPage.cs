using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using NUnit.Core;
using NUnit.UiKit;

namespace NUnit.UiKit
{
	public class TextOutputSettingsPage : NUnit.UiKit.SettingsPage
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.CheckBox showStandardOutput;
		private System.Windows.Forms.CheckBox showErrorOutput;
		private System.Windows.Forms.CheckBox showTraceOutput;
		private System.Windows.Forms.CheckBox showLogOutput;
		private System.Windows.Forms.ComboBox tabSelectComboBox;
		private System.Windows.Forms.Button useDefaultsButton;
		private System.Windows.Forms.CheckBox testCaseLabels;
		private System.Windows.Forms.CheckBox suppressLabelsIfNoOutput;
		private System.ComponentModel.IContainer components = null;

		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.CheckBox displayTab;

		private TextDisplayTabSettings tabSettings = new TextDisplayTabSettings();

		public TextOutputSettingsPage(string key) : base(key)
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.showStandardOutput = new System.Windows.Forms.CheckBox();
			this.showErrorOutput = new System.Windows.Forms.CheckBox();
			this.showTraceOutput = new System.Windows.Forms.CheckBox();
			this.showLogOutput = new System.Windows.Forms.CheckBox();
			this.testCaseLabels = new System.Windows.Forms.CheckBox();
			this.tabSelectComboBox = new System.Windows.Forms.ComboBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.useDefaultsButton = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label3 = new System.Windows.Forms.Label();
			this.suppressLabelsIfNoOutput = new System.Windows.Forms.CheckBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.label4 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.displayTab = new System.Windows.Forms.CheckBox();
			this.label5 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(224, 16);
			this.label1.TabIndex = 11;
			this.label1.Text = "Show Settings for";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 120);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(104, 16);
			this.label2.TabIndex = 16;
			this.label2.Text = "Content";
			// 
			// showStandardOutput
			// 
			this.showStandardOutput.Location = new System.Drawing.Point(40, 144);
			this.showStandardOutput.Name = "showStandardOutput";
			this.showStandardOutput.Size = new System.Drawing.Size(136, 24);
			this.showStandardOutput.TabIndex = 17;
			this.showStandardOutput.Text = "Standard Output";
			this.showStandardOutput.CheckedChanged += new System.EventHandler(this.showStandardOutput_CheckedChanged);
			// 
			// showErrorOutput
			// 
			this.showErrorOutput.Location = new System.Drawing.Point(200, 144);
			this.showErrorOutput.Name = "showErrorOutput";
			this.showErrorOutput.Size = new System.Drawing.Size(128, 24);
			this.showErrorOutput.TabIndex = 18;
			this.showErrorOutput.Text = "Error Output";
			this.showErrorOutput.CheckedChanged += new System.EventHandler(this.showErrorOutput_CheckedChanged);
			// 
			// showTraceOutput
			// 
			this.showTraceOutput.Location = new System.Drawing.Point(40, 176);
			this.showTraceOutput.Name = "showTraceOutput";
			this.showTraceOutput.Size = new System.Drawing.Size(120, 24);
			this.showTraceOutput.TabIndex = 19;
			this.showTraceOutput.Text = "Trace Output";
			this.showTraceOutput.CheckedChanged += new System.EventHandler(this.showTraceOutput_CheckedChanged);
			// 
			// showLogOutput
			// 
			this.showLogOutput.Location = new System.Drawing.Point(200, 176);
			this.showLogOutput.Name = "showLogOutput";
			this.showLogOutput.Size = new System.Drawing.Size(120, 24);
			this.showLogOutput.TabIndex = 20;
			this.showLogOutput.Text = "Log Output";
			this.showLogOutput.CheckedChanged += new System.EventHandler(this.showLogOutput_CheckedChanged);
			// 
			// testCaseLabels
			// 
			this.testCaseLabels.Location = new System.Drawing.Point(40, 240);
			this.testCaseLabels.Name = "testCaseLabels";
			this.testCaseLabels.Size = new System.Drawing.Size(184, 24);
			this.testCaseLabels.TabIndex = 21;
			this.testCaseLabels.Text = "Display TestCase Labels";
			this.testCaseLabels.CheckedChanged += new System.EventHandler(this.testCaseLabels_CheckedChanged);
			// 
			// tabSelectComboBox
			// 
			this.tabSelectComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.tabSelectComboBox.Items.AddRange(new object[] {
																   "Console.Out",
																   "Console.Error",
																   "Trace",
																   "Log"});
			this.tabSelectComboBox.Location = new System.Drawing.Point(8, 16);
			this.tabSelectComboBox.Name = "tabSelectComboBox";
			this.tabSelectComboBox.Size = new System.Drawing.Size(240, 24);
			this.tabSelectComboBox.TabIndex = 22;
			this.tabSelectComboBox.SelectedIndexChanged += new System.EventHandler(this.tabSelectComboBox_SelectedIndexChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Location = new System.Drawing.Point(56, 120);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(400, 8);
			this.groupBox2.TabIndex = 24;
			this.groupBox2.TabStop = false;
			// 
			// useDefaultsButton
			// 
			this.useDefaultsButton.Location = new System.Drawing.Point(280, 16);
			this.useDefaultsButton.Name = "useDefaultsButton";
			this.useDefaultsButton.Size = new System.Drawing.Size(160, 23);
			this.useDefaultsButton.TabIndex = 25;
			this.useDefaultsButton.Text = "Restore Default Tabs";
			this.useDefaultsButton.Click += new System.EventHandler(this.button1_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Location = new System.Drawing.Point(80, 216);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(376, 8);
			this.groupBox1.TabIndex = 27;
			this.groupBox1.TabStop = false;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 216);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(104, 16);
			this.label3.TabIndex = 26;
			this.label3.Text = "Test Labels";
			// 
			// suppressLabelsIfNoOutput
			// 
			this.suppressLabelsIfNoOutput.Enabled = false;
			this.suppressLabelsIfNoOutput.Location = new System.Drawing.Point(72, 264);
			this.suppressLabelsIfNoOutput.Name = "suppressLabelsIfNoOutput";
			this.suppressLabelsIfNoOutput.Size = new System.Drawing.Size(296, 24);
			this.suppressLabelsIfNoOutput.TabIndex = 0;
			this.suppressLabelsIfNoOutput.Text = "Suppress label if no output is displayed";
			this.suppressLabelsIfNoOutput.CheckedChanged += new System.EventHandler(this.suppressLabelsIfNoOutput_CheckedChanged);
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.Location = new System.Drawing.Point(80, 56);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(376, 8);
			this.groupBox3.TabIndex = 29;
			this.groupBox3.TabStop = false;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 56);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(104, 16);
			this.label4.TabIndex = 28;
			this.label4.Text = "Output Tab";
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(80, 80);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(112, 22);
			this.textBox1.TabIndex = 30;
			this.textBox1.Text = "";
			this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
			// 
			// displayTab
			// 
			this.displayTab.Location = new System.Drawing.Point(240, 80);
			this.displayTab.Name = "displayTab";
			this.displayTab.Size = new System.Drawing.Size(160, 24);
			this.displayTab.TabIndex = 31;
			this.displayTab.Text = "Display this tab";
			this.displayTab.CheckedChanged += new System.EventHandler(this.displayTab_CheckedChanged);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(40, 80);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(40, 23);
			this.label5.TabIndex = 32;
			this.label5.Text = "Title:";
			// 
			// TextOutputSettingsPage
			// 
			this.Controls.Add(this.label5);
			this.Controls.Add(this.displayTab);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.suppressLabelsIfNoOutput);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.useDefaultsButton);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.tabSelectComboBox);
			this.Controls.Add(this.testCaseLabels);
			this.Controls.Add(this.showLogOutput);
			this.Controls.Add(this.showTraceOutput);
			this.Controls.Add(this.showErrorOutput);
			this.Controls.Add(this.showStandardOutput);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "TextOutputSettingsPage";
			this.ResumeLayout(false);

		}
		#endregion

		public override void LoadSettings()
		{
			tabSettings.LoadSettings(settings);

			this.tabSelectComboBox.SelectedIndex = 0;
			this.InitDisplay(tabSettings.Tabs[0]);
		}

		public override void ApplySettings()
		{
			tabSettings.ApplySettings();
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			tabSettings.LoadDefaults();
			InitDisplay( tabSettings.Tabs[0] );
		}

		private void InitDisplay(TextDisplayTabSettings.TabInfo tabInfo)
		{
			textBox1.Text = tabInfo.Title;

			TextDisplayContent content = tabInfo.Content;
			showStandardOutput.Checked = (content & TextDisplayContent.Out) != 0;
			showErrorOutput.Checked = (content & TextDisplayContent.Error) != 0;
			showTraceOutput.Checked = (content & TextDisplayContent.Trace) != 0;
			showLogOutput.Checked = (content & TextDisplayContent.Log) != 0;
			testCaseLabels.Checked = (content & TextDisplayContent.Labels) != 0;
			suppressLabelsIfNoOutput.Checked = (content & TextDisplayContent.LabelOnlyOnOutput) != 0;

			displayTab.Checked = tabInfo.Visible;
		}

		private void tabSelectComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			InitDisplay(tabSettings.Tabs[tabSelectComboBox.SelectedIndex]);
		}

		private void testCaseLabels_CheckedChanged(object sender, System.EventArgs e)
		{
			suppressLabelsIfNoOutput.Enabled = testCaseLabels.Checked;
			SetContent( TextDisplayContent.Labels, testCaseLabels.Checked );
		}

		private void suppressLabelsIfNoOutput_CheckedChanged(object sender, System.EventArgs e)
		{
			SetContent( TextDisplayContent.LabelOnlyOnOutput, suppressLabelsIfNoOutput.Checked );
		}

		private void SetContent( TextDisplayContent mask, bool enable )
		{
			int index = tabSelectComboBox.SelectedIndex;
			if ( enable )
				tabSettings.Tabs[index].Content |= mask;
			else
				tabSettings.Tabs[index].Content &= ~mask;
		}

		private void showStandardOutput_CheckedChanged(object sender, System.EventArgs e)
		{
			SetContent( TextDisplayContent.Out, showStandardOutput.Checked );		
		}

		private void showErrorOutput_CheckedChanged(object sender, System.EventArgs e)
		{
			SetContent( TextDisplayContent.Error, showErrorOutput.Checked );		
		}

		private void showTraceOutput_CheckedChanged(object sender, System.EventArgs e)
		{
			SetContent( TextDisplayContent.Trace, showTraceOutput.Checked );		
		}

		private void showLogOutput_CheckedChanged(object sender, System.EventArgs e)
		{
			SetContent( TextDisplayContent.Log, showLogOutput.Checked );		
		}
		
		private void textBox1_TextChanged(object sender, System.EventArgs e)
		{
			tabSettings.Tabs[tabSelectComboBox.SelectedIndex].Title = textBox1.Text;
		}

		private void displayTab_CheckedChanged(object sender, System.EventArgs e)
		{
			tabSettings.Tabs[tabSelectComboBox.SelectedIndex].Visible = displayTab.Checked;
		}
	}
}

