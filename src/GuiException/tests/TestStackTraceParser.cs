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

namespace NUnit.UiException.Tests
{
    [TestFixture]
    public class TestStackTraceParser
    {
        private StackTraceParser _parser;

        [SetUp]
        public void SetUp()
        {
            _parser = new StackTraceParser();

            return;
        }

        [Test]
        public void Test_Default()
        {
            Assert.That(_parser.Items, Is.Not.Null);
            Assert.That(_parser.Items.Count, Is.EqualTo(0));

            return;
        }

        [Test]
        public void Test_Parse()
        {
            _parser.Parse("à NUnit.UiException.TraceItem.get_Text() dans C:\\TraceItem.cs:ligne 43");

            Assert.That(_parser.Items.Count, Is.EqualTo(1));
            Assert.That(_parser.Items[0], 
                Is.EqualTo(new ExceptionItem("C:\\TraceItem.cs", "NUnit.UiException.TraceItem.get_Text()", 43)));

            // Parse should clear previous item

            _parser.Parse("");
            Assert.That(_parser.Items.Count, Is.EqualTo(0));

            return;
        }

        [Test]
        public void Test_Parse_MultipleExtension()
        {
            _parser.Parse("à get_Text() dans C:\\TraceItem.cs.cs.cs.cs:ligne 43");
            Assert.That(_parser.Items.Count, Is.EqualTo(1));
            Assert.That(_parser.Items[0].Path, Is.EqualTo("C:\\TraceItem.cs.cs.cs.cs"));

            _parser.Parse("à get_Text() dans C:\\my Document1\\my document2 containing space\\file.cs:line 1");
            Assert.That(_parser.Items.Count, Is.EqualTo(1));
            Assert.That(_parser.Items[0].Path,
                Is.EqualTo("C:\\my Document1\\my document2 containing space\\file.cs"));

            _parser.Parse("à get_Text() dans C:\\my doc\\my doc2.cs\\file.cs:line 1");
            Assert.That(_parser.Items.Count, Is.EqualTo(1));
            Assert.That(_parser.Items[0].Path,
                Is.EqualTo("C:\\my doc\\my doc2.cs\\file.cs"));

            return;
        }

        [Test]
        public void Test_Parse_With_Real_Life_Samples()
        {
            // test ability to extract one trace

            _parser.Parse(
                "à Test.TestTraceItem.Can_Set_Properties() dans " +
                "C:\\Documents and Settings\\ihottier\\Mes documents\\" +
                "NUnit_Stacktrace\\Test\\TestTraceItem.cs:ligne 42\r\n");

            Assert.That(_parser.Items.Count, Is.EqualTo(1));
            Assert.That(_parser.Items[0],
                Is.EqualTo(new ExceptionItem(
                    "C:\\Documents and Settings\\ihottier\\Mes documents\\" +
                    "NUnit_Stacktrace\\Test\\TestTraceItem.cs",
                    "Test.TestTraceItem.Can_Set_Properties()",
                    42)));

            // test ability to extract two traces

            _parser.Parse(
                "à NUnit.UiException.TraceItem.get_Text() " +
                "dans C:\\Documents and Settings\\ihottier\\Mes documents\\" +
                "NUnit.UiException\\TraceItem.cs:ligne 43\r\n" +
                "à Test.TestTaggedText.SetUp() dans C:\\Documents and Settings\\" +
                "ihottier\\Mes documents\\NUnit_Stacktrace\\Test\\TestTaggedText.cs:ligne 30\r\n");

            Assert.That(_parser.Items.Count, Is.EqualTo(2));

            Assert.That(_parser.Items[0],
                Is.EqualTo(
                new ExceptionItem(
                    "C:\\Documents and Settings\\ihottier\\Mes documents\\" +
                    "NUnit.UiException\\TraceItem.cs",
                    "NUnit.UiException.TraceItem.get_Text()",
                    43)));

            Assert.That(_parser.Items[1],
                Is.EqualTo(
                new ExceptionItem(
                    "C:\\Documents and Settings\\" +
                    "ihottier\\Mes documents\\NUnit_Stacktrace\\Test\\TestTaggedText.cs",
                    "Test.TestTaggedText.SetUp()",
                    30)));

            return;
        }

        [Test]
        public void Test_Parse_Should_Not_Be_Case_Sensitive()
        {
            //
            // NUnit.UiException.Tests ability to not be case sensitive when
            // working with csharp file extension
            //

            _parser.Parse("à test() C:\\file.CS:ligne 1");
            Assert.That(_parser.Items.Count, Is.EqualTo(1));
            Assert.That(_parser.Items[0], Is.EqualTo(
                new ExceptionItem("C:\\file.CS", "test()", 1)));

            _parser.Parse("à test() C:\\file.cS:ligne 1");
            Assert.That(_parser.Items.Count, Is.EqualTo(1));
            Assert.That(_parser.Items[0], Is.EqualTo(
                new ExceptionItem("C:\\file.cS", "test()", 1)));

            return;
        }

        [Test]
        public void Test_Trace_When_Missing_File()
        {
            //
            // NUnit.UiException.Tests ability to not be confused
            // if source code attachment is missing
            //

            _parser.Parse(
                "à System.String.InternalSubStringWithChecks(Int32 startIndex, Int32 length, Boolean fAlwaysCopy)\r\n" +
                "à NUnit.UiException.StackTraceParser.Parse(String stackTrace) dans C:\\StackTraceParser.cs:ligne 55\r\n" +
                "à Test.TestStackTraceParser.Test_Parse() dans C:\\TestStackTraceParser.cs:ligne 36\r\n"
                );

            Assert.That(_parser.Items.Count, Is.EqualTo(3));

            Assert.That(_parser.Items[0].HasSourceAttachment, Is.False);
            Assert.That(_parser.Items[0].FullyQualifiedMethodName,
                Is.EqualTo("System.String.InternalSubStringWithChecks()"));

            Assert.That(_parser.Items[1], 
                Is.EqualTo(
                    new ExceptionItem(
                        "C:\\StackTraceParser.cs", 
                        "NUnit.UiException.StackTraceParser.Parse()",
                        55)));
            Assert.That(_parser.Items[2],
                Is.EqualTo(
                    new ExceptionItem(
                        "C:\\TestStackTraceParser.cs",
                        "Test.TestStackTraceParser.Test_Parse()",
                        36)));

            return;
        }

        [Test]
        public void Test_Missing_Line_Number()
        {
            //
            // NUnit.UiException.Tests ability to not be confused
            // if line number is missing
            //

            _parser.Parse("à Test.TestStackTraceParser.Test_Parse() dans C:\\TestStackTraceParser.cs:\r\n");

            Assert.That(_parser.Items.Count, Is.EqualTo(1));
            Assert.That(_parser.Items[0], 
                Is.EqualTo(new ExceptionItem(
                    "C:\\TestStackTraceParser.cs", 
                    "Test.TestStackTraceParser.Test_Parse()",
                    0)));

            return;
        }

        [Test]
        public void Test_English_Stack()
        {
            //
            // NUnit.UiException.Tests ability of the parser to not depend
            // of the language
            //

            _parser.Parse("at Test.TestStackTraceParser.Test_Parse() in C:\\TestStackTraceParser.cs:line 36\r\n");

            Assert.That(_parser.Items.Count, Is.EqualTo(1));
            Assert.That(_parser.Items[0], Is.EqualTo(
                new ExceptionItem("C:\\TestStackTraceParser.cs", 
                    "Test.TestStackTraceParser.Test_Parse()", 36)));

            return;
        }

        [Test]
        public void Test_Can_Set_DirectorySeparatorChar()
        {
            //
            // NUnit.UiException.Tests ability to not depend of one file system
            //

            _parser.DirectorySeparator = '/';
            Assert.That(_parser.DirectorySeparator, Is.EqualTo('/'));

            _parser.Parse(
                "at Test.TestStackTraceParser.Test_Parse() in /home/ihottier/work/stacktrace/test/TestStackTraceParser.cs:line 36"
                );

            Assert.That(_parser.Items.Count, Is.EqualTo(1));
            Assert.That(_parser.Items[0], Is.EqualTo(
                new ExceptionItem(
                    "/home/ihottier/work/stacktrace/test/TestStackTraceParser.cs",
                    "Test.TestStackTraceParser.Test_Parse()",
                    36)));

            return;
        }

        [Test]
        public void Test_Unknown_Extension_Is_Ignored()
        {
            //
            // NUnit.UiException.Tests that a non-csharp file is ignored
            //
            // TODO: This is not the optimal behavior, we should try to
            // display with a different textmanager or use the default
            // with no highlighting. That will require a different way
            // to locate the filename in a stack entry.

            _parser.Parse("à Test.TestStackTraceParser.Test_Parse() in C:\\TestStackTraceParser.vb:line 36");

            Assert.That(_parser.Items.Count, Is.EqualTo(1));
            Assert.That(_parser.Items[0], Is.EqualTo(
                new ExceptionItem(
                    null,
                    "Test.TestStackTraceParser.Test_Parse()",
                    36)));

            return;
        }

        [Test]
        public void Test_Can_Set_CSharp_File_Extension()
        {
            //
            // NUnit.UiException.Tests ability to not depend of a specific extension
            //

            _parser.FileExtension = ".csharp";
            Assert.That(_parser.FileExtension, Is.EqualTo(".csharp"));

            _parser.Parse("à Test.TestStackTraceParser.Test_Parse() in C:\\TestStackTraceParser.csharp:line 36");

            Assert.That(_parser.Items.Count, Is.EqualTo(1));
            Assert.That(_parser.Items[0], Is.EqualTo(
                new ExceptionItem(
                    "C:\\TestStackTraceParser.csharp",
                    "Test.TestStackTraceParser.Test_Parse()",
                    36)));

            return;
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException),
            ExpectedMessage = "FileExtension",
            MatchType = MessageMatch.Contains)]
        public void Test_FileExtension_Can_Throw_NullExtensionValue()
        {
            _parser.FileExtension = null; // throws exception
        }

        [Test]
        public void Test_Parse_Null()
        {
            _parser.Parse(null);
        }
    }
}
