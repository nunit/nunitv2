#region Copyright (c) 2002-2003 Charlie Poole
/************************************************************************************
'
' Copyright (c) 2002-2003 Charlie Poole
'
' Later versions may be available at http://charliepoole.org.
'
' This software is provided 'as-is', without any express or implied warranty. In no 
' event will the author be held liable for any damages arising from the use of this 
' software.
' 
' Permission is granted to anyone to use this software for any purpose, including 
' commercial applications, and to alter it and redistribute it freely, subject to the 
' following restrictions:
'
' 1. The origin of this software must not be misrepresented; you must not claim that
' you wrote the original software. If you use this software in a product, you must
' include the following notice in the product documentation and/or other materials
' provided with the distribution.
'
' Portions Copyright (c) 2002-2003 Charlie Poole
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed from or altered in any source distribution.
'
'***********************************************************************************/
#endregion

using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace CP.Windows.Forms
{
	/// <summary>
	/// A special type of label which can display a tooltip-like
	/// window to show the full extent of any text which doesn't 
	/// fit. The window may be placed directly over the label
	/// or immediately beneath it and will expand to fit in
	/// a horizontal, vertical or both directions as needed.
	/// </summary>
	public class ExpandingLabel : System.Windows.Forms.Label
	{
		/// <summary>
		/// Direction in which to expand
		/// </summary>
		public enum ExpansionStyle
		{
			Horizontal,
			Vertical,
			Both
		}

		#region Instance Variables

		/// <summary>
		/// Our window for displaying expanded text
		/// </summary>
		private TipWindow tipWindow;
		
		/// <summary>
		/// Direction of expansion
		/// </summary>
		private ExpansionStyle expansion = ExpansionStyle.Horizontal;
		
		/// <summary>
		/// True if tipWindow may overlay the label
		/// </summary>
		private bool overlay = true;
		
		/// <summary>
		/// Time in milliseconds that the tip window
		/// will remain displayed.
		/// </summary>
		private int autoCloseDelay = 0;

		/// <summary>
		/// Time in milliseconds that the window stays
		/// open after the mouse leaves the control.
		/// </summary>
		private int mouseLeaveDelay = 300;

		#endregion

		#region Properties

		[Browsable( false )]
		public bool Expanded
		{
			get { return tipWindow != null && tipWindow.Visible; }
		}

		[Category ( "Behavior"  ), DefaultValue( ExpansionStyle.Horizontal )]
		public ExpansionStyle Expansion
		{
			get { return expansion; }
			set { expansion = value; }
		}

		[Category( "Behavior" ), DefaultValue( true )]
		[Description("Indicates whether the tip window should overlay the label")]
		public bool Overlay
		{
			get { return overlay; }
			set { overlay = value; }
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

		#region Public Methods

		public void Expand()
		{
			if ( !Expanded )
			{
				tipWindow = new TipWindow( this );
				tipWindow.Closed += new EventHandler( tipWindow_Closed );
				tipWindow.Show();
			}
		}

		public void Unexpand()
		{
			if ( Expanded )
			{
				tipWindow.Close();
			}
		}

		#endregion

		#region Nested TipWindow class

		private class TipWindow : Form
		{
			#region Instance Variables

			/// <summary>
			/// The label for which we are showing expanded text
			/// </summary>
			private ExpandingLabel label;
			private Control control;

			/// <summary>
			/// True if we may overlay label
			/// </summary>
			private bool overlay;
			
			/// <summary>
			/// Directions we are allowed to expand
			/// </summary>
			private ExpansionStyle expansion;

			/// <summary>
			/// Time before automatically closing
			/// </summary>
			private int autoCloseDelay;

			/// <summary>
			/// Timer used for auto-close
			/// </summary>
			private System.Windows.Forms.Timer autoCloseTimer;

			/// <summary>
			/// Time to wait for after mouse leaves
			/// the window or the label before closing.
			/// </summary>
			private int mouseLeaveDelay;

			/// <summary>
			/// Timer used for mouse leave delay
			/// </summary>
			private System.Windows.Forms.Timer mouseLeaveTimer;

			/// <summary>
			/// Text we are displaying
			/// </summary>
			private string tipText;

			/// <summary>
			/// Rectangle used to draw border
			/// </summary>
			private Rectangle outlineRect;
			
			/// <summary>
			/// Rectangle used to display text
			/// </summary>
			private Rectangle textRect;

			#endregion

			#region Construction and Initialization

			public TipWindow( ExpandingLabel label )
			{
				this.label = label;
				this.Owner = label.FindForm();

				this.ControlBox = false;
				this.MaximizeBox = false;
				this.MinimizeBox = false;
				this.BackColor = Color.LightYellow;
				this.FormBorderStyle = FormBorderStyle.None;
				this.StartPosition = FormStartPosition.Manual; 			
			}

			protected override void OnLoad(System.EventArgs e)
			{
				// At this point, further changes to the properties
				// of the label will have no effect on the tip.
				this.expansion = label.Expansion;
				this.overlay = label.Overlay;
				this.autoCloseDelay = label.AutoCloseDelay;
				this.mouseLeaveDelay = label.MouseLeaveDelay;
				this.tipText = label.Text;
				this.Font = label.Font;

				Point origin = label.Parent.PointToScreen( label.Location );
				if ( !label.Overlay )
					origin.Offset( 0, label.Height );
				this.Location = origin;

				Graphics g = Graphics.FromHwnd( Handle );

				// This works if expanding in both directons
				Size sizeNeeded;
				if ( expansion == ExpansionStyle.Vertical )
					sizeNeeded = Size.Ceiling( g.MeasureString( tipText, Font, label.Width ) );
				else
					sizeNeeded = Size.Ceiling( g.MeasureString( tipText, Font ) );
				
				if ( expansion == ExpansionStyle.Horizontal )
					sizeNeeded.Height = label.Height;

				this.ClientSize = sizeNeeded + new Size( 2, 2 );
				this.outlineRect = new Rectangle( 0, 0, sizeNeeded.Width + 1, sizeNeeded.Height + 1 );
				this.textRect = new Rectangle( 1, 1, sizeNeeded.Width, sizeNeeded.Height );

				// Catch mouse entering and leaving the label
				label.MouseEnter += new EventHandler( label_MouseEnter );
				label.MouseLeave += new EventHandler( label_MouseLeave );

				// Catch the form that holds the label closing
				label.FindForm().Closed += new EventHandler( label_FormClosed );

				// Make sure we'll fit on the screen
				Screen screen = Screen.FromControl( label );
				if ( this.Right > screen.WorkingArea.Right )
					this.Left = screen.WorkingArea.Right - this.Width;

				if ( this.Bottom > screen.WorkingArea.Bottom )
				{
					if ( overlay )
						this.Top = screen.WorkingArea.Bottom - this.Height;
					else if ( label.Top > this.Height )
						this.Top = origin.Y - label.Height - this.Height;
				}

				if ( autoCloseDelay > 0 )
				{
					autoCloseTimer = new System.Windows.Forms.Timer();
					autoCloseTimer.Interval = autoCloseDelay;
					autoCloseTimer.Tick += new EventHandler( OnAutoClose );
					autoCloseTimer.Start();
				}

//				if ( mouseLeaveDelay > 0 )
//				{
//					mouseLeaveTimer = new System.Windows.Forms.Timer();
//					mouseLeaveTimer.Interval = mouseLeaveDelay;
//					mouseLeaveTimer.Tick += new EventHandler( OnAutoClose );
//					// Don't start this one yet
//				}
			}

			#endregion

			#region Event Handlers

			protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
			{
				base.OnPaint( e );
				
				Graphics g = e.Graphics;
				g.DrawRectangle( Pens.Black, outlineRect );
				g.DrawString( tipText, Font, Brushes.Black, textRect );
			}

			private void OnAutoClose( object sender, System.EventArgs e )
			{
				this.Close();
			}

			protected override void OnMouseEnter(System.EventArgs e)
			{
				if ( mouseLeaveTimer != null )
				{
					mouseLeaveTimer.Stop();
					mouseLeaveTimer.Dispose();
				}
			}

			protected override void OnMouseLeave(System.EventArgs e)
			{
				if ( mouseLeaveDelay > 0 )
				{
					mouseLeaveTimer = new System.Windows.Forms.Timer();
					mouseLeaveTimer.Interval = mouseLeaveDelay;
					mouseLeaveTimer.Tick += new EventHandler( OnAutoClose );
					mouseLeaveTimer.Start();
				}
			}

			/// <summary>
			/// The form our label is on closed, so we should. 
			/// </summary>
			private void label_FormClosed( object sender, System.EventArgs e )
			{
				this.Close();
			}

			private void label_MouseEnter( object sender, System.EventArgs e )
			{
				if ( mouseLeaveTimer != null )
				{
					mouseLeaveTimer.Stop();
					mouseLeaveTimer.Dispose();
				}
			}

			/// <summary>
			/// The mouse left the label. We ignore if we are
			/// overlaying the label but otherwise start a
			/// delay for closing the window
			/// </summary>
			private void label_MouseLeave( object sender, System.EventArgs e )
			{
				if ( mouseLeaveDelay > 0 && !overlay )
				{
					mouseLeaveTimer = new System.Windows.Forms.Timer();
					mouseLeaveTimer.Interval = mouseLeaveDelay;
					mouseLeaveTimer.Tick += new EventHandler( OnAutoClose );
					mouseLeaveTimer.Start();
				}
			}

			#endregion
		}

		#endregion

		#region Event Handlers

		private void tipWindow_Closed( object sender, EventArgs e )
		{
			tipWindow = null;
		}

		protected override void OnMouseHover(System.EventArgs e)
		{
			Graphics g = Graphics.FromHwnd( Handle );
			SizeF sizeNeeded = g.MeasureString( Text, Font );
			bool expansionNeeded = 
				Width < (int)sizeNeeded.Width ||
				Height < (int)sizeNeeded.Height;

			if ( expansionNeeded ) Expand();
		}

	#endregion
	}
}
