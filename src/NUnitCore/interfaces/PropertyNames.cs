using System;

namespace NUnit.Core
{
    /// <summary>
    /// The PropertyNames struct lists common property names, which are
    /// accessed by reflection in the NUnit core. This provides a modicum 
    /// of type safety as opposed to using the strings directly.
    /// </summary>
    public struct PropertyNames
    {
        public static readonly string ExpectedException = "ExpectedException";
        public static readonly string ExpectedExceptionName = "ExpectedExceptionName";
        public static readonly string ExpectedMessage = "ExpectedMessage";
        public static readonly string Result = "Result";
        public static readonly string Description = "Description";
        public static readonly string TestName = "TestName";
        public static readonly string Arguments = "Arguments";
        public static readonly string Properties = "Properties";
        public static readonly string Categories = "Categories";
        public static readonly string CategoryName = "Name";
        public static readonly string Reason = "Reason";
        public static readonly string RequiredAddin = "RequiredAddin";
    }
}
