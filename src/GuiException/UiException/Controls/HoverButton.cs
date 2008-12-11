// ----------------------------------------------------------------
// ExceptionBrowser
// Version 1.0.0
// Copyright 2008, Irénée HOTTIER,
// 
// This is free software licensed under the NUnit license, You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NUnit.UiException.Properties;

namespace NUnit.UiException.Controls
{
    public enum HoverButtonDirection
    {
        Up,
        Left,
        Down,
        Right
    }

    /// <summary>
    /// Draws a box decorated by a small arrow. This object is graphic only
    /// and exposes no special behavior.
    ///   The arrows are drawn from an image located in the 'Resources'. The
    /// same arrow is presented several times but with small differences so
    /// each image can suit a particular control State.
    /// </summary>
    public partial class HoverButton : UserControl
    {
        /// <summary>
        /// The current direction represented by this control.
        /// </summary>
        private HoverButtonDirection _direction;

        /// <summary>
        /// Stores the information needed to extract from the picture
        /// the part of the image that matches both the direction and
        /// the state of the control (enabled/disabled).
        /// </summary>
        private ButtonGraphicAttributes _graphicAttributes;

        /// <summary>
        /// Builds a new instance of HoverButton.
        /// </summary>
        public HoverButton()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            Direction = HoverButtonDirection.Up;

            EnabledChanged += new EventHandler(HoverButton_EnabledChanged);

            return;
        }           

        /// <summary>
        /// Gets or sets the direction represented by this button.
        /// </summary>
        public HoverButtonDirection Direction
        {
            get { return (_direction); }

            set
            {
                if (_direction == value &&
                    _graphicAttributes != null)
                    return;

                _direction = value;
                _graphicAttributes = ButtonGraphicAttributes.CreateFromDirection(_direction);
                Invalidate();

                return;
            }
        }

        /// <summary>
        /// Invoked when Enabled property has changed.
        /// </summary>
        void HoverButton_EnabledChanged(object sender, EventArgs e)
        {
            _graphicAttributes.IsEnabled = Enabled;
            Invalidate();
        }            

        /// <summary>
        /// Invoked when the control need to be repainted.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Brush brush;
            int x;
            int y;
            
            brush = _graphicAttributes.Brush;

            x = (ClientRectangle.Width - _graphicAttributes.Size.Width) / 2;
            y = (ClientRectangle.Height - _graphicAttributes.Size.Height) / 2;

            e.Graphics.FillRectangle(brush, ClientRectangle);

            e.Graphics.DrawImage(
                Resources.Arrows,
                new Rectangle(x, y, _graphicAttributes.Size.Width, _graphicAttributes.Size.Height),
                _graphicAttributes.CurrentState.X,
                _graphicAttributes.CurrentState.Y, 
                _graphicAttributes.Size.Width,
                _graphicAttributes.Size.Height,
                GraphicsUnit.Pixel);

            brush.Dispose();

            return;
        }

        /// <summary>
        /// Invoked when mouse cursor has entered this control.
        /// </summary>
        private void HoverButton_MouseEnter(object sender, EventArgs e)
        {
            _graphicAttributes.SetHovered(true);
            Invalidate();
        }

        /// <summary>
        /// Invoked when the mouse cursor has left the control.
        /// </summary>
        private void HoverButton_MouseLeave(object sender, EventArgs e)
        {
            _graphicAttributes.SetHovered(false);
            Invalidate();
        }

        #region ButtonGraphicAttributes

        /// <summary>
        /// Holds all the knowledges to extract the image that fit best
        /// current direction and state of this control.
        /// </summary>
        class ButtonGraphicAttributes
        {
            private Size _size;
            private Point _normal;
            private Point _disabled;
            private Point _hoverNormal;
            private Point _hoverDisabled;
            private bool _enabled;
            private bool _hovered;

            public static ButtonGraphicAttributes CreateFromDirection(HoverButtonDirection dir)
            {
                switch (dir)
                {
                    case HoverButtonDirection.Up:
                        return (new ButtonGraphicAttributes(7, 5, 0, 0, 8, 0, 16, 0, 24, 0));

                    case HoverButtonDirection.Left:
                        return (new ButtonGraphicAttributes(5, 7, 0, 12, 6, 12, 0, 20, 6, 20));

                    case HoverButtonDirection.Down:
                        return (new ButtonGraphicAttributes(7, 5, 0, 6, 8, 6, 16, 6, 24, 6));

                    case HoverButtonDirection.Right:
                        return (new ButtonGraphicAttributes(5, 7, 12, 12, 18, 12, 12, 20, 18, 20));

                    default:
                        TraceExceptionHelper.Fail("should not reach this code");
                        break;
                }

                return (null);
            }

            private ButtonGraphicAttributes(int w, int h, int x0, int y0, int x1, int y1, int x2, int y2, int x3, int y3)
            {
                _size = new Size(w, h);
                _normal = new Point(x0, y0);
                _disabled = new Point(x1, y1);
                _hoverNormal = new Point(x2, y2);
                _hoverDisabled = new Point(x3, y3);

                _enabled = true;
                _hovered = false;

                return;
            }

            public bool IsEnabled
            {
                get { return (_enabled); }
                set { _enabled = value; }
            }

            public void SetHovered(bool hovered) {
                _hovered = hovered;
            }

            public Size Size {
                get { return (_size); }
            }

            public Point Normal {
                get { return (_normal); }
            }

            public Point Disabled {
                get { return (_disabled); }
            }

            public Point HoverNormal {
                get { return (_hoverNormal); }
            }

            public Point HoverDisabled {
                get { return (_hoverDisabled); }
            }

            public Point CurrentState
            {
                get {
                    if (_enabled)
                        return ((_hovered == true) ? HoverNormal : Normal);
                    return ((_hovered == true) ? HoverDisabled : Disabled);
                }
            }

            public Brush Brush
            {
                get
                {
                    if (_hovered)
                        return (new SolidBrush(Color.FromArgb(232, 237, 255)));
                    return (new SolidBrush(Color.White));
                }
            }
        }
        
        #endregion       
    }
}
