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
using System.IO;
using System.Threading;
using NUnit.Core;
using NUnit.Util;
using NUnit.Framework;

namespace NUnit.Tests.Util
{
	/// <summary>
	/// 
	/// </summary>
	[TestFixture]
	public class TestLoaderAssemblyTests
	{
		private readonly string assembly = "mock-assembly.dll";
		private readonly string badFile = "x.dll";

		private TestLoader loader;
		private TestEventCatcher catcher;
		
		[SetUp]
		public void SetUp()
		{
			NUnitRegistry.TestMode = true;
			NUnitRegistry.ClearTestKeys();

			loader = new TestLoader( Console.Out, Console.Error );
			catcher = new TestEventCatcher( loader.Events );
		}

		[TearDown]
		public void TearDown()
		{
			if ( loader.IsTestLoaded )
				loader.UnloadTest();

			if ( loader.IsProjectLoaded )
				loader.UnloadProject();

			FileInfo file = new FileInfo( badFile );
			if ( file.Exists )
				file.Delete();

			NUnitRegistry.TestMode = true;
		}

		[Test]
		public void LoadProject()
		{
			loader.LoadProject( assembly );
			Assert.True(loader.IsProjectLoaded,  "Project not loaded");
			Assert.False(loader.IsTestLoaded,  "Test should not be loaded");
			Assert.Equals( 2, catcher.Events.Count );
			Assert.Equals( TestAction.ProjectLoading, catcher.Events[0].Action );
			Assert.Equals( TestAction.ProjectLoaded, catcher.Events[1].Action );
		}

		[Test]
		public void UnloadProject()
		{
			loader.LoadProject( assembly );
			loader.UnloadProject();
			Assert.False( loader.IsProjectLoaded, "Project not unloaded" );
			Assert.False( loader.IsTestLoaded, "Test not unloaded" );
			Assert.Equals( 4, catcher.Events.Count );
			Assert.Equals( TestAction.ProjectUnloading, catcher.Events[2].Action );
			Assert.Equals( TestAction.ProjectUnloaded, catcher.Events[3].Action );
		}

		[Test]
		public void LoadTest()
		{
			loader.LoadTest( assembly );
			Assert.True( loader.IsProjectLoaded, "Project not loaded" );
			Assert.True( loader.IsTestLoaded, "Test not loaded" );
			Assert.Equals( 4, catcher.Events.Count );
			Assert.Equals( TestAction.TestLoading, catcher.Events[2].Action );
			Assert.Equals( TestAction.TestLoaded, catcher.Events[3].Action );
			Assert.Equals( 7, catcher.Events[3].Test.CountTestCases );
		}

		[Test]
		public void UnloadTest()
		{
			loader.LoadTest( assembly );
			loader.UnloadTest();
			Assert.Equals( 6, catcher.Events.Count );
			Assert.Equals( TestAction.TestUnloading, catcher.Events[4].Action );
			Assert.Equals( TestAction.TestUnloaded, catcher.Events[5].Action );
		}

		[Test]
		public void FileNotFound()
		{
			loader.LoadTest( "xxxxx" );
			Assert.False( loader.IsProjectLoaded, "Project should not load" );
			Assert.False( loader.IsTestLoaded, "Test should not load" );
			Assert.Equals( 2, catcher.Events.Count );
			Assert.Equals( TestAction.ProjectLoadFailed, catcher.Events[1].Action );
			Assert.Equals( typeof( FileNotFoundException ), catcher.Events[1].Exception.GetType() );
		}

		[Test]
		public void InvalidAssembly()
		{
			FileInfo file = new FileInfo( badFile );

			StreamWriter sw = file.AppendText();

			sw.WriteLine("This is a new entry to add to the file");
			sw.WriteLine("This is yet another line to add...");
			sw.Flush();
			sw.Close();

			loader.LoadTest( badFile );
			Assert.True( loader.IsProjectLoaded, "Project not loaded" );
			Assert.False( loader.IsTestLoaded, "Test should not be loaded" );
			Assert.Equals( 4, catcher.Events.Count );
			Assert.Equals( TestAction.TestLoadFailed, catcher.Events[3].Action );
			Assert.Equals( typeof( BadImageFormatException ), catcher.Events[3].Exception.GetType() );
		}

		[Test]
		public void AssemblyWithNoTests()
		{
			loader.LoadTest( "nunit.util.dll" );
			Assert.True( loader.IsProjectLoaded, "Project not loaded" );
			Assert.True( loader.IsTestLoaded, "Test should be loaded" );
			Assert.Equals( 4, catcher.Events.Count );
			Assert.Equals( TestAction.TestLoaded, catcher.Events[3].Action );
		}

		// TODO: Should wrapper project be unloaded on failure?

		[Test]
		public void RunTest()
		{
			UserSettings.Options.ReloadOnRun = false;
			
			loader.LoadTest( assembly );
			loader.RunTestSuite( catcher.Events[3].Test );
			while( loader.IsTestRunning )
				Thread.Sleep( 500 );
			
			Assert.Equals( 38, catcher.Events.Count );
			Assert.Equals( TestAction.RunStarting, catcher.Events[4].Action );
			Assert.Equals( TestAction.RunFinished, catcher.Events[37].Action );

			int nTests = 0;
			int nRun = 0;
			foreach( TestEventArgs e in catcher.Events )
			{
				if ( e.Action == TestAction.TestFinished )
				{
					++nTests;
					if ( e.Result.Executed )
						++nRun;
				}
			}
			Assert.Equals( 7, nTests );
			Assert.Equals( 5, nRun );
		}
	}
}
