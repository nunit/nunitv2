﻿using System;

namespace NUnit.Framework.Syntax
{
    public class SamePathTest : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            string path = "/path/to/match";

            parseTree = string.Format(@"<samepath ""{0}"">", path);
            staticSyntax = Is.SamePath(path);
            inheritedSyntax = Helper().SamePath(path);
            builderSyntax = Builder().SamePath(path);
        }
    }

    public class SamePathTest_IgnoreCase : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            string path = "/path/to/match";

            parseTree = string.Format(@"<samepath ""{0}"">", path);
            staticSyntax = Is.SamePath(path).IgnoreCase;
            inheritedSyntax = Helper().SamePath(path).IgnoreCase;
            builderSyntax = Builder().SamePath(path).IgnoreCase;
        }
    }

    public class SamePathTest_RespectCase : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            string path = "/path/to/match";

            parseTree = string.Format(@"<samepath ""{0}"">", path);
            staticSyntax = Is.SamePath(path).RespectCase;
            inheritedSyntax = Helper().SamePath(path).RespectCase;
            builderSyntax = Builder().SamePath(path).RespectCase;
        }
    }

    public class SamePathOrUnderTest : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            string path = "/path/to/match";

            parseTree = string.Format(@"<samepathorunder ""{0}"">", path);
            staticSyntax = Is.SamePathOrUnder(path);
            inheritedSyntax = Helper().SamePathOrUnder(path);
            builderSyntax = Builder().SamePathOrUnder(path);
        }
    }

    public class SamePathOrUnderTest_IgnoreCase : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            string path = "/path/to/match";

            parseTree = string.Format(@"<samepathorunder ""{0}"">", path);
            staticSyntax = Is.SamePathOrUnder(path).IgnoreCase;
            inheritedSyntax = Helper().SamePathOrUnder(path).IgnoreCase;
            builderSyntax = Builder().SamePathOrUnder(path).IgnoreCase;
        }
    }

    public class SamePathOrUnderTest_RespectCase : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            string path = "/path/to/match";

            parseTree = string.Format(@"<samepathorunder ""{0}"">", path);
            staticSyntax = Is.SamePathOrUnder(path).RespectCase;
            inheritedSyntax = Helper().SamePathOrUnder(path).RespectCase;
            builderSyntax = Builder().SamePathOrUnder(path).RespectCase;
        }
    }
}
