namespace NUnit.Util
{
	using System;
	using NUnit.Core;

	/// <summary>
	/// Summary description for TestResultInfo.
	/// </summary>
	public class TestResultInfo
	{
		#region Instance Variables

		/// <summary>
		/// Name of this result - usually matches that of the test
		/// </summary>
		private string name;

		/// <summary>
		/// True if the test was executed
		/// </summary>
		private bool executed;

		/// <summary>
		/// True if the test failed
		/// </summary>
		private bool failed;

		/// <summary>
		/// Time of test execution, in milliseconds?
		/// </summary>
		private double time;

		/// <summary>
		/// Message associated with a failing test
		/// </summary>
		private string message;

		/// <summary>
		/// Stack trace associated with a failing test case
		/// </summary>
		private string stacktrace;

		/// <summary>
		/// The test for which this is a result
		/// </summary>
		TestInfo test;

#if NUNIT_LEAKAGE_TEST
		private long leakage;
#endif

		#endregion

		#region Construction and Conversion

		/// <summary>
		/// Construct from a TestCaseResult
		/// </summary>
		/// <param name="result">Result from which to construct the object</param>
		public TestResultInfo( TestCaseResult result )
		{
			test = new TestInfo( result.Test );

			name = result.Name;
			executed = result.Executed;
			failed = result.IsFailure;
			time = result.Time;
#if NUNIT_LEAKAGE_TEST
			leakage = result.Leakage;
#endif
			message = result.Message;
			stacktrace = result.StackTrace;
		}

		/// <summary>
		/// Construct from a test suite result
		/// </summary>
		/// <param name="result">Result from which to construct the object</param>
		public TestResultInfo( TestSuiteResult result )
		{
			test = new TestInfo( result.Test );

			name = result.Name;
			executed = result.Executed;
			failed = result.IsFailure;
			time = result.Time;
#if NUNIT_LEAKAGE_TEST
			leakage = result.Leakage;
#endif
			message = result.Message;
		}

		/// <summary>
		/// Construct from a TestSuite and name - for testing convenience
		/// </summary>
		/// <param name="suite">Suite for which to construct a result</param>
		/// <param name="name">Name to give the result</param>
		public TestResultInfo( TestSuite suite, string name )
		{
			test = new TestInfo( suite );

			TestSuiteResult result = new TestSuiteResult( suite, name );
			name = result.Name;
			executed = result.Executed;
			failed = result.IsFailure;
			time = result.Time;
#if NUNIT_LEAKAGE_TEST
			leakage = result.Leakage;
#endif
			message = result.Message;
		}

		/// <summary>
		/// Construct from a TestCase - for testing convenience
		/// </summary>
		/// <param name="suite">TestCase for which to construct a result</param>
		public TestResultInfo( NUnit.Core.TestCase testCase )
		{
			test = new TestInfo( testCase );

			TestCaseResult result = new TestCaseResult( testCase );
			name = result.Name;
			executed = result.Executed;
			failed = result.IsFailure;
			time = result.Time;
#if NUNIT_LEAKAGE_TEST
			leakage = result.Leakage;
#endif
			message = result.Message;
			stacktrace = result.StackTrace;
		}

		/// <summary>
		/// Allow implicit conversion of a TestCaseResult
		/// </summary>
		/// <param name="result">TestCaseResult on which to base the object</param>
		/// <returns>A new TestCaseResult</returns>
		public static implicit operator TestResultInfo( TestCaseResult result )
		{
			return new TestResultInfo( result );
		}

		/// <summary>
		/// Allow implicit conversion of a TestSuiteResult
		/// </summary>
		/// <param name="result">TestSuite result to be converted</param>
		/// <returns>A new TestResultInfo</returns>
		public static implicit operator TestResultInfo( TestSuiteResult result )
		{
			return new TestResultInfo( result );
		}

		#endregion

		#region Properties

		/// <summary>
		/// The test for which this is a result
		/// </summary>
		public TestInfo Test
		{
			get { return test; }
		}

		/// <summary>
		///  The name of the result
		/// </summary>
		public string Name 
		{
			get { return name; }
		}

		/// <summary>
		/// True if the test was executed
		/// </summary>
		public bool Executed
		{
			get { return executed; }
		}

		/// <summary>
		/// True if the test failed
		/// </summary>
		public bool IsFailure
		{
			get { return failed; }
		}

		/// <summary>
		/// True if the test did not fail
		/// </summary>
		public bool IsSuccess
		{
			get { return !failed; }
		}

		/// <summary>
		/// Message associated with a failing test
		/// </summary>
		public string Message
		{
			get { return message; }
		}

		/// <summary>
		/// Stack trace associated with a failing test case
		/// </summary>
		public string StackTrace
		{
			get { return stacktrace; }
		}

#if NUNIT_LEAKAGE_TEST
		public long Leakage
		{
			get { return leakage; }
		}
#endif

		#endregion
	}
}
