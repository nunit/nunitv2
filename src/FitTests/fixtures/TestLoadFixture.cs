using System;
using System.IO;
using System.Text;
using System.Collections;
using NUnit.Core;
using NUnit.Util;

namespace NUnit.Fixtures
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class TestLoadFixture : CodeSnippetFixture
	{
		public TestLoadFixture() : base( Action.Run ) { }

		public TestTree Tree()
		{
			if ( testRunner.Test == null )
				return new TestTree( "NULL" );

			if ( testRunner.Test.Tests.Count == 0 )
				return new TestTree( "EMPTY" );

			StringBuilder sb = new StringBuilder();
			AppendTests( sb, "", testRunner.Test.Tests );

			return new TestTree( sb.ToString() );
		}

		private void AppendTests( StringBuilder sb, string prefix, IList tests )
		{
			foreach( TestNode test in tests )
			{
				sb.Append( prefix );
				sb.Append( test.TestName.Name );
				sb.Append( Environment.NewLine );
				if ( test.Tests != null )
					AppendTests( sb, prefix + ">", test.Tests );
			}
		}

		public int Skipped()
		{
			return testRunner.Test.TestCount - testSummary.ResultCount - testSummary.TestsNotRun;
		}

		public int Run()
		{
			return testSummary.ResultCount;
		}

		public int Failures()
		{
			return testSummary.Failures;
		}

		public int Ignored()
		{
			return testSummary.TestsNotRun;
		}
	}
}
