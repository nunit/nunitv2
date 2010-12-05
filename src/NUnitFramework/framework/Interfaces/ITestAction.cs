using System;
using System.Collections.Generic;
using System.Text;

namespace NUnit.Framework
{
    public interface ITestAction : IAction
    {
        void BeforeTest(object fixture);
        void AfterTest(object fixture);
    }
}
