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
