#if CLR_2_0 || CLR_4_0
using System;
using System.Collections.Specialized;
using NUnit.Framework;
using System.Diagnostics;
using System.Reflection;
using NUnit.Framework.Interfaces;
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
    public class SampleActionAttribute : Attribute, ITestAction
    {
        private string _Prefix = null;

        public SampleActionAttribute(string prefix)
        {
            _Prefix = prefix;
        }

        void ITestAction.BeforeTest(TestDetails testDetails)
        {
            AddResult("Before", testDetails);
        }

        void ITestAction.AfterTest(TestDetails testDetails)
        {
            AddResult("After", testDetails);
        }

        private void AddResult(string phase, TestDetails testDetails)
        {
            string message = string.Format("{0}.{1}-{2}" + (testDetails.Method != null ? "-{3}" : ""),
                                           _Prefix,
                                           phase + testDetails.Type,
                                           testDetails.Fixture == null ? "{no-fixture}" : testDetails.Fixture.GetType().Name,
                                           testDetails.Method != null ? testDetails.Method.Name : "");

            if(ActionAttributeFixture.Results != null)
                ActionAttributeFixture.Results.Add(message);
        }
    }
}
#endif
