using System;
using System.Collections.Specialized;
using NUnit.Framework;

namespace NUnit.TestData
{
    [TestFixture]
    [SuiteBehavior(/* priority: */ 2)]
    [SuiteBehavior(/* priority: */ 1)]
    [SuiteBehavior(/* priority: */ 3)]
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

        [TestBehavior(/* priority: */ 2)]
        [TestBehavior(/* priority: */ 1)]
        [TestBehavior(/* priority: */ 3)]
        [Test]
        public void SomeTest()
        {
            ((IWithBehavior)this).Results.Add("SomeTest");
        }

        [Test]
        public void SomeOtherTest()
        {
            ((IWithBehavior)this).Results.Add("SomeOtherTest");
        }

        [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
        private class SuiteBehaviorAttribute : Attribute, ISuiteBehavior, ITestBehavior
        {
            private int _Priority = 0;

            public SuiteBehaviorAttribute(int priority)
            {
                _Priority = priority;
            }

            void ISuiteBehavior.BeforeSuite(object fixture)
            {
                ((IWithBehavior)fixture).Results.Add("Fixture.BeforeSuite-" + _Priority);
            }

            void ISuiteBehavior.AfterSuite(object fixture)
            {
                ((IWithBehavior)fixture).Results.Add("Fixture.AfterSuite-" + _Priority);
            }

            void ITestBehavior.BeforeTest(object fixture)
            {
                ((IWithBehavior)fixture).Results.Add("Fixture.BeforeTest-" + _Priority);
            }

            void ITestBehavior.AfterTest(object fixture)
            {
                ((IWithBehavior)fixture).Results.Add("Fixture.AfterTest-" + _Priority);
            }

            int IBehavior.Priority
            {
                get { return _Priority; }
            }
        }

        [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
        private class TestBehaviorAttribute : Attribute, ITestBehavior
        {
            private int _Priority = 0;

            public TestBehaviorAttribute(int priority)
            {
                _Priority = priority;
            }

            void ITestBehavior.BeforeTest(object fixture)
            {
                ((IWithBehavior)fixture).Results.Add("Test.BeforeTest-" + _Priority);
            }

            void ITestBehavior.AfterTest(object fixture)
            {
                ((IWithBehavior)fixture).Results.Add("Test.AfterTest-" + _Priority);
            }

            int IBehavior.Priority
            {
                get { return _Priority; }
            }
        }

    }

    [InterfaceBehavior(/* priority: */ 2)]
    public interface IWithBehavior
    {
        StringCollection Results { get; }
    }

    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
    public class InterfaceBehaviorAttribute : Attribute, ISuiteBehavior, ITestBehavior
    {
        private int _Priority = 0;

        public InterfaceBehaviorAttribute(int priority)
        {
            _Priority = priority;
        }

        void ISuiteBehavior.BeforeSuite(object fixture)
        {
            ((IWithBehavior)fixture).Results.Add("Interface.BeforeSuite-" + _Priority);
        }

        void ISuiteBehavior.AfterSuite(object fixture)
        {
            ((IWithBehavior)fixture).Results.Add("Interface.AfterSuite-" + _Priority);
        }

        void ITestBehavior.BeforeTest(object fixture)
        {
            ((IWithBehavior)fixture).Results.Add("Interface.BeforeTest-" + _Priority);
        }

        void ITestBehavior.AfterTest(object fixture)
        {
            ((IWithBehavior)fixture).Results.Add("Interface.AfterTest-" + _Priority);
        }

        int IBehavior.Priority
        {
            get { return _Priority; }
        }
    }
}
