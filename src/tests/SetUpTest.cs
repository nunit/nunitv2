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

using NUnit.Framework;
using NUnit.Core;

namespace NUnit.Tests.Core
{
	[TestFixture]
	public class SetUpTest
	{	
		internal class SetUpAndTearDownFixture
		{
			internal bool wasSetUpCalled;
			internal bool wasTearDownCalled;

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


		internal class SetUpAndTearDownCounterFixture
		{
			internal int setUpCounter;
			internal int tearDownCounter;

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
		
		internal class InheritSetUpAndTearDown : SetUpAndTearDownFixture
		{
			[Test]
			public void AnotherTest(){}
		}

		[Test]
		public void SetUpAndTearDownCounter()
		{
			SetUpAndTearDownCounterFixture testFixture = new SetUpAndTearDownCounterFixture();
			TestSuite suite = new TestSuite("SetUpAndTearDownSuite");
			suite.Add(testFixture);
			suite.Run(NullListener.NULL);

			Assert.AreEqual(3, testFixture.setUpCounter);
			Assert.AreEqual(3, testFixture.tearDownCounter);
		}

		
		[Test]
		public void MakeSureSetUpAndTearDownAreCalled()
		{
			SetUpAndTearDownFixture testFixture = new SetUpAndTearDownFixture();
			TestSuite suite = new TestSuite("SetUpAndTearDownSuite");
			suite.Add(testFixture);
			suite.Run(NullListener.NULL);

			Assert.IsTrue(testFixture.wasSetUpCalled);
			Assert.IsTrue(testFixture.wasTearDownCalled);
		}

		[Test]
		public void CheckInheritedSetUpAndTearDownAreCalled()
		{
			InheritSetUpAndTearDown testFixture = new InheritSetUpAndTearDown();
			TestSuite suite = new TestSuite("SetUpAndTearDownSuite");
			suite.Add(testFixture);
			suite.Run(NullListener.NULL);

			Assert.IsTrue(testFixture.wasSetUpCalled);
			Assert.IsTrue(testFixture.wasTearDownCalled);
		}

		internal class DefineInheritSetUpAndTearDown : SetUpAndTearDownFixture
		{
			internal bool derivedSetUpCalled;
			internal bool derivedTearDownCalled;

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

		[Test]
		public void CheckInheritedSetUpAndTearDownAreNotCalled()
		{
			DefineInheritSetUpAndTearDown testFixture = new DefineInheritSetUpAndTearDown();
			TestSuite suite = new TestSuite("SetUpAndTearDownSuite");
			suite.Add(testFixture);
			suite.Run(NullListener.NULL);

			Assert.IsFalse(testFixture.wasSetUpCalled);
			Assert.IsFalse(testFixture.wasTearDownCalled);
			Assert.IsTrue(testFixture.derivedSetUpCalled);
			Assert.IsTrue(testFixture.derivedTearDownCalled);
		}
	}
}
