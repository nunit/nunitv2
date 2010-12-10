using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace NUnit.Framework
{
    public interface ITestAction : IAction
    {
        void BeforeTest(object fixture, MethodInfo method);
        void AfterTest(object fixture, MethodInfo method);
    }
}
