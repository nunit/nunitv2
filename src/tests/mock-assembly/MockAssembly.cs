// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org/?p=license&r=2.4.
// ****************************************************************
using System;
using NUnit.Framework;
using NUnit.Core;

namespace NUnit.Tests
{
	namespace Assemblies
	{
		/// <summary>
		/// Constant definitions for the mock-assembly dll.
		/// </summary>
		public class MockAssembly
		{
			public static int Fixtures = 5; 

			public static int NamespaceSuites = 6; // assembly, NUnit, Tests, Assemblies, Singletons, TestAssembly

			public static int Suites = Fixtures + NamespaceSuites;

			public static int ExplicitFixtures = 1;

			public static int Tests
			{
				get 
				{ 
					return MockTestFixture.Tests 
						+ Singletons.OneTestCase.Tests 
						+ TestAssembly.MockTestFixture.Tests 
						+ IgnoredFixture.Tests
						+ ExplicitFixture.Tests;
				}
			}

			public static int Ignored
			{
				get { return MockTestFixture.Ignored + IgnoredFixture.Tests; }
			}

			public static int Explicit
			{
				get { return MockTestFixture.Explicit + ExplicitFixture.Tests; }
			}

			public static int NotRun
			{
				get { return Ignored + Explicit; }
			}

			public static int Nodes
			{
				get 
				{ 
					return MockTestFixture.Nodes 
						+ Singletons.OneTestCase.Nodes
						+ TestAssembly.MockTestFixture.Nodes 
						+ IgnoredFixture.Nodes
						+ ExplicitFixture.Nodes
						+ 6;  // assembly, NUnit, Tests, Assemblies, Singletons, TestAssembly 
				}
			}

			public static int Categories
			{
				get { return MockTestFixture.Categories; }
			}
		}

		public class MockSuite
		{
			[Suite]
			public static TestSuite Suite
			{
				get
				{
					return new TestSuite( "MockSuite" );
				}
			}
		}

		[TestFixture(Description="Fake Test Fixture")]
		[Category("FixtureCategory")]
		public class MockTestFixture
		{
			public static readonly int Tests = 7;
			public static readonly int Ignored = 2;
			public static readonly int Explicit = 1;
			public static readonly int NotRun = Ignored + Explicit;
			public static readonly int Nodes = Tests + 1;
			public static readonly int Categories = 5;

			[Test(Description="Mock Test #1")]
			public void MockTest1()
			{}

			[Test]
			[Category("MockCategory")]
			[Property("Severity","Critical")]
			public void MockTest2()
			{}

			[Test]
			[Category("MockCategory")]
			[Category("AnotherCategory")]
			public void MockTest3()
			{}

			[Test]
			protected void MockTest5()
			{}

			[Test, Property("TargetMethod", "SomeClassName"), Property("Size", 5), Property("TargetType", typeof( System.Threading.Thread ))]
			public void TestWithManyProperties()
			{}

			[Test]
			[Ignore("ignoring this test method for now")]
			[Category("Foo")]
			public void MockTest4()
			{}

			[Test, Explicit]
			[Category( "Special" )]
			public void ExplicitlyRunTest()
			{}
		}
	}

	namespace Singletons
	{
		[TestFixture]
		public class OneTestCase
		{
			public static readonly int Tests = 1;
			public static readonly int Nodes = Tests + 1;

			[Test]
			public virtual void TestCase() 
			{}
		}
	}

	namespace TestAssembly
	{
		[TestFixture]
		public class MockTestFixture
		{
			public static int Tests = 1;
			public static int Nodes = Tests + 1;

			[Test]
			public void MyTest()
			{
			}
		}
	}

	[TestFixture, Ignore]
	public class IgnoredFixture
	{
		public static int Tests = 3;
		public static int Nodes = Tests + 1;

		[Test]
		public void Test1() { }

		[Test]
		public void Test2() { }
		
		[Test]
		public void Test3() { }
	}

	[TestFixture,Explicit]
	public class ExplicitFixture
	{
		public static int Tests = 2;
		public static int Nodes = Tests + 1;

		[Test]
		public void Test1() { }

		[Test]
		public void Test2() { }
	}
}
