using System;

namespace NUnit.Core.Extensions
{
	/// <summary>
	/// SampleSuiteExtensionAttribute is used to identify a SampleSuiteExtension fixture
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
	public sealed class SampleSuiteExtensionAttribute : Attribute
	{
	}
}
