using System;

namespace NUnit.Framework.Syntax
{
    [TestFixture]
    public class ExactTypeTest : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<typeof System.String>";
            staticSyntax = Is.TypeOf(typeof(string));
            inheritedSyntax = Helper().TypeOf(typeof(string));
            builderSyntax = Builder().TypeOf(typeof(string));
        }
    }

    [TestFixture]
    public class InstanceOfTest : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<instanceof System.String>";
            staticSyntax = Is.InstanceOfType(typeof(string));
            inheritedSyntax = Helper().InstanceOfType(typeof(string));
            builderSyntax = Builder().InstanceOfType(typeof(string));
        }
    }

    [TestFixture]
    public class AssignableFromTest : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<assignablefrom System.String>";
            staticSyntax = Is.AssignableFrom(typeof(string));
            inheritedSyntax = Helper().AssignableFrom(typeof(string));
            builderSyntax = Builder().AssignableFrom(typeof(string));
        }
    }

    [TestFixture]
    public class AssignableToTest : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<assignableto System.String>";
            staticSyntax = Is.AssignableTo(typeof(string));
            inheritedSyntax = Helper().AssignableTo(typeof(string));
            builderSyntax = Builder().AssignableTo(typeof(string));
        }
    }

#if NET_2_0
    [TestFixture]
    public class ExactTypeTest_Generic : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<typeof System.String>";
            staticSyntax = Is.TypeOf<string>();
            inheritedSyntax = Helper().TypeOf<string>();
            builderSyntax = Builder().TypeOf<string>();
        }
    }

    [TestFixture]
    public class InstanceOfTest_Generic : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<instanceof System.String>";
            staticSyntax = Is.InstanceOfType<string>();
            inheritedSyntax = Helper().InstanceOfType<string>();
            builderSyntax = Builder().InstanceOfType<string>();
        }
    }

    [TestFixture]
    public class AssignableFromTest_Generic : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<assignablefrom System.String>";
            staticSyntax = Is.AssignableFrom<string>();
            inheritedSyntax = Helper().AssignableFrom<string>();
            builderSyntax = Builder().AssignableFrom<string>();
        }
    }

    [TestFixture]
    public class AssignableToTest_Generic : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<assignableto System.String>";
            staticSyntax = Is.AssignableTo<string>();
            inheritedSyntax = Helper().AssignableTo<string>();
            builderSyntax = Builder().AssignableTo<string>();
        }
    }
#endif
}
