// *********************************************************************
// Copyright 2007, Andreas Schlapsi
// This is free software licensed under the MIT license. 
// *********************************************************************
using System;
using System.Reflection;
using NUnit.Core;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace NUnit.Core.Extensions.RowTest.UnitTests
{
	[TestFixture]
	public class RowTestSuiteTest : BaseTestFixture
	{
		[Test]
		public void Initialize()
		{
			MethodInfo method = GetRowTestMethodWith2Rows();
			
			RowTestSuite testSuite = new RowTestSuite(method);
			
			string pathName = method.DeclaringType.ToString();
			Assert.That(testSuite.TestName.FullName, Is.EqualTo (pathName + "." + method.Name));
		}
		
		[Test]
		public void Run()
		{
			TestClass fixture = new TestClass();
			
			TestSuite parentSuite = new TestSuite("ParentSuiteName", "Name");
			parentSuite.Fixture = fixture;
			
			RowTestSuite rowTestSuite = new RowTestSuite(GetRowTestMethodWith2Rows());
			parentSuite.Add(rowTestSuite);
			
			rowTestSuite.Run(new NullListener());
			
			Assert.That(rowTestSuite.Fixture, Is.SameAs(fixture));
		}
		
		[Test]
		public void Run_WithoutParent()
		{
			RowTestSuite rowTestSuite = new RowTestSuite(GetRowTestMethodWith2Rows());
			
			rowTestSuite.Run(new NullListener());
			
			Assert.That(rowTestSuite.Fixture, Is.Null);
		}
		
		[Test]
		public void Run_WithTestFilter()
		{
			TestClass fixture = new TestClass();
			
			TestSuite parentSuite = new TestSuite("ParentSuiteName", "Name");
			parentSuite.Fixture = fixture;
			
			RowTestSuite rowTestSuite = new RowTestSuite(GetRowTestMethodWith2Rows());
			parentSuite.Add(rowTestSuite);
			
			rowTestSuite.Run(new NullListener(), TestFilter.Empty);
			
			Assert.That(rowTestSuite.Fixture, Is.SameAs(fixture));
		}
	}
}
