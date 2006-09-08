#region Copyright (c) 2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright  2000-2002 Philip A. Craig
'
' This software is provided 'as-is', without any express or implied warranty. In no 
' event will the authors be held liable for any damages arising from the use of this 
' software.
' 
' Permission is granted to anyone to use this software for any purpose, including 
' commercial applications, and to alter it and redistribute it freely, subject to the 
' following restrictions:
'
' 1. The origin of this software must not be misrepresented; you must not claim that 
' you wrote the original software. If you use this software in a product, an 
' acknowledgment (see the following) in the product documentation is required.
'
' Portions Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright  2000-2002 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

using System;
using System.Collections;
using NUnit.Core;
using NUnit.Framework;

namespace NUnit.Util.Tests
{
	[TestFixture]
	public class EventDispatcherTests
	{
		private TestEventDispatcher dispatcher;
		private TestEventCatcher catcher;
		private TestInfo test;
		private TestResult result;
		private Exception exception;

		private readonly string FILENAME = "MyTestFileName";
		private readonly string TESTNAME = "MyTestName";
		private readonly string MESSAGE = "My message!";
		private readonly string RSLTNAME = "MyResult";

		[SetUp]
		public void SetUp()
		{
			dispatcher = new TestEventDispatcher();
			catcher = new TestEventCatcher( dispatcher );
			test = new TestInfo( new TestSuite( TESTNAME ) );
			result = new TestSuiteResult( test, RSLTNAME );
			exception = new Exception( MESSAGE );
		}

		[Test]
		public void ProjectLoading()
		{
			dispatcher.FireProjectLoading( FILENAME );
			CheckEvent( TestAction.ProjectLoading, FILENAME );
		}

		[Test]
		public void ProjectLoaded()
		{
			dispatcher.FireProjectLoaded( FILENAME );
			CheckEvent( TestAction.ProjectLoaded, FILENAME );
		}

		[Test]
		public void ProjectLoadFailed()
		{
			dispatcher.FireProjectLoadFailed( FILENAME, exception );
			CheckEvent( TestAction.ProjectLoadFailed, FILENAME, exception );
		}

		[Test]
		public void ProjectUnloading()
		{
			dispatcher.FireProjectUnloading( FILENAME );
			CheckEvent( TestAction.ProjectUnloading, FILENAME );
		}

		[Test]
		public void ProjectUnloaded()
		{
			dispatcher.FireProjectUnloaded( FILENAME );
			CheckEvent( TestAction.ProjectUnloaded, FILENAME );
		}

		[Test]
		public void ProjectUnloadFailed()
		{
			dispatcher.FireProjectUnloadFailed( FILENAME, exception );
			CheckEvent( TestAction.ProjectUnloadFailed, FILENAME, exception );
		}

		[Test]
		public void TestLoading()
		{
			dispatcher.FireTestLoading( FILENAME );
			CheckEvent( TestAction.TestLoading, FILENAME );
		}

		[Test]
		public void TestLoaded()
		{
			dispatcher.FireTestLoaded( FILENAME, test );
			CheckEvent( TestAction.TestLoaded, FILENAME, test );
		}

		[Test]
		public void TestLoadFailed()
		{
			dispatcher.FireTestLoadFailed( FILENAME, exception );
			CheckEvent( TestAction.TestLoadFailed, FILENAME, exception );
		}

		[Test]
		public void TestUnloading()
		{
			dispatcher.FireTestUnloading( FILENAME, test );
			CheckEvent( TestAction.TestUnloading, FILENAME );
		}

		[Test]
		public void TestUnloaded()
		{
			dispatcher.FireTestUnloaded( FILENAME, test );
			CheckEvent( TestAction.TestUnloaded, FILENAME, test );
		}

		[Test]
		public void TestUnloadFailed()
		{
			dispatcher.FireTestUnloadFailed( FILENAME, exception );
			CheckEvent( TestAction.TestUnloadFailed, FILENAME, exception );
		}

		[Test]
		public void TestReloading()
		{
			dispatcher.FireTestReloading( FILENAME, test );
			CheckEvent( TestAction.TestReloading, FILENAME );
		}

		[Test]
		public void TestReloaded()
		{
			dispatcher.FireTestReloaded( FILENAME, test );
			CheckEvent( TestAction.TestReloaded, FILENAME, test );
		}

		[Test]
		public void TestReloadFailed()
		{
			dispatcher.FireTestReloadFailed( FILENAME, exception );
			CheckEvent( TestAction.TestReloadFailed, FILENAME, exception );
		}

		[Test]
		public void RunStarting()
		{
			dispatcher.FireRunStarting( test.FullName, test.TestCount );
			CheckEvent( TestAction.RunStarting, test.FullName, test.TestCount );
		}

		[Test]
		public void RunFinished()
		{
			dispatcher.FireRunFinished( result );
			CheckEvent( TestAction.RunFinished, result );
		}

		[Test]
		public void RunFailed()
		{
			dispatcher.FireRunFinished( exception );
			CheckEvent( TestAction.RunFinished, exception );
		}

		[Test]
		public void SuiteStarting()
		{
			dispatcher.FireSuiteStarting( test );
			CheckEvent( TestAction.SuiteStarting, test );
		}

		[Test]
		public void SuiteFinished()
		{
			dispatcher.FireSuiteFinished( result );
			CheckEvent( TestAction.SuiteFinished, result );
		}

		[Test]
		public void TestStarting()
		{
			dispatcher.FireTestStarting( test );
			CheckEvent( TestAction.TestStarting, test );
		}

		[Test]
		public void TestFinished()
		{
			dispatcher.FireTestFinished( result );
			CheckEvent( TestAction.TestFinished, result );
		}

		private void CheckEvent( TestAction action )
		{
			Assert.AreEqual( 1, catcher.Events.Count );
			Assert.AreEqual( action, ((TestEventArgs)catcher.Events[0]).Action );
		}

		private void CheckEvent( TestAction action, string fileName )
		{
			CheckEvent( action );
			Assert.AreEqual( fileName, ((TestEventArgs)catcher.Events[0]).Name );
		}

		private void CheckEvent( TestAction action, string fileName, int testCount )
		{
			CheckEvent( action, fileName );
			Assert.AreEqual( testCount, ((TestEventArgs)catcher.Events[0]).TestCount );
		}

		private void CheckEvent( TestAction action, string fileName, TestInfo test )
		{
			CheckEvent( action, fileName );
			Assert.AreEqual( TESTNAME, ((TestEventArgs)catcher.Events[0]).Test.Name );
		}

		private void CheckEvent( TestAction action, string fileName, Exception exception )
		{
			CheckEvent( action, fileName );
			Assert.AreEqual( MESSAGE, ((TestEventArgs)catcher.Events[0]).Exception.Message );
		}

		private void CheckEvent( TestAction action, TestInfo test )
		{
			CheckEvent( action );
			Assert.AreEqual( TESTNAME, ((TestEventArgs)catcher.Events[0]).Test.Name );
		}

		private void CheckEvent( TestAction action, TestResult result )
		{
			CheckEvent( action );
			Assert.AreEqual( RSLTNAME, result.Name );
		}

		private void CheckEvent( TestAction action, Exception exception )
		{
			CheckEvent( TestAction.RunFinished );
			Assert.AreEqual( MESSAGE, ((TestEventArgs)catcher.Events[0]).Exception.Message );
		}
	}
}
