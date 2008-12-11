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
using NUnit.Framework;
using NUnit.UiException;
using System.Windows.Forms;

namespace NUnit.UiException.Tests
{
    [TestFixture]
    public class TestMouseWatcher
    {
        private TestingMouseHoverManager _empty;
        private TestingMouseHoverManager _filled;
        private TestingControl _controlA;
        private TestingControl _controlB;
        private int _notification;

        [SetUp]
        public void SetUp()
        {
            _controlA = new TestingControl();
            _controlB = new TestingControl();

            _empty = new TestingMouseHoverManager();

            _filled = new TestingMouseHoverManager();
            _filled.Register(_controlA);
            _filled.Register(_controlB);

            _filled.MouseLeaved += new EventHandler(_filled_MouseLeaved);
            _notification = 0;

            return;
        }

        void _filled_MouseLeaved(object sender, EventArgs e)
        {
            _notification++;
        }

        [Test]
        public void Test_Default()
        {
            Assert.That(_empty.Count, Is.EqualTo(0));
            Assert.That(_empty.Active, Is.Null);
            Assert.That(_empty.Timer.Enabled, Is.False);

            return;
        }

        [Test]
        public void Test_Register()
        {
            _empty.Register(_controlA);
            Assert.That(_empty.Count, Is.EqualTo(1));
        }

        [Test]
        public void Test_Register_Is_Recursive()
        {
            TestingControl panelA;
            TestingControl panelB;
            TestingMouseHoverManager mgr;

            mgr = new TestingMouseHoverManager();

            panelA = new TestingControl();
            panelB = new TestingControl();
            panelA.Controls.Add(panelB);

            mgr.Register(panelA);

            panelB.FireMouseEnter();

            Assert.That(mgr.Active, Is.EqualTo(panelB));

            return;
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException),
            ExpectedMessage = "control",
            MatchType = MessageMatch.Contains)]
        public void Test_Register_Throws_NullControlException()
        {
            _empty.Register(null); // throws exception
        }

        [Test]
        public void Test_HandleMouse_Enter_And_Leave()
        {
            _controlA.FireMouseEnter();
            Assert.That(_filled.Active, Is.EqualTo(_controlA));
            Assert.That(_filled.Timer.Enabled, Is.False);

            _controlA.FireMouseLeave();
            Assert.That(_filled.Active, Is.Null);
            Assert.That(_filled.Timer.Enabled, Is.True);

            ///

            _controlA.FireMouseEnter();
            _controlB.FireMouseLeave();
            Assert.That(_filled.Active, Is.EqualTo(_controlA));

            return;
        }

        [Test]
        public void Test_HandleMouse_Enter_2()
        {
            _controlA.FireMouseEnter();
            _controlB.FireMouseEnter();
            _controlA.FireMouseLeave();

            Assert.That(_filled.Active, Is.EqualTo(_controlB));

            return;
        }

        [Test]
        public void Test_TimerTick_FireMouseLeaveEvent()
        {
            _controlA.FireMouseEnter();
            _controlA.FireMouseLeave();

            Assert.That(_filled.Timer.Enabled, Is.True);
            _filled.HandleTimerTick();
            Assert.That(_notification, Is.EqualTo(1));
            Assert.That(_filled.Timer.Enabled, Is.False);

            _notification = 0;
            _controlA.FireMouseEnter();
            _controlA.FireMouseLeave();
            _controlB.FireMouseEnter();
            _filled.HandleTimerTick();
            Assert.That(_notification, Is.EqualTo(0));
            Assert.That(_filled.Timer.Enabled, Is.False);

            return;
        }

        #region TestingControl

        class TestingControl :
            Control
        {
            public void FireMouseEnter()
            {
                OnMouseEnter(new EventArgs());
            }

            public void FireMouseLeave()
            {
                OnMouseLeave(new EventArgs());
            }
        }

        #endregion

        #region TestingMouseHoverManager

        class TestingMouseHoverManager :
            MouseWatcher
        {
            public new Timer Timer {
                get { return (base.Timer); }
            }

            public new void HandleTimerTick()
            {
                base.HandleTimerTick();
            }
        }

        #endregion
    }
}
