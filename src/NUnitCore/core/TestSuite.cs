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
	using System.Reflection;
	using NUnit.Core.Filters;

	/// <summary>
	/// Summary description for TestSuite.
	/// </summary>
	/// 
	[Serializable]
	public class TestSuite : Test
	{
		private static readonly string EXPLICIT_SELECTION_REQUIRED = "Explicit selection required";
	
		public enum SetUpState
		{
			SetUpNeeded,
			SetUpComplete,
			SetUpFailed
		}

		#region Fields
		/// <summary>
		/// Our collection of child tests
		/// </summary>
		private ArrayList tests = new ArrayList();

		/// <summary>
		/// The Type of the fixture, or null
		/// </summary>
		private Type fixtureType;

		/// <summary>
		/// The fixture object, if it has been created
		/// </summary>
		private object fixture;
		
		/// <summary>
		/// The test setup method for this suite
		/// </summary>
		protected MethodInfo testSetUp;

		/// <summary>
		/// The test teardown method for this suite
		/// </summary>
		protected MethodInfo testTearDown;

		/// <summary>
		/// The fixture setup method for this suite
		/// </summary>
		protected MethodInfo fixtureSetUp;

		/// <summary>
		/// The fixture teardown method for this suite
		/// </summary>
		protected MethodInfo fixtureTearDown;

		private bool needParentTearDown;

//		/// <summary>
//		/// True if the fixture has been set up successfully
//		/// </summary>
//		private bool isSetUp;

		/// <summary>
		/// Indicates whether setup has been done yet
		/// </summary>
		private SetUpState status = SetUpState.SetUpNeeded;

		#endregion

		#region Constructors

		public TestSuite( string name ) 
			: base( name ) { }

		public TestSuite( string parentSuiteName, string name ) 
			: base( parentSuiteName, name ) { }

		public TestSuite( Type fixtureType ) 
			: base( fixtureType.FullName ) 
		{
			this.fixtureType = fixtureType;
			if ( fixtureType.Namespace != null )
				this.TestName.Name = FullName.Substring( FullName.LastIndexOf( '.' ) + 1 );
		}

		#endregion

		#region Public Methods
		public void Sort()
		{
			this.tests.Sort();

			foreach( Test test in Tests )
			{
				TestSuite suite = test as TestSuite;
				if ( suite != null )
					suite.Sort();
			}		
		}

		public void Add( Test test ) 
		{
			if(test.ShouldRun)
			{
				test.ShouldRun = ShouldRun;
				test.IgnoreReason = IgnoreReason;
			}
			test.Parent = this;
			tests.Add(test);
		}

		public void Add( object fixture )
		{
			Test test = TestFixtureBuilder.Make( fixture );
			if ( test != null )
				Add( test );
		}
		#endregion

		#region Properties

		public override IList Tests 
		{
			get { return tests; }
		}

		public bool SetUpComplete
		{
			get { return status == SetUpState.SetUpComplete; }
		}

		public bool SetUpNeeded
		{
			get { return status == SetUpState.SetUpNeeded; }
		}

		public bool SetUpFailed
		{
			get { return status == SetUpState.SetUpFailed; }
		}

		public SetUpState Status
		{
			get { return status; }
			set { status = value; }
		}

		public object Fixture
		{
			get { return fixture; }
			set { fixture = value; }
		}

		public Type FixtureType
		{
			get { return fixtureType; }
		}

		public MethodInfo SetUpMethod
		{
			get { return testSetUp; }
		}

		public MethodInfo TearDownMethod
		{
			get { return testTearDown; }
		}

		public override bool IsSuite
		{
			get { return true; }
		}

		public override bool IsTestCase
		{
			get { return false; }
		}

		/// <summary>
		/// True if this is a fixture.
		/// TODO: An easier way to tell this?
		/// </summary>
		public override bool IsFixture
		{
			get	{ return false;	}
		}

		#endregion

		#region Test Overrides
		public override int CountTestCases()
		{
			int count = 0;

			foreach(Test test in Tests)
			{
				count += test.CountTestCases();
			}
			return count;
		}

		public override int CountTestCases(ITestFilter filter)
		{
			int count = 0;

			if(this.Filter(filter)) 
			{
				foreach(Test test in Tests)
				{
					count += test.CountTestCases(filter);
				}
			}
			return count;
		}

		public override TestResult Run(EventListener listener)
		{
			return Run( listener, EmptyFilter.Empty);
		}
			
		public override TestResult Run(EventListener listener, ITestFilter filter)
		{
			TestSuiteResult suiteResult = new TestSuiteResult( this, Name);

			listener.SuiteStarted( new TestInfo( this ) );
			long startTime = DateTime.Now.Ticks;

			if ( ShouldRun )
			{
				suiteResult.Executed = true;	
				DoOneTimeSetUp( suiteResult );
			}

			RunAllTests( suiteResult, listener, filter );

			if ( ShouldRun && SetUpComplete )
				DoOneTimeTearDown( suiteResult );

			long stopTime = DateTime.Now.Ticks;
			double time = ((double)(stopTime - startTime)) / (double)TimeSpan.TicksPerSecond;
			suiteResult.Time = time;

			listener.SuiteFinished(suiteResult);
			return suiteResult;
		}

		public override bool Filter(ITestFilter filter) 
		{
			return filter.Pass(this);
		}
		#endregion

		#region Virtual Methods
		public virtual void DoOneTimeSetUp( TestResult suiteResult )
		{
			// TODO: Get rid of the cast, if possible
			TestSuite parentSuite = this.Parent as TestSuite;
			if ( parentSuite != null && parentSuite.SetUpNeeded )
			{
				parentSuite.DoOneTimeSetUp( suiteResult );
				needParentTearDown = parentSuite.SetUpComplete;
			}

			if ( parentSuite == null || parentSuite.SetUpComplete )
				DoFixtureSetUp( suiteResult );
		}

		public virtual void DoFixtureSetUp( TestResult suiteResult )
		{
			this.status = SetUpState.SetUpComplete;
		}

		public virtual void DoOneTimeTearDown( TestResult suiteResult )
		{
			DoFixtureTearDown( suiteResult );
			
			// TODO: Remove the need to do this cast
			TestSuite parentSuite = this.Parent as TestSuite;
			if ( parentSuite != null  && parentSuite.SetUpComplete && needParentTearDown )
			{
				needParentTearDown = false; // Do first in case of exception
				parentSuite.DoOneTimeTearDown( suiteResult );
			}
		}

		public virtual void DoFixtureTearDown( TestResult suiteResult )
		{
			this.status = SetUpState.SetUpNeeded;
		}

		protected virtual void RunAllTests(
			TestSuiteResult suiteResult, EventListener listener, ITestFilter filter )
		{
			foreach(Test test in ArrayList.Synchronized(Tests))
			{
				bool saveShouldRun = test.ShouldRun;

				if (test.ShouldRun)
				{
					if (this.ShouldRun == false)
					{
						test.ShouldRun = false;
						test.IgnoreReason = this.IgnoreReason;
					}
					else if ( test.IsExplicit && ( filter is EmptyFilter || filter is NotFilter ) )
					{
						test.ShouldRun = false;
						test.IgnoreReason = EXPLICIT_SELECTION_REQUIRED;
					}
				}
					
				if ( filter == null || test.Filter( filter ) )
				{
					suiteResult.AddResult( test.Run( listener, filter ) );
				}
				
				if ( saveShouldRun && !test.ShouldRun ) 
				{
					test.ShouldRun = true;
					test.IgnoreReason = null;
				}
			}
		}
		#endregion
	}
}
