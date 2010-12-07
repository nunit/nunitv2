using System;
using System.Collections.Specialized;
using NUnit.Framework;

namespace NUnit.TestData.ActionAttributeTests
{
    [SetUpFixtureAction(4, "SetUpFixture")]
    [SetUpFixture]
    public class SetupFixtureWithActionAttribute
    {
    }

    [TestFixture]
    [SuiteAction(/* priority: */ 2, "Fixture")]
    [SuiteAction(/* priority: */ 1, "Fixture")]
    [SuiteAction(/* priority: */ 3, "Fixture")]
    public class ActionAttributeFixture : IWithAction
    {
        private static StringCollection _Results = null;
        public static StringCollection Results
        {
            get { return _Results; }
            set { _Results = value; }
        }

        StringCollection IWithAction.Results { get { return Results; } }

        [TestAction(/* priority: */ 2, "Test")]
        [TestAction(/* priority: */ 1, "Test")]
        [TestAction(/* priority: */ 3, "Test")]
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

    }

    [InterfaceAction(/* priority: */ 2, "Interface")]
    public interface IWithAction
    {
        StringCollection Results { get; }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class SetUpFixtureActionAttribute : Attribute, ISuiteAction, ITestAction
    {
        private int _Priority = 0;
        private string _Prefix = null;

        public SetUpFixtureActionAttribute(int priority, string prefix)
        {
            _Priority = priority;
            _Prefix = prefix;
        }

        void ISuiteAction.BeforeSuite(object fixture)
        {
            ActionAttributeFixture.Results.Add(_Prefix + ".BeforeSuite-" + _Priority);
        }

        void ISuiteAction.AfterSuite(object fixture)
        {
            ActionAttributeFixture.Results.Add(_Prefix + ".AfterSuite-" + _Priority);
        }

        void ITestAction.BeforeTest(object fixture)
        {
            ActionAttributeFixture.Results.Add(_Prefix + ".BeforeTest-" + _Priority);
        }

        void ITestAction.AfterTest(object fixture)
        {
            ActionAttributeFixture.Results.Add(_Prefix + ".AfterTest-" + _Priority);
        }

        int IAction.Priority
        {
            get { return _Priority; }
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class SuiteActionAttribute : Attribute, ISuiteAction, ITestAction
    {
        private int _Priority = 0;
        private string _Prefix = null;

        public SuiteActionAttribute(int priority, string prefix)
        {
            _Priority = priority;
            _Prefix = prefix;
        }

        void ISuiteAction.BeforeSuite(object fixture)
        {
            ((IWithAction)fixture).Results.Add(_Prefix + ".BeforeSuite-" + _Priority);
        }

        void ISuiteAction.AfterSuite(object fixture)
        {
            ((IWithAction)fixture).Results.Add(_Prefix + ".AfterSuite-" + _Priority);
        }

        void ITestAction.BeforeTest(object fixture)
        {
            ((IWithAction)fixture).Results.Add(_Prefix + ".BeforeTest-" + _Priority);
        }

        void ITestAction.AfterTest(object fixture)
        {
            ((IWithAction)fixture).Results.Add(_Prefix + ".AfterTest-" + _Priority);
        }

        int IAction.Priority
        {
            get { return _Priority; }
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class TestActionAttribute : Attribute, ITestAction
    {
        private int _Priority = 0;
        private string _Prefix = null;

        public TestActionAttribute(int priority, string prefix)
        {
            _Priority = priority;
            _Prefix = prefix;
        }

        void ITestAction.BeforeTest(object fixture)
        {
            ((IWithAction)fixture).Results.Add(_Prefix + ".BeforeTest-" + _Priority);
        }

        void ITestAction.AfterTest(object fixture)
        {
            ((IWithAction)fixture).Results.Add(_Prefix + ".AfterTest-" + _Priority);
        }

        int IAction.Priority
        {
            get { return _Priority; }
        }
    }

    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
    public class InterfaceActionAttribute : Attribute, ISuiteAction, ITestAction
    {
        private int _Priority = 0;
        private string _Prefix = null;

        public InterfaceActionAttribute(int priority, string prefix)
        {
            _Priority = priority;
            _Prefix = prefix;
        }

        void ISuiteAction.BeforeSuite(object fixture)
        {
            ((IWithAction)fixture).Results.Add(_Prefix + ".BeforeSuite-" + _Priority);
        }

        void ISuiteAction.AfterSuite(object fixture)
        {
            ((IWithAction)fixture).Results.Add(_Prefix + ".AfterSuite-" + _Priority);
        }

        void ITestAction.BeforeTest(object fixture)
        {
            ((IWithAction)fixture).Results.Add(_Prefix + ".BeforeTest-" + _Priority);
        }

        void ITestAction.AfterTest(object fixture)
        {
            ((IWithAction)fixture).Results.Add(_Prefix + ".AfterTest-" + _Priority);
        }

        int IAction.Priority
        {
            get { return _Priority; }
        }
    }

}
