using System;
using System.Collections.Generic;
using System.Text;

namespace NUnit.Framework
{
    public interface IAssemblyBehavior : IBehavior
    {
        void BeforeAllTests();
        void AfterAllTests();
    }
}
