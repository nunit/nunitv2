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
		/// True if the test is valid and could be run
		/// </summary>
		private bool isRunnable;

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
			this.testName = (TestName)test.TestName.Clone();

			this.shouldRun = test.ShouldRun;
			this.ignoreReason = test.IgnoreReason;
			this.description = test.Description;
			this.isExplicit = test.IsExplicit;
			this.isRunnable = test.IsRunnable;
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

		/// <summary>
		/// Construct as a parent to multiple tests.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="tests"></param>
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
			this.isRunnable = true;
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
		/// Gets the completely specified name of the test
		/// encapsulated in a TestName object. TestInfo exposes
		/// getters for retrieving components of the name
		/// directly, but this property must be used to set
		/// any of them.
		/// </summary>
		public TestName TestName
		{
			get { return testName; }
		}

		/// <summary>
		/// Name of the test
		/// </summary>
		public string Name
		{
			get { return testName.Name; }
		}

		/// <summary>
		/// Full name of the test
		/// </summary>
		public string FullName 
		{
			get { return testName.FullName; }
		}

		/// <summary>
		/// Gets or sets the ID of the runner that holds the test.
		/// </summary>
		public int RunnerID
		{
			get { return testName.RunnerID; }
			set { testName.RunnerID = value; }
		}

		/// <summary>
		/// Gets the string representation of the TestName, 
		/// which uniquely identifies a test.
		/// </summary>
		public string UniqueName
		{
			get { return testName.UniqueName; }
		}

		/// <summary>
		/// The test description 
		/// </summary>
		public string Description
		{
			get { return description; }
			set { description = value; }
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
		/// The reason for ignoring a test
		/// </summary>
		public string IgnoreReason
		{
			get { return ignoreReason; }
			set { ignoreReason = value; }
		}

		/// <summary>
		/// Count of test cases in this test.
		/// </summary>
		public int TestCount
		{ 
			get { return testCaseCount; } 
		}

		/// <summary>
		///  Gets the parent test of this test
		/// </summary>
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

		public bool IsRunnable
		{
			get { return isRunnable; }
			set { isRunnable = value; }
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
