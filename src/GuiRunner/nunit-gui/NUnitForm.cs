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
using System.Configuration;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace NUnit.Gui
{
	using NUnit.Core;
	using NUnit.Util;
	using NUnit.UiKit;
	using CP.Windows.Forms;

	public class NUnitForm : System.Windows.Forms.Form
	{
		#region Instance variables

		// Handlers for our recentFiles and recentProjects
		private RecentFileMenuHandler recentProjectsMenuHandler;

		// Structure used for command line options
		public struct CommandLineOptions
		{
			public string testFileName;
			public string configName;
			public string testName;
			public bool noload;
			public bool autorun;
		}

		// Our current run command line options
		private CommandLineOptions commandLineOptions;

		// TipWindow for the detail list
		CP.Windows.Forms.TipWindow tipWindow;
		int hoverIndex = -1;

		private TestTree testTree;
		public System.Windows.Forms.Splitter splitter1;
		public System.Windows.Forms.Panel panel1;
		public System.Windows.Forms.TabPage testsNotRun;
		public System.Windows.Forms.MenuItem exitMenuItem;
		public System.Windows.Forms.MenuItem helpMenuItem;
		public System.Windows.Forms.MenuItem aboutMenuItem;
		public System.Windows.Forms.MainMenu mainMenu;
		private System.Windows.Forms.ListBox detailList;
		public System.Windows.Forms.Splitter splitter3;
		public System.Windows.Forms.TextBox stackTrace;
		public NUnit.UiKit.StatusBar statusBar;
		public System.Windows.Forms.TabControl resultTabs;
		public System.Windows.Forms.TabPage errorPage;
		public System.Windows.Forms.TabPage stderr;
		public System.Windows.Forms.TabPage stdout;
		public System.Windows.Forms.RichTextBox stdErrTab;
		public System.Windows.Forms.RichTextBox stdOutTab;
		public NUnit.UiKit.NotRunTree notRunTree;
		private System.ComponentModel.IContainer components;
		public System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.MenuItem closeMenuItem;
		public System.Windows.Forms.MenuItem fileMenu;
		public System.Windows.Forms.MenuItem helpItem;
		public System.Windows.Forms.MenuItem helpMenuSeparator1;
		private System.Windows.Forms.MenuItem reloadMenuItem;
		public System.Windows.Forms.GroupBox groupBox1;
		public System.Windows.Forms.Button runButton;
		public System.Windows.Forms.Label suiteName;
		public NUnit.UiKit.ProgressBar progressBar;
		private System.Windows.Forms.Button stopButton;
		private System.Windows.Forms.MenuItem toolsMenu;
		private System.Windows.Forms.MenuItem optionsMenuItem;
		private System.Windows.Forms.MenuItem saveXmlResultsMenuItem;
		private System.Windows.Forms.MenuItem projectMenu;
		private System.Windows.Forms.MenuItem editProjectMenuItem;
		private System.Windows.Forms.MenuItem configMenuItem;
		private System.Windows.Forms.MenuItem saveMenuItem;
		private System.Windows.Forms.MenuItem saveAsMenuItem;
		private System.Windows.Forms.MenuItem newMenuItem;
		private System.Windows.Forms.MenuItem openMenuItem;
		private System.Windows.Forms.MenuItem recentProjectsMenu;
		private System.Windows.Forms.MenuItem fileMenuSeparator1;
		private System.Windows.Forms.MenuItem fileMenuSeparator2;
		public System.Windows.Forms.MenuItem fileMenuSeparator4;
		private System.Windows.Forms.MenuItem projectMenuSeparator1;
		private System.Windows.Forms.MenuItem projectMenuSeparator2;
		private System.Windows.Forms.MenuItem toolsMenuSeparator1;
		private System.Windows.Forms.MenuItem addVSProjectMenuItem;
		private System.Windows.Forms.ContextMenu detailListContextMenu;
		private System.Windows.Forms.MenuItem copyDetailMenuItem;
		private System.Windows.Forms.MenuItem exceptionDetailsMenuItem;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.MenuItem addAssemblyMenuItem;

		#endregion
		
		#region Construction and Disposal

		public NUnitForm( CommandLineOptions commandLineOptions )
		{
			InitializeComponent();

			this.mainMenu.MenuItems.Add(1, testTree.ViewMenu);

			this.commandLineOptions = commandLineOptions;

			stdErrTab.Enabled = true;
			stdOutTab.Enabled = true;

			runButton.Enabled = false;
			stopButton.Enabled = false;

			AppUI.Init(
				this,
				new TextBoxWriter( stdOutTab ),
				new TextBoxWriter( stdErrTab ) );

			recentProjectsMenuHandler = new RecentFileMenuHandler( recentProjectsMenu, UserSettings.RecentProjects );
		}

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
		#endregion
		
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(NUnitForm));
			this.statusBar = new NUnit.UiKit.StatusBar();
			this.mainMenu = new System.Windows.Forms.MainMenu();
			this.fileMenu = new System.Windows.Forms.MenuItem();
			this.newMenuItem = new System.Windows.Forms.MenuItem();
			this.openMenuItem = new System.Windows.Forms.MenuItem();
			this.closeMenuItem = new System.Windows.Forms.MenuItem();
			this.fileMenuSeparator1 = new System.Windows.Forms.MenuItem();
			this.saveMenuItem = new System.Windows.Forms.MenuItem();
			this.saveAsMenuItem = new System.Windows.Forms.MenuItem();
			this.fileMenuSeparator2 = new System.Windows.Forms.MenuItem();
			this.reloadMenuItem = new System.Windows.Forms.MenuItem();
			this.recentProjectsMenu = new System.Windows.Forms.MenuItem();
			this.fileMenuSeparator4 = new System.Windows.Forms.MenuItem();
			this.exitMenuItem = new System.Windows.Forms.MenuItem();
			this.projectMenu = new System.Windows.Forms.MenuItem();
			this.configMenuItem = new System.Windows.Forms.MenuItem();
			this.projectMenuSeparator1 = new System.Windows.Forms.MenuItem();
			this.addAssemblyMenuItem = new System.Windows.Forms.MenuItem();
			this.addVSProjectMenuItem = new System.Windows.Forms.MenuItem();
			this.projectMenuSeparator2 = new System.Windows.Forms.MenuItem();
			this.editProjectMenuItem = new System.Windows.Forms.MenuItem();
			this.toolsMenu = new System.Windows.Forms.MenuItem();
			this.saveXmlResultsMenuItem = new System.Windows.Forms.MenuItem();
			this.exceptionDetailsMenuItem = new System.Windows.Forms.MenuItem();
			this.toolsMenuSeparator1 = new System.Windows.Forms.MenuItem();
			this.optionsMenuItem = new System.Windows.Forms.MenuItem();
			this.helpItem = new System.Windows.Forms.MenuItem();
			this.helpMenuItem = new System.Windows.Forms.MenuItem();
			this.helpMenuSeparator1 = new System.Windows.Forms.MenuItem();
			this.aboutMenuItem = new System.Windows.Forms.MenuItem();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.panel1 = new System.Windows.Forms.Panel();
			this.resultTabs = new System.Windows.Forms.TabControl();
			this.errorPage = new System.Windows.Forms.TabPage();
			this.stackTrace = new System.Windows.Forms.TextBox();
			this.splitter3 = new System.Windows.Forms.Splitter();
			this.detailList = new System.Windows.Forms.ListBox();
			this.testsNotRun = new System.Windows.Forms.TabPage();
			this.notRunTree = new NUnit.UiKit.NotRunTree();
			this.stderr = new System.Windows.Forms.TabPage();
			this.stdErrTab = new System.Windows.Forms.RichTextBox();
			this.stdout = new System.Windows.Forms.TabPage();
			this.stdOutTab = new System.Windows.Forms.RichTextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.stopButton = new System.Windows.Forms.Button();
			this.runButton = new System.Windows.Forms.Button();
			this.suiteName = new System.Windows.Forms.Label();
			this.progressBar = new NUnit.UiKit.ProgressBar();
			this.detailListContextMenu = new System.Windows.Forms.ContextMenu();
			this.copyDetailMenuItem = new System.Windows.Forms.MenuItem();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.testTree = new NUnit.UiKit.TestTree();
			this.panel2 = new System.Windows.Forms.Panel();
			this.panel1.SuspendLayout();
			this.resultTabs.SuspendLayout();
			this.errorPage.SuspendLayout();
			this.testsNotRun.SuspendLayout();
			this.stderr.SuspendLayout();
			this.stdout.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// statusBar
			// 
			this.statusBar.DisplayTestProgress = true;
			this.statusBar.Location = new System.Drawing.Point(0, 497);
			this.statusBar.Name = "statusBar";
			this.statusBar.ShowPanels = true;
			this.statusBar.Size = new System.Drawing.Size(623, 32);
			this.statusBar.TabIndex = 0;
			this.statusBar.Text = "Status";
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.fileMenu,
																					 this.projectMenu,
																					 this.toolsMenu,
																					 this.helpItem});
			// 
			// fileMenu
			// 
			this.fileMenu.Index = 0;
			this.fileMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.newMenuItem,
																					 this.openMenuItem,
																					 this.closeMenuItem,
																					 this.fileMenuSeparator1,
																					 this.saveMenuItem,
																					 this.saveAsMenuItem,
																					 this.fileMenuSeparator2,
																					 this.reloadMenuItem,
																					 this.recentProjectsMenu,
																					 this.fileMenuSeparator4,
																					 this.exitMenuItem});
			this.fileMenu.Text = "&File";
			this.fileMenu.Popup += new System.EventHandler(this.fileMenu_Popup);
			// 
			// newMenuItem
			// 
			this.newMenuItem.Index = 0;
			this.newMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlN;
			this.newMenuItem.Text = "&New Project...";
			this.newMenuItem.Click += new System.EventHandler(this.newMenuItem_Click);
			// 
			// openMenuItem
			// 
			this.openMenuItem.Index = 1;
			this.openMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
			this.openMenuItem.Text = "&Open...";
			this.openMenuItem.Click += new System.EventHandler(this.openMenuItem_Click);
			// 
			// closeMenuItem
			// 
			this.closeMenuItem.Index = 2;
			this.closeMenuItem.Text = "&Close";
			this.closeMenuItem.Click += new System.EventHandler(this.closeMenuItem_Click);
			// 
			// fileMenuSeparator1
			// 
			this.fileMenuSeparator1.Index = 3;
			this.fileMenuSeparator1.Text = "-";
			// 
			// saveMenuItem
			// 
			this.saveMenuItem.Index = 4;
			this.saveMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
			this.saveMenuItem.Text = "&Save";
			this.saveMenuItem.Click += new System.EventHandler(this.saveMenuItem_Click);
			// 
			// saveAsMenuItem
			// 
			this.saveAsMenuItem.Index = 5;
			this.saveAsMenuItem.Text = "Save &As...";
			this.saveAsMenuItem.Click += new System.EventHandler(this.saveAsMenuItem_Click);
			// 
			// fileMenuSeparator2
			// 
			this.fileMenuSeparator2.Index = 6;
			this.fileMenuSeparator2.Text = "-";
			// 
			// reloadMenuItem
			// 
			this.reloadMenuItem.Index = 7;
			this.reloadMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlR;
			this.reloadMenuItem.Text = "&Reload";
			this.reloadMenuItem.Click += new System.EventHandler(this.reloadMenuItem_Click);
			// 
			// recentProjectsMenu
			// 
			this.recentProjectsMenu.Index = 8;
			this.recentProjectsMenu.Text = "Recent &Files";
			// 
			// fileMenuSeparator4
			// 
			this.fileMenuSeparator4.Index = 9;
			this.fileMenuSeparator4.Text = "-";
			// 
			// exitMenuItem
			// 
			this.exitMenuItem.Index = 10;
			this.exitMenuItem.Text = "E&xit";
			this.exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
			// 
			// projectMenu
			// 
			this.projectMenu.Index = 1;
			this.projectMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						this.configMenuItem,
																						this.projectMenuSeparator1,
																						this.addAssemblyMenuItem,
																						this.addVSProjectMenuItem,
																						this.projectMenuSeparator2,
																						this.editProjectMenuItem});
			this.projectMenu.Text = "&Project";
			this.projectMenu.Visible = false;
			this.projectMenu.Popup += new System.EventHandler(this.projectMenu_Popup);
			// 
			// configMenuItem
			// 
			this.configMenuItem.Index = 0;
			this.configMenuItem.Text = "&Configurations";
			// 
			// projectMenuSeparator1
			// 
			this.projectMenuSeparator1.Index = 1;
			this.projectMenuSeparator1.Text = "-";
			// 
			// addAssemblyMenuItem
			// 
			this.addAssemblyMenuItem.Index = 2;
			this.addAssemblyMenuItem.Text = "Add Assembly...";
			this.addAssemblyMenuItem.Click += new System.EventHandler(this.addAssemblyMenuItem_Click);
			// 
			// addVSProjectMenuItem
			// 
			this.addVSProjectMenuItem.Index = 3;
			this.addVSProjectMenuItem.Text = "Add VS Project...";
			this.addVSProjectMenuItem.Click += new System.EventHandler(this.addVSProjectMenuItem_Click);
			// 
			// projectMenuSeparator2
			// 
			this.projectMenuSeparator2.Index = 4;
			this.projectMenuSeparator2.Text = "-";
			// 
			// editProjectMenuItem
			// 
			this.editProjectMenuItem.Index = 5;
			this.editProjectMenuItem.Text = "Edit...";
			this.editProjectMenuItem.Click += new System.EventHandler(this.editProjectMenuItem_Click);
			// 
			// toolsMenu
			// 
			this.toolsMenu.Index = 2;
			this.toolsMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.saveXmlResultsMenuItem,
																					  this.exceptionDetailsMenuItem,
																					  this.toolsMenuSeparator1,
																					  this.optionsMenuItem});
			this.toolsMenu.Text = "&Tools";
			this.toolsMenu.Popup += new System.EventHandler(this.toolsMenu_Popup);
			// 
			// saveXmlResultsMenuItem
			// 
			this.saveXmlResultsMenuItem.Index = 0;
			this.saveXmlResultsMenuItem.Text = "&Save Results as XML...";
			this.saveXmlResultsMenuItem.Click += new System.EventHandler(this.saveXmlResultsMenuItem_Click);
			// 
			// exceptionDetailsMenuItem
			// 
			this.exceptionDetailsMenuItem.Index = 1;
			this.exceptionDetailsMenuItem.Text = "&Exception Details...";
			this.exceptionDetailsMenuItem.Click += new System.EventHandler(this.exceptionDetailsMenuItem_Click);
			// 
			// toolsMenuSeparator1
			// 
			this.toolsMenuSeparator1.Index = 2;
			this.toolsMenuSeparator1.Text = "-";
			// 
			// optionsMenuItem
			// 
			this.optionsMenuItem.Index = 3;
			this.optionsMenuItem.Text = "&Options...";
			this.optionsMenuItem.Click += new System.EventHandler(this.optionsMenuItem_Click);
			// 
			// helpItem
			// 
			this.helpItem.Index = 3;
			this.helpItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.helpMenuItem,
																					 this.helpMenuSeparator1,
																					 this.aboutMenuItem});
			this.helpItem.Text = "&Help";
			// 
			// helpMenuItem
			// 
			this.helpMenuItem.Index = 0;
			this.helpMenuItem.Shortcut = System.Windows.Forms.Shortcut.F1;
			this.helpMenuItem.Text = "NUnit &Help...";
			this.helpMenuItem.Click += new System.EventHandler(this.helpMenuItem_Click);
			// 
			// helpMenuSeparator1
			// 
			this.helpMenuSeparator1.Index = 1;
			this.helpMenuSeparator1.Text = "-";
			// 
			// aboutMenuItem
			// 
			this.aboutMenuItem.Index = 2;
			this.aboutMenuItem.Text = "&About NUnit...";
			this.aboutMenuItem.Click += new System.EventHandler(this.aboutMenuItem_Click);
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(200, 0);
			this.splitter1.MinSize = 240;
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(5, 497);
			this.splitter1.TabIndex = 2;
			this.splitter1.TabStop = false;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.resultTabs);
			this.panel1.Controls.Add(this.groupBox1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(205, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(418, 497);
			this.panel1.TabIndex = 3;
			// 
			// resultTabs
			// 
			this.resultTabs.Controls.Add(this.errorPage);
			this.resultTabs.Controls.Add(this.testsNotRun);
			this.resultTabs.Controls.Add(this.stderr);
			this.resultTabs.Controls.Add(this.stdout);
			this.resultTabs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.resultTabs.Location = new System.Drawing.Point(0, 88);
			this.resultTabs.Name = "resultTabs";
			this.resultTabs.SelectedIndex = 0;
			this.resultTabs.Size = new System.Drawing.Size(418, 409);
			this.resultTabs.TabIndex = 2;
			// 
			// errorPage
			// 
			this.errorPage.Controls.Add(this.stackTrace);
			this.errorPage.Controls.Add(this.splitter3);
			this.errorPage.Controls.Add(this.detailList);
			this.errorPage.Location = new System.Drawing.Point(4, 22);
			this.errorPage.Name = "errorPage";
			this.errorPage.Size = new System.Drawing.Size(410, 383);
			this.errorPage.TabIndex = 0;
			this.errorPage.Text = "Errors and Failures";
			// 
			// stackTrace
			// 
			this.stackTrace.Dock = System.Windows.Forms.DockStyle.Fill;
			this.stackTrace.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.stackTrace.Location = new System.Drawing.Point(0, 127);
			this.stackTrace.Multiline = true;
			this.stackTrace.Name = "stackTrace";
			this.stackTrace.ReadOnly = true;
			this.stackTrace.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.stackTrace.Size = new System.Drawing.Size(410, 256);
			this.stackTrace.TabIndex = 2;
			this.stackTrace.Text = "";
			this.stackTrace.WordWrap = false;
			// 
			// splitter3
			// 
			this.splitter3.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter3.Location = new System.Drawing.Point(0, 124);
			this.splitter3.MinSize = 100;
			this.splitter3.Name = "splitter3";
			this.splitter3.Size = new System.Drawing.Size(410, 3);
			this.splitter3.TabIndex = 1;
			this.splitter3.TabStop = false;
			// 
			// detailList
			// 
			this.detailList.Dock = System.Windows.Forms.DockStyle.Top;
			this.detailList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
			this.detailList.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.detailList.HorizontalExtent = 2000;
			this.detailList.HorizontalScrollbar = true;
			this.detailList.ItemHeight = 16;
			this.detailList.Location = new System.Drawing.Point(0, 0);
			this.detailList.Name = "detailList";
			this.detailList.ScrollAlwaysVisible = true;
			this.detailList.Size = new System.Drawing.Size(410, 124);
			this.detailList.TabIndex = 0;
			this.detailList.MouseHover += new System.EventHandler(this.detailList_MouseHover);
			this.detailList.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.detailList_MeasureItem);
			this.detailList.MouseMove += new System.Windows.Forms.MouseEventHandler(this.detailList_MouseMove);
			this.detailList.MouseLeave += new System.EventHandler(this.detailList_MouseLeave);
			this.detailList.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.detailList_DrawItem);
			this.detailList.SelectedIndexChanged += new System.EventHandler(this.detailList_SelectedIndexChanged);
			// 
			// testsNotRun
			// 
			this.testsNotRun.Controls.Add(this.notRunTree);
			this.testsNotRun.Location = new System.Drawing.Point(4, 22);
			this.testsNotRun.Name = "testsNotRun";
			this.testsNotRun.Size = new System.Drawing.Size(410, 383);
			this.testsNotRun.TabIndex = 1;
			this.testsNotRun.Text = "Tests Not Run";
			// 
			// notRunTree
			// 
			this.notRunTree.Dock = System.Windows.Forms.DockStyle.Fill;
			this.notRunTree.ImageIndex = -1;
			this.notRunTree.Location = new System.Drawing.Point(0, 0);
			this.notRunTree.Name = "notRunTree";
			this.notRunTree.SelectedImageIndex = -1;
			this.notRunTree.Size = new System.Drawing.Size(410, 383);
			this.notRunTree.TabIndex = 0;
			// 
			// stderr
			// 
			this.stderr.Controls.Add(this.stdErrTab);
			this.stderr.Location = new System.Drawing.Point(4, 22);
			this.stderr.Name = "stderr";
			this.stderr.Size = new System.Drawing.Size(410, 383);
			this.stderr.TabIndex = 2;
			this.stderr.Text = "Console.Error";
			// 
			// stdErrTab
			// 
			this.stdErrTab.Dock = System.Windows.Forms.DockStyle.Fill;
			this.stdErrTab.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.stdErrTab.Location = new System.Drawing.Point(0, 0);
			this.stdErrTab.Name = "stdErrTab";
			this.stdErrTab.ReadOnly = true;
			this.stdErrTab.Size = new System.Drawing.Size(410, 383);
			this.stdErrTab.TabIndex = 0;
			this.stdErrTab.Text = "";
			this.stdErrTab.WordWrap = false;
			// 
			// stdout
			// 
			this.stdout.Controls.Add(this.stdOutTab);
			this.stdout.Location = new System.Drawing.Point(4, 22);
			this.stdout.Name = "stdout";
			this.stdout.Size = new System.Drawing.Size(410, 383);
			this.stdout.TabIndex = 3;
			this.stdout.Text = "Console.Out";
			// 
			// stdOutTab
			// 
			this.stdOutTab.Dock = System.Windows.Forms.DockStyle.Fill;
			this.stdOutTab.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.stdOutTab.Location = new System.Drawing.Point(0, 0);
			this.stdOutTab.Name = "stdOutTab";
			this.stdOutTab.ReadOnly = true;
			this.stdOutTab.Size = new System.Drawing.Size(410, 383);
			this.stdOutTab.TabIndex = 0;
			this.stdOutTab.Text = "";
			this.stdOutTab.WordWrap = false;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.stopButton);
			this.groupBox1.Controls.Add(this.runButton);
			this.groupBox1.Controls.Add(this.suiteName);
			this.groupBox1.Controls.Add(this.progressBar);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(418, 88);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			// 
			// stopButton
			// 
			this.stopButton.Location = new System.Drawing.Point(94, 16);
			this.stopButton.Name = "stopButton";
			this.stopButton.Size = new System.Drawing.Size(73, 31);
			this.stopButton.TabIndex = 4;
			this.stopButton.Text = "&Stop";
			this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
			// 
			// runButton
			// 
			this.runButton.Location = new System.Drawing.Point(7, 16);
			this.runButton.Name = "runButton";
			this.runButton.Size = new System.Drawing.Size(74, 31);
			this.runButton.TabIndex = 3;
			this.runButton.Text = "&Run";
			this.runButton.Click += new System.EventHandler(this.runButton_Click);
			// 
			// suiteName
			// 
			this.suiteName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.suiteName.Location = new System.Drawing.Point(179, 24);
			this.suiteName.Name = "suiteName";
			this.suiteName.Size = new System.Drawing.Size(231, 25);
			this.suiteName.TabIndex = 2;
			// 
			// progressBar
			// 
			this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.progressBar.CausesValidation = false;
			this.progressBar.Enabled = false;
			this.progressBar.ForeColor = System.Drawing.SystemColors.Highlight;
			this.progressBar.Location = new System.Drawing.Point(7, 56);
			this.progressBar.Maximum = 100;
			this.progressBar.Minimum = 0;
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(403, 25);
			this.progressBar.Step = 1;
			this.progressBar.TabIndex = 0;
			this.progressBar.Value = 0;
			// 
			// detailListContextMenu
			// 
			this.detailListContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																								  this.copyDetailMenuItem});
			// 
			// copyDetailMenuItem
			// 
			this.copyDetailMenuItem.Index = 0;
			this.copyDetailMenuItem.Text = "Copy";
			this.copyDetailMenuItem.Click += new System.EventHandler(this.copyDetailMenuItem_Click);
			// 
			// testTree
			// 
			this.testTree.Dock = System.Windows.Forms.DockStyle.Fill;
			this.testTree.Location = new System.Drawing.Point(0, 0);
			this.testTree.Name = "testTree";
			this.testTree.Size = new System.Drawing.Size(200, 497);
			this.testTree.TabIndex = 0;
			this.testTree.SelectedTestsChanged += new NUnit.UiKit.SelectedTestsChangedEventHandler(this.testTree_SelectedTestsChanged);
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.testTree);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(200, 497);
			this.panel2.TabIndex = 4;
			// 
			// NUnitForm
			// 
			this.AcceptButton = this.runButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(623, 529);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.statusBar);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenu;
			this.MinimumSize = new System.Drawing.Size(160, 32);
			this.Name = "NUnitForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "NUnit";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.NUnitForm_Closing);
			this.Load += new System.EventHandler(this.NUnitForm_Load);
			this.panel1.ResumeLayout(false);
			this.resultTabs.ResumeLayout(false);
			this.errorPage.ResumeLayout(false);
			this.testsNotRun.ResumeLayout(false);
			this.stderr.ResumeLayout(false);
			this.stdout.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Properties used internally

		private TestLoader TestLoader
		{
			get { return AppUI.TestLoader; }
		}

		private TestLoaderUI TestLoaderUI
		{
			get { return AppUI.TestLoaderUI; }
		}

		private bool IsProjectLoaded
		{
			get { return TestLoader.IsProjectLoaded; }
		}

		private NUnitProject TestProject
		{
			get { return TestLoader.TestProject; }
		}

		private bool IsTestLoaded
		{
			get { return TestLoader.IsTestLoaded; }
		}

		private bool IsTestRunning
		{
			get { return TestLoader.IsTestRunning; }
		}
		#endregion

		#region File Menu Handlers

		private void fileMenu_Popup(object sender, System.EventArgs e)
		{
			newMenuItem.Enabled = !IsTestRunning;
			openMenuItem.Enabled = !IsTestRunning;
			closeMenuItem.Enabled = IsProjectLoaded && !IsTestRunning;

			saveMenuItem.Enabled = IsProjectLoaded;
			saveAsMenuItem.Enabled = IsProjectLoaded;

			reloadMenuItem.Enabled = IsTestLoaded && !IsTestRunning;

			recentProjectsMenu.Enabled = !IsTestRunning;

			if ( !IsTestRunning )
				recentProjectsMenuHandler.Load();
		}

		private void newMenuItem_Click(object sender, System.EventArgs e)
		{
			if ( IsProjectLoaded )
				TestLoaderUI.CloseProject();

			TestLoaderUI.NewProject();
		}

		private void openMenuItem_Click(object sender, System.EventArgs e)
		{
			TestLoaderUI.OpenProject();
		}

		private void closeMenuItem_Click(object sender, System.EventArgs e)
		{
			TestLoaderUI.CloseProject();
		}

		private void saveMenuItem_Click(object sender, System.EventArgs e)
		{
			TestLoaderUI.SaveProject();
		}

		private void saveAsMenuItem_Click(object sender, System.EventArgs e)
		{
			TestLoaderUI.SaveProjectAs();
		}

		private void reloadMenuItem_Click(object sender, System.EventArgs e)
		{
			using ( new CP.Windows.Forms.WaitCursor() )
			{
				TestLoader.ReloadTest();
			}
		}

		private void exitMenuItem_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		#endregion

		#region Project Menu Handlers

		/// <summary>
		/// When the project menu pops up, we populate the
		/// submenu for configurations dynamically.
		/// </summary>
		private void projectMenu_Popup(object sender, System.EventArgs e)
		{
			int index = 0;
			configMenuItem.MenuItems.Clear();

			foreach ( ProjectConfig config in TestProject.Configs )
			{
				string text = string.Format( "&{0} {1}", index+1, config.Name );
				MenuItem item = new MenuItem( 
					text, new EventHandler( configMenuItem_Click ) );
				if ( config.Name == TestProject.ActiveConfigName ) 
					item.Checked = true;
				configMenuItem.MenuItems.Add( index++, item );
			}

			configMenuItem.MenuItems.Add( "-" );

			configMenuItem.MenuItems.Add( "&Add...",
				new System.EventHandler( addConfigurationMenuItem_Click ) );

			configMenuItem.MenuItems.Add( "&Edit...", 
				new System.EventHandler( editConfigurationsMenuItem_Click ) );

			addVSProjectMenuItem.Visible = UserSettings.Options.VisualStudioSupport;
		}

		private void configMenuItem_Click( object sender, EventArgs e )
		{
			MenuItem item = (MenuItem)sender;
			if ( !item.Checked )
				TestProject.SetActiveConfig( TestProject.Configs[item.Index].Name );
		}

		private void addConfigurationMenuItem_Click( object sender, System.EventArgs e )
		{
			ConfigurationEditor.AddConfiguration( TestProject );
		}

		private void editConfigurationsMenuItem_Click( object sender, System.EventArgs e )
		{
			ConfigurationEditor.Edit( TestProject );
		}

		private void addAssemblyMenuItem_Click(object sender, System.EventArgs e)
		{
			TestLoaderUI.AddAssembly();
		}

		private void addVSProjectMenuItem_Click(object sender, System.EventArgs e)
		{
			TestLoaderUI.AddVSProject();
		}

		private void editProjectMenuItem_Click(object sender, System.EventArgs e)
		{
			ProjectEditor editor = new ProjectEditor( TestProject );
			editor.ShowDialog( this );
		}

		#endregion

		#region Tools Menu Handlers

		private void toolsMenu_Popup(object sender, System.EventArgs e)
		{		
			saveXmlResultsMenuItem.Enabled = IsTestLoaded && TestLoader.Results != null;
			exceptionDetailsMenuItem.Enabled = TestLoader.LastException != null;
		}

		private void saveXmlResultsMenuItem_Click(object sender, System.EventArgs e)
		{
			TestLoaderUI.SaveLastResult();
		}

		private void exceptionDetailsMenuItem_Click(object sender, System.EventArgs e)
		{
			ExceptionDetailsForm details = new ExceptionDetailsForm( TestLoader.LastException );
			details.ShowDialog();
		}

		private void optionsMenuItem_Click(object sender, System.EventArgs e)
		{
			OptionsDialog.EditOptions();
		}

		#endregion

		#region Help Menu Handlers

		/// <summary>
		/// Display the about box when menu item is selected
		/// </summary>
		private void aboutMenuItem_Click(object sender, System.EventArgs e)
		{
			AboutBox aboutBox = new AboutBox();
			aboutBox.ShowDialog();
		}

		#endregion

		#region Form Level Events

		/// <summary>
		/// Exit application when space key is tapped
		/// </summary>
		protected override bool ProcessKeyPreview(ref 
			System.Windows.Forms.Message m) 
		{ 
			const int SPACE_BAR=32; 
			const int WM_CHAR = 258;

			if (m.Msg == WM_CHAR && m.WParam.ToInt32() == SPACE_BAR )
			{ 
				int altKeyBit = (int)m.LParam & 0x10000000;
				if ( altKeyBit == 0 )
				{
					this.Close();
					return true;
				}
			}

			return base.ProcessKeyEventArgs( ref m ); 
		} 

		/// <summary>
		///	Form is about to close, first see if we 
		///	have a test run going on and if so whether
		///	we should cancel it. Then unload the 
		///	test and save the latest form position.
		/// </summary>
		private void NUnitForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if ( IsTestRunning )
			{
				DialogResult dialogResult = UserMessage.Ask( 
					"A test is running, do you want to stop the test and exit?" );

				if ( dialogResult == DialogResult.No )
					e.Cancel = true;
				else
					TestLoader.CancelTestRun();
			}

			if ( !e.Cancel && IsProjectLoaded && 
				 TestLoaderUI.CloseProject() == DialogResult.Cancel )
				e.Cancel = true;
		}

		/// <summary>
		/// Get saved options when form loads
		/// </summary>
		private void NUnitForm_Load(object sender, System.EventArgs e)
		{
			LoadFormSettings();
			SubscribeToTestEvents();
			InitializeControls();
			
			// Load test specified on command line or
			// the most recent one if options call for it
			if ( commandLineOptions.testFileName != null )
				TestLoaderUI.OpenProject( commandLineOptions.testFileName, commandLineOptions.configName, commandLineOptions.testName );
			else if( UserSettings.Options.LoadLastProject && !commandLineOptions.noload )
			{
				string recentProjectName = UserSettings.RecentProjects.RecentFile;
				if ( recentProjectName != null )
					TestLoaderUI.OpenProject( recentProjectName, commandLineOptions.configName, commandLineOptions.testName );
			}

			// Run loaded test automatically if called for
			if ( commandLineOptions.autorun && TestLoader.IsTestLoaded )
			{
				// TODO: Temporary fix to avoid problem when /run is used 
				// with ReloadOnRun turned on. Refactor TestLoader so
				// we can just do a run without reload.
				bool reload = TestLoader.ReloadOnRun;
				
				try
				{
					TestLoader.ReloadOnRun = false;
					TestLoader.RunLoadedTest();
				}
				finally
				{
					TestLoader.ReloadOnRun = reload;
				}
			}
		}
			
		private void LoadFormSettings()
		{
			// Set position of the form
			this.Location = UserSettings.Form.Location;
			this.Size = UserSettings.Form.Size;

			// Maximize window if that was it's last state
			if ( UserSettings.Form.IsMaximized )
				this.WindowState = FormWindowState.Maximized;

			// Handle changes to form position
			this.Move += new System.EventHandler(this.NUnitForm_Move);
			this.Resize += new System.EventHandler(this.NUnitForm_Resize);

			// Set the splitter positions
			this.splitter1.SplitPosition = UserSettings.Form.TreeSplitterPosition;
			this.splitter3.SplitPosition = UserSettings.Form.TabSplitterPosition;

			// Handle changes in splitter positions
			this.splitter1.SplitterMoved += new SplitterEventHandler( splitter1_SplitterMoved );
			this.splitter3.SplitterMoved += new SplitterEventHandler( splitter3_SplitterMoved );
		}

		private void SubscribeToTestEvents()
		{
			IProjectEvents events = TestLoader.Events;

			events.RunStarting += new TestEventHandler( OnRunStarting );
			events.RunFinished += new TestEventHandler( OnRunFinished );

			events.ProjectLoaded	+= new TestProjectEventHandler( OnTestProjectLoaded );
			events.ProjectLoadFailed+= new TestProjectEventHandler( OnProjectLoadFailure );
			events.ProjectUnloaded	+= new TestProjectEventHandler( OnTestProjectUnloaded );

			events.TestLoading		+= new TestEventHandler( OnTestLoadStarting );
			events.TestLoaded		+= new TestEventHandler( OnTestLoaded );
			events.TestLoadFailed	+= new TestEventHandler( OnTestLoadFailure );
			events.TestUnloading	+= new TestEventHandler( OnTestUnloadStarting );
			events.TestUnloaded		+= new TestEventHandler( OnTestUnloaded );
			events.TestReloading	+= new TestEventHandler( OnReloadStarting );
			events.TestReloaded		+= new TestEventHandler( OnTestChanged );
			events.TestReloadFailed	+= new TestEventHandler( OnTestLoadFailure );
			events.TestFinished		+= new TestEventHandler( OnTestFinished );
			events.SuiteFinished	+= new TestEventHandler( OnSuiteFinished );
			events.TestException	+= new TestEventHandler( OnTestException );
		}

		private void InitializeControls()
		{
			// ToDo: Migrate more ui elements to handle events on their own.
			this.testTree.Initialize(TestLoader);
			this.progressBar.Initialize( TestLoader.Events );
			this.statusBar.Initialize( TestLoader.Events );

			// Set controls to match option settings. We do this
			// here rather than in the controls since there may
			// be more than one app that uses the same controls.
			testTree.tests.ClearResultsOnChange = 
				UserSettings.Options.ClearResults;
		}

		private void NUnitForm_Move(object sender, System.EventArgs e)
		{
			if ( this.WindowState == FormWindowState.Normal )
			{
				UserSettings.Form.Location = this.Location;
				UserSettings.Form.IsMaximized = false;

				this.statusBar.SizingGrip = true;
			}
			else if ( this.WindowState == FormWindowState.Maximized )
			{
				UserSettings.Form.IsMaximized = true;

				this.statusBar.SizingGrip = false;
			}
		}

		private void NUnitForm_Resize(object sender, System.EventArgs e)
		{
			if ( this.WindowState == FormWindowState.Normal )
				UserSettings.Form.Size = this.Size;
		}

		private void splitter1_SplitterMoved( object sender, SplitterEventArgs e )
		{
			UserSettings.Form.TreeSplitterPosition = splitter1.SplitPosition;
		}

		private void splitter3_SplitterMoved( object sender, SplitterEventArgs e )
		{
			UserSettings.Form.TabSplitterPosition = splitter3.SplitPosition;
		}

		#endregion

		#region Other UI Event Handlers

		/// <summary>
		/// When the Run Button is clicked, run the selected test.
		/// </summary>
		private void runButton_Click(object sender, System.EventArgs e)
		{
//			if ( testTree.tests.SelectedCategories == null )
//				AppUI.TestLoader.SetFilter( null );
//			else
//				AppUI.TestLoader.SetFilter( new CategoryFilter( testTree.tests.SelectedCategories, testTree.tests.ExcludeSelectedCategories ) );
//
//			TestLoader.RunTests( testTree.tests.SelectedTests );
			testTree.RunTests();
		}

		/// <summary>
		/// When the Stop Button is clicked, cancel running test
		/// </summary>
		private void stopButton_Click(object sender, System.EventArgs e)
		{
			stopButton.Enabled = false;

			if ( IsTestRunning )
			{
				DialogResult dialogResult = UserMessage.Ask( 
					"Do you want to cancel the running test?" );

				if ( dialogResult == DialogResult.No )
					stopButton.Enabled = true;
				else
					TestLoader.CancelTestRun();
			}
		}

		/// <summary>
		/// When a tree item is selected, display info pertaining 
		/// to that test unless a test is running.
		/// </summary>
//		private void OnSelectedTestChanged( UITestNode test )
//		{
//			if ( !IsTestRunning )
//			{
//				suiteName.Text = test.ShortName;
//				statusBar.Initialize( test.CountTestCases() );
//			}
//		}
//
//		private void OnCheckedTestChanged(System.Collections.IList tests)
//		{
//			if ( !IsTestRunning ) 
//			{
//				int count = 0;
//				foreach (UITestNode test in tests) 
//				{
//					count += test.CountTestCases();
//				}
//				statusBar.Initialize(count);
//			}
//		}

		#endregion

		#region Event Handlers for Test Load and Unload

		private void OnTestProjectLoaded( object sender, TestProjectEventArgs e )
		{
			SetTitleBar( e.ProjectName );
			projectMenu.Visible = true;
		}

		private void OnTestProjectUnloaded( object sender, TestProjectEventArgs e )
		{
			SetTitleBar( null );
			projectMenu.Visible = false;
		}

		private void OnTestLoadStarting( object sender, TestEventArgs e )
		{
			runButton.Enabled = false;
		}

		private void OnTestUnloadStarting( object sender, TestEventArgs e )
		{
			runButton.Enabled = false;
		}

		private void OnReloadStarting( object sender, TestEventArgs e )
		{
			runButton.Enabled = false;
		}

		/// <summary>
		/// A test suite has been loaded, so update 
		/// recent assemblies and display the tests in the UI
		/// </summary>
		private void OnTestLoaded( object sender, TestEventArgs e )
		{
			runButton.Enabled = true;
			ClearTabs();
			
			Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
			if ( TestLoader.FrameworkVersion == null )
			{
				UserMessage.Display( "This assembly was not built with the NUnit framework and contains no tests.", "Not a Test Assembly");
			}
			else if ( TestLoader.FrameworkVersion != currentVersion )
			{
				string msg = string.Format( "This assembly is using version {0} of the framework.\rThe {1} assembly is referencing version {2} of the framework.\r\rIf problems arise, rebuild the assembly referencing version {0}",
					currentVersion, e.Name, TestLoader.FrameworkVersion, currentVersion );
				UserMessage.Display( msg, "Incompatible nunit.framework.dll");
			}
		}

		/// <summary>
		/// A test suite has been unloaded, so clear the UI
		/// and remove any references to the suite.
		/// </summary>
		private void OnTestUnloaded( object sender, TestEventArgs e )
		{
			suiteName.Text = null;
			runButton.Enabled = false;

			ClearTabs();
		}

		/// <summary>
		/// The current test suite has changed in some way,
		/// so update the info in the UI and clear the
		/// test results, since they are no longer valid.
		/// </summary>
		private void OnTestChanged( object sender, TestEventArgs e )
		{
			if ( UserSettings.Options.ClearResults )
				ClearTabs();

			runButton.Enabled = true;
		}

		private void OnProjectLoadFailure( object sender, TestProjectEventArgs e )
		{
			UserMessage.DisplayFailure( e.Exception, "Project Not Loaded" );

			UserSettings.RecentProjects.Remove( e.ProjectName );

			runButton.Enabled = true;
		}

		/// <summary>
		/// Event handler for assembly load faiulres. We do this via
		/// an event since some errors may occur asynchronously.
		/// </summary>
		private void OnTestLoadFailure( object sender, TestEventArgs e )
		{
			UserMessage.DisplayFailure( e.Exception, "Assembly Not Loaded" );

			if ( !IsTestLoaded )
				OnTestUnloaded( sender, e );
			else
				runButton.Enabled = true;
		}

		#endregion

		#region Handlers for Test Running Events

		/// <summary>
		/// A test run is starting, so prepare the UI
		/// </summary>
//		private void InvokeRunStarting( object sender, TestEventArgs e )
//		{
//			Invoke( new TestEventHandler( OnRunStarting ), new object[] { e } );
//		}

		private void OnRunStarting( object sender, TestEventArgs e )
		{
			suiteName.Text = e.Test.ShortName;
			runButton.Enabled = false;
			stopButton.Enabled = true;

			ClearTabs();
		}

		/// <summary>
		/// A test run has finished, so display the results
		/// and re-enable the run button.
		/// </summary>
		private void InvokeRunFinished( object sender, TestEventArgs e )
		{
			Invoke( new TestEventHandler( OnRunFinished ), new object[] { e } );
		}

		private void OnRunFinished( object sender, TestEventArgs e )
		{
			stopButton.Enabled = false;
			runButton.Enabled = false;

			if ( e.Exception != null )
			{
				if ( ! ( e.Exception is System.Threading.ThreadAbortException ) )
					UserMessage.DisplayFailure( e.Exception, "NUnit Test Run Failed" );
			}

			runButton.Enabled = true;
		}

		private void OnTestFinished(object sender, TestEventArgs args)
		{
			TestCaseResult result = (TestCaseResult) args.Result;
			if(result.Executed)
			{
				if(result.IsFailure)
				{
					TestResultItem item = new TestResultItem(result);
					string resultString = String.Format("{0}:{1}", result.Name, result.Message);
					detailList.BeginUpdate();
					detailList.Items.Insert(detailList.Items.Count, item);
					detailList.EndUpdate();
				}
			}
			else
			{
				notRunTree.Add( result );
			}
		}

		private void OnSuiteFinished(object sender, TestEventArgs args)
		{
			TestSuiteResult suiteResult = (TestSuiteResult) args.Result;
			if(!suiteResult.Executed)
				notRunTree.Add( suiteResult );
		}

		private void OnTestException(object sender, TestEventArgs args)
		{
			string msg = string.Format(
				"An unhandled exception was detected. Since it was most likely thrown on a separate thread, it may or may not have been caused by the current test.\r\r{0}",
				args.Exception.ToString() );

			UserMessage.DisplayFailure( msg, "Unhandled Exception" );
		}

		#endregion

		#region Helper methods for modifying the UI display

		/// <summary>
		/// Clear info out of our four tabs and stack trace
		/// </summary>
		private void ClearTabs()
		{
			detailList.Items.Clear();
			detailList.ContextMenu = null;
			toolTip.SetToolTip( detailList, null );
			notRunTree.Nodes.Clear();

			stdErrTab.Clear();
			stdOutTab.Clear();
			
			stackTrace.Text = "";
		}

		/// <summary>
		/// Set the title bar based on the loaded file or project
		/// </summary>
		/// <param name="fileName"></param>
		private void SetTitleBar( string fileName )
		{
			this.Text = fileName == null 
				? "NUnit"
				: string.Format( "{0} - NUnit", Path.GetFileName( fileName ) );
		}

		#endregion	

		#region DetailList Events

		// Note: These items should all be moved to a separate user control

		/// <summary>
		/// When one of the detail failure items is selected, display
		/// the stack trace and set up the tool tip for that item.
		/// </summary>
		private void detailList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			TestResultItem resultItem = (TestResultItem)detailList.SelectedItem;
			//string stackTrace = resultItem.StackTrace;
			stackTrace.Text = resultItem.StackTrace;

			//			toolTip.SetToolTip(detailList,resultItem.GetToolTipMessage());
			detailList.ContextMenu = detailListContextMenu;
		}

		private void detailList_MeasureItem(object sender, System.Windows.Forms.MeasureItemEventArgs e)
		{
			TestResultItem item = (TestResultItem) detailList.Items[e.Index];
			string s = item.ToString();
			SizeF size = e.Graphics.MeasureString(item.GetMessage(), detailList.Font);
			e.ItemHeight = (int)size.Height;
			e.ItemWidth = (int)size.Width;
		}

		private void detailList_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
		{
			if (e.Index >= 0) 
			{
				e.DrawBackground();
				TestResultItem item = (TestResultItem) detailList.Items[e.Index];
				bool selected = ((e.State & DrawItemState.Selected) == DrawItemState.Selected) ? true : false;
				Brush brush = selected ? Brushes.White : Brushes.Black;
				e.Graphics.DrawString(item.GetMessage(),detailList.Font, brush, e.Bounds);
				
			}
		}

		private void copyDetailMenuItem_Click(object sender, System.EventArgs e)
		{
			if ( detailList.SelectedItem != null )
				Clipboard.SetDataObject( detailList.SelectedItem.ToString() );
		}

		private void detailList_MouseHover(object sender, System.EventArgs e)
		{
			if ( tipWindow != null ) tipWindow.Close();

			if ( hoverIndex >= 0 && hoverIndex < detailList.Items.Count )
			{
				Rectangle itemRect = detailList.GetItemRectangle( hoverIndex );
				string text = detailList.Items[hoverIndex].ToString();

				tipWindow = new TipWindow( detailList, hoverIndex );
				tipWindow.ItemBounds = itemRect;
				tipWindow.TipText = text;
				tipWindow.Expansion = TipWindow.ExpansionStyle.Both;
				tipWindow.Overlay = true;
				tipWindow.WantClicks = true;
				tipWindow.Closed += new EventHandler( tipWindow_Closed );
				tipWindow.Show();
			}		
		}

		private void tipWindow_Closed( object sender, System.EventArgs e )
		{
			tipWindow = null;
		}

		private void detailList_MouseLeave(object sender, System.EventArgs e)
		{
			hoverIndex = -1;
		}

		private void detailList_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			hoverIndex = detailList.IndexFromPoint( e.X, e.Y );		
		}

		#endregion

		private void testTree_SelectedTestsChanged(object sender, SelectedTestsChangedEventArgs e)
		{
			if (!IsTestRunning) 
			{
				suiteName.Text = e.TestName;
				statusBar.Initialize(e.TestCount);
			}
		}

		private void helpMenuItem_Click(object sender, System.EventArgs e)
		{
			FileInfo exe = new FileInfo( Assembly.GetExecutingAssembly().Location );
			// In normal install, exe is in bin directory, so we get parent
			DirectoryInfo dir = exe.Directory.Parent;
			// If running from bin\Release or bin\Debug, go down two more
			if ( dir.Name == "bin" ) dir = dir.Parent.Parent;

			string helpUrl = ConfigurationSettings.AppSettings["helpUrl"];

			if ( helpUrl == null )
			{
				UriBuilder uri = new UriBuilder();
				uri.Scheme = "file";
				uri.Host = "localhost";
				uri.Path = Path.Combine( dir.FullName, @"doc/index.html" );
				helpUrl = uri.ToString();
			}

			System.Diagnostics.Process.Start( helpUrl );
		}
	}
}

