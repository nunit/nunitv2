using System;

namespace NUnit.Framework.Syntax
{
    public class NullTest : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<equal null>";
            staticSyntax = Is.Null;
            inheritedSyntax = Helper().Null;
            builderSyntax = Builder().Null;
        }
    }

    public class TrueTest : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<equal True>";
            staticSyntax = Is.True;
            inheritedSyntax = Helper().True;
            builderSyntax = Builder().True;
        }
    }

    public class FalseTest : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<equal False>";
            staticSyntax = Is.False;
            inheritedSyntax = Helper().False;
            builderSyntax = Builder().False;
        }
    }

    public class NaNTest : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<equal NaN>";
            staticSyntax = Is.NaN;
            inheritedSyntax = Helper().NaN;
            builderSyntax = Builder().NaN;
        }
    }
}
