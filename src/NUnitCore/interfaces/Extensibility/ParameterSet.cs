// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;

namespace NUnit.Core.Extensibility
{
    /// <summary>
    /// ParameterSet encapsulates method arguments and
    /// other selected parameters needed for constructing
    /// a parameterized test case.
    /// </summary>
    public class ParameterSet : NUnit.Framework.ITestCaseData
    {
        #region Constants
        private static readonly string DESCRIPTION = "_DESCRIPTION";
        private static readonly string IGNOREREASON = "_IGNOREREASON";
        private static readonly string CATEGORIES = "_CATEGORIES";
        #endregion

        #region Instance Fields
        private RunState runState;
        private string notRunReason;
        private object[] arguments;
        private System.Type expectedExceptionType;
        private string expectedExceptionName;
        private string expectedExceptionMessage;
        private object result;
        private string description;
        private string testName;

        /// <summary>
        /// A dictionary of properties, used to add information
        /// to tests without requiring the class to change.
        /// </summary>
        private IDictionary properties;
        #endregion

        #region Properties
        /// <summary>
        /// The RunState for this set of parameters.
        /// </summary>
        public RunState RunState
        {
            get { return runState; }
            set { runState = value; }
        }

        /// <summary>
        /// The reason for not running the test case
        /// represented by this ParameterSet
        /// </summary>
        public string NotRunReason
        {
            get { return notRunReason; }
            set { notRunReason = value; }
        }

        /// <summary>
        /// The arguments to be used in running the test,
        /// which must match the method signature.
        /// </summary>
        public object[] Arguments
        {
            get { return arguments; }
            set { arguments = value; }
        }

        /// <summary>
        /// The Type of any exception that is expected.
        /// </summary>
        public System.Type ExpectedException
        {
            get { return expectedExceptionType; }
            set { expectedExceptionType = value; }
        }

        /// <summary>
        /// The FullName of any exception that is expected
        /// </summary>
        public string ExpectedExceptionName
        {
            get { return expectedExceptionName; }
            set { expectedExceptionName = value; }
        }

        /// <summary>
        /// The Message of any exception that is expected
        /// </summary>
        public string ExpectedExceptionMessage
        {
        	get { return expectedExceptionMessage; }
        	set { expectedExceptionMessage = value; }
        }

        /// <summary>
        /// The expected result of the test, which
        /// must match the method return type.
        /// </summary>
        public object Result
        {
            get { return result; }
            set { result = value; }
        }

        /// <summary>
        /// A description to be applied to this test case
        /// </summary>
        public string Description
        {
            get { return (string) Properties[DESCRIPTION]; }
            set 
            {
                if (value != null)
                    Properties[DESCRIPTION] = value;
                else
                    Properties.Remove(DESCRIPTION);
            }
        }

        /// <summary>
        /// A name to be used for this test case in lieu
        /// of the standard generated name containing
        /// the argument list.
        /// </summary>
        public string TestName
        {
            get { return testName; }
            set { testName = value; }
        }

        /// <summary>
        /// Gets a list of categories associated with this test.
        /// </summary>
        public IList Categories
        {
            get
            {
                if (Properties[CATEGORIES] == null)
                    Properties[CATEGORIES] = new ArrayList();

                return (IList)Properties[CATEGORIES];
            }
        }

        /// <summary>
        /// Gets the property dictionary for this test
        /// </summary>
        public IDictionary Properties
        {
            get
            {
                if (properties == null)
                    properties = new ListDictionary();

                return properties;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Construct a ParameterSet, setting it's RunState
        /// and specifying the reason why it can't be run.
        /// </summary>
        /// <param name="runState"></param>
        /// <param name="reason"></param>
        public ParameterSet(RunState runState, string reason)
        {
            this.runState = runState;
            this.notRunReason = reason;
        }

        /// <summary>
        /// Construct an empty parameter set, which
        /// defaults to being Runnable.
        /// </summary>
        public ParameterSet()
        {
            this.runState = RunState.Runnable;
        }
        #endregion

        #region Static Methods
        /// <summary>
        /// Constructs a ParameterSet from another object, accessing properties 
        /// by reflection. The object must expose at least an Arguments property
        /// in order for the test to be runnable.
        /// </summary>
        /// <param name="source"></param>
        public static ParameterSet FromDataSource(object source)
        {
            ParameterSet parms = new ParameterSet();

            parms.Arguments = GetParm(source, "Arguments") as object[];
            parms.ExpectedException = GetParm(source, "ExpectedException") as Type;
            if (parms.ExpectedException != null)
                parms.ExpectedExceptionName = parms.ExpectedException.FullName;
            else
                parms.ExpectedExceptionName = GetParm(source, "ExpectedExceptionName") as string;
            parms.ExpectedExceptionMessage = GetParm(source, "ExpectedExceptionMessage") as string;
            parms.Result = GetParm(source, "Result");
            parms.Description = GetParm(source, "Description") as string;
            parms.TestName = GetParm(source, "TestName") as string;

            // Some sources may also implement Properties and/or Categories
            bool gotCategories = false;
            IDictionary props = GetParm(source, "Properties") as IDictionary;
            if ( props != null )
                foreach (string key in props.Keys)
                {
                    parms.Properties.Add(key, props[key]);
                    if (key == CATEGORIES) gotCategories = true;
                }

            // Some sources implement Categories. They may have been
            // provided as properties or they may be separate.
            if (!gotCategories)
            {
                IList categories = GetParm(source, "Categories") as IList;
                if (categories != null && props[CATEGORIES] == null)
                    foreach (string cat in categories)
                        categories.Add(cat);
            }

            return parms;
        }

        private static object GetParm(object source, string name)
        {
            Type type = source.GetType();
            PropertyInfo prop = type.GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
            if (prop != null)
                return prop.GetValue(source, null);

            FieldInfo field = type.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField);
            if (field != null)
                return field.GetValue(source);

            return null;
        }
        #endregion
    }
}
