using System;
using System.IO;
using NUnit.Core;
using NUnit.Util;
using NUnit.Framework;

namespace NUnit.Tests
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
		}

		[Test]
		public void LoadProject()
		{
			loader.LoadProject( assembly );
			Assert.True( loader.IsProjectLoaded );
			Assert.True( loader.TestProject.IsWrapper );
			Assert.False( loader.IsTestLoaded );
			Assert.Equals( 2, catcher.Events.Count );
			Assert.Equals( TestAction.ProjectLoading, catcher.Events[0].Action );
			Assert.Equals( TestAction.ProjectLoaded, catcher.Events[1].Action );
		}

		[Test]
		public void UnloadProject()
		{
			loader.LoadProject( assembly );
			loader.UnloadProject();
			Assert.False( loader.IsProjectLoaded );
			Assert.False( loader.IsTestLoaded );
			Assert.Equals( 4, catcher.Events.Count );
			Assert.Equals( TestAction.ProjectUnloading, catcher.Events[2].Action );
			Assert.Equals( TestAction.ProjectUnloaded, catcher.Events[3].Action );
		}

		[Test]
		public void LoadTest()
		{
			loader.LoadTest( assembly );
			Assert.True( loader.IsProjectLoaded );
			Assert.True( loader.TestProject.IsWrapper );
			Assert.True( loader.IsTestLoaded );
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
			Assert.True( loader.IsProjectLoaded );
			Assert.True( loader.TestProject.IsWrapper );
			Assert.False( loader.IsTestLoaded );
			Assert.Equals( 4, catcher.Events.Count );
			Assert.Equals( TestAction.TestLoadFailed, catcher.Events[3].Action );
			Assert.Equals( typeof( FileNotFoundException ), catcher.Events[3].Exception.GetType() );
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
			Assert.True( loader.IsProjectLoaded );
			Assert.True( loader.TestProject.IsWrapper );
			Assert.False( loader.IsTestLoaded );
			Assert.Equals( 4, catcher.Events.Count );
			Assert.Equals( TestAction.TestLoadFailed, catcher.Events[3].Action );
			Assert.Equals( typeof( BadImageFormatException ), catcher.Events[3].Exception.GetType() );
		}

		[Test]
		public void AssemblyWithNoTests()
		{
			loader.LoadTest( "nunit.util.dll" );
			Assert.True( loader.IsProjectLoaded );
			Assert.True( loader.TestProject.IsWrapper );
			Assert.False( loader.IsTestLoaded );
			Assert.Equals( 4, catcher.Events.Count );
			Assert.Equals( TestAction.TestLoadFailed, catcher.Events[3].Action );
			Assert.Equals( typeof( NoTestFixturesException), catcher.Events[3].Exception.GetType() );
		}

		// TODO: Should wrapper project be unloaded on failure?
	}
}
