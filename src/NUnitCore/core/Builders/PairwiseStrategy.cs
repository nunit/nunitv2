// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

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
