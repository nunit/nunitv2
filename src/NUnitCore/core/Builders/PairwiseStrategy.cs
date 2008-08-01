using System;
using System.Collections;
using System.Reflection;
using NUnit.Core.Extensibility;

namespace NUnit.Core.Builders
{
    public class PairwiseStrategy : CombiningStrategy
    {
        public PairwiseStrategy(IEnumerable[] sources) : base(sources) { }

        public override IEnumerable GetTestCases()
        {
#if NET_2_0
            yield break;
#else
			return new object[0];
#endif
        }
    }
}
