#region Copyright (c) 2002, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov
' Copyright © 2000-2002 Philip A. Craig
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
' Portions Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov 
' or Copyright © 2000-2002 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

namespace NUnit.Tests
{
	using System;
	using System.Collections;
	using Microsoft.Win32;

	using NUnit.Framework;
	using NUnit.Util;

	/// <summary>
	/// Summary description for RecentAssemblyFixture.
	/// </summary>
	/// 
	[TestFixture]
	public class RecentAssemblyFixture
	{
//		RecentAssemblyUtil assemblies;
		RecentAssemblySettings assemblies;

		[SetUp]
		public void CreateUtil()
		{
//			assemblies = new RecentAssemblyUtil("test-recent-assemblies");
			NUnitRegistry.TestMode = true;
			NUnitRegistry.ClearTestKeys();
			assemblies = UserSettings.RecentAssemblies;
		}

		[TearDown]
		public void ClearRegistry()
		{
//			assemblies.Clear();
			NUnitRegistry.TestMode = false;
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

		[Test]
		public void RemoveAssemblyFromList()
		{
			assemblies.RecentAssembly = "3";
			assemblies.RecentAssembly = "2";
			assemblies.RecentAssembly = "1";

			assemblies.Remove("2");

			IList assemblyList = assemblies.GetAssemblies();
			Assertion.AssertEquals(2, assemblyList.Count);
			Assertion.AssertEquals("1", assemblyList[0]);
			Assertion.AssertEquals("3", assemblyList[1]);
		}
	}
}
