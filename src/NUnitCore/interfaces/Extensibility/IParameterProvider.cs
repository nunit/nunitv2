using System.Collections;
using System.Reflection;

namespace NUnit.Core.Extensibility
{
    /// <summary>
    /// The ITestCaseProvider interface is used by extensions
    /// that provide data for parameterized tests, along with
    /// certain flags and other indicators used in the test.
    /// </summary>
    public interface IParameterProvider
    {
        /// <summary>
        /// Determine whether any ParameterSets
        /// are available for a method.
        /// </summary>
        /// <param name="method">A MethodInfo representing the a parameterized test</param>
        /// <returns>True if any are available, otherwise false.</returns>
        bool HasParametersFor(MethodInfo method);

        /// <summary>
        /// Return an enumeration providing ParameterSets
        /// for use in running a test.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        IEnumerable GetParametersFor(MethodInfo method);
    }
}
