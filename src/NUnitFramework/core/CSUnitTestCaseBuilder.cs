using System;
using System.Reflection;
using NUnit.Core.Builders;

namespace NUnit.Core
{
	public class CSUnitTestCaseBuilder : GenericTestCaseBuilder
	{
		public CSUnitTestCaseBuilder()
			: base( CSUnitTestFixture.Parameters ) { }

		protected override bool IsOldStyleTestMethod(MethodInfo method)
		{
			return method.Name.ToLower().StartsWith( "test" );
		}

	}
}