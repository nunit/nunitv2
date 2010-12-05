using System;
using System.Collections.Generic;
using System.Text;

namespace NUnit.Framework
{
    public interface ISuiteAction : IAction
    {
        void BeforeSuite(object fixture);
        void AfterSuite(object fixture);
    }
}
