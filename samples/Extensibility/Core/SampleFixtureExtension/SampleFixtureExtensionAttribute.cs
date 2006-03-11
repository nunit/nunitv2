using System;

namespace NUnit.Core.Extensions
{
	/// <summary>
	/// SampleFixtureExtensionAttribute is used to identify a SampleFixtureExtension class
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
	public sealed class SampleFixtureExtensionAttribute : Attribute
	{
	}
}
