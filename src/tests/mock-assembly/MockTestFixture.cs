//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov. All Rights Reserved.
//
namespace Nunit.Tests.Assemblies
{
	using System;
	using Nunit.Framework;

	/// <summary>
	/// Summary description for MockTestFixture.
	/// </summary>
	/// 
	[TestFixture]
	public class MockTestFixture
	{
		[Test]
		public void MockTest1()
		{}

		[Test]
		public void MockTest2()
		{}

		[Test]
		public void MockTest3()
		{}

		[Test]
		protected void MockTest5()
		{}


		[Test]
		[Ignore("ignoring this test method for now")]
		public void MockTest4()
		{}
	}
}
