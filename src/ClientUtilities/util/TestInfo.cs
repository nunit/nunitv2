namespace NUnit.Util
{
	using System;
	using System.Collections;
	using NUnit.Core;

	/// <summary>
	/// TestInfo holds common info needed about a test
	/// locally, avoiding the need to worry about 
	/// cross-domain references
	/// </summary>
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
		/// For a test suite, the child tests or suites
		/// Null if this is not a test suite
		/// </summary>
		private ArrayList tests;

		/// <summary>
		/// True if this is a suite
		/// </summary>
		private bool isSuite;

		/// <summary>
		/// The test suite from which this object was 
		/// constructed. Used for deferred population
		/// of the object.
		/// </summary>
		private TestSuite testSuite;

		#endregion;

		#region Construction and Conversion

		/// <summary>
		/// Construct from a test and, for a suite, optionally
		/// populate the array of child tests.
		/// </summary>
		/// <param name="test">Test for which a TestInfo is to be constructed</param>
		/// <param name="populate">True if child array is to be populated</param>
		public TestInfo ( Test test, bool populate )
		{
			fullName = test.FullName;
			testName = test.Name;
			shouldRun = test.ShouldRun;
			ignoreReason = test.IgnoreReason;
			
			if ( test is TestSuite )
			{
				testCaseCount = 0;
				testSuite = (TestSuite)test;
				isSuite = true;

				tests = new ArrayList();

				if ( populate ) PopulateTests();
			}
			else
			{
				testCaseCount = 1;
				isSuite = false;
			}
		}

		/// <summary>
		/// Default construction uses lazy population approach
		/// </summary>
		/// <param name="test"></param>
		public TestInfo ( Test test ) : this( test, false ) { }

		/// <summary>
		/// Populate the arraylist of child Tests recursively.
		/// If already populated, it has no effect.
		/// </summary>
		public void PopulateTests( )
		{
			if ( !Populated )
			{
				foreach( Test test in testSuite.Tests )
				{
					TestInfo info = new TestInfo( test, true );
					tests.Add( info );
					testCaseCount += info.CountTestCases;
				}

				testSuite = null;
			}
		}

		/// <summary>
		/// Allow implicit conversion of a Test to a TestInfo
		/// </summary>
		/// <param name="test"></param>
		/// <returns></returns>
		public static implicit operator TestInfo( Test test )
		{
			return new TestInfo( test );
		}

		#endregion

		#region Properties

		/// <summary>
		/// The reason for ignoring a test
		/// </summary>
		public string IgnoreReason
		{
			get { return ignoreReason; }
		}

		/// <summary>
		/// True if the test should be run
		/// </summary>
		public bool ShouldRun
		{
			get { return shouldRun; }
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
		/// Count of test cases in this test. If the suite
		/// has never been populated, it will be done now.
		/// </summary>
		public int CountTestCases
		{ 
			get 
			{ 
				if ( !Populated )
					PopulateTests();

				return testCaseCount; 
			}
		}

		/// <summary>
		/// Array of child tests, null if this is a test case.
		/// The array is populated on access if necessary.
		/// </summary>
		public ArrayList Tests 
		{
			get 
			{
				if ( !Populated )
					PopulateTests();

				return tests;
			}
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
		/// True if this is a fixture. May populate the test's
		/// children as a side effect.
		/// TODO: An easier way to tell this?
		/// </summary>
		public bool IsFixture
		{
			get
			{
				// A test case is obviously not a fixture
				if ( IsTestCase ) return false;

				// We have no way of constructing an empty suite unless it's a fixture
				if ( Tests.Count == 0 ) return true;
				
				// Any suite with children is a fixture if the children are test cases
				TestInfo firstChild = (TestInfo)Tests[0];
				return !firstChild.IsSuite;
			}
		}

		/// <summary>
		/// False for suites that have not yet been populated
		/// with their children, otherwise true - used for testing.
		/// </summary>
		public bool Populated
		{
			get { return testSuite == null; }
		}

		#endregion
	}
}
