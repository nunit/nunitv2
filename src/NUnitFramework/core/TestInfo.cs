using System;
using System.Collections;
using System.Collections.Specialized;

namespace NUnit.Core
{
	/// <summary>
	/// TestInfo holds common info about a test. It represents only
	/// a single test or a suite and contains no references to other
	/// tests. Since it is informational only, it can easily be passed
	/// around using .Net remoting.
	/// 
	/// TestInfo also serves as the base class for TestNode, which
	/// adds hierarchical information.
	/// </summary>
	[Serializable]
	public class TestInfo
	{
		#region Instance Variables

		/// <summary>
		/// The full name of the test, including the assembly and namespaces
		/// </summary>
		private string fullName;

		/// <summary>
		/// The test name
		/// </summary>
		private string testName;

		/// <summary>
		/// Used to distinguish tests in multiple assemblies;
		/// </summary>
		private int assemblyKey;

		/// <summary>
		/// True if the test should be run
		/// </summary>
		private bool shouldRun;

		/// <summary>
		/// Reason for not running the test
		/// </summary>
		private string ignoreReason;

		/// <summary>
		/// Number of test cases in this test or suite
		/// </summary>
		private int testCaseCount;

		/// <summary>
		/// True if this is a suite
		/// </summary>
		private bool isSuite;

		/// <summary>
		/// True if this is a fixture
		/// </summary>
		private bool isFixture;

		/// <summary>
		/// The test description
		/// </summary>
		private string description;

		private ArrayList categories = new ArrayList();

		private ListDictionary properties;

		private bool isExplicit;

		private int key;

		#endregion

		#region Constructors
		/// <summary>
		/// Construct from a Test
		/// </summary>
		/// <param name="test">Test from which a TestNode is to be constructed</param>
		public TestInfo( Test test )
		{
			this.fullName = test.FullName;
			this.testName = test.Name;
			this.assemblyKey = test.AssemblyKey;
			this.shouldRun = test.ShouldRun;
			this.ignoreReason = test.IgnoreReason;
			this.description = test.Description;
			this.isExplicit = test.IsExplicit;
			this.isSuite = test.IsSuite;
			this.isFixture = test.IsFixture;

			if (test.Categories != null) 
				this.categories.AddRange(test.Categories);

			this.testCaseCount = test.CountTestCases();
			this.key = test.Key;
		}
		#endregion

		#region Properties

		/// <summary>
		/// The test description 
		/// </summary>
		public string Description
		{
			get { return description; }
			set { description = value; }
		}

		/// <summary>
		/// The reason for ignoring a test
		/// </summary>
		public string IgnoreReason
		{
			get { return ignoreReason; }
			set { ignoreReason = value; }
		}

		/// <summary>
		/// True if the test should be run
		/// </summary>
		public bool ShouldRun
		{
			get { return shouldRun; }
			set { shouldRun = value; }
		}

		/// <summary>
		/// Full name of the test
		/// </summary>
		public string FullName 
		{
			get { return fullName; }
		}

		/// <summary>
		/// Name of the test
		/// </summary>
		public string Name
		{
			get { return testName; }
		}

		/// <summary>
		/// Identifier for assembly containing this test
		/// </summary>
		public int AssemblyKey
		{
			get { return assemblyKey; }
			set { assemblyKey = value; }
		}

		public string UniqueName
		{
			get{ return string.Format( "[{0}]{1}", assemblyKey, fullName ); }
		}

		public bool IsExplicit
		{
			get { return isExplicit; }
			set { isExplicit = value; }
		}

		public IList Categories 
		{
			get { return categories; }
		}

		public bool HasCategory( string name )
		{
			return categories != null && categories.Contains( name );
		}

		public bool HasCategory( IList names )
		{
			if ( categories == null )
				return false;

			foreach( string name in names )
				if ( categories.Contains( name ) )
					return true;

			return false;
		}

		/// <summary>
		/// Count of test cases in this test.
		/// </summary>
		public int TestCount
		{ 
			get { return testCaseCount; } 
		}

		/// <summary>
		/// True if this is a suite, false if a test case
		/// </summary>
		public bool IsSuite
		{
			get { return isSuite; }
		}

		/// <summary>
		/// True if this is a test case, false if a suite
		/// </summary>
		public bool IsTestCase
		{
			get { return !isSuite; }
		}

		/// <summary>
		/// True if this is a fixture.
		/// </summary>
		public bool IsFixture
		{
			get { return isFixture; }
		}

		public ListDictionary Properties
		{
			get 
			{
				if ( properties == null )
					properties = new ListDictionary();

				return properties; 
			}
		}

		public int Key
		{
			get { return key; }
		}

		#endregion
	}
}
