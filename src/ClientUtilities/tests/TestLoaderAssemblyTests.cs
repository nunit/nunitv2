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
using NUnit.Framework;
using NUnit.Tests.Assemblies;

namespace NUnit.Util.Tests
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

		private void LoadTest( string assembly )
		{
			loader.LoadProject( assembly );
			if (loader.IsProjectLoaded && loader.TestProject.IsLoadable)
				loader.LoadTest();
		}
		
		[SetUp]
		public void SetUp()
		{
			loader = new TestLoader( );
			loader.MergeAssemblies = false;
			loader.AutoNamespaceSuites = true;
			loader.MultiDomain = false;

			catcher = new TestEventCatcher( loader.Events );
		}

		[TearDown]
		public void TearDown()
		{
			if ( loader.IsTestLoaded )
				loader.UnloadTest();

			if ( loader.IsProjectLoaded )
				loader.UnloadProject();
		}

		[Test]
		public void LoadProject()
		{
			loader.LoadProject( assembly );
			Assert.IsTrue(loader.IsProjectLoaded,  "Project not loaded");
			Assert.IsFalse(loader.IsTestLoaded,  "Test should not be loaded");
			Assert.AreEqual( 2, catcher.Events.Count );
			Assert.AreEqual( TestAction.ProjectLoading, ((TestEventArgs)catcher.Events[0]).Action );
			Assert.AreEqual( TestAction.ProjectLoaded, ((TestEventArgs)catcher.Events[1]).Action );
		}

		[Test]
		public void UnloadProject()
		{
			loader.LoadProject( assembly );
			loader.UnloadProject();
			Assert.IsFalse( loader.IsProjectLoaded, "Project not unloaded" );
			Assert.IsFalse( loader.IsTestLoaded, "Test not unloaded" );
			Assert.AreEqual( 4, catcher.Events.Count );
			Assert.AreEqual( TestAction.ProjectUnloading, ((TestEventArgs)catcher.Events[2]).Action );
			Assert.AreEqual( TestAction.ProjectUnloaded, ((TestEventArgs)catcher.Events[3]).Action );
		}

		[Test]
		public void LoadTest()
		{
			LoadTest( assembly );
			Assert.IsTrue( loader.IsProjectLoaded, "Project not loaded" );
			Assert.IsTrue( loader.IsTestLoaded, "Test not loaded" );
			Assert.AreEqual( 4, catcher.Events.Count );
			Assert.AreEqual( TestAction.TestLoading, ((TestEventArgs)catcher.Events[2]).Action );
			Assert.AreEqual( TestAction.TestLoaded, ((TestEventArgs)catcher.Events[3]).Action );
			Assert.AreEqual( MockAssembly.Tests, ((TestEventArgs)catcher.Events[3]).TestCount );
		}

		[Test]
		public void UnloadTest()
		{
			LoadTest( assembly );
			loader.UnloadTest();
			Assert.AreEqual( 6, catcher.Events.Count );
			Assert.AreEqual( TestAction.TestUnloading, ((TestEventArgs)catcher.Events[4]).Action );
			Assert.AreEqual( TestAction.TestUnloaded, ((TestEventArgs)catcher.Events[5]).Action );
		}

		[Test]
		public void FileNotFound()
		{
			LoadTest( "xxxxx" );
			Assert.IsFalse( loader.IsProjectLoaded, "Project should not load" );
			Assert.IsFalse( loader.IsTestLoaded, "Test should not load" );
			Assert.AreEqual( 2, catcher.Events.Count );
			Assert.AreEqual( TestAction.ProjectLoadFailed, ((TestEventArgs)catcher.Events[1]).Action );
			Assert.AreEqual( typeof( FileNotFoundException ), ((TestEventArgs)catcher.Events[1]).Exception.GetType() );
		}

		// Doesn't work under .NET 2.0 Beta 2
		//[Test]
		public void InvalidAssembly()
		{
			FileInfo file = new FileInfo(badFile);
			try
			{
				StreamWriter sw = file.AppendText();
				sw.WriteLine("This is a new entry to add to the file");
				sw.WriteLine("This is yet another line to add...");
				sw.Flush();
				sw.Close();

				LoadTest(badFile);
				Assert.IsTrue(loader.IsProjectLoaded, "Project not loaded");
				Assert.IsFalse(loader.IsTestLoaded, "Test should not be loaded");
				Assert.AreEqual(4, catcher.Events.Count);
				Assert.AreEqual(TestAction.TestLoadFailed, ((TestEventArgs)catcher.Events[3]).Action);
				Assert.AreEqual(typeof(BadImageFormatException), ((TestEventArgs)catcher.Events[3]).Exception.GetType());
			}
			finally
			{
				if ( file.Exists )
				    file.Delete();
			}
		}

		[Test]
		public void AssemblyWithNoTests()
		{
			LoadTest( "notestfixtures-assembly.dll" );
			Assert.IsTrue( loader.IsProjectLoaded, "Project not loaded" );
			Assert.IsTrue( loader.IsTestLoaded, "Test should be loaded" );
			Assert.AreEqual( 4, catcher.Events.Count );
			Assert.AreEqual( TestAction.TestLoaded, ((TestEventArgs)catcher.Events[3]).Action );
		}

		// TODO: Should wrapper project be unloaded on failure?

		[Test]
		public void RunTest()
		{
			loader.ReloadOnRun = false;
			
			LoadTest( assembly );
			loader.RunTests();
			do 
			{
				// TODO: Find a more robust way of handling this
				Thread.Sleep( 500 );
			}
			while( loader.Running );
			
			Assert.AreEqual( 46, catcher.Events.Count );
			Assert.AreEqual( TestAction.RunStarting, ((TestEventArgs)catcher.Events[4]).Action );
			Assert.AreEqual( TestAction.RunFinished, ((TestEventArgs)catcher.Events[45]).Action );

			int nTests = 0;
			int nRun = 0;
			foreach( object o in catcher.Events )
			{
				TestEventArgs e = o as TestEventArgs;

				if ( e != null && e.Action == TestAction.TestFinished )
				{
					++nTests;
					if ( e.Result.Executed )
						++nRun;
				}
			}
			Assert.AreEqual( MockAssembly.Tests - MockAssembly.Explicit, nTests );
			Assert.AreEqual( MockAssembly.Tests - MockAssembly.NotRun, nRun );
		}
	}
}
