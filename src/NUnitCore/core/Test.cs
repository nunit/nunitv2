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
	using System.Reflection;

	/// <summary>
	///		Test Class.
	/// </summary>
	public abstract class Test : LongLivingMarshalByRefObject, ITest, IComparable
	{
		#region Fields
		/// <summary>
		/// TestName that identifies this test
		/// </summary>
		private TestName testName;

		/// <summary>
		/// Indicates whether the test should be executed
		/// </summary>
		private RunState runState;

		/// <summary>
		/// The reason for not running the test
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
		/// True if the test is valid and could be run
		/// </summary>
		private bool isRunnable;
		#endregion

		#region Construction

		protected Test( string name )
		{
			this.testName = new TestName();
			this.testName.FullName = name;
			this.testName.Name = name;
			this.testName.TestID = new TestID();

			this.isRunnable = true;
            this.runState = RunState.Runnable;
		}

		protected Test( string pathName, string name ) 
		{ 
			this.testName = new TestName();
			this.testName.FullName = pathName == null || pathName == string.Empty 
				? name : pathName + "." + name;
			this.testName.Name = name;
			this.testName.TestID = new TestID();

			this.isRunnable = true;
            this.runState = RunState.Runnable;
		}
	
		internal void SetRunnerID( int runnerID, bool recursive )
		{
			this.testName.RunnerID = runnerID;

			if ( recursive && this.Tests != null )
				foreach( Test child in this.Tests )
					child.SetRunnerID( runnerID, true );
		}

		#endregion

		#region Properties
		public TestName TestName
		{
			get { return testName; }
		}

		public string Name
		{
			get { return testName.Name; }
		}

		public string FullName 
		{
			get { return testName.FullName; }
		}

		/// <summary>
		/// Key used to locate a test. Although the
		/// ID alone would be sufficient, we combine it with the
		/// FullName for ease in debugging and for use in messages.
		/// </summary>
		public string UniqueName
		{
			get { return testName.UniqueName; }
		}

		/// <summary>
		/// The TestID is a quasi-unique identifier for tests. It supports
		/// over four billion test nodes in a single runner tree.
		/// </summary>
		/// <summary>
		/// ID of the runner that loaded or created this test.
		/// </summary>
		public int RunnerID
		{
			get { return testName.RunnerID; }
			set { testName.RunnerID = value; }
		}

		/// <summary>
		/// Whether or not the test should be run
		/// </summary>
		public virtual bool ShouldRun
		{
            get { return runState == RunState.Runnable; }
		}

        public RunState RunState
        {
            get { return runState; }
            set { runState = value; }
        }

		/// <summary>
		/// Reason for not running the test, if applicable
		/// </summary>
		public string IgnoreReason
		{
			get { return ignoreReason; }
			set { ignoreReason = value; }
		}

		ITest ITest.Parent 
		{
			get { return parent; }
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

		public String Description
		{
			get { return description; }
			set { description = value; }
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
		public abstract int CountTestCases(TestFilter filter);
		
		public abstract int TestCount { get; }

		public abstract bool IsSuite { get; }
		public abstract bool IsFixture{ get; }
		public abstract bool IsTestCase{ get; }

		public abstract IList Tests { get; }

		public abstract bool Filter(TestFilter filter);

		public abstract TestResult Run( EventListener listener );
		public abstract TestResult Run(EventListener listener, TestFilter filter);
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
