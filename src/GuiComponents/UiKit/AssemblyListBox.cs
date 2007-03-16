// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using CP.Windows.Forms;

namespace NUnit.UiKit
{
	/// <summary>
	/// Summary description for AssemblyListBox.
	/// </summary>
	public class AssemblyListBox : ListBox
	{
		#region Instance Variables

		/// <summary>
		/// The item index which the mouse is currently over
		/// </summary>
		private int hoverIndex = -1;

		/// <summary>
		/// Our private TipWindow
		/// </summary>
		private TipWindow tipWindow;

		/// <summary>
		/// Time in milliseconds that the mouse must
		/// be stationary over an item before the
		/// tip window will display.
		/// </summary>
		private int mouseHoverDelay = 300;

		/// <summary>
		/// Time in milliseconds that the tip window
		/// will remain displayed.
		/// </summary>
		private int autoCloseDelay = 0;

		/// <summary>
		/// Time in milliseconds that the window stays
		/// open after the mouse leaves an item.
		/// </summary>
		private int mouseLeaveDelay = 300;

		/// <summary>
		/// Timer used to control display behavior on hover.
		/// </summary>
		private System.Windows.Forms.Timer hoverTimer;

		#endregion

		#region Properties

		[Browsable( false )]
		public bool Expanded
		{
			get { return tipWindow != null && tipWindow.Visible; }
		}

		/// <summary>
		/// Time in milliseconds that the mouse must
		/// be stationary over an item before the
		/// tip window will display.
		/// </summary>
		[Category( "Behavior" ), DefaultValue( 300 )]
		[Description("Time in milliseconds that mouse must be stationary over an item before the tip is displayed.")]
		public int MouseHoverDelay
		{
			get { return mouseHoverDelay; }
			set { mouseHoverDelay = value; }
		}

		/// <summary>
		/// Time in milliseconds that the tip window
		/// will remain displayed.
		/// </summary>
		[Category( "Behavior" ), DefaultValue( 0 )]
		[Description("Time in milliseconds that the tip is displayed. Zero indicates no automatic timeout.")]
		public int AutoCloseDelay
		{
			get { return autoCloseDelay; }
			set { autoCloseDelay = value; }
		}

		/// <summary>
		/// Time in milliseconds that the window stays
		/// open after the mouse leaves the control.
		/// Reentering the control resets this.
		/// </summary>
		[Category( "Behavior" ), DefaultValue( 300 )]
		[Description("Time in milliseconds that the tip is displayed after the mouse levaes the control")]
		public int MouseLeaveDelay
		{
			get { return mouseLeaveDelay; }
			set { mouseLeaveDelay = value; }
		}

		#endregion

		protected void OnMouseHover( object sender, System.EventArgs e)
		{
			if ( tipWindow != null ) tipWindow.Close();

			if ( hoverIndex >= 0 )
			{
				Graphics g = Graphics.FromHwnd( Handle );

				Rectangle itemRect = GetItemRectangle( hoverIndex );
//				itemRect.Offset( 17, 0 );
//				itemRect.Width -= 17;

				string text = Items[hoverIndex] as string;

				SizeF sizeNeeded = g.MeasureString( text, Font );
				bool expansionNeeded = 
					itemRect.Width < (int)sizeNeeded.Width ||
					itemRect.Height < (int)sizeNeeded.Height;

				if ( expansionNeeded )
				{
					tipWindow = new TipWindow( this, hoverIndex );
					tipWindow.ItemBounds = itemRect;
					tipWindow.TipText = text;
					tipWindow.Expansion = TipWindow.ExpansionStyle.Horizontal;
					tipWindow.Overlay = true;
					tipWindow.WantClicks = true;
					tipWindow.AutoCloseDelay = this.AutoCloseDelay;
					tipWindow.MouseLeaveDelay = this.MouseLeaveDelay;
					tipWindow.Closed += new EventHandler( tipWindow_Closed );
					tipWindow.Show();
				}
			}		
		}
	
		private void tipWindow_Closed( object sender, System.EventArgs e )
		{
			tipWindow = null;
			ClearTimer();
		}

		protected override void OnMouseLeave( System.EventArgs  e)
		{
			ClearTimer();
		}

		protected override void OnMouseMove( System.Windows.Forms.MouseEventArgs e )
		{
			ClearTimer();

			hoverIndex = IndexFromPoint( e.X, e.Y );
			
			hoverTimer = new System.Windows.Forms.Timer();
			hoverTimer.Interval = mouseHoverDelay;
			hoverTimer.Tick += new EventHandler( OnMouseHover );
			hoverTimer.Start();
		}

		private void ClearTimer()
		{
			if ( hoverTimer != null )
			{
				hoverTimer.Stop();
				hoverTimer.Dispose();
			}
		}
	}
}
