namespace Nunit.Tests
{
	using System;
	using System.Collections;
	using Microsoft.Win32;

	using Nunit.Framework;
	using Nunit.Util;

	/// <summary>
	/// Summary description for RecentAssemblyFixture.
	/// </summary>
	/// 
	[TestFixture]
	public class RecentAssemblyFixture
	{
		RecentAssemblyUtil assemblies;

		[SetUp]
		public void CreateUtil()
		{
			assemblies = new RecentAssemblyUtil("test-recent-assemblies");
		}

		[TearDown]
		public void ClearRegistry()
		{
			assemblies.Clear();
		}

		[Test]
		public void RetrieveSubKey()
		{
			Assertion.AssertNotNull(assemblies);
		}

		[Test]
		public void GetMostRecentAssembly()
		{
			string assemblyFileName = "tests.dll";
			Assertion.AssertNull("first time this should be null", assemblies.RecentAssembly);
			assemblies.RecentAssembly = assemblyFileName;
			Assertion.AssertEquals(assemblyFileName, assemblies.RecentAssembly);
		}

		[Test]
		public void GetAssemblies()
		{
			assemblies.RecentAssembly = "3";
			assemblies.RecentAssembly = "2";
			assemblies.RecentAssembly = "1";
			IList list = assemblies.GetAssemblies();
			Assertion.AssertEquals(3, list.Count);
			Assertion.AssertEquals("1", list[0]);
		}


		private void SetMockRegistryValues()
		{
			assemblies.RecentAssembly = "5";
			assemblies.RecentAssembly = "4";
			assemblies.RecentAssembly = "3";
			assemblies.RecentAssembly = "2";
			assemblies.RecentAssembly = "1";  // this is the most recent
		}

		[Test]
		public void ReorderAssemblies5()
		{
			SetMockRegistryValues();
			assemblies.RecentAssembly = "5";

			IList assemblyList = assemblies.GetAssemblies();
			Assertion.AssertEquals("5", assemblyList[0]);
			Assertion.AssertEquals("1", assemblyList[1]);
			Assertion.AssertEquals("2", assemblyList[2]);
			Assertion.AssertEquals("3", assemblyList[3]);
			Assertion.AssertEquals("4", assemblyList[4]);
		}

		[Test]
		public void ReorderAssemblies4()
		{
			SetMockRegistryValues();
			assemblies.RecentAssembly = "4";

			IList assemblyList = assemblies.GetAssemblies();
			Assertion.AssertEquals("4", assemblyList[0]);
			Assertion.AssertEquals("1", assemblyList[1]);
			Assertion.AssertEquals("2", assemblyList[2]);
			Assertion.AssertEquals("3", assemblyList[3]);
			Assertion.AssertEquals("5", assemblyList[4]);
		}

		[Test]
		public void ReorderAssembliesNew()
		{
			SetMockRegistryValues();
			assemblies.RecentAssembly = "6";

			IList assemblyList = assemblies.GetAssemblies();
			Assertion.AssertEquals("6", assemblyList[0]);
			Assertion.AssertEquals("1", assemblyList[1]);
			Assertion.AssertEquals("2", assemblyList[2]);
			Assertion.AssertEquals("3", assemblyList[3]);
			Assertion.AssertEquals("4", assemblyList[4]);
		}


		[Test]
		public void ReorderAssemblies3()
		{
			SetMockRegistryValues();
			assemblies.RecentAssembly = "3";

			IList assemblyList = assemblies.GetAssemblies();
			Assertion.AssertEquals("3", assemblyList[0]);
			Assertion.AssertEquals("1", assemblyList[1]);
			Assertion.AssertEquals("2", assemblyList[2]);
			Assertion.AssertEquals("4", assemblyList[3]);
			Assertion.AssertEquals("5", assemblyList[4]);
		}

		[Test]
		public void ReorderAssemblies2()
		{
			SetMockRegistryValues();
			assemblies.RecentAssembly = "2";

			IList assemblyList = assemblies.GetAssemblies();
			Assertion.AssertEquals("2", assemblyList[0]);
			Assertion.AssertEquals("1", assemblyList[1]);
			Assertion.AssertEquals("3", assemblyList[2]);
			Assertion.AssertEquals("4", assemblyList[3]);
			Assertion.AssertEquals("5", assemblyList[4]);
		}

		[Test]
		public void ReorderAssemblies1()
		{
			SetMockRegistryValues();
			assemblies.RecentAssembly = "1";

			IList assemblyList = assemblies.GetAssemblies();
			Assertion.AssertEquals("1", assemblyList[0]);
			Assertion.AssertEquals("2", assemblyList[1]);
			Assertion.AssertEquals("3", assemblyList[2]);
			Assertion.AssertEquals("4", assemblyList[3]);
			Assertion.AssertEquals("5", assemblyList[4]);
		}

		[Test]
		public void AddAssemblyListNotFull()
		{
			assemblies.RecentAssembly = "3";
			assemblies.RecentAssembly = "2";
			assemblies.RecentAssembly = "1";  // this is the most recent

			assemblies.RecentAssembly = "3";

			IList assemblyList = assemblies.GetAssemblies();
			Assertion.AssertEquals(3, assemblyList.Count);
			Assertion.AssertEquals("3", assemblyList[0]);
			Assertion.AssertEquals("1", assemblyList[1]);
			Assertion.AssertEquals("2", assemblyList[2]);
		}

		[Test]
		public void AddAssemblyToList()
		{
			assemblies.RecentAssembly = "1";
			assemblies.RecentAssembly = "3";

			IList assemblyList = assemblies.GetAssemblies();
			Assertion.AssertEquals(2, assemblyList.Count);
			Assertion.AssertEquals("3", assemblyList[0]);
			Assertion.AssertEquals("1", assemblyList[1]);
		}
	}
}
