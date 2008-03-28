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
        /// <summary>
        /// The arguments to be used in running the test,
        /// which must match the method signature.
        /// </summary>
        public object[] Arguments;

        /// <summary>
        /// The Type of any exception that is expected.
        /// </summary>
        public System.Type ExpectedExceptionType;

        /// <summary>
        /// The FullName of any exception that is expected
        /// </summary>
        public string ExpectedExceptionName;

        /// <summary>
        /// The expected result of the test, which
        /// must match the method return type.
        /// </summary>
        public object Result;

        public string Description;

        public string TestName;

        /// <summary>
        /// Constructs a ParameterSet from an array
        /// of arguments for use in running the test.
        /// </summary>
        /// <param name="args"></param>
        public ParameterSet(params object[] args)
        {
            this.Arguments = args;
        }

        /// <summary>
        /// Factory method that constructs a ParameterSet 
        /// from an  Attribute, which must expose at least 
        /// an Arguments property.
        /// </summary>
        /// <param name="attr"></param>
        public static ParameterSet FromAttribute(System.Attribute attr)
        {
            Type attrType = attr.GetType();
            BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty;

            PropertyInfo prop = attrType.GetProperty("Arguments", flags);
            if (prop == null)
                throw new ArgumentException("Attribute must provide an Arguments property", "attr");

            object[] args = prop.GetValue(attr, null) as object[];
            ParameterSet parms = new ParameterSet(args);

            prop = attrType.GetProperty("ExpectedException", flags);
            if (prop != null)
                parms.ExpectedExceptionType = prop.GetValue(attr, null) as Type;

            prop = attrType.GetProperty("ExpectedExceptionName", flags);
            if (prop != null)
                parms.ExpectedExceptionName = prop.GetValue(attr, null) as string;

            prop = attrType.GetProperty("Result", flags);
            if (prop != null)
                parms.Result = prop.GetValue(attr, null);

            prop = attrType.GetProperty("Description", flags);
            if (prop != null)
                parms.Description = (string)prop.GetValue(attr, null);

            prop = attrType.GetProperty("TestName", flags);
            if (prop != null)
                parms.TestName = (string)prop.GetValue(attr, null);

            return parms;
        }
    }
}
