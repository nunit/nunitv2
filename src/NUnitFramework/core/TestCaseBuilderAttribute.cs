using System;

namespace NUnit.Core
{
	/// <summary>
	/// TestBuilderAttribute is used to mark custom test case builders.
	/// The class so marked must implement the ITestCaseBuilder interface.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
	public sealed class TestCaseBuilderAttribute : System.Attribute
	{}
}
