using System;
using System.Collections.Specialized;
using NUnit.Framework;
using System.Diagnostics;
using System.Reflection;
using NUnit.TestData.ActionAttributeTests;

[assembly: SampleAction("Assembly")]

namespace NUnit.TestData.ActionAttributeTests
{
    [SetUpFixture]
    [SampleAction("SetUpFixture")]
    public class SetupFixture
    {
    }

    [TestFixture]
    [SampleAction("Fixture")]
    public class ActionAttributeFixture : IWithAction
    {
        private static StringCollection _Results = null;
        public static StringCollection Results
        {
            get { return _Results; }
            set { _Results = value; }
        }

        StringCollection IWithAction.Results { get { return Results; } }

        [Test, TestCase("SomeTest-Case1"), TestCase("SomeTest-Case2")]
        [SampleAction("Method")]
        public void SomeTest(string message)
        {
            ((IWithAction)this).Results.Add(message);
        }

        [Test]
        public void SomeOtherTest()
        {
            ((IWithAction)this).Results.Add("SomeOtherTest");
        }

    }

    [SampleAction("Interface")]
    public interface IWithAction
    {
        StringCollection Results { get; }
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method | AttributeTargets.Module, AllowMultiple = true, Inherited = true)]
    public class SampleActionAttribute : Attribute, ISuiteAction, ITestAction
    {
        private string _Prefix = null;

        public SampleActionAttribute(string prefix)
        {
            _Prefix = prefix;
        }

        void ISuiteAction.BeforeSuite(object fixture)
        {
            AddResult(fixture, null);
        }

        void ISuiteAction.AfterSuite(object fixture)
        {
            AddResult(fixture, null);
        }

        void ITestAction.BeforeTest(object fixture, MethodInfo method)
        {
            AddResult(fixture, method);
        }

        void ITestAction.AfterTest(object fixture, MethodInfo method)
        {
            AddResult(fixture, method);
        }

        private void AddResult(object fixture, MethodInfo method)
        {
            StackFrame frame = new StackFrame(1);
            MethodBase actionMethod = frame.GetMethod();

            string actionMethodName = actionMethod.Name.Substring(actionMethod.Name.LastIndexOf('.') + 1);
            string message = string.Format("{0}.{1}-{2}" + (method != null ? "-{3}" : ""),
                                           _Prefix,
                                           actionMethodName,
                                           fixture == null ? "{no-fixture}" : fixture.GetType().Name,
                                           method != null ? method.Name : "");

            ActionAttributeFixture.Results.Add(message);
        }
    }
}
