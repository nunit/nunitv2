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
	/// TestInfo is used directly in all EventListener events and in
	/// TestResults. It contains an ID, which can be used by a 
	/// runner to locate the actual test.
	/// 
	/// TestInfo also serves as the base class for TestNode, which
	/// adds hierarchical information and is used in client code to
	/// maintain a visible image of the structure of the tests.
	/// </summary>
	[Serializable]
	public class TestInfo : ITest
	{
		#region Instance Variables
		/// <summary>
		/// TestName that identifies this test
		/// </summary>
		private TestName testName;

		private ITest parent;

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

		/// <summary>
		/// A list of all the categories assigned to a test
		/// </summary>
		private ArrayList categories = new ArrayList();

		/// <summary>
		/// A dictionary of properties, used to add information
		/// to tests without requiring the class to change.
		/// </summary>
		private ListDictionary properties = new ListDictionary();

		/// <summary>
		/// True if the test is marked as Explicit
		/// </summary>
		private bool isExplicit;
		
		/// <summary>
		/// Unique Test identifier 
		/// </summary>
//		private TestID testID;

		#endregion

		#region Constructors
		/// <summary>
		/// Construct from an ITest
		/// </summary>
		/// <param name="test">Test from which a TestNode is to be constructed</param>
		public TestInfo( ITest test )
		{
			this.testName = new TestName();
			this.testName.FullName = test.FullName;
			this.testName.Name = test.Name;
			this.testName.TestID = test.TestID;

			this.shouldRun = test.ShouldRun;
			this.ignoreReason = test.IgnoreReason;
			this.description = test.Description;
			this.isExplicit = test.IsExplicit;
			this.isSuite = test.IsSuite;
			this.isFixture = test.IsFixture;

			if (test.Categories != null) 
				this.categories.AddRange(test.Categories);
			if (test.Properties != null)
			{
				this.properties = new ListDictionary();
				foreach( DictionaryEntry entry in test.Properties )
					this.properties.Add( entry.Key, entry.Value );
			}

			this.testCaseCount = test.TestCount;
		}

		public TestInfo( string name, ITest[] tests )
		{
			this.testName = new TestName();
			this.testName.FullName = name;
			this.testName.Name = name;
			this.testName.TestID = new TestID();

			this.shouldRun = true;
			this.ignoreReason = null;
			this.description = null;
			this.isExplicit = false;
			this.isSuite = true;
			this.isFixture = false;

			foreach( ITest test in tests )
			{
				this.testCaseCount += test.TestCount;
			}
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
			get { return testName.FullName; }
		}

		/// <summary>
		/// Name of the test
		/// </summary>
		public string Name
		{
			get { return testName.Name; }
		}

		public string UniqueName
		{
			get { return testName.UniqueName; }
		}

		public TestID TestID
		{
			get { return testName.TestID; }
		}

		public ITest Parent
		{
			get { return parent; }
			set { parent = value; }
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

		public virtual IList Tests
		{
			get { return null; }
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

		public IDictionary Properties
		{
			get 
			{
				if ( properties == null )
					properties = new ListDictionary();

				return properties; 
			}
		}
		#endregion
	}
}
