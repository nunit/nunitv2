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
using System.Reflection;
using NUnit.Framework;
using NUnit.Core;
using NUnit.Tests.TestClasses;

namespace NUnit.Tests.Core
{
	/// <summary>
	/// Summary description for AttributeTests.
	/// </summary>
	/// 
	[TestFixture]
	public class TestFixtureBuilderTests
	{
		private string testsDll = "nunit.tests.dll";
		TestSuiteBuilder builder = new TestSuiteBuilder();

		#region Private & Internal Classes Used by Tests

		#endregion

		#region Helper Methods

		private void InvalidSignatureTest(string methodName, string reason)
		{
			TestSuite fixture = LoadFixture("NUnit.Tests.TestClasses.SignatureTestFixture");
			NUnit.Core.TestCase foundTest = FindTestByName(fixture, methodName);
			Assert.IsNotNull(foundTest);
			Assert.IsFalse(foundTest.ShouldRun);
			string expected = String.Format("Method {0}'s signature is not correct: {1}.", methodName, reason);
			Assert.AreEqual(expected, foundTest.IgnoreReason);
		}

		private TestSuite LoadFixture(string fixtureName)
		{
			TestSuite suite = builder.Build(testsDll, fixtureName );
			Assert.IsNotNull(suite);

			return suite;
		}

		private NUnit.Core.TestCase FindTestByName(TestSuite fixture, string methodName)
		{
			NUnit.Core.TestCase foundTest = null;
			foreach(Test test in fixture.Tests)
			{
				NUnit.Core.TestCase testCase = test as NUnit.Core.TestCase;
				if(testCase != null)
				{
					if(testCase.Name.Equals(methodName))
						foundTest = testCase;
				}

				if(foundTest != null)
					break;
			}

			return foundTest;
		}

		#endregion

		[Test]
		public void GoodSignature()
		{
			string methodName = "TestVoid";
			TestSuite fixture = LoadFixture("NUnit.Tests.TestClasses.SignatureTestFixture");
			NUnit.Core.TestCase foundTest = FindTestByName(fixture, methodName);
			Assert.IsNotNull(foundTest);
			Assert.IsTrue(foundTest.ShouldRun);
		}

		[TestFixture]
		[Category("fixture category")]
		[Category("second")]
		private class HasCategories 
		{
			[Test] public void OneTest()
			{}
		}

		[Test]
		public void LoadCategories() 
		{
			TestSuite fixture = LoadFixture("NUnit.Tests.Core.TestFixtureBuilderTests+HasCategories");
			Assert.IsNotNull(fixture);
			Assert.AreEqual(2, fixture.Categories.Count);
		}
	}
}
