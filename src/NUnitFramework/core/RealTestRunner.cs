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
	using System.IO;
	using System.Collections;

	/// <summary>
	/// Summary description for RemoteTestRunner.
	/// </summary>
	/// 
	[Serializable]
	public class RealTestRunner : LongLivingMarshalByRefObject, TestRunner
	{
		#region Instance variables

		/// <summary>
		/// The loaded test suite
		/// </summary>
		private TestSuite suite;

		/// <summary>
		/// Saved paths of the assemblies we loaded - used to set 
		/// current directory when we are running the tests.
		/// </summary>
		private string[] assemblies;

		private EventListener listener; // Temp

		private IFilter filter;

		private bool displayTestLabels;

		/// <summary>
		/// Results from the last test run
		/// </summary>
		private TestResult[] results;

		#endregion

		#region Constructors

		/// <summary>
		/// Construct with stdOut and stdErr writers
		/// </summary>
		public RealTestRunner()
		{
			filter = EmptyFilter.Empty;
		}

		#endregion

		#region Properties

		public IList TestFrameworks
		{
			get { return TestFramework.GetLoadedFrameworks(); }
		}

		public bool DisplayTestLabels
		{
			get { return displayTestLabels; }
			set { displayTestLabels = value; }
		}

		/// <summary>
		/// Results from the last test run
		/// </summary>
		public TestResult[] Results
		{
			get { return results; }
		}

		/// <summary>
		/// First (or only) result from the last test run
		/// </summary>
		public TestResult Result
		{
			get { return results == null ? null : results[0]; }
		}

		#endregion

		#region Methods for Loading Tests

		/// <summary>
		/// Load an assembly
		/// </summary>
		/// <param name="assemblyName"></param>
		public Test Load( string assemblyName )
		{
			this.assemblies = new string[] { assemblyName };
			TestSuiteBuilder builder = new TestSuiteBuilder();
			this.suite = builder.Build( assemblyName );
			return suite;
		}

		/// <summary>
		/// Load a particular test in an assembly
		/// </summary>
		public Test Load( string assemblyName, string testName )
		{
			this.assemblies = new string[] { assemblyName };
			TestSuiteBuilder builder = new TestSuiteBuilder();
			this.suite = builder.Build( assemblyName, testName );
			return suite;
		}

		public Test Load( TestProject testProject )
		{
			this.assemblies = (string[])testProject.Assemblies.Clone();
			TestSuiteBuilder builder = new TestSuiteBuilder();
			this.suite = builder.Build( testProject );
			return suite;
		}

		/// <summary>
		/// Load a particular test from a test project
		/// </summary>
		public Test Load( TestProject project, string testName )
		{
			this.assemblies = (string[])project.Assemblies.Clone();
			TestSuiteBuilder builder = new TestSuiteBuilder();
			this.suite = builder.Build( project, testName );
			return suite;
		}

		public void Unload()
		{
			this.suite = null; // All for now
		}

		#endregion

		#region Methods for Counting TestCases

		public int CountTestCases( string testName )
		{
			Test test = FindTest( suite, testName );
			return test == null ? 0 : test.CountTestCases();
		}

		public int CountTestCases(string[] testNames ) 
		{
			int count = 0;
			foreach( string testName in testNames)
				count += CountTestCases( testName );

			return count;
		}

		public ICollection GetCategories()
		{
			return CategoryManager.Categories;
		}

		#endregion

		#region Methods for Running Tests

		public void SetFilter( IFilter filter )
		{
			if (filter == null)
				filter = EmptyFilter.Empty;
			this.filter = filter;
		}

		public virtual TestResult Run( EventListener listener )
		{
			return Run( listener, suite );
		}

		public virtual TestResult[] Run(NUnit.Core.EventListener listener, string[] testNames)
		{
			if ( testNames == null || testNames.Length == 0 )
				return new TestResult[] { Run( listener, suite ) };
			else
				return Run( listener, FindTests( suite, testNames ) );
		}

		public virtual void CancelRun()
		{
			throw new NotImplementedException();
		}

		public virtual void Wait()
		{
			// Wait isn't implemented!!!
			// throw new NotImplementedException();
		}

		#endregion

		#region Helper Routines

		/// <summary>
		/// Private method to run a single test
		/// </summary>
		private TestResult Run( EventListener listener, Test test )
		{
			// Create array with the one test
			Test[] tests = new Test[] { test };
			// Call our workhorse method
			results = Run( listener, tests );
			// Return the first result we got
			return results[0];
		}

		/// <summary>
		/// Private method to run a set of tests. This routine is the workhorse
		/// that is called anytime tests are run.
		/// </summary>
		private TestResult[] Run( EventListener listener, Test[] tests )
		{
			// Save previous state of Console. This is needed because Console.Out and
			// Console.Error are static. In the case where the test itself calls this
			// method, we can lose output if we don't save and restore their values.
			// This is exactly what happens when we are testing NUnit itself.
			TextWriter saveOut = Console.Out;
			TextWriter saveError = Console.Error;

			EventListenerTextWriter outWriter = new EventListenerTextWriter(listener, TestOutputType.Out);
			EventListenerTextWriter errorWriter = new EventListenerTextWriter(listener, TestOutputType.Error);

			// Set Console to go to our buffers. Note that any changes made by
			// the user in the test code or the code it calls will defeat this.
			Console.SetOut( outWriter );
			Console.SetError( errorWriter ); 

//			AddinManager.Addins.Save();
//			AddinManager.Addins.Clear();

			try
			{
				// Create an array for the results
				results = new TestResult[ tests.Length ];

				// Signal that we are starting the run
				this.listener = listener;
				listener.RunStarted( tests );
				
				// TODO: Get rid of count
				int count = 0;
				foreach( Test test in tests )
					count += test.CountTestCases( filter );
		
				// Run each test, saving the results
				int index = 0;
				foreach( Test test in tests )
				{
					using( new DirectorySwapper( 
						Path.GetDirectoryName( this.assemblies[test.AssemblyKey] ) ) )
					{
						EventListener flushingListener = new FlushingEventListener(listener, this.displayTestLabels, outWriter, errorWriter);
						results[index++] = test.Run( flushingListener, filter );
					}
				}

				// Signal that we are done
				listener.RunFinished( results );

				// Return result array
				return results;
			}
			catch( Exception exception )
			{
				// Signal that we finished with an exception
				listener.RunFinished( exception );
				// Rethrow - should we do this?
				throw;
			}
			finally
			{
				outWriter.Flush();
				errorWriter.Flush();

				Console.SetOut( saveOut );
				Console.SetError( saveError ); 
//				AddinManager.Addins.Restore();
			}
		}

		private Test FindTest(Test test, string fullName)
		{
			if(test.UniqueName.Equals(fullName)) return test;
			if(test.FullName.Equals(fullName)) return test;
			
			Test result = null;
			if(test is TestSuite)
			{
				TestSuite suite = (TestSuite)test;
				foreach(Test testCase in suite.Tests)
				{
					result = FindTest(testCase, fullName);
					if(result != null) break;
				}
			}

			return result;
		}

		private Test[] FindTests( Test test, string[] names )
		{
			Test[] tests = new Test[ names.Length ];

			int index = 0;
			foreach( string name in names )
				tests[index++] = FindTest( test, name );

			return tests;
		}

		#endregion

		class FlushingEventListener : ProxyEventListener
		{
			EventListener eventListener;
			bool displayTestLabels;
			TextWriter outWriter;
			TextWriter errorWriter;

			public FlushingEventListener(EventListener eventListener, bool displayTestLabels,
				TextWriter outWriter, TextWriter errorWriter) : base(eventListener)
			{
				this.eventListener = eventListener;
				this.displayTestLabels = displayTestLabels;
				this.outWriter = outWriter;
				this.errorWriter = errorWriter;
			}

			public override void TestStarted(TestCase testCase)
			{
				if ( this.displayTestLabels )
				{
					string text = string.Format("***** {0}", testCase.FullName);
					TestOutput output = new TestOutput(text, TestOutputType.Out);
					TestOutput(output);
				}
				base.TestStarted( testCase );
			}

			public override void TestFinished(TestCaseResult result)
			{
				this.outWriter.Flush();
				this.errorWriter.Flush();
				base.TestFinished (result);
			}
		}
	}
}

