namespace NUnit.Core
{
	using System.Collections;
	using System.IO;

	/// <summary>
	/// ProxyTestRunner is the abstract base for all TestRunner
	/// implementations that operate by controlling a downstream
	/// TestRunner. All calls are simply passed on to the
	/// TestRunner that is provided to the constructor.
	/// 
	/// In spite of its name, the class is part of core and is
	/// used in the test domain as well as the client domain.
	/// 
	/// Although the class is abstract, it has no abstract 
	/// methods specified because each implementation will
	/// need to override different methods. All methods are
	/// specified using interface syntax and the derived class
	/// must explicitly implement TestRunner in order to 
	/// redefine the selected methods.
	/// </summary>
	public abstract class ProxyTestRunner : LongLivingMarshalByRefObject, TestRunner
	{
		#region Instance Variables

		/// <summary>
		/// The downstream TestRunner
		/// </summary>
		protected TestRunner testRunner;

		#endregion

		#region Constructors

		public ProxyTestRunner(TestRunner testRunner)
		{
			this.testRunner = testRunner;
		}

		protected ProxyTestRunner() { }

		#endregion

		#region Properties

		/// <summary>
		/// Writer for standard output. Set throws if a test is running
		/// </summary>
//		public TextWriter Out
//		{
//			get { return this.testRunner.Out; }
//			set { this.testRunner.Out = value; }
//		}
//
//		/// <summary>
//		/// Writer for error output. Set throws if a test is running
//		/// </summary>
//		public TextWriter Error
//		{
//			get { return this.testRunner.Error; }
//			set { this.testRunner.Error = value; }
//		}

		public virtual bool Running
		{
			get { return testRunner != null && testRunner.Running; }
		}

		public virtual IList TestFrameworks
		{
			get { return testRunner == null ? null : testRunner.TestFrameworks; }
		}

		public virtual TestResult[] Results
		{
			get { return testRunner == null ? null : testRunner.Results; }
		}

		public virtual IFilter Filter
		{
			get { return this.testRunner.Filter; }
			set { this.testRunner.Filter = value; }
		}

		#endregion

		#region Load and Unload Methods

		public virtual Test Load(string assemblyName)
		{
			return this.testRunner.Load(assemblyName);
		}

		public virtual Test Load(string assemblyName, string testName)
		{
			return this.testRunner.Load(assemblyName, testName);
		}

		public virtual Test Load(TestProject testProject)
		{
			return this.testRunner.Load(testProject);
		}

		public virtual Test Load(TestProject testProject, string testName)
		{
			return this.testRunner.Load(testProject, testName);
		}

		public virtual void Unload()
		{
			this.testRunner.Unload();
		}

		#endregion

		#region Methods for Counting TestCases

		public virtual int CountTestCases(string testName)
		{
			return this.testRunner.CountTestCases(testName);
		}

		public virtual int CountTestCases(string[] testNames)
		{
			return this.testRunner.CountTestCases(testNames);
		}

		#endregion

		#region GetCategories Method

		public virtual ICollection GetCategories()
		{
			return this.testRunner.GetCategories();
		}

		#endregion

		#region Methods for Running Tests

		public virtual TestResult Run(EventListener listener)
		{
			TestResult[] results = doRun( listener, null );
			return results == null ? null : results[0];
		}

		public virtual TestResult[] Run(EventListener listener, string[] testNames)
		{
			return doRun(listener, testNames);
		}

		// Derived classes can change the behavior of both Run methods by
		// overriding this method.
		public virtual TestResult[] doRun( EventListener listener, string[] testNames )
		{
			return this.testRunner.Run( listener, testNames );
		}

#if STARTRUN_SUPPORT
		public virtual void StartRun( EventListener listener )
		{
			doStartRun( listener, null );
		}

		public virtual void StartRun( EventListener listener, string[] testNames )
		{
			doStartRun( listener, testNames );
		}

		public virtual void doStartRun( EventListener listener, string[] testNames )
		{
			this.testRunner.StartRun( listener, testNames );
		}
#endif
		public virtual void CancelRun()
		{
			this.testRunner.CancelRun();
		}

		public virtual void Wait()
		{
			this.testRunner.Wait();
		}

		#endregion
	}
}
