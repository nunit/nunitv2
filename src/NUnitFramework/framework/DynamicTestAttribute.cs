using System;

namespace NUnit.Framework
{
    /// <summary>
    /// Adding this attribute to a method within a <seealso cref="TestFixtureAttribute"/> 
    /// class makes the method callable from the NUnit test runner. In addition, it
    /// indicates that individual test case instances using the method will be created
    /// at runt take parameters.
    /// </summary>
    /// 
    /// <example>
    /// [TestFixture]
    /// public class Fixture
    /// {
    ///   [DynamicTest]
    ///   public void MethodToTest()
    ///   {}
    /// }
    /// </example>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class DynamicTestAttribute : Attribute
    {
    }
}
