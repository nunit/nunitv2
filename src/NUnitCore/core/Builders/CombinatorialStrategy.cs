using System;
using System.Collections;
using System.Reflection;
using NUnit.Core.Extensibility;

namespace NUnit.Core.Builders
{
    public class CombinatorialStrategy : CombiningStrategy
    {
        public CombinatorialStrategy(IEnumerable[] sources) : base(sources) { }

        public override IEnumerable GetTestCases()
        {
            IEnumerator[] enumerators = new IEnumerator[Sources.Length];
            int index = -1;

#if !NET_2_0
			ArrayList testCases = new ArrayList();
#endif
            for (; ; )
            {
                while (++index < Sources.Length)
                {
                    enumerators[index] = Sources[index].GetEnumerator();
                    if (!enumerators[index].MoveNext())
#if NET_2_0
                        yield break;
#else
						return testCases;
#endif
                }

                object[] testdata = new object[Sources.Length];

                for (int i = 0; i < Sources.Length; i++)
                    testdata[i] = enumerators[i].Current;

#if NET_2_0
                yield return testdata;
#else
				testCases.Add(testdata);
#endif

                index = Sources.Length;

                while (--index >= 0 && !enumerators[index].MoveNext()) ;

                if (index < 0) break;
            }
#if !NET_2_0
			return testCases;
#endif
        }
    }
}
