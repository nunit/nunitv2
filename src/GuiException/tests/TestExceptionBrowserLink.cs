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
using System.Drawing;
using System.Windows.Forms;
using NUnit.UiException.Controls;

namespace NUnit.UiException.Tests
{
    [TestFixture]
    public class TestExceptionBrowserLink
    {
        private ExceptionBrowserLink _browser;

        [SetUp]
        public void SetUp()
        {
            _browser = new ExceptionBrowserLink();

            return;
        }

        [Test]
        public void Test_Default()
        {
            Assert.That(_browser.ExceptionOrder, Is.EqualTo(ExceptionOrder.Reverse));
            Assert.That(_browser.FormStack, Is.Not.Null);
            Assert.That(_browser.FormCode, Is.Not.Null);
            Assert.That(_browser.FormStack.Visible, Is.False);
            Assert.That(_browser.FormCode.Visible, Is.False);

            Assert.That(_browser.FileExtension, Is.EqualTo(".cs"));
            Assert.That(_browser.DirectorySeparator, Is.EqualTo('\\'));

            Assert.That(_browser.Items, Is.Not.Null);
            Assert.That(_browser.Items.Count, Is.EqualTo(0));
            Assert.That(_browser.Pagineer, Is.Not.Null);
            Assert.That(_browser.Pagineer.PageSize, Is.EqualTo(ExceptionList.DEFAULT_PAGE_SIZE));

            Assert.That(_browser.Caption, Is.Not.Null);
            Assert.That(_browser.Caption, Is.EqualTo("Browse exception details here"));
            Assert.That(_browser.ForeColor, Is.EqualTo(Color.Blue));
            Assert.That(_browser.Font.Underline, Is.True);

            return;
        }

        [Test]
        public void Test_Set_Properties()
        {
            _browser.FileExtension = ".csharp";
            _browser.DirectorySeparator = '/';

            Assert.That(_browser.FileExtension, Is.EqualTo(".csharp"));
            Assert.That(_browser.DirectorySeparator, Is.EqualTo('/'));

            return;
        }

        [Test]
        public void Test_StackTrace()
        {
            Assert.That(_browser.StackTrace, Is.Null);
            Assert.That(_browser.Items.Count, Is.EqualTo(0));

            _browser.StackTrace = "à test() C:\\file.cs:ligne 1";
            Assert.That(_browser.StackTrace, Is.EqualTo("à test() C:\\file.cs:ligne 1"));
            Assert.That(_browser.Items.Count, Is.EqualTo(1));
            Assert.That(_browser.Items[0],
                Is.EqualTo(
                new ExceptionItem("C:\\file.cs", "test()", 1)));

            //
            // StackTrace property automatically clears previous entries
            //

            _browser.FileExtension = ".csharp";
            _browser.DirectorySeparator = '/';
            _browser.StackTrace = "à test() /home/ihottier/file2.csharp:ligne 1";
            Assert.That(_browser.Items.Count, Is.EqualTo(1));
            Assert.That(_browser.Items[0],
                Is.EqualTo(
                new ExceptionItem("/home/ihottier/file2.csharp", "test()", 1)));

            return;
        }

        [Test]
        public void Test_ExceptionOrder()
        {
            _browser.StackTrace = 
                "à func_1() C:\\file_1.cs:ligne 1\r\n" +
                "à func_2() C:\\file_2.cs:ligne 2\r\n" +
                "à func_3() C:\\file_3.cs:ligne 3\r\n";

            // check the order in which the control lists these items.

            Assert.That(_browser.ExceptionOrder, Is.EqualTo(ExceptionOrder.Reverse));

            Assert.That(_browser.Items[0].LineNumber, Is.EqualTo(3));
            Assert.That(_browser.Items[1].LineNumber, Is.EqualTo(2));
            Assert.That(_browser.Items[2].LineNumber, Is.EqualTo(1));

            // reverse the order

            _browser.ExceptionOrder = ExceptionOrder.Normal;
            Assert.That(_browser.ExceptionOrder, Is.EqualTo(ExceptionOrder.Normal));

            _browser.StackTrace =
                "à func_1() C:\\file_1.cs:ligne 1\r\n" +
                "à func_2() C:\\file_2.cs:ligne 2\r\n" +
                "à func_3() C:\\file_3.cs:ligne 3\r\n";

            Assert.That(_browser.Items[0].LineNumber, Is.EqualTo(1));
            Assert.That(_browser.Items[1].LineNumber, Is.EqualTo(2));
            Assert.That(_browser.Items[2].LineNumber, Is.EqualTo(3));

            // change ExceptionOrder, without re-assigning text.
            // this should work as well.

            _browser.ExceptionOrder = ExceptionOrder.Reverse;
            Assert.That(_browser.Items[0].LineNumber, Is.EqualTo(3));
            Assert.That(_browser.Items[1].LineNumber, Is.EqualTo(2));
            Assert.That(_browser.Items[2].LineNumber, Is.EqualTo(1));

            _browser.ExceptionOrder = ExceptionOrder.Normal;
            Assert.That(_browser.Items[0].LineNumber, Is.EqualTo(1));
            Assert.That(_browser.Items[1].LineNumber, Is.EqualTo(2));
            Assert.That(_browser.Items[2].LineNumber, Is.EqualTo(3));

            // set Normal order once again. The test check that order
            // is reversed only when there is a real change.

            _browser.ExceptionOrder = ExceptionOrder.Normal;
            Assert.That(_browser.Items[0].LineNumber, Is.EqualTo(1));
            Assert.That(_browser.Items[1].LineNumber, Is.EqualTo(2));
            Assert.That(_browser.Items[2].LineNumber, Is.EqualTo(3));

            return;
        }
    }
}
