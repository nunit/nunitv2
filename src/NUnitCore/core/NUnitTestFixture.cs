// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org.
// ****************************************************************

using System;
using System.Reflection;
using System.Collections;

namespace NUnit.Core
{
    /// <summary>
    /// Class to implement an NUnit test fixture
    /// </summary>
    public class NUnitTestFixture : TestFixture
    {
        public NUnitTestFixture(Type fixtureType)
            : this(fixtureType, null) { }

        public NUnitTestFixture(Type fixtureType, object[] arguments)
            : base(fixtureType, arguments)
        {
            this.fixtureSetUpMethods =
                Reflect.GetMethodsWithAttribute(fixtureType, NUnitFramework.FixtureSetUpAttribute, true);
            this.fixtureTearDownMethods =
                Reflect.GetMethodsWithAttribute(fixtureType, NUnitFramework.FixtureTearDownAttribute, true);
            this.setUpMethods = 
                Reflect.GetMethodsWithAttribute(this.FixtureType, NUnitFramework.SetUpAttribute, true);
            this.tearDownMethods = 
                Reflect.GetMethodsWithAttribute(this.FixtureType, NUnitFramework.TearDownAttribute, true);

            ArrayList collectedActions = new ArrayList();

            collectedActions.AddRange(ActionsHelper.GetActionsFromAttributes(fixtureType));

            Type[] fixtureInterfaces = this.FixtureType.GetInterfaces();

            foreach (Type fixtureInterface in fixtureInterfaces)
                collectedActions.AddRange(ActionsHelper.GetActionsFromAttributes(fixtureInterface));

            this.actions = new Attribute[collectedActions.Count];
            collectedActions.CopyTo(this.actions);
        }

        protected override void DoOneTimeSetUp(TestResult suiteResult)
        {
            base.DoOneTimeSetUp(suiteResult);

			suiteResult.AssertCount = NUnitFramework.Assert.GetAssertCount(); ;
        }

        protected override void DoOneTimeTearDown(TestResult suiteResult)
        {
            base.DoOneTimeTearDown(suiteResult);

			suiteResult.AssertCount += NUnitFramework.Assert.GetAssertCount();
        }
    }
}
