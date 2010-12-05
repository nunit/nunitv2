using System;
using System.Collections.Generic;
using System.Text;

namespace NUnit.Framework
{
    public interface IAssemblyAction : IAction
    {
        void BeforeAllTests();
        void AfterAllTests();
    }
}
