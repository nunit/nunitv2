using System;
using System.Reflection;
using System.Collections;
using NUnit.Core.Extensibility;

namespace NUnit.Core.Builders
{
    public class CombinatorialTestCaseProvider : ITestCaseProvider
    {
        #region Static Members
        static IDataPointProvider dataPointProvider =
            (IDataPointProvider)CoreExtensions.Host.GetExtensionPoint("DataPointProviders");

        //static readonly string CombinatorialAttribute = "NUnit.Framework.CombinatorialAttribute";
        static readonly string PairwiseAttribute = "NUnit.Framework.PairwiseAttribute";
        static readonly string SequentialAttribute = "NUnit.Framework.SequentialAttribute";
        #endregion

        #region ITestCaseProvider Members

        public bool HasTestCasesFor(System.Reflection.MethodInfo method)
        {
            if (method.GetParameters().Length == 0)
                return false;

            foreach( ParameterInfo parameter in method.GetParameters() )
                if ( ! dataPointProvider.HasDataFor( parameter ) )
                    return false;

            return true;
        }

        public IEnumerable GetTestCasesFor(MethodInfo method)
        {
            if( Reflect.HasAttribute(method, SequentialAttribute, false))
                return GetSequentialTestCases(method);

            if( Reflect.HasAttribute(method, PairwiseAttribute, false))
                return GetPairwiseTestCases(method);
            
            return GetCombinatorialTestCases(method);
        }

        private IEnumerable GetPairwiseTestCases(MethodInfo method)
        {
#if NET_2_0
            yield break;
#else
			return new object[0];
#endif
        }

        private IEnumerable GetSequentialTestCases(MethodInfo method)
        {
            ParameterInfo[] parameters = method.GetParameters();
            int parmCount = parameters.Length;

            IEnumerator[] enumerators = new IEnumerator[parmCount];
            for (int i = 0; i < parmCount; i++)
                enumerators[i] = dataPointProvider.GetDataFor(parameters[i]).GetEnumerator();

#if !NET_2_0
			ArrayList testCases = new ArrayList();
#endif

            for(;;)
            {
                bool gotData = false;
                object[] testdata = new object[parmCount];

                for (int i = 0; i < parmCount; i++)
                    if (enumerators[i].MoveNext())
                    {
                        testdata[i] = enumerators[i].Current;
                        gotData = true;
                    }
                    else
                        testdata[i] = null;

				if (!gotData)
					break;
#if NET_2_0
                yield return testdata;
#else
                testCases.Add(testdata);
#endif
            }
#if !NET_2_0
			return testCases;
#endif
        }

        private IEnumerable GetCombinatorialTestCases(MethodInfo method)
        {
            ParameterInfo[] parameters = method.GetParameters();
            int parmCount = parameters.Length;

            IEnumerator[] enumerators = new IEnumerator[parmCount];
            int index = -1;

#if !NET_2_0
			ArrayList testCases = new ArrayList();
#endif
            for (;;)
            {
                while (++index < parmCount)
                {
                    enumerators[index] = dataPointProvider.GetDataFor(parameters[index]).GetEnumerator();
                    if (!enumerators[index].MoveNext())
#if NET_2_0
                        yield break;
#else
						return testCases;
#endif
                }

                object[] testdata = new object[parmCount];

                for (int i = 0; i < parmCount; i++)
                    testdata[i] = enumerators[i].Current;

#if NET_2_0
                yield return testdata;
#else
				testCases.Add(testdata);
#endif

                index = parmCount;

                while (--index >= 0 && !enumerators[index].MoveNext()) ;

                if (index < 0) break;
            }
#if !NET_2_0
			return testCases;
#endif
        }
        #endregion
    }
}
