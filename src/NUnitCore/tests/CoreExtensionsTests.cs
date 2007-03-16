// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************
using System;
using NUnit.Framework;
using NUnit.Core.Extensibility;

namespace NUnit.Core.Tests
{
	[TestFixture]
	public class CoreExtensionsTests
	{
		private CoreExtensions host;

		[SetUp]
		public void CreateHost()
		{
			host = new CoreExtensions();
		}

		[Test]
		public void HasSuiteBuildersExtensionPoint()
		{
			IExtensionPoint ep = host.GetExtensionPoint( "SuiteBuilders" );
			Assert.IsNotNull( ep );
			Assert.AreEqual( "SuiteBuilders", ep.Name );
			Assert.AreEqual( typeof( SuiteBuilderCollection ), ep.GetType() );
		}

		[Test]
		public void HasTestCaseBuildersExtensionPoint()
		{
			IExtensionPoint ep = host.GetExtensionPoint( "TestCaseBuilders" );
			Assert.IsNotNull( ep );
			Assert.AreEqual( "TestCaseBuilders", ep.Name );
			Assert.AreEqual( typeof( TestCaseBuilderCollection ), ep.GetType() );
		}

		[Test]
		public void HasTestDecoratorsExtensionPoint()
		{
			IExtensionPoint ep = host.GetExtensionPoint( "TestDecorators" );
			Assert.IsNotNull( ep );
			Assert.AreEqual( "TestDecorators", ep.Name );
			Assert.AreEqual( typeof( TestDecoratorCollection ), ep.GetType() );
		}

		[Test]
		public void HasTestFrameworkRegistry()
		{
            Assert.IsNotNull(host.FrameworkRegistry);
		}

//		[Test]
//		public void CanAddDecorator()
//		{
//			host.Install( new Decorator() );
//		}
//
//		[Test, Ignore("In Process")]
//		public void ConstructorCopiesEntries()
//		{
//			AddinHost a = new AddinHost();
//			Decorator decorator = new Decorator();
//			SuiteBuilder suiteBuilder = new SuiteBuilder();
//			TestCaseBuilder testCaseBuilder = new TestCaseBuilder();
//
//			a.Install( decorator );
////			Assert.Contains( decorator, a.TestDecorators );
//
//			a.Install( suiteBuilder );
////			Assert.Contains( suiteBuilder, a.SuiteBuilders );
//
//			a.Install( testCaseBuilder );
////			Assert.Contains( testCaseBuilder, a.TestBuilders );
//
//			AddinHost b = new AddinHost(a);
////			Assert.Contains( decorator, a.TestDecorators );
////			Assert.Contains( suiteBuilder, a.SuiteBuilders );
////			Assert.Contains( testCaseBuilder, a.TestBuilders );
//		}

		class Decorator : ITestDecorator
		{
			#region ITestDecorator Members

//			public Test Decorate(Test test, Type fixtureType)
//			{
//				// TODO:  Add Decorator.Decorate implementation
//				return null;
//			}

			public Test Decorate(Test test, System.Reflection.MemberInfo member)
			{
				// TODO:  Add Decorator.NUnit.Core.ITestDecorator.Decorate implementation
				return null;
			}

			#endregion
		}

		class SuiteBuilder : ISuiteBuilder
		{
			#region ISuiteBuilder Members

			public Test BuildFrom(Type type)
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

			public Test BuildFrom(System.Reflection.MethodInfo method)
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
