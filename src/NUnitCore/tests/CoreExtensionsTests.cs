// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************
using System;
using NUnit.Framework;
using NUnit.Core.Extensibility;
using NUnit.Mocks;

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
		public void HasEventListenerExtensionPoint()
		{
			IExtensionPoint ep = host.GetExtensionPoint( "EventListeners" );
			Assert.IsNotNull( ep );
			Assert.AreEqual( "EventListeners", ep.Name );
			Assert.AreEqual( typeof( EventListenerCollection ), ep.GetType() );
		}

		[Test]
		public void HasTestFrameworkRegistry()
		{
			IExtensionPoint ep = host.GetExtensionPoint( "FrameworkRegistry" );
			Assert.IsNotNull( ep );
			Assert.AreEqual( "FrameworkRegistry", ep.Name );
			Assert.AreEqual( typeof( FrameworkRegistry ), ep.GetType() );
		}

		[Test]
		public void CanAddDecorator()
		{
			DynamicMock mock = new DynamicMock( typeof(ITestDecorator) );
			mock.Expect( "Decorate" );
			
			IExtensionPoint ep = host.GetExtensionPoint("TestDecorators");
			ep.Install( mock.MockInstance );
			host.TestDecorators.Decorate( null, null );

			mock.Verify();
		}

		[Test]
		public void CanAddSuiteBuilder()
		{
			DynamicMock mock = new DynamicMock( typeof(ISuiteBuilder) );
			mock.ExpectAndReturn( "CanBuildFrom", true, null );
			mock.Expect( "BuildFrom" );
			
			IExtensionPoint ep = host.GetExtensionPoint("SuiteBuilders");
			ep.Install( mock.MockInstance );
			host.SuiteBuilders.BuildFrom( null );

			mock.Verify();
		}

		[Test]
		public void CanAddTestCaseBuilder()
		{
			DynamicMock mock = new DynamicMock( typeof(ITestCaseBuilder) );
			mock.ExpectAndReturn( "CanBuildFrom", true, null );
			mock.Expect( "BuildFrom" );
			
			IExtensionPoint ep = host.GetExtensionPoint("TestCaseBuilders");
			ep.Install( mock.MockInstance );
			host.TestBuilders.BuildFrom( null );

			mock.Verify();
		}

		[Test]
		public void CanAddEventListener()
		{
			DynamicMock mock = new DynamicMock( typeof(EventListener) );
			mock.Expect( "RunStarted" );
			mock.Expect( "RunFinished" );

			IExtensionPoint ep = host.GetExtensionPoint("EventListeners");
			ep.Install( mock.MockInstance );
			host.Listeners.RunStarted( "test", 0 );
			host.Listeners.RunFinished( new TestSuiteResult( new TestInfo( new TestSuite( "test" ) ) ) );

			mock.Verify();
		}
	}
}
