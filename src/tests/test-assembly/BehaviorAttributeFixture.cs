using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace NUnit.TestData
{
    [TestFixture]
    [FixtureBehavior(Priority = 2)]
    [FixtureBehavior(Priority = 1)]
    [FixtureBehavior(Priority = 3)]
    public class BehaviorAttributeFixture
    {
        public static List<string> Results = null;

        [TestBehavior(Priority = 2)]
        [TestBehavior(Priority = 1)]
        [TestBehavior(Priority = 3)]
        [Test]
        public void SomeTest()
        {
            Results.Add("SomeTest");
        }

        private class FixtureBehaviorAttribute : BehaviorAttribute
        {
            public override void BeforeTestFixture()
            {
                Results.Add("Fixture.BeforeTestFixture-" + Priority);
            }

            public override void AfterTestFixture()
            {
                Results.Add("Fixture.AfterTestFixture-" + Priority);
            }

            public override void BeforeTest()
            {
                Results.Add("Fixture.BeforeTest-" + Priority);
            }

            public override void AfterTest()
            {
                Results.Add("Fixture.AfterTest-" + Priority);
            }
        }

        private class TestBehavior : BehaviorAttribute
        {
            public override void BeforeTest()
            {
                Results.Add("Test.BeforeTest-" + Priority);
            }

            public override void AfterTest()
            {
                Results.Add("Test.AfterTest-" + Priority);
            }
        }
    }
}
