using System;
using System.Collections.Generic;
using System.Text;

namespace NUnit.Framework
{
    public interface ISuiteBehavior : IBehavior
    {
        void BeforeSuite(object fixture);
        void AfterSuite(object fixture);
    }
}
