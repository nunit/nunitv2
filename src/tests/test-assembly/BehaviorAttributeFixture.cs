using System;
using System.Collections.Specialized;
using NUnit.Framework;

namespace NUnit.TestData
{
    [TestFixture]
    [FixtureBehavior(Priority = 2)]
    [FixtureBehavior(Priority = 1)]
    [FixtureBehavior(Priority = 3)]
    public class BehaviorAttributeFixture : IWithBehavior
    {
        public static StringCollection Results { get { return _LastRunFixture == null ? null : _LastRunFixture._Results; } }

        private static BehaviorAttributeFixture _LastRunFixture = null;
        private StringCollection _Results = null;

        public BehaviorAttributeFixture()
        {
            _Results = new StringCollection();
            _LastRunFixture = this;
        }

        StringCollection IWithBehavior.Results { get { return _Results; } }

        [TestBehavior(Priority = 2)]
        [TestBehavior(Priority = 1)]
        [TestBehavior(Priority = 3)]
        [Test]
        public void SomeTest()
        {
            ((IWithBehavior)this).Results.Add("SomeTest");
        }

        private class FixtureBehaviorAttribute : BehaviorAttribute
        {
            public override void BeforeTestFixture(object fixture)
            {
                ((IWithBehavior)fixture).Results.Add("Fixture.BeforeTestFixture-" + Priority);
            }

            public override void AfterTestFixture(object fixture)
            {
                ((IWithBehavior)fixture).Results.Add("Fixture.AfterTestFixture-" + Priority);
            }

            public override void BeforeTest(object fixture)
            {
                ((IWithBehavior)fixture).Results.Add("Fixture.BeforeTest-" + Priority);
            }

            public override void AfterTest(object fixture)
            {
                ((IWithBehavior)fixture).Results.Add("Fixture.AfterTest-" + Priority);
            }
        }

        private class TestBehavior : BehaviorAttribute
        {
            public override void BeforeTest(object fixture)
            {
                ((IWithBehavior)fixture).Results.Add("Test.BeforeTest-" + Priority);
            }

            public override void AfterTest(object fixture)
            {
                ((IWithBehavior)fixture).Results.Add("Test.AfterTest-" + Priority);
            }
        }

    }

    [InterfaceBehavior(Priority = 2)]
    public interface IWithBehavior
    {
        StringCollection Results { get; }
    }

    public class InterfaceBehaviorAttribute : BehaviorAttribute
    {
        public override void BeforeTestFixture(object fixture)
        {
            ((IWithBehavior)fixture).Results.Add("Interface.BeforeTestFixture-" + Priority);
        }

        public override void AfterTestFixture(object fixture)
        {
            ((IWithBehavior)fixture).Results.Add("Interface.AfterTestFixture-" + Priority);
        }

        public override void BeforeTest(object fixture)
        {
            ((IWithBehavior)fixture).Results.Add("Interface.BeforeTest-" + Priority);
        }

        public override void AfterTest(object fixture)
        {
            ((IWithBehavior)fixture).Results.Add("Interface.AfterTest-" + Priority);
        }
    }
}
