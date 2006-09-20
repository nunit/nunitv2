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

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Reflection;

namespace NUnit.Gui
{
	public class AboutBox : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button OkButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label versionLabel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.LinkLabel linkLabel1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label copyright;
		private System.Windows.Forms.Label dotNetVersionLabel;
		private System.Windows.Forms.Label clrTypeLabel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public AboutBox()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			Version version = executingAssembly.GetName().Version;

			object[] objectAttrs = executingAssembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);

			objectAttrs = executingAssembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
			AssemblyCopyrightAttribute copyrightAttr = (AssemblyCopyrightAttribute)objectAttrs[0];
			versionLabel.Text = version.ToString(3);
			clrTypeLabel.Text = Type.GetType( "Mono.Runtime", false ) != null 
				? "Mono Version:" : ".NET Version:";
			dotNetVersionLabel.Text = Environment.Version.ToString();

			copyright.Text = copyrightAttr.Copyright;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(AboutBox));
			this.OkButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.versionLabel = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.copyright = new System.Windows.Forms.Label();
			this.clrTypeLabel = new System.Windows.Forms.Label();
			this.dotNetVersionLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// OkButton
			// 
			this.OkButton.AccessibleDescription = resources.GetString("OkButton.AccessibleDescription");
			this.OkButton.AccessibleName = resources.GetString("OkButton.AccessibleName");
			this.OkButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("OkButton.Anchor")));
			this.OkButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("OkButton.BackgroundImage")));
			this.OkButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.OkButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("OkButton.Dock")));
			this.OkButton.Enabled = ((bool)(resources.GetObject("OkButton.Enabled")));
			this.OkButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("OkButton.FlatStyle")));
			this.OkButton.Font = ((System.Drawing.Font)(resources.GetObject("OkButton.Font")));
			this.OkButton.Image = ((System.Drawing.Image)(resources.GetObject("OkButton.Image")));
			this.OkButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("OkButton.ImageAlign")));
			this.OkButton.ImageIndex = ((int)(resources.GetObject("OkButton.ImageIndex")));
			this.OkButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("OkButton.ImeMode")));
			this.OkButton.Location = ((System.Drawing.Point)(resources.GetObject("OkButton.Location")));
			this.OkButton.Name = "OkButton";
			this.OkButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("OkButton.RightToLeft")));
			this.OkButton.Size = ((System.Drawing.Size)(resources.GetObject("OkButton.Size")));
			this.OkButton.TabIndex = ((int)(resources.GetObject("OkButton.TabIndex")));
			this.OkButton.Text = resources.GetString("OkButton.Text");
			this.OkButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("OkButton.TextAlign")));
			this.OkButton.Visible = ((bool)(resources.GetObject("OkButton.Visible")));
			this.OkButton.Click += new System.EventHandler(this.button1_Click);
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
			// versionLabel
			// 
			this.versionLabel.AccessibleDescription = resources.GetString("versionLabel.AccessibleDescription");
			this.versionLabel.AccessibleName = resources.GetString("versionLabel.AccessibleName");
			this.versionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("versionLabel.Anchor")));
			this.versionLabel.AutoSize = ((bool)(resources.GetObject("versionLabel.AutoSize")));
			this.versionLabel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("versionLabel.Dock")));
			this.versionLabel.Enabled = ((bool)(resources.GetObject("versionLabel.Enabled")));
			this.versionLabel.Font = ((System.Drawing.Font)(resources.GetObject("versionLabel.Font")));
			this.versionLabel.Image = ((System.Drawing.Image)(resources.GetObject("versionLabel.Image")));
			this.versionLabel.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("versionLabel.ImageAlign")));
			this.versionLabel.ImageIndex = ((int)(resources.GetObject("versionLabel.ImageIndex")));
			this.versionLabel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("versionLabel.ImeMode")));
			this.versionLabel.Location = ((System.Drawing.Point)(resources.GetObject("versionLabel.Location")));
			this.versionLabel.Name = "versionLabel";
			this.versionLabel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("versionLabel.RightToLeft")));
			this.versionLabel.Size = ((System.Drawing.Size)(resources.GetObject("versionLabel.Size")));
			this.versionLabel.TabIndex = ((int)(resources.GetObject("versionLabel.TabIndex")));
			this.versionLabel.Text = resources.GetString("versionLabel.Text");
			this.versionLabel.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("versionLabel.TextAlign")));
			this.versionLabel.Visible = ((bool)(resources.GetObject("versionLabel.Visible")));
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
			// linkLabel1
			// 
			this.linkLabel1.AccessibleDescription = resources.GetString("linkLabel1.AccessibleDescription");
			this.linkLabel1.AccessibleName = resources.GetString("linkLabel1.AccessibleName");
			this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("linkLabel1.Anchor")));
			this.linkLabel1.AutoSize = ((bool)(resources.GetObject("linkLabel1.AutoSize")));
			this.linkLabel1.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("linkLabel1.Dock")));
			this.linkLabel1.Enabled = ((bool)(resources.GetObject("linkLabel1.Enabled")));
			this.linkLabel1.Font = ((System.Drawing.Font)(resources.GetObject("linkLabel1.Font")));
			this.linkLabel1.Image = ((System.Drawing.Image)(resources.GetObject("linkLabel1.Image")));
			this.linkLabel1.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("linkLabel1.ImageAlign")));
			this.linkLabel1.ImageIndex = ((int)(resources.GetObject("linkLabel1.ImageIndex")));
			this.linkLabel1.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("linkLabel1.ImeMode")));
			this.linkLabel1.LinkArea = ((System.Windows.Forms.LinkArea)(resources.GetObject("linkLabel1.LinkArea")));
			this.linkLabel1.Location = ((System.Drawing.Point)(resources.GetObject("linkLabel1.Location")));
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("linkLabel1.RightToLeft")));
			this.linkLabel1.Size = ((System.Drawing.Size)(resources.GetObject("linkLabel1.Size")));
			this.linkLabel1.TabIndex = ((int)(resources.GetObject("linkLabel1.TabIndex")));
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = resources.GetString("linkLabel1.Text");
			this.linkLabel1.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("linkLabel1.TextAlign")));
			this.linkLabel1.Visible = ((bool)(resources.GetObject("linkLabel1.Visible")));
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
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
			// copyright
			// 
			this.copyright.AccessibleDescription = resources.GetString("copyright.AccessibleDescription");
			this.copyright.AccessibleName = resources.GetString("copyright.AccessibleName");
			this.copyright.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("copyright.Anchor")));
			this.copyright.AutoSize = ((bool)(resources.GetObject("copyright.AutoSize")));
			this.copyright.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("copyright.Dock")));
			this.copyright.Enabled = ((bool)(resources.GetObject("copyright.Enabled")));
			this.copyright.Font = ((System.Drawing.Font)(resources.GetObject("copyright.Font")));
			this.copyright.Image = ((System.Drawing.Image)(resources.GetObject("copyright.Image")));
			this.copyright.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("copyright.ImageAlign")));
			this.copyright.ImageIndex = ((int)(resources.GetObject("copyright.ImageIndex")));
			this.copyright.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("copyright.ImeMode")));
			this.copyright.Location = ((System.Drawing.Point)(resources.GetObject("copyright.Location")));
			this.copyright.Name = "copyright";
			this.copyright.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("copyright.RightToLeft")));
			this.copyright.Size = ((System.Drawing.Size)(resources.GetObject("copyright.Size")));
			this.copyright.TabIndex = ((int)(resources.GetObject("copyright.TabIndex")));
			this.copyright.Text = resources.GetString("copyright.Text");
			this.copyright.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("copyright.TextAlign")));
			this.copyright.Visible = ((bool)(resources.GetObject("copyright.Visible")));
			// 
			// clrTypeLabel
			// 
			this.clrTypeLabel.AccessibleDescription = resources.GetString("clrTypeLabel.AccessibleDescription");
			this.clrTypeLabel.AccessibleName = resources.GetString("clrTypeLabel.AccessibleName");
			this.clrTypeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("clrTypeLabel.Anchor")));
			this.clrTypeLabel.AutoSize = ((bool)(resources.GetObject("clrTypeLabel.AutoSize")));
			this.clrTypeLabel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("clrTypeLabel.Dock")));
			this.clrTypeLabel.Enabled = ((bool)(resources.GetObject("clrTypeLabel.Enabled")));
			this.clrTypeLabel.Font = ((System.Drawing.Font)(resources.GetObject("clrTypeLabel.Font")));
			this.clrTypeLabel.Image = ((System.Drawing.Image)(resources.GetObject("clrTypeLabel.Image")));
			this.clrTypeLabel.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("clrTypeLabel.ImageAlign")));
			this.clrTypeLabel.ImageIndex = ((int)(resources.GetObject("clrTypeLabel.ImageIndex")));
			this.clrTypeLabel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("clrTypeLabel.ImeMode")));
			this.clrTypeLabel.Location = ((System.Drawing.Point)(resources.GetObject("clrTypeLabel.Location")));
			this.clrTypeLabel.Name = "clrTypeLabel";
			this.clrTypeLabel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("clrTypeLabel.RightToLeft")));
			this.clrTypeLabel.Size = ((System.Drawing.Size)(resources.GetObject("clrTypeLabel.Size")));
			this.clrTypeLabel.TabIndex = ((int)(resources.GetObject("clrTypeLabel.TabIndex")));
			this.clrTypeLabel.Text = resources.GetString("clrTypeLabel.Text");
			this.clrTypeLabel.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("clrTypeLabel.TextAlign")));
			this.clrTypeLabel.Visible = ((bool)(resources.GetObject("clrTypeLabel.Visible")));
			// 
			// dotNetVersionLabel
			// 
			this.dotNetVersionLabel.AccessibleDescription = resources.GetString("dotNetVersionLabel.AccessibleDescription");
			this.dotNetVersionLabel.AccessibleName = resources.GetString("dotNetVersionLabel.AccessibleName");
			this.dotNetVersionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("dotNetVersionLabel.Anchor")));
			this.dotNetVersionLabel.AutoSize = ((bool)(resources.GetObject("dotNetVersionLabel.AutoSize")));
			this.dotNetVersionLabel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("dotNetVersionLabel.Dock")));
			this.dotNetVersionLabel.Enabled = ((bool)(resources.GetObject("dotNetVersionLabel.Enabled")));
			this.dotNetVersionLabel.Font = ((System.Drawing.Font)(resources.GetObject("dotNetVersionLabel.Font")));
			this.dotNetVersionLabel.Image = ((System.Drawing.Image)(resources.GetObject("dotNetVersionLabel.Image")));
			this.dotNetVersionLabel.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("dotNetVersionLabel.ImageAlign")));
			this.dotNetVersionLabel.ImageIndex = ((int)(resources.GetObject("dotNetVersionLabel.ImageIndex")));
			this.dotNetVersionLabel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("dotNetVersionLabel.ImeMode")));
			this.dotNetVersionLabel.Location = ((System.Drawing.Point)(resources.GetObject("dotNetVersionLabel.Location")));
			this.dotNetVersionLabel.Name = "dotNetVersionLabel";
			this.dotNetVersionLabel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("dotNetVersionLabel.RightToLeft")));
			this.dotNetVersionLabel.Size = ((System.Drawing.Size)(resources.GetObject("dotNetVersionLabel.Size")));
			this.dotNetVersionLabel.TabIndex = ((int)(resources.GetObject("dotNetVersionLabel.TabIndex")));
			this.dotNetVersionLabel.Text = resources.GetString("dotNetVersionLabel.Text");
			this.dotNetVersionLabel.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("dotNetVersionLabel.TextAlign")));
			this.dotNetVersionLabel.Visible = ((bool)(resources.GetObject("dotNetVersionLabel.Visible")));
			// 
			// AboutBox
			// 
			this.AcceptButton = this.OkButton;
			this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
			this.AccessibleName = resources.GetString("$this.AccessibleName");
			this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
			this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
			this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
			this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.CancelButton = this.OkButton;
			this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
			this.Controls.Add(this.dotNetVersionLabel);
			this.Controls.Add(this.clrTypeLabel);
			this.Controls.Add(this.copyright);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.linkLabel1);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.versionLabel);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.OkButton);
			this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
			this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
			this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
			this.MaximizeBox = false;
			this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
			this.MinimizeBox = false;
			this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
			this.Name = "AboutBox";
			this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
			this.ShowInTaskbar = false;
			this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
			this.Text = resources.GetString("$this.Text");
			this.ResumeLayout(false);

		}
		#endregion

		private void button1_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start("http://nunit.org");
			linkLabel1.LinkVisited = true;
		}
	}
}
