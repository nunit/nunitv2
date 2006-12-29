using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using NUnit.Util;
using NUnit.Core;
using CP.Windows.Forms;

namespace NUnit.UiKit
{
	/// <summary>
	/// Summary description for ResultTabs.
	/// </summary>
	public class ResultTabs : System.Windows.Forms.UserControl, TestObserver
	{
		private ISettings settings;
		int hoverIndex = -1;
		private System.Windows.Forms.Timer hoverTimer;
		CP.Windows.Forms.TipWindow tipWindow;
		private MenuItem tabsMenu;
		private MenuItem errorsTabMenuItem;
		private MenuItem notRunTabMenuItem;
		private MenuItem consoleOutMenuItem;
		private MenuItem consoleErrorMenuItem;
		private MenuItem traceTabMenuItem;
		private MenuItem menuSeparator;
		private MenuItem internalTraceTabMenuItem;

		public System.Windows.Forms.TabPage errorTab;
		public CP.Windows.Forms.ExpandingTextBox stackTrace;
		public System.Windows.Forms.Splitter tabSplitter;
		private System.Windows.Forms.ListBox detailList;
		public NUnit.UiKit.RichEditTabPage stdoutTab;
		private NUnit.UiKit.RichEditTabPage traceTab;
		private NUnit.UiKit.RichEditTabPage internalTraceTab;
		public NUnit.UiKit.RichEditTabPage stderrTab;
		public System.Windows.Forms.TabPage notRunTab;
		public NUnit.UiKit.NotRunTree notRunTree;
		public System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.ContextMenu detailListContextMenu;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem copyDetailMenuItem;
		private System.Windows.Forms.PrintDialog printDialog1;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ResultTabs()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			if ( !this.DesignMode )
			{
				settings = NUnit.Util.Services.UserSettings;

				int splitPosition = settings.GetSetting( "Gui.ResultTabs.ErrorsTabSplitterPosition", tabSplitter.SplitPosition );
				if ( splitPosition >= tabSplitter.MinSize && splitPosition < this.ClientSize.Height )
					this.tabSplitter.SplitPosition = splitPosition;
			}

			this.tabsMenu = new MenuItem();
			this.errorsTabMenuItem = new System.Windows.Forms.MenuItem();
			this.notRunTabMenuItem = new System.Windows.Forms.MenuItem();
			this.consoleOutMenuItem = new System.Windows.Forms.MenuItem();
			this.consoleErrorMenuItem = new System.Windows.Forms.MenuItem();
			this.traceTabMenuItem = new System.Windows.Forms.MenuItem();
			this.menuSeparator = new System.Windows.Forms.MenuItem();
			this.internalTraceTabMenuItem = new System.Windows.Forms.MenuItem();

			this.tabsMenu.MergeType = MenuMerge.Add;
			this.tabsMenu.MergeOrder = 1;
			this.tabsMenu.MenuItems.AddRange(
				new System.Windows.Forms.MenuItem[] 
				{
					this.errorsTabMenuItem,
					this.notRunTabMenuItem,
					this.consoleOutMenuItem,
					this.consoleErrorMenuItem,
					this.traceTabMenuItem,
					this.menuSeparator,
					this.internalTraceTabMenuItem
				} );
			this.tabsMenu.Text = "&Result Tabs";
			this.tabsMenu.Visible = true;

			this.errorsTabMenuItem.Index = 0;
			this.errorsTabMenuItem.Text = "&Errors && Failures";
			this.errorsTabMenuItem.Click += new System.EventHandler(this.errorsTabMenuItem_Click);

			this.notRunTabMenuItem.Index = 1;
			this.notRunTabMenuItem.Text = "Tests &Not Run";
			this.notRunTabMenuItem.Click += new System.EventHandler(this.notRunTabMenuItem_Click);

			this.consoleOutMenuItem.Index = 2;
			this.consoleOutMenuItem.Text = "Console.&Out";
			this.consoleOutMenuItem.Click += new System.EventHandler(this.consoleOutMenuItem_Click);

			this.consoleErrorMenuItem.Index = 3;
			this.consoleErrorMenuItem.Text = "Console.&Error";
			this.consoleErrorMenuItem.Click += new System.EventHandler(this.consoleErrorMenuItem_Click);

			this.traceTabMenuItem.Index = 4;
			this.traceTabMenuItem.Text = "&Trace Output";
			this.traceTabMenuItem.Click += new System.EventHandler(this.traceTabMenuItem_Click);

			this.menuSeparator.Index = 5;
			this.menuSeparator.Text = "-";
			
			this.internalTraceTabMenuItem.Index = 6;
			this.internalTraceTabMenuItem.Text = "&Internal Trace";
			this.internalTraceTabMenuItem.Click += new System.EventHandler(this.internalTraceTabMenuItem_Click);
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tabControl = new System.Windows.Forms.TabControl();
			this.errorTab = new System.Windows.Forms.TabPage();
			this.stackTrace = new CP.Windows.Forms.ExpandingTextBox();
			this.tabSplitter = new System.Windows.Forms.Splitter();
			this.detailList = new System.Windows.Forms.ListBox();
			this.notRunTab = new System.Windows.Forms.TabPage();
			this.notRunTree = new NUnit.UiKit.NotRunTree();
			this.stdoutTab = new NUnit.UiKit.RichEditTabPage();
			this.stderrTab = new NUnit.UiKit.RichEditTabPage();
			this.traceTab = new NUnit.UiKit.RichEditTabPage();
			this.internalTraceTab = new NUnit.UiKit.RichEditTabPage();
			this.detailListContextMenu = new System.Windows.Forms.ContextMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.copyDetailMenuItem = new System.Windows.Forms.MenuItem();
			this.printDialog1 = new System.Windows.Forms.PrintDialog();
			this.tabControl.SuspendLayout();
			this.errorTab.SuspendLayout();
			this.notRunTab.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.errorTab);
			this.tabControl.Controls.Add(this.notRunTab);
			this.tabControl.Controls.Add(this.stdoutTab);
			this.tabControl.Controls.Add(this.stderrTab);
			this.tabControl.Controls.Add(this.traceTab);
			this.tabControl.Controls.Add(this.internalTraceTab);
			this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl.ForeColor = System.Drawing.Color.Red;
			this.tabControl.ItemSize = new System.Drawing.Size(99, 18);
			this.tabControl.Location = new System.Drawing.Point(0, 0);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(488, 304);
			this.tabControl.TabIndex = 3;
			// 
			// errorTab
			// 
			this.errorTab.Controls.Add(this.stackTrace);
			this.errorTab.Controls.Add(this.tabSplitter);
			this.errorTab.Controls.Add(this.detailList);
			this.errorTab.Location = new System.Drawing.Point(4, 22);
			this.errorTab.Name = "errorTab";
			this.errorTab.Size = new System.Drawing.Size(480, 278);
			this.errorTab.TabIndex = 0;
			this.errorTab.Text = "Errors and Failures";
			// 
			// stackTrace
			// 
			this.stackTrace.Dock = System.Windows.Forms.DockStyle.Fill;
			this.stackTrace.Font = new System.Drawing.Font("Courier New", 9.75F);
			this.stackTrace.Location = new System.Drawing.Point(0, 137);
			this.stackTrace.Multiline = true;
			this.stackTrace.Name = "stackTrace";
			this.stackTrace.ReadOnly = true;
			this.stackTrace.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.stackTrace.Size = new System.Drawing.Size(480, 141);
			this.stackTrace.TabIndex = 2;
			this.stackTrace.Text = "";
			this.stackTrace.WordWrap = false;
			// 
			// tabSplitter
			// 
			this.tabSplitter.Dock = System.Windows.Forms.DockStyle.Top;
			this.tabSplitter.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.tabSplitter.Location = new System.Drawing.Point(0, 128);
			this.tabSplitter.MinSize = 100;
			this.tabSplitter.Name = "tabSplitter";
			this.tabSplitter.Size = new System.Drawing.Size(480, 9);
			this.tabSplitter.TabIndex = 1;
			this.tabSplitter.TabStop = false;
			this.tabSplitter.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.tabSplitter_SplitterMoved);
			// 
			// detailList
			// 
			this.detailList.Dock = System.Windows.Forms.DockStyle.Top;
			this.detailList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
			this.detailList.Font = new System.Drawing.Font("Courier New", 8.25F);
			this.detailList.HorizontalExtent = 2000;
			this.detailList.HorizontalScrollbar = true;
			this.detailList.ItemHeight = 16;
			this.detailList.Location = new System.Drawing.Point(0, 0);
			this.detailList.Name = "detailList";
			this.detailList.ScrollAlwaysVisible = true;
			this.detailList.Size = new System.Drawing.Size(480, 128);
			this.detailList.TabIndex = 0;
			this.detailList.MouseHover += new System.EventHandler(this.OnMouseHover);
			this.detailList.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.detailList_MeasureItem);
			this.detailList.MouseMove += new System.Windows.Forms.MouseEventHandler(this.detailList_MouseMove);
			this.detailList.MouseLeave += new System.EventHandler(this.detailList_MouseLeave);
			this.detailList.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.detailList_DrawItem);
			this.detailList.SelectedIndexChanged += new System.EventHandler(this.detailList_SelectedIndexChanged);
			// 
			// notRunTab
			// 
			this.notRunTab.Controls.Add(this.notRunTree);
			this.notRunTab.Location = new System.Drawing.Point(4, 22);
			this.notRunTab.Name = "notRunTab";
			this.notRunTab.Size = new System.Drawing.Size(480, 278);
			this.notRunTab.TabIndex = 1;
			this.notRunTab.Text = "Tests Not Run";
			this.notRunTab.Visible = false;
			// 
			// notRunTree
			// 
			this.notRunTree.Dock = System.Windows.Forms.DockStyle.Fill;
			this.notRunTree.ImageIndex = -1;
			this.notRunTree.Indent = 19;
			this.notRunTree.ItemHeight = 16;
			this.notRunTree.Location = new System.Drawing.Point(0, 0);
			this.notRunTree.Name = "notRunTree";
			this.notRunTree.SelectedImageIndex = -1;
			this.notRunTree.Size = new System.Drawing.Size(480, 278);
			this.notRunTree.TabIndex = 0;
			// 
			// stdoutTab
			// 
			this.stdoutTab.Location = new System.Drawing.Point(4, 22);
			this.stdoutTab.Name = "stdoutTab";
			this.stdoutTab.Size = new System.Drawing.Size(480, 278);
			this.stdoutTab.TabIndex = 3;
			this.stdoutTab.Text = "Console.Out";
			this.stdoutTab.Visible = false;
			// 
			// stderrTab
			// 
			this.stderrTab.Location = new System.Drawing.Point(4, 22);
			this.stderrTab.Name = "stderrTab";
			this.stderrTab.Size = new System.Drawing.Size(480, 278);
			this.stderrTab.TabIndex = 2;
			this.stderrTab.Text = "Console.Error";
			this.stderrTab.Visible = false;
			// 
			// traceTab
			// 
			this.traceTab.Location = new System.Drawing.Point(4, 22);
			this.traceTab.Name = "traceTab";
			this.traceTab.Size = new System.Drawing.Size(480, 278);
			this.traceTab.TabIndex = 4;
			this.traceTab.Text = "Trace Output";
			this.traceTab.Visible = false;
			// 
			// internalTraceTab
			// 
			this.internalTraceTab.Location = new System.Drawing.Point(4, 22);
			this.internalTraceTab.Name = "internalTraceTab";
			this.internalTraceTab.Size = new System.Drawing.Size(480, 278);
			this.internalTraceTab.TabIndex = 5;
			this.internalTraceTab.Text = "Internal Trace";
			this.internalTraceTab.Visible = false;
			// 
			// detailListContextMenu
			// 
			this.detailListContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																								  this.menuItem1});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.copyDetailMenuItem});
			this.menuItem1.Text = "";
			// 
			// copyDetailMenuItem
			// 
			this.copyDetailMenuItem.Index = 0;
			this.copyDetailMenuItem.Text = "Copy";
			this.copyDetailMenuItem.Click += new System.EventHandler(this.copyDetailMenuItem_Click);
			// 
			// ResultTabs
			// 
			this.Controls.Add(this.tabControl);
			this.Name = "ResultTabs";
			this.Size = new System.Drawing.Size(488, 304);
			this.tabControl.ResumeLayout(false);
			this.errorTab.ResumeLayout(false);
			this.notRunTab.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
	
		public void Clear()
		{
			detailList.Items.Clear();
			detailList.ContextMenu = null;
//			toolTip.SetToolTip( detailList, null );
			notRunTree.Nodes.Clear();

			stdoutTab.Clear();
			stderrTab.Clear();
			traceTab.Clear();
			
			stackTrace.Text = "";
		}

		public MenuItem TabsMenu
		{
			get { return tabsMenu; }
		}

		public bool AutoExpand
		{
			get { return stackTrace.AutoExpand; }
			set { stackTrace.AutoExpand = value; }
		}

		public bool WordWrap
		{
			get { return stackTrace.WordWrap; }
			set { stackTrace.WordWrap = value; }
		}

		public void LoadSettingsAndUpdateTabPages()
		{
			errorsTabMenuItem.Checked = settings.GetSetting( "Gui.ResultTabs.DisplayErrorsTab", true );
			notRunTabMenuItem.Checked = settings.GetSetting( "Gui.ResultTabs.DisplayNotRunTab", true );
			consoleOutMenuItem.Checked = settings.GetSetting( "Gui.ResultTabs.DisplayConsoleOutputTab", true );
			consoleErrorMenuItem.Checked = settings.GetSetting( "Gui.ResultTabs.DisplayConsoleErrorTab", true );
			traceTabMenuItem.Checked = settings.GetSetting( "Gui.ResultTabs.DisplayTraceTab", true );
			internalTraceTabMenuItem.Checked = settings.GetSetting( "Gui.ResultTabs.DisplayInternalTraceTab", true );
			UpdateTabPages();
		}

		public void UpdateTabPages()
		{
			tabControl.TabPages.Clear();
			string selectedTab = settings.GetSetting( "Gui.ResultTabs.SelectedTab", "" );

			if ( errorsTabMenuItem.Checked )
				tabControl.TabPages.Add( errorTab );
			if ( notRunTabMenuItem.Checked )
				tabControl.TabPages.Add( notRunTab );
			if ( consoleOutMenuItem.Checked )
				tabControl.TabPages.Add( stdoutTab );
			if ( consoleErrorMenuItem.Checked )
				tabControl.TabPages.Add( stderrTab );
			if ( traceTabMenuItem.Checked )
				tabControl.TabPages.Add( traceTab );
			if ( internalTraceTabMenuItem.Checked )
				tabControl.TabPages.Add( internalTraceTab );
		}

		public void OnOptionsChanged()
		{
			LoadSettingsAndUpdateTabPages();

			this.stackTrace.AutoExpand = settings.GetSetting( "Gui.ResultTabs.ErrorsTab.ToolTipsEnabled ", false );
			bool wordWrap = settings.GetSetting( "Gui.ResultTabs.ErrorsTab.WordWrapEnabled", true );
		
			if ( this.stackTrace.WordWrap != wordWrap )
			{
				this.stackTrace.WordWrap = wordWrap;

				this.detailList.BeginUpdate();
				ArrayList copiedItems = new ArrayList( detailList.Items );
				this.detailList.Items.Clear();
				foreach( object item in copiedItems )
					this.detailList.Items.Add( item );
				this.detailList.EndUpdate();
				this.stackTrace.WordWrap = wordWrap;
			}
		}

		public void InsertTestResultItem( TestResult result )
		{
			TestResultItem item = new TestResultItem(result);
			detailList.BeginUpdate();
			detailList.Items.Insert(detailList.Items.Count, item);
			detailList.EndUpdate();
		}

		#region DetailList Events
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
			SizeF size = settings.GetSetting( "Gui.ResultTabs.ErrorsTab.WordWrapEnabled", false )
				? e.Graphics.MeasureString(item.ToString(), detailList.Font, detailList.ClientSize.Width )
				: e.Graphics.MeasureString(item.ToString(), detailList.Font );
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
				Brush brush = selected ? SystemBrushes.HighlightText : SystemBrushes.WindowText;
				RectangleF layoutRect = e.Bounds;
				if (settings.GetSetting( "Gui.ResultTabs.ErrorsTab.WordWrapEnabled", true ) && layoutRect.Width > detailList.ClientSize.Width )
					layoutRect.Width = detailList.ClientSize.Width;
				e.Graphics.DrawString(item.ToString(),detailList.Font, brush, layoutRect);
				
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

			if ( settings.GetSetting( "Gui.ResultTabs.ErrorsTab.ToolTipsEnabled", false ) && hoverIndex >= 0 && hoverIndex < detailList.Items.Count )
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

		private void stackTrace_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if ( e.KeyCode == Keys.A && e.Modifiers == Keys.Control )
			{
				stackTrace.SelectAll();
			}
		}

		//		private void enableWordWrapCheckBox_CheckedChanged(object sender, System.EventArgs e)
		//		{
		//			this.detailList.BeginUpdate();
		//			ArrayList copiedItems = new ArrayList( detailList.Items );
		//			this.detailList.Items.Clear();
		//			foreach( object item in copiedItems )
		//				this.detailList.Items.Add( item );
		//			this.detailList.EndUpdate();
		//			this.stackTrace.WordWrap = this.enableWordWrapCheckBox.Checked;
		//		}

		#endregion

		private void tabSplitter_SplitterMoved( object sender, SplitterEventArgs e )
		{
			settings.SaveSetting( "Gui.ResultTabs.ErrorsTabSplitterPosition", tabSplitter.SplitPosition );
		}

		private void errorsTabMenuItem_Click(object sender, System.EventArgs e)
		{
			settings.SaveSetting( "Gui.ResultTabs.DisplayErrorsTab", errorsTabMenuItem.Checked = !errorsTabMenuItem.Checked );
			UpdateTabPages();
		}

		private void notRunTabMenuItem_Click(object sender, System.EventArgs e)
		{
			settings.SaveSetting( "Gui.ResultTabs.DisplayNotRunTab", notRunTabMenuItem.Checked = !notRunTabMenuItem.Checked );
			UpdateTabPages();
		}

		private void consoleOutMenuItem_Click(object sender, System.EventArgs e)
		{
			settings.SaveSetting( "Gui.ResultTabs.DisplayConsoleOutputTab", consoleOutMenuItem.Checked = !consoleOutMenuItem.Checked );
			UpdateTabPages();
		}

		private void consoleErrorMenuItem_Click(object sender, System.EventArgs e)
		{
			settings.SaveSetting( "Gui.ResultTabs.DisplayConsoleErrorTab", consoleErrorMenuItem.Checked = !consoleErrorMenuItem.Checked );
			UpdateTabPages();
		}

		private void traceTabMenuItem_Click(object sender, System.EventArgs e)
		{
			settings.SaveSetting( "Gui.ResultTabs.DisplayTraceTab", traceTabMenuItem.Checked = !traceTabMenuItem.Checked );
			UpdateTabPages();
		}

		private void internalTraceTabMenuItem_Click(object sender, System.EventArgs e)
		{
			settings.SaveSetting( "Gui.ResultTabs.DisplayInternalTraceTab", internalTraceTabMenuItem.Checked = !internalTraceTabMenuItem.Checked );
			UpdateTabPages();
		}

		#region TestObserver Members

		public void Subscribe(ITestEvents events)
		{
			events.TestStarting += new TestEventHandler(OnTestStarting);
			events.TestFinished += new TestEventHandler(OnTestFinished);
			events.SuiteFinished += new TestEventHandler(OnSuiteFinished);
			events.TestOutput += new TestEventHandler(OnTestOutput);
			events.TestReloaded += new TestEventHandler(OnTestReloaded);
		}

		private void OnTestStarting(object sender, TestEventArgs args)
		{
			if ( settings.GetSetting( "Gui.ResultTabs.DisplayTestLabels", false ) )
			{
				this.stdoutTab.AppendText( string.Format( "***** {0}\n", args.TestName.FullName ) );
			}
		}

		private void OnTestFinished(object sender, TestEventArgs args)
		{
			TestResult result = args.Result;
			if(result.Executed)
			{
				if(result.IsFailure && result.FailureSite != FailureSite.Parent )
					InsertTestResultItem( result );
			}
			else
			{
				notRunTree.Add( result );
			}
		}

		private void OnSuiteFinished(object sender, TestEventArgs args)
		{
			TestResult result = args.Result;
			if(result.Executed)
			{
				if ( result.IsFailure && result.FailureSite != FailureSite.Child )
					InsertTestResultItem(result);
			}
			else
			{
				notRunTree.Add( result );
			}
		}

		private void OnTestReloaded(object sender, TestEventArgs args)
		{
			if ( settings.GetSetting( "Options.TestLoader.ClearResultsOnReload", false ) )
				this.Clear();
		}

		private void OnTestOutput(object sender, TestEventArgs args)
		{
			TestOutput output = args.TestOutput;
			switch(output.Type)
			{
				case TestOutputType.Out:
					this.stdoutTab.AppendText( output.Text );
					break;
				case TestOutputType.Error:
					if ( settings.GetSetting( "Gui.ResultTabs.MergeErrorOutput", false ) )
						this.stdoutTab.AppendText( output.Text );
					else
						this.stderrTab.AppendText( output.Text );
					break;
				case TestOutputType.Trace:
					if ( settings.GetSetting( "Gui.ResultTabs.MergeTraceOutput", false ) )
						this.stdoutTab.AppendText( output.Text );
					else
						this.traceTab.AppendText( output.Text );
					break;
			}
		}
		#endregion
	}
}
