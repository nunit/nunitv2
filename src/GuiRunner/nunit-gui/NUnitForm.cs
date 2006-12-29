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
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Text;

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

		private RecentFiles recentFilesService;
		private ISettings userSettings;

		private string displayFormat = "Full";

		// Structure used for command line options
		public struct CommandLineOptions
		{
			public string testFileName;
			public string configName;
			public string testName;
			public string categories;
			public bool exclude;
			public bool noload;
			public bool autorun;
		}

		// Our current run command line options
		private CommandLineOptions commandLineOptions;

		// The currently executing test, used in reporting exceptions
        private string currentTestName;

		private System.ComponentModel.IContainer components;

		private System.Windows.Forms.Panel leftPanel;
		public System.Windows.Forms.Splitter treeSplitter;
		public System.Windows.Forms.Panel rightPanel;

		private TestTree testTree;

		public System.Windows.Forms.GroupBox groupBox1;
		public System.Windows.Forms.Button runButton;
		private System.Windows.Forms.Button stopButton;
		public System.Windows.Forms.Label suiteName;
		public NUnit.UiKit.TestProgressBar progressBar;
		private System.Windows.Forms.Label runCount;

		public NUnit.UiKit.ResultTabs resultTabs;

		public NUnit.UiKit.StatusBar statusBar;

		public System.Windows.Forms.ToolTip toolTip;

		public System.Windows.Forms.MainMenu mainMenu;
		
		public System.Windows.Forms.MenuItem fileMenu;
		private System.Windows.Forms.MenuItem saveMenuItem;
		private System.Windows.Forms.MenuItem saveAsMenuItem;
		private System.Windows.Forms.MenuItem newMenuItem;
		private System.Windows.Forms.MenuItem openMenuItem;
		private System.Windows.Forms.MenuItem recentProjectsMenu;
		private System.Windows.Forms.MenuItem fileMenuSeparator1;
		private System.Windows.Forms.MenuItem fileMenuSeparator2;
		public System.Windows.Forms.MenuItem fileMenuSeparator4;
		private System.Windows.Forms.MenuItem closeMenuItem;
		private System.Windows.Forms.MenuItem reloadMenuItem;
		public System.Windows.Forms.MenuItem exitMenuItem;

		private System.Windows.Forms.MenuItem projectMenu;
		private System.Windows.Forms.MenuItem editProjectMenuItem;
		private System.Windows.Forms.MenuItem configMenuItem;
		private System.Windows.Forms.MenuItem projectMenuSeparator1;
		private System.Windows.Forms.MenuItem projectMenuSeparator2;

		private System.Windows.Forms.MenuItem toolsMenu;
		private System.Windows.Forms.MenuItem optionsMenuItem;
		private System.Windows.Forms.MenuItem saveXmlResultsMenuItem;
		private System.Windows.Forms.MenuItem toolsMenuSeparator1;

		public System.Windows.Forms.MenuItem helpMenuItem;
		public System.Windows.Forms.MenuItem helpItem;
		public System.Windows.Forms.MenuItem helpMenuSeparator1;
		public System.Windows.Forms.MenuItem aboutMenuItem;

		private System.Windows.Forms.MenuItem addVSProjectMenuItem;
		private System.Windows.Forms.MenuItem exceptionDetailsMenuItem;
		private System.Windows.Forms.MenuItem viewMenu;
		private System.Windows.Forms.MenuItem statusBarMenuItem;
		private System.Windows.Forms.MenuItem toolsMenuSeparator2;
		private System.Windows.Forms.MenuItem miniGuiMenuItem;
		private System.Windows.Forms.MenuItem fullGuiMenuItem;
		private System.Windows.Forms.MenuItem fontMenuItem;
		private System.Windows.Forms.MenuItem fontChangeMenuItem;
		private System.Windows.Forms.MenuItem defaultFontMenuItem;
		private System.Windows.Forms.MenuItem decreaseFontMenuItem;
		private System.Windows.Forms.MenuItem increaseFontMenuItem;
		private System.Windows.Forms.MenuItem testMenu;
		private System.Windows.Forms.MenuItem runAllMenuItem;
		private System.Windows.Forms.MenuItem runSelectedMenuItem;
		private System.Windows.Forms.MenuItem runFailedMenuItem;
		private System.Windows.Forms.MenuItem stopRunMenuItem;
		private System.Windows.Forms.MenuItem assemblyDetailsMenuItem;
		private System.Windows.Forms.MenuItem addinInfoMenuItem;
		private System.Windows.Forms.MenuItem viewMenuSeparator1;
		private System.Windows.Forms.MenuItem viewMenuSeparator2;
		private System.Windows.Forms.MenuItem viewMenuSeparator3;
		private System.Windows.Forms.MenuItem viewMenuSeparator4;
		private System.Windows.Forms.MenuItem fontMenuSeparator;
		private System.Windows.Forms.MenuItem testMenuSeparator;
		private System.Windows.Forms.MenuItem addAssemblyMenuItem;

		#endregion
		
		#region Construction and Disposal

		public NUnitForm( CommandLineOptions commandLineOptions )
		{
			InitializeComponent();

			this.commandLineOptions = commandLineOptions;
			this.recentFilesService = NUnit.Util.Services.RecentFiles;
			this.userSettings = NUnit.Util.Services.UserSettings;
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
			this.fullGuiMenuItem = new System.Windows.Forms.MenuItem();
			this.miniGuiMenuItem = new System.Windows.Forms.MenuItem();
			this.viewMenuSeparator1 = new System.Windows.Forms.MenuItem();
			this.viewMenuSeparator2 = new System.Windows.Forms.MenuItem();
			this.fontMenuItem = new System.Windows.Forms.MenuItem();
			this.increaseFontMenuItem = new System.Windows.Forms.MenuItem();
			this.decreaseFontMenuItem = new System.Windows.Forms.MenuItem();
			this.fontMenuSeparator = new System.Windows.Forms.MenuItem();
			this.fontChangeMenuItem = new System.Windows.Forms.MenuItem();
			this.defaultFontMenuItem = new System.Windows.Forms.MenuItem();
			this.viewMenuSeparator3 = new System.Windows.Forms.MenuItem();
			this.assemblyDetailsMenuItem = new System.Windows.Forms.MenuItem();
			this.viewMenuSeparator4 = new System.Windows.Forms.MenuItem();
			this.statusBarMenuItem = new System.Windows.Forms.MenuItem();
			this.projectMenu = new System.Windows.Forms.MenuItem();
			this.configMenuItem = new System.Windows.Forms.MenuItem();
			this.projectMenuSeparator1 = new System.Windows.Forms.MenuItem();
			this.addAssemblyMenuItem = new System.Windows.Forms.MenuItem();
			this.addVSProjectMenuItem = new System.Windows.Forms.MenuItem();
			this.projectMenuSeparator2 = new System.Windows.Forms.MenuItem();
			this.editProjectMenuItem = new System.Windows.Forms.MenuItem();
			this.testMenu = new System.Windows.Forms.MenuItem();
			this.runAllMenuItem = new System.Windows.Forms.MenuItem();
			this.runSelectedMenuItem = new System.Windows.Forms.MenuItem();
			this.runFailedMenuItem = new System.Windows.Forms.MenuItem();
			this.testMenuSeparator = new System.Windows.Forms.MenuItem();
			this.stopRunMenuItem = new System.Windows.Forms.MenuItem();
			this.toolsMenu = new System.Windows.Forms.MenuItem();
			this.saveXmlResultsMenuItem = new System.Windows.Forms.MenuItem();
			this.exceptionDetailsMenuItem = new System.Windows.Forms.MenuItem();
			this.toolsMenuSeparator1 = new System.Windows.Forms.MenuItem();
			this.optionsMenuItem = new System.Windows.Forms.MenuItem();
			this.toolsMenuSeparator2 = new System.Windows.Forms.MenuItem();
			this.addinInfoMenuItem = new System.Windows.Forms.MenuItem();
			this.helpItem = new System.Windows.Forms.MenuItem();
			this.helpMenuItem = new System.Windows.Forms.MenuItem();
			this.helpMenuSeparator1 = new System.Windows.Forms.MenuItem();
			this.aboutMenuItem = new System.Windows.Forms.MenuItem();
			this.treeSplitter = new System.Windows.Forms.Splitter();
			this.rightPanel = new System.Windows.Forms.Panel();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.runCount = new System.Windows.Forms.Label();
			this.stopButton = new System.Windows.Forms.Button();
			this.runButton = new System.Windows.Forms.Button();
			this.suiteName = new System.Windows.Forms.Label();
			this.progressBar = new NUnit.UiKit.TestProgressBar();
			this.resultTabs = new NUnit.UiKit.ResultTabs();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.testTree = new NUnit.UiKit.TestTree();
			this.leftPanel = new System.Windows.Forms.Panel();
			this.rightPanel.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.leftPanel.SuspendLayout();
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
																					 this.viewMenu,
																					 this.projectMenu,
																					 this.testMenu,
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
			// viewMenu
			// 
			this.viewMenu.Enabled = ((bool)(resources.GetObject("viewMenu.Enabled")));
			this.viewMenu.Index = 1;
			this.viewMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.fullGuiMenuItem,
																					 this.miniGuiMenuItem,
																					 this.viewMenuSeparator1,
																					 this.viewMenuSeparator2,
																					 this.fontMenuItem,
																					 this.viewMenuSeparator3,
																					 this.assemblyDetailsMenuItem,
																					 this.viewMenuSeparator4,
																					 this.statusBarMenuItem});
			this.viewMenu.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("viewMenu.Shortcut")));
			this.viewMenu.ShowShortcut = ((bool)(resources.GetObject("viewMenu.ShowShortcut")));
			this.viewMenu.Text = resources.GetString("viewMenu.Text");
			this.viewMenu.Visible = ((bool)(resources.GetObject("viewMenu.Visible")));
			this.viewMenu.Popup += new System.EventHandler(this.viewMenu_Popup);
			// 
			// fullGuiMenuItem
			// 
			this.fullGuiMenuItem.Checked = true;
			this.fullGuiMenuItem.Enabled = ((bool)(resources.GetObject("fullGuiMenuItem.Enabled")));
			this.fullGuiMenuItem.Index = 0;
			this.fullGuiMenuItem.RadioCheck = true;
			this.fullGuiMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("fullGuiMenuItem.Shortcut")));
			this.fullGuiMenuItem.ShowShortcut = ((bool)(resources.GetObject("fullGuiMenuItem.ShowShortcut")));
			this.fullGuiMenuItem.Text = resources.GetString("fullGuiMenuItem.Text");
			this.fullGuiMenuItem.Visible = ((bool)(resources.GetObject("fullGuiMenuItem.Visible")));
			this.fullGuiMenuItem.Click += new System.EventHandler(this.fullGuiMenuItem_Click);
			// 
			// miniGuiMenuItem
			// 
			this.miniGuiMenuItem.Enabled = ((bool)(resources.GetObject("miniGuiMenuItem.Enabled")));
			this.miniGuiMenuItem.Index = 1;
			this.miniGuiMenuItem.RadioCheck = true;
			this.miniGuiMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("miniGuiMenuItem.Shortcut")));
			this.miniGuiMenuItem.ShowShortcut = ((bool)(resources.GetObject("miniGuiMenuItem.ShowShortcut")));
			this.miniGuiMenuItem.Text = resources.GetString("miniGuiMenuItem.Text");
			this.miniGuiMenuItem.Visible = ((bool)(resources.GetObject("miniGuiMenuItem.Visible")));
			this.miniGuiMenuItem.Click += new System.EventHandler(this.miniGuiMenuItem_Click);
			// 
			// viewMenuSeparator1
			// 
			this.viewMenuSeparator1.Enabled = ((bool)(resources.GetObject("viewMenuSeparator1.Enabled")));
			this.viewMenuSeparator1.Index = 2;
			this.viewMenuSeparator1.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("viewMenuSeparator1.Shortcut")));
			this.viewMenuSeparator1.ShowShortcut = ((bool)(resources.GetObject("viewMenuSeparator1.ShowShortcut")));
			this.viewMenuSeparator1.Text = resources.GetString("viewMenuSeparator1.Text");
			this.viewMenuSeparator1.Visible = ((bool)(resources.GetObject("viewMenuSeparator1.Visible")));
			// 
			// viewMenuSeparator2
			// 
			this.viewMenuSeparator2.Enabled = ((bool)(resources.GetObject("viewMenuSeparator2.Enabled")));
			this.viewMenuSeparator2.Index = 3;
			this.viewMenuSeparator2.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("viewMenuSeparator2.Shortcut")));
			this.viewMenuSeparator2.ShowShortcut = ((bool)(resources.GetObject("viewMenuSeparator2.ShowShortcut")));
			this.viewMenuSeparator2.Text = resources.GetString("viewMenuSeparator2.Text");
			this.viewMenuSeparator2.Visible = ((bool)(resources.GetObject("viewMenuSeparator2.Visible")));
			// 
			// fontMenuItem
			// 
			this.fontMenuItem.Enabled = ((bool)(resources.GetObject("fontMenuItem.Enabled")));
			this.fontMenuItem.Index = 4;
			this.fontMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.increaseFontMenuItem,
																						 this.decreaseFontMenuItem,
																						 this.fontMenuSeparator,
																						 this.fontChangeMenuItem,
																						 this.defaultFontMenuItem});
			this.fontMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("fontMenuItem.Shortcut")));
			this.fontMenuItem.ShowShortcut = ((bool)(resources.GetObject("fontMenuItem.ShowShortcut")));
			this.fontMenuItem.Text = resources.GetString("fontMenuItem.Text");
			this.fontMenuItem.Visible = ((bool)(resources.GetObject("fontMenuItem.Visible")));
			// 
			// increaseFontMenuItem
			// 
			this.increaseFontMenuItem.Enabled = ((bool)(resources.GetObject("increaseFontMenuItem.Enabled")));
			this.increaseFontMenuItem.Index = 0;
			this.increaseFontMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("increaseFontMenuItem.Shortcut")));
			this.increaseFontMenuItem.ShowShortcut = ((bool)(resources.GetObject("increaseFontMenuItem.ShowShortcut")));
			this.increaseFontMenuItem.Text = resources.GetString("increaseFontMenuItem.Text");
			this.increaseFontMenuItem.Visible = ((bool)(resources.GetObject("increaseFontMenuItem.Visible")));
			this.increaseFontMenuItem.Click += new System.EventHandler(this.increaseFontMenuItem_Click);
			// 
			// decreaseFontMenuItem
			// 
			this.decreaseFontMenuItem.Enabled = ((bool)(resources.GetObject("decreaseFontMenuItem.Enabled")));
			this.decreaseFontMenuItem.Index = 1;
			this.decreaseFontMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("decreaseFontMenuItem.Shortcut")));
			this.decreaseFontMenuItem.ShowShortcut = ((bool)(resources.GetObject("decreaseFontMenuItem.ShowShortcut")));
			this.decreaseFontMenuItem.Text = resources.GetString("decreaseFontMenuItem.Text");
			this.decreaseFontMenuItem.Visible = ((bool)(resources.GetObject("decreaseFontMenuItem.Visible")));
			this.decreaseFontMenuItem.Click += new System.EventHandler(this.decreaseFontMenuItem_Click);
			// 
			// fontMenuSeparator
			// 
			this.fontMenuSeparator.Enabled = ((bool)(resources.GetObject("fontMenuSeparator.Enabled")));
			this.fontMenuSeparator.Index = 2;
			this.fontMenuSeparator.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("fontMenuSeparator.Shortcut")));
			this.fontMenuSeparator.ShowShortcut = ((bool)(resources.GetObject("fontMenuSeparator.ShowShortcut")));
			this.fontMenuSeparator.Text = resources.GetString("fontMenuSeparator.Text");
			this.fontMenuSeparator.Visible = ((bool)(resources.GetObject("fontMenuSeparator.Visible")));
			// 
			// fontChangeMenuItem
			// 
			this.fontChangeMenuItem.Enabled = ((bool)(resources.GetObject("fontChangeMenuItem.Enabled")));
			this.fontChangeMenuItem.Index = 3;
			this.fontChangeMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("fontChangeMenuItem.Shortcut")));
			this.fontChangeMenuItem.ShowShortcut = ((bool)(resources.GetObject("fontChangeMenuItem.ShowShortcut")));
			this.fontChangeMenuItem.Text = resources.GetString("fontChangeMenuItem.Text");
			this.fontChangeMenuItem.Visible = ((bool)(resources.GetObject("fontChangeMenuItem.Visible")));
			this.fontChangeMenuItem.Click += new System.EventHandler(this.fontChangeMenuItem_Click);
			// 
			// defaultFontMenuItem
			// 
			this.defaultFontMenuItem.Enabled = ((bool)(resources.GetObject("defaultFontMenuItem.Enabled")));
			this.defaultFontMenuItem.Index = 4;
			this.defaultFontMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("defaultFontMenuItem.Shortcut")));
			this.defaultFontMenuItem.ShowShortcut = ((bool)(resources.GetObject("defaultFontMenuItem.ShowShortcut")));
			this.defaultFontMenuItem.Text = resources.GetString("defaultFontMenuItem.Text");
			this.defaultFontMenuItem.Visible = ((bool)(resources.GetObject("defaultFontMenuItem.Visible")));
			this.defaultFontMenuItem.Click += new System.EventHandler(this.defaultFontMenuItem_Click);
			// 
			// viewMenuSeparator3
			// 
			this.viewMenuSeparator3.Enabled = ((bool)(resources.GetObject("viewMenuSeparator3.Enabled")));
			this.viewMenuSeparator3.Index = 5;
			this.viewMenuSeparator3.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("viewMenuSeparator3.Shortcut")));
			this.viewMenuSeparator3.ShowShortcut = ((bool)(resources.GetObject("viewMenuSeparator3.ShowShortcut")));
			this.viewMenuSeparator3.Text = resources.GetString("viewMenuSeparator3.Text");
			this.viewMenuSeparator3.Visible = ((bool)(resources.GetObject("viewMenuSeparator3.Visible")));
			// 
			// assemblyDetailsMenuItem
			// 
			this.assemblyDetailsMenuItem.Enabled = ((bool)(resources.GetObject("assemblyDetailsMenuItem.Enabled")));
			this.assemblyDetailsMenuItem.Index = 6;
			this.assemblyDetailsMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("assemblyDetailsMenuItem.Shortcut")));
			this.assemblyDetailsMenuItem.ShowShortcut = ((bool)(resources.GetObject("assemblyDetailsMenuItem.ShowShortcut")));
			this.assemblyDetailsMenuItem.Text = resources.GetString("assemblyDetailsMenuItem.Text");
			this.assemblyDetailsMenuItem.Visible = ((bool)(resources.GetObject("assemblyDetailsMenuItem.Visible")));
			this.assemblyDetailsMenuItem.Click += new System.EventHandler(this.assemblyDetailsMenuItem_Click);
			// 
			// viewMenuSeparator4
			// 
			this.viewMenuSeparator4.Enabled = ((bool)(resources.GetObject("viewMenuSeparator4.Enabled")));
			this.viewMenuSeparator4.Index = 7;
			this.viewMenuSeparator4.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("viewMenuSeparator4.Shortcut")));
			this.viewMenuSeparator4.ShowShortcut = ((bool)(resources.GetObject("viewMenuSeparator4.ShowShortcut")));
			this.viewMenuSeparator4.Text = resources.GetString("viewMenuSeparator4.Text");
			this.viewMenuSeparator4.Visible = ((bool)(resources.GetObject("viewMenuSeparator4.Visible")));
			// 
			// statusBarMenuItem
			// 
			this.statusBarMenuItem.Checked = true;
			this.statusBarMenuItem.Enabled = ((bool)(resources.GetObject("statusBarMenuItem.Enabled")));
			this.statusBarMenuItem.Index = 8;
			this.statusBarMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("statusBarMenuItem.Shortcut")));
			this.statusBarMenuItem.ShowShortcut = ((bool)(resources.GetObject("statusBarMenuItem.ShowShortcut")));
			this.statusBarMenuItem.Text = resources.GetString("statusBarMenuItem.Text");
			this.statusBarMenuItem.Visible = ((bool)(resources.GetObject("statusBarMenuItem.Visible")));
			this.statusBarMenuItem.Click += new System.EventHandler(this.statusBarMenuItem_Click);
			// 
			// projectMenu
			// 
			this.projectMenu.Enabled = ((bool)(resources.GetObject("projectMenu.Enabled")));
			this.projectMenu.Index = 2;
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
			// testMenu
			// 
			this.testMenu.Enabled = ((bool)(resources.GetObject("testMenu.Enabled")));
			this.testMenu.Index = 3;
			this.testMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.runAllMenuItem,
																					 this.runSelectedMenuItem,
																					 this.runFailedMenuItem,
																					 this.testMenuSeparator,
																					 this.stopRunMenuItem});
			this.testMenu.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("testMenu.Shortcut")));
			this.testMenu.ShowShortcut = ((bool)(resources.GetObject("testMenu.ShowShortcut")));
			this.testMenu.Text = resources.GetString("testMenu.Text");
			this.testMenu.Visible = ((bool)(resources.GetObject("testMenu.Visible")));
			// 
			// runAllMenuItem
			// 
			this.runAllMenuItem.Enabled = ((bool)(resources.GetObject("runAllMenuItem.Enabled")));
			this.runAllMenuItem.Index = 0;
			this.runAllMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("runAllMenuItem.Shortcut")));
			this.runAllMenuItem.ShowShortcut = ((bool)(resources.GetObject("runAllMenuItem.ShowShortcut")));
			this.runAllMenuItem.Text = resources.GetString("runAllMenuItem.Text");
			this.runAllMenuItem.Visible = ((bool)(resources.GetObject("runAllMenuItem.Visible")));
			this.runAllMenuItem.Click += new System.EventHandler(this.runAllMenuItem_Click);
			// 
			// runSelectedMenuItem
			// 
			this.runSelectedMenuItem.Enabled = ((bool)(resources.GetObject("runSelectedMenuItem.Enabled")));
			this.runSelectedMenuItem.Index = 1;
			this.runSelectedMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("runSelectedMenuItem.Shortcut")));
			this.runSelectedMenuItem.ShowShortcut = ((bool)(resources.GetObject("runSelectedMenuItem.ShowShortcut")));
			this.runSelectedMenuItem.Text = resources.GetString("runSelectedMenuItem.Text");
			this.runSelectedMenuItem.Visible = ((bool)(resources.GetObject("runSelectedMenuItem.Visible")));
			this.runSelectedMenuItem.Click += new System.EventHandler(this.runSelectedMenuItem_Click);
			// 
			// runFailedMenuItem
			// 
			this.runFailedMenuItem.Enabled = ((bool)(resources.GetObject("runFailedMenuItem.Enabled")));
			this.runFailedMenuItem.Index = 2;
			this.runFailedMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("runFailedMenuItem.Shortcut")));
			this.runFailedMenuItem.ShowShortcut = ((bool)(resources.GetObject("runFailedMenuItem.ShowShortcut")));
			this.runFailedMenuItem.Text = resources.GetString("runFailedMenuItem.Text");
			this.runFailedMenuItem.Visible = ((bool)(resources.GetObject("runFailedMenuItem.Visible")));
			this.runFailedMenuItem.Click += new System.EventHandler(this.runFailedMenuItem_Click);
			// 
			// testMenuSeparator
			// 
			this.testMenuSeparator.Enabled = ((bool)(resources.GetObject("testMenuSeparator.Enabled")));
			this.testMenuSeparator.Index = 3;
			this.testMenuSeparator.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("testMenuSeparator.Shortcut")));
			this.testMenuSeparator.ShowShortcut = ((bool)(resources.GetObject("testMenuSeparator.ShowShortcut")));
			this.testMenuSeparator.Text = resources.GetString("testMenuSeparator.Text");
			this.testMenuSeparator.Visible = ((bool)(resources.GetObject("testMenuSeparator.Visible")));
			// 
			// stopRunMenuItem
			// 
			this.stopRunMenuItem.Enabled = ((bool)(resources.GetObject("stopRunMenuItem.Enabled")));
			this.stopRunMenuItem.Index = 4;
			this.stopRunMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("stopRunMenuItem.Shortcut")));
			this.stopRunMenuItem.ShowShortcut = ((bool)(resources.GetObject("stopRunMenuItem.ShowShortcut")));
			this.stopRunMenuItem.Text = resources.GetString("stopRunMenuItem.Text");
			this.stopRunMenuItem.Visible = ((bool)(resources.GetObject("stopRunMenuItem.Visible")));
			this.stopRunMenuItem.Click += new System.EventHandler(this.stopRunMenuItem_Click);
			// 
			// toolsMenu
			// 
			this.toolsMenu.Enabled = ((bool)(resources.GetObject("toolsMenu.Enabled")));
			this.toolsMenu.Index = 4;
			this.toolsMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.saveXmlResultsMenuItem,
																					  this.exceptionDetailsMenuItem,
																					  this.toolsMenuSeparator1,
																					  this.optionsMenuItem,
																					  this.toolsMenuSeparator2,
																					  this.addinInfoMenuItem});
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
			// toolsMenuSeparator2
			// 
			this.toolsMenuSeparator2.Enabled = ((bool)(resources.GetObject("toolsMenuSeparator2.Enabled")));
			this.toolsMenuSeparator2.Index = 4;
			this.toolsMenuSeparator2.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("toolsMenuSeparator2.Shortcut")));
			this.toolsMenuSeparator2.ShowShortcut = ((bool)(resources.GetObject("toolsMenuSeparator2.ShowShortcut")));
			this.toolsMenuSeparator2.Text = resources.GetString("toolsMenuSeparator2.Text");
			this.toolsMenuSeparator2.Visible = ((bool)(resources.GetObject("toolsMenuSeparator2.Visible")));
			// 
			// addinInfoMenuItem
			// 
			this.addinInfoMenuItem.Enabled = ((bool)(resources.GetObject("addinInfoMenuItem.Enabled")));
			this.addinInfoMenuItem.Index = 5;
			this.addinInfoMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("addinInfoMenuItem.Shortcut")));
			this.addinInfoMenuItem.ShowShortcut = ((bool)(resources.GetObject("addinInfoMenuItem.ShowShortcut")));
			this.addinInfoMenuItem.Text = resources.GetString("addinInfoMenuItem.Text");
			this.addinInfoMenuItem.Visible = ((bool)(resources.GetObject("addinInfoMenuItem.Visible")));
			this.addinInfoMenuItem.Click += new System.EventHandler(this.addinInfoMenuItem_Click);
			// 
			// helpItem
			// 
			this.helpItem.Enabled = ((bool)(resources.GetObject("helpItem.Enabled")));
			this.helpItem.Index = 5;
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
			// treeSplitter
			// 
			this.treeSplitter.AccessibleDescription = resources.GetString("treeSplitter.AccessibleDescription");
			this.treeSplitter.AccessibleName = resources.GetString("treeSplitter.AccessibleName");
			this.treeSplitter.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("treeSplitter.Anchor")));
			this.treeSplitter.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("treeSplitter.BackgroundImage")));
			this.treeSplitter.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("treeSplitter.Dock")));
			this.treeSplitter.Enabled = ((bool)(resources.GetObject("treeSplitter.Enabled")));
			this.treeSplitter.Font = ((System.Drawing.Font)(resources.GetObject("treeSplitter.Font")));
			this.treeSplitter.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("treeSplitter.ImeMode")));
			this.treeSplitter.Location = ((System.Drawing.Point)(resources.GetObject("treeSplitter.Location")));
			this.treeSplitter.MinExtra = ((int)(resources.GetObject("treeSplitter.MinExtra")));
			this.treeSplitter.MinSize = ((int)(resources.GetObject("treeSplitter.MinSize")));
			this.treeSplitter.Name = "treeSplitter";
			this.treeSplitter.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("treeSplitter.RightToLeft")));
			this.treeSplitter.Size = ((System.Drawing.Size)(resources.GetObject("treeSplitter.Size")));
			this.treeSplitter.TabIndex = ((int)(resources.GetObject("treeSplitter.TabIndex")));
			this.treeSplitter.TabStop = false;
			this.toolTip.SetToolTip(this.treeSplitter, resources.GetString("treeSplitter.ToolTip"));
			this.treeSplitter.Visible = ((bool)(resources.GetObject("treeSplitter.Visible")));
			// 
			// rightPanel
			// 
			this.rightPanel.AccessibleDescription = resources.GetString("rightPanel.AccessibleDescription");
			this.rightPanel.AccessibleName = resources.GetString("rightPanel.AccessibleName");
			this.rightPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("rightPanel.Anchor")));
			this.rightPanel.AutoScroll = ((bool)(resources.GetObject("rightPanel.AutoScroll")));
			this.rightPanel.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("rightPanel.AutoScrollMargin")));
			this.rightPanel.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("rightPanel.AutoScrollMinSize")));
			this.rightPanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("rightPanel.BackgroundImage")));
			this.rightPanel.Controls.Add(this.groupBox1);
			this.rightPanel.Controls.Add(this.resultTabs);
			this.rightPanel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("rightPanel.Dock")));
			this.rightPanel.Enabled = ((bool)(resources.GetObject("rightPanel.Enabled")));
			this.rightPanel.Font = ((System.Drawing.Font)(resources.GetObject("rightPanel.Font")));
			this.rightPanel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("rightPanel.ImeMode")));
			this.rightPanel.Location = ((System.Drawing.Point)(resources.GetObject("rightPanel.Location")));
			this.rightPanel.Name = "rightPanel";
			this.rightPanel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("rightPanel.RightToLeft")));
			this.rightPanel.Size = ((System.Drawing.Size)(resources.GetObject("rightPanel.Size")));
			this.rightPanel.TabIndex = ((int)(resources.GetObject("rightPanel.TabIndex")));
			this.rightPanel.Text = resources.GetString("rightPanel.Text");
			this.toolTip.SetToolTip(this.rightPanel, resources.GetString("rightPanel.ToolTip"));
			this.rightPanel.Visible = ((bool)(resources.GetObject("rightPanel.Visible")));
			// 
			// groupBox1
			// 
			this.groupBox1.AccessibleDescription = resources.GetString("groupBox1.AccessibleDescription");
			this.groupBox1.AccessibleName = resources.GetString("groupBox1.AccessibleName");
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("groupBox1.Anchor")));
			this.groupBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("groupBox1.BackgroundImage")));
			this.groupBox1.Controls.Add(this.runCount);
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
			// runCount
			// 
			this.runCount.AccessibleDescription = resources.GetString("runCount.AccessibleDescription");
			this.runCount.AccessibleName = resources.GetString("runCount.AccessibleName");
			this.runCount.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("runCount.Anchor")));
			this.runCount.AutoSize = ((bool)(resources.GetObject("runCount.AutoSize")));
			this.runCount.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("runCount.Dock")));
			this.runCount.Enabled = ((bool)(resources.GetObject("runCount.Enabled")));
			this.runCount.Font = ((System.Drawing.Font)(resources.GetObject("runCount.Font")));
			this.runCount.Image = ((System.Drawing.Image)(resources.GetObject("runCount.Image")));
			this.runCount.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("runCount.ImageAlign")));
			this.runCount.ImageIndex = ((int)(resources.GetObject("runCount.ImageIndex")));
			this.runCount.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("runCount.ImeMode")));
			this.runCount.Location = ((System.Drawing.Point)(resources.GetObject("runCount.Location")));
			this.runCount.Name = "runCount";
			this.runCount.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("runCount.RightToLeft")));
			this.runCount.Size = ((System.Drawing.Size)(resources.GetObject("runCount.Size")));
			this.runCount.TabIndex = ((int)(resources.GetObject("runCount.TabIndex")));
			this.runCount.Text = resources.GetString("runCount.Text");
			this.runCount.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("runCount.TextAlign")));
			this.toolTip.SetToolTip(this.runCount, resources.GetString("runCount.ToolTip"));
			this.runCount.Visible = ((bool)(resources.GetObject("runCount.Visible")));
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
			this.progressBar.BackColor = System.Drawing.SystemColors.Control;
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
			this.progressBar.Segmented = true;
			this.progressBar.Size = ((System.Drawing.Size)(resources.GetObject("progressBar.Size")));
			this.progressBar.Step = 1;
			this.progressBar.TabIndex = ((int)(resources.GetObject("progressBar.TabIndex")));
			this.progressBar.Text = resources.GetString("progressBar.Text");
			this.toolTip.SetToolTip(this.progressBar, resources.GetString("progressBar.ToolTip"));
			this.progressBar.Value = 0;
			this.progressBar.Visible = ((bool)(resources.GetObject("progressBar.Visible")));
			// 
			// resultTabs
			// 
			this.resultTabs.AccessibleDescription = resources.GetString("resultTabs.AccessibleDescription");
			this.resultTabs.AccessibleName = resources.GetString("resultTabs.AccessibleName");
			this.resultTabs.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("resultTabs.Anchor")));
			this.resultTabs.AutoExpand = true;
			this.resultTabs.AutoScroll = ((bool)(resources.GetObject("resultTabs.AutoScroll")));
			this.resultTabs.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("resultTabs.AutoScrollMargin")));
			this.resultTabs.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("resultTabs.AutoScrollMinSize")));
			this.resultTabs.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("resultTabs.BackgroundImage")));
			this.resultTabs.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("resultTabs.Dock")));
			this.resultTabs.Enabled = ((bool)(resources.GetObject("resultTabs.Enabled")));
			this.resultTabs.Font = ((System.Drawing.Font)(resources.GetObject("resultTabs.Font")));
			this.resultTabs.ForeColor = System.Drawing.Color.Red;
			this.resultTabs.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resultTabs.ImeMode")));
			this.resultTabs.Location = ((System.Drawing.Point)(resources.GetObject("resultTabs.Location")));
			this.resultTabs.Name = "resultTabs";
			this.resultTabs.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("resultTabs.RightToLeft")));
			this.resultTabs.Size = ((System.Drawing.Size)(resources.GetObject("resultTabs.Size")));
			this.resultTabs.TabIndex = ((int)(resources.GetObject("resultTabs.TabIndex")));
			this.toolTip.SetToolTip(this.resultTabs, resources.GetString("resultTabs.ToolTip"));
			this.resultTabs.Visible = ((bool)(resources.GetObject("resultTabs.Visible")));
			this.resultTabs.WordWrap = false;
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
			this.testTree.Location = ((System.Drawing.Point)(resources.GetObject("testTree.Location")));
			this.testTree.Name = "testTree";
			this.testTree.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("testTree.RightToLeft")));
			this.testTree.ShowCheckBoxes = false;
			this.testTree.Size = ((System.Drawing.Size)(resources.GetObject("testTree.Size")));
			this.testTree.TabIndex = ((int)(resources.GetObject("testTree.TabIndex")));
			this.toolTip.SetToolTip(this.testTree, resources.GetString("testTree.ToolTip"));
			this.testTree.Visible = ((bool)(resources.GetObject("testTree.Visible")));
			this.testTree.SelectedTestsChanged += new NUnit.UiKit.SelectedTestsChangedEventHandler(this.testTree_SelectedTestsChanged);
			// 
			// leftPanel
			// 
			this.leftPanel.AccessibleDescription = resources.GetString("leftPanel.AccessibleDescription");
			this.leftPanel.AccessibleName = resources.GetString("leftPanel.AccessibleName");
			this.leftPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("leftPanel.Anchor")));
			this.leftPanel.AutoScroll = ((bool)(resources.GetObject("leftPanel.AutoScroll")));
			this.leftPanel.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("leftPanel.AutoScrollMargin")));
			this.leftPanel.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("leftPanel.AutoScrollMinSize")));
			this.leftPanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("leftPanel.BackgroundImage")));
			this.leftPanel.Controls.Add(this.testTree);
			this.leftPanel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("leftPanel.Dock")));
			this.leftPanel.Enabled = ((bool)(resources.GetObject("leftPanel.Enabled")));
			this.leftPanel.Font = ((System.Drawing.Font)(resources.GetObject("leftPanel.Font")));
			this.leftPanel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("leftPanel.ImeMode")));
			this.leftPanel.Location = ((System.Drawing.Point)(resources.GetObject("leftPanel.Location")));
			this.leftPanel.Name = "leftPanel";
			this.leftPanel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("leftPanel.RightToLeft")));
			this.leftPanel.Size = ((System.Drawing.Size)(resources.GetObject("leftPanel.Size")));
			this.leftPanel.TabIndex = ((int)(resources.GetObject("leftPanel.TabIndex")));
			this.leftPanel.Text = resources.GetString("leftPanel.Text");
			this.toolTip.SetToolTip(this.leftPanel, resources.GetString("leftPanel.ToolTip"));
			this.leftPanel.Visible = ((bool)(resources.GetObject("leftPanel.Visible")));
			// 
			// NUnitForm
			// 
			this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
			this.AccessibleName = resources.GetString("$this.AccessibleName");
			this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
			this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
			this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
			this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
			this.Controls.Add(this.rightPanel);
			this.Controls.Add(this.treeSplitter);
			this.Controls.Add(this.leftPanel);
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
			this.rightPanel.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.leftPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		#region Properties used internally

		private TestLoader _testLoader;
		private TestLoader TestLoader
		{
			get
			{ 
				if ( _testLoader == null )
					_testLoader = (TestLoader)GetService( typeof( TestLoader ) );
				return _testLoader;
			}
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
			get { return TestLoader.Running; }
		}

//		private ISettings _userSettings;
//		private ISettings UserSettings
//		{
//			get
//			{
//				if ( _userSettings == null )
//					_userSettings = NUnit.Util.Services.UserSettings;
//				return _userSettings;
//			}
//		}
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
			{
				recentProjectsMenuHandler.Load();
			}
		}

		private void newMenuItem_Click(object sender, System.EventArgs e)
		{
			if ( IsProjectLoaded )
				TestLoaderUI.CloseProject( this );

			TestLoaderUI.NewProject( this );
		}

		private void openMenuItem_Click(object sender, System.EventArgs e)
		{
			TestLoaderUI.OpenProject( this, userSettings.GetSetting( "Options.TestLoader.VisualStudioSupport", false ) );
		}

		private void closeMenuItem_Click(object sender, System.EventArgs e)
		{
			TestLoaderUI.CloseProject( this );
		}

		private void saveMenuItem_Click(object sender, System.EventArgs e)
		{
			TestLoaderUI.SaveProject( this );
		}

		private void saveAsMenuItem_Click(object sender, System.EventArgs e)
		{
			TestLoaderUI.SaveProjectAs( this );
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
		private void statusBarMenuItem_Click(object sender, System.EventArgs e)
		{
			statusBarMenuItem.Checked = !statusBarMenuItem.Checked;
			statusBar.Visible = statusBarMenuItem.Checked;
		}


		private void fontChangeMenuItem_Click(object sender, System.EventArgs e)
		{
			FontDialog fontDialog = new FontDialog();
			fontDialog.FontMustExist = true;
			fontDialog.Font = this.Font;
			fontDialog.MinSize = 6;
			fontDialog.MaxSize = 12;
			fontDialog.AllowVectorFonts = false;
			fontDialog.ScriptsOnly = true;
			fontDialog.ShowEffects = false;
			fontDialog.ShowApply = true;
			fontDialog.Apply += new EventHandler(fontDialog_Apply);
			if( fontDialog.ShowDialog() == DialogResult.OK )
				applyFont( fontDialog.Font );
		}

		private void fontDialog_Apply(object sender, EventArgs e)
		{
			applyFont( ((FontDialog)sender).Font );
		}


		private void defaultFontMenuItem_Click(object sender, System.EventArgs e)
		{
			applyFont( System.Windows.Forms.Form.DefaultFont );
		}

		private void fullGuiMenuItem_Click(object sender, System.EventArgs e)
		{
			if ( !fullGuiMenuItem.Checked )
			{
				fullGuiMenuItem.Checked = true;
				miniGuiMenuItem.Checked = false;
				displayFullGui();
			}
		}

		private void miniGuiMenuItem_Click(object sender, System.EventArgs e)
		{
			if ( !miniGuiMenuItem.Checked )
			{
				miniGuiMenuItem.Checked = true;
				fullGuiMenuItem.Checked = false;
				displayMiniGui();
			}
		}

		private void displayFullGui()
		{
			this.displayFormat = "Full";
			userSettings.SaveSetting( "Gui.DisplayFormat", "Full" );

			this.Controls.Clear();
			leftPanel.Dock = DockStyle.Left;
			this.Controls.Add( rightPanel );
			this.Controls.Add( treeSplitter );
			this.Controls.Add( leftPanel );
			this.Controls.Add( statusBar );

			int x = userSettings.GetSetting( "Gui.MainForm.Left", 10 );
			int y = userSettings.GetSetting( "Gui.MainForm.Top", 10 );
			Point location = new Point( x, y );

			if ( !IsValidLocation( location ) )
				location = new Point( 10, 10 );
			this.Location = location;

			int width = userSettings.GetSetting( "Gui.MainForm.Width", this.Size.Width );
			int height = userSettings.GetSetting( "Gui.MainForm.Height", this.Size.Height );
			if ( width < 160 ) width = 160;
			if ( height < 32 ) height = 32;
			this.Size = new Size( width, height );

			// Set the font to use
			string fontDescription = userSettings.GetSetting( "Gui.MainForm.Font", "" );
			if ( fontDescription != "" )
			{
				TypeConverter converter = TypeDescriptor.GetConverter(typeof(Font));
				this.Font = (Font)converter.ConvertFrom(fontDescription);
			}
		}

		private void displayMiniGui()
		{
			this.displayFormat = "Mini";
			userSettings.SaveSetting( "Gui.DisplayFormat", "Mini" );

			this.Controls.Remove( rightPanel );
			this.Controls.Remove( treeSplitter );
			leftPanel.Dock = DockStyle.Fill;

			int x = userSettings.GetSetting( "Gui.MiniForm.Left", 10 );
			int y = userSettings.GetSetting( "Gui.MiniForm.Top", 10 );
			Point location = new Point( x, y );

			if ( !IsValidLocation( location ) )
				location = new Point( 10, 10 );
			this.Location = location;

			int width = userSettings.GetSetting( "Gui.MiniForm.Width", 300 );
			int height = userSettings.GetSetting( "Gui.MiniForm.Height", this.Size.Height );
			if ( width < 160 ) width = 160;
			if ( height < 32 ) height = 32;
			this.Size = new Size( width, height );

			// Set the font to use
			string fontDescription = userSettings.GetSetting( "Gui.MiniForm.Font", "" );
			if ( fontDescription != "" )
			{
				TypeConverter converter = TypeDescriptor.GetConverter(typeof(Font));
				this.Font = (Font)converter.ConvertFrom(fontDescription);
			}
		}

		private void increaseFontMenuItem_Click(object sender, System.EventArgs e)
		{
			applyFont( new Font( this.Font.FontFamily, this.Font.SizeInPoints * 1.2f, this.Font.Style ) );;
		}

		private void decreaseFontMenuItem_Click(object sender, System.EventArgs e)
		{
			applyFont( new Font( this.Font.FontFamily, this.Font.SizeInPoints / 1.2f, this.Font.Style ) );;
		}

		private void applyFont( Font font )
		{
			this.Font = font;
			TypeConverter converter = TypeDescriptor.GetConverter(typeof(Font));
			userSettings.SaveSetting( 
				displayFormat == "Mini" ? "Gui.MiniForm.Font" : "Gui.MainForm.Font", 
				converter.ConvertToString( font ) );
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

			addVSProjectMenuItem.Visible = userSettings.GetSetting( "Options.TestLoader.VisualStudioSupport", false );
			addAssemblyMenuItem.Enabled = TestProject.Configs.Count > 0;
		}

		private void configMenuItem_Click( object sender, EventArgs e )
		{
			MenuItem item = (MenuItem)sender;
			if ( !item.Checked )
				TestProject.SetActiveConfig( TestProject.Configs[item.Index].Name );
		}

		private void addConfigurationMenuItem_Click( object sender, System.EventArgs e )
		{
			using( AddConfigurationDialog dlg = new AddConfigurationDialog( TestProject ) )
			{
				this.Site.Container.Add( dlg );
				dlg.ShowDialog();
			}
		}

		private void editConfigurationsMenuItem_Click( object sender, System.EventArgs e )
		{
			using( ConfigurationEditor editor = new ConfigurationEditor( TestProject ) )
			{
				this.Site.Container.Add( editor );
				editor.ShowDialog();
			}
		}

		private void addAssemblyMenuItem_Click(object sender, System.EventArgs e)
		{
			TestLoaderUI.AddAssembly( this );
		}

		private void addVSProjectMenuItem_Click(object sender, System.EventArgs e)
		{
			TestLoaderUI.AddVSProject( this );
		}

		private void editProjectMenuItem_Click(object sender, System.EventArgs e)
		{
			using ( ProjectEditor editor = new ProjectEditor( TestProject ) )
			{
				this.Site.Container.Add( editor );
				editor.VisualStudioSupport = userSettings.GetSetting( "Options.TestLoader.VisualStudioSupport", false );
				editor.ShowDialog( this );
			}
		}

		#endregion

		#region Tools Menu Handlers

		private void toolsMenu_Popup(object sender, System.EventArgs e)
		{		
			saveXmlResultsMenuItem.Enabled = IsTestLoaded && TestLoader.TestResult != null;
			exceptionDetailsMenuItem.Enabled = TestLoader.LastException != null;
		}

		private void saveXmlResultsMenuItem_Click(object sender, System.EventArgs e)
		{
			TestLoaderUI.SaveLastResult( this );
		}

		private void exceptionDetailsMenuItem_Click(object sender, System.EventArgs e)
		{
			using( ExceptionDetailsForm details = new ExceptionDetailsForm( TestLoader.LastException ) )
			{
				this.Site.Container.Add( details );
				details.ShowDialog();
			}
		}

		private void optionsMenuItem_Click(object sender, System.EventArgs e)
		{
			using( OptionsDialog dialog = new OptionsDialog() )
			{
				this.Site.Container.Add( dialog );
				dialog.Font = this.Font;
				dialog.ShowDialog();

				resultTabs.OnOptionsChanged();	
			}
		}

		private void assemblyDetailsMenuItem_Click(object sender, System.EventArgs e)
		{
			IList infoList = TestLoader.AssemblyInfo;
			string msg = "No assemblies are loaded.";

			if ( infoList != null && infoList.Count > 0 )
			{
				StringBuilder sb = new StringBuilder( "Test Assemblies - \r\n\r\n" );

				foreach( TestAssemblyInfo info in infoList )
				{
					sb.AppendFormat( "{0}\r\n", info.Name );
					sb.AppendFormat( "Runtime Version:   {0}\r\n", info.RuntimeVersion.ToString() );
					if ( info.TestFrameworks != null )
					{
						string prefix = "Uses: ";
						foreach( AssemblyName framework in info.TestFrameworks )
						{
							sb.AppendFormat( "{0}{1}\r\n", prefix, framework.FullName );
							prefix = "      ";
						}
					}
					sb.Append("\r\n" );
				}

				msg = sb.ToString();
			}

			UserMessage.Display( msg, "Test Assembly Info" );
		}

		private void addinInfoMenuItem_Click(object sender, System.EventArgs e)
		{
			AddinDialog dlg = new AddinDialog();
			dlg.ShowDialog();
		}

		#endregion

		#region Help Menu Handlers

		private void helpMenuItem_Click(object sender, System.EventArgs e)
		{
			FileInfo exe = new FileInfo( Assembly.GetExecutingAssembly().Location );
			// In normal install, exe is in bin directory, so we get parent
			DirectoryInfo dir = exe.Directory.Parent;
			// If running from bin\Release or bin\Debug, go down four more
			// to the parent of the src and doc directories
			if ( dir.Name == "bin" ) dir = dir.Parent.Parent.Parent.Parent;

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

		/// <summary>
		/// Display the about box when menu item is selected
		/// </summary>
		private void aboutMenuItem_Click(object sender, System.EventArgs e)
		{
			using( AboutBox aboutBox = new AboutBox() )
			{
				this.Site.Container.Add( aboutBox );
				aboutBox.ShowDialog();
			}
		}

		#endregion

		#region Form Level Events
		/// <summary>
		/// Get saved options when form loads
		/// </summary>
		private void NUnitForm_Load(object sender, System.EventArgs e)
		{
			this.viewMenu.MenuItems.Add(3, resultTabs.TabsMenu);
			this.viewMenu.MenuItems.Add(4, testTree.TreeMenu);

			EnableRunCommand( false );
			EnableStopCommand( false );

			recentProjectsMenuHandler = new RecentFileMenuHandler( recentProjectsMenu, recentFilesService );

			LoadFormSettings();
			SubscribeToTestEvents();
			InitializeControls();

			// Load test specified on command line or
			// the most recent one if options call for it
			if ( commandLineOptions.testFileName != null )
				TestLoaderUI.OpenProject( this, commandLineOptions.testFileName, commandLineOptions.configName, commandLineOptions.testName );
			else if( userSettings.GetSetting( "Options.LoadLastProject", true ) && !commandLineOptions.noload )
			{
				RecentFilesCollection entries = recentFilesService.Entries;
				if ( entries.Count > 0 )
				{
					RecentFileEntry entry = recentFilesService.Entries[0];
					if ( entry != null )
						TestLoaderUI.OpenProject( this, entry.Path, commandLineOptions.configName, commandLineOptions.testName );
				}
			}

			if ( commandLineOptions.categories != null )
			{
				string[] categories = commandLineOptions.categories.Split( ',' );
				if ( categories.Length > 0 )
					this.testTree.SelectCategories( commandLineOptions.categories.Split( ',' ), commandLineOptions.exclude );
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
					TestLoader.RunTests();
				}
				finally
				{
					TestLoader.ReloadOnRun = reload;
				}
			}
		}
		
		private void LoadFormSettings()
		{
			this.displayFormat = userSettings.GetSetting( "Gui.DisplayFormat", "Full" );

			int x, y, width, height;
			Point location;

			switch( displayFormat )
			{
				case "Full":
					x = userSettings.GetSetting( "Gui.MainForm.Left", 10 );
					y = userSettings.GetSetting( "Gui.MainForm.Top", 10 );
					location = new Point( x, y );

					if ( !IsValidLocation( location ) )
						location = new Point( 10, 10 );
					this.Location = location;

					width = userSettings.GetSetting( "Gui.MainForm.Width", this.Width );
					height = userSettings.GetSetting( "Gui.MainForm.Height", this.Height );
					this.Size = new Size( width, height );

					if ( userSettings.GetSetting( "Gui.MainForm.Maximized", false ) )
						this.WindowState = FormWindowState.Maximized;
					
					fullGuiMenuItem.Checked = true;
					miniGuiMenuItem.Checked = false;

					displayFullGui();

					break;
				case "Mini":
					x = userSettings.GetSetting( "Gui.MiniForm.Left", 10 );
					y = userSettings.GetSetting( "Gui.MiniForm.Top", 10 );
					location = new Point( x, y );

					if ( !IsValidLocation( location ) )
						location = new Point( 10, 10 );
					this.Location = location;

					width = userSettings.GetSetting( "Gui.MiniForm.Width", this.Width );
					height = userSettings.GetSetting( "Gui.MiniForm.Height", this.Height );
					this.Size = new Size( width, height );

					fullGuiMenuItem.Checked = false;
					miniGuiMenuItem.Checked = true;

					displayMiniGui();

					break;
				default:
					throw new ApplicationException( "Invalid Setting" );
			}

			// Handle changes to form position
			this.Move += new System.EventHandler(this.NUnitForm_Move);
			this.Resize += new System.EventHandler(this.NUnitForm_Resize);

			// Set the splitter position
			int splitPosition = userSettings.GetSetting( "Gui.MainForm.SplitPosition", treeSplitter.SplitPosition );
			if ( splitPosition >= treeSplitter.MinSize && splitPosition < this.ClientSize.Width )
				this.treeSplitter.SplitPosition = splitPosition;

			// Handle changes in splitter positions
			this.treeSplitter.SplitterMoved += new SplitterEventHandler( treeSplitter_SplitterMoved );

			// Turn stacktrace tooltips on or off
			this.resultTabs.AutoExpand = userSettings.GetSetting( "Gui.ResultTabs.ErrorsTab.ToolTipsEnabled", true );
			this.resultTabs.WordWrap = userSettings.GetSetting( "Gui.ResultTabs.ErrorsTab.WordWrapEnabled", true );

			// Update tab menu items and display those that are used
			resultTabs.LoadSettingsAndUpdateTabPages();
		}

		private bool IsValidLocation( Point location )
		{
			Rectangle myArea = new Rectangle( location, this.Size );
			bool intersect = false;
			foreach (System.Windows.Forms.Screen screen in System.Windows.Forms.Screen.AllScreens)
			{
				intersect |= myArea.IntersectsWith(screen.WorkingArea);
			}
			return intersect;
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
			events.TestStarting		+= new TestEventHandler( OnTestStarting );
			events.TestException	+= new TestEventHandler( OnTestException );
		}

		private void InitializeControls()
		{
			// ToDo: Migrate more ui elements to handle events on their own.
			this.testTree.Initialize(TestLoader);
			this.progressBar.Subscribe( TestLoader.Events );
			this.statusBar.Subscribe( TestLoader.Events );
			this.resultTabs.Subscribe( TestLoader.Events );
		}

		// Save settings changed by moving the form
		private void NUnitForm_Move(object sender, System.EventArgs e)
		{
			switch( this.displayFormat )
			{
				case "Full":
				default:
					if ( this.WindowState == FormWindowState.Normal )
					{
						userSettings.SaveSetting( "Gui.MainForm.Left", this.Location.X );
						userSettings.SaveSetting( "Gui.MainForm.Top", this.Location.Y );
						userSettings.SaveSetting( "Gui.MainForm.Maximized", false );

						this.statusBar.SizingGrip = true;
					}
					else if ( this.WindowState == FormWindowState.Maximized )
					{
						userSettings.SaveSetting( "Gui.MainForm.Maximized", true );

						this.statusBar.SizingGrip = false;
					}
					break;
				case "Mini":
					if ( this.WindowState == FormWindowState.Normal )
					{
						userSettings.SaveSetting( "Gui.MiniForm.Left", this.Location.X );
						userSettings.SaveSetting( "Gui.MiniForm.Top", this.Location.Y );
						userSettings.SaveSetting( "Gui.MiniForm.Maximized", false );

						this.statusBar.SizingGrip = true;
					}
					else if ( this.WindowState == FormWindowState.Maximized )
					{
						userSettings.SaveSetting( "Gui.MiniForm.Maximized", true );

						this.statusBar.SizingGrip = false;
					}
					break;
			}
		}

		// Save settings that change when window is resized
		private void NUnitForm_Resize(object sender,System.EventArgs e)
		{
			if ( this.WindowState == FormWindowState.Normal )
			{
				if ( this.displayFormat == "Full" )
				{
					userSettings.SaveSetting( "Gui.MainForm.Width", this.Size.Width );
					userSettings.SaveSetting( "Gui.MainForm.Height", this.Size.Height );
				}
				else
				{
					userSettings.SaveSetting( "Gui.MiniForm.Width", this.Size.Width );
					userSettings.SaveSetting( "Gui.MiniForm.Height", this.Size.Height );
				}
			}
		}

		// Splitter moved so save it's position
		private void treeSplitter_SplitterMoved( object sender, SplitterEventArgs e )
		{
			userSettings.SaveSetting( "Gui.MainForm.SplitPosition", treeSplitter.SplitPosition );
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
				TestLoaderUI.CloseProject( this ) == DialogResult.Cancel )
				e.Cancel = true;
		}

		#endregion

		#region Other UI Event Handlers

		/// <summary>
		/// When the Run Button is clicked, run the selected test.
		/// </summary>
		private void runButton_Click(object sender, System.EventArgs e)
		{
			testTree.RunSelectedTests();
		}

		/// <summary>
		/// When the Stop Button is clicked, cancel running test
		/// </summary>
		private void stopButton_Click(object sender, System.EventArgs e)
		{
			CancelRun();
		}

		private void CancelRun()
		{
			EnableStopCommand( false );

			if ( IsTestRunning )
			{
				DialogResult dialogResult = UserMessage.Ask( 
					"Do you want to cancel the running test?" );

				if ( dialogResult == DialogResult.No )
					EnableStopCommand( true );
				else
					TestLoader.CancelTestRun();
			}
		}

		private void testTree_SelectedTestsChanged(object sender, SelectedTestsChangedEventArgs e)
		{
			if (!IsTestRunning) 
			{
				suiteName.Text = e.TestName;
				statusBar.Initialize(e.TestCount, e.TestName );
			}
		}

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
			{
				Version version = Environment.Version;
				foreach( TestAssemblyInfo info in TestLoader.AssemblyInfo )
					if ( info.RuntimeVersion < version )
						version = info.RuntimeVersion;
			
				recentFilesService.SetMostRecent( new RecentFileEntry( e.Name, version ) );
			}
		}

		private void OnTestProjectUnloaded( object sender, TestEventArgs e )
		{
			SetTitleBar( null );
			projectMenu.Visible = false;
		}

		private void OnTestLoadStarting( object sender, TestEventArgs e )
		{
			EnableRunCommand( false );
		}

		private void OnTestUnloadStarting( object sender, TestEventArgs e )
		{
			EnableRunCommand( false );
		}

		private void OnReloadStarting( object sender, TestEventArgs e )
		{
			EnableRunCommand( false );
		}

		/// <summary>
		/// A test suite has been loaded, so update 
		/// recent assemblies and display the tests in the UI
		/// </summary>
		private void OnTestLoaded( object sender, TestEventArgs e )
		{
			EnableRunCommand( true );
			ClearTabs();
			
			if ( TestLoader.TestCount == 0 )
			{
				foreach( TestAssemblyInfo info in TestLoader.AssemblyInfo )
					if ( info.TestFrameworks.Count > 0 ) return;

				UserMessage.Display( "This assembly was not built with any known testing framework.", "Not a Test Assembly");
			}
		}

		/// <summary>
		/// A test suite has been unloaded, so clear the UI
		/// and remove any references to the suite.
		/// </summary>
		private void OnTestUnloaded( object sender, TestEventArgs e )
		{
			suiteName.Text = null;
			EnableRunCommand( false );

			ClearTabs();
		}

		/// <summary>
		/// The current test suite has changed in some way,
		/// so update the info in the UI and clear the
		/// test results, since they are no longer valid.
		/// </summary>
		private void OnTestChanged( object sender, TestEventArgs e )
		{
			EnableRunCommand( true );
		}

		private void OnProjectLoadFailure( object sender, TestEventArgs e )
		{
			UserMessage.DisplayFailure( e.Exception, "Project Not Loaded" );

			recentFilesService.Remove( e.Name );

			EnableRunCommand( true );
		}

		/// <summary>
		/// Event handler for assembly load faiulres. We do this via
		/// an event since some errors may occur asynchronously.
		/// </summary>
		private void OnTestLoadFailure( object sender, TestEventArgs e )
		{
			string message = null;
			if ( e.Exception is BadImageFormatException )
				message = string.Format(
					@"You may be attempting to load an assembly built with a later version of the CLR than
the version under which NUnit is currently running, {0}.",
					Environment.Version.ToString(3) );

			UserMessage.DisplayFailure( e.Exception, message, "Assembly Not Loaded" );

			if ( !IsTestLoaded )
				OnTestUnloaded( sender, e );
			else
				EnableRunCommand( true );
		}

		#endregion

		#region Handlers for Test Running Events

		private void OnRunStarting( object sender, TestEventArgs e )
		{
			suiteName.Text = e.Name;
			EnableRunCommand( false );
			EnableStopCommand( true );

			ClearTabs();
		}

		private void OnRunFinished( object sender, TestEventArgs e )
		{
			EnableStopCommand( false );
			EnableRunCommand( false );

			if ( e.Exception != null )
			{
				if ( ! ( e.Exception is System.Threading.ThreadAbortException ) )
					UserMessage.DisplayFailure( e.Exception, "NUnit Test Run Failed" );
			}

			EnableRunCommand( true );
		}

		private void OnTestStarting(object sender, TestEventArgs args)
		{
			this.currentTestName = args.TestName.FullName;
		}

		private void OnTestException(object sender, TestEventArgs args)
		{
            TestCaseResult result = new TestCaseResult(this.currentTestName);
 
            // Don't throw inside an exception handler!
			try
			{
                // TODO: The unhandled exception message should be created at a lower level
                result.Error( new ApplicationException( 
                    "An unhandled Exception was thrown during execution of this test", 
                    args.Exception ) );
            }
			catch( Exception ex )
			{
                result.Error(new ApplicationException(
                    "An unhandled " + args.Exception.GetType().FullName +
                    "was thrown during execution of this test" + Environment.NewLine +
                    "The exception handler threw " + ex.GetType().FullName));
			}

            resultTabs.InsertTestResultItem(result);
        }

		#endregion

		#region Helper methods for modifying the UI display

		/// <summary>
		/// Clear info out of our four tabs and stack trace
		/// </summary>
		private void ClearTabs()
		{
			resultTabs.Clear();
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

		private void runAllMenuItem_Click(object sender, System.EventArgs e)
		{
			this.testTree.RunAllTests();
		}

		private void runSelectedMenuItem_Click(object sender, System.EventArgs e)
		{
			this.testTree.RunSelectedTests();
		
		}

		private void runFailedMenuItem_Click(object sender, System.EventArgs e)
		{
			this.testTree.RunFailedTests();
		
		}

		private void EnableRunCommand( bool enable )
		{
			runButton.Enabled = enable;
			runAllMenuItem.Enabled = enable;
			runSelectedMenuItem.Enabled = enable;
			runFailedMenuItem.Enabled = enable &&
				this.TestLoader.TestResult != null &&
				this.TestLoader.TestResult.IsFailure;
		}

		private void EnableStopCommand( bool enable )
		{
			stopButton.Enabled = enable;
			stopRunMenuItem.Enabled = enable;
		}

		private void stopRunMenuItem_Click(object sender, System.EventArgs e)
		{
			CancelRun();
		}

		private void viewMenu_Popup(object sender, System.EventArgs e)
		{
			assemblyDetailsMenuItem.Enabled = this.TestLoader.IsTestLoaded;
		}
	}
}

