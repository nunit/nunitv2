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
using System.Diagnostics;

namespace NUnit.UiKit
{
	/// <summary>
	/// Summary description for ResultTabs.
	/// </summary>
	public class ResultTabs : System.Windows.Forms.UserControl, TestObserver
	{
		private ISettings settings;
		private TraceListener traceListener;

		private MenuItem tabsMenu;
		private MenuItem errorsTabMenuItem;
		private MenuItem notRunTabMenuItem;
		private MenuItem consoleOutMenuItem;
		private MenuItem consoleErrorMenuItem;
		private MenuItem traceTabMenuItem;
		private MenuItem menuSeparator;
		private MenuItem internalTraceTabMenuItem;

		private System.Windows.Forms.TabPage errorTab;
		private NUnit.UiKit.ErrorDisplay errorDisplay;
		private NUnit.UiKit.RichEditTabPage stdoutTab;
		private NUnit.UiKit.RichEditTabPage traceTab;
		private NUnit.UiKit.RichEditTabPage internalTraceTab;
		private NUnit.UiKit.RichEditTabPage stderrTab;
		private System.Windows.Forms.TabPage notRunTab;
		private NUnit.UiKit.NotRunTree notRunTree;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.MenuItem copyDetailMenuItem;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ResultTabs()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

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

			this.traceListener = new TextWriterTraceListener( this.internalTraceTab.Writer, "Internal" );
			System.Diagnostics.Trace.Listeners.Add( this.traceListener );
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
			this.errorDisplay = new NUnit.UiKit.ErrorDisplay();
			this.notRunTab = new System.Windows.Forms.TabPage();
			this.notRunTree = new NUnit.UiKit.NotRunTree();
			this.stdoutTab = new NUnit.UiKit.RichEditTabPage();
			this.stderrTab = new NUnit.UiKit.RichEditTabPage();
			this.traceTab = new NUnit.UiKit.RichEditTabPage();
			this.internalTraceTab = new NUnit.UiKit.RichEditTabPage();
			this.copyDetailMenuItem = new System.Windows.Forms.MenuItem();
			this.tabControl.SuspendLayout();
			this.errorTab.SuspendLayout();
			this.notRunTab.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.errorTab);
			this.tabControl.Controls.Add(this.stdoutTab);
			this.tabControl.Controls.Add(this.stderrTab);
			this.tabControl.Controls.Add(this.notRunTab);
			this.tabControl.Controls.Add(this.traceTab);
			this.tabControl.Controls.Add(this.internalTraceTab);
			this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl.ForeColor = System.Drawing.Color.Red;
			this.tabControl.ItemSize = new System.Drawing.Size(99, 18);
			this.tabControl.Location = new System.Drawing.Point(0, 0);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(488, 280);
			this.tabControl.TabIndex = 3;
			// 
			// errorTab
			// 
			this.errorTab.Controls.Add(this.errorDisplay);
			this.errorTab.Location = new System.Drawing.Point(4, 22);
			this.errorTab.Name = "errorTab";
			this.errorTab.Size = new System.Drawing.Size(480, 254);
			this.errorTab.TabIndex = 0;
			this.errorTab.Text = "Errors and Failures";
			// 
			// errorDisplay
			// 
			this.errorDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
			this.errorDisplay.Location = new System.Drawing.Point(0, 0);
			this.errorDisplay.Name = "errorDisplay";
			this.errorDisplay.Size = new System.Drawing.Size(480, 254);
			this.errorDisplay.TabIndex = 0;
			// 
			// notRunTab
			// 
			this.notRunTab.Controls.Add(this.notRunTree);
			this.notRunTab.Location = new System.Drawing.Point(4, 22);
			this.notRunTab.Name = "notRunTab";
			this.notRunTab.Size = new System.Drawing.Size(480, 254);
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
			this.notRunTree.Size = new System.Drawing.Size(480, 254);
			this.notRunTree.TabIndex = 0;
			// 
			// stdoutTab
			// 
			this.stdoutTab.Font = new System.Drawing.Font("Courier New", 8F);
			this.stdoutTab.ForeColor = System.Drawing.SystemColors.ControlText;
			this.stdoutTab.Location = new System.Drawing.Point(4, 22);
			this.stdoutTab.Name = "stdoutTab";
			this.stdoutTab.Size = new System.Drawing.Size(480, 254);
			this.stdoutTab.TabIndex = 3;
			this.stdoutTab.Text = "Console.Out";
			this.stdoutTab.Visible = false;
			// 
			// stderrTab
			// 
			this.stderrTab.Font = new System.Drawing.Font("Courier New", 8F);
			this.stderrTab.Location = new System.Drawing.Point(4, 22);
			this.stderrTab.Name = "stderrTab";
			this.stderrTab.Size = new System.Drawing.Size(480, 254);
			this.stderrTab.TabIndex = 2;
			this.stderrTab.Text = "Console.Error";
			this.stderrTab.Visible = false;
			// 
			// traceTab
			// 
			this.traceTab.Font = new System.Drawing.Font("Courier New", 8F);
			this.traceTab.Location = new System.Drawing.Point(4, 22);
			this.traceTab.Name = "traceTab";
			this.traceTab.Size = new System.Drawing.Size(480, 254);
			this.traceTab.TabIndex = 4;
			this.traceTab.Text = "Trace Output";
			this.traceTab.Visible = false;
			// 
			// internalTraceTab
			// 
			this.internalTraceTab.Font = new System.Drawing.Font("Courier New", 8F);
			this.internalTraceTab.Location = new System.Drawing.Point(4, 22);
			this.internalTraceTab.Name = "internalTraceTab";
			this.internalTraceTab.Size = new System.Drawing.Size(480, 254);
			this.internalTraceTab.TabIndex = 5;
			this.internalTraceTab.Text = "Internal Trace";
			this.internalTraceTab.Visible = false;
			// 
			// copyDetailMenuItem
			// 
			this.copyDetailMenuItem.Index = -1;
			this.copyDetailMenuItem.Text = "Copy";
			// 
			// ResultTabs
			// 
			this.Controls.Add(this.tabControl);
			this.Name = "ResultTabs";
			this.Size = new System.Drawing.Size(488, 280);
			this.tabControl.ResumeLayout(false);
			this.errorTab.ResumeLayout(false);
			this.notRunTab.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
	
		public void Clear()
		{
			errorDisplay.Clear();
			notRunTree.Nodes.Clear();

			stdoutTab.Clear();
			stderrTab.Clear();
			traceTab.Clear();
		}

		public MenuItem TabsMenu
		{
			get { return tabsMenu; }
		}

		protected override void OnLoad(EventArgs e)
		{
			if ( !this.DesignMode )
			{
				this.settings = Services.UserSettings;

				LoadSettingsAndUpdateTabPages();
				UpdateFixedFont();
				Subscribe( Services.TestLoader.Events );
				Services.UserSettings.Changed += new SettingsEventHandler(UserSettings_Changed);

				errorDisplay.Subscribe( Services.TestLoader.Events );
				notRunTree.Subscribe( Services.TestLoader.Events );
			}

			base.OnLoad (e);
		}

		private void LoadSettingsAndUpdateTabPages()
		{
			errorsTabMenuItem.Checked = settings.GetSetting( "Gui.ResultTabs.DisplayErrorsTab", true );
			notRunTabMenuItem.Checked = settings.GetSetting( "Gui.ResultTabs.DisplayNotRunTab", true );
			consoleOutMenuItem.Checked = settings.GetSetting( "Gui.ResultTabs.DisplayConsoleOutputTab", true );
			consoleErrorMenuItem.Checked = settings.GetSetting( "Gui.ResultTabs.DisplayConsoleErrorTab", true );
			traceTabMenuItem.Checked = settings.GetSetting( "Gui.ResultTabs.DisplayTraceTab", true );
			internalTraceTabMenuItem.Checked = settings.GetSetting( "Gui.ResultTabs.DisplayInternalTraceTab", true );

			UpdateTabPages();
		}

		private void UpdateFixedFont()
		{
			Font fixedFont = null;
			string fontDescription = settings.GetSetting( "Gui.FixedFont", "" );
			if ( fontDescription == "" )
			{
				fixedFont = new Font( "Courier New", 8.0f );
			}
			else
			{
				TypeConverter converter = TypeDescriptor.GetConverter(typeof(Font));
				fixedFont = (Font)converter.ConvertFrom(fontDescription);
			}

			stdoutTab.Font = stderrTab.Font = traceTab.Font = internalTraceTab.Font = fixedFont;
		}

		private void UpdateTabPages()
		{
			tabControl.TabPages.Clear();

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

		private void UserSettings_Changed( object sender, SettingsEventArgs e )
		{
			if( e.SettingName.StartsWith( "Gui.ResultTabs" ) )
				LoadSettingsAndUpdateTabPages();
			else if ( e.SettingName == "Gui.FixedFont" )
				UpdateFixedFont();
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
			events.TestLoaded += new TestEventHandler(OnTestLoaded);
			events.TestUnloaded += new TestEventHandler(OnTestUnloaded);
			events.TestReloaded += new TestEventHandler(OnTestReloaded);
			events.RunStarting += new TestEventHandler(OnRunStarting);
			events.TestStarting += new TestEventHandler(OnTestStarting);
			events.TestOutput += new TestEventHandler(OnTestOutput);
		}

		private void OnRunStarting(object sender, TestEventArgs args)
		{
			this.Clear();
		}

		private void OnTestStarting(object sender, TestEventArgs args)
		{
			if ( settings.GetSetting( "Gui.ResultTabs.DisplayTestLabels", false ) )
			{
				//this.stdoutTab.AppendText( string.Format( "***** {0}\n", args.TestName.FullName ) );
				this.stdoutTab.Writer.WriteLine( "***** {0}", args.TestName.FullName );
			}
		}

		private void OnTestLoaded(object sender, TestEventArgs args)
		{
			this.Clear();
		}

		private void OnTestUnloaded(object sender, TestEventArgs args)
		{
			this.Clear();
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
					this.stdoutTab.Writer.Write( output.Text );
					break;
				case TestOutputType.Error:
					if ( settings.GetSetting( "Gui.ResultTabs.MergeErrorOutput", false ) )
						this.stdoutTab.Writer.Write( output.Text );
					else
						this.stderrTab.Writer.Write( output.Text );
					break;
				case TestOutputType.Trace:
					if ( settings.GetSetting( "Gui.ResultTabs.MergeTraceOutput", false ) )
						this.stdoutTab.Writer.Write( output.Text );
					else
						this.traceTab.Writer.Write( output.Text );
					break;
			}
		}
		#endregion
	}
}
