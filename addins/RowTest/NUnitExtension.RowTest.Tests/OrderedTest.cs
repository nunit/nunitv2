// *********************************************************************
// Copyright 2007, Andreas Schlapsi
// This is free software licensed under the MIT license. 
// *********************************************************************
using System;
using NUnit.Framework;
using NUnitExtension.RowTest;

namespace NUnitExtension.RowTest.Tests
{
	// The tests in this fixture should appear ordered in the NUnit GUI.
	[TestFixture]
	public class OrderedTest
	{
		[Test]
		public void Test1_SomeTest()
		{
			Assert.AreEqual (1, 1);
		}

		[RowTest]
		[Row(100)]
		[Row(200)]
		public void Test2_OtherTest(int argument)
		{
			Assert.AreEqual (argument, argument);
		}
		
		[Test]
		public void Test3_OneMoreTest()
		{
			Assert.AreEqual (1, 1);
		}
	}
}
