// ****************************************************************
// Copyright 2012, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

using System;
using System.IO;
using NUnit.Framework;

namespace NUnit.Core.Tests
{
    [TestFixture]
    public class AssemblyHelperTests
    {
        [Test]
        public void GetPathForAssembly()
        {
            string path = AssemblyHelper.GetAssemblyPath(this.GetType().Assembly);
            Assert.That(Path.GetFileName(path), Is.EqualTo("nunit.core.tests.dll").IgnoreCase);
            Assert.That(File.Exists(path));
        }

        [Test]
        public void GetPathForType()
        {
            string path = AssemblyHelper.GetAssemblyPath(this.GetType());
            Assert.That(Path.GetFileName(path), Is.EqualTo("nunit.core.tests.dll").IgnoreCase);
            Assert.That(File.Exists(path));
        }
		
		[Platform("Win")]
        [TestCase(@"file:///C:/path/to/assembly.dll", Result=@"C:\path\to\assembly.dll")]
        [TestCase(@"file://C:/path/to/assembly.dll", Result=@"C:\path\to\assembly.dll")]
        [TestCase(@"file://C:/my%20path/to%20my/assembly.dll", Result = @"C:\my path\to my\assembly.dll")]
        [TestCase(@"file:///C:/dev/C%23/assembly.dll", Result = @"C:\dev\C#\assembly.dll")]
        [TestCase(@"file:///C:/dev/funnychars?:=/assembly.dll", Result = @"C:\dev\funnychars?:=\assembly.dll")]
        [TestCase(@"file:///path/to/assembly.dll", Result = @"/path/to/assembly.dll")]
        [TestCase(@"file://path/to/assembly.dll", Result = @"//path/to/assembly.dll")]
        [TestCase(@"file:///my%20path/to%20my/assembly.dll", Result = @"/my path/to my/assembly.dll")]
        [TestCase(@"file:///dev/C%23/assembly.dll", Result = @"/dev/C#/assembly.dll")]
        [TestCase(@"file:///dev/funnychars?:=/assembly.dll", Result = @"/dev/funnychars?:=/assembly.dll")]
        //[TestCase(@"http://server/path/to/assembly.dll", Result="//server/path/to/assembly.dll")]
        public string GetAssemblyPathFromEscapedCodeBase_Windows(string uri)
        {
            return AssemblyHelper.GetAssemblyPathFromEscapedCodeBase(uri);
        }
		
		[Platform("Linux")]
        [TestCase(@"file:///path/to/assembly.dll", Result = @"/path/to/assembly.dll")]
        [TestCase(@"file://path/to/assembly.dll", Result = @"/path/to/assembly.dll")]
        [TestCase(@"file:///my path/to my/assembly.dll", Result = @"/my path/to my/assembly.dll")]
        [TestCase(@"file://my path/to my/assembly.dll", Result = @"/my path/to my/assembly.dll")]
        [TestCase(@"file:///dev/C#/assembly.dll", Result = @"/dev/C#/assembly.dll")]
        [TestCase(@"file:///dev/funnychars?:=/assembly.dll", Result = @"/dev/funnychars?:=/assembly.dll")]
        //[TestCase(@"http://server/path/to/assembly.dll", Result="//server/path/to/assembly.dll")]
        public string GetAssemblyPathFromEscapedCodeBase_Linux(string uri)
        {
            return AssemblyHelper.GetAssemblyPathFromEscapedCodeBase(uri);
        }
    }
}