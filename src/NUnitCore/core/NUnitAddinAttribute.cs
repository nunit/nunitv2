using System;

namespace NUnit.Core
{
	/// <summary>
	/// SuiteBuilderAttribute is used to mark custom suite builders.
	/// The class so marked must implement the ISuiteBuilder interface.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
		public sealed class NUnitAddinAttribute : Attribute
	{}
}
