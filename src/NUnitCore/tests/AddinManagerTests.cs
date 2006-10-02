using System;
using NUnit.Framework;

namespace NUnit.Core.Tests
{
	[TestFixture]
	public class AddinManagerTests
	{
		[Test]
		public void ConstructorChainsPriorState()
		{
			AddinManager a = new AddinManager();
			AddinManager b = new AddinManager(a);
			AddinManager c = new AddinManager(b);

			Assert.AreSame( a, b.PriorState );
			Assert.AreSame( b, c.PriorState );
		}

		[Test]
		public void ConstructorCopiesEntries()
		{
			AddinManager a = new AddinManager();
			Decorator decorator = new Decorator();
			SuiteBuilder suiteBuilder = new SuiteBuilder();
			TestCaseBuilder testCaseBuilder = new TestCaseBuilder();
			a.Register( decorator );
			a.Register( suiteBuilder );
			a.Register( testCaseBuilder );
			CollectionAssert.AreEquivalent( new string[] { "Decorator", "SuiteBuilder", "TestCaseBuilder" }, a.Names );
			AddinManager b = new AddinManager(a);
			CollectionAssert.AreEquivalent( new string[] { "Decorator", "SuiteBuilder", "TestCaseBuilder" }, b.Names );
		}

		class Decorator : ITestDecorator
		{
			#region ITestDecorator Members

			public TestSuite Decorate(TestSuite suite, Type fixtureType)
			{
				// TODO:  Add Decorator.Decorate implementation
				return null;
			}

			TestCase NUnit.Core.ITestDecorator.Decorate(TestCase testCase, System.Reflection.MethodInfo method)
			{
				// TODO:  Add Decorator.NUnit.Core.ITestDecorator.Decorate implementation
				return null;
			}

			#endregion
		}

		class SuiteBuilder : ISuiteBuilder
		{
			#region ISuiteBuilder Members

			public TestSuite BuildFrom(Type type)
			{
				// TODO:  Add SuiteBuilder.BuildFrom implementation
				return null;
			}

			public bool CanBuildFrom(Type type)
			{
				// TODO:  Add SuiteBuilder.CanBuildFrom implementation
				return false;
			}

			#endregion
		}

		class TestCaseBuilder : ITestCaseBuilder
		{
			#region ITestCaseBuilder Members

			public TestCase BuildFrom(System.Reflection.MethodInfo method)
			{
				// TODO:  Add TestCaseBuilder.BuildFrom implementation
				return null;
			}

			public bool CanBuildFrom(System.Reflection.MethodInfo method)
			{
				// TODO:  Add TestCaseBuilder.CanBuildFrom implementation
				return false;
			}

			#endregion
		}

	}
}
