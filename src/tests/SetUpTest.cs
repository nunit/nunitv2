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

namespace NUnit.Tests
{
	using NUnit.Framework;
	using NUnit.Core;

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

		internal class InheritSetUpAndTearDown : SetUpAndTearDownFixture
		{
			[Test]
			public void AnotherTest(){}
		}

		[Test]
		public void MakeSureSetUpAndTearDownAreCalled()
		{
			SetUpAndTearDownFixture testFixture = new SetUpAndTearDownFixture();
			TestSuite suite = new TestSuite("SetUpAndTearDownSuite");
			suite.Add(testFixture);
			suite.Run(NullListener.NULL);

			Assertion.Assert(testFixture.wasSetUpCalled);
			Assertion.Assert(testFixture.wasTearDownCalled);
		}

		[Test]
		public void CheckInheritedSetUpAndTearDownAreCalled()
		{
			InheritSetUpAndTearDown testFixture = new InheritSetUpAndTearDown();
			TestSuite suite = new TestSuite("SetUpAndTearDownSuite");
			suite.Add(testFixture);
			suite.Run(NullListener.NULL);

			Assertion.Assert(testFixture.wasSetUpCalled);
			Assertion.Assert(testFixture.wasTearDownCalled);
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

			Assertion.Assert(!testFixture.wasSetUpCalled);
			Assertion.Assert(!testFixture.wasTearDownCalled);
			Assertion.Assert(testFixture.derivedSetUpCalled);
			Assertion.Assert(testFixture.derivedTearDownCalled);
		}
	}
}
