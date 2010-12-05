using System;
using System.Collections.Generic;
using System.Text;

namespace NUnit.Framework
{
    public interface ITestCaseAction : IAction
    {
        void BeforeTestCase(object fixture);
        void AfterTestCase(object fixture);
    }
}
