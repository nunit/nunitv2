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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using NUnit.Core;
using NUnit.Util;

namespace NUnit.UiKit
{
	/// <summary>
	/// Summary description for ProgressBar.
	/// </summary>
	public class ProgressBar : System.Windows.Forms.Control, TestObserver
	{
		#region Instance Variables
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private int fValue = 0;
		private int fmin = 0;
		private int fmax = 100;
		private int fStep = 1;
		private Color fBarColor = SystemColors.ControlText;
		
		private float FMaxSegmentCount=0;
		private int fSegmentWidth=0;
		private int fLastSegmentCount=0;

		private Brush BarBrush = null;
		private Brush NotBarBrush = null;

		#endregion

		#region Constructors & Disposer
		public ProgressBar()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			SetStyle(ControlStyles.ResizeRedraw, true);
			Initialize( 100 );
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
				this.ReleaseDrawers();
			}
			base.Dispose( disposing );
		}
		#endregion

		#region Properties
		
		[Category("Behavior")]
		public int Minimum
		{
			get { return this.fmin; }
			set
			{
				if (value <= Maximum) 
				{
					if (this.fmin != value) 
					{
						this.fmin = value;
						this.PaintBar();
					}
				}
				else
				{
					throw new ArgumentOutOfRangeException("Minimum", value
						,"Minimum must be <= Maximum.");
				}
			}
		}

		[Category("Behavior")]
		public int Maximum 
		{
			get	{ return this.fmax; }
			set
			{
				if (value >= Minimum) 
				{
					if (this.fmax != value) 
					{
						this.fmax = value;
						this.PaintBar();
					}
				}
				else
				{
					throw new ArgumentOutOfRangeException("Maximum", value
						,"Maximum must be >= Minimum.");
				}
			}
		}

		[Category("Behavior")]
		public int Step
		{
			get	{ return this.fStep; }
			set
			{
				if (value <= Maximum && value >= Minimum) 
				{
					this.fStep = value;
				}
				else
				{
					throw new ArgumentOutOfRangeException("Step", value
						,"Must fall between Minimum and Maximum inclusive.");
				}
			}
		}
		
		[Browsable(false)]
		private float PercentValue
		{
			get
            {
                if (0 != Maximum - Minimum) // NRG 05/28/03: Prevent divide by zero
                    return((float)this.fValue / ((float)Maximum - (float)Minimum));
                else
                    return(0);
            }
		}	

		[Category("Behavior")]
		public int Value 
		{
			get { return this.fValue; }
			set 
			{
				if(value == this.fValue)
					return;
				else if(value <= Maximum && value >= Minimum)
				{
					this.fValue = value;
					this.PaintBar();
				}
				else
				{
					throw new ArgumentOutOfRangeException("Value", value
						,"Must fall between Minimum and Maximum inclusive.");
				}
			}
		}

		#endregion

		#region Methods

		private void Initialize( int testCount )
		{
			ForeColor = Color.Lime;
			Value = 0;
			Maximum = testCount;
		}

		protected override void OnCreateControl()
		{
		}

		public void PerformStep()
		{
			int newValue = Value + Step;

			if( newValue > Maximum )
				newValue = Maximum;

			Value = newValue;
		}

		private void OnRunStarting( object Sender, TestEventArgs e )
		{
			Initialize( e.TestCount );
		}

		private void OnLoadComplete( object sender, TestEventArgs e )
		{
			Initialize( e.TestCount );
		}

		private void OnUnloadComplete( object sender, TestEventArgs e )
		{
			Initialize( 100 );
		}

		private void OnTestFinished( object sender, TestEventArgs e )
		{
			PerformStep();

            switch (e.Result.RunState)
            {
                case RunState.Executed:
                    if (e.Result.IsFailure)
                        ForeColor = Color.Red;
                    break;
                case RunState.Ignored:
                    if (ForeColor == Color.Lime)
                        ForeColor = Color.Yellow;
                    break;
                default:
                    break;
            }
		}

        private void OnTestException(object senderk, TestEventArgs e)
        {
            ForeColor = Color.Red;
        }

		protected override void OnResize(System.EventArgs e)
		{
			base.OnResize(e);
			this.fSegmentWidth = (int)((float)ClientRectangle.Height*.66f);
			this.FMaxSegmentCount = ((float)(ClientRectangle.Width - 5))
				/((float)fSegmentWidth);
		}

		protected override void OnBackColorChanged(System.EventArgs e)
		{
			base.OnBackColorChanged(e);
			this.Refresh();
		}
		protected override void OnForeColorChanged(System.EventArgs e)
		{
			base.OnForeColorChanged(e);
			this.Refresh();
		}
		
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			this.fLastSegmentCount=0;
			this.ReleaseDrawers();
			PaintBar(e.Graphics);
			ControlPaint.DrawBorder3D(
				e.Graphics
				,this.ClientRectangle
				,Border3DStyle.SunkenOuter);
			//e.Graphics.Flush();
		}

		private void ReleaseDrawers()
		{
			if(BarBrush != null)
			{
				BarBrush.Dispose();
				NotBarBrush.Dispose();
				BarBrush=null;
				NotBarBrush=null;
			}
		}

		private void AcquireDrawers()
		{
			if(BarBrush == null)
			{
				BarBrush = new SolidBrush(this.ForeColor);
				NotBarBrush = new SolidBrush(this.BackColor);
			}
		}

		private void PaintBar()
		{
			using(Graphics g = this.CreateGraphics())
			{
				this.PaintBar(g);
			}
		}
		
		private void PaintBar(Graphics g)
		{
			Rectangle Bar = Rectangle.Inflate(ClientRectangle, -2, -2);
			int maxRight = Bar.Right-1;
			//int maxRight = Bar.Right;
			int newSegmentCount = (int)System.Math.Ceiling(PercentValue*FMaxSegmentCount);
			this.AcquireDrawers();
			if(newSegmentCount > fLastSegmentCount)
			{
				Bar.X += fLastSegmentCount*fSegmentWidth;
				while (fLastSegmentCount < newSegmentCount )
				{
					Bar.Width = System.Math.Min(maxRight-Bar.X,fSegmentWidth-2);
					g.FillRectangle(BarBrush, Bar);
					Bar.X+=fSegmentWidth;
					fLastSegmentCount++;
				}
			}
			else if(newSegmentCount < fLastSegmentCount)
			{
				Bar.X += newSegmentCount*fSegmentWidth;
				Bar.Width = maxRight-Bar.X;
				g.FillRectangle(NotBarBrush, Bar);
				fLastSegmentCount = newSegmentCount;
			}
			if(Value == Minimum || Value == Maximum)
				this.ReleaseDrawers();
		}

		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// ProgressBar
			// 
			this.CausesValidation = false;
			this.Enabled = false;
			this.ForeColor = System.Drawing.SystemColors.Highlight;
			this.Name = "ProgressBar";
			this.Size = new System.Drawing.Size(432, 24);
		}
		#endregion

		#region TestObserver Members

		public void Subscribe(ITestEvents events)
		{
			events.TestLoaded	+= new TestEventHandler( OnLoadComplete );
			events.TestReloaded	+= new TestEventHandler( OnLoadComplete );
			events.TestUnloaded	+= new TestEventHandler( OnUnloadComplete );
			events.RunStarting	+= new TestEventHandler( OnRunStarting );
			events.TestFinished	+= new TestEventHandler( OnTestFinished );
			events.TestException += new TestEventHandler(OnTestException);
		}

		#endregion
	}
}