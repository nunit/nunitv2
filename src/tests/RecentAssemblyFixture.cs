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
		RecentAssemblySettings assemblies;

		[SetUp]
		public void SetUp()
		{
			NUnitRegistry.TestMode = true;
			NUnitRegistry.ClearTestKeys();
			assemblies = UserSettings.RecentAssemblies;
		}

		[TearDown]
		public void TearDown()
		{
			NUnitRegistry.TestMode = false;
		}

		[Test]
		public void RetrieveSubKey()
		{
			Assert.NotNull(assemblies);
		}

		[Test]
		public void GetMostRecentAssembly()
		{
			string assemblyFileName = "tests.dll";
			Assert.Null("first time this should be null", assemblies.RecentFile);
			assemblies.RecentFile = assemblyFileName;
			Assert.Equals(assemblyFileName, assemblies.RecentFile);
		}

		[Test]
		public void GetAssemblies()
		{
			assemblies.RecentFile = "3";
			assemblies.RecentFile = "2";
			assemblies.RecentFile = "1";
			IList list = assemblies.GetFiles();
			Assert.Equals(3, list.Count);
			Assert.Equals("1", list[0]);
		}


		private void SetMockRegistryValues()
		{
			assemblies.RecentFile = "5";
			assemblies.RecentFile = "4";
			assemblies.RecentFile = "3";
			assemblies.RecentFile = "2";
			assemblies.RecentFile = "1";  // this is the most recent
		}

		[Test]
		public void ReorderAssemblies5()
		{
			SetMockRegistryValues();
			assemblies.RecentFile = "5";

			IList assemblyList = assemblies.GetFiles();
			Assert.Equals("5", assemblyList[0]);
			Assert.Equals("1", assemblyList[1]);
			Assert.Equals("2", assemblyList[2]);
			Assert.Equals("3", assemblyList[3]);
			Assert.Equals("4", assemblyList[4]);
		}

		[Test]
		public void ReorderAssemblies4()
		{
			SetMockRegistryValues();
			assemblies.RecentFile = "4";

			IList assemblyList = assemblies.GetFiles();
			Assert.Equals("4", assemblyList[0]);
			Assert.Equals("1", assemblyList[1]);
			Assert.Equals("2", assemblyList[2]);
			Assert.Equals("3", assemblyList[3]);
			Assert.Equals("5", assemblyList[4]);
		}

		[Test]
		public void ReorderAssembliesNew()
		{
			SetMockRegistryValues();
			assemblies.RecentFile = "6";

			IList assemblyList = assemblies.GetFiles();
			Assert.Equals("6", assemblyList[0]);
			Assert.Equals("1", assemblyList[1]);
			Assert.Equals("2", assemblyList[2]);
			Assert.Equals("3", assemblyList[3]);
			Assert.Equals("4", assemblyList[4]);
		}


		[Test]
		public void ReorderAssemblies3()
		{
			SetMockRegistryValues();
			assemblies.RecentFile = "3";

			IList assemblyList = assemblies.GetFiles();
			Assert.Equals("3", assemblyList[0]);
			Assert.Equals("1", assemblyList[1]);
			Assert.Equals("2", assemblyList[2]);
			Assert.Equals("4", assemblyList[3]);
			Assert.Equals("5", assemblyList[4]);
		}

		[Test]
		public void ReorderAssemblies2()
		{
			SetMockRegistryValues();
			assemblies.RecentFile = "2";

			IList assemblyList = assemblies.GetFiles();
			Assert.Equals("2", assemblyList[0]);
			Assert.Equals("1", assemblyList[1]);
			Assert.Equals("3", assemblyList[2]);
			Assert.Equals("4", assemblyList[3]);
			Assert.Equals("5", assemblyList[4]);
		}

		[Test]
		public void ReorderAssemblies1()
		{
			SetMockRegistryValues();
			assemblies.RecentFile = "1";

			IList assemblyList = assemblies.GetFiles();
			Assert.Equals("1", assemblyList[0]);
			Assert.Equals("2", assemblyList[1]);
			Assert.Equals("3", assemblyList[2]);
			Assert.Equals("4", assemblyList[3]);
			Assert.Equals("5", assemblyList[4]);
		}

		[Test]
		public void AddAssemblyListNotFull()
		{
			assemblies.RecentFile = "3";
			assemblies.RecentFile = "2";
			assemblies.RecentFile = "1";  // this is the most recent

			assemblies.RecentFile = "3";

			IList assemblyList = assemblies.GetFiles();
			Assert.Equals(3, assemblyList.Count);
			Assert.Equals("3", assemblyList[0]);
			Assert.Equals("1", assemblyList[1]);
			Assert.Equals("2", assemblyList[2]);
		}

		[Test]
		public void AddAssemblyToList()
		{
			assemblies.RecentFile = "1";
			assemblies.RecentFile = "3";

			IList assemblyList = assemblies.GetFiles();
			Assert.Equals(2, assemblyList.Count);
			Assert.Equals("3", assemblyList[0]);
			Assert.Equals("1", assemblyList[1]);
		}

		[Test]
		public void RemoveAssemblyFromList()
		{
			assemblies.RecentFile = "3";
			assemblies.RecentFile = "2";
			assemblies.RecentFile = "1";

			assemblies.Remove("2");

			IList assemblyList = assemblies.GetFiles();
			Assert.Equals(2, assemblyList.Count);
			Assert.Equals("1", assemblyList[0]);
			Assert.Equals("3", assemblyList[1]);
		}
	}
}
