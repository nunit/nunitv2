// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using NUnit.Framework;

namespace NUnit.TestData.SetUpTest
{
	[TestFixture]
	public class SetUpAndTearDownFixture
	{
		public bool wasSetUpCalled;
		public bool wasTearDownCalled;

		[SetUp]
		public virtual void Init()
		{
			wasSetUpCalled = true;
		}

		[TearDown]
		public virtual void Destroy()
		{
			wasTearDownCalled = true;
		}

		[Test]
		public void Success(){}
	}


	[TestFixture]
	public class SetUpAndTearDownCounterFixture
	{
		public int setUpCounter;
		public int tearDownCounter;

		[SetUp]
		public virtual void Init()
		{
			setUpCounter++;
		}

		[TearDown]
		public virtual void Destroy()
		{
			tearDownCounter++;
		}

		[Test]
		public void TestOne(){}

		[Test]
		public void TestTwo(){}

		[Test]
		public void TestThree(){}
	}
		
	[TestFixture]
	public class InheritSetUpAndTearDown : SetUpAndTearDownFixture
	{
		[Test]
		public void AnotherTest(){}
	}

	[TestFixture]
	public class DefineInheritSetUpAndTearDown : SetUpAndTearDownFixture
	{
		public bool derivedSetUpCalled;
		public bool derivedTearDownCalled;

		[SetUp]
		public override void Init()
		{
			derivedSetUpCalled = true;
		}

		[TearDown]
		public override void Destroy()
		{
			derivedTearDownCalled = true;
		}

		[Test]
		public void AnotherTest(){}
	}
}
