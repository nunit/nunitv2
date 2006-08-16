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

namespace NUnit.UiKit.Tests
{
	using System;
	using System.Drawing;
	using NUnit.Framework;
	using NUnit.Core;
	using NUnit.Util;
	using NUnit.Tests.Assemblies;
	using NUnit.TestUtilities;

	/// <summary>
	/// Summary description for ProgressBarTests.
	/// </summary>
	[TestFixture]
	public class ProgressBarTests
	{
		private TestProgressBar progressBar;
		private MockTestEventSource mockEvents;
		private string testsDll = "mock-assembly.dll";
		private TestNode suite;
		int testCount;

		[SetUp]
		public void Setup()
		{
			progressBar = new TestProgressBar();

			TestSuiteBuilder builder = new TestSuiteBuilder();
			suite = new TestNode( builder.Build( testsDll ) );

			mockEvents = new MockTestEventSource( suite );
		}

        // .NET 1.0 sometimes throws:
        // ExternalException : A generic error occurred in GDI+.
        [Test, Platform(Exclude = "Net-1.0")]
        public void TestProgressDisplay()
		{
			progressBar.Subscribe( mockEvents );
			mockEvents.TestFinished += new TestEventHandler( OnTestFinished );

			testCount = 0;
			mockEvents.SimulateTestRun();
			
			Assert.AreEqual( 0, progressBar.Minimum );
			Assert.AreEqual( MockAssembly.Tests, progressBar.Maximum );
			Assert.AreEqual( 1, progressBar.Step );
			Assert.AreEqual( MockAssembly.Tests, progressBar.Value );
			Assert.AreEqual( Color.Yellow, progressBar.ForeColor );
		}

		private void OnTestFinished( object sender, TestEventArgs e )
		{
			++testCount;
			// Assumes delegates are called in order of adding
			Assert.AreEqual( testCount, progressBar.Value );
		}
	}
}
