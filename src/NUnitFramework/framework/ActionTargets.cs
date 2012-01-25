#if CLR_2_0 || CLR_4_0
using System;

namespace NUnit.Framework
{
    /// <summary>
    /// The different targets a test action attribute can be applied to
    /// </summary>
    [Flags]
    public enum ActionTargets : ushort
    {
        /// <summary>
        /// Target is defined by where the action attribute is attached
        /// </summary>
        Site = 1,

        /// <summary>
        /// Target a individual test case
        /// </summary>
        Test = 2,

        /// <summary>
        /// Target a suite of test cases
        /// </summary>
        Suite = 4
    }
}
#endif
