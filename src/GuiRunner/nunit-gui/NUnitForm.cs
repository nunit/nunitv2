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
using System.Windows.Forms;
using System.IO;

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
			public bool noload;
		}

		// Our current run command line options
		private CommandLineOptions commandLineOptions;

		// TipWindow for the detail list
		CP.Windows.Forms.TipWindow tipWindow;
		int hoverIndex = -1;
		System.Windows.Forms.Timer hoverTimer;

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
		public NUnit.UiKit.TestSuiteTreeView testSuiteTreeView;
		public System.Windows.Forms.TabControl resultTabs;
		public System.Windows.Forms.TabPage errorPage;
		public System.Windows.Forms.TabPage stderr;
		public System.Windows.Forms.TabPage stdout;
		public System.Windows.Forms.RichTextBox stdErrTab;
		public System.Windows.Forms.RichTextBox stdOutTab;
		public System.Windows.Forms.TreeView notRunTree;
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
		private System.Windows.Forms.MenuItem viewMenu;
		private System.Windows.Forms.MenuItem toolsMenu;
		private System.Windows.Forms.MenuItem optionsMenuItem;
		private System.Windows.Forms.MenuItem expandMenuItem;
		private System.Windows.Forms.MenuItem collapseMenuItem;
		private System.Windows.Forms.MenuItem viewMenuSeparatorItem1;
		private System.Windows.Forms.MenuItem expandAllMenuItem;
		private System.Windows.Forms.MenuItem collapseAllMenuItem;
		private System.Windows.Forms.MenuItem viewMenuSeparatorItem2;
		private System.Windows.Forms.MenuItem expandFixturesMenuItem;
		private System.Windows.Forms.MenuItem collapseFixturesMenuItem;
		private System.Windows.Forms.MenuItem viewMenuSeparatorItem3;
		private System.Windows.Forms.MenuItem propertiesMenuItem;
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
		private System.Windows.Forms.MenuItem addAssemblyMenuItem;

		#endregion
		
		#region Construction and Disposal

		public NUnitForm( CommandLineOptions commandLineOptions )
		{
			InitializeComponent();

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
			this.viewMenu = new System.Windows.Forms.MenuItem();
			this.expandMenuItem = new System.Windows.Forms.MenuItem();
			this.collapseMenuItem = new System.Windows.Forms.MenuItem();
			this.viewMenuSeparatorItem1 = new System.Windows.Forms.MenuItem();
			this.expandAllMenuItem = new System.Windows.Forms.MenuItem();
			this.collapseAllMenuItem = new System.Windows.Forms.MenuItem();
			this.viewMenuSeparatorItem2 = new System.Windows.Forms.MenuItem();
			this.expandFixturesMenuItem = new System.Windows.Forms.MenuItem();
			this.collapseFixturesMenuItem = new System.Windows.Forms.MenuItem();
			this.viewMenuSeparatorItem3 = new System.Windows.Forms.MenuItem();
			this.propertiesMenuItem = new System.Windows.Forms.MenuItem();
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
			this.testSuiteTreeView = new NUnit.UiKit.TestSuiteTreeView();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.panel1 = new System.Windows.Forms.Panel();
			this.resultTabs = new System.Windows.Forms.TabControl();
			this.errorPage = new System.Windows.Forms.TabPage();
			this.stackTrace = new System.Windows.Forms.TextBox();
			this.splitter3 = new System.Windows.Forms.Splitter();
			this.detailList = new System.Windows.Forms.ListBox();
			this.testsNotRun = new System.Windows.Forms.TabPage();
			this.notRunTree = new System.Windows.Forms.TreeView();
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
			this.panel1.SuspendLayout();
			this.resultTabs.SuspendLayout();
			this.errorPage.SuspendLayout();
			this.testsNotRun.SuspendLayout();
			this.stderr.SuspendLayout();
			this.stdout.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// statusBar
			// 
			this.statusBar.DisplayTestProgress = true;
			this.statusBar.Location = new System.Drawing.Point(0, 417);
			this.statusBar.Name = "statusBar";
			this.statusBar.ShowPanels = true;
			this.statusBar.Size = new System.Drawing.Size(748, 37);
			this.statusBar.TabIndex = 0;
			this.statusBar.Text = "Status";
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.fileMenu,
																					 this.viewMenu,
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
			// viewMenu
			// 
			this.viewMenu.Index = 1;
			this.viewMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.expandMenuItem,
																					 this.collapseMenuItem,
																					 this.viewMenuSeparatorItem1,
																					 this.expandAllMenuItem,
																					 this.collapseAllMenuItem,
																					 this.viewMenuSeparatorItem2,
																					 this.expandFixturesMenuItem,
																					 this.collapseFixturesMenuItem,
																					 this.viewMenuSeparatorItem3,
																					 this.propertiesMenuItem});
			this.viewMenu.Text = "&View";
			this.viewMenu.Visible = false;
			this.viewMenu.Popup += new System.EventHandler(this.viewMenu_Popup);
			// 
			// expandMenuItem
			// 
			this.expandMenuItem.Index = 0;
			this.expandMenuItem.Text = "&Expand";
			this.expandMenuItem.Click += new System.EventHandler(this.expandMenuItem_Click);
			// 
			// collapseMenuItem
			// 
			this.collapseMenuItem.Index = 1;
			this.collapseMenuItem.Text = "&Collapse";
			this.collapseMenuItem.Click += new System.EventHandler(this.collapseMenuItem_Click);
			// 
			// viewMenuSeparatorItem1
			// 
			this.viewMenuSeparatorItem1.Index = 2;
			this.viewMenuSeparatorItem1.Text = "-";
			// 
			// expandAllMenuItem
			// 
			this.expandAllMenuItem.Index = 3;
			this.expandAllMenuItem.Text = "Expand All";
			this.expandAllMenuItem.Click += new System.EventHandler(this.expandAllMenuItem_Click);
			// 
			// collapseAllMenuItem
			// 
			this.collapseAllMenuItem.Index = 4;
			this.collapseAllMenuItem.Text = "Collapse All";
			this.collapseAllMenuItem.Click += new System.EventHandler(this.collapseAllMenuItem_Click);
			// 
			// viewMenuSeparatorItem2
			// 
			this.viewMenuSeparatorItem2.Index = 5;
			this.viewMenuSeparatorItem2.Text = "-";
			// 
			// expandFixturesMenuItem
			// 
			this.expandFixturesMenuItem.Index = 6;
			this.expandFixturesMenuItem.Text = "Expand Fixtures";
			this.expandFixturesMenuItem.Click += new System.EventHandler(this.expandFixturesMenuItem_Click);
			// 
			// collapseFixturesMenuItem
			// 
			this.collapseFixturesMenuItem.Index = 7;
			this.collapseFixturesMenuItem.Text = "Collapse Fixtures";
			this.collapseFixturesMenuItem.Click += new System.EventHandler(this.collapseFixturesMenuItem_Click);
			// 
			// viewMenuSeparatorItem3
			// 
			this.viewMenuSeparatorItem3.Index = 8;
			this.viewMenuSeparatorItem3.Text = "-";
			// 
			// propertiesMenuItem
			// 
			this.propertiesMenuItem.Index = 9;
			this.propertiesMenuItem.Text = "&Properties";
			this.propertiesMenuItem.Click += new System.EventHandler(this.propertiesMenuItem_Click);
			// 
			// projectMenu
			// 
			this.projectMenu.Index = 2;
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
			this.toolsMenu.Index = 3;
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
			this.helpItem.Index = 4;
			this.helpItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.helpMenuItem,
																					 this.helpMenuSeparator1,
																					 this.aboutMenuItem});
			this.helpItem.Text = "&Help";
			// 
			// helpMenuItem
			// 
			this.helpMenuItem.Enabled = false;
			this.helpMenuItem.Index = 0;
			this.helpMenuItem.Text = "Help";
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
			// testSuiteTreeView
			// 
			this.testSuiteTreeView.AllowDrop = true;
			this.testSuiteTreeView.Dock = System.Windows.Forms.DockStyle.Left;
			this.testSuiteTreeView.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.testSuiteTreeView.HideSelection = false;
			this.testSuiteTreeView.Location = new System.Drawing.Point(0, 0);
			this.testSuiteTreeView.Name = "testSuiteTreeView";
			this.testSuiteTreeView.Size = new System.Drawing.Size(224, 417);
			this.testSuiteTreeView.TabIndex = 1;
			this.testSuiteTreeView.SelectedTestChanged += new NUnit.UiKit.SelectedTestChangedHandler(this.OnSelectedTestChanged);
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(224, 0);
			this.splitter1.MinSize = 240;
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(6, 417);
			this.splitter1.TabIndex = 2;
			this.splitter1.TabStop = false;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.resultTabs);
			this.panel1.Controls.Add(this.groupBox1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(230, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(518, 417);
			this.panel1.TabIndex = 3;
			// 
			// resultTabs
			// 
			this.resultTabs.Controls.Add(this.errorPage);
			this.resultTabs.Controls.Add(this.testsNotRun);
			this.resultTabs.Controls.Add(this.stderr);
			this.resultTabs.Controls.Add(this.stdout);
			this.resultTabs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.resultTabs.Location = new System.Drawing.Point(0, 102);
			this.resultTabs.Name = "resultTabs";
			this.resultTabs.SelectedIndex = 0;
			this.resultTabs.Size = new System.Drawing.Size(518, 315);
			this.resultTabs.TabIndex = 2;
			// 
			// errorPage
			// 
			this.errorPage.Controls.Add(this.stackTrace);
			this.errorPage.Controls.Add(this.splitter3);
			this.errorPage.Controls.Add(this.detailList);
			this.errorPage.Location = new System.Drawing.Point(4, 25);
			this.errorPage.Name = "errorPage";
			this.errorPage.Size = new System.Drawing.Size(510, 286);
			this.errorPage.TabIndex = 0;
			this.errorPage.Text = "Errors and Failures";
			// 
			// stackTrace
			// 
			this.stackTrace.Dock = System.Windows.Forms.DockStyle.Fill;
			this.stackTrace.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.stackTrace.Location = new System.Drawing.Point(0, 147);
			this.stackTrace.Multiline = true;
			this.stackTrace.Name = "stackTrace";
			this.stackTrace.ReadOnly = true;
			this.stackTrace.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.stackTrace.Size = new System.Drawing.Size(510, 139);
			this.stackTrace.TabIndex = 2;
			this.stackTrace.Text = "";
			this.stackTrace.WordWrap = false;
			// 
			// splitter3
			// 
			this.splitter3.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter3.Location = new System.Drawing.Point(0, 143);
			this.splitter3.MinSize = 100;
			this.splitter3.Name = "splitter3";
			this.splitter3.Size = new System.Drawing.Size(510, 4);
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
			this.detailList.Size = new System.Drawing.Size(510, 143);
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
			this.testsNotRun.Location = new System.Drawing.Point(4, 25);
			this.testsNotRun.Name = "testsNotRun";
			this.testsNotRun.Size = new System.Drawing.Size(510, 286);
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
			this.notRunTree.Size = new System.Drawing.Size(510, 286);
			this.notRunTree.TabIndex = 0;
			// 
			// stderr
			// 
			this.stderr.Controls.Add(this.stdErrTab);
			this.stderr.Location = new System.Drawing.Point(4, 25);
			this.stderr.Name = "stderr";
			this.stderr.Size = new System.Drawing.Size(510, 286);
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
			this.stdErrTab.Size = new System.Drawing.Size(510, 286);
			this.stdErrTab.TabIndex = 0;
			this.stdErrTab.Text = "";
			this.stdErrTab.WordWrap = false;
			// 
			// stdout
			// 
			this.stdout.Controls.Add(this.stdOutTab);
			this.stdout.Location = new System.Drawing.Point(4, 25);
			this.stdout.Name = "stdout";
			this.stdout.Size = new System.Drawing.Size(510, 286);
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
			this.stdOutTab.Size = new System.Drawing.Size(510, 286);
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
			this.groupBox1.Size = new System.Drawing.Size(518, 102);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			// 
			// stopButton
			// 
			this.stopButton.Location = new System.Drawing.Point(113, 18);
			this.stopButton.Name = "stopButton";
			this.stopButton.Size = new System.Drawing.Size(87, 36);
			this.stopButton.TabIndex = 4;
			this.stopButton.Text = "&Stop";
			this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
			// 
			// runButton
			// 
			this.runButton.Location = new System.Drawing.Point(8, 18);
			this.runButton.Name = "runButton";
			this.runButton.Size = new System.Drawing.Size(89, 36);
			this.runButton.TabIndex = 3;
			this.runButton.Text = "&Run";
			this.runButton.Click += new System.EventHandler(this.runButton_Click);
			// 
			// suiteName
			// 
			this.suiteName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.suiteName.Location = new System.Drawing.Point(215, 28);
			this.suiteName.Name = "suiteName";
			this.suiteName.Size = new System.Drawing.Size(294, 29);
			this.suiteName.TabIndex = 2;
			// 
			// progressBar
			// 
			this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.progressBar.CausesValidation = false;
			this.progressBar.Enabled = false;
			this.progressBar.ForeColor = System.Drawing.SystemColors.Highlight;
			this.progressBar.Location = new System.Drawing.Point(8, 65);
			this.progressBar.Maximum = 100;
			this.progressBar.Minimum = 0;
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(501, 28);
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
			// NUnitForm
			// 
			this.AcceptButton = this.runButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.ClientSize = new System.Drawing.Size(748, 454);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.testSuiteTreeView);
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

		#region View Menu Handlers

		private void viewMenu_Popup(object sender, System.EventArgs e)
		{
			TreeNode selectedNode = testSuiteTreeView.SelectedNode;
			if ( selectedNode != null && selectedNode.Nodes.Count > 0 )
			{
				bool isExpanded = testSuiteTreeView.SelectedNode.IsExpanded;
				collapseMenuItem.Enabled = isExpanded;
				expandMenuItem.Enabled = !isExpanded;		
			}
			else
			{
				collapseMenuItem.Enabled = expandMenuItem.Enabled = false;
			}
		}

		private void collapseMenuItem_Click(object sender, System.EventArgs e)
		{
			testSuiteTreeView.SelectedNode.Collapse();
		}

		private void expandMenuItem_Click(object sender, System.EventArgs e)
		{
			testSuiteTreeView.SelectedNode.Expand();
		}

		private void collapseAllMenuItem_Click(object sender, System.EventArgs e)
		{
			testSuiteTreeView.CollapseAll();
		}

		private void expandAllMenuItem_Click(object sender, System.EventArgs e)
		{
			testSuiteTreeView.ExpandAll();
		}

		private void collapseFixturesMenuItem_Click(object sender, System.EventArgs e)
		{
			testSuiteTreeView.CollapseFixtures();		
		}

		private void expandFixturesMenuItem_Click(object sender, System.EventArgs e)
		{
			testSuiteTreeView.ExpandFixtures();		
		}

		private void propertiesMenuItem_Click(object sender, System.EventArgs e)
		{
			testSuiteTreeView.ShowPropertiesDialog( testSuiteTreeView.SelectedTest );
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
			saveXmlResultsMenuItem.Enabled = IsTestLoaded && TestLoader.LastResult != null;
			exceptionDetailsMenuItem.Enabled = TestLoader.LastException != null;
		}

		private void saveXmlResultsMenuItem_Click(object sender, System.EventArgs e)
		{
			TestLoaderUI.SaveLastResult();
		}

		private void exceptionDetailsMenuItem_Click(object sender, System.EventArgs e)
		{
			UserMessage.DisplayFailure( TestLoader.LastException.ToString(), "Exception Details" );
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

		#region Other UI Event Handlers

		/// <summary>
		/// When the Run Button is clicked, run the selected test.
		/// </summary>
		private void runButton_Click(object sender, System.EventArgs e)
		{
			TestLoader.RunTestSuite( testSuiteTreeView.SelectedTest );
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
		private void OnSelectedTestChanged( UITestNode test )
		{
			if ( !IsTestRunning )
			{
				suiteName.Text = test.ShortName;
				statusBar.Initialize( test.CountTestCases );
			}
		}

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
				{
					e.Cancel = true;
					return;
				}
				
				TestLoader.CancelTestRun();
			}

			if ( IsProjectLoaded )
				TestLoaderUI.CloseProject();

			if ( this.WindowState == FormWindowState.Normal )
			{
				UserSettings.Form.Location = this.Location;
				UserSettings.Form.Size = this.Size;
				UserSettings.Form.TreeSplitterPosition = this.splitter1.SplitPosition;
				UserSettings.Form.TabSplitterPosition = this.splitter3.SplitPosition;
			}
		}

		/// <summary>
		/// Get saved options when form loads
		/// </summary>
		private void NUnitForm_Load(object sender, System.EventArgs e)
		{
			// Set position of the form
			this.Location = UserSettings.Form.Location;
			this.Size = UserSettings.Form.Size;

			// Set the splitter positions
			this.splitter1.SplitPosition = UserSettings.Form.TreeSplitterPosition;
			this.splitter3.SplitPosition = UserSettings.Form.TabSplitterPosition;
		
			// Set up events handled by the form
			ITestEvents events = TestLoader.Events;

			events.RunStarting += new TestEventHandler( OnRunStarting );
			events.RunFinished += new TestEventHandler( OnRunFinished );

			events.ProjectLoaded	+= new TestEventHandler( OnTestProjectLoaded );
			events.ProjectLoadFailed+= new TestEventHandler( OnProjectLoadFailure );
			events.ProjectUnloaded	+= new TestEventHandler( OnTestProjectUnloaded );

			events.TestLoading		+= new TestEventHandler( OnTestLoadStarting );
			events.TestLoaded		+= new TestEventHandler( OnTestLoaded );
			events.TestLoadFailed	+= new TestEventHandler( OnTestLoadFailure );
			events.TestUnloading	+= new TestEventHandler( OnTestUnloadStarting );
			events.TestUnloaded		+= new TestEventHandler( OnTestUnloaded );
			events.TestReloading	+= new TestEventHandler( OnReloadStarting );
			events.TestReloaded		+= new TestEventHandler( OnTestChanged );
			events.TestReloadFailed	+= new TestEventHandler( OnTestLoadFailure );

			// Allow controls to initialize as well
			// ToDo: Migrate more ui elements to handle events on their own.
			this.testSuiteTreeView.Initialize( TestLoader, TestLoader.Events );
			this.progressBar.Initialize( TestLoader.Events );
			this.statusBar.Initialize( TestLoader.Events );

			// Set controls to match option settings. We do this
			// here rather than in the controls since there may
			// be more than one app that uses the same controls.
			testSuiteTreeView.ClearResultsOnChange = 
				UserSettings.Options.ClearResults;
			
			// Load test specified on command line or
			// the most recent one if options call for it
			if ( commandLineOptions.testFileName != null )
				TestLoaderUI.OpenProject( commandLineOptions.testFileName, commandLineOptions.configName );
			else if( UserSettings.Options.LoadLastProject && !commandLineOptions.noload )
			{
				string recentProjectName = UserSettings.RecentProjects.RecentFile;
				if ( recentProjectName != null )
					TestLoaderUI.OpenProject( recentProjectName );
			}
		}

		#endregion

		#region Event Handlers for Test Load and Unload

		private void OnTestProjectLoaded( object sender, TestEventArgs e )
		{
			SetTitleBar( e.TestFileName );
			projectMenu.Visible = true;
		}

		private void OnTestProjectUnloaded( object sender, TestEventArgs e )
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
			viewMenu.Visible = true;		
			runButton.Enabled = true;
			ClearTabs();
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
			viewMenu.Visible = false;
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

		private void OnProjectLoadFailure( object sender, TestEventArgs e )
		{
			UserMessage.DisplayFailure( e.Exception, "Project Not Loaded" );

			UserSettings.RecentProjects.Remove( e.TestFileName );

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

			if ( e.Result != null )
			{
				DisplayResults(e.Result);
			}

			if ( e.Exception != null )
			{
				if ( ! ( e.Exception is System.Threading.ThreadAbortException ) )
					UserMessage.DisplayFailure( e.Exception, "NUnit Test Run Failed" );
			}

			runButton.Enabled = true;
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
	
		private delegate void DisplayResultsHandler( TestResult results );
 
 		// TODO: Temporary fix - find better way to ensure proper thread is always used
		private void DisplayResults( TestResult results )
		{
 			Invoke( new DisplayResultsHandler( DoDisplayResults ), new object[] { results } );
 		}
 
		/// <summary>
		/// Display test results in the UI
		/// </summary>
		/// <param name="results">Test results to be displayed</param>
		private void DoDisplayResults(TestResult results)
		{
			DetailResults detailResults = new DetailResults(detailList, notRunTree);
			detailResults.DisplayResults( results );
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
//			ClearTimer();
		}

		private void detailList_MouseLeave(object sender, System.EventArgs e)
		{
//			ClearTimer();
		}

		private void detailList_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
//			ClearTimer();

			hoverIndex = detailList.IndexFromPoint( e.X, e.Y );
			
//			hoverTimer = new System.Windows.Forms.Timer();
//			hoverTimer.Interval = mouseHoverDelay;
//			hoverTimer.Tick += new EventHandler( OnMouseHover );
//			hoverTimer.Start();
		}

		private void ClearTimer()
		{
			if ( hoverTimer != null )
			{
				hoverTimer.Stop();
				hoverTimer.Dispose();
			}
		}

		#endregion

	}
}

