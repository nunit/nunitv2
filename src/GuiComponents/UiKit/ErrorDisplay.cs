// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

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
	/// Summary description for ErrorDisplay.
	/// </summary>
	public class ErrorDisplay : System.Windows.Forms.UserControl, TestObserver
	{
		private ISettings settings = null;
		int hoverIndex = -1;
		private System.Windows.Forms.Timer hoverTimer;
		TipWindow tipWindow;
		private bool wordWrap = false;

		private System.Windows.Forms.ListBox detailList;
		public CP.Windows.Forms.ExpandingTextBox stackTrace;
		public System.Windows.Forms.Splitter tabSplitter;
		private System.Windows.Forms.ContextMenu detailListContextMenu;
		private System.Windows.Forms.MenuItem copyDetailMenuItem;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ErrorDisplay()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
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

		#region Properties
		private bool WordWrap
		{
			get { return wordWrap; }
			set 
			{ 
				if ( value != this.wordWrap )
				{
					this.wordWrap = value; 
					this.stackTrace.WordWrap = value;
					RefillDetailList();
				}
			}
		}
		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.detailList = new System.Windows.Forms.ListBox();
			this.tabSplitter = new System.Windows.Forms.Splitter();
			this.stackTrace = new CP.Windows.Forms.ExpandingTextBox();
			this.detailListContextMenu = new System.Windows.Forms.ContextMenu();
			this.copyDetailMenuItem = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
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
			this.detailList.Size = new System.Drawing.Size(496, 128);
			this.detailList.TabIndex = 1;
			this.detailList.Resize += new System.EventHandler(this.detailList_Resize);
			this.detailList.MouseHover += new System.EventHandler(this.OnMouseHover);
			this.detailList.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.detailList_MeasureItem);
			this.detailList.MouseMove += new System.Windows.Forms.MouseEventHandler(this.detailList_MouseMove);
			this.detailList.MouseLeave += new System.EventHandler(this.detailList_MouseLeave);
			this.detailList.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.detailList_DrawItem);
			this.detailList.SelectedIndexChanged += new System.EventHandler(this.detailList_SelectedIndexChanged);
			// 
			// tabSplitter
			// 
			this.tabSplitter.Dock = System.Windows.Forms.DockStyle.Top;
			this.tabSplitter.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.tabSplitter.Location = new System.Drawing.Point(0, 128);
			this.tabSplitter.MinSize = 100;
			this.tabSplitter.Name = "tabSplitter";
			this.tabSplitter.Size = new System.Drawing.Size(496, 9);
			this.tabSplitter.TabIndex = 3;
			this.tabSplitter.TabStop = false;
			this.tabSplitter.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.tabSplitter_SplitterMoved);
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
			this.stackTrace.Size = new System.Drawing.Size(496, 151);
			this.stackTrace.TabIndex = 2;
			this.stackTrace.Text = "";
			this.stackTrace.WordWrap = false;
			this.stackTrace.KeyUp += new System.Windows.Forms.KeyEventHandler(this.stackTrace_KeyUp);
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
			// ErrorDisplay
			// 
			this.Controls.Add(this.stackTrace);
			this.Controls.Add(this.tabSplitter);
			this.Controls.Add(this.detailList);
			this.Name = "ErrorDisplay";
			this.Size = new System.Drawing.Size(496, 288);
			this.ResumeLayout(false);

		}
		#endregion

		#region Form Level Events
		protected override void OnLoad(EventArgs e)
		{
			// NOTE: DesignMode is not true when display is nested in another
			// user control and the containing form is displayed in the designer.
			// This is a problem with VS.Net.
			//
			// Consequently, we rely on the fact that Services.UserSettings
			// returns a dummy Service, if the ServiceManager has not been
			// initialized.
			if ( !this.DesignMode )
			{
				this.settings = Services.UserSettings;
				settings.Changed += new SettingsEventHandler(UserSettings_Changed);

				int splitPosition = settings.GetSetting( "Gui.ResultTabs.ErrorsTabSplitterPosition", tabSplitter.SplitPosition );
				if ( splitPosition >= tabSplitter.MinSize && splitPosition < this.ClientSize.Height )
					this.tabSplitter.SplitPosition = splitPosition;

				stackTrace.AutoExpand = settings.GetSetting( "Gui.ResultTabs.ErrorsTab.ToolTipsEnabled", true );
				this.WordWrap = settings.GetSetting( "Gui.ResultTabs.ErrorsTab.WordWrapEnabled", true );
			}

			base.OnLoad (e);
		}
		#endregion

		#region Public Methods
		public void Clear()
		{
			detailList.Items.Clear();
			detailList.ContextMenu = null;
			stackTrace.Text = "";
		}
		#endregion

		#region UserSettings Events
		private void UserSettings_Changed( object sender, SettingsEventArgs args )
		{
			this.stackTrace.AutoExpand = settings.GetSetting( "Gui.ResultTabs.ErrorsTab.ToolTipsEnabled ", false );
			this.WordWrap = settings.GetSetting( "Gui.ResultTabs.ErrorsTab.WordWrapEnabled", true );
		}
		#endregion

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
			SizeF size = this.WordWrap
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
				if ( this.WordWrap && layoutRect.Width > detailList.ClientSize.Width )
					layoutRect.Width = detailList.ClientSize.Width;
				e.Graphics.DrawString(item.ToString(),detailList.Font, brush, layoutRect);
				
			}
		}

		private void detailList_Resize(object sender, System.EventArgs e)
		{
			if ( this.WordWrap ) RefillDetailList();
		}

		private void RefillDetailList()
		{
			if ( this.detailList.Items.Count > 0 )
			{
				this.detailList.BeginUpdate();
				ArrayList copiedItems = new ArrayList( detailList.Items );
				this.detailList.Items.Clear();
				foreach( object item in copiedItems )
					this.detailList.Items.Add( item );
				this.detailList.EndUpdate();
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

		private void tabSplitter_SplitterMoved( object sender, SplitterEventArgs e )
		{
			settings.SaveSetting( "Gui.ResultTabs.ErrorsTabSplitterPosition", tabSplitter.SplitPosition );
		}

		#endregion

		#region TestObserver Interface
		public void Subscribe(ITestEvents events)
		{
			events.TestFinished += new TestEventHandler(OnTestFinished);
			events.SuiteFinished += new TestEventHandler(OnSuiteFinished);
			events.TestException += new TestEventHandler(OnTestException);
		}
		#endregion

		#region Test Event Handlers
		private void OnTestFinished(object sender, TestEventArgs args)
		{
			TestResult result = args.Result;
			if( result.Executed && result.IsFailure && result.FailureSite != FailureSite.Parent )
				InsertTestResultItem( result );
		}
		
		private void OnSuiteFinished(object sender, TestEventArgs args)
		{
			TestResult result = args.Result;
			if( result.Executed && result.IsFailure && 
				result.FailureSite != FailureSite.Child )
				InsertTestResultItem( result );
		}
		
		private void OnTestException(object sender, TestEventArgs args)
		{
			string msg = string.Format( "An unhandled {0} was thrown while executing this test : {1}",
				args.Exception.GetType().FullName, args.Exception.Message );
			TestResultItem item = new TestResultItem( args.Name, msg, args.Exception.StackTrace );
				
			InsertTestResultItem( item );
		}

		private void InsertTestResultItem( TestResult result )
		{
			TestResultItem item = new TestResultItem(result);
			InsertTestResultItem( item );
		}

		private void InsertTestResultItem( TestResultItem item )
		{
			detailList.BeginUpdate();
			detailList.Items.Insert(detailList.Items.Count, item);
			detailList.EndUpdate();
		}
		#endregion
	}
}
