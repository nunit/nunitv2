using System;

namespace NUnit.Core
{
	[AttributeUsage(AttributeTargets.Class)]
	public class TestBuilderAttribute : Attribute
	{
		private Type builderType;

		public TestBuilderAttribute(Type builderType)
		{
			this.builderType = builderType;
		}

		public Type BuilderType
		{
			get { return builderType; }
		}
	}
}
