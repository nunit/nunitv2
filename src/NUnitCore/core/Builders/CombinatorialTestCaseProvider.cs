// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

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
            return GetStrategy(method).GetTestCases();
        }

        private CombiningStrategy GetStrategy(MethodInfo method)
        {
            ParameterInfo[] parameters = method.GetParameters();
            IEnumerable[] sources = new IEnumerable[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
                sources[i] = dataPointProvider.GetDataFor(parameters[i]);

            if (Reflect.HasAttribute(method, SequentialAttribute, false))
                return new SequentialStrategy(sources);

            if (Reflect.HasAttribute(method, PairwiseAttribute, false) &&
                method.GetParameters().Length > 2)
                    return new PairwiseStrategy(sources);

            return new CombinatorialStrategy(sources);
        }
        #endregion
    }
}
