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
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace NUnit.UiException
{
    /// <summary>
    /// Watch events coming from the mouse over a given control hierarchy and
    /// proprage up events such as: MouseLeaved or MouseClicked to observers.
    ///     When a MouseLeave event is fired, the class waits a certain amount of time
    /// to check whether or not a mouseEnter event is coming. If nothing happens
    /// after this time, the class delivers a mouseLeaved event to registered observers.
    ///     Unlike MouseLeave, the MouseClicked event is fired when it occurs on
    /// one of the underlying controls.
    /// </summary>
    public class MouseWatcher
    {
        /// <summary>
        /// Default setting that indicate the amount of time to wait (in milliseconds)
        /// after a mouseLeave event occured.
        /// </summary>
        public const int DEFAULT_TIMER_INTERVAL = 500;

        /// <summary>
        /// Fired when MouseWatcher has detected that the mouse completely
        /// get out of the watched control collection.
        /// </summary>
        public event EventHandler MouseLeaved;

        /// <summary>
        /// Fired when MouseWatcher has detected a click on one of underlying
        /// control hierarchy.
        /// </summary>
        public event EventHandler MouseClicked;

        /// <summary>
        /// The list of controls to be watched.
        /// </summary>
        private List<Control> _controls;

        /// <summary>
        /// The current active control.
        /// </summary>
        private Control _active;

        /// <summary>
        /// The timer that waits amount of time explained above.
        /// </summary>
        private Timer _timer;

        /// <summary>
        /// Builds a new instance of MouseWatcher.
        /// </summary>
        public MouseWatcher()
        {
            _controls = new List<Control>();
            _timer = new Timer();
            _timer.Tick += new EventHandler(_timer_Tick);
            _timer.Interval = DEFAULT_TIMER_INTERVAL;

            return;
        }       

        /// <summary>
        /// Registers this control in the collection of controls for which 
        /// mouse interactions must be watched.
        /// </summary>
        /// <param name="control">Appends this control to the list of control for which
        /// mouse interactions must be watched.</param>
        public void Register(Control control)
        {
            TraceExceptionHelper.CheckNotNull(control, "control");
            _controls.Add(control);

            control.MouseEnter += new EventHandler(control_MouseEnter);
            control.MouseLeave += new EventHandler(control_MouseLeave);
            control.MouseClick += new MouseEventHandler(control_MouseClick);

            foreach (Control child in control.Controls)
                Register(child);

            return;
        }
        
        /// <summary>
        /// Gets the current active control.
        /// </summary>
        public Control Active
        {
            get { return (_active); }
        }

        /// <summary>
        /// Gets the count of controls in the list.
        /// </summary>
        public int Count
        {
            get { return (_controls.Count); }
        }

        /// <summary>
        /// Gives access to the timer.
        /// </summary>
        protected Timer Timer
        {
            get { return (_timer); }
        }

        /// <summary>
        /// Handler called when a tick occurs.
        /// </summary>
        protected void HandleTimerTick()
        {
            //Trace.WriteLine("timer: active=" + _active);

            if (_active != null)
            {
                _timer.Enabled = false;
                return;
            }

            //Trace.WriteLine("====> LEAVE");

            _timer.Enabled = false;

            if (MouseLeaved != null)
                MouseLeaved(this, new EventArgs());

            return;
        }

        /// <summary>
        /// Timer event handler.
        /// </summary>
        void _timer_Tick(object sender, EventArgs e)
        {
            HandleTimerTick();
        }

        /// <summary>
        /// Handler called when mouse cursor leaved a control.
        /// </summary>
        void control_MouseLeave(object sender, EventArgs e)
        {
            // Trace.WriteLine(sender + " said: mouse leave");

            if (_active != sender)
                return;

            _active = null;
            _timer.Enabled = true;

            return;
        }

        /// <summary>
        /// Handler called when a click has occured on
        /// control hierarchy.
        /// </summary>
        void control_MouseClick(object sender, MouseEventArgs e)
        {
            if (MouseClicked != null)
                MouseClicked(this, e);

            return;
        }

        /// <summary>
        /// Handler called when mouse cursor entered a control.
        /// </summary>
        void control_MouseEnter(object sender, EventArgs e)
        {
            //Trace.WriteLine(sender + " said mouse enter");
            _active = (Control)sender;
        }        
    }
}
