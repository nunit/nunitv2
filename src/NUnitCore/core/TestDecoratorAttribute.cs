using System;

namespace NUnit.Core
{
	/// <summary>
	/// TestDecoratorAttribute is used to mark custom suite builders.
	/// The class so marked must implement the ISuiteBuilder interface.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
	public sealed class TestDecoratorAttribute : Attribute
	{}
}
