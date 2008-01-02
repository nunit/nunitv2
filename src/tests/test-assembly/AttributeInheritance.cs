using System;
using NUnit.Framework;

namespace NUnit.TestData
{
	// Sample Test from a post by Scott Bellware

	[AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
	class ConcernAttribute : TestFixtureAttribute
	{
		private Type typeOfConcern;

		public ConcernAttribute( Type typeOfConcern )
		{
			this.typeOfConcern = typeOfConcern;
		}
	}

	[AttributeUsage(AttributeTargets.Method, AllowMultiple=false)]
	class SpecAttribute : TestAttribute
	{
	}

	/// <summary>
	/// Summary description for AttributeInheritance.
	/// </summary>
	[Concern(typeof(NUnit.Core.TestRunner))]
	public class When_collecting_test_fixtures
	{
		[Spec]
		public void should_include_classes_with_an_attribute_derived_from_TestFixtureAttribute()
		{
		}
	}
}
