using System;

namespace NUnit.Core
{
	/// <summary>
	/// Struct used to define behavior of a GenericTestFixtureBuilder
	/// </summary>
	public struct TestFixtureParameters
	{
		public string RequiredFramework;
		public string TestFixtureType;
		public string TestFixturePattern;
		public string TestCaseType;
		public string TestCasePattern;
		public string ExpectedExceptionType;
		public string SetUpType;
		public string TearDownType;
		public string FixtureSetUpType;
		public string FixtureTearDownType;
		public string IgnoreType;

		public TestFixtureParameters(
			string RequiredFramework,
			string Namespace,
			string TestFixtureType,
			string TestFixturePattern,
			string TestCaseType,
			string TestCasePattern,
			string ExpectedExceptionType,
			string SetUpType,
			string TearDownType,
			string FixtureSetUpType,
			string FixtureTearDownType,
			string IgnoreType )
		{
			this.RequiredFramework = RequiredFramework;
			this.TestFixtureType = Namespace + "." + TestFixtureType;
			this.TestFixturePattern = TestFixturePattern;
			this.TestCaseType = Namespace + "." + TestCaseType;
			this.TestCasePattern = TestCasePattern;
			this.ExpectedExceptionType = Namespace + "." + ExpectedExceptionType;
			this.SetUpType = Namespace + "." + SetUpType;
			this.TearDownType = Namespace + "." + TearDownType;
			this.FixtureSetUpType = Namespace + "." + FixtureSetUpType;
			this.FixtureTearDownType = Namespace + "." + FixtureTearDownType;
			this.IgnoreType = Namespace + "." + IgnoreType;
		}

		public bool HasRequiredFramework
		{
			get { return IsValid( this.RequiredFramework ); }
		}

		public bool HasTestFixtureType
		{
			get { return IsValid( this.TestFixtureType ); }
		}

		public bool HasTestFixturePattern
		{
			get { return IsValid( this.TestFixturePattern ); }
		}

		public bool HasTestCaseType
		{
			get { return IsValid( this.TestCaseType ); }
		}

		public bool HasTestCasePattern
		{
			get { return IsValid( this.TestCasePattern ); }
		}

		public bool HasExpectedExceptionType
		{
			get { return IsValid( this.ExpectedExceptionType ); }
		}

		public bool HasSetUpType
		{
			get { return IsValid( this.SetUpType ); }
		}

		public bool HasTearDownType
		{
			get { return IsValid( this.TearDownType ); }
		}

		public bool HasFixtureSetUpType
		{
			get { return IsValid( this.FixtureSetUpType ); }
		}

		public bool HasFixtureTearDownType
		{
			get { return IsValid( this.FixtureTearDownType ); }
		}

		public bool HasIgnoreType
		{
			get { return IsValid( this.IgnoreType ); }
		}

		private bool IsValid( string s )
		{
			return s != null && s != string.Empty;
		}
	}
}
