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
using NUnit.Framework;
using NUnit.Core;

namespace NUnit.Tests.Core
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
			TestSuite suite = builder.Build( testsDll, "NUnit.Tests.Core.AllTests" );
			Assert.IsNotNull(suite);
		}

		[Test]
		public void BuildTestSuiteFromAssembly() 
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			TestSuite suite = builder.Build(testsDll);
			Assert.IsNotNull(suite);
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
			TestSuite suite = builder.Build( testsDll, "NUnit.Tests.Core.SuiteBuilderTests+Suite" );
			Assert.IsNotNull(suite, "Could not discover suite attribute");
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
			TestSuite suite = builder.Build( testsDll, "NUnit.Tests.Assemblies.AssemblyTests+NonConformingSuite" );
			Assert.IsNull(suite, "Suite propertye returns wrong type");
		}

		[Test]
		public void TrimPathAndExtensionTest() 
		{
			string fileName = @"d:\somedirectory\foo.txt";
			FileInfo info = new FileInfo(fileName);
			string extension = info.Extension;
			Assert.AreEqual(".txt", extension);
			Assert.AreEqual("foo.txt", info.Name);

			TestSuiteBuilder builder = new TestSuiteBuilder();
			string result = builder.TrimPathAndExtension(fileName);
			Assert.AreEqual("foo", result);
		}
	}
}
