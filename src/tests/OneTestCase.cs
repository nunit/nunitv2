//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov. All Rights Reserved.
//
namespace NUnit.Tests
{
	using System;
	using NUnit.Framework;

	/// <summary>
	/// Summary description for OneTestCase. This class serves the purpose of 
	/// having a test fixture that has one and only one test case. It is used 
	/// internally for the framwork tests. 
	/// </summary>
	/// 
	[TestFixture]
	public class OneTestCase
	{
		/// <summary>
		///  The one and only test case in this fixture. It always succeeds. 
		/// </summary>
		[Test]
		public virtual void TestCase() {}
	}
}
