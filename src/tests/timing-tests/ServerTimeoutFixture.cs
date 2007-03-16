// ****************************************************************
// Copyright 2002-2003, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using System.IO;
using System.Threading;
using NUnit.Core;
using NUnit.Framework;
using NUnit.Util;
using NUnit.Tests.Assemblies;

namespace NUnit.Tests.TimingTests
{
// This no longer makes sense, since Load no
// longer returns a reference to the Tests
//	[TestFixture]
//	public class ServerTimeoutFixture
//	{
//		private TestDomain domain; 
//		private TestNode test;
//
//		// Test using timeout greater than default of five minutes
//		private readonly TimeSpan timeout = TimeSpan.FromMinutes( 6 );
//
//		[SetUp]
//		public void MakeAppDomain()
//		{
//			domain = new TestDomain();
//			test = domain.Load("mock-assembly.dll");
//		}
//
//		[TearDown]
//		public void UnloadTestDomain()
//		{
//			domain.Unload();
//			domain = null;
//		}
//			
//		[Test]
//		public void ServerTimeoutTest()
//		{
//			// Delay after loading the test
//			Thread.Sleep( timeout );
//
//			// Copy all the tests from the remote domain
//			// to verify that Test object is connected.
//			TestNode node = new TestNode( test, true );
//			
//			
//			// Run the tests, which also verifies that
//			// RemoteTestRunner has not been disconnected
//			TestResult result = domain.Run( NullListener.NULL );
//
//			// Delay again to let the results "ripen"
//			Thread.Sleep( timeout );
//
//			// Visit the results of the test after another delay
//			ResultSummarizer summarizer = new ResultSummarizer(result);
//			Assert.AreEqual(MockAssembly.Tests - MockAssembly.NotRun, summarizer.ResultCount);
//			Assert.AreEqual(MockAssembly.NotRun, summarizer.TestsNotRun);
//
//			// Make sure we can still access the tests
//			// using the Test property of the result
//			node = new TestNode( result.Test, true );
//		}
//	}
}
