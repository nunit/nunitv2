/********************************************************************************************************************
'
' Copyright (c) 2002, James Newkirk, Michael C. Two, Alexei Vorontsov, Philip Craig
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
' THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
'
'*******************************************************************************************************************/
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using Microsoft.Win32;

namespace NUnit.Gui
{
	using NUnit.Core;
	using NUnit.Util;

	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class NUnitForm : System.Windows.Forms.Form
	{
		private string assemblyFileName;
		private Test suite;
		private FileSystemWatcher watcher;
		private UIActions actions;

		public System.Windows.Forms.MenuItem menuItem1;
		public System.Windows.Forms.Splitter splitter1;
		public System.Windows.Forms.Panel panel1;
		public System.Windows.Forms.GroupBox groupBox1;
		public System.Windows.Forms.Splitter splitter2;
		public System.Windows.Forms.TabPage testsNotRun;
		public System.Windows.Forms.MenuItem menuItem4;
		public System.Windows.Forms.MenuItem menuItem6;
		public System.Windows.Forms.MenuItem menuItem8;
		public System.Windows.Forms.MenuItem openMenuItem;
		public System.Windows.Forms.MenuItem exitMenuItem;
		public System.Windows.Forms.MenuItem helpMenuItem;
		public System.Windows.Forms.MenuItem aboutMenuItem;
		public System.Windows.Forms.MainMenu mainMenu;
		public System.Windows.Forms.ListBox detailList;
		public System.Windows.Forms.Splitter splitter3;
		public System.Windows.Forms.TextBox stackTrace;
		public System.Windows.Forms.Label label1;
		public System.Windows.Forms.Label suiteName;
		public System.Windows.Forms.Button runButton;
		public System.Windows.Forms.StatusBar statusBar;
		public System.Windows.Forms.StatusBarPanel status;
		public System.Windows.Forms.StatusBarPanel testCaseCount;
		public System.Windows.Forms.StatusBarPanel testsRun;
		public System.Windows.Forms.StatusBarPanel failures;
		public System.Windows.Forms.StatusBarPanel time;
		public System.Windows.Forms.OpenFileDialog openFileDialog;
		public System.Windows.Forms.ContextMenu treeViewMenu;
		public System.Windows.Forms.MenuItem menuItem5;
		public System.Windows.Forms.ImageList treeImages;
		public System.Windows.Forms.TreeView assemblyViewer;
		public System.Windows.Forms.TabControl resultTabs;
		public System.Windows.Forms.TabPage errorPage;
		public System.Windows.Forms.TabPage stderr;
		public System.Windows.Forms.TabPage stdout;
		public System.Windows.Forms.TextBox stdErrTab;
		public System.Windows.Forms.TextBox stdOutTab;
		public System.Windows.Forms.MenuItem RecentAssemblies;
		public NUnit.Gui.ProgressBar progressBar;
		public System.Windows.Forms.TreeView notRunTree;
		private System.ComponentModel.IContainer components;
		public TextWriter stdOutWriter;
		public System.Windows.Forms.ToolTip toolTip;
		public TextWriter stdErrWriter;

		public NUnitForm(string assemblyFileName)
		{
			InitializeComponent();

			stdErrTab.Enabled = true;
			stdOutTab.Enabled = true;

			SetDefault(runButton);

			actions = new UIActions(this);

			stdErrWriter = new TextBoxWriter(stdErrTab);
			Console.SetError(stdErrWriter);
			stdOutWriter = new TextBoxWriter(stdOutTab);
			Console.SetOut(stdOutWriter);

			if (assemblyFileName != null) 
			{
				UIActions.SetMostRecentAssembly(assemblyFileName);
			}
			
			if (assemblyFileName == null)
				assemblyFileName = UIActions.GetMostRecentAssembly();

			if(assemblyFileName != null)
			{
				LoadAssembly(assemblyFileName);
				this.assemblyFileName = assemblyFileName;
			}

		}

		private void SetDefault(Button myDefaultBtn)
		{
			this.AcceptButton = myDefaultBtn;
		}

		public delegate void loadAssemblyDelegate(string assemblyFileName);

		public void OnChanged(object source, FileSystemEventArgs e) 
		{
			FileInfo info = new FileInfo(e.FullPath);
			this.Invoke(new loadAssemblyDelegate(this.LoadAssembly), new object[]{assemblyFileName});
		}

		private void LoadRecentAssemblies() 
		{
			IList assemblies = UIActions.GetMostRecentAssemblies();
			if (assemblies.Count == 0)
				RecentAssemblies.Enabled = false;
			else 
			{
				RecentAssemblies.Enabled = true;
				RecentAssemblies.MenuItems.Clear();
				int index = 1;
				foreach (string name in assemblies) 
				{
					MenuItem item = new MenuItem(String.Format("{0} {1}", index++, name));
					item.Click += new System.EventHandler(recentFile_clicked);
					this.RecentAssemblies.MenuItems.Add(item);
				}
			}
		}
		private void recentFile_clicked(object sender, System.EventArgs args) 
		{
			MenuItem item = (MenuItem) sender;
			string text = item.Text;

			assemblyFileName = text.Substring(2);
			LoadAssembly(assemblyFileName);
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(NUnitForm));
			this.statusBar = new System.Windows.Forms.StatusBar();
			this.status = new System.Windows.Forms.StatusBarPanel();
			this.testCaseCount = new System.Windows.Forms.StatusBarPanel();
			this.testsRun = new System.Windows.Forms.StatusBarPanel();
			this.failures = new System.Windows.Forms.StatusBarPanel();
			this.time = new System.Windows.Forms.StatusBarPanel();
			this.mainMenu = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.openMenuItem = new System.Windows.Forms.MenuItem();
			this.RecentAssemblies = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.exitMenuItem = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.helpMenuItem = new System.Windows.Forms.MenuItem();
			this.menuItem8 = new System.Windows.Forms.MenuItem();
			this.aboutMenuItem = new System.Windows.Forms.MenuItem();
			this.assemblyViewer = new System.Windows.Forms.TreeView();
			this.treeViewMenu = new System.Windows.Forms.ContextMenu();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.treeImages = new System.Windows.Forms.ImageList(this.components);
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
			this.splitter2 = new System.Windows.Forms.Splitter();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.runButton = new System.Windows.Forms.Button();
			this.suiteName = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.progressBar = new NUnit.Gui.ProgressBar();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			((System.ComponentModel.ISupportInitialize)(this.status)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.testCaseCount)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.testsRun)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.failures)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.time)).BeginInit();
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
			this.statusBar.Location = new System.Drawing.Point(0, 355);
			this.statusBar.Name = "statusBar";
			this.statusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
																						 this.status,
																						 this.testCaseCount,
																						 this.testsRun,
																						 this.failures,
																						 this.time});
			this.statusBar.ShowPanels = true;
			this.statusBar.Size = new System.Drawing.Size(624, 22);
			this.statusBar.TabIndex = 0;
			this.statusBar.Text = "Status";
			// 
			// status
			// 
			this.status.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.status.Text = "Status";
			this.status.Width = 208;
			// 
			// testCaseCount
			// 
			this.testCaseCount.Text = "Test Cases:";
			// 
			// testsRun
			// 
			this.testsRun.Text = "Tests Run:";
			// 
			// failures
			// 
			this.failures.Text = "Failures:";
			// 
			// time
			// 
			this.time.Text = "Time:";
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.menuItem1,
																					 this.menuItem6});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.openMenuItem,
																					  this.RecentAssemblies,
																					  this.menuItem4,
																					  this.exitMenuItem});
			this.menuItem1.Text = "File";
			// 
			// openMenuItem
			// 
			this.openMenuItem.Index = 0;
			this.openMenuItem.Text = "Open...";
			this.openMenuItem.Click += new System.EventHandler(this.openMenuItem_Click);
			// 
			// RecentAssemblies
			// 
			this.RecentAssemblies.Index = 1;
			this.RecentAssemblies.Text = "Recent Assemblies";
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 2;
			this.menuItem4.Text = "-";
			// 
			// exitMenuItem
			// 
			this.exitMenuItem.Index = 3;
			this.exitMenuItem.Text = "Exit";
			this.exitMenuItem.Click += new System.EventHandler(this.menuItem5_Click);
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 1;
			this.menuItem6.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.helpMenuItem,
																					  this.menuItem8,
																					  this.aboutMenuItem});
			this.menuItem6.Text = "Help";
			// 
			// helpMenuItem
			// 
			this.helpMenuItem.Enabled = false;
			this.helpMenuItem.Index = 0;
			this.helpMenuItem.Text = "Help";
			// 
			// menuItem8
			// 
			this.menuItem8.Index = 1;
			this.menuItem8.Text = "-";
			// 
			// aboutMenuItem
			// 
			this.aboutMenuItem.Index = 2;
			this.aboutMenuItem.Text = "About NUnit...";
			this.aboutMenuItem.Click += new System.EventHandler(this.aboutMenuItem_Click);
			// 
			// assemblyViewer
			// 
			this.assemblyViewer.ContextMenu = this.treeViewMenu;
			this.assemblyViewer.Dock = System.Windows.Forms.DockStyle.Left;
			this.assemblyViewer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.assemblyViewer.ImageList = this.treeImages;
			this.assemblyViewer.Name = "assemblyViewer";
			this.assemblyViewer.Size = new System.Drawing.Size(240, 355);
			this.assemblyViewer.TabIndex = 1;
			this.assemblyViewer.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.assemblyViewer_AfterSelect);
			// 
			// treeViewMenu
			// 
			this.treeViewMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuItem5});
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 0;
			this.menuItem5.Text = "Run";
			this.menuItem5.Click += new System.EventHandler(this.menuItem5_Click_1);
			// 
			// treeImages
			// 
			this.treeImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			this.treeImages.ImageSize = new System.Drawing.Size(16, 16);
			this.treeImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("treeImages.ImageStream")));
			this.treeImages.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(240, 0);
			this.splitter1.MinSize = 240;
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 355);
			this.splitter1.TabIndex = 2;
			this.splitter1.TabStop = false;
			// 
			// panel1
			// 
			this.panel1.Controls.AddRange(new System.Windows.Forms.Control[] {
																				 this.resultTabs,
																				 this.splitter2,
																				 this.groupBox1});
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(243, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(381, 355);
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
			this.resultTabs.Location = new System.Drawing.Point(0, 131);
			this.resultTabs.Name = "resultTabs";
			this.resultTabs.SelectedIndex = 0;
			this.resultTabs.Size = new System.Drawing.Size(381, 224);
			this.resultTabs.TabIndex = 2;
			// 
			// errorPage
			// 
			this.errorPage.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.stackTrace,
																					this.splitter3,
																					this.detailList});
			this.errorPage.Location = new System.Drawing.Point(4, 22);
			this.errorPage.Name = "errorPage";
			this.errorPage.Size = new System.Drawing.Size(373, 198);
			this.errorPage.TabIndex = 0;
			this.errorPage.Text = "Errors and Failures";
			// 
			// stackTrace
			// 
			this.stackTrace.Dock = System.Windows.Forms.DockStyle.Fill;
			this.stackTrace.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.stackTrace.Location = new System.Drawing.Point(0, 87);
			this.stackTrace.Multiline = true;
			this.stackTrace.Name = "stackTrace";
			this.stackTrace.ReadOnly = true;
			this.stackTrace.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.stackTrace.Size = new System.Drawing.Size(373, 111);
			this.stackTrace.TabIndex = 2;
			this.stackTrace.Text = "";
			this.stackTrace.WordWrap = false;
			// 
			// splitter3
			// 
			this.splitter3.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter3.Location = new System.Drawing.Point(0, 84);
			this.splitter3.MinSize = 100;
			this.splitter3.Name = "splitter3";
			this.splitter3.Size = new System.Drawing.Size(373, 3);
			this.splitter3.TabIndex = 1;
			this.splitter3.TabStop = false;
			// 
			// detailList
			// 
			this.detailList.Dock = System.Windows.Forms.DockStyle.Top;
			this.detailList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.detailList.HorizontalExtent = 2000;
			this.detailList.HorizontalScrollbar = true;
			this.detailList.ItemHeight = 16;
			this.detailList.Name = "detailList";
			this.detailList.ScrollAlwaysVisible = true;
			this.detailList.Size = new System.Drawing.Size(373, 84);
			this.detailList.TabIndex = 0;
			this.detailList.SelectedIndexChanged += new System.EventHandler(this.detailList_SelectedIndexChanged);
			// 
			// testsNotRun
			// 
			this.testsNotRun.Controls.AddRange(new System.Windows.Forms.Control[] {
																					  this.notRunTree});
			this.testsNotRun.Location = new System.Drawing.Point(4, 22);
			this.testsNotRun.Name = "testsNotRun";
			this.testsNotRun.Size = new System.Drawing.Size(373, 198);
			this.testsNotRun.TabIndex = 1;
			this.testsNotRun.Text = "Tests Not Run";
			// 
			// notRunTree
			// 
			this.notRunTree.Dock = System.Windows.Forms.DockStyle.Fill;
			this.notRunTree.ImageIndex = -1;
			this.notRunTree.Name = "notRunTree";
			this.notRunTree.SelectedImageIndex = -1;
			this.notRunTree.Size = new System.Drawing.Size(373, 198);
			this.notRunTree.TabIndex = 0;
			// 
			// stderr
			// 
			this.stderr.Controls.AddRange(new System.Windows.Forms.Control[] {
																				 this.stdErrTab});
			this.stderr.Location = new System.Drawing.Point(4, 22);
			this.stderr.Name = "stderr";
			this.stderr.Size = new System.Drawing.Size(373, 198);
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
			this.stdErrTab.Size = new System.Drawing.Size(373, 198);
			this.stdErrTab.TabIndex = 0;
			this.stdErrTab.Text = "";
			this.stdErrTab.WordWrap = false;
			// 
			// stdout
			// 
			this.stdout.Controls.AddRange(new System.Windows.Forms.Control[] {
																				 this.stdOutTab});
			this.stdout.Location = new System.Drawing.Point(4, 22);
			this.stdout.Name = "stdout";
			this.stdout.Size = new System.Drawing.Size(373, 198);
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
			this.stdOutTab.Size = new System.Drawing.Size(373, 198);
			this.stdOutTab.TabIndex = 0;
			this.stdOutTab.Text = "";
			this.stdOutTab.WordWrap = false;
			// 
			// splitter2
			// 
			this.splitter2.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter2.Location = new System.Drawing.Point(0, 128);
			this.splitter2.MinSize = 130;
			this.splitter2.Name = "splitter2";
			this.splitter2.Size = new System.Drawing.Size(381, 3);
			this.splitter2.TabIndex = 1;
			this.splitter2.TabStop = false;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.runButton,
																					this.suiteName,
																					this.label1,
																					this.progressBar});
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(381, 128);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Run";
			// 
			// runButton
			// 
			this.runButton.Location = new System.Drawing.Point(8, 56);
			this.runButton.Name = "runButton";
			this.runButton.TabIndex = 3;
			this.runButton.Text = "Run";
			this.runButton.Click += new System.EventHandler(this.runButton_Click);
			// 
			// suiteName
			// 
			this.suiteName.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.suiteName.Location = new System.Drawing.Point(72, 24);
			this.suiteName.Name = "suiteName";
			this.suiteName.Size = new System.Drawing.Size(248, 16);
			this.suiteName.TabIndex = 2;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(40, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Test:";
			// 
			// progressBar
			// 
			this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.progressBar.CausesValidation = false;
			this.progressBar.Enabled = false;
			this.progressBar.ForeColor = System.Drawing.SystemColors.Highlight;
			this.progressBar.Location = new System.Drawing.Point(8, 88);
			this.progressBar.Maximum = 100;
			this.progressBar.Minimum = 0;
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(360, 24);
			this.progressBar.Step = 1;
			this.progressBar.TabIndex = 0;
			this.progressBar.Value = 0;
			this.progressBar.Click += new System.EventHandler(this.progressBar1_Click);
			// 
			// openFileDialog
			// 
			this.openFileDialog.Filter = "Assemblies (*.dll)|*.dll|Executables (*.exe)|*.exe|All Files (*.*)|*.*";
			// 
			// NUnitForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(624, 377);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.panel1,
																		  this.splitter1,
																		  this.assemblyViewer,
																		  this.statusBar});
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenu;
			this.Name = "NUnitForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "NUnit";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.NUnitForm_Closing);
			this.Load += new System.EventHandler(this.NUnitForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.status)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.testCaseCount)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.testsRun)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.failures)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.time)).EndInit();
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

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static int Main(string[] args) 
		{
			string assemblyName = null;

			try 
			{
				ArrayList allowedParameters = new ArrayList();
				allowedParameters.Add(CommandLineParser.ASSEMBLY_PARM);
				CommandLineParser parser = new CommandLineParser(allowedParameters, args);
				if(!parser.NoArgs)
					assemblyName = parser.AssemblyName;

				if(assemblyName != null)
				{
					FileInfo fileInfo = new FileInfo(assemblyName);
					if(!fileInfo.Exists)
					{
						string message = String.Format("{0} does not exist", fileInfo.FullName);
						MessageBox.Show(null,message,"assembly does not exist",
							MessageBoxButtons.OK,MessageBoxIcon.Stop);
						return 1;
					}
					else
					{
						assemblyName = fileInfo.FullName;
					}
				}

				NUnitForm form = new NUnitForm(assemblyName);
				Application.Run(form);
			}
			catch(CommandLineException cle)
			{
				string message = cle.Message;
				MessageBox.Show(null,message,"Invalid Command Line Options",
					MessageBoxButtons.OK,MessageBoxIcon.Stop);
				return 2;
			}	

			return 0;

		}

		private void menuItem5_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void progressBar1_Click(object sender, System.EventArgs e)
		{
		
		}

		private void runButton_Click(object sender, System.EventArgs e)
		{
			actions.RunTestSuite(suite);
		}
		private void UpdateRecentAssemblies(string assemblyFileName)
		{
			UIActions.SetMostRecentAssembly(assemblyFileName);
			LoadRecentAssemblies();
		}

		private void InstallWatcher(string assemblyFileName)
		{
			if(watcher!=null)
			{
				watcher.EnableRaisingEvents=false;
			}

			watcher = new FileSystemWatcher();
			FileInfo info = new FileInfo(assemblyFileName);
			watcher.Path = info.DirectoryName;
			watcher.Filter = info.Name;
			
			watcher.NotifyFilter = NotifyFilters.Size;

			watcher.Changed += new FileSystemEventHandler(OnChanged);
			watcher.EnableRaisingEvents = true;
		}

		private static void SetWorkingDirectory(string assemblyFileName)
		{
			FileInfo info = new FileInfo(assemblyFileName);
			Directory.SetCurrentDirectory(info.DirectoryName);
		}

		private void LoadAssembly(string assemblyFileName)
		{
			try
			{
				suite = actions.LoadAssembly(assemblyFileName, suite);
				SetWorkingDirectory(assemblyFileName);
				InstallWatcher(assemblyFileName);
				UpdateRecentAssemblies(assemblyFileName);
				runButton.Enabled = true;
			}
			catch(Exception exception)
			{
				string message = exception.Message;
				if(exception.InnerException != null)
					message = exception.InnerException.Message;
				MessageBox.Show(this,message,"Assembly Load Failure",
					MessageBoxButtons.OK,MessageBoxIcon.Stop);
			}
		}

		private void openMenuItem_Click(object sender, System.EventArgs e)
		{
			if (openFileDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK) 
			{
				assemblyFileName = openFileDialog.FileName;
				LoadAssembly(assemblyFileName);
			}
		
		}

		private void menuItem5_Click_1(object sender, System.EventArgs e)
		{
			actions.RunTestSuite(suite);
		}

		private void assemblyViewer_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			suite = actions.TestSelected();
		}

		private void detailList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			actions.DetailItemSelected();
		}

		protected override bool ProcessKeyPreview(ref 
			System.Windows.Forms.Message m) 
		{ 
			const int SPACE_BAR=32; 
			if (m.WParam.ToInt32() == SPACE_BAR)
			{ 
				this.Close();
			} 
			return true; 
		} 


		private void aboutMenuItem_Click(object sender, System.EventArgs e)
		{
			AboutBox aboutBox = new AboutBox();
			aboutBox.Show();
		}

		private static string KEY = "Software\\Nascent Software\\Nunit\\";
		private static string WIDTH = "width";
		private static string HEIGHT = "height";
		private static string XLOCATION = "x-location";
		private static string YLOCATION = "y-location";


		private void NUnitForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			string subKey = String.Format("{0}{1}", KEY, "form");
			RegistryKey key = Registry.CurrentUser.CreateSubKey(subKey);

			key.SetValue(WIDTH, this.Size.Width.ToString());
			key.SetValue(HEIGHT, this.Size.Height.ToString());
			key.SetValue(XLOCATION, this.Location.X.ToString());
			key.SetValue(YLOCATION, this.Location.Y.ToString());
		}

		private void NUnitForm_Load(object sender, System.EventArgs e)
		{
			string subKey = String.Format("{0}{1}", KEY, "form");
			RegistryKey key = Registry.CurrentUser.OpenSubKey(subKey);

			int xLocation = 10; 
			int yLocation = 10;
			int width = 632;
			int height = 432; 

			if(key != null)
			{
				width = int.Parse((string)key.GetValue(WIDTH));
				height = int.Parse((string)key.GetValue(HEIGHT));
				xLocation = int.Parse((string)key.GetValue(XLOCATION));
				yLocation = int.Parse((string)key.GetValue(YLOCATION));
			}

			SetBounds(xLocation, yLocation, width, height);	
		}
	}
}
