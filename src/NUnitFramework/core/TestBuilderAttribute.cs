using System;

namespace NUnit.Core
{
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
