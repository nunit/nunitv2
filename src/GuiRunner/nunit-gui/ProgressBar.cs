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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace NUnit.Gui
{
	/// <summary>
	/// Summary description for ProgressBar.
	/// </summary>
	public class ProgressBar : System.Windows.Forms.Control
	{
		#region Instance Variables
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		
		private int fValue = 0;
		private int fmin = 0;
		private int fmax = 100;
		private int fStep = 10;
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

			// TODO: Add any initialization after the InitForm call
			//SetStyle(ControlStyles.Opaque, true);
			SetStyle(ControlStyles.ResizeRedraw, true);
			//SetStyle(ControlStyles.ResizeRedraw | ControlStyles.Opaque, true);
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
			get{return ((float)this.fValue / ((float)Maximum - (float)Minimum));}
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

		public void PerformStep()
		{
			if(Value < Maximum)
			{
				this.Value+=Step;
			}
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
	}
}