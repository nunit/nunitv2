using System;
using NUnit.Framework;

namespace NUnit.Tests.TestClasses
{
	/// <summary>
	/// Classes used for testing NUnit
	/// </summary>

	[TestFixture]
	internal class NoDefaultCtorFixture
	{
		public NoDefaultCtorFixture(int index) { }

		[Test] public void OneTest() { }
	}

	[TestFixture]
	internal class BadCtorFixture
	{
		BadCtorFixture()
		{
			throw new Exception();
		}

		[Test] public void OneTest()
		{}
	}

	class AssemblyType
	{
		internal bool called;

		public AssemblyType()
		{
			called = true;
		}
	}

	[TestFixture]
	class MultipleSetUpAttributes
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
	class MultipleTearDownAttributes
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
	class IgnoredFixture
	{
		[Test]
		public void Success()
		{}
	}

	[TestFixture]
	internal class SignatureTestFixture
	{
		[Test]
		public static void Static()
		{
		}

		[Test]
		public int NotVoid() 
		{
			return 1;
		}

		[Test]
		public void Parameters(string test) 
		{}
		
		[Test]
		protected void Protected() 
		{}

		[Test]
		private void Private() 
		{}


		[Test]
		public void TestVoid() 
		{}
	}

	class OuterClass
	{
		[TestFixture]
		internal class NestedTestFixture
		{
			[TestFixture]
				internal class DoublyNestedTestFixture
			{
				[Test]
				public void Test()
				{
				}
			}
		}
	}

	[TestFixture]
	abstract class AbstractTestFixture
	{
		[TearDown]
		public void Destroy1()
		{}
	}

	[TestFixture]
	class MultipleFixtureSetUpAttributes
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
	class MultipleFixtureTearDownAttributes
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
	class PrivateSetUp
	{
		[SetUp]
		private void Setup()	{}
	}

	[TestFixture]
	class ProtectedSetUp
	{
		[SetUp]
		protected void Setup()	{}
	}

	[TestFixture]
	class StaticSetUp
	{
		[SetUp]
		public static void Setup() {}
	}

	[TestFixture]
	class SetUpWithReturnValue
	{
		[SetUp]
		public int Setup() { return 0; }
	}

	[TestFixture]
	class SetUpWithParameters
	{
		[SetUp]
		public void Setup(int j) { }
	}

	[TestFixture]
	class PrivateTearDown
	{
		[TearDown]
		private void Teardown()	{}
	}

	[TestFixture]
	class ProtectedTearDown
	{
		[TearDown]
		protected void Teardown()	{}
	}

	[TestFixture]
	class StaticTearDown
	{
		[SetUp]
		public static void TearDown() {}
	}

	[TestFixture]
	class TearDownWithReturnValue
	{
		[TearDown]
		public int Teardown() { return 0; }
	}

	[TestFixture]
	class TearDownWithParameters
	{
		[TearDown]
		public void Teardown(int j) { }
	}

	[TestFixture]
	class PrivateFixtureSetUp
	{
		[TestFixtureSetUp]
		private void Setup()	{}
	}

	[TestFixture]
	class ProtectedFixtureSetUp
	{
		[TestFixtureSetUp]
		protected void Setup()	{}
	}

	[TestFixture]
	class StaticFixtureSetUp
	{
		[TestFixtureSetUp]
		public static void Setup() {}
	}

	[TestFixture]
	class FixtureSetUpWithReturnValue
	{
		[TestFixtureSetUp]
		public int Setup() { return 0; }
	}

	[TestFixture]
	class FixtureSetUpWithParameters
	{
		[SetUp]
		public void Setup(int j) { }
	}

	[TestFixture]
	class PrivateFixtureTearDown
	{
		[TestFixtureTearDown]
		private void Teardown()	{}
	}

	[TestFixture]
	class ProtectedFixtureTearDown
	{
		[TestFixtureTearDown]
		protected void Teardown()	{}
	}

	[TestFixture]
	class StaticFixtureTearDown
	{
		[TestFixtureTearDown]
		public static void Teardown() {}
	}

	[TestFixture]
	class FixtureTearDownWithReturnValue
	{
		[TestFixtureTearDown]
		public int Teardown() { return 0; }
	}

	[TestFixture]
	class FixtureTearDownWithParameters
	{
		[TestFixtureTearDown]
		public void Teardown(int j) { }
	}

}
