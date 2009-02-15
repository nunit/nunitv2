// ----------------------------------------------------------------
// ErrorBrowser
// Copyright 2008-2009, Irénée HOTTIER,
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
using System.Diagnostics;
using NUnit.UiException.CodeFormatters;

namespace NUnit.UiException.Controls
{
    /// <summary>
    /// Displays a CodeBox control surrounded by four buttons. Passing the mouse
    /// cursor hover one of the buttons translates the view in the direction
    /// indicated by the button.
    /// </summary>
    public partial class CodeBoxComposite :
        UserControl
    {
        /// <summary>
        /// Default distance by which moving text.
        /// </summary>
        private const double DEFAULT_SCROLLING_DISTANCE = 5;

        /// <summary>
        /// Time interval (in milliseconds) for scroll movement.
        /// </summary>
        private const int DEFAULT_SCROLL_TICK = 10;

        /// <summary>
        /// When scrolling to the line where the exception occured, 
        /// this constant is used to slightly alter the line startingPosition.
        /// So user can see line of codes before/after the exact line
        /// where the exception occured.
        /// </summary>
        public const int LINECOUNT_BEFORE_EXCEPTIONLINE = 8;

        /// <summary>
        /// Notify client code when mouse cursor leaves current control.
        /// </summary>
        public event EventHandler MouseLeaveWindow;

        /// <summary>
        /// Notify client code when a click has occured on this control hierarchy.
        /// </summary>
        public event EventHandler MouseClickedWindow;

        /// <summary>
        /// small value to address common issues when handling
        /// comparison with double precision values.
        /// </summary>
        public const double EPSILON = 10e-3d;

        /// <summary>
        /// Represents the current translation direction as an enum.
        /// </summary>
        public enum TranslationDirection
        {
            /// <summary>
            /// There is not translation at all.
            /// </summary>
            None,

            /// <summary>
            /// Left translation.
            /// </summary>
            Left,

            /// <summary>
            /// Up translation.
            /// </summary>
            Up,

            /// <summary>
            /// Right translation.
            /// </summary>
            Right,

            /// <summary>
            /// Down translation.
            /// </summary>
            Down
        }

        private GeneralCodeFormatter _formatter;

        /// <summary>
        /// Tracks time ticks to provide smooth scroll animation over time.
        /// </summary>
        private Timer _timerScroll;

        /// <summary>
        /// holds data to be displayed in the control.
        /// </summary>
        private ErrorItem _item;

        /// <summary>
        /// The current translation's direction.
        /// </summary>
        private TranslationDirection _translation;

        /// <summary>
        /// The value by which moving texts at each time tick.
        /// </summary>
        private double _scrollingDistance;

        /// <summary>
        /// The message of the error (if any) to be reported to the user.
        /// </summary>
        private string _errorMessage;

        /// <summary>
        /// Keep tracks whether or not the mouse cursor is on this control
        /// (and its children), in order to emit 'MouseLeaveControl' event.
        /// </summary>
        private MouseWatcher _mouseWatcher;

        /// <summary>
        /// Builds a new instance of ScrollableCodeBox.
        /// </summary>
        public CodeBoxComposite()
        {
            InitializeComponent();
            
            _translation = TranslationDirection.None;

            _timerScroll = new Timer();
            _timerScroll.Enabled = false;
            _timerScroll.Interval = DEFAULT_SCROLL_TICK;
            _timerScroll.Tick += new EventHandler(_timer_Tick);

            _btnUp.MouseEnter += new EventHandler(btnUp_MouseEnter);
            _btnLeft.MouseEnter += new EventHandler(btnLeft_MouseEnter);
            _btnDown.MouseEnter += new EventHandler(btnDown_MouseEnter);
            _btnRight.MouseEnter += new EventHandler(btnRight_MouseEnter);
            _codeBox.MouseEnter += new EventHandler(_codeBox_MouseEnter);           

            _codeBox.Viewport.LocationChanged += new EventHandler(Viewport_LocationChanged);
            _codeBox.TextChanged += new EventHandler(_codeBox_TextChanged);

            _scrollingDistance = DEFAULT_SCROLLING_DISTANCE;

            _mouseWatcher = new MouseWatcher();
            _mouseWatcher.Register(this);

            _mouseWatcher.MouseLeaved += new EventHandler(_mouseWatcher_MouseLeaved);
            _mouseWatcher.MouseClicked += new EventHandler(_mouseWatcher_MouseClicked);

            _formatter = new GeneralCodeFormatter();

            return;
        }

        void _mouseWatcher_MouseClicked(object sender, EventArgs e)
        {
            if (MouseClickedWindow != null)
                MouseClickedWindow(this, e);

            return;
        }       

        /// <summary>
        /// Gets or sets the distance in pixels by which moving the text.
        /// </summary>
        public double ScrollingDistance
        {
            get { return (_scrollingDistance); }
            set { _scrollingDistance = value; }
        }

        /// <summary>
        /// Gets or sets the instance of ErrorItem to display in
        /// this control.
        /// </summary>
        public ErrorItem ErrorSource
        {
            get { return (_item); }
            set
            {
                _item = value;
                _codeBox.Text = "";

                if (_item == null)
                    return;

                try
                {
                    _codeBox.SetFormattedCode(_formatter.FormatFromExtension(value.Text, value.FileExtension));
                    _codeBox.HighlightedLine = _item.LineNumber;
                    _codeBox.Viewport.ScrollToLine(_item.LineNumber - 1 - LINECOUNT_BEFORE_EXCEPTIONLINE);                    
                }
                catch (Exception e)
                {
                    ErrorMessage = String.Format(
                        "Fail to open file: '{0}'\r\n, the following error was reported: {1}.\r\n",
                        _item.Path, e.Message);
                    _codeBox.Text = ErrorMessage;
                }

                return;
            }
        }

        /// <summary>
        /// Gives access to the underlying CodeBox instance.
        /// </summary>
        public CodeBox CodeBox
        {
            get { return (_codeBox); }
        }

        #region internal definitions

        /// <summary>
        /// Gets the current translation's direction.
        /// </summary>
        protected TranslationDirection Translation
        {
            get { return (_translation); }
        }

        /// <summary>
        /// Gives access to the underlying timer.
        /// </summary>
        protected Timer Timer
        {
            get { return (_timerScroll); }
        }

        /// <summary>
        /// Gives access to the underlying left button.
        /// </summary>
        protected HoverButton BtnLeft
        {
            get { return (_btnLeft); }
        }

        /// <summary>
        /// Gives access to the underlying up button.
        /// </summary>
        protected HoverButton BtnUp
        {
            get { return (_btnUp); }
        }

        /// <summary>
        /// Gives access to the underlying right button.
        /// </summary>
        protected HoverButton BtnRight
        {
            get { return (_btnRight); }
        }

        /// <summary>
        /// Gives access to the underlying down button.
        /// </summary>
        protected HoverButton BtnDown
        {
            get { return (_btnDown); }
        }

        /// <summary>
        /// Gets or sets the error message to be displayed to the user.
        /// </summary>
        protected string ErrorMessage
        {
            get { return (_errorMessage); }
            set { _errorMessage = value; }
        }

        #endregion

        #region Mouse Hover

        /// <summary>
        /// Handler when mouse hovers up button.
        /// </summary>
        protected void HandleMouseHoverUp()
        {
            _translation = TranslationDirection.Up;
            _timerScroll.Enabled = true;
        }

        /// <summary>
        /// Handler when mouse hovers down button.
        /// </summary>
        protected void HandleMouseHoverDown()
        {
            _translation = TranslationDirection.Down;
            _timerScroll.Enabled = true;
        }

        /// <summary>
        /// Handler when mouse hovers left button.
        /// </summary>
        protected void HandleMouseHoverLeft()
        {
            _translation = TranslationDirection.Left;
            _timerScroll.Enabled = true;
        }

        /// <summary>
        /// Handler when mouse hovers right button.
        /// </summary>
        protected void HandleMouseHoverRight()
        {
            _translation = TranslationDirection.Right;
            _timerScroll.Enabled = true;
        }

        /// <summary>
        /// Handler when mouse hovers CodeBox.
        /// </summary>
        protected void HandleMouseHoverCode()
        {
            ActiveControl = _codeBox;
            _translation = TranslationDirection.None;
            _timerScroll.Enabled = false;
        }

        /// <summary>
        /// Timer handler.
        /// </summary>
        protected void HandleTimerTick()
        {
            if (_translation == TranslationDirection.Up)
            {
                _codeBox.TranslateView(0, -ScrollingDistance);
                if (_codeBox.Viewport.RemainingTop <= EPSILON)
                    _timerScroll.Enabled = false;

                return;
            }

            if (_translation == TranslationDirection.Left)
            {
                _codeBox.TranslateView(-ScrollingDistance, 0);
                if (_codeBox.Viewport.RemainingLeft <= EPSILON)
                    _timerScroll.Enabled = false;

                return;
            }

            if (_translation == TranslationDirection.Down)
            {
                _codeBox.TranslateView(0, ScrollingDistance);
                if (_codeBox.Viewport.RemainingBottom <= EPSILON)
                    _timerScroll.Enabled = false;
            }

            if (_translation == TranslationDirection.Right)
            {
                _codeBox.TranslateView(ScrollingDistance, 0);
                if (_codeBox.Viewport.RemainingRight <= EPSILON)
                    _timerScroll.Enabled = false;
            }

            return;
        }

        #endregion        

        /// <summary>
        /// Invoked when text has changed in CodeBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _codeBox_TextChanged(object sender, EventArgs e)
        {
            UpdateHoverButtonsState();
        }

        /// <summary>
        /// Invoked when HoverMgr has detected that mouse cursor
        /// has leaved control and children.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _mouseWatcher_MouseLeaved(object sender, EventArgs e)
        {
            HandleLeaveEvent();
        }

        /// <summary>
        /// Handler when mouse leave this window.
        /// </summary>
        protected void HandleLeaveEvent()
        {
            _translation = TranslationDirection.None;
            _timerScroll.Enabled = false;

            if (MouseLeaveWindow != null)
                MouseLeaveWindow(this, new EventArgs());

            return;
        }

        /// <summary>
        /// Update HoverButton's enable property according the current
        /// smState of viewport.
        /// </summary>
        void UpdateHoverButtonsState()
        {
            _btnUp.Enabled = (_codeBox.Viewport.RemainingTop > EPSILON);
            _btnLeft.Enabled = (_codeBox.Viewport.RemainingLeft > EPSILON);
            _btnRight.Enabled = (_codeBox.Viewport.RemainingRight > EPSILON);
            _btnDown.Enabled = (_codeBox.Viewport.RemainingBottom > EPSILON);

            return;
        }

        /// <summary>
        /// Handler when viewport's location changes.
        /// </summary>
        void Viewport_LocationChanged(object sender, EventArgs e)
        {
            UpdateHoverButtonsState();
        }

        /// <summary>
        /// Invoked when mouse enters BtnUp.
        /// </summary>
        private void btnUp_MouseEnter(object sender, EventArgs e)
        {
            HandleMouseHoverUp();
        }

        /// <summary>
        /// Invoked when mouse enters BtnDown.
        /// </summary>
        private void btnDown_MouseEnter(object sender, EventArgs e)
        {
            HandleMouseHoverDown();
        }            

        /// <summary>
        /// Invoked when mouse enters BtnRight.
        /// </summary>
        private void btnRight_MouseEnter(object sender, EventArgs e)
        {
            HandleMouseHoverRight();
        }

        /// <summary>
        /// Invoked when mouse enter BtnLeft.
        /// </summary>
        private void btnLeft_MouseEnter(object sender, EventArgs e)
        {
            HandleMouseHoverLeft();
        }

        /// <summary>
        /// Invoked when mouse enter CodeBox.
        /// </summary>
        private void _codeBox_MouseEnter(object sender, EventArgs e)
        {
            HandleMouseHoverCode();
        }

        /// <summary>
        /// Invoked when tick occured.
        /// </summary>
        void _timer_Tick(object sender, EventArgs e)
        {
            HandleTimerTick();
        }
    }
}
