using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using NUnit.Util;

namespace NUnit.UiKit
{
	public delegate void SelectedTestsChangedEventHandler(object sender, SelectedTestsChangedEventArgs e);
	/// <summary>
	/// Summary description for TestTree.
	/// </summary>
	public class TestTree : System.Windows.Forms.UserControl
	{
		// Contains all available categories, whether
		// selected or not. Unselected members of this
		// list are displayed in selectedList
		private IList availableCategories;

		private System.Windows.Forms.TabControl tabs;
		private System.Windows.Forms.TabPage testPage;
		private System.Windows.Forms.TabPage categoryPage;
		private System.Windows.Forms.Panel testPanel;
		private System.Windows.Forms.Panel categoryPanel;
		private System.Windows.Forms.Panel treePanel;
		private System.Windows.Forms.Panel buttonPanel;
		public NUnit.UiKit.TestSuiteTreeView tests;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ListBox availableList;
		private System.Windows.Forms.GroupBox selectedCategories;
		private System.Windows.Forms.ListBox selectedList;
		private System.Windows.Forms.Panel categoryButtonPanel;
		private System.Windows.Forms.Button addCategory;
		private System.Windows.Forms.Button removeCategory;
		private System.Windows.Forms.Button clearAllButton;
		private System.Windows.Forms.Button checkFailedButton;
		private System.Windows.Forms.MenuItem viewMenu;
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
		private System.Windows.Forms.MenuItem checkBoxesMenuItem;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.CheckBox excludeCheckbox;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MenuItem ViewMenu 
		{
			get { return viewMenu; }
		}

		public string[] SelectedCategories
		{
			get
			{
				int n = this.selectedList.Items.Count;
				string[] categories = new string[n];
				for( int i = 0; i < n; i++ )
					categories[i] = this.selectedList.Items[i].ToString();
				return categories;
			}
		}

		#region Construction and Initialization

		public TestTree()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			viewMenu = new MenuItem();
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
			this.checkBoxesMenuItem = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();

			// 
			// viewMenu
			// 
			this.viewMenu.MergeType = MenuMerge.Add;
			this.viewMenu.MergeOrder = 1;
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
										this.propertiesMenuItem,
										this.checkBoxesMenuItem,
										this.menuItem2});
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
			// checkBoxesMenuItem
			// 
			this.checkBoxesMenuItem.Index = 0;
			this.checkBoxesMenuItem.Text = "Check&Boxes";
			this.checkBoxesMenuItem.Click += new System.EventHandler(this.checkBoxesMenuItem_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 1;
			this.menuItem2.Text = "-";


			tests.SelectedTestChanged += new SelectedTestChangedHandler(tests_SelectedTestChanged);
			tests.CheckedTestChanged += new CheckedTestChangedHandler(tests_CheckedTestChanged);

			ShowCheckBoxes( UserSettings.Options.ShowCheckBoxes );

			this.excludeCheckbox.Enabled = this.SelectedCategories.Length > 0;
		}

		public void Initialize(TestLoader loader) 
		{
			this.tests.Initialize(loader, loader.Events);
			loader.Events.TestLoaded += new NUnit.Core.TestEventHandler(events_TestLoaded);
			loader.Events.TestReloaded += new NUnit.Core.TestEventHandler(events_TestReloaded);
			loader.Events.TestUnloaded += new NUnit.Core.TestEventHandler(Events_TestUnloaded);
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

		#region View Menu Handlers

		private void viewMenu_Popup(object sender, System.EventArgs e)
		{
			TreeNode selectedNode = tests.SelectedNode;
			if ( selectedNode != null && selectedNode.Nodes.Count > 0 )
			{
				bool isExpanded = selectedNode.IsExpanded;
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
			tests.SelectedNode.Collapse();
		}

		private void expandMenuItem_Click(object sender, System.EventArgs e)
		{
			tests.SelectedNode.Expand();
		}

		private void collapseAllMenuItem_Click(object sender, System.EventArgs e)
		{
			tests.CollapseAll();

			// Compensate for a bug in the underlying control
			if ( tests.Nodes.Count > 0 )
				tests.SelectedNode = tests.Nodes[0];	
		}

		private void expandAllMenuItem_Click(object sender, System.EventArgs e)
		{
			tests.ExpandAll();
		}

		private void collapseFixturesMenuItem_Click(object sender, System.EventArgs e)
		{
			tests.CollapseFixtures();		
		}

		private void expandFixturesMenuItem_Click(object sender, System.EventArgs e)
		{
			tests.ExpandFixtures();		
		}

		private void propertiesMenuItem_Click(object sender, System.EventArgs e)
		{
			if ( tests.SelectedTest != null )
				tests.ShowPropertiesDialog( tests.SelectedTest );
		}

		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tabs = new System.Windows.Forms.TabControl();
			this.testPage = new System.Windows.Forms.TabPage();
			this.testPanel = new System.Windows.Forms.Panel();
			this.treePanel = new System.Windows.Forms.Panel();
			this.tests = new NUnit.UiKit.TestSuiteTreeView();
			this.buttonPanel = new System.Windows.Forms.Panel();
			this.checkFailedButton = new System.Windows.Forms.Button();
			this.clearAllButton = new System.Windows.Forms.Button();
			this.categoryPage = new System.Windows.Forms.TabPage();
			this.categoryPanel = new System.Windows.Forms.Panel();
			this.categoryButtonPanel = new System.Windows.Forms.Panel();
			this.removeCategory = new System.Windows.Forms.Button();
			this.addCategory = new System.Windows.Forms.Button();
			this.selectedCategories = new System.Windows.Forms.GroupBox();
			this.selectedList = new System.Windows.Forms.ListBox();
			this.excludeCheckbox = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.availableList = new System.Windows.Forms.ListBox();
			this.tabs.SuspendLayout();
			this.testPage.SuspendLayout();
			this.testPanel.SuspendLayout();
			this.treePanel.SuspendLayout();
			this.buttonPanel.SuspendLayout();
			this.categoryPage.SuspendLayout();
			this.categoryPanel.SuspendLayout();
			this.categoryButtonPanel.SuspendLayout();
			this.selectedCategories.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabs
			// 
			this.tabs.Controls.Add(this.testPage);
			this.tabs.Controls.Add(this.categoryPage);
			this.tabs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabs.Location = new System.Drawing.Point(0, 0);
			this.tabs.Name = "tabs";
			this.tabs.SelectedIndex = 0;
			this.tabs.Size = new System.Drawing.Size(248, 496);
			this.tabs.TabIndex = 0;
			// 
			// testPage
			// 
			this.testPage.Controls.Add(this.testPanel);
			this.testPage.Location = new System.Drawing.Point(4, 22);
			this.testPage.Name = "testPage";
			this.testPage.Size = new System.Drawing.Size(240, 470);
			this.testPage.TabIndex = 0;
			this.testPage.Text = "Tests";
			// 
			// testPanel
			// 
			this.testPanel.Controls.Add(this.treePanel);
			this.testPanel.Controls.Add(this.buttonPanel);
			this.testPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.testPanel.Location = new System.Drawing.Point(0, 0);
			this.testPanel.Name = "testPanel";
			this.testPanel.Size = new System.Drawing.Size(240, 470);
			this.testPanel.TabIndex = 0;
			// 
			// treePanel
			// 
			this.treePanel.Controls.Add(this.tests);
			this.treePanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treePanel.Location = new System.Drawing.Point(0, 0);
			this.treePanel.Name = "treePanel";
			this.treePanel.Size = new System.Drawing.Size(240, 430);
			this.treePanel.TabIndex = 0;
			// 
			// tests
			// 
			this.tests.AllowDrop = true;
			this.tests.CheckBoxes = true;
			this.tests.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tests.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.tests.HideSelection = false;
			this.tests.Location = new System.Drawing.Point(0, 0);
			this.tests.Name = "tests";
			this.tests.SelectedCategories = null;
			this.tests.Size = new System.Drawing.Size(240, 430);
			this.tests.TabIndex = 0;
			// 
			// buttonPanel
			// 
			this.buttonPanel.Controls.Add(this.checkFailedButton);
			this.buttonPanel.Controls.Add(this.clearAllButton);
			this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.buttonPanel.Location = new System.Drawing.Point(0, 430);
			this.buttonPanel.Name = "buttonPanel";
			this.buttonPanel.Size = new System.Drawing.Size(240, 40);
			this.buttonPanel.TabIndex = 1;
			// 
			// checkFailedButton
			// 
			this.checkFailedButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.checkFailedButton.Location = new System.Drawing.Point(128, 8);
			this.checkFailedButton.Name = "checkFailedButton";
			this.checkFailedButton.Size = new System.Drawing.Size(80, 23);
			this.checkFailedButton.TabIndex = 1;
			this.checkFailedButton.Text = "Check Failed";
			this.checkFailedButton.Click += new System.EventHandler(this.checkFailedButton_Click);
			// 
			// clearAllButton
			// 
			this.clearAllButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.clearAllButton.Location = new System.Drawing.Point(40, 8);
			this.clearAllButton.Name = "clearAllButton";
			this.clearAllButton.TabIndex = 0;
			this.clearAllButton.Text = "Clear All";
			this.clearAllButton.Click += new System.EventHandler(this.clearAllButton_Click);
			// 
			// categoryPage
			// 
			this.categoryPage.Controls.Add(this.categoryPanel);
			this.categoryPage.Location = new System.Drawing.Point(4, 22);
			this.categoryPage.Name = "categoryPage";
			this.categoryPage.Size = new System.Drawing.Size(240, 470);
			this.categoryPage.TabIndex = 1;
			this.categoryPage.Text = "Categories";
			// 
			// categoryPanel
			// 
			this.categoryPanel.Controls.Add(this.categoryButtonPanel);
			this.categoryPanel.Controls.Add(this.selectedCategories);
			this.categoryPanel.Controls.Add(this.groupBox1);
			this.categoryPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.categoryPanel.Location = new System.Drawing.Point(0, 0);
			this.categoryPanel.Name = "categoryPanel";
			this.categoryPanel.Size = new System.Drawing.Size(240, 470);
			this.categoryPanel.TabIndex = 0;
			// 
			// categoryButtonPanel
			// 
			this.categoryButtonPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.categoryButtonPanel.Controls.Add(this.removeCategory);
			this.categoryButtonPanel.Controls.Add(this.addCategory);
			this.categoryButtonPanel.Location = new System.Drawing.Point(8, 259);
			this.categoryButtonPanel.Name = "categoryButtonPanel";
			this.categoryButtonPanel.Size = new System.Drawing.Size(224, 40);
			this.categoryButtonPanel.TabIndex = 2;
			// 
			// removeCategory
			// 
			this.removeCategory.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.removeCategory.Location = new System.Drawing.Point(120, 8);
			this.removeCategory.Name = "removeCategory";
			this.removeCategory.TabIndex = 1;
			this.removeCategory.Text = "Remove";
			this.removeCategory.Click += new System.EventHandler(this.removeCategory_Click);
			// 
			// addCategory
			// 
			this.addCategory.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.addCategory.Location = new System.Drawing.Point(32, 8);
			this.addCategory.Name = "addCategory";
			this.addCategory.TabIndex = 0;
			this.addCategory.Text = "Add";
			this.addCategory.Click += new System.EventHandler(this.addCategory_Click);
			// 
			// selectedCategories
			// 
			this.selectedCategories.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.selectedCategories.Controls.Add(this.selectedList);
			this.selectedCategories.Controls.Add(this.excludeCheckbox);
			this.selectedCategories.Location = new System.Drawing.Point(8, 307);
			this.selectedCategories.Name = "selectedCategories";
			this.selectedCategories.Size = new System.Drawing.Size(224, 152);
			this.selectedCategories.TabIndex = 1;
			this.selectedCategories.TabStop = false;
			this.selectedCategories.Text = "Selected Categories";
			// 
			// selectedList
			// 
			this.selectedList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.selectedList.Location = new System.Drawing.Point(8, 16);
			this.selectedList.Name = "selectedList";
			this.selectedList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.selectedList.Size = new System.Drawing.Size(208, 95);
			this.selectedList.TabIndex = 0;
			// 
			// excludeCheckbox
			// 
			this.excludeCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.excludeCheckbox.Location = new System.Drawing.Point(8, 128);
			this.excludeCheckbox.Name = "excludeCheckbox";
			this.excludeCheckbox.Size = new System.Drawing.Size(200, 16);
			this.excludeCheckbox.TabIndex = 3;
			this.excludeCheckbox.Text = "Exclude these categories";
			this.excludeCheckbox.CheckedChanged += new System.EventHandler(this.excludeCheckbox_CheckedChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.availableList);
			this.groupBox1.Location = new System.Drawing.Point(8, 16);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(224, 235);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Available Categories";
			// 
			// availableList
			// 
			this.availableList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.availableList.Location = new System.Drawing.Point(8, 24);
			this.availableList.Name = "availableList";
			this.availableList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.availableList.Size = new System.Drawing.Size(208, 199);
			this.availableList.TabIndex = 0;
			// 
			// TestTree
			// 
			this.Controls.Add(this.tabs);
			this.Name = "TestTree";
			this.Size = new System.Drawing.Size(248, 496);
			this.tabs.ResumeLayout(false);
			this.testPage.ResumeLayout(false);
			this.testPanel.ResumeLayout(false);
			this.treePanel.ResumeLayout(false);
			this.buttonPanel.ResumeLayout(false);
			this.categoryPage.ResumeLayout(false);
			this.categoryPanel.ResumeLayout(false);
			this.categoryButtonPanel.ResumeLayout(false);
			this.selectedCategories.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region events
		public event SelectedTestsChangedEventHandler SelectedTestsChanged;
		#endregion


		public void RunTests()
		{
			tests.RunTests();
		}

		private void addCategory_Click(object sender, System.EventArgs e)
		{
			if (availableList.SelectedItems.Count > 0) 
			{
				ArrayList categories = new ArrayList(availableList.SelectedItems);
				foreach ( string category in categories ) 
				{
					selectedList.Items.Add(category);
					availableList.Items.Remove(category);
//					CheckCategoryVisitor visitor = new CheckCategoryVisitor(category);
//					tests.Accept(visitor);
				}

				tests.SelectedCategories = this.SelectedCategories;
				if (this.SelectedCategories.Length > 0)
					this.excludeCheckbox.Enabled = true;
			}
		}

		private void removeCategory_Click(object sender, System.EventArgs e)
		{
			if (selectedList.SelectedItems.Count > 0) 
			{
				ArrayList categories = new ArrayList(selectedList.SelectedItems);
				foreach ( string category in categories )
				{
					selectedList.Items.Remove(category);
					availableList.Items.Add(category);
//					UnCheckCategoryVisitor visitor = new UnCheckCategoryVisitor(category);
//					tests.Accept(visitor);
				}

				tests.SelectedCategories = this.SelectedCategories;
				if (this.SelectedCategories.Length == 0)
					this.excludeCheckbox.Enabled = false;
			}
		}

		private void clearAllButton_Click(object sender, System.EventArgs e)
		{
			tests.ClearCheckedNodes();
		}

		private void checkFailedButton_Click(object sender, System.EventArgs e)
		{
			tests.CheckFailedNodes();
		}

		private void tests_SelectedTestChanged(UITestNode test)
		{
			if (SelectedTestsChanged != null) 
			{
			    SelectedTestsChangedEventArgs args = new SelectedTestsChangedEventArgs(test.Name, test.CountTestCases());
				SelectedTestsChanged(tests, args);
			}
		}

		private void events_TestLoaded(object sender, NUnit.Core.TestEventArgs args)
		{			
			viewMenu.Visible = true;

			availableCategories = AppUI.TestLoader.GetCategories();
			availableList.Items.Clear();
			selectedList.Items.Clear();
			
			availableList.SuspendLayout();
			foreach (string category in availableCategories) 
				availableList.Items.Add(category);
			availableList.ResumeLayout();
		}

		private void events_TestReloaded(object sender, NUnit.Core.TestEventArgs args)
		{
			// Get new list of available categories
			availableCategories = AppUI.TestLoader.GetCategories();

			// Remove any selected items that are no longer available
			int index = selectedList.Items.Count;
			selectedList.SuspendLayout();
			while( --index >= 0 )
			{
				string category = selectedList.Items[index].ToString();
				if ( !availableCategories.Contains( category )  )
					selectedList.Items.RemoveAt( index );
			}
			selectedList.ResumeLayout();

			// Put any unselected available items availableList
			availableList.Items.Clear();
			availableList.SuspendLayout();
			foreach( string category in availableCategories )
				if( selectedList.FindStringExact( category ) < 0 )
					availableList.Items.Add( category );
			availableList.ResumeLayout();

			// Tell the tree what is selected
			tests.SelectedCategories = this.SelectedCategories;
		}

		private void excludeCheckbox_CheckedChanged(object sender, System.EventArgs e)
		{
			tests.ExcludeSelectedCategories = excludeCheckbox.Checked;
		}

		private void Events_TestUnloaded(object sender, NUnit.Core.TestEventArgs args)
		{
			viewMenu.Visible = false;
		}

		private void tests_CheckedTestChanged(UITestNode[] tests)
		{
			if (SelectedTestsChanged != null) 
			{
				SelectedTestsChangedEventArgs args = new SelectedTestsChangedEventArgs("", tests.Length);
				SelectedTestsChanged(tests, args);
			}

			if (tests.Length > 0) 
			{
			}
		}

		private void ShowCheckBoxes( bool show )
		{
			tests.SaveVisualState();
			tests.CheckBoxes = show;
			buttonPanel.Visible	= show;
			clearAllButton.Visible = show;
			checkFailedButton.Visible = show;
			checkBoxesMenuItem.Checked = show;
			UserSettings.Options.ShowCheckBoxes = show;
			tests.RestoreVisualState();
		}

		private void checkBoxesMenuItem_Click(object sender, System.EventArgs e)
		{
			ShowCheckBoxes( !checkBoxesMenuItem.Checked );
			
			// Temporary till we can save tree state and restore
			//this.SetInitialExpansion();
		}

	}

	public class SelectedTestsChangedEventArgs : EventArgs 
	{
		private string testName;
		private int count;

		public SelectedTestsChangedEventArgs(string testName, int count) 
		{
			this.testName = testName;
			this.count = count;
		}

		public string TestName 
		{
			get { return testName; }
		}

		public int TestCount 
		{
			get { return count; }
		}
	}
}
