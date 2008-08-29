// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using System.Collections;
using System.CodeDom.Compiler;
using NUnit.Framework.Constraints;
#if NET_2_0
using System.Collections.Generic;
#endif

namespace NUnit.Framework.Syntax
{
    [TestFixture]
    public class InvalidCodeTests : AssertionHelper
    {
        static readonly string template =
@"using System;
using NUnit.Framework;
using NUnit.Framework.Constraints;

class SomeClass
{
    void SomeMethod()
    {
        object c = $FRAGMENT$;
    }
}";

        [TestCase("Is.Null.Not")]
        [TestCase("Is.Not.Null.GreaterThan(10))")]
        [TestCase("Is.Null.All")]
        [TestCase("Is.And")]
        [TestCase("Is.All.And.And")]
        [TestCase("Is.Null.And.Throws")]
        public void CodeShouldNotCompile(string fragment)
        {
            string code = template.Replace("$FRAGMENT$", fragment);
            TestCompiler compiler = new TestCompiler(
                new string[] { "system.dll", "nunit.framework.dll" },
                "test.dll");
            CompilerResults results = compiler.CompileCode(code);
            if (results.NativeCompilerReturnValue == 0)
                Assert.Fail("Code fragment \"" + fragment + "\" should not compile but it did");
        }

        static object[] UnresolvedConstraints = new object[]
            {
                new TestCaseData( Is.Not ).WithName("Is.Not"),
                new TestCaseData( Is.All ).WithName("Is.All"),
                new TestCaseData( Is.Not.All ).WithName("Is.Not.All"),
                new TestCaseData( Is.All.Not ).WithName("Is.All.Not"),
            };

        [Test, TestCases("UnresolvedConstraints")]
        public void CodeCompilesButIsNotCompleteConstraint(ConstraintBuilder builder)
        {
            Assert.That(builder, Is.Not.AssignableTo(typeof(IConstraint)));
        }
    }
}