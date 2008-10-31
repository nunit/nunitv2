// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

using System;
using System.Threading;
using System.Collections;

namespace NUnit.Framework.Syntax
{
	public class AfterTest_SimpleConstraint : SyntaxTest
	{
		[SetUp]
		public void SetUp()
		{
			parseTree = "<after 1000 <equal 10>>";
			staticSyntax = Is.EqualTo(10).After(1000);
			inheritedSyntax = Helper().EqualTo(10).After(1000);
			builderSyntax = Builder().EqualTo(10).After(1000);
		}
	}

	public class AfterTest_ProperyTest : SyntaxTest
	{
		[SetUp]
		public void SetUp()
		{
			parseTree = "<after 1000 <property X <equal 10>>>";
			staticSyntax = Has.Property("X").EqualTo(10).After(1000);
			inheritedSyntax = Helper().Property("X").EqualTo(10).After(1000);
			builderSyntax = Builder().Property("X").EqualTo(10).After(1000);
		}
	}

	public class AfterTest_AndOperator : SyntaxTest
	{
		[SetUp]
		public void SetUp()
		{
			parseTree = "<after 1000 <and <greaterthan 0> <lessthan 10>>>";
			staticSyntax = Is.GreaterThan(0).And.LessThan(10).After(1000);
			inheritedSyntax = Helper().GreaterThan(0).And.LessThan(10).After(1000);
			builderSyntax = Builder().GreaterThan(0).And.LessThan(10).After(1000);
		}
	}

#if NET_2_0
    public class AfterSyntaxUsingAnonymousDelegates
    {
        [Test]
        public void TrueTest()
        {
            bool value = false;

            new Timer(delegate { value = true; }, null, 100, Timeout.Infinite);

            Assert.That(delegate { return value; }, Is.True.After(5000, 200));
        }

        [Test]
        public void EqualToTest()
        {
            int value = 0;

            new Timer(delegate { value = 1; }, null, 100, Timeout.Infinite);

            Assert.That(delegate { return value; }, Is.EqualTo(1).After(5000, 200));
        }

        [Test]
        public void SameAsTest()
        {
            object oldValue = new object();
            object newValue = new object();

            new Timer(delegate { oldValue = newValue; }, null, 100, Timeout.Infinite);

            Assert.That(delegate { return oldValue; }, Is.SameAs(newValue).After(5000, 200));
        }

        [Test]
        public void GreaterTest()
        {
            int value = 0;

            new Timer(delegate { value = 5; }, null, 100, Timeout.Infinite);

            Assert.That(delegate { return value; }, Is.GreaterThan(1).After(5000,200));
        }

        [Test]
        public void HasMemberTest()
        {
            ArrayList list = new ArrayList();
            list.Add(1);
            list.Add(2);
            list.Add(3);

            new Timer(delegate { list.Add(4); }, null, 100, Timeout.Infinite);

            Assert.That(delegate { return list; }, Has.Member(4).After(5000, 200));
        }

        [Test]
        public void NullTest()
        {
            object value = new object();

            new Timer(delegate { value = null; }, null, 100, Timeout.Infinite);

            Assert.That(delegate { return value; }, Is.Null.After(5000, 200));
        }

        [Test]
        public void TextTest()
        {
            string value = "hello";

            new Timer(delegate { value += "world"; }, null, 100, Timeout.Infinite);

            Assert.That(delegate { return value; }, Text.EndsWith("world").After(5000, 200));
        }
    }

    public class AfterSyntaxUsingActualPassedByRef
    {
        [Test]
        public void TrueTest()
        {
            bool value = false;

            new Timer(delegate { value = true; }, null, 100, Timeout.Infinite);

            Assert.That(ref value, Is.True.After(5000, 200));
        }

        [Test]
        public void EqualToTest()
        {
            int value = 0;

            new Timer(delegate { value = 1; }, null, 100, Timeout.Infinite);

            Assert.That(ref value, Is.EqualTo(1).After(5000, 200));
        }

        [Test]
        public void SameAsTest()
        {
            object oldValue = new object();
            object newValue = new object();

            new Timer(delegate { oldValue = newValue; }, null, 100, Timeout.Infinite);

            Assert.That(ref oldValue, Is.SameAs(newValue).After(5000, 200));
        }

        [Test]
        public void GreaterTest()
        {
            int value = 0;

            new Timer(delegate { value = 5; }, null, 100, Timeout.Infinite);

            Assert.That(ref value, Is.GreaterThan(1).After(5000, 200));
        }

        [Test]
        public void HasMemberTest()
        {
            ArrayList list = new ArrayList();
            list.Add(1);
            list.Add(2);
            list.Add(3);

            new Timer(delegate { list.Add(4); }, null, 100, Timeout.Infinite);

            Assert.That(ref list, Has.Member(4).After(5000, 200));
        }

        [Test]
        public void NullTest()
        {
            object value = new object();

            new Timer(delegate { value = null; }, null, 100, Timeout.Infinite);

            Assert.That(ref value, Is.Null.After(5000, 200));
        }

        [Test]
        public void TextTest()
        {
            string value = "hello";

            new Timer(delegate { value += "world"; }, null, 100, Timeout.Infinite);

            Assert.That(ref value, Text.EndsWith("world").After(5000, 200));
        }
    }
#endif
}