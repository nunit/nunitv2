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

		// Our TextBoxWriters for stdErr and stdOut
		TextBoxWriter outWriter;
		TextBoxWriter errWriter;

		// Our current run command line options
		private CommandLineOptions commandLineOptions;

		// TipWindow for the detail list
		CP.Windows.Forms.TipWindow tipWindow;
		int hoverIndex = -1;
		private System.Windows.Forms.Timer hoverTimer;

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
		public CP.Windows.Forms.ExpandingTextBox stackTrace;
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

			this.testTree.ShowCheckBoxes = UserSettings.Options.ShowCheckBoxes;
			this.testTree.VisualStudioSupport = UserSettings.Options.VisualStudioSupport;
			this.testTree.InitialDisplay = 
				(TestSuiteTreeView.DisplayStyle)UserSettings.Options.InitialTreeDisplay;
			this.mainMenu.MenuItems.Add(1, testTree.ViewMenu);
			this.commandLineOptions = commandLineOptions;

			stdErrTab.Enabled = true;
			stdOutTab.Enabled = true;

			runButton.Enabled = false;
			stopButton.Enabled = false;

			outWriter = new TextBoxWriter( stdOutTab );
			errWriter = new TextBoxWriter( stdErrTab );

			TestLoader loader = new TestLoader( outWriter, errWriter, new GuiTestEventDispatcher() );
			loader.ReloadOnRun = UserSettings.Options.ReloadOnRun;
			loader.ReloadOnChange = UserSettings.Options.ReloadOnChange;
			loader.DisplayTestLabels = UserSettings.Options.TestLabels;

			bool vsSupport = UserSettings.Options.VisualStudioSupport;

			AppUI.Init(	this, outWriter, errWriter, loader, vsSupport );

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
			this.stackTrace = new CP.Windows.Forms.ExpandingTextBox();
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
			this.statusBar.AccessibleDescription = resources.GetString("statusBar.AccessibleDescription");
			this.statusBar.AccessibleName = resources.GetString("statusBar.AccessibleName");
			this.statusBar.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("statusBar.Anchor")));
			this.statusBar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("statusBar.BackgroundImage")));
			this.statusBar.DisplayTestProgress = true;
			this.statusBar.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("statusBar.Dock")));
			this.statusBar.Enabled = ((bool)(resources.GetObject("statusBar.Enabled")));
			this.statusBar.Font = ((System.Drawing.Font)(resources.GetObject("statusBar.Font")));
			this.statusBar.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("statusBar.ImeMode")));
			this.statusBar.Location = ((System.Drawing.Point)(resources.GetObject("statusBar.Location")));
			this.statusBar.Name = "statusBar";
			this.statusBar.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("statusBar.RightToLeft")));
			this.statusBar.ShowPanels = true;
			this.statusBar.Size = ((System.Drawing.Size)(resources.GetObject("statusBar.Size")));
			this.statusBar.TabIndex = ((int)(resources.GetObject("statusBar.TabIndex")));
			this.statusBar.Text = resources.GetString("statusBar.Text");
			this.toolTip.SetToolTip(this.statusBar, resources.GetString("statusBar.ToolTip"));
			this.statusBar.Visible = ((bool)(resources.GetObject("statusBar.Visible")));
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.fileMenu,
																					 this.projectMenu,
																					 this.toolsMenu,
																					 this.helpItem});
			this.mainMenu.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mainMenu.RightToLeft")));
			// 
			// fileMenu
			// 
			this.fileMenu.Enabled = ((bool)(resources.GetObject("fileMenu.Enabled")));
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
			this.fileMenu.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("fileMenu.Shortcut")));
			this.fileMenu.ShowShortcut = ((bool)(resources.GetObject("fileMenu.ShowShortcut")));
			this.fileMenu.Text = resources.GetString("fileMenu.Text");
			this.fileMenu.Visible = ((bool)(resources.GetObject("fileMenu.Visible")));
			this.fileMenu.Popup += new System.EventHandler(this.fileMenu_Popup);
			// 
			// newMenuItem
			// 
			this.newMenuItem.Enabled = ((bool)(resources.GetObject("newMenuItem.Enabled")));
			this.newMenuItem.Index = 0;
			this.newMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("newMenuItem.Shortcut")));
			this.newMenuItem.ShowShortcut = ((bool)(resources.GetObject("newMenuItem.ShowShortcut")));
			this.newMenuItem.Text = resources.GetString("newMenuItem.Text");
			this.newMenuItem.Visible = ((bool)(resources.GetObject("newMenuItem.Visible")));
			this.newMenuItem.Click += new System.EventHandler(this.newMenuItem_Click);
			// 
			// openMenuItem
			// 
			this.openMenuItem.Enabled = ((bool)(resources.GetObject("openMenuItem.Enabled")));
			this.openMenuItem.Index = 1;
			this.openMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("openMenuItem.Shortcut")));
			this.openMenuItem.ShowShortcut = ((bool)(resources.GetObject("openMenuItem.ShowShortcut")));
			this.openMenuItem.Text = resources.GetString("openMenuItem.Text");
			this.openMenuItem.Visible = ((bool)(resources.GetObject("openMenuItem.Visible")));
			this.openMenuItem.Click += new System.EventHandler(this.openMenuItem_Click);
			// 
			// closeMenuItem
			// 
			this.closeMenuItem.Enabled = ((bool)(resources.GetObject("closeMenuItem.Enabled")));
			this.closeMenuItem.Index = 2;
			this.closeMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("closeMenuItem.Shortcut")));
			this.closeMenuItem.ShowShortcut = ((bool)(resources.GetObject("closeMenuItem.ShowShortcut")));
			this.closeMenuItem.Text = resources.GetString("closeMenuItem.Text");
			this.closeMenuItem.Visible = ((bool)(resources.GetObject("closeMenuItem.Visible")));
			this.closeMenuItem.Click += new System.EventHandler(this.closeMenuItem_Click);
			// 
			// fileMenuSeparator1
			// 
			this.fileMenuSeparator1.Enabled = ((bool)(resources.GetObject("fileMenuSeparator1.Enabled")));
			this.fileMenuSeparator1.Index = 3;
			this.fileMenuSeparator1.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("fileMenuSeparator1.Shortcut")));
			this.fileMenuSeparator1.ShowShortcut = ((bool)(resources.GetObject("fileMenuSeparator1.ShowShortcut")));
			this.fileMenuSeparator1.Text = resources.GetString("fileMenuSeparator1.Text");
			this.fileMenuSeparator1.Visible = ((bool)(resources.GetObject("fileMenuSeparator1.Visible")));
			// 
			// saveMenuItem
			// 
			this.saveMenuItem.Enabled = ((bool)(resources.GetObject("saveMenuItem.Enabled")));
			this.saveMenuItem.Index = 4;
			this.saveMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("saveMenuItem.Shortcut")));
			this.saveMenuItem.ShowShortcut = ((bool)(resources.GetObject("saveMenuItem.ShowShortcut")));
			this.saveMenuItem.Text = resources.GetString("saveMenuItem.Text");
			this.saveMenuItem.Visible = ((bool)(resources.GetObject("saveMenuItem.Visible")));
			this.saveMenuItem.Click += new System.EventHandler(this.saveMenuItem_Click);
			// 
			// saveAsMenuItem
			// 
			this.saveAsMenuItem.Enabled = ((bool)(resources.GetObject("saveAsMenuItem.Enabled")));
			this.saveAsMenuItem.Index = 5;
			this.saveAsMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("saveAsMenuItem.Shortcut")));
			this.saveAsMenuItem.ShowShortcut = ((bool)(resources.GetObject("saveAsMenuItem.ShowShortcut")));
			this.saveAsMenuItem.Text = resources.GetString("saveAsMenuItem.Text");
			this.saveAsMenuItem.Visible = ((bool)(resources.GetObject("saveAsMenuItem.Visible")));
			this.saveAsMenuItem.Click += new System.EventHandler(this.saveAsMenuItem_Click);
			// 
			// fileMenuSeparator2
			// 
			this.fileMenuSeparator2.Enabled = ((bool)(resources.GetObject("fileMenuSeparator2.Enabled")));
			this.fileMenuSeparator2.Index = 6;
			this.fileMenuSeparator2.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("fileMenuSeparator2.Shortcut")));
			this.fileMenuSeparator2.ShowShortcut = ((bool)(resources.GetObject("fileMenuSeparator2.ShowShortcut")));
			this.fileMenuSeparator2.Text = resources.GetString("fileMenuSeparator2.Text");
			this.fileMenuSeparator2.Visible = ((bool)(resources.GetObject("fileMenuSeparator2.Visible")));
			// 
			// reloadMenuItem
			// 
			this.reloadMenuItem.Enabled = ((bool)(resources.GetObject("reloadMenuItem.Enabled")));
			this.reloadMenuItem.Index = 7;
			this.reloadMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("reloadMenuItem.Shortcut")));
			this.reloadMenuItem.ShowShortcut = ((bool)(resources.GetObject("reloadMenuItem.ShowShortcut")));
			this.reloadMenuItem.Text = resources.GetString("reloadMenuItem.Text");
			this.reloadMenuItem.Visible = ((bool)(resources.GetObject("reloadMenuItem.Visible")));
			this.reloadMenuItem.Click += new System.EventHandler(this.reloadMenuItem_Click);
			// 
			// recentProjectsMenu
			// 
			this.recentProjectsMenu.Enabled = ((bool)(resources.GetObject("recentProjectsMenu.Enabled")));
			this.recentProjectsMenu.Index = 8;
			this.recentProjectsMenu.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("recentProjectsMenu.Shortcut")));
			this.recentProjectsMenu.ShowShortcut = ((bool)(resources.GetObject("recentProjectsMenu.ShowShortcut")));
			this.recentProjectsMenu.Text = resources.GetString("recentProjectsMenu.Text");
			this.recentProjectsMenu.Visible = ((bool)(resources.GetObject("recentProjectsMenu.Visible")));
			// 
			// fileMenuSeparator4
			// 
			this.fileMenuSeparator4.Enabled = ((bool)(resources.GetObject("fileMenuSeparator4.Enabled")));
			this.fileMenuSeparator4.Index = 9;
			this.fileMenuSeparator4.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("fileMenuSeparator4.Shortcut")));
			this.fileMenuSeparator4.ShowShortcut = ((bool)(resources.GetObject("fileMenuSeparator4.ShowShortcut")));
			this.fileMenuSeparator4.Text = resources.GetString("fileMenuSeparator4.Text");
			this.fileMenuSeparator4.Visible = ((bool)(resources.GetObject("fileMenuSeparator4.Visible")));
			// 
			// exitMenuItem
			// 
			this.exitMenuItem.Enabled = ((bool)(resources.GetObject("exitMenuItem.Enabled")));
			this.exitMenuItem.Index = 10;
			this.exitMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("exitMenuItem.Shortcut")));
			this.exitMenuItem.ShowShortcut = ((bool)(resources.GetObject("exitMenuItem.ShowShortcut")));
			this.exitMenuItem.Text = resources.GetString("exitMenuItem.Text");
			this.exitMenuItem.Visible = ((bool)(resources.GetObject("exitMenuItem.Visible")));
			this.exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
			// 
			// projectMenu
			// 
			this.projectMenu.Enabled = ((bool)(resources.GetObject("projectMenu.Enabled")));
			this.projectMenu.Index = 1;
			this.projectMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						this.configMenuItem,
																						this.projectMenuSeparator1,
																						this.addAssemblyMenuItem,
																						this.addVSProjectMenuItem,
																						this.projectMenuSeparator2,
																						this.editProjectMenuItem});
			this.projectMenu.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("projectMenu.Shortcut")));
			this.projectMenu.ShowShortcut = ((bool)(resources.GetObject("projectMenu.ShowShortcut")));
			this.projectMenu.Text = resources.GetString("projectMenu.Text");
			this.projectMenu.Visible = ((bool)(resources.GetObject("projectMenu.Visible")));
			this.projectMenu.Popup += new System.EventHandler(this.projectMenu_Popup);
			// 
			// configMenuItem
			// 
			this.configMenuItem.Enabled = ((bool)(resources.GetObject("configMenuItem.Enabled")));
			this.configMenuItem.Index = 0;
			this.configMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("configMenuItem.Shortcut")));
			this.configMenuItem.ShowShortcut = ((bool)(resources.GetObject("configMenuItem.ShowShortcut")));
			this.configMenuItem.Text = resources.GetString("configMenuItem.Text");
			this.configMenuItem.Visible = ((bool)(resources.GetObject("configMenuItem.Visible")));
			// 
			// projectMenuSeparator1
			// 
			this.projectMenuSeparator1.Enabled = ((bool)(resources.GetObject("projectMenuSeparator1.Enabled")));
			this.projectMenuSeparator1.Index = 1;
			this.projectMenuSeparator1.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("projectMenuSeparator1.Shortcut")));
			this.projectMenuSeparator1.ShowShortcut = ((bool)(resources.GetObject("projectMenuSeparator1.ShowShortcut")));
			this.projectMenuSeparator1.Text = resources.GetString("projectMenuSeparator1.Text");
			this.projectMenuSeparator1.Visible = ((bool)(resources.GetObject("projectMenuSeparator1.Visible")));
			// 
			// addAssemblyMenuItem
			// 
			this.addAssemblyMenuItem.Enabled = ((bool)(resources.GetObject("addAssemblyMenuItem.Enabled")));
			this.addAssemblyMenuItem.Index = 2;
			this.addAssemblyMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("addAssemblyMenuItem.Shortcut")));
			this.addAssemblyMenuItem.ShowShortcut = ((bool)(resources.GetObject("addAssemblyMenuItem.ShowShortcut")));
			this.addAssemblyMenuItem.Text = resources.GetString("addAssemblyMenuItem.Text");
			this.addAssemblyMenuItem.Visible = ((bool)(resources.GetObject("addAssemblyMenuItem.Visible")));
			this.addAssemblyMenuItem.Click += new System.EventHandler(this.addAssemblyMenuItem_Click);
			// 
			// addVSProjectMenuItem
			// 
			this.addVSProjectMenuItem.Enabled = ((bool)(resources.GetObject("addVSProjectMenuItem.Enabled")));
			this.addVSProjectMenuItem.Index = 3;
			this.addVSProjectMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("addVSProjectMenuItem.Shortcut")));
			this.addVSProjectMenuItem.ShowShortcut = ((bool)(resources.GetObject("addVSProjectMenuItem.ShowShortcut")));
			this.addVSProjectMenuItem.Text = resources.GetString("addVSProjectMenuItem.Text");
			this.addVSProjectMenuItem.Visible = ((bool)(resources.GetObject("addVSProjectMenuItem.Visible")));
			this.addVSProjectMenuItem.Click += new System.EventHandler(this.addVSProjectMenuItem_Click);
			// 
			// projectMenuSeparator2
			// 
			this.projectMenuSeparator2.Enabled = ((bool)(resources.GetObject("projectMenuSeparator2.Enabled")));
			this.projectMenuSeparator2.Index = 4;
			this.projectMenuSeparator2.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("projectMenuSeparator2.Shortcut")));
			this.projectMenuSeparator2.ShowShortcut = ((bool)(resources.GetObject("projectMenuSeparator2.ShowShortcut")));
			this.projectMenuSeparator2.Text = resources.GetString("projectMenuSeparator2.Text");
			this.projectMenuSeparator2.Visible = ((bool)(resources.GetObject("projectMenuSeparator2.Visible")));
			// 
			// editProjectMenuItem
			// 
			this.editProjectMenuItem.Enabled = ((bool)(resources.GetObject("editProjectMenuItem.Enabled")));
			this.editProjectMenuItem.Index = 5;
			this.editProjectMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("editProjectMenuItem.Shortcut")));
			this.editProjectMenuItem.ShowShortcut = ((bool)(resources.GetObject("editProjectMenuItem.ShowShortcut")));
			this.editProjectMenuItem.Text = resources.GetString("editProjectMenuItem.Text");
			this.editProjectMenuItem.Visible = ((bool)(resources.GetObject("editProjectMenuItem.Visible")));
			this.editProjectMenuItem.Click += new System.EventHandler(this.editProjectMenuItem_Click);
			// 
			// toolsMenu
			// 
			this.toolsMenu.Enabled = ((bool)(resources.GetObject("toolsMenu.Enabled")));
			this.toolsMenu.Index = 2;
			this.toolsMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.saveXmlResultsMenuItem,
																					  this.exceptionDetailsMenuItem,
																					  this.toolsMenuSeparator1,
																					  this.optionsMenuItem});
			this.toolsMenu.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("toolsMenu.Shortcut")));
			this.toolsMenu.ShowShortcut = ((bool)(resources.GetObject("toolsMenu.ShowShortcut")));
			this.toolsMenu.Text = resources.GetString("toolsMenu.Text");
			this.toolsMenu.Visible = ((bool)(resources.GetObject("toolsMenu.Visible")));
			this.toolsMenu.Popup += new System.EventHandler(this.toolsMenu_Popup);
			// 
			// saveXmlResultsMenuItem
			// 
			this.saveXmlResultsMenuItem.Enabled = ((bool)(resources.GetObject("saveXmlResultsMenuItem.Enabled")));
			this.saveXmlResultsMenuItem.Index = 0;
			this.saveXmlResultsMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("saveXmlResultsMenuItem.Shortcut")));
			this.saveXmlResultsMenuItem.ShowShortcut = ((bool)(resources.GetObject("saveXmlResultsMenuItem.ShowShortcut")));
			this.saveXmlResultsMenuItem.Text = resources.GetString("saveXmlResultsMenuItem.Text");
			this.saveXmlResultsMenuItem.Visible = ((bool)(resources.GetObject("saveXmlResultsMenuItem.Visible")));
			this.saveXmlResultsMenuItem.Click += new System.EventHandler(this.saveXmlResultsMenuItem_Click);
			// 
			// exceptionDetailsMenuItem
			// 
			this.exceptionDetailsMenuItem.Enabled = ((bool)(resources.GetObject("exceptionDetailsMenuItem.Enabled")));
			this.exceptionDetailsMenuItem.Index = 1;
			this.exceptionDetailsMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("exceptionDetailsMenuItem.Shortcut")));
			this.exceptionDetailsMenuItem.ShowShortcut = ((bool)(resources.GetObject("exceptionDetailsMenuItem.ShowShortcut")));
			this.exceptionDetailsMenuItem.Text = resources.GetString("exceptionDetailsMenuItem.Text");
			this.exceptionDetailsMenuItem.Visible = ((bool)(resources.GetObject("exceptionDetailsMenuItem.Visible")));
			this.exceptionDetailsMenuItem.Click += new System.EventHandler(this.exceptionDetailsMenuItem_Click);
			// 
			// toolsMenuSeparator1
			// 
			this.toolsMenuSeparator1.Enabled = ((bool)(resources.GetObject("toolsMenuSeparator1.Enabled")));
			this.toolsMenuSeparator1.Index = 2;
			this.toolsMenuSeparator1.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("toolsMenuSeparator1.Shortcut")));
			this.toolsMenuSeparator1.ShowShortcut = ((bool)(resources.GetObject("toolsMenuSeparator1.ShowShortcut")));
			this.toolsMenuSeparator1.Text = resources.GetString("toolsMenuSeparator1.Text");
			this.toolsMenuSeparator1.Visible = ((bool)(resources.GetObject("toolsMenuSeparator1.Visible")));
			// 
			// optionsMenuItem
			// 
			this.optionsMenuItem.Enabled = ((bool)(resources.GetObject("optionsMenuItem.Enabled")));
			this.optionsMenuItem.Index = 3;
			this.optionsMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("optionsMenuItem.Shortcut")));
			this.optionsMenuItem.ShowShortcut = ((bool)(resources.GetObject("optionsMenuItem.ShowShortcut")));
			this.optionsMenuItem.Text = resources.GetString("optionsMenuItem.Text");
			this.optionsMenuItem.Visible = ((bool)(resources.GetObject("optionsMenuItem.Visible")));
			this.optionsMenuItem.Click += new System.EventHandler(this.optionsMenuItem_Click);
			// 
			// helpItem
			// 
			this.helpItem.Enabled = ((bool)(resources.GetObject("helpItem.Enabled")));
			this.helpItem.Index = 3;
			this.helpItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.helpMenuItem,
																					 this.helpMenuSeparator1,
																					 this.aboutMenuItem});
			this.helpItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("helpItem.Shortcut")));
			this.helpItem.ShowShortcut = ((bool)(resources.GetObject("helpItem.ShowShortcut")));
			this.helpItem.Text = resources.GetString("helpItem.Text");
			this.helpItem.Visible = ((bool)(resources.GetObject("helpItem.Visible")));
			// 
			// helpMenuItem
			// 
			this.helpMenuItem.Enabled = ((bool)(resources.GetObject("helpMenuItem.Enabled")));
			this.helpMenuItem.Index = 0;
			this.helpMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("helpMenuItem.Shortcut")));
			this.helpMenuItem.ShowShortcut = ((bool)(resources.GetObject("helpMenuItem.ShowShortcut")));
			this.helpMenuItem.Text = resources.GetString("helpMenuItem.Text");
			this.helpMenuItem.Visible = ((bool)(resources.GetObject("helpMenuItem.Visible")));
			this.helpMenuItem.Click += new System.EventHandler(this.helpMenuItem_Click);
			// 
			// helpMenuSeparator1
			// 
			this.helpMenuSeparator1.Enabled = ((bool)(resources.GetObject("helpMenuSeparator1.Enabled")));
			this.helpMenuSeparator1.Index = 1;
			this.helpMenuSeparator1.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("helpMenuSeparator1.Shortcut")));
			this.helpMenuSeparator1.ShowShortcut = ((bool)(resources.GetObject("helpMenuSeparator1.ShowShortcut")));
			this.helpMenuSeparator1.Text = resources.GetString("helpMenuSeparator1.Text");
			this.helpMenuSeparator1.Visible = ((bool)(resources.GetObject("helpMenuSeparator1.Visible")));
			// 
			// aboutMenuItem
			// 
			this.aboutMenuItem.Enabled = ((bool)(resources.GetObject("aboutMenuItem.Enabled")));
			this.aboutMenuItem.Index = 2;
			this.aboutMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("aboutMenuItem.Shortcut")));
			this.aboutMenuItem.ShowShortcut = ((bool)(resources.GetObject("aboutMenuItem.ShowShortcut")));
			this.aboutMenuItem.Text = resources.GetString("aboutMenuItem.Text");
			this.aboutMenuItem.Visible = ((bool)(resources.GetObject("aboutMenuItem.Visible")));
			this.aboutMenuItem.Click += new System.EventHandler(this.aboutMenuItem_Click);
			// 
			// splitter1
			// 
			this.splitter1.AccessibleDescription = resources.GetString("splitter1.AccessibleDescription");
			this.splitter1.AccessibleName = resources.GetString("splitter1.AccessibleName");
			this.splitter1.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("splitter1.Anchor")));
			this.splitter1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("splitter1.BackgroundImage")));
			this.splitter1.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("splitter1.Dock")));
			this.splitter1.Enabled = ((bool)(resources.GetObject("splitter1.Enabled")));
			this.splitter1.Font = ((System.Drawing.Font)(resources.GetObject("splitter1.Font")));
			this.splitter1.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("splitter1.ImeMode")));
			this.splitter1.Location = ((System.Drawing.Point)(resources.GetObject("splitter1.Location")));
			this.splitter1.MinExtra = ((int)(resources.GetObject("splitter1.MinExtra")));
			this.splitter1.MinSize = ((int)(resources.GetObject("splitter1.MinSize")));
			this.splitter1.Name = "splitter1";
			this.splitter1.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("splitter1.RightToLeft")));
			this.splitter1.Size = ((System.Drawing.Size)(resources.GetObject("splitter1.Size")));
			this.splitter1.TabIndex = ((int)(resources.GetObject("splitter1.TabIndex")));
			this.splitter1.TabStop = false;
			this.toolTip.SetToolTip(this.splitter1, resources.GetString("splitter1.ToolTip"));
			this.splitter1.Visible = ((bool)(resources.GetObject("splitter1.Visible")));
			// 
			// panel1
			// 
			this.panel1.AccessibleDescription = resources.GetString("panel1.AccessibleDescription");
			this.panel1.AccessibleName = resources.GetString("panel1.AccessibleName");
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("panel1.Anchor")));
			this.panel1.AutoScroll = ((bool)(resources.GetObject("panel1.AutoScroll")));
			this.panel1.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("panel1.AutoScrollMargin")));
			this.panel1.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("panel1.AutoScrollMinSize")));
			this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
			this.panel1.Controls.Add(this.resultTabs);
			this.panel1.Controls.Add(this.groupBox1);
			this.panel1.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("panel1.Dock")));
			this.panel1.Enabled = ((bool)(resources.GetObject("panel1.Enabled")));
			this.panel1.Font = ((System.Drawing.Font)(resources.GetObject("panel1.Font")));
			this.panel1.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("panel1.ImeMode")));
			this.panel1.Location = ((System.Drawing.Point)(resources.GetObject("panel1.Location")));
			this.panel1.Name = "panel1";
			this.panel1.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("panel1.RightToLeft")));
			this.panel1.Size = ((System.Drawing.Size)(resources.GetObject("panel1.Size")));
			this.panel1.TabIndex = ((int)(resources.GetObject("panel1.TabIndex")));
			this.panel1.Text = resources.GetString("panel1.Text");
			this.toolTip.SetToolTip(this.panel1, resources.GetString("panel1.ToolTip"));
			this.panel1.Visible = ((bool)(resources.GetObject("panel1.Visible")));
			// 
			// resultTabs
			// 
			this.resultTabs.AccessibleDescription = resources.GetString("resultTabs.AccessibleDescription");
			this.resultTabs.AccessibleName = resources.GetString("resultTabs.AccessibleName");
			this.resultTabs.Alignment = ((System.Windows.Forms.TabAlignment)(resources.GetObject("resultTabs.Alignment")));
			this.resultTabs.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("resultTabs.Anchor")));
			this.resultTabs.Appearance = ((System.Windows.Forms.TabAppearance)(resources.GetObject("resultTabs.Appearance")));
			this.resultTabs.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("resultTabs.BackgroundImage")));
			this.resultTabs.Controls.Add(this.errorPage);
			this.resultTabs.Controls.Add(this.testsNotRun);
			this.resultTabs.Controls.Add(this.stderr);
			this.resultTabs.Controls.Add(this.stdout);
			this.resultTabs.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("resultTabs.Dock")));
			this.resultTabs.Enabled = ((bool)(resources.GetObject("resultTabs.Enabled")));
			this.resultTabs.Font = ((System.Drawing.Font)(resources.GetObject("resultTabs.Font")));
			this.resultTabs.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resultTabs.ImeMode")));
			this.resultTabs.ItemSize = ((System.Drawing.Size)(resources.GetObject("resultTabs.ItemSize")));
			this.resultTabs.Location = ((System.Drawing.Point)(resources.GetObject("resultTabs.Location")));
			this.resultTabs.Name = "resultTabs";
			this.resultTabs.Padding = ((System.Drawing.Point)(resources.GetObject("resultTabs.Padding")));
			this.resultTabs.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("resultTabs.RightToLeft")));
			this.resultTabs.SelectedIndex = 0;
			this.resultTabs.ShowToolTips = ((bool)(resources.GetObject("resultTabs.ShowToolTips")));
			this.resultTabs.Size = ((System.Drawing.Size)(resources.GetObject("resultTabs.Size")));
			this.resultTabs.TabIndex = ((int)(resources.GetObject("resultTabs.TabIndex")));
			this.resultTabs.Text = resources.GetString("resultTabs.Text");
			this.toolTip.SetToolTip(this.resultTabs, resources.GetString("resultTabs.ToolTip"));
			this.resultTabs.Visible = ((bool)(resources.GetObject("resultTabs.Visible")));
			// 
			// errorPage
			// 
			this.errorPage.AccessibleDescription = resources.GetString("errorPage.AccessibleDescription");
			this.errorPage.AccessibleName = resources.GetString("errorPage.AccessibleName");
			this.errorPage.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("errorPage.Anchor")));
			this.errorPage.AutoScroll = ((bool)(resources.GetObject("errorPage.AutoScroll")));
			this.errorPage.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("errorPage.AutoScrollMargin")));
			this.errorPage.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("errorPage.AutoScrollMinSize")));
			this.errorPage.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("errorPage.BackgroundImage")));
			this.errorPage.Controls.Add(this.stackTrace);
			this.errorPage.Controls.Add(this.splitter3);
			this.errorPage.Controls.Add(this.detailList);
			this.errorPage.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("errorPage.Dock")));
			this.errorPage.Enabled = ((bool)(resources.GetObject("errorPage.Enabled")));
			this.errorPage.Font = ((System.Drawing.Font)(resources.GetObject("errorPage.Font")));
			this.errorPage.ImageIndex = ((int)(resources.GetObject("errorPage.ImageIndex")));
			this.errorPage.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("errorPage.ImeMode")));
			this.errorPage.Location = ((System.Drawing.Point)(resources.GetObject("errorPage.Location")));
			this.errorPage.Name = "errorPage";
			this.errorPage.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("errorPage.RightToLeft")));
			this.errorPage.Size = ((System.Drawing.Size)(resources.GetObject("errorPage.Size")));
			this.errorPage.TabIndex = ((int)(resources.GetObject("errorPage.TabIndex")));
			this.errorPage.Text = resources.GetString("errorPage.Text");
			this.toolTip.SetToolTip(this.errorPage, resources.GetString("errorPage.ToolTip"));
			this.errorPage.ToolTipText = resources.GetString("errorPage.ToolTipText");
			this.errorPage.Visible = ((bool)(resources.GetObject("errorPage.Visible")));
			// 
			// stackTrace
			// 
			this.stackTrace.AccessibleDescription = resources.GetString("stackTrace.AccessibleDescription");
			this.stackTrace.AccessibleName = resources.GetString("stackTrace.AccessibleName");
			this.stackTrace.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("stackTrace.Anchor")));
			this.stackTrace.AutoSize = ((bool)(resources.GetObject("stackTrace.AutoSize")));
			this.stackTrace.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("stackTrace.BackgroundImage")));
			this.stackTrace.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("stackTrace.Dock")));
			this.stackTrace.Enabled = ((bool)(resources.GetObject("stackTrace.Enabled")));
			this.stackTrace.Font = ((System.Drawing.Font)(resources.GetObject("stackTrace.Font")));
			this.stackTrace.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("stackTrace.ImeMode")));
			this.stackTrace.Location = ((System.Drawing.Point)(resources.GetObject("stackTrace.Location")));
			this.stackTrace.MaxLength = ((int)(resources.GetObject("stackTrace.MaxLength")));
			this.stackTrace.Multiline = ((bool)(resources.GetObject("stackTrace.Multiline")));
			this.stackTrace.Name = "stackTrace";
			this.stackTrace.PasswordChar = ((char)(resources.GetObject("stackTrace.PasswordChar")));
			this.stackTrace.ReadOnly = true;
			this.stackTrace.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("stackTrace.RightToLeft")));
			this.stackTrace.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("stackTrace.ScrollBars")));
			this.stackTrace.Size = ((System.Drawing.Size)(resources.GetObject("stackTrace.Size")));
			this.stackTrace.TabIndex = ((int)(resources.GetObject("stackTrace.TabIndex")));
			this.stackTrace.Text = resources.GetString("stackTrace.Text");
			this.stackTrace.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("stackTrace.TextAlign")));
			this.toolTip.SetToolTip(this.stackTrace, resources.GetString("stackTrace.ToolTip"));
			this.stackTrace.Visible = ((bool)(resources.GetObject("stackTrace.Visible")));
			this.stackTrace.WordWrap = ((bool)(resources.GetObject("stackTrace.WordWrap")));
			// 
			// splitter3
			// 
			this.splitter3.AccessibleDescription = resources.GetString("splitter3.AccessibleDescription");
			this.splitter3.AccessibleName = resources.GetString("splitter3.AccessibleName");
			this.splitter3.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("splitter3.Anchor")));
			this.splitter3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("splitter3.BackgroundImage")));
			this.splitter3.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("splitter3.Dock")));
			this.splitter3.Enabled = ((bool)(resources.GetObject("splitter3.Enabled")));
			this.splitter3.Font = ((System.Drawing.Font)(resources.GetObject("splitter3.Font")));
			this.splitter3.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("splitter3.ImeMode")));
			this.splitter3.Location = ((System.Drawing.Point)(resources.GetObject("splitter3.Location")));
			this.splitter3.MinExtra = ((int)(resources.GetObject("splitter3.MinExtra")));
			this.splitter3.MinSize = ((int)(resources.GetObject("splitter3.MinSize")));
			this.splitter3.Name = "splitter3";
			this.splitter3.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("splitter3.RightToLeft")));
			this.splitter3.Size = ((System.Drawing.Size)(resources.GetObject("splitter3.Size")));
			this.splitter3.TabIndex = ((int)(resources.GetObject("splitter3.TabIndex")));
			this.splitter3.TabStop = false;
			this.toolTip.SetToolTip(this.splitter3, resources.GetString("splitter3.ToolTip"));
			this.splitter3.Visible = ((bool)(resources.GetObject("splitter3.Visible")));
			// 
			// detailList
			// 
			this.detailList.AccessibleDescription = resources.GetString("detailList.AccessibleDescription");
			this.detailList.AccessibleName = resources.GetString("detailList.AccessibleName");
			this.detailList.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("detailList.Anchor")));
			this.detailList.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("detailList.BackgroundImage")));
			this.detailList.ColumnWidth = ((int)(resources.GetObject("detailList.ColumnWidth")));
			this.detailList.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("detailList.Dock")));
			this.detailList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
			this.detailList.Enabled = ((bool)(resources.GetObject("detailList.Enabled")));
			this.detailList.Font = ((System.Drawing.Font)(resources.GetObject("detailList.Font")));
			this.detailList.HorizontalExtent = ((int)(resources.GetObject("detailList.HorizontalExtent")));
			this.detailList.HorizontalScrollbar = ((bool)(resources.GetObject("detailList.HorizontalScrollbar")));
			this.detailList.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("detailList.ImeMode")));
			this.detailList.IntegralHeight = ((bool)(resources.GetObject("detailList.IntegralHeight")));
			this.detailList.ItemHeight = ((int)(resources.GetObject("detailList.ItemHeight")));
			this.detailList.Location = ((System.Drawing.Point)(resources.GetObject("detailList.Location")));
			this.detailList.Name = "detailList";
			this.detailList.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("detailList.RightToLeft")));
			this.detailList.ScrollAlwaysVisible = ((bool)(resources.GetObject("detailList.ScrollAlwaysVisible")));
			this.detailList.Size = ((System.Drawing.Size)(resources.GetObject("detailList.Size")));
			this.detailList.TabIndex = ((int)(resources.GetObject("detailList.TabIndex")));
			this.toolTip.SetToolTip(this.detailList, resources.GetString("detailList.ToolTip"));
			this.detailList.Visible = ((bool)(resources.GetObject("detailList.Visible")));
			this.detailList.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.detailList_MeasureItem);
			this.detailList.MouseMove += new System.Windows.Forms.MouseEventHandler(this.detailList_MouseMove);
			this.detailList.MouseLeave += new System.EventHandler(this.detailList_MouseLeave);
			this.detailList.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.detailList_DrawItem);
			this.detailList.SelectedIndexChanged += new System.EventHandler(this.detailList_SelectedIndexChanged);
			// 
			// testsNotRun
			// 
			this.testsNotRun.AccessibleDescription = resources.GetString("testsNotRun.AccessibleDescription");
			this.testsNotRun.AccessibleName = resources.GetString("testsNotRun.AccessibleName");
			this.testsNotRun.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("testsNotRun.Anchor")));
			this.testsNotRun.AutoScroll = ((bool)(resources.GetObject("testsNotRun.AutoScroll")));
			this.testsNotRun.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("testsNotRun.AutoScrollMargin")));
			this.testsNotRun.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("testsNotRun.AutoScrollMinSize")));
			this.testsNotRun.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("testsNotRun.BackgroundImage")));
			this.testsNotRun.Controls.Add(this.notRunTree);
			this.testsNotRun.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("testsNotRun.Dock")));
			this.testsNotRun.Enabled = ((bool)(resources.GetObject("testsNotRun.Enabled")));
			this.testsNotRun.Font = ((System.Drawing.Font)(resources.GetObject("testsNotRun.Font")));
			this.testsNotRun.ImageIndex = ((int)(resources.GetObject("testsNotRun.ImageIndex")));
			this.testsNotRun.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("testsNotRun.ImeMode")));
			this.testsNotRun.Location = ((System.Drawing.Point)(resources.GetObject("testsNotRun.Location")));
			this.testsNotRun.Name = "testsNotRun";
			this.testsNotRun.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("testsNotRun.RightToLeft")));
			this.testsNotRun.Size = ((System.Drawing.Size)(resources.GetObject("testsNotRun.Size")));
			this.testsNotRun.TabIndex = ((int)(resources.GetObject("testsNotRun.TabIndex")));
			this.testsNotRun.Text = resources.GetString("testsNotRun.Text");
			this.toolTip.SetToolTip(this.testsNotRun, resources.GetString("testsNotRun.ToolTip"));
			this.testsNotRun.ToolTipText = resources.GetString("testsNotRun.ToolTipText");
			this.testsNotRun.Visible = ((bool)(resources.GetObject("testsNotRun.Visible")));
			// 
			// notRunTree
			// 
			this.notRunTree.AccessibleDescription = resources.GetString("notRunTree.AccessibleDescription");
			this.notRunTree.AccessibleName = resources.GetString("notRunTree.AccessibleName");
			this.notRunTree.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("notRunTree.Anchor")));
			this.notRunTree.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("notRunTree.BackgroundImage")));
			this.notRunTree.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("notRunTree.Dock")));
			this.notRunTree.Enabled = ((bool)(resources.GetObject("notRunTree.Enabled")));
			this.notRunTree.Font = ((System.Drawing.Font)(resources.GetObject("notRunTree.Font")));
			this.notRunTree.ImageIndex = ((int)(resources.GetObject("notRunTree.ImageIndex")));
			this.notRunTree.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("notRunTree.ImeMode")));
			this.notRunTree.Indent = ((int)(resources.GetObject("notRunTree.Indent")));
			this.notRunTree.ItemHeight = ((int)(resources.GetObject("notRunTree.ItemHeight")));
			this.notRunTree.Location = ((System.Drawing.Point)(resources.GetObject("notRunTree.Location")));
			this.notRunTree.Name = "notRunTree";
			this.notRunTree.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("notRunTree.RightToLeft")));
			this.notRunTree.SelectedImageIndex = ((int)(resources.GetObject("notRunTree.SelectedImageIndex")));
			this.notRunTree.Size = ((System.Drawing.Size)(resources.GetObject("notRunTree.Size")));
			this.notRunTree.TabIndex = ((int)(resources.GetObject("notRunTree.TabIndex")));
			this.notRunTree.Text = resources.GetString("notRunTree.Text");
			this.toolTip.SetToolTip(this.notRunTree, resources.GetString("notRunTree.ToolTip"));
			this.notRunTree.Visible = ((bool)(resources.GetObject("notRunTree.Visible")));
			// 
			// stderr
			// 
			this.stderr.AccessibleDescription = resources.GetString("stderr.AccessibleDescription");
			this.stderr.AccessibleName = resources.GetString("stderr.AccessibleName");
			this.stderr.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("stderr.Anchor")));
			this.stderr.AutoScroll = ((bool)(resources.GetObject("stderr.AutoScroll")));
			this.stderr.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("stderr.AutoScrollMargin")));
			this.stderr.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("stderr.AutoScrollMinSize")));
			this.stderr.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("stderr.BackgroundImage")));
			this.stderr.Controls.Add(this.stdErrTab);
			this.stderr.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("stderr.Dock")));
			this.stderr.Enabled = ((bool)(resources.GetObject("stderr.Enabled")));
			this.stderr.Font = ((System.Drawing.Font)(resources.GetObject("stderr.Font")));
			this.stderr.ImageIndex = ((int)(resources.GetObject("stderr.ImageIndex")));
			this.stderr.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("stderr.ImeMode")));
			this.stderr.Location = ((System.Drawing.Point)(resources.GetObject("stderr.Location")));
			this.stderr.Name = "stderr";
			this.stderr.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("stderr.RightToLeft")));
			this.stderr.Size = ((System.Drawing.Size)(resources.GetObject("stderr.Size")));
			this.stderr.TabIndex = ((int)(resources.GetObject("stderr.TabIndex")));
			this.stderr.Text = resources.GetString("stderr.Text");
			this.toolTip.SetToolTip(this.stderr, resources.GetString("stderr.ToolTip"));
			this.stderr.ToolTipText = resources.GetString("stderr.ToolTipText");
			this.stderr.Visible = ((bool)(resources.GetObject("stderr.Visible")));
			// 
			// stdErrTab
			// 
			this.stdErrTab.AccessibleDescription = resources.GetString("stdErrTab.AccessibleDescription");
			this.stdErrTab.AccessibleName = resources.GetString("stdErrTab.AccessibleName");
			this.stdErrTab.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("stdErrTab.Anchor")));
			this.stdErrTab.AutoSize = ((bool)(resources.GetObject("stdErrTab.AutoSize")));
			this.stdErrTab.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("stdErrTab.BackgroundImage")));
			this.stdErrTab.BulletIndent = ((int)(resources.GetObject("stdErrTab.BulletIndent")));
			this.stdErrTab.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("stdErrTab.Dock")));
			this.stdErrTab.Enabled = ((bool)(resources.GetObject("stdErrTab.Enabled")));
			this.stdErrTab.Font = ((System.Drawing.Font)(resources.GetObject("stdErrTab.Font")));
			this.stdErrTab.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("stdErrTab.ImeMode")));
			this.stdErrTab.Location = ((System.Drawing.Point)(resources.GetObject("stdErrTab.Location")));
			this.stdErrTab.MaxLength = ((int)(resources.GetObject("stdErrTab.MaxLength")));
			this.stdErrTab.Multiline = ((bool)(resources.GetObject("stdErrTab.Multiline")));
			this.stdErrTab.Name = "stdErrTab";
			this.stdErrTab.ReadOnly = true;
			this.stdErrTab.RightMargin = ((int)(resources.GetObject("stdErrTab.RightMargin")));
			this.stdErrTab.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("stdErrTab.RightToLeft")));
			this.stdErrTab.ScrollBars = ((System.Windows.Forms.RichTextBoxScrollBars)(resources.GetObject("stdErrTab.ScrollBars")));
			this.stdErrTab.Size = ((System.Drawing.Size)(resources.GetObject("stdErrTab.Size")));
			this.stdErrTab.TabIndex = ((int)(resources.GetObject("stdErrTab.TabIndex")));
			this.stdErrTab.Text = resources.GetString("stdErrTab.Text");
			this.toolTip.SetToolTip(this.stdErrTab, resources.GetString("stdErrTab.ToolTip"));
			this.stdErrTab.Visible = ((bool)(resources.GetObject("stdErrTab.Visible")));
			this.stdErrTab.WordWrap = ((bool)(resources.GetObject("stdErrTab.WordWrap")));
			this.stdErrTab.ZoomFactor = ((System.Single)(resources.GetObject("stdErrTab.ZoomFactor")));
			// 
			// stdout
			// 
			this.stdout.AccessibleDescription = resources.GetString("stdout.AccessibleDescription");
			this.stdout.AccessibleName = resources.GetString("stdout.AccessibleName");
			this.stdout.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("stdout.Anchor")));
			this.stdout.AutoScroll = ((bool)(resources.GetObject("stdout.AutoScroll")));
			this.stdout.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("stdout.AutoScrollMargin")));
			this.stdout.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("stdout.AutoScrollMinSize")));
			this.stdout.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("stdout.BackgroundImage")));
			this.stdout.Controls.Add(this.stdOutTab);
			this.stdout.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("stdout.Dock")));
			this.stdout.Enabled = ((bool)(resources.GetObject("stdout.Enabled")));
			this.stdout.Font = ((System.Drawing.Font)(resources.GetObject("stdout.Font")));
			this.stdout.ImageIndex = ((int)(resources.GetObject("stdout.ImageIndex")));
			this.stdout.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("stdout.ImeMode")));
			this.stdout.Location = ((System.Drawing.Point)(resources.GetObject("stdout.Location")));
			this.stdout.Name = "stdout";
			this.stdout.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("stdout.RightToLeft")));
			this.stdout.Size = ((System.Drawing.Size)(resources.GetObject("stdout.Size")));
			this.stdout.TabIndex = ((int)(resources.GetObject("stdout.TabIndex")));
			this.stdout.Text = resources.GetString("stdout.Text");
			this.toolTip.SetToolTip(this.stdout, resources.GetString("stdout.ToolTip"));
			this.stdout.ToolTipText = resources.GetString("stdout.ToolTipText");
			this.stdout.Visible = ((bool)(resources.GetObject("stdout.Visible")));
			// 
			// stdOutTab
			// 
			this.stdOutTab.AccessibleDescription = resources.GetString("stdOutTab.AccessibleDescription");
			this.stdOutTab.AccessibleName = resources.GetString("stdOutTab.AccessibleName");
			this.stdOutTab.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("stdOutTab.Anchor")));
			this.stdOutTab.AutoSize = ((bool)(resources.GetObject("stdOutTab.AutoSize")));
			this.stdOutTab.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("stdOutTab.BackgroundImage")));
			this.stdOutTab.BulletIndent = ((int)(resources.GetObject("stdOutTab.BulletIndent")));
			this.stdOutTab.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("stdOutTab.Dock")));
			this.stdOutTab.Enabled = ((bool)(resources.GetObject("stdOutTab.Enabled")));
			this.stdOutTab.Font = ((System.Drawing.Font)(resources.GetObject("stdOutTab.Font")));
			this.stdOutTab.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("stdOutTab.ImeMode")));
			this.stdOutTab.Location = ((System.Drawing.Point)(resources.GetObject("stdOutTab.Location")));
			this.stdOutTab.MaxLength = ((int)(resources.GetObject("stdOutTab.MaxLength")));
			this.stdOutTab.Multiline = ((bool)(resources.GetObject("stdOutTab.Multiline")));
			this.stdOutTab.Name = "stdOutTab";
			this.stdOutTab.ReadOnly = true;
			this.stdOutTab.RightMargin = ((int)(resources.GetObject("stdOutTab.RightMargin")));
			this.stdOutTab.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("stdOutTab.RightToLeft")));
			this.stdOutTab.ScrollBars = ((System.Windows.Forms.RichTextBoxScrollBars)(resources.GetObject("stdOutTab.ScrollBars")));
			this.stdOutTab.Size = ((System.Drawing.Size)(resources.GetObject("stdOutTab.Size")));
			this.stdOutTab.TabIndex = ((int)(resources.GetObject("stdOutTab.TabIndex")));
			this.stdOutTab.Text = resources.GetString("stdOutTab.Text");
			this.toolTip.SetToolTip(this.stdOutTab, resources.GetString("stdOutTab.ToolTip"));
			this.stdOutTab.Visible = ((bool)(resources.GetObject("stdOutTab.Visible")));
			this.stdOutTab.WordWrap = ((bool)(resources.GetObject("stdOutTab.WordWrap")));
			this.stdOutTab.ZoomFactor = ((System.Single)(resources.GetObject("stdOutTab.ZoomFactor")));
			// 
			// groupBox1
			// 
			this.groupBox1.AccessibleDescription = resources.GetString("groupBox1.AccessibleDescription");
			this.groupBox1.AccessibleName = resources.GetString("groupBox1.AccessibleName");
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("groupBox1.Anchor")));
			this.groupBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("groupBox1.BackgroundImage")));
			this.groupBox1.Controls.Add(this.stopButton);
			this.groupBox1.Controls.Add(this.runButton);
			this.groupBox1.Controls.Add(this.suiteName);
			this.groupBox1.Controls.Add(this.progressBar);
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
			this.toolTip.SetToolTip(this.groupBox1, resources.GetString("groupBox1.ToolTip"));
			this.groupBox1.Visible = ((bool)(resources.GetObject("groupBox1.Visible")));
			// 
			// stopButton
			// 
			this.stopButton.AccessibleDescription = resources.GetString("stopButton.AccessibleDescription");
			this.stopButton.AccessibleName = resources.GetString("stopButton.AccessibleName");
			this.stopButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("stopButton.Anchor")));
			this.stopButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("stopButton.BackgroundImage")));
			this.stopButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("stopButton.Dock")));
			this.stopButton.Enabled = ((bool)(resources.GetObject("stopButton.Enabled")));
			this.stopButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("stopButton.FlatStyle")));
			this.stopButton.Font = ((System.Drawing.Font)(resources.GetObject("stopButton.Font")));
			this.stopButton.Image = ((System.Drawing.Image)(resources.GetObject("stopButton.Image")));
			this.stopButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("stopButton.ImageAlign")));
			this.stopButton.ImageIndex = ((int)(resources.GetObject("stopButton.ImageIndex")));
			this.stopButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("stopButton.ImeMode")));
			this.stopButton.Location = ((System.Drawing.Point)(resources.GetObject("stopButton.Location")));
			this.stopButton.Name = "stopButton";
			this.stopButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("stopButton.RightToLeft")));
			this.stopButton.Size = ((System.Drawing.Size)(resources.GetObject("stopButton.Size")));
			this.stopButton.TabIndex = ((int)(resources.GetObject("stopButton.TabIndex")));
			this.stopButton.Text = resources.GetString("stopButton.Text");
			this.stopButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("stopButton.TextAlign")));
			this.toolTip.SetToolTip(this.stopButton, resources.GetString("stopButton.ToolTip"));
			this.stopButton.Visible = ((bool)(resources.GetObject("stopButton.Visible")));
			this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
			// 
			// runButton
			// 
			this.runButton.AccessibleDescription = resources.GetString("runButton.AccessibleDescription");
			this.runButton.AccessibleName = resources.GetString("runButton.AccessibleName");
			this.runButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("runButton.Anchor")));
			this.runButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("runButton.BackgroundImage")));
			this.runButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("runButton.Dock")));
			this.runButton.Enabled = ((bool)(resources.GetObject("runButton.Enabled")));
			this.runButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("runButton.FlatStyle")));
			this.runButton.Font = ((System.Drawing.Font)(resources.GetObject("runButton.Font")));
			this.runButton.Image = ((System.Drawing.Image)(resources.GetObject("runButton.Image")));
			this.runButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("runButton.ImageAlign")));
			this.runButton.ImageIndex = ((int)(resources.GetObject("runButton.ImageIndex")));
			this.runButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("runButton.ImeMode")));
			this.runButton.Location = ((System.Drawing.Point)(resources.GetObject("runButton.Location")));
			this.runButton.Name = "runButton";
			this.runButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("runButton.RightToLeft")));
			this.runButton.Size = ((System.Drawing.Size)(resources.GetObject("runButton.Size")));
			this.runButton.TabIndex = ((int)(resources.GetObject("runButton.TabIndex")));
			this.runButton.Text = resources.GetString("runButton.Text");
			this.runButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("runButton.TextAlign")));
			this.toolTip.SetToolTip(this.runButton, resources.GetString("runButton.ToolTip"));
			this.runButton.Visible = ((bool)(resources.GetObject("runButton.Visible")));
			this.runButton.Click += new System.EventHandler(this.runButton_Click);
			// 
			// suiteName
			// 
			this.suiteName.AccessibleDescription = resources.GetString("suiteName.AccessibleDescription");
			this.suiteName.AccessibleName = resources.GetString("suiteName.AccessibleName");
			this.suiteName.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("suiteName.Anchor")));
			this.suiteName.AutoSize = ((bool)(resources.GetObject("suiteName.AutoSize")));
			this.suiteName.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("suiteName.Dock")));
			this.suiteName.Enabled = ((bool)(resources.GetObject("suiteName.Enabled")));
			this.suiteName.Font = ((System.Drawing.Font)(resources.GetObject("suiteName.Font")));
			this.suiteName.Image = ((System.Drawing.Image)(resources.GetObject("suiteName.Image")));
			this.suiteName.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("suiteName.ImageAlign")));
			this.suiteName.ImageIndex = ((int)(resources.GetObject("suiteName.ImageIndex")));
			this.suiteName.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("suiteName.ImeMode")));
			this.suiteName.Location = ((System.Drawing.Point)(resources.GetObject("suiteName.Location")));
			this.suiteName.Name = "suiteName";
			this.suiteName.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("suiteName.RightToLeft")));
			this.suiteName.Size = ((System.Drawing.Size)(resources.GetObject("suiteName.Size")));
			this.suiteName.TabIndex = ((int)(resources.GetObject("suiteName.TabIndex")));
			this.suiteName.Text = resources.GetString("suiteName.Text");
			this.suiteName.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("suiteName.TextAlign")));
			this.toolTip.SetToolTip(this.suiteName, resources.GetString("suiteName.ToolTip"));
			this.suiteName.Visible = ((bool)(resources.GetObject("suiteName.Visible")));
			// 
			// progressBar
			// 
			this.progressBar.AccessibleDescription = resources.GetString("progressBar.AccessibleDescription");
			this.progressBar.AccessibleName = resources.GetString("progressBar.AccessibleName");
			this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("progressBar.Anchor")));
			this.progressBar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("progressBar.BackgroundImage")));
			this.progressBar.CausesValidation = false;
			this.progressBar.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("progressBar.Dock")));
			this.progressBar.Enabled = ((bool)(resources.GetObject("progressBar.Enabled")));
			this.progressBar.Font = ((System.Drawing.Font)(resources.GetObject("progressBar.Font")));
			this.progressBar.ForeColor = System.Drawing.SystemColors.Highlight;
			this.progressBar.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("progressBar.ImeMode")));
			this.progressBar.Location = ((System.Drawing.Point)(resources.GetObject("progressBar.Location")));
			this.progressBar.Maximum = 100;
			this.progressBar.Minimum = 0;
			this.progressBar.Name = "progressBar";
			this.progressBar.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("progressBar.RightToLeft")));
			this.progressBar.Size = ((System.Drawing.Size)(resources.GetObject("progressBar.Size")));
			this.progressBar.Step = 1;
			this.progressBar.TabIndex = ((int)(resources.GetObject("progressBar.TabIndex")));
			this.progressBar.Text = resources.GetString("progressBar.Text");
			this.toolTip.SetToolTip(this.progressBar, resources.GetString("progressBar.ToolTip"));
			this.progressBar.Value = 0;
			this.progressBar.Visible = ((bool)(resources.GetObject("progressBar.Visible")));
			// 
			// detailListContextMenu
			// 
			this.detailListContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																								  this.copyDetailMenuItem});
			this.detailListContextMenu.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("detailListContextMenu.RightToLeft")));
			// 
			// copyDetailMenuItem
			// 
			this.copyDetailMenuItem.Enabled = ((bool)(resources.GetObject("copyDetailMenuItem.Enabled")));
			this.copyDetailMenuItem.Index = 0;
			this.copyDetailMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("copyDetailMenuItem.Shortcut")));
			this.copyDetailMenuItem.ShowShortcut = ((bool)(resources.GetObject("copyDetailMenuItem.ShowShortcut")));
			this.copyDetailMenuItem.Text = resources.GetString("copyDetailMenuItem.Text");
			this.copyDetailMenuItem.Visible = ((bool)(resources.GetObject("copyDetailMenuItem.Visible")));
			this.copyDetailMenuItem.Click += new System.EventHandler(this.copyDetailMenuItem_Click);
			// 
			// testTree
			// 
			this.testTree.AccessibleDescription = resources.GetString("testTree.AccessibleDescription");
			this.testTree.AccessibleName = resources.GetString("testTree.AccessibleName");
			this.testTree.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("testTree.Anchor")));
			this.testTree.AutoScroll = ((bool)(resources.GetObject("testTree.AutoScroll")));
			this.testTree.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("testTree.AutoScrollMargin")));
			this.testTree.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("testTree.AutoScrollMinSize")));
			this.testTree.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("testTree.BackgroundImage")));
			this.testTree.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("testTree.Dock")));
			this.testTree.Enabled = ((bool)(resources.GetObject("testTree.Enabled")));
			this.testTree.Font = ((System.Drawing.Font)(resources.GetObject("testTree.Font")));
			this.testTree.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("testTree.ImeMode")));
			this.testTree.InitialDisplay = NUnit.UiKit.TestSuiteTreeView.DisplayStyle.Auto;
			this.testTree.Location = ((System.Drawing.Point)(resources.GetObject("testTree.Location")));
			this.testTree.Name = "testTree";
			this.testTree.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("testTree.RightToLeft")));
			this.testTree.ShowCheckBoxes = true;
			this.testTree.Size = ((System.Drawing.Size)(resources.GetObject("testTree.Size")));
			this.testTree.TabIndex = ((int)(resources.GetObject("testTree.TabIndex")));
			this.toolTip.SetToolTip(this.testTree, resources.GetString("testTree.ToolTip"));
			this.testTree.Visible = ((bool)(resources.GetObject("testTree.Visible")));
			this.testTree.VisualStudioSupport = false;
			this.testTree.SelectedTestsChanged += new NUnit.UiKit.SelectedTestsChangedEventHandler(this.testTree_SelectedTestsChanged);
			// 
			// panel2
			// 
			this.panel2.AccessibleDescription = resources.GetString("panel2.AccessibleDescription");
			this.panel2.AccessibleName = resources.GetString("panel2.AccessibleName");
			this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("panel2.Anchor")));
			this.panel2.AutoScroll = ((bool)(resources.GetObject("panel2.AutoScroll")));
			this.panel2.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("panel2.AutoScrollMargin")));
			this.panel2.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("panel2.AutoScrollMinSize")));
			this.panel2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel2.BackgroundImage")));
			this.panel2.Controls.Add(this.testTree);
			this.panel2.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("panel2.Dock")));
			this.panel2.Enabled = ((bool)(resources.GetObject("panel2.Enabled")));
			this.panel2.Font = ((System.Drawing.Font)(resources.GetObject("panel2.Font")));
			this.panel2.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("panel2.ImeMode")));
			this.panel2.Location = ((System.Drawing.Point)(resources.GetObject("panel2.Location")));
			this.panel2.Name = "panel2";
			this.panel2.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("panel2.RightToLeft")));
			this.panel2.Size = ((System.Drawing.Size)(resources.GetObject("panel2.Size")));
			this.panel2.TabIndex = ((int)(resources.GetObject("panel2.TabIndex")));
			this.panel2.Text = resources.GetString("panel2.Text");
			this.toolTip.SetToolTip(this.panel2, resources.GetString("panel2.ToolTip"));
			this.panel2.Visible = ((bool)(resources.GetObject("panel2.Visible")));
			// 
			// NUnitForm
			// 
			this.AcceptButton = this.runButton;
			this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
			this.AccessibleName = resources.GetString("$this.AccessibleName");
			this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
			this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
			this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
			this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.statusBar);
			this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
			this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
			this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
			this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
			this.Menu = this.mainMenu;
			this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
			this.Name = "NUnitForm";
			this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
			this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
			this.Text = resources.GetString("$this.Text");
			this.toolTip.SetToolTip(this, resources.GetString("$this.ToolTip"));
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

			UserSettings.Options.ShowCheckBoxes = testTree.ShowCheckBoxes;
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
			ITestEvents events = TestLoader.Events;

			events.RunStarting += new TestEventHandler( OnRunStarting );
			events.RunFinished += new TestEventHandler( OnRunFinished );

			events.ProjectLoaded	+= new TestEventHandler( OnTestProjectLoaded );
			events.ProjectLoadFailed+= new TestEventHandler( OnProjectLoadFailure );
			events.ProjectUnloading += new TestEventHandler( OnTestProjectUnloading );
			events.ProjectUnloaded	+= new TestEventHandler( OnTestProjectUnloaded );

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
			testTree.ClearResultsOnChange = 
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

		private void OnTestProjectLoaded( object sender, TestEventArgs e )
		{
			SetTitleBar( e.Name );
			projectMenu.Visible = true;
		}

		private void OnTestProjectUnloading( object sender, TestEventArgs e )
		{
			if ( e.Name != null && File.Exists( e.Name ) )
				UserSettings.RecentProjects.RecentFile = e.Name;
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
			runButton.Enabled = true;
			ClearTabs();
			
			Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
			if ( TestLoader.FrameworkVersion == null )
			{
				UserMessage.Display( "This assembly was not built with the NUnit framework and contains no tests.", "Not a Test Assembly");
			}
			else if ( TestLoader.FrameworkVersion != currentVersion )
			{
				string msg = string.Format( "The {0} assembly uses version {1} of the NUnit framework.\r\rIf problems arise, try changing the reference to version {2} and rebuilding the assembly",
					Path.GetFileNameWithoutExtension( e.Name ), TestLoader.FrameworkVersion, currentVersion );
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

		private void OnProjectLoadFailure( object sender, TestEventArgs e )
		{
			UserMessage.DisplayFailure( e.Exception, "Project Not Loaded" );

			UserSettings.RecentProjects.Remove( e.Name );

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
			suiteName.Text = e.Test.Name;
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

			errWriter.Clear();
			outWriter.Clear();
			
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
			//string s = item.ToString();
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

		private void OnMouseHover(object sender, System.EventArgs e)
		{
			if ( tipWindow != null ) tipWindow.Close();

			if ( hoverIndex >= 0 && hoverIndex < detailList.Items.Count )
			{
				Graphics g = Graphics.FromHwnd( detailList.Handle );

				Rectangle itemRect = detailList.GetItemRectangle( hoverIndex );
				string text = detailList.Items[hoverIndex].ToString();

				SizeF sizeNeeded = g.MeasureString( text, detailList.Font );
				bool expansionNeeded = 
					itemRect.Width < (int)sizeNeeded.Width ||
					itemRect.Height < (int)sizeNeeded.Height;

				if ( expansionNeeded )
				{
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
		}

		private void tipWindow_Closed( object sender, System.EventArgs e )
		{
			tipWindow = null;
			hoverIndex = -1;
			ClearTimer();
		}

		private void detailList_MouseLeave(object sender, System.EventArgs e)
		{
			hoverIndex = -1;
			ClearTimer();
		}

		private void detailList_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			ClearTimer();

			hoverIndex = detailList.IndexFromPoint( e.X, e.Y );	

			if ( hoverIndex >= 0 && hoverIndex < detailList.Items.Count )
			{
				// Workaround problem of IndexFromPoint returning an
				// index when mouse is over bottom part of list.
				Rectangle r = detailList.GetItemRectangle( hoverIndex );
				if ( e.Y > r.Bottom )
					hoverIndex = -1;
				else
				{
					hoverTimer = new System.Windows.Forms.Timer();
					hoverTimer.Interval = 800;
					hoverTimer.Tick += new EventHandler( OnMouseHover );
					hoverTimer.Start();
				}
			}
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

		private void testTree_SelectedTestsChanged(object sender, SelectedTestsChangedEventArgs e)
		{
			if (!IsTestRunning) 
			{
				suiteName.Text = e.TestName;
				statusBar.Initialize(e.TestCount, e.TestName );
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

