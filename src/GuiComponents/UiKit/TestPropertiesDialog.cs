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

using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;
using System.IO;
using System.Collections;
using NUnit.Core;

namespace NUnit.UiKit
{
	/// <summary>
	/// Summary description for TestPropertiesDialog.
	/// </summary>
	public class TestPropertiesDialog : System.Windows.Forms.Form
	{
		#region Instance Variables;

		private TestSuiteTreeNode node;
		private TestInfo test;
		private TestResult result;
		private Image pinnedImage;
		private Image unpinnedImage;
		private System.Windows.Forms.CheckBox pinButton;
		private System.Windows.Forms.Label testResult;
		private System.Windows.Forms.Label testName;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label categories;
		private System.Windows.Forms.Label label7;
		private CP.Windows.Forms.ExpandingLabel description;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label testCaseCount;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label shouldRun;
		private System.Windows.Forms.Label label2;
		private CP.Windows.Forms.ExpandingLabel fullName;
		private System.Windows.Forms.Label label1;
		private CP.Windows.Forms.ExpandingLabel ignoreReason;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label testType;
		private System.Windows.Forms.Label label3;
		private CP.Windows.Forms.ExpandingLabel stackTrace;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label10;
		private CP.Windows.Forms.ExpandingLabel expandingLabel1;
		private System.Windows.Forms.Label elapsedTime;
		private CP.Windows.Forms.ExpandingLabel message;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label properties;
		private System.Windows.Forms.Label assertCount;
		private System.ComponentModel.IContainer components = null;

		#endregion

		#region Construction and Disposal

		public TestPropertiesDialog( TestSuiteTreeNode node )
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			this.node = node;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(TestPropertiesDialog));
			this.pinButton = new System.Windows.Forms.CheckBox();
			this.testResult = new System.Windows.Forms.Label();
			this.testName = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.properties = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.ignoreReason = new CP.Windows.Forms.ExpandingLabel();
			this.label5 = new System.Windows.Forms.Label();
			this.testType = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.categories = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.description = new CP.Windows.Forms.ExpandingLabel();
			this.label6 = new System.Windows.Forms.Label();
			this.testCaseCount = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.shouldRun = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.fullName = new CP.Windows.Forms.ExpandingLabel();
			this.label1 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.stackTrace = new CP.Windows.Forms.ExpandingLabel();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label10 = new System.Windows.Forms.Label();
			this.expandingLabel1 = new CP.Windows.Forms.ExpandingLabel();
			this.elapsedTime = new System.Windows.Forms.Label();
			this.message = new CP.Windows.Forms.ExpandingLabel();
			this.label12 = new System.Windows.Forms.Label();
			this.assertCount = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// pinButton
			// 
			this.pinButton.AccessibleDescription = resources.GetString("pinButton.AccessibleDescription");
			this.pinButton.AccessibleName = resources.GetString("pinButton.AccessibleName");
			this.pinButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("pinButton.Anchor")));
			this.pinButton.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("pinButton.Appearance")));
			this.pinButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pinButton.BackgroundImage")));
			this.pinButton.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("pinButton.CheckAlign")));
			this.pinButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("pinButton.Dock")));
			this.pinButton.Enabled = ((bool)(resources.GetObject("pinButton.Enabled")));
			this.pinButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("pinButton.FlatStyle")));
			this.pinButton.Font = ((System.Drawing.Font)(resources.GetObject("pinButton.Font")));
			this.pinButton.Image = ((System.Drawing.Image)(resources.GetObject("pinButton.Image")));
			this.pinButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("pinButton.ImageAlign")));
			this.pinButton.ImageIndex = ((int)(resources.GetObject("pinButton.ImageIndex")));
			this.pinButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("pinButton.ImeMode")));
			this.pinButton.Location = ((System.Drawing.Point)(resources.GetObject("pinButton.Location")));
			this.pinButton.Name = "pinButton";
			this.pinButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("pinButton.RightToLeft")));
			this.pinButton.Size = ((System.Drawing.Size)(resources.GetObject("pinButton.Size")));
			this.pinButton.TabIndex = ((int)(resources.GetObject("pinButton.TabIndex")));
			this.pinButton.Text = resources.GetString("pinButton.Text");
			this.pinButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("pinButton.TextAlign")));
			this.pinButton.Visible = ((bool)(resources.GetObject("pinButton.Visible")));
			this.pinButton.Click += new System.EventHandler(this.pinButton_Click);
			this.pinButton.CheckedChanged += new System.EventHandler(this.pinButton_CheckedChanged);
			// 
			// testResult
			// 
			this.testResult.AccessibleDescription = resources.GetString("testResult.AccessibleDescription");
			this.testResult.AccessibleName = resources.GetString("testResult.AccessibleName");
			this.testResult.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("testResult.Anchor")));
			this.testResult.AutoSize = ((bool)(resources.GetObject("testResult.AutoSize")));
			this.testResult.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("testResult.Dock")));
			this.testResult.Enabled = ((bool)(resources.GetObject("testResult.Enabled")));
			this.testResult.Font = ((System.Drawing.Font)(resources.GetObject("testResult.Font")));
			this.testResult.Image = ((System.Drawing.Image)(resources.GetObject("testResult.Image")));
			this.testResult.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("testResult.ImageAlign")));
			this.testResult.ImageIndex = ((int)(resources.GetObject("testResult.ImageIndex")));
			this.testResult.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("testResult.ImeMode")));
			this.testResult.Location = ((System.Drawing.Point)(resources.GetObject("testResult.Location")));
			this.testResult.Name = "testResult";
			this.testResult.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("testResult.RightToLeft")));
			this.testResult.Size = ((System.Drawing.Size)(resources.GetObject("testResult.Size")));
			this.testResult.TabIndex = ((int)(resources.GetObject("testResult.TabIndex")));
			this.testResult.Text = resources.GetString("testResult.Text");
			this.testResult.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("testResult.TextAlign")));
			this.testResult.Visible = ((bool)(resources.GetObject("testResult.Visible")));
			// 
			// testName
			// 
			this.testName.AccessibleDescription = resources.GetString("testName.AccessibleDescription");
			this.testName.AccessibleName = resources.GetString("testName.AccessibleName");
			this.testName.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("testName.Anchor")));
			this.testName.AutoSize = ((bool)(resources.GetObject("testName.AutoSize")));
			this.testName.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("testName.Dock")));
			this.testName.Enabled = ((bool)(resources.GetObject("testName.Enabled")));
			this.testName.Font = ((System.Drawing.Font)(resources.GetObject("testName.Font")));
			this.testName.Image = ((System.Drawing.Image)(resources.GetObject("testName.Image")));
			this.testName.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("testName.ImageAlign")));
			this.testName.ImageIndex = ((int)(resources.GetObject("testName.ImageIndex")));
			this.testName.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("testName.ImeMode")));
			this.testName.Location = ((System.Drawing.Point)(resources.GetObject("testName.Location")));
			this.testName.Name = "testName";
			this.testName.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("testName.RightToLeft")));
			this.testName.Size = ((System.Drawing.Size)(resources.GetObject("testName.Size")));
			this.testName.TabIndex = ((int)(resources.GetObject("testName.TabIndex")));
			this.testName.Text = resources.GetString("testName.Text");
			this.testName.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("testName.TextAlign")));
			this.testName.Visible = ((bool)(resources.GetObject("testName.Visible")));
			// 
			// groupBox1
			// 
			this.groupBox1.AccessibleDescription = resources.GetString("groupBox1.AccessibleDescription");
			this.groupBox1.AccessibleName = resources.GetString("groupBox1.AccessibleName");
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("groupBox1.Anchor")));
			this.groupBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("groupBox1.BackgroundImage")));
			this.groupBox1.Controls.Add(this.properties);
			this.groupBox1.Controls.Add(this.label11);
			this.groupBox1.Controls.Add(this.ignoreReason);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.testType);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Controls.Add(this.categories);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.description);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.testCaseCount);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.shouldRun);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.fullName);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("groupBox1.Dock")));
			this.groupBox1.Enabled = ((bool)(resources.GetObject("groupBox1.Enabled")));
			this.groupBox1.Font = ((System.Drawing.Font)(resources.GetObject("groupBox1.Font")));
			this.groupBox1.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("groupBox1.ImeMode")));
			this.groupBox1.Location = ((System.Drawing.Point)(resources.GetObject("groupBox1.Location")));
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("groupBox1.RightToLeft")));
			this.groupBox1.Size = ((System.Drawing.Size)(resources.GetObject("groupBox1.Size")));
			this.groupBox1.TabIndex = ((int)(resources.GetObject("groupBox1.TabIndex")));
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = resources.GetString("groupBox1.Text");
			this.groupBox1.Visible = ((bool)(resources.GetObject("groupBox1.Visible")));
			// 
			// properties
			// 
			this.properties.AccessibleDescription = resources.GetString("properties.AccessibleDescription");
			this.properties.AccessibleName = resources.GetString("properties.AccessibleName");
			this.properties.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("properties.Anchor")));
			this.properties.AutoSize = ((bool)(resources.GetObject("properties.AutoSize")));
			this.properties.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("properties.Dock")));
			this.properties.Enabled = ((bool)(resources.GetObject("properties.Enabled")));
			this.properties.Font = ((System.Drawing.Font)(resources.GetObject("properties.Font")));
			this.properties.Image = ((System.Drawing.Image)(resources.GetObject("properties.Image")));
			this.properties.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("properties.ImageAlign")));
			this.properties.ImageIndex = ((int)(resources.GetObject("properties.ImageIndex")));
			this.properties.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("properties.ImeMode")));
			this.properties.Location = ((System.Drawing.Point)(resources.GetObject("properties.Location")));
			this.properties.Name = "properties";
			this.properties.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("properties.RightToLeft")));
			this.properties.Size = ((System.Drawing.Size)(resources.GetObject("properties.Size")));
			this.properties.TabIndex = ((int)(resources.GetObject("properties.TabIndex")));
			this.properties.Text = resources.GetString("properties.Text");
			this.properties.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("properties.TextAlign")));
			this.properties.Visible = ((bool)(resources.GetObject("properties.Visible")));
			// 
			// label11
			// 
			this.label11.AccessibleDescription = resources.GetString("label11.AccessibleDescription");
			this.label11.AccessibleName = resources.GetString("label11.AccessibleName");
			this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label11.Anchor")));
			this.label11.AutoSize = ((bool)(resources.GetObject("label11.AutoSize")));
			this.label11.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label11.Dock")));
			this.label11.Enabled = ((bool)(resources.GetObject("label11.Enabled")));
			this.label11.Font = ((System.Drawing.Font)(resources.GetObject("label11.Font")));
			this.label11.Image = ((System.Drawing.Image)(resources.GetObject("label11.Image")));
			this.label11.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label11.ImageAlign")));
			this.label11.ImageIndex = ((int)(resources.GetObject("label11.ImageIndex")));
			this.label11.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label11.ImeMode")));
			this.label11.Location = ((System.Drawing.Point)(resources.GetObject("label11.Location")));
			this.label11.Name = "label11";
			this.label11.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label11.RightToLeft")));
			this.label11.Size = ((System.Drawing.Size)(resources.GetObject("label11.Size")));
			this.label11.TabIndex = ((int)(resources.GetObject("label11.TabIndex")));
			this.label11.Text = resources.GetString("label11.Text");
			this.label11.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label11.TextAlign")));
			this.label11.Visible = ((bool)(resources.GetObject("label11.Visible")));
			// 
			// ignoreReason
			// 
			this.ignoreReason.AccessibleDescription = resources.GetString("ignoreReason.AccessibleDescription");
			this.ignoreReason.AccessibleName = resources.GetString("ignoreReason.AccessibleName");
			this.ignoreReason.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("ignoreReason.Anchor")));
			this.ignoreReason.AutoSize = ((bool)(resources.GetObject("ignoreReason.AutoSize")));
			this.ignoreReason.CopySupported = true;
			this.ignoreReason.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("ignoreReason.Dock")));
			this.ignoreReason.Enabled = ((bool)(resources.GetObject("ignoreReason.Enabled")));
			this.ignoreReason.Expansion = CP.Windows.Forms.TipWindow.ExpansionStyle.Vertical;
			this.ignoreReason.Font = ((System.Drawing.Font)(resources.GetObject("ignoreReason.Font")));
			this.ignoreReason.Image = ((System.Drawing.Image)(resources.GetObject("ignoreReason.Image")));
			this.ignoreReason.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("ignoreReason.ImageAlign")));
			this.ignoreReason.ImageIndex = ((int)(resources.GetObject("ignoreReason.ImageIndex")));
			this.ignoreReason.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("ignoreReason.ImeMode")));
			this.ignoreReason.Location = ((System.Drawing.Point)(resources.GetObject("ignoreReason.Location")));
			this.ignoreReason.Name = "ignoreReason";
			this.ignoreReason.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("ignoreReason.RightToLeft")));
			this.ignoreReason.Size = ((System.Drawing.Size)(resources.GetObject("ignoreReason.Size")));
			this.ignoreReason.TabIndex = ((int)(resources.GetObject("ignoreReason.TabIndex")));
			this.ignoreReason.Text = resources.GetString("ignoreReason.Text");
			this.ignoreReason.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("ignoreReason.TextAlign")));
			this.ignoreReason.Visible = ((bool)(resources.GetObject("ignoreReason.Visible")));
			// 
			// label5
			// 
			this.label5.AccessibleDescription = resources.GetString("label5.AccessibleDescription");
			this.label5.AccessibleName = resources.GetString("label5.AccessibleName");
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label5.Anchor")));
			this.label5.AutoSize = ((bool)(resources.GetObject("label5.AutoSize")));
			this.label5.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label5.Dock")));
			this.label5.Enabled = ((bool)(resources.GetObject("label5.Enabled")));
			this.label5.Font = ((System.Drawing.Font)(resources.GetObject("label5.Font")));
			this.label5.Image = ((System.Drawing.Image)(resources.GetObject("label5.Image")));
			this.label5.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label5.ImageAlign")));
			this.label5.ImageIndex = ((int)(resources.GetObject("label5.ImageIndex")));
			this.label5.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label5.ImeMode")));
			this.label5.Location = ((System.Drawing.Point)(resources.GetObject("label5.Location")));
			this.label5.Name = "label5";
			this.label5.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label5.RightToLeft")));
			this.label5.Size = ((System.Drawing.Size)(resources.GetObject("label5.Size")));
			this.label5.TabIndex = ((int)(resources.GetObject("label5.TabIndex")));
			this.label5.Text = resources.GetString("label5.Text");
			this.label5.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label5.TextAlign")));
			this.label5.Visible = ((bool)(resources.GetObject("label5.Visible")));
			// 
			// testType
			// 
			this.testType.AccessibleDescription = resources.GetString("testType.AccessibleDescription");
			this.testType.AccessibleName = resources.GetString("testType.AccessibleName");
			this.testType.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("testType.Anchor")));
			this.testType.AutoSize = ((bool)(resources.GetObject("testType.AutoSize")));
			this.testType.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("testType.Dock")));
			this.testType.Enabled = ((bool)(resources.GetObject("testType.Enabled")));
			this.testType.Font = ((System.Drawing.Font)(resources.GetObject("testType.Font")));
			this.testType.Image = ((System.Drawing.Image)(resources.GetObject("testType.Image")));
			this.testType.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("testType.ImageAlign")));
			this.testType.ImageIndex = ((int)(resources.GetObject("testType.ImageIndex")));
			this.testType.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("testType.ImeMode")));
			this.testType.Location = ((System.Drawing.Point)(resources.GetObject("testType.Location")));
			this.testType.Name = "testType";
			this.testType.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("testType.RightToLeft")));
			this.testType.Size = ((System.Drawing.Size)(resources.GetObject("testType.Size")));
			this.testType.TabIndex = ((int)(resources.GetObject("testType.TabIndex")));
			this.testType.Text = resources.GetString("testType.Text");
			this.testType.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("testType.TextAlign")));
			this.testType.Visible = ((bool)(resources.GetObject("testType.Visible")));
			// 
			// label8
			// 
			this.label8.AccessibleDescription = resources.GetString("label8.AccessibleDescription");
			this.label8.AccessibleName = resources.GetString("label8.AccessibleName");
			this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label8.Anchor")));
			this.label8.AutoSize = ((bool)(resources.GetObject("label8.AutoSize")));
			this.label8.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label8.Dock")));
			this.label8.Enabled = ((bool)(resources.GetObject("label8.Enabled")));
			this.label8.Font = ((System.Drawing.Font)(resources.GetObject("label8.Font")));
			this.label8.Image = ((System.Drawing.Image)(resources.GetObject("label8.Image")));
			this.label8.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label8.ImageAlign")));
			this.label8.ImageIndex = ((int)(resources.GetObject("label8.ImageIndex")));
			this.label8.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label8.ImeMode")));
			this.label8.Location = ((System.Drawing.Point)(resources.GetObject("label8.Location")));
			this.label8.Name = "label8";
			this.label8.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label8.RightToLeft")));
			this.label8.Size = ((System.Drawing.Size)(resources.GetObject("label8.Size")));
			this.label8.TabIndex = ((int)(resources.GetObject("label8.TabIndex")));
			this.label8.Text = resources.GetString("label8.Text");
			this.label8.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label8.TextAlign")));
			this.label8.Visible = ((bool)(resources.GetObject("label8.Visible")));
			// 
			// categories
			// 
			this.categories.AccessibleDescription = resources.GetString("categories.AccessibleDescription");
			this.categories.AccessibleName = resources.GetString("categories.AccessibleName");
			this.categories.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("categories.Anchor")));
			this.categories.AutoSize = ((bool)(resources.GetObject("categories.AutoSize")));
			this.categories.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("categories.Dock")));
			this.categories.Enabled = ((bool)(resources.GetObject("categories.Enabled")));
			this.categories.Font = ((System.Drawing.Font)(resources.GetObject("categories.Font")));
			this.categories.Image = ((System.Drawing.Image)(resources.GetObject("categories.Image")));
			this.categories.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("categories.ImageAlign")));
			this.categories.ImageIndex = ((int)(resources.GetObject("categories.ImageIndex")));
			this.categories.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("categories.ImeMode")));
			this.categories.Location = ((System.Drawing.Point)(resources.GetObject("categories.Location")));
			this.categories.Name = "categories";
			this.categories.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("categories.RightToLeft")));
			this.categories.Size = ((System.Drawing.Size)(resources.GetObject("categories.Size")));
			this.categories.TabIndex = ((int)(resources.GetObject("categories.TabIndex")));
			this.categories.Text = resources.GetString("categories.Text");
			this.categories.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("categories.TextAlign")));
			this.categories.Visible = ((bool)(resources.GetObject("categories.Visible")));
			// 
			// label7
			// 
			this.label7.AccessibleDescription = resources.GetString("label7.AccessibleDescription");
			this.label7.AccessibleName = resources.GetString("label7.AccessibleName");
			this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label7.Anchor")));
			this.label7.AutoSize = ((bool)(resources.GetObject("label7.AutoSize")));
			this.label7.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label7.Dock")));
			this.label7.Enabled = ((bool)(resources.GetObject("label7.Enabled")));
			this.label7.Font = ((System.Drawing.Font)(resources.GetObject("label7.Font")));
			this.label7.Image = ((System.Drawing.Image)(resources.GetObject("label7.Image")));
			this.label7.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label7.ImageAlign")));
			this.label7.ImageIndex = ((int)(resources.GetObject("label7.ImageIndex")));
			this.label7.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label7.ImeMode")));
			this.label7.Location = ((System.Drawing.Point)(resources.GetObject("label7.Location")));
			this.label7.Name = "label7";
			this.label7.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label7.RightToLeft")));
			this.label7.Size = ((System.Drawing.Size)(resources.GetObject("label7.Size")));
			this.label7.TabIndex = ((int)(resources.GetObject("label7.TabIndex")));
			this.label7.Text = resources.GetString("label7.Text");
			this.label7.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label7.TextAlign")));
			this.label7.Visible = ((bool)(resources.GetObject("label7.Visible")));
			// 
			// description
			// 
			this.description.AccessibleDescription = resources.GetString("description.AccessibleDescription");
			this.description.AccessibleName = resources.GetString("description.AccessibleName");
			this.description.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("description.Anchor")));
			this.description.AutoSize = ((bool)(resources.GetObject("description.AutoSize")));
			this.description.CopySupported = true;
			this.description.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("description.Dock")));
			this.description.Enabled = ((bool)(resources.GetObject("description.Enabled")));
			this.description.Expansion = CP.Windows.Forms.TipWindow.ExpansionStyle.Both;
			this.description.Font = ((System.Drawing.Font)(resources.GetObject("description.Font")));
			this.description.Image = ((System.Drawing.Image)(resources.GetObject("description.Image")));
			this.description.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("description.ImageAlign")));
			this.description.ImageIndex = ((int)(resources.GetObject("description.ImageIndex")));
			this.description.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("description.ImeMode")));
			this.description.Location = ((System.Drawing.Point)(resources.GetObject("description.Location")));
			this.description.Name = "description";
			this.description.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("description.RightToLeft")));
			this.description.Size = ((System.Drawing.Size)(resources.GetObject("description.Size")));
			this.description.TabIndex = ((int)(resources.GetObject("description.TabIndex")));
			this.description.Text = resources.GetString("description.Text");
			this.description.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("description.TextAlign")));
			this.description.Visible = ((bool)(resources.GetObject("description.Visible")));
			// 
			// label6
			// 
			this.label6.AccessibleDescription = resources.GetString("label6.AccessibleDescription");
			this.label6.AccessibleName = resources.GetString("label6.AccessibleName");
			this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label6.Anchor")));
			this.label6.AutoSize = ((bool)(resources.GetObject("label6.AutoSize")));
			this.label6.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label6.Dock")));
			this.label6.Enabled = ((bool)(resources.GetObject("label6.Enabled")));
			this.label6.Font = ((System.Drawing.Font)(resources.GetObject("label6.Font")));
			this.label6.Image = ((System.Drawing.Image)(resources.GetObject("label6.Image")));
			this.label6.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label6.ImageAlign")));
			this.label6.ImageIndex = ((int)(resources.GetObject("label6.ImageIndex")));
			this.label6.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label6.ImeMode")));
			this.label6.Location = ((System.Drawing.Point)(resources.GetObject("label6.Location")));
			this.label6.Name = "label6";
			this.label6.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label6.RightToLeft")));
			this.label6.Size = ((System.Drawing.Size)(resources.GetObject("label6.Size")));
			this.label6.TabIndex = ((int)(resources.GetObject("label6.TabIndex")));
			this.label6.Text = resources.GetString("label6.Text");
			this.label6.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label6.TextAlign")));
			this.label6.Visible = ((bool)(resources.GetObject("label6.Visible")));
			// 
			// testCaseCount
			// 
			this.testCaseCount.AccessibleDescription = resources.GetString("testCaseCount.AccessibleDescription");
			this.testCaseCount.AccessibleName = resources.GetString("testCaseCount.AccessibleName");
			this.testCaseCount.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("testCaseCount.Anchor")));
			this.testCaseCount.AutoSize = ((bool)(resources.GetObject("testCaseCount.AutoSize")));
			this.testCaseCount.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("testCaseCount.Dock")));
			this.testCaseCount.Enabled = ((bool)(resources.GetObject("testCaseCount.Enabled")));
			this.testCaseCount.Font = ((System.Drawing.Font)(resources.GetObject("testCaseCount.Font")));
			this.testCaseCount.Image = ((System.Drawing.Image)(resources.GetObject("testCaseCount.Image")));
			this.testCaseCount.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("testCaseCount.ImageAlign")));
			this.testCaseCount.ImageIndex = ((int)(resources.GetObject("testCaseCount.ImageIndex")));
			this.testCaseCount.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("testCaseCount.ImeMode")));
			this.testCaseCount.Location = ((System.Drawing.Point)(resources.GetObject("testCaseCount.Location")));
			this.testCaseCount.Name = "testCaseCount";
			this.testCaseCount.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("testCaseCount.RightToLeft")));
			this.testCaseCount.Size = ((System.Drawing.Size)(resources.GetObject("testCaseCount.Size")));
			this.testCaseCount.TabIndex = ((int)(resources.GetObject("testCaseCount.TabIndex")));
			this.testCaseCount.Text = resources.GetString("testCaseCount.Text");
			this.testCaseCount.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("testCaseCount.TextAlign")));
			this.testCaseCount.Visible = ((bool)(resources.GetObject("testCaseCount.Visible")));
			// 
			// label4
			// 
			this.label4.AccessibleDescription = resources.GetString("label4.AccessibleDescription");
			this.label4.AccessibleName = resources.GetString("label4.AccessibleName");
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label4.Anchor")));
			this.label4.AutoSize = ((bool)(resources.GetObject("label4.AutoSize")));
			this.label4.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label4.Dock")));
			this.label4.Enabled = ((bool)(resources.GetObject("label4.Enabled")));
			this.label4.Font = ((System.Drawing.Font)(resources.GetObject("label4.Font")));
			this.label4.Image = ((System.Drawing.Image)(resources.GetObject("label4.Image")));
			this.label4.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label4.ImageAlign")));
			this.label4.ImageIndex = ((int)(resources.GetObject("label4.ImageIndex")));
			this.label4.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label4.ImeMode")));
			this.label4.Location = ((System.Drawing.Point)(resources.GetObject("label4.Location")));
			this.label4.Name = "label4";
			this.label4.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label4.RightToLeft")));
			this.label4.Size = ((System.Drawing.Size)(resources.GetObject("label4.Size")));
			this.label4.TabIndex = ((int)(resources.GetObject("label4.TabIndex")));
			this.label4.Text = resources.GetString("label4.Text");
			this.label4.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label4.TextAlign")));
			this.label4.Visible = ((bool)(resources.GetObject("label4.Visible")));
			// 
			// shouldRun
			// 
			this.shouldRun.AccessibleDescription = resources.GetString("shouldRun.AccessibleDescription");
			this.shouldRun.AccessibleName = resources.GetString("shouldRun.AccessibleName");
			this.shouldRun.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("shouldRun.Anchor")));
			this.shouldRun.AutoSize = ((bool)(resources.GetObject("shouldRun.AutoSize")));
			this.shouldRun.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("shouldRun.Dock")));
			this.shouldRun.Enabled = ((bool)(resources.GetObject("shouldRun.Enabled")));
			this.shouldRun.Font = ((System.Drawing.Font)(resources.GetObject("shouldRun.Font")));
			this.shouldRun.Image = ((System.Drawing.Image)(resources.GetObject("shouldRun.Image")));
			this.shouldRun.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("shouldRun.ImageAlign")));
			this.shouldRun.ImageIndex = ((int)(resources.GetObject("shouldRun.ImageIndex")));
			this.shouldRun.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("shouldRun.ImeMode")));
			this.shouldRun.Location = ((System.Drawing.Point)(resources.GetObject("shouldRun.Location")));
			this.shouldRun.Name = "shouldRun";
			this.shouldRun.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("shouldRun.RightToLeft")));
			this.shouldRun.Size = ((System.Drawing.Size)(resources.GetObject("shouldRun.Size")));
			this.shouldRun.TabIndex = ((int)(resources.GetObject("shouldRun.TabIndex")));
			this.shouldRun.Text = resources.GetString("shouldRun.Text");
			this.shouldRun.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("shouldRun.TextAlign")));
			this.shouldRun.Visible = ((bool)(resources.GetObject("shouldRun.Visible")));
			// 
			// label2
			// 
			this.label2.AccessibleDescription = resources.GetString("label2.AccessibleDescription");
			this.label2.AccessibleName = resources.GetString("label2.AccessibleName");
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label2.Anchor")));
			this.label2.AutoSize = ((bool)(resources.GetObject("label2.AutoSize")));
			this.label2.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label2.Dock")));
			this.label2.Enabled = ((bool)(resources.GetObject("label2.Enabled")));
			this.label2.Font = ((System.Drawing.Font)(resources.GetObject("label2.Font")));
			this.label2.Image = ((System.Drawing.Image)(resources.GetObject("label2.Image")));
			this.label2.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label2.ImageAlign")));
			this.label2.ImageIndex = ((int)(resources.GetObject("label2.ImageIndex")));
			this.label2.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label2.ImeMode")));
			this.label2.Location = ((System.Drawing.Point)(resources.GetObject("label2.Location")));
			this.label2.Name = "label2";
			this.label2.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label2.RightToLeft")));
			this.label2.Size = ((System.Drawing.Size)(resources.GetObject("label2.Size")));
			this.label2.TabIndex = ((int)(resources.GetObject("label2.TabIndex")));
			this.label2.Text = resources.GetString("label2.Text");
			this.label2.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label2.TextAlign")));
			this.label2.Visible = ((bool)(resources.GetObject("label2.Visible")));
			// 
			// fullName
			// 
			this.fullName.AccessibleDescription = resources.GetString("fullName.AccessibleDescription");
			this.fullName.AccessibleName = resources.GetString("fullName.AccessibleName");
			this.fullName.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("fullName.Anchor")));
			this.fullName.AutoSize = ((bool)(resources.GetObject("fullName.AutoSize")));
			this.fullName.CopySupported = true;
			this.fullName.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("fullName.Dock")));
			this.fullName.Enabled = ((bool)(resources.GetObject("fullName.Enabled")));
			this.fullName.Font = ((System.Drawing.Font)(resources.GetObject("fullName.Font")));
			this.fullName.Image = ((System.Drawing.Image)(resources.GetObject("fullName.Image")));
			this.fullName.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("fullName.ImageAlign")));
			this.fullName.ImageIndex = ((int)(resources.GetObject("fullName.ImageIndex")));
			this.fullName.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("fullName.ImeMode")));
			this.fullName.Location = ((System.Drawing.Point)(resources.GetObject("fullName.Location")));
			this.fullName.Name = "fullName";
			this.fullName.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("fullName.RightToLeft")));
			this.fullName.Size = ((System.Drawing.Size)(resources.GetObject("fullName.Size")));
			this.fullName.TabIndex = ((int)(resources.GetObject("fullName.TabIndex")));
			this.fullName.Text = resources.GetString("fullName.Text");
			this.fullName.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("fullName.TextAlign")));
			this.fullName.Visible = ((bool)(resources.GetObject("fullName.Visible")));
			// 
			// label1
			// 
			this.label1.AccessibleDescription = resources.GetString("label1.AccessibleDescription");
			this.label1.AccessibleName = resources.GetString("label1.AccessibleName");
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label1.Anchor")));
			this.label1.AutoSize = ((bool)(resources.GetObject("label1.AutoSize")));
			this.label1.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label1.Dock")));
			this.label1.Enabled = ((bool)(resources.GetObject("label1.Enabled")));
			this.label1.Font = ((System.Drawing.Font)(resources.GetObject("label1.Font")));
			this.label1.Image = ((System.Drawing.Image)(resources.GetObject("label1.Image")));
			this.label1.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label1.ImageAlign")));
			this.label1.ImageIndex = ((int)(resources.GetObject("label1.ImageIndex")));
			this.label1.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label1.ImeMode")));
			this.label1.Location = ((System.Drawing.Point)(resources.GetObject("label1.Location")));
			this.label1.Name = "label1";
			this.label1.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label1.RightToLeft")));
			this.label1.Size = ((System.Drawing.Size)(resources.GetObject("label1.Size")));
			this.label1.TabIndex = ((int)(resources.GetObject("label1.TabIndex")));
			this.label1.Text = resources.GetString("label1.Text");
			this.label1.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label1.TextAlign")));
			this.label1.Visible = ((bool)(resources.GetObject("label1.Visible")));
			// 
			// label3
			// 
			this.label3.AccessibleDescription = resources.GetString("label3.AccessibleDescription");
			this.label3.AccessibleName = resources.GetString("label3.AccessibleName");
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label3.Anchor")));
			this.label3.AutoSize = ((bool)(resources.GetObject("label3.AutoSize")));
			this.label3.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label3.Dock")));
			this.label3.Enabled = ((bool)(resources.GetObject("label3.Enabled")));
			this.label3.Font = ((System.Drawing.Font)(resources.GetObject("label3.Font")));
			this.label3.Image = ((System.Drawing.Image)(resources.GetObject("label3.Image")));
			this.label3.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label3.ImageAlign")));
			this.label3.ImageIndex = ((int)(resources.GetObject("label3.ImageIndex")));
			this.label3.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label3.ImeMode")));
			this.label3.Location = ((System.Drawing.Point)(resources.GetObject("label3.Location")));
			this.label3.Name = "label3";
			this.label3.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label3.RightToLeft")));
			this.label3.Size = ((System.Drawing.Size)(resources.GetObject("label3.Size")));
			this.label3.TabIndex = ((int)(resources.GetObject("label3.TabIndex")));
			this.label3.Text = resources.GetString("label3.Text");
			this.label3.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label3.TextAlign")));
			this.label3.Visible = ((bool)(resources.GetObject("label3.Visible")));
			// 
			// stackTrace
			// 
			this.stackTrace.AccessibleDescription = resources.GetString("stackTrace.AccessibleDescription");
			this.stackTrace.AccessibleName = resources.GetString("stackTrace.AccessibleName");
			this.stackTrace.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("stackTrace.Anchor")));
			this.stackTrace.AutoSize = ((bool)(resources.GetObject("stackTrace.AutoSize")));
			this.stackTrace.CopySupported = true;
			this.stackTrace.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("stackTrace.Dock")));
			this.stackTrace.Enabled = ((bool)(resources.GetObject("stackTrace.Enabled")));
			this.stackTrace.Expansion = CP.Windows.Forms.TipWindow.ExpansionStyle.Both;
			this.stackTrace.Font = ((System.Drawing.Font)(resources.GetObject("stackTrace.Font")));
			this.stackTrace.Image = ((System.Drawing.Image)(resources.GetObject("stackTrace.Image")));
			this.stackTrace.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("stackTrace.ImageAlign")));
			this.stackTrace.ImageIndex = ((int)(resources.GetObject("stackTrace.ImageIndex")));
			this.stackTrace.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("stackTrace.ImeMode")));
			this.stackTrace.Location = ((System.Drawing.Point)(resources.GetObject("stackTrace.Location")));
			this.stackTrace.Name = "stackTrace";
			this.stackTrace.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("stackTrace.RightToLeft")));
			this.stackTrace.Size = ((System.Drawing.Size)(resources.GetObject("stackTrace.Size")));
			this.stackTrace.TabIndex = ((int)(resources.GetObject("stackTrace.TabIndex")));
			this.stackTrace.Text = resources.GetString("stackTrace.Text");
			this.stackTrace.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("stackTrace.TextAlign")));
			this.stackTrace.Visible = ((bool)(resources.GetObject("stackTrace.Visible")));
			// 
			// groupBox2
			// 
			this.groupBox2.AccessibleDescription = resources.GetString("groupBox2.AccessibleDescription");
			this.groupBox2.AccessibleName = resources.GetString("groupBox2.AccessibleName");
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("groupBox2.Anchor")));
			this.groupBox2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("groupBox2.BackgroundImage")));
			this.groupBox2.Controls.Add(this.assertCount);
			this.groupBox2.Controls.Add(this.label10);
			this.groupBox2.Controls.Add(this.expandingLabel1);
			this.groupBox2.Controls.Add(this.elapsedTime);
			this.groupBox2.Controls.Add(this.message);
			this.groupBox2.Controls.Add(this.label12);
			this.groupBox2.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("groupBox2.Dock")));
			this.groupBox2.Enabled = ((bool)(resources.GetObject("groupBox2.Enabled")));
			this.groupBox2.Font = ((System.Drawing.Font)(resources.GetObject("groupBox2.Font")));
			this.groupBox2.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("groupBox2.ImeMode")));
			this.groupBox2.Location = ((System.Drawing.Point)(resources.GetObject("groupBox2.Location")));
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("groupBox2.RightToLeft")));
			this.groupBox2.Size = ((System.Drawing.Size)(resources.GetObject("groupBox2.Size")));
			this.groupBox2.TabIndex = ((int)(resources.GetObject("groupBox2.TabIndex")));
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = resources.GetString("groupBox2.Text");
			this.groupBox2.Visible = ((bool)(resources.GetObject("groupBox2.Visible")));
			// 
			// label10
			// 
			this.label10.AccessibleDescription = resources.GetString("label10.AccessibleDescription");
			this.label10.AccessibleName = resources.GetString("label10.AccessibleName");
			this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label10.Anchor")));
			this.label10.AutoSize = ((bool)(resources.GetObject("label10.AutoSize")));
			this.label10.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label10.Dock")));
			this.label10.Enabled = ((bool)(resources.GetObject("label10.Enabled")));
			this.label10.Font = ((System.Drawing.Font)(resources.GetObject("label10.Font")));
			this.label10.Image = ((System.Drawing.Image)(resources.GetObject("label10.Image")));
			this.label10.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label10.ImageAlign")));
			this.label10.ImageIndex = ((int)(resources.GetObject("label10.ImageIndex")));
			this.label10.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label10.ImeMode")));
			this.label10.Location = ((System.Drawing.Point)(resources.GetObject("label10.Location")));
			this.label10.Name = "label10";
			this.label10.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label10.RightToLeft")));
			this.label10.Size = ((System.Drawing.Size)(resources.GetObject("label10.Size")));
			this.label10.TabIndex = ((int)(resources.GetObject("label10.TabIndex")));
			this.label10.Text = resources.GetString("label10.Text");
			this.label10.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label10.TextAlign")));
			this.label10.Visible = ((bool)(resources.GetObject("label10.Visible")));
			// 
			// expandingLabel1
			// 
			this.expandingLabel1.AccessibleDescription = resources.GetString("expandingLabel1.AccessibleDescription");
			this.expandingLabel1.AccessibleName = resources.GetString("expandingLabel1.AccessibleName");
			this.expandingLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("expandingLabel1.Anchor")));
			this.expandingLabel1.AutoSize = ((bool)(resources.GetObject("expandingLabel1.AutoSize")));
			this.expandingLabel1.CopySupported = true;
			this.expandingLabel1.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("expandingLabel1.Dock")));
			this.expandingLabel1.Enabled = ((bool)(resources.GetObject("expandingLabel1.Enabled")));
			this.expandingLabel1.Expansion = CP.Windows.Forms.TipWindow.ExpansionStyle.Both;
			this.expandingLabel1.Font = ((System.Drawing.Font)(resources.GetObject("expandingLabel1.Font")));
			this.expandingLabel1.Image = ((System.Drawing.Image)(resources.GetObject("expandingLabel1.Image")));
			this.expandingLabel1.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("expandingLabel1.ImageAlign")));
			this.expandingLabel1.ImageIndex = ((int)(resources.GetObject("expandingLabel1.ImageIndex")));
			this.expandingLabel1.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("expandingLabel1.ImeMode")));
			this.expandingLabel1.Location = ((System.Drawing.Point)(resources.GetObject("expandingLabel1.Location")));
			this.expandingLabel1.Name = "expandingLabel1";
			this.expandingLabel1.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("expandingLabel1.RightToLeft")));
			this.expandingLabel1.Size = ((System.Drawing.Size)(resources.GetObject("expandingLabel1.Size")));
			this.expandingLabel1.TabIndex = ((int)(resources.GetObject("expandingLabel1.TabIndex")));
			this.expandingLabel1.Text = resources.GetString("expandingLabel1.Text");
			this.expandingLabel1.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("expandingLabel1.TextAlign")));
			this.expandingLabel1.Visible = ((bool)(resources.GetObject("expandingLabel1.Visible")));
			// 
			// elapsedTime
			// 
			this.elapsedTime.AccessibleDescription = resources.GetString("elapsedTime.AccessibleDescription");
			this.elapsedTime.AccessibleName = resources.GetString("elapsedTime.AccessibleName");
			this.elapsedTime.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("elapsedTime.Anchor")));
			this.elapsedTime.AutoSize = ((bool)(resources.GetObject("elapsedTime.AutoSize")));
			this.elapsedTime.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("elapsedTime.Dock")));
			this.elapsedTime.Enabled = ((bool)(resources.GetObject("elapsedTime.Enabled")));
			this.elapsedTime.Font = ((System.Drawing.Font)(resources.GetObject("elapsedTime.Font")));
			this.elapsedTime.Image = ((System.Drawing.Image)(resources.GetObject("elapsedTime.Image")));
			this.elapsedTime.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("elapsedTime.ImageAlign")));
			this.elapsedTime.ImageIndex = ((int)(resources.GetObject("elapsedTime.ImageIndex")));
			this.elapsedTime.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("elapsedTime.ImeMode")));
			this.elapsedTime.Location = ((System.Drawing.Point)(resources.GetObject("elapsedTime.Location")));
			this.elapsedTime.Name = "elapsedTime";
			this.elapsedTime.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("elapsedTime.RightToLeft")));
			this.elapsedTime.Size = ((System.Drawing.Size)(resources.GetObject("elapsedTime.Size")));
			this.elapsedTime.TabIndex = ((int)(resources.GetObject("elapsedTime.TabIndex")));
			this.elapsedTime.Text = resources.GetString("elapsedTime.Text");
			this.elapsedTime.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("elapsedTime.TextAlign")));
			this.elapsedTime.Visible = ((bool)(resources.GetObject("elapsedTime.Visible")));
			// 
			// message
			// 
			this.message.AccessibleDescription = resources.GetString("message.AccessibleDescription");
			this.message.AccessibleName = resources.GetString("message.AccessibleName");
			this.message.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("message.Anchor")));
			this.message.AutoSize = ((bool)(resources.GetObject("message.AutoSize")));
			this.message.CopySupported = true;
			this.message.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("message.Dock")));
			this.message.Enabled = ((bool)(resources.GetObject("message.Enabled")));
			this.message.Expansion = CP.Windows.Forms.TipWindow.ExpansionStyle.Both;
			this.message.Font = ((System.Drawing.Font)(resources.GetObject("message.Font")));
			this.message.Image = ((System.Drawing.Image)(resources.GetObject("message.Image")));
			this.message.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("message.ImageAlign")));
			this.message.ImageIndex = ((int)(resources.GetObject("message.ImageIndex")));
			this.message.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("message.ImeMode")));
			this.message.Location = ((System.Drawing.Point)(resources.GetObject("message.Location")));
			this.message.Name = "message";
			this.message.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("message.RightToLeft")));
			this.message.Size = ((System.Drawing.Size)(resources.GetObject("message.Size")));
			this.message.TabIndex = ((int)(resources.GetObject("message.TabIndex")));
			this.message.Text = resources.GetString("message.Text");
			this.message.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("message.TextAlign")));
			this.message.Visible = ((bool)(resources.GetObject("message.Visible")));
			// 
			// label12
			// 
			this.label12.AccessibleDescription = resources.GetString("label12.AccessibleDescription");
			this.label12.AccessibleName = resources.GetString("label12.AccessibleName");
			this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label12.Anchor")));
			this.label12.AutoSize = ((bool)(resources.GetObject("label12.AutoSize")));
			this.label12.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label12.Dock")));
			this.label12.Enabled = ((bool)(resources.GetObject("label12.Enabled")));
			this.label12.Font = ((System.Drawing.Font)(resources.GetObject("label12.Font")));
			this.label12.Image = ((System.Drawing.Image)(resources.GetObject("label12.Image")));
			this.label12.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label12.ImageAlign")));
			this.label12.ImageIndex = ((int)(resources.GetObject("label12.ImageIndex")));
			this.label12.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label12.ImeMode")));
			this.label12.Location = ((System.Drawing.Point)(resources.GetObject("label12.Location")));
			this.label12.Name = "label12";
			this.label12.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label12.RightToLeft")));
			this.label12.Size = ((System.Drawing.Size)(resources.GetObject("label12.Size")));
			this.label12.TabIndex = ((int)(resources.GetObject("label12.TabIndex")));
			this.label12.Text = resources.GetString("label12.Text");
			this.label12.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label12.TextAlign")));
			this.label12.Visible = ((bool)(resources.GetObject("label12.Visible")));
			// 
			// assertCount
			// 
			this.assertCount.AccessibleDescription = resources.GetString("assertCount.AccessibleDescription");
			this.assertCount.AccessibleName = resources.GetString("assertCount.AccessibleName");
			this.assertCount.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("assertCount.Anchor")));
			this.assertCount.AutoSize = ((bool)(resources.GetObject("assertCount.AutoSize")));
			this.assertCount.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("assertCount.Dock")));
			this.assertCount.Enabled = ((bool)(resources.GetObject("assertCount.Enabled")));
			this.assertCount.Font = ((System.Drawing.Font)(resources.GetObject("assertCount.Font")));
			this.assertCount.Image = ((System.Drawing.Image)(resources.GetObject("assertCount.Image")));
			this.assertCount.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("assertCount.ImageAlign")));
			this.assertCount.ImageIndex = ((int)(resources.GetObject("assertCount.ImageIndex")));
			this.assertCount.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("assertCount.ImeMode")));
			this.assertCount.Location = ((System.Drawing.Point)(resources.GetObject("assertCount.Location")));
			this.assertCount.Name = "assertCount";
			this.assertCount.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("assertCount.RightToLeft")));
			this.assertCount.Size = ((System.Drawing.Size)(resources.GetObject("assertCount.Size")));
			this.assertCount.TabIndex = ((int)(resources.GetObject("assertCount.TabIndex")));
			this.assertCount.Text = resources.GetString("assertCount.Text");
			this.assertCount.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("assertCount.TextAlign")));
			this.assertCount.Visible = ((bool)(resources.GetObject("assertCount.Visible")));
			// 
			// TestPropertiesDialog
			// 
			this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
			this.AccessibleName = resources.GetString("$this.AccessibleName");
			this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
			this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
			this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
			this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.testName);
			this.Controls.Add(this.testResult);
			this.Controls.Add(this.pinButton);
			this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
			this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
			this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
			this.MaximizeBox = false;
			this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
			this.MinimizeBox = false;
			this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
			this.Name = "TestPropertiesDialog";
			this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
			this.ShowInTaskbar = false;
			this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
			this.Text = resources.GetString("$this.Text");
			this.Load += new System.EventHandler(this.TestPropertiesDialog_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		#region Properties

		[Browsable( false )]
		public bool Pinned
		{
			get { return pinButton.Checked; }
			set { pinButton.Checked = value; }
		}

		#endregion

		#region Methods

		private void SetTitleBarText()
		{
			string name = test.TestName.Name;
			int index = name.LastIndexOfAny( new char[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar } );
			if ( index >= 0 )
				name = name.Substring( index + 1 );
			this.Text = string.Format( "{0} Properties - {1}", node.TestType, name );
		}

		/// <summary>
		/// Set up all dialog fields when it loads
		/// </summary>
		private void TestPropertiesDialog_Load(object sender, System.EventArgs e)
		{
			pinnedImage = new Bitmap( typeof( TestPropertiesDialog ), "pinned.gif" );
			unpinnedImage = new Bitmap( typeof( TestPropertiesDialog ), "unpinned.gif" );
			pinButton.Image = unpinnedImage;

			DisplayProperties();

			node.TreeView.AfterSelect += new TreeViewEventHandler( OnSelectedNodeChanged );	
		}

		private void OnSelectedNodeChanged( object sender, TreeViewEventArgs e )
		{
			if ( pinButton.Checked )
			{
				DisplayProperties( (TestSuiteTreeNode)e.Node );
			}
			else
				this.Close();
		}

		public void DisplayProperties( )
		{
			DisplayProperties( this.node );
		}

		public void DisplayProperties( TestSuiteTreeNode node)
		{
			this.node = node;
			this.test = node.Test;
			this.result = node.Result;

			SetTitleBarText();

			string catText = "";
			foreach( string cat in test.Categories )
			{
				if ( catText != "" )
					catText += ',';
				catText += cat;
			}

			testResult.Text = node.StatusText;
			testName.Text = test.TestName.Name;

			testType.Text = node.TestType;
			fullName.Text = test.TestName.FullName;
			switch( test.RunState )
			{
				case RunState.Explicit:
					shouldRun.Text = "Explicit";
					break;
				case RunState.Runnable:
					shouldRun.Text = "Yes";
					break;
				default:
					shouldRun.Text = "No";
					break;
			}
			description.Text = test.Description;
			categories.Text = catText;
			ignoreReason.Text = test.IgnoreReason;
			testCaseCount.Text = test.TestCount.ToString();
			if ( test.Properties.Count == 0 )
				properties.Text = "";
			else
			{
				StringBuilder sb = new StringBuilder();
				foreach( DictionaryEntry entry in test.Properties )
					sb.AppendFormat( "{0}={1} ", entry.Key, entry.Value.ToString() );
				properties.Text = sb.ToString();
			}

			message.Text = "";
			elapsedTime.Text = "Execution Time:";
			assertCount.Text = "Assert Count:";
			stackTrace.Text = "";

			if ( result != null )
			{
				// message may have a leading blank line
				// TODO: take care of this in label?
				message.Text = TrimLeadingBlankLines( result.Message );
				elapsedTime.Text = string.Format( "Execution Time: {0}", result.Time );
				assertCount.Text = string.Format( "Assert Count: {0}", result.AssertCount );
				stackTrace.Text = result.StackTrace;
			}
		}

		private string TrimLeadingBlankLines( string s )
		{
			if ( s == null ) return s;

			int start = 0;
			for( int i = 0; i < s.Length; i++ )
			{
				switch( s[i] )
				{
					case ' ':
					case '\t':
						break;
					case '\r':
					case '\n':
						start = i + 1;
						break;

					default:
						goto getout;
				}
			}

			getout:
			return start == 0 ? s : s.Substring( start );
		}

		protected override bool ProcessKeyPreview(ref System.Windows.Forms.Message m)
		{
			const int ESCAPE = 27;
			const int WM_CHAR = 258;

			if (m.Msg == WM_CHAR && m.WParam.ToInt32() == ESCAPE )
			{
				this.Close();
				return true;
			}

			return base.ProcessKeyEventArgs( ref m ); 
		}

		private void pinButton_Click(object sender, System.EventArgs e)
		{
			if ( pinButton.Checked )
				pinButton.Image = pinnedImage;
			else
				pinButton.Image = unpinnedImage;
		}

		private void pinButton_CheckedChanged(object sender, System.EventArgs e)
		{
		
		}
	}

	#endregion
}
