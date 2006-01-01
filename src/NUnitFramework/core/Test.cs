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
		/// Int used to distinguish suites of the same
		/// name across multiple assemblies.
		/// </summary>
		private int assemblyKey;

		/// <summary>
		/// Int that is set to a unique ascending value by the
		/// TestSuiteBuilder, allowing unique identification
		/// of tests by client software.
		/// </summary>
		private int key;

		private static int nextKey = Int32.MaxValue;

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
		/// True if the test had the Explicit attribute
		/// </summary>
		private bool isExplicit;

		/// <summary>
		/// TestFramework under which this test runs
		/// </summary>
		protected ITestFramework testFramework;

		#endregion

		#region Constructors

		protected Test( string name, int assemblyKey )
		{
			this.fullName = this.testName = name;
			this.assemblyKey = assemblyKey;
			this.shouldRun = true;
			this.key = unchecked( nextKey++ );
		}

		protected Test( string pathName, string testName ) 
			: this( pathName, testName, 0 ) { }

		protected Test( string pathName, string testName, int assemblyKey ) 
		{ 
			fullName = pathName == null || pathName == string.Empty ? testName : pathName + "." + testName;
			this.testName = testName;
			this.assemblyKey = assemblyKey;
			this.shouldRun = true;
			this.key = nextKey++;
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
		/// Int used to distinguish suites of the same
		/// name across multiple assemblies.
		/// </summary>
		public int AssemblyKey
		{
			get { return assemblyKey; }
			set { assemblyKey = value; }
		}

		/// <summary>
		/// Key used to look up a test in a hash table
		/// </summary>
		public string UniqueName
		{
			get { return string.Format( "[{0}]{1}", assemblyKey, fullName ); }
		}

		public int Key
		{
			get { return key; }
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

		public string TestPath 
		{
			get
			{
				string testPath = "";
				if (parent != null)
					testPath = parent.TestPath;
				return testPath + FullName;
			}
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

		public bool IsExplicit
		{
			get { return isExplicit; }
			set { isExplicit = value; }
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
		public abstract ArrayList Tests { get; }

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
