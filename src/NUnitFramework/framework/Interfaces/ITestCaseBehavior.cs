using System;
using System.Collections.Generic;
using System.Text;

namespace NUnit.Framework
{
    public interface ITestCaseBehavior
    {
        void BeforeTestCase(object fixture);
        void AfterTestCase(object fixture);
    }
}
