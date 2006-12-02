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
using NUnit.TestUtilities;
using NUnit.TestData.TestFixtureBuilderTests;

namespace NUnit.Core.Tests
{
	[TestFixture]
	public class TestFixtureBuilderTests
	{
		TestSuiteBuilder builder = new TestSuiteBuilder();

		private Test loadFixture( Type type )
		{
			TestPackage package = new TestPackage( type.Module.Name );
			package.TestName = type.FullName;
			Test suite= builder.Build( package );
			Assert.IsNotNull(suite);

			return suite;
		}

		[Test]
		public void GoodSignature()
		{
			string methodName = "TestVoid";
			Test fixture = loadFixture( typeof( SignatureTestFixture ) );
			Test foundTest = TestFinder.Find( methodName, fixture );
			Assert.IsNotNull( foundTest );
			Assert.AreEqual( RunState.Runnable, foundTest.RunState );
		}

		[Test]
		public void LoadCategories() 
		{
			Test fixture = loadFixture( typeof( HasCategories ) );
			Assert.IsNotNull(fixture);
			Assert.AreEqual(2, fixture.Categories.Count);
		}
	}
}
