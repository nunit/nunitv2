#region Copyright (c) 2002, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov
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
' Portions Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov 
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Xml;
using System.IO;
using Microsoft.Win32;

namespace NUnit.Gui
{
	using NUnit.Core;
	using NUnit.Util;
	using NUnit.UiKit;

	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class NUnitForm : System.Windows.Forms.Form
	{
		#region Static and instance variables

		/// <summary>
		/// Object maintaining the list of recently used assemblies
		/// </summary>
		private static RecentProjectSettings recentProjects;

		/// <summary>
		/// Object maintaining the list of recently used assemblies
		/// </summary>
		private static RecentAssemblySettings recentAssemblies;

		/// <summary>
		/// Object representing current option settings
		/// </summary>
		private static OptionSettings optionSettings;

		/// <summary>
		/// Object that coordinates loading and running of tests
		/// </summary>
		private UIActions actions;

		/// <summary>
		/// Structure used for command line options
		/// </summary>
		public struct CommandLineOptions
		{
			public string testFileName;
		}

		/// <summary>
		/// Our current run command line options
		/// </summary>
		private CommandLineOptions command;

		public System.Windows.Forms.Splitter splitter1;
		public System.Windows.Forms.Panel panel1;
		public System.Windows.Forms.TabPage testsNotRun;
		public System.Windows.Forms.MenuItem exitMenuItem;
		public System.Windows.Forms.MenuItem helpMenuItem;
		public System.Windows.Forms.MenuItem aboutMenuItem;
		public System.Windows.Forms.MainMenu mainMenu;
		public System.Windows.Forms.ListBox detailList;
		public System.Windows.Forms.Splitter splitter3;
		public System.Windows.Forms.TextBox stackTrace;
		public NUnit.UiKit.StatusBar statusBar;
		public System.Windows.Forms.OpenFileDialog openFileDialog;
		public NUnit.UiKit.TestSuiteTreeView testSuiteTreeView;
		public System.Windows.Forms.TabControl resultTabs;
		public System.Windows.Forms.TabPage errorPage;
		public System.Windows.Forms.TabPage stderr;
		public System.Windows.Forms.TabPage stdout;
		public System.Windows.Forms.TextBox stdErrTab;
		public System.Windows.Forms.TextBox stdOutTab;
		public System.Windows.Forms.MenuItem recentAssembliesMenu;
		public System.Windows.Forms.TreeView notRunTree;
		private System.ComponentModel.IContainer components;
		public TextWriter stdOutWriter;
		public System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.MenuItem closeMenuItem;
		public System.Windows.Forms.MenuItem fileMenu;
		private System.Windows.Forms.MenuItem fileMenuSeparator1;
		public System.Windows.Forms.MenuItem fileMenuSeparator2;
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
		private System.Windows.Forms.SaveFileDialog saveResultsDialog;
		private System.Windows.Forms.MenuItem saveXmlResultsMenuItem;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.SaveFileDialog saveProjectDialog;
		private System.Windows.Forms.MenuItem recentProjectsMenu;
		private System.Windows.Forms.MenuItem projectMenu;
		private System.Windows.Forms.MenuItem editProjectMenuItem;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem configMenuItem;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.MenuItem saveMenuItem;
		private System.Windows.Forms.MenuItem saveAsMenuItem;
		private System.Windows.Forms.MenuItem newMenuItem;
		private System.Windows.Forms.MenuItem openMenuItem;
		public TextWriter stdErrWriter;

		#endregion
		
		#region Construction and Disposal

		/// <summary>
		/// Static constructor creates our recent assemblies list
		/// </summary>
		static NUnitForm()	
		{
			recentProjects = UserSettings.RecentProjects;
			recentAssemblies = UserSettings.RecentAssemblies;
			optionSettings = UserSettings.Options;
		}
		
		/// <summary>
		/// Construct our form, optionally providing the
		/// full path of to an test file to be loaded.
		/// </summary>
		public NUnitForm( CommandLineOptions command )
		{
			InitializeComponent();

			this.command = command;

			stdErrTab.Enabled = true;
			stdOutTab.Enabled = true;

			SetDefault(runButton);
			runButton.Enabled = false;
			stopButton.Enabled = false;

			stdOutWriter = new TextBoxWriter(stdOutTab);
			Console.SetOut(stdOutWriter);
			stdErrWriter = new TextBoxWriter(stdErrTab);
			Console.SetError(stdErrWriter);

			actions = new UIActions(stdOutWriter, stdErrWriter);
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
			this.menuItem7 = new System.Windows.Forms.MenuItem();
			this.saveMenuItem = new System.Windows.Forms.MenuItem();
			this.saveAsMenuItem = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.reloadMenuItem = new System.Windows.Forms.MenuItem();
			this.fileMenuSeparator1 = new System.Windows.Forms.MenuItem();
			this.recentProjectsMenu = new System.Windows.Forms.MenuItem();
			this.recentAssembliesMenu = new System.Windows.Forms.MenuItem();
			this.fileMenuSeparator2 = new System.Windows.Forms.MenuItem();
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
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.editProjectMenuItem = new System.Windows.Forms.MenuItem();
			this.toolsMenu = new System.Windows.Forms.MenuItem();
			this.saveXmlResultsMenuItem = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
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
			this.stdErrTab = new System.Windows.Forms.TextBox();
			this.stdout = new System.Windows.Forms.TabPage();
			this.stdOutTab = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.stopButton = new System.Windows.Forms.Button();
			this.runButton = new System.Windows.Forms.Button();
			this.suiteName = new System.Windows.Forms.Label();
			this.progressBar = new NUnit.UiKit.ProgressBar();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.saveResultsDialog = new System.Windows.Forms.SaveFileDialog();
			this.saveProjectDialog = new System.Windows.Forms.SaveFileDialog();
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
			this.statusBar.Location = new System.Drawing.Point(0, 511);
			this.statusBar.Name = "statusBar";
			this.statusBar.ShowPanels = true;
			this.statusBar.Size = new System.Drawing.Size(931, 31);
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
																					 this.menuItem7,
																					 this.saveMenuItem,
																					 this.saveAsMenuItem,
																					 this.menuItem5,
																					 this.reloadMenuItem,
																					 this.fileMenuSeparator1,
																					 this.recentProjectsMenu,
																					 this.recentAssembliesMenu,
																					 this.fileMenuSeparator2,
																					 this.exitMenuItem});
			this.fileMenu.Text = "&File";
			this.fileMenu.Popup += new System.EventHandler(this.fileMenu_Popup);
			// 
			// newMenuItem
			// 
			this.newMenuItem.Index = 0;
			this.newMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlN;
			this.newMenuItem.Text = "&New...";
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
			// menuItem7
			// 
			this.menuItem7.Index = 3;
			this.menuItem7.Text = "-";
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
			// menuItem5
			// 
			this.menuItem5.Index = 6;
			this.menuItem5.Text = "-";
			// 
			// reloadMenuItem
			// 
			this.reloadMenuItem.Index = 7;
			this.reloadMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlR;
			this.reloadMenuItem.Text = "&Reload";
			this.reloadMenuItem.Click += new System.EventHandler(this.reloadMenuItem_Click);
			// 
			// fileMenuSeparator1
			// 
			this.fileMenuSeparator1.Index = 8;
			this.fileMenuSeparator1.Text = "-";
			// 
			// recentProjectsMenu
			// 
			this.recentProjectsMenu.Index = 9;
			this.recentProjectsMenu.Text = "Recent &Projects";
			// 
			// recentAssembliesMenu
			// 
			this.recentAssembliesMenu.Index = 10;
			this.recentAssembliesMenu.Text = "Re&cent Assemblies";
			// 
			// fileMenuSeparator2
			// 
			this.fileMenuSeparator2.Index = 11;
			this.fileMenuSeparator2.Text = "-";
			// 
			// exitMenuItem
			// 
			this.exitMenuItem.Index = 12;
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
																						this.menuItem3,
																						this.menuItem6,
																						this.menuItem4,
																						this.menuItem1,
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
			// menuItem3
			// 
			this.menuItem3.Index = 1;
			this.menuItem3.Text = "-";
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 2;
			this.menuItem6.Text = "Add VS Project...";
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 3;
			this.menuItem4.Text = "Add Assembly...";
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 4;
			this.menuItem1.Text = "-";
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
																					  this.menuItem2,
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
			// menuItem2
			// 
			this.menuItem2.Index = 1;
			this.menuItem2.Text = "-";
			// 
			// optionsMenuItem
			// 
			this.optionsMenuItem.Index = 2;
			this.optionsMenuItem.Text = "&Options";
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
			this.testSuiteTreeView.Name = "testSuiteTreeView";
			this.testSuiteTreeView.Size = new System.Drawing.Size(358, 511);
			this.testSuiteTreeView.Sorted = true;
			this.testSuiteTreeView.TabIndex = 1;
			this.testSuiteTreeView.SelectedTestChanged += new NUnit.UiKit.SelectedTestChangedHandler(this.OnSelectedTestChanged);
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(358, 0);
			this.splitter1.MinSize = 240;
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(6, 511);
			this.splitter1.TabIndex = 2;
			this.splitter1.TabStop = false;
			// 
			// panel1
			// 
			this.panel1.Controls.AddRange(new System.Windows.Forms.Control[] {
																				 this.resultTabs,
																				 this.groupBox1});
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(364, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(567, 511);
			this.panel1.TabIndex = 3;
			// 
			// resultTabs
			// 
			this.resultTabs.Controls.AddRange(new System.Windows.Forms.Control[] {
																					 this.errorPage,
																					 this.testsNotRun,
																					 this.stderr,
																					 this.stdout});
			this.resultTabs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.resultTabs.Location = new System.Drawing.Point(0, 88);
			this.resultTabs.Name = "resultTabs";
			this.resultTabs.SelectedIndex = 0;
			this.resultTabs.Size = new System.Drawing.Size(567, 423);
			this.resultTabs.TabIndex = 2;
			// 
			// errorPage
			// 
			this.errorPage.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.stackTrace,
																					this.splitter3,
																					this.detailList});
			this.errorPage.Location = new System.Drawing.Point(4, 25);
			this.errorPage.Name = "errorPage";
			this.errorPage.Size = new System.Drawing.Size(559, 394);
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
			this.stackTrace.Size = new System.Drawing.Size(559, 267);
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
			this.splitter3.Size = new System.Drawing.Size(559, 3);
			this.splitter3.TabIndex = 1;
			this.splitter3.TabStop = false;
			// 
			// detailList
			// 
			this.detailList.Dock = System.Windows.Forms.DockStyle.Top;
			this.detailList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.detailList.HorizontalExtent = 2000;
			this.detailList.HorizontalScrollbar = true;
			this.detailList.ItemHeight = 20;
			this.detailList.Name = "detailList";
			this.detailList.ScrollAlwaysVisible = true;
			this.detailList.Size = new System.Drawing.Size(559, 124);
			this.detailList.TabIndex = 0;
			this.detailList.SelectedIndexChanged += new System.EventHandler(this.detailList_SelectedIndexChanged);
			// 
			// testsNotRun
			// 
			this.testsNotRun.Controls.AddRange(new System.Windows.Forms.Control[] {
																					  this.notRunTree});
			this.testsNotRun.Location = new System.Drawing.Point(4, 25);
			this.testsNotRun.Name = "testsNotRun";
			this.testsNotRun.Size = new System.Drawing.Size(559, 394);
			this.testsNotRun.TabIndex = 1;
			this.testsNotRun.Text = "Tests Not Run";
			// 
			// notRunTree
			// 
			this.notRunTree.Dock = System.Windows.Forms.DockStyle.Fill;
			this.notRunTree.ImageIndex = -1;
			this.notRunTree.Name = "notRunTree";
			this.notRunTree.SelectedImageIndex = -1;
			this.notRunTree.Size = new System.Drawing.Size(559, 394);
			this.notRunTree.TabIndex = 0;
			// 
			// stderr
			// 
			this.stderr.Controls.AddRange(new System.Windows.Forms.Control[] {
																				 this.stdErrTab});
			this.stderr.Location = new System.Drawing.Point(4, 25);
			this.stderr.Name = "stderr";
			this.stderr.Size = new System.Drawing.Size(559, 394);
			this.stderr.TabIndex = 2;
			this.stderr.Text = "Standard Error";
			// 
			// stdErrTab
			// 
			this.stdErrTab.Dock = System.Windows.Forms.DockStyle.Fill;
			this.stdErrTab.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.stdErrTab.Multiline = true;
			this.stdErrTab.Name = "stdErrTab";
			this.stdErrTab.ReadOnly = true;
			this.stdErrTab.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.stdErrTab.Size = new System.Drawing.Size(559, 394);
			this.stdErrTab.TabIndex = 0;
			this.stdErrTab.Text = "";
			this.stdErrTab.WordWrap = false;
			// 
			// stdout
			// 
			this.stdout.Controls.AddRange(new System.Windows.Forms.Control[] {
																				 this.stdOutTab});
			this.stdout.Location = new System.Drawing.Point(4, 25);
			this.stdout.Name = "stdout";
			this.stdout.Size = new System.Drawing.Size(559, 394);
			this.stdout.TabIndex = 3;
			this.stdout.Text = "Standard Out";
			// 
			// stdOutTab
			// 
			this.stdOutTab.Dock = System.Windows.Forms.DockStyle.Fill;
			this.stdOutTab.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.stdOutTab.Multiline = true;
			this.stdOutTab.Name = "stdOutTab";
			this.stdOutTab.ReadOnly = true;
			this.stdOutTab.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.stdOutTab.Size = new System.Drawing.Size(559, 394);
			this.stdOutTab.TabIndex = 0;
			this.stdOutTab.Text = "";
			this.stdOutTab.WordWrap = false;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.stopButton,
																					this.runButton,
																					this.suiteName,
																					this.progressBar});
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(567, 88);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			// 
			// stopButton
			// 
			this.stopButton.Location = new System.Drawing.Point(96, 16);
			this.stopButton.Name = "stopButton";
			this.stopButton.Size = new System.Drawing.Size(75, 30);
			this.stopButton.TabIndex = 4;
			this.stopButton.Text = "&Stop";
			this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
			// 
			// runButton
			// 
			this.runButton.Location = new System.Drawing.Point(8, 16);
			this.runButton.Name = "runButton";
			this.runButton.Size = new System.Drawing.Size(75, 30);
			this.runButton.TabIndex = 3;
			this.runButton.Text = "&Run";
			this.runButton.Click += new System.EventHandler(this.runButton_Click);
			// 
			// suiteName
			// 
			this.suiteName.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.suiteName.Location = new System.Drawing.Point(184, 24);
			this.suiteName.Name = "suiteName";
			this.suiteName.Size = new System.Drawing.Size(359, 24);
			this.suiteName.TabIndex = 2;
			// 
			// progressBar
			// 
			this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.progressBar.CausesValidation = false;
			this.progressBar.Enabled = false;
			this.progressBar.ForeColor = System.Drawing.SystemColors.Highlight;
			this.progressBar.Location = new System.Drawing.Point(8, 56);
			this.progressBar.Maximum = 100;
			this.progressBar.Minimum = 0;
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(551, 24);
			this.progressBar.Step = 1;
			this.progressBar.TabIndex = 0;
			this.progressBar.Value = 0;
			// 
			// saveResultsDialog
			// 
			this.saveResultsDialog.DefaultExt = "xml";
			this.saveResultsDialog.FileName = "TestResult.xml";
			this.saveResultsDialog.Filter = "Xml Files (*.xml)|*.xml|All Files (*.*)|*.*";
			this.saveResultsDialog.Title = "Save Results as XML";
			// 
			// saveProjectDialog
			// 
			this.saveProjectDialog.Filter = "NUnit Test Project (*.nunit)|*.nunit|All Files (*.*)|*.*";
			this.saveProjectDialog.Title = "New Nunit Test Project";
			// 
			// NUnitForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.ClientSize = new System.Drawing.Size(931, 542);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.panel1,
																		  this.splitter1,
																		  this.testSuiteTreeView,
																		  this.statusBar});
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenu;
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

		#region Application Entry Point

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static int Main(string[] args) 
		{
			CommandLineOptions command = new CommandLineOptions();

			GuiOptions parser = new GuiOptions(args);
			if(parser.Validate() && !parser.help) 
			{
				if(!parser.NoArgs)
					command.testFileName = (string)parser.Assembly;

				if(command.testFileName != null)
				{
					FileInfo fileInfo = new FileInfo(command.testFileName);
					if(!fileInfo.Exists)
					{
						string message = String.Format("{0} does not exist", fileInfo.FullName);
						MessageBox.Show(null,message,"Specified test file does not exist",
							MessageBoxButtons.OK,MessageBoxIcon.Stop);
						return 1;
					}
					else
					{
						command.testFileName = fileInfo.FullName;
					}
				}

				NUnitForm form = new NUnitForm( command );
				Application.Run(form);
			}
			else
			{
				string message = parser.GetHelpText();
				MessageBox.Show(null,message,"Help Syntax",
					MessageBoxButtons.OK,MessageBoxIcon.Stop);
				return 2;
			}	
				
			return 0;
		}

		#endregion

		#region Handlers for UI Events

		#region File Menu

		/// <summary>
		/// When File menu is about to display, enable/disable Close
		/// </summary>
		private void fileMenu_Popup(object sender, System.EventArgs e)
		{
			newMenuItem.Enabled = !actions.IsTestRunning;
			openMenuItem.Enabled = !actions.IsTestRunning;
			closeMenuItem.Enabled = actions.IsTestLoaded && !actions.IsTestRunning;

			saveMenuItem.Enabled = actions.IsTestLoaded;
			saveAsMenuItem.Enabled = actions.IsTestLoaded;

			reloadMenuItem.Enabled = actions.IsTestLoaded && !actions.IsTestRunning;

			recentProjectsMenu.Enabled = !actions.IsTestRunning;
			recentAssembliesMenu.Enabled = !actions.IsTestRunning;

			if ( !actions.IsTestRunning )
			{
				LoadRecentFileMenu( recentProjectsMenu, recentProjects.GetFiles() );
				LoadRecentFileMenu( recentAssembliesMenu, recentAssemblies.GetFiles() );
			}
		}

		private void newMenuItem_Click(object sender, System.EventArgs e)
		{
			if ( saveProjectDialog.ShowDialog( this ) == DialogResult.OK )
			{
				UnloadTest();
				NUnitProject project = new NUnitProject();
				project.Configs.Add( "Debug" );
				project.Configs.Add( "Release" );
				project.IsDirty = true;
			}		
		}

		/// <summary>
		/// Open project selected by user
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void openMenuItem_Click(object sender, System.EventArgs e)
		{
			// ToDo: Move to filehandler object
			if ( optionSettings.VisualStudioSupport )
			{
				openFileDialog.Filter =
					"All Project Types (*.nunit,*.csproj,*.vbproj,*.vcproj,*.sln)|*.nunit;*.csproj;*.vbproj;*.vcproj;*.sln|" +
					"Test Projects (*.nunit)|*.nunit|" +
					"Solutions (*.sln)|*.sln|" +
					"C# Projects (*.csproj)|*.csproj|" +
					"VB Projects (*.vbproj)|*.vbproj|" +
					"C++ Projects (*.vcproj)|*.vcproj|" +
					"Assemblies (*.dll,*.exe)|*.dll;*.exe|" +
					"All Files (*.*)|*.*";
			}
			else
			{
				openFileDialog.Filter =
					"TestProjects (*.nunit)|*.nunit|" + 
					"Assemblies (*.dll,*.exe)|*.dll;*.exe|" +
					"All Files (*.*)|*.*";
			}

			openFileDialog.FilterIndex = 1;
			openFileDialog.FileName = "";

			if (openFileDialog.ShowDialog(this) == DialogResult.OK) 
			{
				LoadTest( openFileDialog.FileName );
			}			
		}

		/// <summary>
		/// When File+Close is selected, unload the test
		/// </summary>
		private void closeMenuItem_Click(object sender, System.EventArgs e)
		{
			if ( actions.IsProject && actions.TestProject.IsDirty )
			{
				if ( MessageBox.Show( 
					"Project has been changed. Do you want to save changes?", 
					"NUnit", MessageBoxButtons.YesNo ) == DialogResult.Yes )
				{
					// ToDo: Move to another object
					saveMenuItem_Click( sender, e );
				}
			}

			UnloadTest();
		}

		private void saveMenuItem_Click(object sender, System.EventArgs e)
		{
			// ToDo: Move to filehandler object
			if ( actions.TestProject.ProjectPath == null )
				saveAsMenuItem_Click( sender, e );
			else
				actions.TestProject.Save();
		}

		private void saveAsMenuItem_Click(object sender, System.EventArgs e)
		{
			// TODo: Move to filehandler object
			saveProjectDialog.FileName = actions.TestProject.ProjectPath;
			if ( saveProjectDialog.ShowDialog( this ) == DialogResult.OK )
				actions.TestProject.Save( saveProjectDialog.FileName );
		}

		/// <summary>
		/// Open recently used assembly when it's menu item is selected.
		/// </summary>
		private void recentFile_clicked(object sender, System.EventArgs args) 
		{
			MenuItem item = (MenuItem) sender;
			string assemblyFileName = item.Text.Substring( 2 );

			LoadTest( assemblyFileName );
		}

		/// <summary>
		/// reload the current AppDoamin which will flush the cache and reload it
		/// </summary>
		private void reloadMenuItem_Click(object sender, System.EventArgs e)
		{
			using ( new WaitCursor() )
			{
				actions.ReloadTest();
			}
		}

		/// <summary>
		/// Exit application when menu item is selected.
		/// </summary>
		private void exitMenuItem_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		#endregion

		#region View Menu

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

		#region Project Menu

		/// <summary>
		/// When the project menu pops up, we populate the
		/// submenu for configurations dynamically.
		/// </summary>
		private void projectMenu_Popup(object sender, System.EventArgs e)
		{
			editProjectMenuItem.Enabled = false;

			int index = 0;
			configMenuItem.MenuItems.Clear();

			foreach ( ProjectConfig config in actions.TestProject.Configs )
			{
				string text = string.Format( "&{0} {1}", index+1, config.Name );
				MenuItem item = new MenuItem( 
					text, new EventHandler( configMenuItem_Click ) );
				if ( config.Name == actions.ActiveConfig ) 
					item.Checked = true;
				configMenuItem.MenuItems.Add( index++, item );
			}

			configMenuItem.MenuItems.Add( "-" );

			configMenuItem.MenuItems.Add( "&Add...",
				new System.EventHandler( addConfigurationMenuItem_Click ) );

			configMenuItem.MenuItems.Add( "&Edit...", 
				new System.EventHandler( editConfigurationsMenuItem_Click ) );
		}

		private void configMenuItem_Click( object sender, EventArgs e )
		{
			MenuItem item = (MenuItem)sender;
			
			if ( !item.Checked )
			{
				NUnitProject project = actions.TestProject;
				ProjectConfig config = project.Configs[item.Index];
				if ( config.Assemblies.Count == 0 )
					MessageBox.Show( "Selected Config cannot be loaded. It contains no assemblies." );
				else
					actions.ActiveConfig = config.Name;
			}
		}

		private void addConfigurationMenuItem_Click( object sender, System.EventArgs e )
		{
			AddConfigurationDialog dlg = new AddConfigurationDialog( actions.TestProject );
			if ( dlg.ShowDialog() == DialogResult.OK )
			{
				// ToDo: Move more of this to project
				ProjectConfig newConfig = new ProjectConfig( dlg.ConfigurationName );
				
				if ( dlg.CopyConfigurationName != null )
				{
					ProjectConfig copyConfig = actions.TestProject.Configs[dlg.CopyConfigurationName];
					if ( copyConfig != null )
						foreach( string path in copyConfig.Assemblies )
							  newConfig.Assemblies.Add( path );
				}

				actions.TestProject.Configs.Add( newConfig );
				actions.TestProject.IsDirty = true;
			}
		}

		private void editConfigurationsMenuItem_Click( object sender, System.EventArgs e )
		{
			ConfigurationEditor.Edit( actions.TestProject );
		}

		private void editProjectMenuItem_Click(object sender, System.EventArgs e)
		{
			ProjectEditor.Edit( actions.TestProject );
		}

		#endregion

		#region Tools Menu

		private void toolsMenu_Popup(object sender, System.EventArgs e)
		{		
			saveXmlResultsMenuItem.Enabled = actions.IsTestLoaded && actions.LastResult != null;
		}

		private void saveXmlResultsMenuItem_Click(object sender, System.EventArgs e)
		{
			SaveXmlResults( actions.LastResult );
		}

		private void optionsMenuItem_Click(object sender, System.EventArgs e)
		{
			ShowOptionsDialog();
		}

		#endregion

		#region Help Menu

		/// <summary>
		/// Display the about box when menu item is selected
		/// </summary>
		private void aboutMenuItem_Click(object sender, System.EventArgs e)
		{
			AboutBox aboutBox = new AboutBox();
			aboutBox.Show();
		}

		#endregion

		/// <summary>
		/// When the Run Button is clicked, run the selected test.
		/// </summary>
		private void runButton_Click(object sender, System.EventArgs e)
		{
			runButton.Enabled = false;
			if ( actions.IsReloadPending || optionSettings.ReloadOnRun )
				actions.ReloadTest();

			actions.RunTestSuite( testSuiteTreeView.SelectedTest );
		}

		/// <summary>
		/// When the Stop Button is clicked, cancel running test
		/// </summary>
		private void stopButton_Click(object sender, System.EventArgs e)
		{
			stopButton.Enabled = false;

			if ( actions.IsTestRunning )
			{
				DialogResult dialogResult = MessageBox.Show( 
					"Do you want to cancel the running test?", "NUnit", MessageBoxButtons.YesNo );

				if ( dialogResult == DialogResult.No )
					stopButton.Enabled = true;
				else
					actions.CancelTestRun();
			}
		}

		/// <summary>
		/// When a tree item is selected, display info pertaining 
		/// to that test unless a test is running.
		/// </summary>
		private void OnSelectedTestChanged( UITestNode test )
		{
			if ( !actions.IsTestRunning )
			{
				suiteName.Text = test.ShortName;
				statusBar.Initialize( test.CountTestCases );
			}
		}

		/// <summary>
		/// When one of the detail failure items is selected, display
		/// the stack trace and set up the tool tip for that item.
		/// </summary>
		private void detailList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			TestResultItem resultItem = (TestResultItem)detailList.SelectedItem;
			//string stackTrace = resultItem.StackTrace;
			stackTrace.Text = resultItem.StackTrace;
			toolTip.SetToolTip(detailList,resultItem.GetMessage());
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
			if ( actions.IsTestRunning )
			{
				DialogResult dialogResult = MessageBox.Show( 
					"A test is running, do you want to stop the test and exit?", "NUnit", MessageBoxButtons.YesNo );

				if ( dialogResult == DialogResult.No )
				{
					e.Cancel = true;
					return;
				}
				
				actions.CancelTestRun();
			}

			UnloadTest();

			if ( this.WindowState == FormWindowState.Normal )
			{
				UserSettings.Form.Location = this.Location;
				UserSettings.Form.Size = this.Size;
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

			// Set up events handled by the form
			actions.RunStartingEvent += new TestEventHandler( OnRunStarting );
			actions.RunFinishedEvent += new TestEventHandler( OnRunFinished );

			actions.LoadCompleteEvent	+= new TestLoadEventHandler( OnTestLoaded );
			actions.UnloadCompleteEvent += new TestLoadEventHandler( OnTestUnloaded );
			actions.ReloadCompleteEvent += new TestLoadEventHandler( OnTestChanged );
			actions.LoadFailedEvent		+= new TestLoadEventHandler( OnTestLoadFailure );
			actions.ReloadFailedEvent	+= new TestLoadEventHandler( OnTestLoadFailure );

			// Set tree options
			testSuiteTreeView.InitialDisplay =
				(TestSuiteTreeView.DisplayStyle) optionSettings.InitialTreeDisplay;

			// Allow controls to initialize as well
			// ToDo: Migrate more ui elements to handle events on their own.
			this.testSuiteTreeView.InitializeEvents( actions );
			this.progressBar.InitializeEvents( actions );
			this.statusBar.InitializeEvents( actions );
			// Load test specified on command line or
			// the most recent one if options call for it
			if ( command.testFileName != null )
				LoadTest( command.testFileName );
			else if( optionSettings.LoadLastProject &&
					 recentProjects.RecentFile != null )
				LoadTest( recentProjects.RecentFile );
		}

		#endregion

		#region Handlers for events related to loading and running tests

		/// <summary>
		/// A test run is starting, so prepare the UI
		/// </summary>
		/// <param name="test">Top level Test for this run</param>
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
		/// <param name="result">Result of the run</param>
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
					FailureMessage( e.Exception, "NUnit Test Run Failed" );
			}

			runButton.Enabled = true;
		}

		/// <summary>
		/// A test suite has been loaded, so update 
		/// recent assemblies and display the tests in the UI
		/// </summary>
		/// <param name="test">Top level test for the assembly</param>
		/// <param name="assemblyFileName">The full path of the assembly file</param>
		/// Note: second argument could be omitted, but tests may not
		/// always have the FullName set to the assembly name in future.
		private void OnTestLoaded( object sender, TestLoadEventArgs e )
		{
			if ( e.IsProjectFile )
			{
				recentProjects.RecentFile = e.TestFileName;
				projectMenu.Visible = true;
			}
			else
			{
				recentAssemblies.RecentFile = e.TestFileName;
				projectMenu.Visible = false;
			}

			SetTitleBar( e.TestFileName );
			viewMenu.Visible = true;
			
			runButton.Enabled = true;
			ClearTabs();
		}

		/// <summary>
		/// A test suite has been unloaded, so clear the UI
		/// and remove any references to the suite.
		/// </summary>
		private void OnTestUnloaded( object sender, TestLoadEventArgs e )
		{
			suiteName.Text = null;
			runButton.Enabled = false;

			ClearTabs();
			SetTitleBar( null );
			projectMenu.Visible = false;
			viewMenu.Visible = false;
		}

		/// <summary>
		/// The current test suite has changed in some way,
		/// so update the info in the UI and clear the
		/// test results, since they are no longer valid.
		/// </summary>
		private void OnTestChanged( object sender, TestLoadEventArgs e )
		{
			if ( UserSettings.Options.ClearResults )
				ClearTabs();
		}

		/// <summary>
		/// Event handler for assembly load faiulres. We do this via
		/// an event since some errors may occur asynchronously.
		/// </summary>
		private void OnTestLoadFailure( object sender, TestLoadEventArgs e )
		{
			FailureMessage( e.Exception, "Assembly Load Failure" );

			if ( e.IsProjectFile )
			{
				recentProjects.Remove( e.TestFileName );
			}
			else
			{
				recentAssemblies.Remove( e.TestFileName );
			}

			if ( !actions.IsTestLoaded )
				OnTestUnloaded( sender, e );
			else
				runButton.Enabled = true;
		}

		#endregion

		#region Helper methods for loading and running tests

		/// <summary>
		/// Display failure message box
		/// </summary>
		/// <param name="exception">Exception that caused the failure</param>
		/// <param name="caption">Caption for the message box</param>
		private void FailureMessage( Exception exception, string caption )
		{
			string message = exception.Message;
			if(exception.InnerException != null)
				message = exception.InnerException.Message;

			FailureMessage( message, caption );
		}

		/// <summary>
		/// Display failure message box
		/// </summary>
		/// <param name="message">Message to display</param>
		/// <param name="caption">Caption for the message box</param>
		private void FailureMessage( string message, string caption )
		{
			MessageBox.Show( this, message, caption,
				MessageBoxButtons.OK,MessageBoxIcon.Stop);
		}

		/// <summary>
		/// Load an assembly into the UI
		/// </summary>
		/// <param name="assemblyFileName">Full path of the assembly to load</param>
		private void LoadTest(string testFileName)
		{
			runButton.Enabled = false;
			actions.LoadTest( testFileName );
		}

		/// <summary>
		/// Unload the current assembly
		/// </summary>
		private void UnloadTest()
		{
			runButton.Enabled = false;
			actions.UnloadTest();
		}

		#endregion

		#region Helper methods for modifying the UI display

		/// <summary>
		/// Set a button as the default for the form.
		/// </summary>
		/// <param name="myDefaultBtn">The button to be set as the default</param>
		private void SetDefault(Button myDefaultBtn)
		{
			this.AcceptButton = myDefaultBtn;
		}

		/// <summary>
		/// Clear info out of our four tabs and stack trace
		/// </summary>
		private void ClearTabs()
		{
			detailList.Items.Clear();
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
				: string.Format( "{0} - NUnit", Path.GetFileNameWithoutExtension( fileName ) );
		}
	
		/// <summary>
		/// Display test results in the UI
		/// </summary>
		/// <param name="results">Test results to be displayed</param>
		private void DisplayResults(TestResult results)
		{
			DetailResults detailResults = new DetailResults(detailList, notRunTree);
			detailResults.DisplayResults( results );
		}

		/// <summary>
		/// Load a menu with recently used files.
		/// </summary>
		private void LoadRecentFileMenu( MenuItem menu, IList files ) 
		{
			if ( files.Count == 0 )
				menu.Enabled = false;
			else 
			{
				menu.Enabled = true;
				menu.MenuItems.Clear();
				int index = 1;
				foreach ( string name in files ) 
				{
					MenuItem item = new MenuItem(String.Format("{0} {1}", index++, name));
					item.Click += new System.EventHandler(recentFile_clicked);
					menu.MenuItems.Add( item );
				}		
			}
		}

		private void SaveXmlResults(TestResult result)
		{
			if ( saveResultsDialog.ShowDialog( this ) == DialogResult.OK )
			{
				try
				{
					string fileName = saveResultsDialog.FileName;

					XmlResultVisitor resultVisitor 
						= new XmlResultVisitor( fileName, result);
					result.Accept(resultVisitor);
					resultVisitor.Write();

					string msg = String.Format( "Results saved as {0}", fileName );
					MessageBox.Show( msg, "Save Results as XML" );
				}
				catch( Exception exception )
				{
					FailureMessage( exception, "Unable to Save Results" );
				}
			}
		}

		private void ShowOptionsDialog( )
		{
			OptionsDialog dialog = new OptionsDialog( optionSettings, actions );
			if ( dialog.ShowDialog() == DialogResult.OK )
				testSuiteTreeView.InitialDisplay =
					(TestSuiteTreeView.DisplayStyle) optionSettings.InitialTreeDisplay;
		}

		#endregion	
	}
}

