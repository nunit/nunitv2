using System;
using System.Reflection;

namespace NUnit.Core.Extensibility
{
    /// <summary>
    /// ParameterSet encapsulates method arguments and
    /// other selected parameters needed for constructing
    /// a parameterized test case.
    /// </summary>
    public class ParameterSet
    {
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
        public System.Type ExpectedExceptionType
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
            get { return description; }
            set { description = value; }
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
        /// Constructs a ParameterSet from another object,
        /// accessing properties by reflection. The object
        /// must expose at least an Arguments property.
        /// </summary>
        /// <param name="source"></param>
        public static ParameterSet FromDataSource(object source)
        {
            ParameterSet parms = new ParameterSet();
            Type type = source.GetType();

            if (source is object[])
                parms.Arguments = source as object[];
            else
            {
                parms.Arguments = GetParm(source, "Arguments") as object[];
                parms.ExpectedExceptionType = GetParm(source, "ExpectedException") as Type;
                if (parms.ExpectedExceptionType != null)
                    parms.ExpectedExceptionName = parms.ExpectedExceptionType.FullName;
                else
                    parms.ExpectedExceptionName = GetParm(source, "ExpectedExceptionName") as string;
                parms.ExpectedExceptionMessage = GetParm(source, "ExpectedExceptionMessage") as string;
                parms.Result = GetParm(source, "Result");
                parms.Description = GetParm(source, "Description") as string;
                parms.TestName = GetParm(source, "TestName") as string;
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
