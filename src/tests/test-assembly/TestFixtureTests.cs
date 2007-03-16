// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using NUnit.Framework;

namespace NUnit.TestData.TestFixtureTests
{
	/// <summary>
	/// Classes used for testing NUnit
	/// </summary>

	[TestFixture]
	public class NoDefaultCtorFixture
	{
		public NoDefaultCtorFixture(int index) { }

		[Test] public void OneTest() { }
	}

	[TestFixture]
	public class BadCtorFixture
	{
		BadCtorFixture()
		{
			throw new Exception();
		}

		[Test] public void OneTest()
		{}
	}

	public class AssemblyType
	{
		internal bool called;

		public AssemblyType()
		{
			called = true;
		}
	}

	[TestFixture]
	public class MultipleSetUpAttributes
	{
		[SetUp]
		public void Init1()
		{}

		[SetUp]
		public void Init2()
		{}

		[Test] public void OneTest()
		{}
	}

	[TestFixture]
	public class MultipleTearDownAttributes
	{
		[TearDown]
		public void Destroy1()
		{}

		[TearDown]
		public void Destroy2()
		{}

		[Test] public void OneTest()
		{}
	}

	[TestFixture]
	[Ignore("testing ignore a suite")]
	public class IgnoredFixture
	{
		[Test]
		public void Success()
		{}
	}

	[TestFixture]
	public class OuterClass
	{
		[TestFixture]
		public class NestedTestFixture
		{
			[TestFixture]
				public class DoublyNestedTestFixture
			{
				[Test]
				public void Test()
				{
				}
			}
		}
	}

	[TestFixture]
	public abstract class AbstractTestFixture
	{
		[TearDown]
		public void Destroy1()
		{}
	}

	[TestFixture]
	public class BaseClassTestFixture
	{
		[Test]
		public void Success() { }
	}
	
	public abstract class AbstractDerivedTestFixture : BaseClassTestFixture
	{
	}

	[TestFixture]
	public class MultipleFixtureSetUpAttributes
	{
		[TestFixtureSetUp]
		public void Init1()
		{}

		[TestFixtureSetUp]
		public void Init2()
		{}

		[Test] public void OneTest()
		{}
	}

	[TestFixture]
	public class MultipleFixtureTearDownAttributes
	{
		[TestFixtureTearDown]
		public void Destroy1()
		{}

		[TestFixtureTearDown]
		public void Destroy2()
		{}

		[Test] public void OneTest()
		{}
	}

	[TestFixture]
	public class PrivateSetUp
	{
		[SetUp]
		private void Setup()	{}
	}

	[TestFixture]
	public class ProtectedSetUp
	{
		[SetUp]
		protected void Setup()	{}
	}

	[TestFixture]
	public class StaticSetUp
	{
		[SetUp]
		public static void Setup() {}
	}

	[TestFixture]
	public class SetUpWithReturnValue
	{
		[SetUp]
		public int Setup() { return 0; }
	}

	[TestFixture]
	public class SetUpWithParameters
	{
		[SetUp]
		public void Setup(int j) { }
	}

	[TestFixture]
	public class PrivateTearDown
	{
		[TearDown]
		private void Teardown()	{}
	}

	[TestFixture]
	public class ProtectedTearDown
	{
		[TearDown]
		protected void Teardown()	{}
	}

	[TestFixture]
	public class StaticTearDown
	{
		[SetUp]
		public static void TearDown() {}
	}

	[TestFixture]
	public class TearDownWithReturnValue
	{
		[TearDown]
		public int Teardown() { return 0; }
	}

	[TestFixture]
	public class TearDownWithParameters
	{
		[TearDown]
		public void Teardown(int j) { }
	}

	[TestFixture]
	public class PrivateFixtureSetUp
	{
		[TestFixtureSetUp]
		private void Setup()	{}
	}

	[TestFixture]
	public class ProtectedFixtureSetUp
	{
		[TestFixtureSetUp]
		protected void Setup()	{}
	}

	[TestFixture]
	public class StaticFixtureSetUp
	{
		[TestFixtureSetUp]
		public static void Setup() {}
	}

	[TestFixture]
	public class FixtureSetUpWithReturnValue
	{
		[TestFixtureSetUp]
		public int Setup() { return 0; }
	}

	[TestFixture]
	public class FixtureSetUpWithParameters
	{
		[SetUp]
		public void Setup(int j) { }
	}

	[TestFixture]
	public class PrivateFixtureTearDown
	{
		[TestFixtureTearDown]
		private void Teardown()	{}
	}

	[TestFixture]
	public class ProtectedFixtureTearDown
	{
		[TestFixtureTearDown]
		protected void Teardown()	{}
	}

	[TestFixture]
	public class StaticFixtureTearDown
	{
		[TestFixtureTearDown]
		public static void Teardown() {}
	}

	[TestFixture]
	public class FixtureTearDownWithReturnValue
	{
		[TestFixtureTearDown]
		public int Teardown() { return 0; }
	}

	[TestFixture]
	public class FixtureTearDownWithParameters
	{
		[TestFixtureTearDown]
		public void Teardown(int j) { }
	}

}
