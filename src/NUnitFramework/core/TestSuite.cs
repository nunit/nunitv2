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
	using System.Text;

	/// <summary>
	/// Summary description for TestSuite.
	/// </summary>
	/// 
	[Serializable]
	public class TestSuite : Test
	{
		private static readonly string EXPLICIT_SELECTION_REQUIRED = "Explicit selection required";
		
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

		/// <summary>
		/// True if the fixture has been set up
		/// </summary>
		private bool isSetUp;
		#endregion

		#region Constructors

		/// <summary>
		/// Only used by tests and by RootTestSuite
		/// </summary>
		/// <param name="name"></param>
		public TestSuite( string name ) : this( name, 0 ) { }

		public TestSuite( string name, int assemblyKey ) 
			: base( name, assemblyKey ) { }

		public TestSuite( string parentSuiteName, string name, int assemblyKey ) 
			: base( parentSuiteName, name, assemblyKey ) { }

		public TestSuite( Type fixtureType, int assemblyKey ) 
			: base( fixtureType.FullName, assemblyKey ) 
		{
			this.fixtureType = fixtureType;
			string uname = fixtureType.AssemblyQualifiedName;
			if ( fixtureType.Namespace != null )
				testName = FullName.Substring( FullName.LastIndexOf( '.' ) + 1 );
		}

		#endregion

		#region Public Methods
		public void Sort()
		{
			this.Tests.Sort();

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
		#endregion

		#region Properties

		public override ArrayList Tests 
		{
			get { return tests; }
		}

		public bool IsSetUp
		{
			get { return isSetUp; }
			set { isSetUp = value; }
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

		public override int CountTestCases(IFilter filter)
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
			return Run( listener, null );
		}
			
		public override TestResult Run(EventListener listener, IFilter filter)
		{
			TestSuiteResult suiteResult = new TestSuiteResult(this, Name);

			listener.SuiteStarted(this);
			long startTime = DateTime.Now.Ticks;

			if ( ShouldRun )
			{
				suiteResult.Executed = true;	
				DoOneTimeSetUp( suiteResult );

				RunAllTests( suiteResult, listener, filter );

				DoOneTimeTearDown( suiteResult );
			}
			else
				suiteResult.NotRun(this.IgnoreReason);

			long stopTime = DateTime.Now.Ticks;
			double time = ((double)(stopTime - startTime)) / (double)TimeSpan.TicksPerSecond;
			suiteResult.Time = time;

			listener.SuiteFinished(suiteResult);
			return suiteResult;
		}

		public override bool Filter(IFilter filter) 
		{
			return filter.Pass(this);
		}
		#endregion

		#region Virtual Methods
		public virtual void DoOneTimeSetUp( TestResult suiteResult )
		{
			if ( this.Parent != null && !this.Parent.IsSetUp )
			{
				Parent.DoOneTimeSetUp( suiteResult );
				needParentTearDown = true;
			}

			DoFixtureSetUp( suiteResult );
		}

		public virtual void DoFixtureSetUp( TestResult suiteResult )
		{
			isSetUp = true;
		}

		public virtual void DoOneTimeTearDown( TestResult suiteResult )
		{
			DoFixtureTearDown( suiteResult );
			
			if ( this.Parent != null  && Parent.IsSetUp && needParentTearDown )
			{
				needParentTearDown = false; // Do first in case of exception
				Parent.DoOneTimeTearDown( suiteResult );
			}
		}

		public virtual void DoFixtureTearDown( TestResult suiteResult )
		{
			isSetUp = false;
		}

		protected virtual void RunAllTests(
			TestSuiteResult suiteResult, EventListener listener, IFilter filter )
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
					else if ( test.IsExplicit && filter == null )
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
