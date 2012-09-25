// ****************************************************************
// Copyright 2011, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

using System;
using System.Collections;

namespace NUnit.Core
{
    public class ContextDictionary : Hashtable
    {
        internal TestExecutionContext _context;

        public ContextDictionary() { }

        public ContextDictionary(TestExecutionContext context)
        {
            _context = context;
        }

        public override object this[object key]
        {
            get
            {
                // Get Result values dynamically, since
                // they may change as execution proceeds
                switch (key as string)
                {
                    case "Test.Name":
                        return _context.CurrentTest.TestName.Name;
                    case "Test.FullName":
                        return _context.CurrentTest.TestName.FullName;
                    case "Test.Properties":
                        return _context.CurrentTest.Properties;
                    case "Result.State":
                        return (int)_context.CurrentResult.ResultState;
                    case "TestDirectory":
                        return AssemblyHelper.GetDirectoryName(_context.CurrentTest.FixtureType.Assembly);
                    case "WorkDirectory":
                        return _context.TestPackage.Settings.Contains("WorkDirectory")
                            ? _context.TestPackage.Settings["WorkDirectory"]
                            : Environment.CurrentDirectory;
                    default:
                        return base[key];
                }
            }
            set
            {
                base[key] = value;
            }
        }
    }
}
