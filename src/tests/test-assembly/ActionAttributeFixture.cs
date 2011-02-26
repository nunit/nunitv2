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
    public class SetupFixture : BaseSetupFixture
    {
    }

    [SampleAction("BaseSetUpFixture")]
    public abstract class BaseSetupFixture
    {
    }

    [TestFixture]
    [SampleAction("Fixture")]
    public class ActionAttributeFixture : BaseActionAttributeFixture, IWithAction
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

    [SampleAction("BaseFixture")]
    public abstract class BaseActionAttributeFixture : IBaseWithAction
    {
    }

    [SampleAction("Interface")]
    public interface IWithAction : IBaseWithAction
    {
        StringCollection Results { get; }
    }

    [SampleAction("BaseInterface")]
    public interface IBaseWithAction
    {
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method | AttributeTargets.Module, AllowMultiple = true, Inherited = true)]
    public class SampleActionAttribute : Attribute, ITestSuiteAction, ITestCaseAction
    {
        private string _Prefix = null;

        public SampleActionAttribute(string prefix)
        {
            _Prefix = prefix;
        }

        void ITestSuiteAction.BeforeTestSuite(object fixture)
        {
            AddResult(fixture, null);
        }

        void ITestSuiteAction.AfterTestSuite(object fixture)
        {
            AddResult(fixture, null);
        }

        void ITestCaseAction.BeforeTestCase(object fixture, MethodInfo method)
        {
            AddResult(fixture, method);
        }

        void ITestCaseAction.AfterTestCase(object fixture, MethodInfo method)
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

            if(ActionAttributeFixture.Results != null)
                ActionAttributeFixture.Results.Add(message);
        }
    }
}
