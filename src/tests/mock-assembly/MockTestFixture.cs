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

namespace NUnit.Tests.Assemblies
{
	using System;
	using NUnit.Framework;

	/// <summary>
	/// Summary description for MockTestFixture.
	/// </summary>
	/// 
	[TestFixture(Description="Fake Test Fixture")]
	[Category("FixtureCategory")]
	public class MockTestFixture
	{
		public static readonly int Tests = 6;
		public static readonly int NotRun = 3;

		[Test(Description="Mock Test #1")]
		public void MockTest1()
		{}

		[Test]
		[Category("MockCategory")]
		public void MockTest2()
		{}

		[Test]
		[Category("MockCategory")]
		[Category("AnotherCategory")]
		public void MockTest3()
		{}

		[Test]
		protected void MockTest5()
		{}

		[Test]
		[Ignore("ignoring this test method for now")]
		[Category("Foo")]
		public void MockTest4()
		{}

		[Test, Explicit]
		[Category( "Special" )]
		public void ExplicitlyRunTest()
		{}
	}
}
