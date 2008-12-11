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
using System.Collections;
using NUnit.UiException;

namespace NUnit.UiException.Tests.CSharpParser
{
    [TestFixture]
    public class TestCSCode
    {
        private CSCode _code;       

        [Test]
        public void Test_SimpleCollection()
        {
            _code = new TestingCSCode(
                "line 1\n  line 2\nline 3\n",
                new int[] { 0, 7, 16 },
                new byte[] { 0, 0,  0 },
                new int[] { 0, 1, 2 }
                );

            Assert.That(_code.Text, Is.EqualTo("line 1\n  line 2\nline 3\n"));
            Assert.That(_code.LineCount, Is.EqualTo(3));

            Assert.That(_code[0], Is.Not.Null);
            Assert.That(_code[0].Text, Is.EqualTo("line 1"));

            Assert.That(_code[1], Is.Not.Null);
            Assert.That(_code[1].Text, Is.EqualTo("  line 2"));

            Assert.That(_code[2], Is.Not.Null);
            Assert.That(_code[2].Text, Is.EqualTo("line 3"));

            // check internal data

            Assert.That(_code[0].Count, Is.EqualTo(1));
            Assert.That(_code[0][0].Text, Is.EqualTo("line 1"));
            Assert.That(_code[0][0].Tag, Is.EqualTo(ClassificationTag.Code));

            Assert.That(_code[1].Count, Is.EqualTo(1));
            Assert.That(_code[1][0].Text, Is.EqualTo("  line 2"));
            Assert.That(_code[1][0].Tag, Is.EqualTo(ClassificationTag.Code));

            Assert.That(_code[2].Count, Is.EqualTo(1));
            Assert.That(_code[2][0].Text, Is.EqualTo("line 3"));
            Assert.That(_code[2][0].Tag, Is.EqualTo(ClassificationTag.Code));

            return;
        }

        [Test]
        public void Test_ComplexCollection()
        {
            _code = new TestingCSCode(
                "int i; //comment\n" +
                "char c='a';\n",
                new int[] { 0, 4, 7, 17, 22, 24, 27 },
                new byte[] { 1, 0, 2,  1,  0,  3,  0 },
                new int[] { 0, 3 }
            );

            Assert.That(_code.Text, Is.EqualTo("int i; //comment\nchar c='a';\n"));
            Assert.That(_code.LineCount, Is.EqualTo(2));

            Assert.That(_code[0], Is.Not.Null);
            Assert.That(_code[0].Text, Is.EqualTo("int i; //comment"));

            Assert.That(_code[1], Is.Not.Null);
            Assert.That(_code[1].Text, Is.EqualTo("char c='a';"));

            // check internal data

            Assert.That(_code[0].Count, Is.EqualTo(3));
            Assert.That(_code[0][0].Text, Is.EqualTo("int "));
            Assert.That(_code[0][0].Tag, Is.EqualTo(ClassificationTag.Keyword));
            Assert.That(_code[0][1].Text, Is.EqualTo("i; "));
            Assert.That(_code[0][1].Tag, Is.EqualTo(ClassificationTag.Code));
            Assert.That(_code[0][2].Text, Is.EqualTo("//comment"));
            Assert.That(_code[0][2].Tag, Is.EqualTo(ClassificationTag.Comment));

            Assert.That(_code[1].Count, Is.EqualTo(4));
            Assert.That(_code[1][0].Text, Is.EqualTo("char "));
            Assert.That(_code[1][0].Tag, Is.EqualTo(ClassificationTag.Keyword));
            Assert.That(_code[1][1].Text, Is.EqualTo("c="));
            Assert.That(_code[1][1].Tag, Is.EqualTo(ClassificationTag.Code));
            Assert.That(_code[1][2].Text, Is.EqualTo("'a'"));
            Assert.That(_code[1][2].Tag, Is.EqualTo(ClassificationTag.String));
            Assert.That(_code[1][3].Text, Is.EqualTo(";"));
            Assert.That(_code[1][3].Tag, Is.EqualTo(ClassificationTag.Code));

            return;
        }

        [Test]
        public void Test_MaxLength()
        {
            _code = new TestingCSCode(
                "", new int[] { }, new byte[] { }, new int[] { });
            Assert.That(_code.MaxLength, Is.EqualTo(0));

            _code = new TestingCSCode(
                "a\r\nabc\r\nab",
                new int[] { 0, 3, 8 },
                new byte[] { 0, 0, 0 },
                new int[] { 0, 1, 2 });
            Assert.That(_code.MaxLength, Is.EqualTo(3));

            _code = new TestingCSCode(
                "a\r\nab\r\nabc",
                new int[] { 0, 3, 7 },
                new byte[] { 0, 0, 0 },
                new int[] { 0, 1, 2 });
            Assert.That(_code.MaxLength, Is.EqualTo(3));

            return;
        }

        [Test]
        public void Test_Set_Text()
        {
            CSCode exp;

            _code.Text = "int i; //comment\n" +
                         "char c='a';\n";

            exp = new TestingCSCode(
                "int i; //comment\n" +
                "char c='a';\n",
                new int[] { 0, 3, 7, 16, 17, 21, 24, 27 },
                new byte[] { 1, 0, 2, 0, 1, 0, 3, 0 },
                new int[] { 0, 4 }
            );

            Assert.That(_code, Is.EqualTo(exp));

            return;
        }

        [Test]
        public void Test_Conserve_Intermediary_Spaces()
        {
            _code = new CSCode();

            _code.Text = "{\r\n" +
                         "    class A { }\r\n" +
                         "}\r\n";

            Assert.That(_code.LineCount, Is.EqualTo(3));
            Assert.That(_code[0].Text, Is.EqualTo("{"));
            Assert.That(_code[1].Text, Is.EqualTo("    class A { }"));
            Assert.That(_code[2].Text, Is.EqualTo("}"));

            Assert.That(_code[0][0].Text, Is.EqualTo("{"));
            Assert.That(_code[1][0].Text, Is.EqualTo("    "));
            Assert.That(_code[1][1].Text, Is.EqualTo("class"));
            Assert.That(_code[1][2].Text, Is.EqualTo(" A { }"));
            Assert.That(_code[2][0].Text, Is.EqualTo("}"));

            return;
        }

        [Test]
        public void Test_Equals()
        {
            _code = new TestingCSCode(
               "line",
               new int[] { 0 },
               new byte[] { 0 },
               new int[] { 0 }
               );

            // Tests to fail

            Assert.That(_code.Equals(null), Is.False);
            Assert.That(_code.Equals("hello"), Is.False);
            Assert.That(_code.Equals(
                new TestingCSCode("a", new int[] { 0 }, new byte[] { 0 }, new int[] { 0 })),
                Is.False);
            Assert.That(_code.Equals(
                new TestingCSCode("line", new int[] { 1 }, new byte[] { 0 }, new int[] { 0 })),
                Is.False);
            Assert.That(_code.Equals(
                new TestingCSCode("line", new int[] { 0 }, new byte[] { 1 }, new int[] { 0 })),
                Is.False);
            Assert.That(_code.Equals(
                new TestingCSCode("line", new int[] { 0 }, new byte[] { 0 }, new int[] { 1 })),
                Is.False);

            Assert.That(_code.Equals(
                new TestingCSCode("line", new int[] { 0, 0 }, new byte[] { 0 }, new int[] { 0 })),
                Is.False);
            Assert.That(_code.Equals(
                new TestingCSCode("line", new int[] { 0 }, new byte[] { 0, 0 }, new int[] { 0 })),
                Is.False);
            Assert.That(_code.Equals(
                new TestingCSCode("line", new int[] { 0 }, new byte[] { 0 }, new int[] { 0, 0 })),
                Is.False);

            // NUnit.UiException.Tests to pass

            Assert.That(_code.Equals(
                new TestingCSCode("line", new int[] { 0 }, new byte[] { 0 }, new int[] { 0 })),
                Is.True);

            return;
        }

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
