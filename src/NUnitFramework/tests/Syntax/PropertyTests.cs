using System;
using System.Collections;

namespace NUnit.Framework.Syntax
{
    public class PropertyTest_Existence : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<property X>";
            staticSyntax = Has.Property("X");
            inheritedSyntax = Helper().Property("X");
            builderSyntax = Builder().Property("X");
        }
    }

    public class PropertyTest_Value : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<property X <equal 5>>";
            staticSyntax = Has.Property("X", 5);
            inheritedSyntax = Helper().Property("X", 5);
            builderSyntax = Builder().Property("X", 5);
        }
    }

    public class PropertyTest_Constraint : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<property X <greaterthan 5>>";
            staticSyntax = Has.Property("X").GreaterThan(5);
            inheritedSyntax = Helper().Property("X").GreaterThan(5);
            builderSyntax = Builder().Property("X").GreaterThan(5);
        }
    }

    public class LengthTest : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<property Length <equal 5>>";
            staticSyntax = Has.Length(5);
            inheritedSyntax = Helper().Length(5);
            builderSyntax = Builder().Length(5);
        }
    }

    //public class LengthTest_Constraint : SyntaxTest
    //{
    //    [SetUp]
    //    public void SetUp()
    //    {
    //        parseTree = "<property Length <greaterthan 5>>";
    //        staticSyntax = Has.Length().GreaterThan(5);
    //        inheritedSyntax = Helper().Length().GreaterThan(5);
    //        builderSyntax = Builder().Length().GreaterThan(5);
    //    }
    //}

    public class CountTest : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<property Count <equal 5>>";
            staticSyntax = Has.Count(5);
            inheritedSyntax = Helper().Count(5);
            builderSyntax = Builder().Count(5);
        }
    }

    //public class CountTest_Constraint : SyntaxTest
    //{
    //    [SetUp]
    //    public void SetUp()
    //    {
    //        parseTree = "<property Count <greaterthan 5>>";
    //        staticSyntax = Has.Count.GreaterThan(5);
    //        inheritedSyntax = Helper().Count.GreaterThan(5);
    //        builderSyntax = Builder().Count.GreaterThan(5);
    //    }
    //}

    public class MessageTest : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = @"<property Message <equal ""my message"">>";
            staticSyntax = Has.Message("my message");
            inheritedSyntax = Helper().Message("my message");
            builderSyntax = Builder().Message("my message");
        }
    }

    //public class MessageTest_Constraint : SyntaxTest
    //{
    //    [SetUp]
    //    public void SetUp()
    //    {
    //        parseTree = @"<property Message <startswith ""Expected"">>";
    //        staticSyntax = Has.Message().StartsWith("Expected");
    //        inheritedSyntax = Helper().Message().StartsWith("Expected");
    //        builderSyntax = Builder().Message().StartsWith("Expected");
    //    }
    //}
}
