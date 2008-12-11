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
using NUnit.UiException.Tests.data;

namespace NUnit.UiException.Tests
{
    [TestFixture]
    public class TestExceptionItem
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException),
            ExpectedMessage = "path",
            MatchType = MessageMatch.Contains)]
        public void Ctor_Throws_NullPathException()
        {
           new ExceptionItem(null, 1); // throws exception
        }

        [Test]        
        public void Ctor_With_Line_0()
        {
            new ExceptionItem("file.txt", 0);
        }

        [Test]
        public void Ctor_2()
        {
            ExceptionItem item;
            
            item = new ExceptionItem("Test.cs", "myFunction()", 1);

            Assert.That(item.Path, Is.EqualTo("Test.cs"));
            Assert.That(item.FullyQualifiedMethodName, Is.EqualTo("myFunction()"));            
            Assert.That(item.LineNumber, Is.EqualTo(1));
            Assert.That(item.HasSourceAttachment, Is.True);

            item = new ExceptionItem(null, "myFunction()", 1);
            Assert.That(item.Path, Is.Null);
            Assert.That(item.FullyQualifiedMethodName, Is.EqualTo("myFunction()"));
            Assert.That(item.LineNumber, Is.EqualTo(1));
            Assert.That(item.HasSourceAttachment, Is.False);

            return;
        }

        [Test]
        public void Test_MethodName()
        {
            ExceptionItem item;

            // test to pass

            item = new ExceptionItem("path", "namespace1.class.fullMethodName()", 1);
            Assert.That(item.MethodName, Is.EqualTo("fullMethodName()"));
            Assert.That(item.ClassName, Is.EqualTo("class"));

            item = new ExceptionItem("path", ".class.fullMethodName()", 1);
            Assert.That(item.MethodName, Is.EqualTo("fullMethodName()"));
            Assert.That(item.ClassName, Is.EqualTo("class"));

            // test to fail

            item = new ExceptionItem("path", "fullMethodName()", 1);
            Assert.That(item.MethodName, Is.EqualTo("fullMethodName()"));
            Assert.That(item.ClassName, Is.EqualTo(""));

            item = new ExceptionItem("path", "", 1);
            Assert.That(item.MethodName, Is.EqualTo(""));
            Assert.That(item.ClassName, Is.EqualTo(""));

            return;
        }

        [Test]
        public void Can_Set_Properties()
        {
            ExceptionItem item;

            item = new ExceptionItem("C:\\dir\\file.txt", 13);

            Assert.That(item.Filename, Is.EqualTo("file.txt"));
            Assert.That(item.Path, Is.EqualTo("C:\\dir\\file.txt"));
            Assert.That(item.LineNumber, Is.EqualTo(13));
            Assert.That(item.HasSourceAttachment, Is.True);

            item = new ExceptionItem();
            Assert.That(item.Filename, Is.Null);
            Assert.That(item.Path, Is.Null);
            Assert.That(item.LineNumber, Is.EqualTo(0));
            Assert.That(item.HasSourceAttachment, Is.False);

            return;
        }

        [Test]
        [ExpectedException(typeof(ApplicationException),
            ExpectedMessage = "unknown.txt",
            MatchType = MessageMatch.Contains)]
        public void Text_Property_Throws_FileNotExistException()
        {
            ExceptionItem item;

            item = new ExceptionItem("C:\\unknown\\unknown.txt", 1);

            string text = item.Text; // throws exception
        }

        [Test]
        public void Test_Text()
        {
            ExceptionItem item;

            using (new TestResource("HelloWorld.txt"))
            {
                item = new ExceptionItem("HelloWorld.txt", 1);

                Assert.That(item.Text, Is.Not.Null);
                Assert.That(item.Text, Is.EqualTo("Hello world!"));
            }

            return;
        }        

        [Test]
        public void Test_Equals()
        {
            ExceptionItem itemA;
            ExceptionItem itemB;
            ExceptionItem itemC;

            itemA = new ExceptionItem("file1.txt", 43);
            itemB = new ExceptionItem("file2.txt", 44);
            itemC = new ExceptionItem("file1.txt", "myFunction()", 43);

            Assert.That(itemA.Equals(null), Is.False);
            Assert.That(itemA.Equals("hello"), Is.False);
            Assert.That(itemA.Equals(itemB), Is.False);
            Assert.That(itemA.Equals(itemC), Is.False);
            Assert.That(itemA.Equals(itemA), Is.True);
            Assert.That(itemA.Equals(new ExceptionItem("file", 43)), Is.False);
            Assert.That(itemA.Equals(new ExceptionItem("file1.txt", 42)), Is.False);
            Assert.That(itemA.Equals(new ExceptionItem("file1.txt", 43)), Is.True);

            return;
        }
    }
}
