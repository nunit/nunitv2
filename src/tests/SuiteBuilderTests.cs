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

using System;
using System.IO;
using NUnit.Framework;
using NUnit.Core;

namespace NUnit.Tests
{
	/// <summary>
	/// Summary description for SuiteBuilderTests.
	/// </summary>
	/// 
	[TestFixture]
	public class SuiteBuilderTests
	{
		private string testsDll = "NUnit.Tests.dll";

		[Test]
		public void LoadTestSuiteFromAssembly()
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			TestSuite suite = builder.Build("NUnit.Tests.AllTests", testsDll);
			Assertion.Assert(suite != null);
		}

		[Test]
		public void BuildTestSuiteFromAssembly() 
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			TestSuite suite = builder.Build(testsDll);
			Assertion.Assert(suite != null);
		}


		class Suite
		{
			[Suite]
			public static TestSuite MockSuite
			{
				get 
				{
					TestSuite testSuite = new TestSuite("TestSuite");
					return testSuite;
				}
			}
		}

		[Test]
		public void DiscoverSuite()
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			TestSuite suite = builder.Build("NUnit.Tests.SuiteBuilderTests+Suite",testsDll);
			Assertion.AssertNotNull("Could not discover suite attribute",suite);
		}

		class NonConformingSuite
		{
			[Suite]
			public static int Integer
			{
				get 
				{
					return 5;
				}
			}
		}

		[Test]
		public void WrongReturnTypeSuite()
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			TestSuite suite = builder.Build("NUnit.Tests.assemblies.AssemblyTests+NonConformingSuite",testsDll);
			Assertion.AssertNull("Suite propertye returns wrong type",suite);
		}

		[Test]
		public void TrimPathAndExtensionTest() 
		{
			string fileName = @"d:\somedirectory\foo.txt";
			FileInfo info = new FileInfo(fileName);
			string extension = info.Extension;
			Assertion.AssertEquals(".txt", extension);
			Assertion.AssertEquals("foo.txt", info.Name);

			TestSuiteBuilder builder = new TestSuiteBuilder();
			string result = builder.TrimPathAndExtension(fileName);
			Assertion.AssertEquals("foo", result);
		}
	}
}
