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
using NUnit.Framework;
using NUnit.Core;

namespace NUnit.Tests
{
	/// <summary>
	/// Summary description for TestFixtureExtension.
	/// </summary>
	/// 
	[TestFixture]
	public class TestFixtureExtension
	{
		private TestSuite suite;

		[TestFixture]
		private abstract class BaseTestFixture : NUnit.Framework.TestCase
		{
			internal bool baseSetup = false;
			internal bool baseTeardown = false;

			protected override void SetUp()
			{ baseSetup = true; }

			protected override void TearDown()
			{ baseTeardown = true; }
		}

		private class DerivedTestFixture : BaseTestFixture
		{
			[Test]
			public void Success()
			{
				Assert(true);
			}
		}

		private class SetUpDerivedTestFixture : BaseTestFixture
		{
			[SetUp]
			public void Init()
			{
				base.SetUp();
			}

			[Test]
			public void Success()
			{
				Assert(true);
			}
		}

		[SetUp] public void LoadFixture()
		{
			string testsDll = "NUnit.Tests.dll";
			TestSuiteBuilder builder = new TestSuiteBuilder();
			suite = builder.Build("NUnit.Tests.TestFixtureExtension+DerivedTestFixture", testsDll);
		}

		[Test] 
		public void CheckMultipleSetUp()
		{
			SetUpDerivedTestFixture testFixture = new SetUpDerivedTestFixture();
			TestSuite suite = new TestSuite("SetUpDerivedTestFixture");
			suite.Add(testFixture);
			suite.Run(NullListener.NULL);

			Assertion.AssertEquals(true, testFixture.baseSetup);		}

		[Test]
		public void DerivedTest()
		{
			Assertion.AssertNotNull(suite);

			TestSuite fixture = (TestSuite)suite.Tests[0];
			Assertion.AssertNotNull(fixture);

			TestResult result = fixture.Run(NullListener.NULL);
			Assertion.Assert(result.IsSuccess);
		}

		[Test]
		public void InheritSetup()
		{
			DerivedTestFixture testFixture = new DerivedTestFixture();
			TestSuite suite = new TestSuite("DerivedTestFixtureSuite");
			suite.Add(testFixture);
			suite.Run(NullListener.NULL);

			Assertion.AssertEquals(true, testFixture.baseSetup);
		}

		[Test]
		public void InheritTearDown()
		{
			DerivedTestFixture testFixture = new DerivedTestFixture();
			TestSuite suite = new TestSuite("DerivedTestFixtureSuite");
			suite.Add(testFixture);
			suite.Run(NullListener.NULL);

			Assertion.AssertEquals(true, testFixture.baseTeardown);
		}
	}
}
