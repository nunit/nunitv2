using System;

namespace NUnit.Core.Builders
{
	/// <summary>
	/// Built-in SuiteBuilder for LegacySuite
	/// </summary>
	public class LegacySuiteBuilder : ISuiteBuilder
	{
		public bool CanBuildFrom( Type type )
		{
			return !type.IsAbstract && LegacySuite.GetSuiteProperty( type ) != null;
		}

		public TestSuite BuildFrom( Type type )
		{
			return new LegacySuite( type );
		}
	}
}
