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
using System.Collections;
using NUnit.Framework;
using NUnit.Core.Builders;

namespace NUnit.Core.Tests
{
	[TestFixture]
	public class TestCaseNameTest
	{
		[Test]
		public void TestName()
		{
			TestSuite suite = new NUnitTestFixtureBuilder().BuildFrom( typeof( OneTestCase ) );
			NUnit.Core.TestCase testCase = (NUnit.Core.TestCase)suite.Tests[0];
			Assert.AreEqual("NUnit.Core.Tests.OneTestCase.TestCase", testCase.FullName);
			Assert.AreEqual("TestCase", testCase.Name);
		}

		[Test]
		public void TestExpectedException()
		{
			TestSuite suite = new NUnitTestFixtureBuilder().BuildFrom( typeof( ExpectExceptionTest ) );
			NUnit.Core.TestCase testCase = (NUnit.Core.TestCase)suite.Tests[0];
			Assert.AreEqual("NUnit.Core.Tests.ExpectExceptionTest.TestSingle", testCase.FullName);
		}
	}
}
