using System;
using System.Collections.Specialized;
using NUnit.Framework;

namespace NUnit.TestData
{
    [TestFixture]
    [SuiteAction(/* priority: */ 2)]
    [SuiteAction(/* priority: */ 1)]
    [SuiteAction(/* priority: */ 3)]
    public class ActionAttributeFixture : IWithAction
    {
        public static StringCollection Results { get { return _LastRunFixture == null ? null : _LastRunFixture._Results; } }

        private static ActionAttributeFixture _LastRunFixture = null;
        private StringCollection _Results = null;

        public ActionAttributeFixture()
        {
            _Results = new StringCollection();
            _LastRunFixture = this;
        }

        StringCollection IWithAction.Results { get { return _Results; } }

        [TestAction(/* priority: */ 2)]
        [TestAction(/* priority: */ 1)]
        [TestAction(/* priority: */ 3)]
        [Test]
        public void SomeTest()
        {
            ((IWithAction)this).Results.Add("SomeTest");
        }

        [Test]
        public void SomeOtherTest()
        {
            ((IWithAction)this).Results.Add("SomeOtherTest");
        }

        [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
        private class SuiteActionAttribute : Attribute, ISuiteAction, ITestAction
        {
            private int _Priority = 0;

            public SuiteActionAttribute(int priority)
            {
                _Priority = priority;
            }

            void ISuiteAction.BeforeSuite(object fixture)
            {
                ((IWithAction)fixture).Results.Add("Fixture.BeforeSuite-" + _Priority);
            }

            void ISuiteAction.AfterSuite(object fixture)
            {
                ((IWithAction)fixture).Results.Add("Fixture.AfterSuite-" + _Priority);
            }

            void ITestAction.BeforeTest(object fixture)
            {
                ((IWithAction)fixture).Results.Add("Fixture.BeforeTest-" + _Priority);
            }

            void ITestAction.AfterTest(object fixture)
            {
                ((IWithAction)fixture).Results.Add("Fixture.AfterTest-" + _Priority);
            }

            int IAction.Priority
            {
                get { return _Priority; }
            }
        }

        [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
        private class TestActionAttribute : Attribute, ITestAction
        {
            private int _Priority = 0;

            public TestActionAttribute(int priority)
            {
                _Priority = priority;
            }

            void ITestAction.BeforeTest(object fixture)
            {
                ((IWithAction)fixture).Results.Add("Test.BeforeTest-" + _Priority);
            }

            void ITestAction.AfterTest(object fixture)
            {
                ((IWithAction)fixture).Results.Add("Test.AfterTest-" + _Priority);
            }

            int IAction.Priority
            {
                get { return _Priority; }
            }
        }

    }

    [InterfaceAction(/* priority: */ 2)]
    public interface IWithAction
    {
        StringCollection Results { get; }
    }

    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
    public class InterfaceActionAttribute : Attribute, ISuiteAction, ITestAction
    {
        private int _Priority = 0;

        public InterfaceActionAttribute(int priority)
        {
            _Priority = priority;
        }

        void ISuiteAction.BeforeSuite(object fixture)
        {
            ((IWithAction)fixture).Results.Add("Interface.BeforeSuite-" + _Priority);
        }

        void ISuiteAction.AfterSuite(object fixture)
        {
            ((IWithAction)fixture).Results.Add("Interface.AfterSuite-" + _Priority);
        }

        void ITestAction.BeforeTest(object fixture)
        {
            ((IWithAction)fixture).Results.Add("Interface.BeforeTest-" + _Priority);
        }

        void ITestAction.AfterTest(object fixture)
        {
            ((IWithAction)fixture).Results.Add("Interface.AfterTest-" + _Priority);
        }

        int IAction.Priority
        {
            get { return _Priority; }
        }
    }
}
