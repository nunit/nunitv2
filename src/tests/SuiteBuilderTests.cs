/********************************************************************************************************************
'
' Copyright (c) 2002, James Newkirk, Michael C. Two, Alexei Vorontsov, Philip Craig
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
' THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
'
'*******************************************************************************************************************/
using System;
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
			TestSuite suite = TestSuiteBuilder.Build("NUnit.Tests.AllTests", testsDll);
			Assertion.Assert(suite != null);
		}

		[Test]
		public void BuildTestSuiteFromAssembly() 
		{
			TestSuite suite = TestSuiteBuilder.Build(testsDll);
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
			TestSuite suite = TestSuiteBuilder.Build("NUnit.Tests.SuiteBuilderTests+Suite",testsDll);
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
			TestSuite suite = TestSuiteBuilder.Build("NUnit.Tests.assemblies.AssemblyTests+NonConformingSuite",testsDll);
			Assertion.AssertNull("Suite propertye returns wrong type",suite);
		}
	}
}
