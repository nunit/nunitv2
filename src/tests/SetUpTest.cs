/********************************************************************************************************************
'
' Copyright (c) 2002, James Newkirk, Michael C. Two, Alexei Vorontsov, Philip Craig
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
' THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
'
'*******************************************************************************************************************/
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
