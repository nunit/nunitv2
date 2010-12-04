using System;
using System.Collections.Generic;
using System.Text;

namespace NUnit.Framework
{
    public interface ITestBehavior : IBehavior
    {
        void BeforeTest(object fixture);
        void AfterTest(object fixture);
    }
}
