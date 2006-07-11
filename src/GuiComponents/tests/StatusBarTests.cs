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
using NUnit.Framework;
using NUnit.Core;
using NUnit.Util;
using NUnit.Tests.Assemblies;
using NUnit.TestUtilities;

namespace NUnit.UiKit.Tests
{
	/// <summary>
	/// Summary description for StatusBarTests.
	/// </summary>
	[TestFixture]
	public class StatusBarTests
	{
		private StatusBar statusBar;
		private MockTestEventSource mockEvents;
		private string testsDll = "mock-assembly.dll";
		TestNode suite;
		int testCount;

		[SetUp]
		public void Setup()
		{
			statusBar = new StatusBar();

			TestSuiteBuilder builder = new TestSuiteBuilder();
			suite = new TestNode( builder.Build( testsDll ) );

			mockEvents = new MockTestEventSource( suite );
		}

		[Test]
		public void TestConstruction()
		{
			Assert.AreEqual( "Status", statusBar.Panels[0].Text );
			Assert.AreEqual( "Test Cases : 0", statusBar.Panels[1].Text );
			Assert.AreEqual( "", statusBar.Panels[2].Text );
			Assert.AreEqual( "", statusBar.Panels[3].Text );
			Assert.AreEqual( "", statusBar.Panels[4].Text );
		}

		[Test]
		public void TestInitialization()
		{
			statusBar.Initialize( 0 );
			Assert.AreEqual( "", statusBar.Panels[0].Text );
			Assert.AreEqual( "Test Cases : 0", statusBar.Panels[1].Text );
			Assert.AreEqual( "", statusBar.Panels[2].Text );
			Assert.AreEqual( "", statusBar.Panels[3].Text );
			Assert.AreEqual( "", statusBar.Panels[4].Text );

			statusBar.Initialize( 50 );
			Assert.AreEqual( "Ready", statusBar.Panels[0].Text );
			Assert.AreEqual( "Test Cases : 50", statusBar.Panels[1].Text );
			Assert.AreEqual( "", statusBar.Panels[2].Text );
			Assert.AreEqual( "", statusBar.Panels[3].Text );
			Assert.AreEqual( "", statusBar.Panels[4].Text );
		}

		[Test]
		public void TestFinalDisplay()
		{
			Assert.AreEqual( false, statusBar.DisplayTestProgress );
			statusBar.Subscribe( mockEvents );

			mockEvents.SimulateTestRun();
			Assert.AreEqual( "Completed", statusBar.Panels[0].Text );
			Assert.AreEqual( 
				PanelMessage( "Test Cases", MockAssembly.Tests ), 
				statusBar.Panels[1].Text );
			Assert.AreEqual( 
				PanelMessage( "Tests Run", MockAssembly.Tests - MockAssembly.NotRun ),
				statusBar.Panels[2].Text );
			Assert.AreEqual( "Failures : 0", statusBar.Panels[3].Text );
			Assert.AreEqual( "Time : 0", statusBar.Panels[4].Text );
		}

        // .NET 1.0 sometimes throws:
        // ExternalException : A generic error occurred in GDI+.
        [Test, Platform(Exclude = "Net-1.0")]
        public void TestProgressDisplay()
		{
			statusBar.DisplayTestProgress = true;
			statusBar.Subscribe( mockEvents );

			testCount = 0;
			mockEvents.TestFinished += new TestEventHandler( OnTestFinished );

			mockEvents.SimulateTestRun();
			Assert.AreEqual( "Completed", statusBar.Panels[0].Text );
			Assert.AreEqual( 
				PanelMessage( "Test Cases", MockAssembly.Tests ), 
				statusBar.Panels[1].Text );
			Assert.AreEqual( 
				PanelMessage( "Tests Run", MockAssembly.Tests - MockAssembly.NotRun ),
				statusBar.Panels[2].Text );
			Assert.AreEqual( "Failures : 0", statusBar.Panels[3].Text );
			Assert.AreEqual( "Time : 0", statusBar.Panels[4].Text );
		}

		private void OnTestFinished( object sender, TestEventArgs e )
		{
			Assert.AreEqual( 
				PanelMessage( "Test Cases", MockAssembly.Tests ),
				statusBar.Panels[1].Text );
			Assert.AreEqual( "Failures : 0", statusBar.Panels[3].Text );
			Assert.AreEqual( "Time : 0", statusBar.Panels[4].Text );

			// Note: Assumes delegates are called in order of adding
			switch( ++testCount )
			{
				case 1:
					CheckTestDisplay( "NUnit.Tests.Assemblies.MockTestFixture.ExplicitlyRunTest", e, 0 );
					break;
				case 2:
					CheckTestDisplay( "NUnit.Tests.Assemblies.MockTestFixture.MockTest1", e, 1 );
					break;
				case 3:
					CheckTestDisplay( "NUnit.Tests.Assemblies.MockTestFixture.MockTest2", e, 2 );
					break;
				case 4:
					CheckTestDisplay( "NUnit.Tests.Assemblies.MockTestFixture.MockTest3", e, 3 );
					break;
				case 5:
					CheckTestDisplay( "NUnit.Tests.Assemblies.MockTestFixture.MockTest4", e, 3 );
					break;
				case 6:
					CheckTestDisplay( "NUnit.Tests.Assemblies.MockTestFixture.MockTest5", e, 3 );
					break;
				case 7:
					CheckTestDisplay( "NUnit.Tests.IgnoredFixture.Test1", e, 3 );
					break;
				case 8:
					CheckTestDisplay( "NUnit.Tests.IgnoredFixture.Test2", e, 3 );
					break;
				case 9:
					CheckTestDisplay( "NUnit.Tests.IgnoredFixture.Test3", e, 3 );
					break;
				case 10:
					CheckTestDisplay( "NUnit.Tests.Singletons.OneTestCase.TestCase", e, 4 );
					break;
				case 11:			
					CheckTestDisplay( "NUnit.Tests.TestAssembly.MockTestFixture.MyTest", e, 5 );
					break;
			}
		}

		private void CheckTestDisplay( string expected, TestEventArgs e, int testsRun )
		{
			Assert.AreEqual( expected, e.Result.Test.FullName );
			int index = expected.LastIndexOf( '.' ) + 1;
			StringAssert.EndsWith( expected.Substring( index ), statusBar.Panels[0].Text );
			if ( testsRun > 0 )
				Assert.AreEqual( PanelMessage( "Tests Run", testsRun ), statusBar.Panels[2].Text );
			else
				Assert.AreEqual( "", statusBar.Panels[2].Text );
		}

		private static string PanelMessage( string text, int count )
		{
			return string.Format( "{0} : {1}", text, count );
		}
	}
}
