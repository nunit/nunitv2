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
using NUnit.Core;
using NUnit.Framework;
using NUnit.Util;
using NUnit.Tests.Assemblies;

namespace NUnit.Tests.Util
{
	/// <summary>
	/// Summary description for TestInfoTests.
	/// </summary>
	[TestFixture]	
	public class UITestNodeTests
	{
		TestSuite testSuite;
		TestSuite testFixture;
		NUnit.Core.TestCase testCase1;

		[SetUp]
		public void SetUp()
		{
			MockTestFixture mock = new MockTestFixture();
			testSuite = new TestSuite("MyTestSuite");
			testSuite.Add( mock );

			testFixture = (TestSuite)testSuite.Tests[0];

			testCase1 = (NUnit.Core.TestCase)testFixture.Tests[0];
		}

		[Test]
		public void Construction()
		{
			UITestNode test1 = new UITestNode( testSuite );
			Assert.Equals( "MyTestSuite", test1.Name );
			Assert.Equals( "MyTestSuite", test1.FullName );
			Assert.True( test1.ShouldRun, "ShouldRun" );
			Assert.True( test1.IsSuite, "IsSuite" );
			Assert.False( test1.IsTestCase, "!IsTestCase" );
			Assert.False( test1.IsFixture, "!IsFixture" );
			Assert.Equals( 5, test1.CountTestCases );

			UITestNode test2 = new UITestNode( testFixture, true );
			Assert.Equals( "MockTestFixture", test2.Name );
			Assert.Equals( "NUnit.Tests.Assemblies.MockTestFixture", test2.FullName );
			Assert.True( test2.ShouldRun, "ShouldRun" );
			Assert.True( test2.IsSuite, "IsSuite" );
			Assert.False( test2.IsTestCase, "!IsTestCase" );
			Assert.True( test2.IsFixture, "IsFixture" );
			Assert.Equals( 5, test2.CountTestCases );

			UITestNode test3 = new UITestNode( testCase1 );
			Assert.Equals( "MockTest1", test3.Name );
			Assert.Equals( "NUnit.Tests.Assemblies.MockTestFixture.MockTest1", test3.FullName );
			Assert.True( test3.ShouldRun, "ShouldRun" );
			Assert.False( test3.IsSuite, "!IsSuite" );
			Assert.True( test3.IsTestCase, "IsTestCase" );
			Assert.False( test3.IsFixture, "!IsFixture" );
			Assert.Equals( 1, test3.CountTestCases );
		}

		[Test]
		public void PopulateTests()
		{
			UITestNode test1 = new UITestNode( testSuite );
			Assert.False( test1.Populated, "Default should be to not populate" );
			test1.PopulateTests();
			Assert.True( test1.Populated, "Should be populated after call" );
			UITestNode test1Child = test1.Tests[0] as UITestNode;
			Assert.True( test1Child.Populated, "Child should be populated" );

			UITestNode test2 = new UITestNode( testSuite, true );
			Assert.True( test2.Populated, "Should be populated initialy" );
			UITestNode test2Child = test2.Tests[0] as UITestNode;
			Assert.True( test2Child.Populated, "Child should be populated" );

			UITestNode test3 = new UITestNode( testSuite );
			int count = test3.CountTestCases;
			Assert.True( test3.Populated, "CountTestCases should populate" );

			UITestNode test4 = new UITestNode( testSuite );
			UITestNode test4Child = test4.Tests[0] as UITestNode;
			Assert.True( test4.Populated, "Accessing Tests should populate" );

			UITestNode test5 = new UITestNode( testSuite );
			bool fixture = test5.IsFixture;
			Assert.True( test5.Populated, "IsFixture should populate" );
		}

		[Test]
		public void Conversion()
		{
			UITestNode test1 = testSuite;
			Assert.Equals( "MyTestSuite", test1.Name );
			Assert.Equals( "MyTestSuite", test1.FullName );
			Assert.True( test1.ShouldRun, "ShouldRun" );
			Assert.True( test1.IsSuite, "IsSuite" );
			Assert.False( test1.IsTestCase, "!IsTestCase" );
			Assert.False( test1.IsFixture, "!IsFixture" );
			Assert.Equals( 5, test1.CountTestCases );

			UITestNode test2 = testFixture;
			Assert.Equals( "MockTestFixture", test2.Name );
			Assert.Equals( "NUnit.Tests.Assemblies.MockTestFixture", test2.FullName );
			Assert.True( test2.ShouldRun, "ShouldRun" );
			Assert.True( test2.IsSuite, "IsSuite" );
			Assert.False( test2.IsTestCase, "!IsTestCase" );
			Assert.True( test2.IsFixture, "IsFixture" );
			Assert.Equals( 5, test2.CountTestCases );

			UITestNode test3 = testCase1;
			Assert.Equals( "MockTest1", test3.Name );
			Assert.Equals( "NUnit.Tests.Assemblies.MockTestFixture.MockTest1", test3.FullName );
			Assert.True( test3.ShouldRun, "ShouldRun" );
			Assert.False( test3.IsSuite, "!IsSuite" );
			Assert.True( test3.IsTestCase, "IsTestCase" );
			Assert.False( test3.IsFixture, "!IsFixture" );
			Assert.Equals( 1, test3.CountTestCases );
		}
	}
}
