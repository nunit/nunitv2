#region Copyright (c) 2002-2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright © 2000-2003 Philip A. Craig
'
' This software is provided 'as-is', without any express or implied warranty. In no 
' event will the authors be held liable for any damages arising from the use of this 
' software.
' 
' Permission is granted to anyone to use this software for any purpose, including 
' commercial applications, and to alter it and redistribute it freely, subject to the 
' following restrictions:
'
' 1. The origin of this software must not be misrepresented; you must not claim that 
' you wrote the original software. If you use this software in a product, an 
' acknowledgment (see the following) in the product documentation is required.
'
' Portions Copyright © 2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright © 2000-2003 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

namespace NUnit.Core
{
	using System;
	using System.Collections;
	using System.Collections.Specialized;

	/// <summary>
	///		Test Class.
	/// </summary>
	public abstract class Test : LongLivingMarshalByRefObject, ITest, IComparable
	{
		#region Fields

		/// <summary>
		/// Name of the test
		/// </summary>
		protected string testName;

		/// <summary>
		/// Full Name of the test
		/// </summary>
		private string fullName;

		/// <summary>
		/// Integer id that is set as each test is built, allowing
		/// tests to be located and identified by the test runner.
		/// </summary>
		private TestID testID;

		/// <summary>
		/// Whether or not the test should be run
		/// </summary>
		private bool shouldRun;
		
		/// <summary>
		/// Reason for not running the test, if applicable
		/// </summary>
		private string ignoreReason;
		
		/// <summary>
		/// Description for this test 
		/// </summary>
		private string description;
		
		/// <summary>
		/// Test suite containing this test, or null
		/// </summary>
		private TestSuite parent;
		
		/// <summary>
		/// List of categories applying to this test
		/// </summary>
		private IList categories;

		/// <summary>
		/// A dictionary of properties, used to add information
		/// to tests without requiring the class to change.
		/// </summary>
		private ListDictionary properties;

		/// <summary>
		/// True if the test had the Explicit attribute
		/// </summary>
		private bool isExplicit;

		/// <summary>
		/// TestFramework under which this test runs
		/// </summary>
		protected ITestFramework testFramework;

		#endregion

		#region Construction

		protected Test( string name )
		{
			this.fullName = this.testName = name;
			this.shouldRun = true;
			this.testID = new TestID();
		}

		protected Test( string pathName, string testName ) 
		{ 
			fullName = pathName == null || pathName == string.Empty ? testName : pathName + "." + testName;
			this.testName = testName;
			this.shouldRun = true;
			this.testID = new TestID();
		}

		internal void SetRunnerID( int runnerID, bool recursive )
		{
			this.testID.RunnerID = runnerID;

			if ( recursive && this.Tests != null )
				foreach( Test child in this.Tests )
					child.SetRunnerID( runnerID, true );
		}

		#endregion

		#region Properties

		public string Name
		{
			get { return testName; }
		}

		public string FullName 
		{
			get { return fullName; }
		}

		/// <summary>
		/// Key used to locate a test. Although the
		/// ID alone would be sufficient, we combine it with the
		/// FullName for ease in debugging and for use in messages.
		/// </summary>
		public string UniqueName
		{
			get { return string.Format( "[{0}-{1}]{2}", TestID.RunnerID, TestID.TestKey, fullName ); }
		}

		/// <summary>
		/// The TestID is a quasi-unique identifier for tests. It supports
		/// over four billion test nodes in a single runner tree.
		/// </summary>
		public TestID TestID
		{
			get { return testID; }
		}

		/// <summary>
		/// ID of the runner that loaded or created this test.
		/// </summary>
		public int RunnerID
		{
			get { return testID.RunnerID; }
		}

		/// <summary>
		/// Whether or not the test should be run
		/// </summary>
		public virtual bool ShouldRun
		{
			get { return shouldRun; }
			set { shouldRun = value; }
		}

		/// <summary>
		/// Reason for not running the test, if applicable
		/// </summary>
		public string IgnoreReason
		{
			get { return ignoreReason; }
			set { ignoreReason = value; }
		}

		public TestSuite Parent 
		{
			get { return parent; }
			set { parent = value; }
		}

		public IList Categories 
		{
			get { return categories; }
			set { categories = value; }
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

		public bool IsDescendant(Test test)
		{
			if (parent != null) 
			{
				return parent == test || parent.IsDescendant(test);
			}

			return false;
		}

		public String Description
		{
			get { return description; }
			set { description = value; }
		}

		public int TestCount
		{
			get { return CountTestCases(); }
		}

		public bool IsExplicit
		{
			get { return isExplicit; }
			set { isExplicit = value; }
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

		#region Abstract Methods and Properties

		/// <summary>
		/// Count of the test cases ( 1 if this is a test case )
		/// </summary>
		public abstract int CountTestCases();
		public abstract int CountTestCases(IFilter filter);
		
		public abstract bool IsSuite { get; }
		public abstract bool IsFixture{ get; }
		public abstract bool IsTestCase{ get; }
		public abstract IList Tests { get; }

		public abstract bool Filter(IFilter filter);

		public abstract TestResult Run( EventListener listener );
		public abstract TestResult Run(EventListener listener, IFilter filter);

		#endregion

		#region IComparable Members

		public int CompareTo(object obj)
		{
			Test other = obj as Test;
			
			if ( other == null )
				return -1;

			return this.FullName.CompareTo( other.FullName );
		}

		#endregion
	}
}
