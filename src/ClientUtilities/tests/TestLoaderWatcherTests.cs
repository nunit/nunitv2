// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org.
// ****************************************************************

using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Tests.Assemblies;

namespace NUnit.Util.Tests
{
	[TestFixture]
	public class TestLoaderWatcherTests
	{
		private readonly string assembly = MockAssembly.AssemblyPath;
		private MockAssemblyWatcher2 mockWatcher;
		private ITestLoader testLoader;
		private const string ReloadOnChangeSetting = "Options.TestLoader.ReloadOnChange";

		[SetUp]
		public void PreprareTestLoader()
		{
			// arrange
			mockWatcher = new MockAssemblyWatcher2();
			testLoader = new TestLoader(mockWatcher);
			testLoader.LoadProject(assembly);
		}

		[TearDown]
		public void CleanUpSettings()
		{
			Services.UserSettings.RemoveSetting(ReloadOnChangeSetting);
		}

		private void AssertWatcherIsPrepared()
		{
			Assert.IsTrue(mockWatcher.IsWatching);
			CollectionAssert.AreEquivalent(new string[] { assembly }, mockWatcher.AssembliesToWatch);
		}

		[Test]
		public void LoadShouldStartWatcher()
		{
			// act
			testLoader.LoadTest();

			// assert
			AssertWatcherIsPrepared();
		}

		[Test]
		public void ReloadShouldStartWatcher()
		{
			// arrange
			testLoader.LoadTest();
			mockWatcher.AssembliesToWatch = null;
			mockWatcher.IsWatching = false;

			// act
			testLoader.ReloadTest();

			// assert
			AssertWatcherIsPrepared();
		}

		[Test]
		public void UnloadShouldStopWatcherAndFreeResources()
		{
			// act
			testLoader.LoadTest();
			testLoader.UnloadTest();

			// assert
			Assert.IsFalse(mockWatcher.IsWatching);
			Assert.IsTrue(mockWatcher.AreResourcesFreed);
		}

		[Test]
		public void LoadShouldStartWatcherDepedningOnSettings()
		{
			// arrange
			Services.UserSettings.SaveSetting(ReloadOnChangeSetting, false);
			testLoader.LoadTest();

			// assert
			Assert.IsFalse(mockWatcher.IsWatching);
		}

		[Test]
		public void ReloadShouldStartWatcherDepedningOnSettings()
		{
			// arrange
			Services.UserSettings.SaveSetting(ReloadOnChangeSetting, false);
			testLoader.LoadTest();
			testLoader.ReloadTest();

			// assert
			Assert.IsFalse(mockWatcher.IsWatching);
		}
	}

	internal class MockAssemblyWatcher2 : IAssemblyWatcher
	{
		public bool IsWatching;
		public IList<string> AssembliesToWatch;
		public bool AreResourcesFreed;

		public void Stop()
		{
			IsWatching = false;
		}

		public void Start()
		{
			IsWatching = true;
		}

		public void Setup(int delayInMs, IList<string> assemblies)
		{
			AssembliesToWatch = assemblies;
		}

		public void Setup(int delayInMs, string assemblyFileName)
		{
			Setup(delayInMs, new string[] {assemblyFileName});
		}

		public void FreeResources()
		{
			AreResourcesFreed = true;
		}

#pragma warning disable 67
		public event AssemblyChangedHandler AssemblyChanged;
#pragma warning restore 67
	}
}