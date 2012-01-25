#if CLR_2_0 || CLR_4_0
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
        [SampleAction("ParameterizedMethodSuite", ActionTargets.Suite)]
        [SampleAction("Method")]
        public void SomeTest(string message)
        {
            ((IWithAction)this).Results.Add(message);
        }

        [Test]
        public void SomeTestNotParameterized()
        {
            ((IWithAction)this).Results.Add("SomeTestNotParameterized");
        }
    }

    [SampleAction("BaseFixture")]
    public abstract class BaseActionAttributeFixture : IBaseWithAction
    {
    }

    [SampleAction("Interface")]
    public interface IWithAction
    {
        StringCollection Results { get; }
    }

    [SampleAction("BaseInterface")]
    public interface IBaseWithAction
    {
    }

    public class SampleActionAttribute : TestActionAttribute
    {
        private readonly string _Prefix = null;
        private readonly ActionTargets _Targets = ActionTargets.Site;

        public SampleActionAttribute(string prefix)
        {
            _Prefix = prefix;
        }

        public SampleActionAttribute(string prefix, ActionTargets targets)
        {
            _Prefix = prefix;
            _Targets = targets;
        }

        public override void BeforeTest(TestDetails testDetails)
        {
            AddResult("Before", testDetails);
        }

        public override void AfterTest(TestDetails testDetails)
        {
            AddResult("After", testDetails);
        }

        public override ActionTargets Targets
        {
            get { return _Targets; }
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
