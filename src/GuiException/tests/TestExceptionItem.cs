// ----------------------------------------------------------------
// ErrorBrowser
// Copyright 2008-2009, Irénée HOTTIER,
// 
// This is free software licensed under the NUnit license, You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NUnit.UiException;
using NUnit.UiException.Tests.data;

namespace NUnit.UiException.Tests
{
    [TestFixture]
    public class TestErrorItem
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException),
            ExpectedMessage = "path",
            MatchType = MessageMatch.Contains)]
        public void Ctor_Throws_NullPathException()
        {
           new ErrorItem(null, 1); // throws exception
        }

        [Test]        
        public void Ctor_With_Line_0()
        {
            new ErrorItem("file.txt", 0);
        }

        [Test]
        public void Ctor_2()
        {
            ErrorItem item;
            
            item = new ErrorItem("Test.cs", "myFunction()", 1);

            Assert.That(item.Path, Is.EqualTo("Test.cs"));
            Assert.That(item.FullyQualifiedMethodName, Is.EqualTo("myFunction()"));            
            Assert.That(item.LineNumber, Is.EqualTo(1));
            Assert.That(item.HasSourceAttachment, Is.True);
            Assert.That(item.FileExtension, Is.EqualTo("cs"));

            item = new ErrorItem(null, "myFunction()", 1);
            Assert.That(item.Path, Is.Null);
            Assert.That(item.FileExtension, Is.Null);
            Assert.That(item.FullyQualifiedMethodName, Is.EqualTo("myFunction()"));
            Assert.That(item.LineNumber, Is.EqualTo(1));
            Assert.That(item.HasSourceAttachment, Is.False);

            return;
        }

        [Test]
        public void Test_MethodName()
        {
            ErrorItem item;

            // test to pass

            item = new ErrorItem("path", "namespace1.class.fullMethodName()", 1);
            Assert.That(item.MethodName, Is.EqualTo("fullMethodName()"));
            Assert.That(item.ClassName, Is.EqualTo("class"));

            item = new ErrorItem("path", ".class.fullMethodName()", 1);
            Assert.That(item.MethodName, Is.EqualTo("fullMethodName()"));
            Assert.That(item.ClassName, Is.EqualTo("class"));

            // test to fail

            item = new ErrorItem("path", "fullMethodName()", 1);
            Assert.That(item.MethodName, Is.EqualTo("fullMethodName()"));
            Assert.That(item.ClassName, Is.EqualTo(""));

            item = new ErrorItem("path", "", 1);
            Assert.That(item.MethodName, Is.EqualTo(""));
            Assert.That(item.ClassName, Is.EqualTo(""));

            return;
        }

        [Test]
        public void Can_Set_Properties()
        {
            ErrorItem item;

            item = new ErrorItem("C:\\dir\\file.txt", 13);

            Assert.That(item.Filename, Is.EqualTo("file.txt"));
            Assert.That(item.FileExtension, Is.EqualTo("txt"));
            Assert.That(item.Path, Is.EqualTo("C:\\dir\\file.txt"));
            Assert.That(item.LineNumber, Is.EqualTo(13));
            Assert.That(item.HasSourceAttachment, Is.True);

            item = new ErrorItem();
            Assert.That(item.Filename, Is.Null);
            Assert.That(item.FileExtension, Is.Null);
            Assert.That(item.Path, Is.Null);
            Assert.That(item.LineNumber, Is.EqualTo(0));
            Assert.That(item.HasSourceAttachment, Is.False);

            return;
        }

        [Test]
        public void Test_FileExtension()
        {
            ErrorItem item;

            item = new ErrorItem("C:\\dir\\file.cs", 1);
            Assert.That(item.FileExtension, Is.EqualTo("cs"));

            item = new ErrorItem("C:\\dir\\file.cpp", 1);
            Assert.That(item.FileExtension, Is.EqualTo("cpp"));

            item = new ErrorItem("C:\\dir\\file.cs.cpp.plop", 1);
            Assert.That(item.FileExtension, Is.EqualTo("plop"));

            item = new ErrorItem("C:\\dir\\file.", 1);
            Assert.That(item.FileExtension, Is.Null);

            item = new ErrorItem("C:\\dir\\file", 1);
            Assert.That(item.FileExtension, Is.Null);

            return;
        }

        [Test]
        [ExpectedException(typeof(ApplicationException),
            ExpectedMessage = "unknown.txt",
            MatchType = MessageMatch.Contains)]
        public void Text_Property_Throws_FileNotExistException()
        {
            ErrorItem item;

            item = new ErrorItem("C:\\unknown\\unknown.txt", 1);

            string text = item.Text; // throws exception
        }

        [Test]
        public void Test_Text()
        {
            ErrorItem item;

            using (new TestResource("HelloWorld.txt"))
            {
                item = new ErrorItem("HelloWorld.txt", 1);

                Assert.That(item.Text, Is.Not.Null);
                Assert.That(item.Text, Is.EqualTo("Hello world!"));
            }

            return;
        }        

        [Test]
        public void Test_Equals()
        {
            ErrorItem itemA;
            ErrorItem itemB;
            ErrorItem itemC;

            itemA = new ErrorItem("file1.txt", 43);
            itemB = new ErrorItem("file2.txt", 44);
            itemC = new ErrorItem("file1.txt", "myFunction()", 43);

            Assert.That(itemA.Equals(null), Is.False);
            Assert.That(itemA.Equals("hello"), Is.False);
            Assert.That(itemA.Equals(itemB), Is.False);
            Assert.That(itemA.Equals(itemC), Is.False);
            Assert.That(itemA.Equals(itemA), Is.True);
            Assert.That(itemA.Equals(new ErrorItem("file", 43)), Is.False);
            Assert.That(itemA.Equals(new ErrorItem("file1.txt", 42)), Is.False);
            Assert.That(itemA.Equals(new ErrorItem("file1.txt", 43)), Is.True);

            return;
        }
    }
}
