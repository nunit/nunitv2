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
	/// <summary>
	/// Summary description for AboutBox.
	/// </summary>
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
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label dotNetVersionLabel;
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
			AssemblyProductAttribute productAttr = (AssemblyProductAttribute)objectAttrs[0];

			objectAttrs = executingAssembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
			AssemblyCopyrightAttribute copyrightAttr = (AssemblyCopyrightAttribute)objectAttrs[0];
			versionLabel.Text = version.ToString(3);
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
			this.label8 = new System.Windows.Forms.Label();
			this.dotNetVersionLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// OkButton
			// 
			this.OkButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.OkButton.Location = new System.Drawing.Point(369, 240);
			this.OkButton.Name = "OkButton";
			this.OkButton.Size = new System.Drawing.Size(96, 29);
			this.OkButton.TabIndex = 0;
			this.OkButton.Text = "OK";
			this.OkButton.Click += new System.EventHandler(this.button1_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(31, 217);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(102, 15);
			this.label1.TabIndex = 1;
			this.label1.Text = "Version:";
			// 
			// versionLabel
			// 
			this.versionLabel.Location = new System.Drawing.Point(164, 217);
			this.versionLabel.Name = "versionLabel";
			this.versionLabel.Size = new System.Drawing.Size(82, 23);
			this.versionLabel.TabIndex = 2;
			this.versionLabel.Text = "label2";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(31, 109);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(102, 28);
			this.label2.TabIndex = 3;
			this.label2.Text = "Developers:";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(164, 109);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(287, 59);
			this.label3.TabIndex = 4;
			this.label3.Text = "James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Philip Craig, Ethan Smith," +
				" Doug de la Torre, Charlie Poole";
			// 
			// linkLabel1
			// 
			this.linkLabel1.Location = new System.Drawing.Point(164, 79);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(266, 28);
			this.linkLabel1.TabIndex = 5;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "http://www.nunit.org ";
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(31, 79);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(102, 28);
			this.label4.TabIndex = 6;
			this.label4.Text = "Information:";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(31, 178);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(102, 28);
			this.label5.TabIndex = 7;
			this.label5.Text = "Thanks to:";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(164, 178);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(215, 28);
			this.label6.TabIndex = 8;
			this.label6.Text = "Kent Beck and Erich Gamma";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(31, 20);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(102, 28);
			this.label7.TabIndex = 9;
			this.label7.Text = "Copyright:";
			// 
			// copyright
			// 
			this.copyright.Location = new System.Drawing.Point(164, 20);
			this.copyright.Name = "copyright";
			this.copyright.Size = new System.Drawing.Size(297, 59);
			this.copyright.TabIndex = 10;
			this.copyright.Text = "label8";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(31, 248);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(102, 15);
			this.label8.TabIndex = 11;
			this.label8.Text = ".NET Version:";
			// 
			// dotNetVersionLabel
			// 
			this.dotNetVersionLabel.Location = new System.Drawing.Point(164, 248);
			this.dotNetVersionLabel.Name = "dotNetVersionLabel";
			this.dotNetVersionLabel.Size = new System.Drawing.Size(132, 23);
			this.dotNetVersionLabel.TabIndex = 12;
			this.dotNetVersionLabel.Text = "label9";
			// 
			// AboutBox
			// 
			this.AcceptButton = this.OkButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.CancelButton = this.OkButton;
			this.ClientSize = new System.Drawing.Size(491, 282);
			this.Controls.Add(this.dotNetVersionLabel);
			this.Controls.Add(this.label8);
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
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AboutBox";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "About NUnit";
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
