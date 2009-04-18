// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org.
// ****************************************************************

using NUnit.Framework;
using NUnit.Core.Builders;
using NUnit.TestUtilities;
using NUnit.TestData.SetUpTest;

namespace NUnit.Core.Tests
{
	[TestFixture]
	public class SetUpTest
	{	
		[Test]
		public void SetUpAndTearDownCounter()
		{
			SetUpAndTearDownCounterFixture fixture = new SetUpAndTearDownCounterFixture();
			TestBuilder.RunTestFixture( fixture );

			Assert.AreEqual(3, fixture.setUpCounter);
			Assert.AreEqual(3, fixture.tearDownCounter);
		}

		
		[Test]
		public void MakeSureSetUpAndTearDownAreCalled()
		{
			SetUpAndTearDownFixture fixture = new SetUpAndTearDownFixture();
			TestBuilder.RunTestFixture( fixture );

			Assert.IsTrue(fixture.wasSetUpCalled);
			Assert.IsTrue(fixture.wasTearDownCalled);
		}

		[Test]
		public void CheckInheritedSetUpAndTearDownAreCalled()
		{
			InheritSetUpAndTearDown fixture = new InheritSetUpAndTearDown();
			TestBuilder.RunTestFixture( fixture );

			Assert.IsTrue(fixture.wasSetUpCalled);
			Assert.IsTrue(fixture.wasTearDownCalled);
		}

		[Test]
		public void CheckOverriddenSetUpAndTearDownAreNotCalled()
		{
			DefineInheritSetUpAndTearDown fixture = new DefineInheritSetUpAndTearDown();
			TestBuilder.RunTestFixture( fixture );

			Assert.IsFalse(fixture.wasSetUpCalled);
			Assert.IsFalse(fixture.wasTearDownCalled);
			Assert.IsTrue(fixture.derivedSetUpCalled);
			Assert.IsTrue(fixture.derivedTearDownCalled);
		}

        [Test]
        public void MultipleSetUpAndTearDownMethodsAreCalled()
        {
            MultipleSetUpTearDownFixture fixture = new MultipleSetUpTearDownFixture();
            TestBuilder.RunTestFixture(fixture);

            Assert.IsTrue(fixture.wasSetUp1Called, "SetUp1");
            Assert.IsTrue(fixture.wasSetUp2Called, "SetUp2");
            Assert.IsTrue(fixture.wasSetUp3Called, "SetUp3");
            Assert.IsTrue(fixture.wasTearDown1Called, "TearDown1");
            Assert.IsTrue(fixture.wasTearDown2Called, "TearDown2");
        }

        [Test]
        public void BaseSetUpIsCalledFirstTearDownLast()
        {
            DerivedClassWithSeparateSetUp fixture = new DerivedClassWithSeparateSetUp();
            TestBuilder.RunTestFixture(fixture);

            Assert.IsTrue(fixture.wasSetUpCalled, "Base SetUp Called");
            Assert.IsTrue(fixture.wasTearDownCalled, "Base TearDown Called");
            Assert.IsTrue(fixture.wasDerivedSetUpCalled, "Derived SetUp Called");
            Assert.IsTrue(fixture.wasDerivedTearDownCalled, "Derived TearDown Called");
            Assert.IsTrue(fixture.wasBaseSetUpCalledFirst, "SetUp Order");
            Assert.IsTrue(fixture.wasBaseTearDownCalledLast, "TearDown Order");
        }
	}
}
