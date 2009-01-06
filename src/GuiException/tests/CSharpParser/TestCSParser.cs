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
using NUnit.UiException.CSharpParser;

namespace NUnit.UiException.Tests.CSharpParser
{
    [TestFixture]
    public class TestCSParser
    {
        private TestingCSParser _parser;

        [SetUp]
        public void SetUp()
        {
            _parser = new TestingCSParser();

            return;
        }

        [Test]
        public void Test_Default()
        {
            Assert.That(_parser.CSCode, Is.Not.Null);
            Assert.That(_parser.CSCode.Text, Is.EqualTo(""));
            Assert.That(_parser.CSCode.LineCount, Is.EqualTo(0));

            return;
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_Parse_Throw_NullStringException()
        {
            _parser.Parse(null); // throws exception
        }

        [Test]
        public void Test_PreProcess()
        {
            // PreProcess is expected to remove '\t' sequences.
            // This test expects that normal strings are left untouched.

            Assert.That(_parser.PreProcess("hello world"), Is.EqualTo("hello world"));

            // This test expects to see differences
            Assert.That(_parser.PreProcess("hello\tworld"), Is.EqualTo("hello    world"));

            // test to fail: passing null has no effect.
            Assert.That(_parser.PreProcess(null), Is.Null);

            return;
        }

        [Test]
        public void Test_Parse()
        {
            CSCode exp;

            _parser.Parse("line 1\n  line 2\nline 3\n");

            exp = new TestingCSCode(
                "line 1\n  line 2\nline 3\n",
                new int[] { 0, 7, 16 },
                new byte[] { 0, 0, 0 },
                new int[] { 0, 1, 2 }
                );

            Assert.That(_parser.CSCode, Is.EqualTo(exp));

            return;
        }

        [Test]
        public void Test_Parse_2()
        {
            CSCode exp;

            _parser.Parse(
                "int i; //comment\n" +
                "char c='a';\n");

            exp = new TestingCSCode(
                "int i; //comment\n" +
                "char c='a';\n",
                new int[] { 0, 3, 7, 16, 17, 21, 24, 27 },
                new byte[] { 1, 0, 2, 0, 1, 0, 3, 0 },
                new int[] { 0, 4 }
            );

            Assert.That(_parser.CSCode, Is.EqualTo(exp));

            return;
        }

        [Test]
        public void Test_Parse_3()
        {
            CSCode exp;

            // Ensure that escaping sequences are
            // handled correctly
            //             0  2           14   17    21        
            _parser.Parse("s=\"<font class=\\\"cls\\\">hi, there!</font>");

            exp = new TestingCSCode(
                "s=\"<font class=\\\"cls\\\">hi, there!</font>",
                new int[] { 0, 2 },
                new byte[] { 0, 3 },
                new int[] { 0 });

            Assert.That(_parser.CSCode, Is.EqualTo(exp));

            _parser = new TestingCSParser();

            //             0  2              
            _parser.Parse("s=\"<font class=\\\\\"cls\\\">hi, there!</font>");
            exp = new TestingCSCode(
                "s=\"<font class=\\\\\"cls\\\">hi, there!</font>",
                new int[] { 0, 2, 18, 22 },
                new byte[] { 0, 3, 0, 3 },
                new int[] { 0 });

            Assert.That(_parser.CSCode, Is.EqualTo(exp));

            _parser.Parse("s=\"<font class=\\\\\\\"cls\\\">hi, there!</font>");

            return;
        }

        #region TestingCSParser

        class TestingCSParser :
            CSParser
        {
            public new string PreProcess(string text)
            {
                return (base.PreProcess(text));
            }
        }

        #endregion

        #region TestingCSCode

        class TestingCSCode :
            CSCode
        {
            public TestingCSCode(string csharpText, int[] strIndexes, byte[] tagValues, int[] lineIndexes)
            {
                _codeInfo = new CodeInfo();

                _codeInfo.Text = csharpText;

                _codeInfo.IndexArray = new List<int>();
                foreach (int index in strIndexes)
                    _codeInfo.IndexArray.Add(index);

                _codeInfo.TagArray = new List<byte>();
                foreach (byte tag in tagValues)
                    _codeInfo.TagArray.Add(tag);

                _codeInfo.LineArray = new List<int>();
                foreach (int line in lineIndexes)
                    _codeInfo.LineArray.Add(line);

                return;
            }
        }

        #endregion
    }
}
