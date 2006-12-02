namespace NUnit.Core
{
	using System;
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
	public abstract class ProxyTestRunner : MarshalByRefObject, TestRunner
	{
		#region Instance Variables

		/// <summary>
		/// Our runner ID
		/// </summary>
		protected int runnerID;

		/// <summary>
		/// The downstream TestRunner
		/// </summary>
		private TestRunner testRunner;

		/// <summary>
		/// The event listener for the currently running test
		/// </summary>
		protected EventListener listener;

		/// <summary>
		/// Our dictionary of settings, used to hold any settings
		/// if the downstream TestRunner has not been set.
		/// </summary>
		private TestRunnerSettings settings;
		#endregion

		#region Construction
		public ProxyTestRunner(TestRunner testRunner)
		{
			this.testRunner = testRunner;
			this.runnerID = testRunner.ID;
			this.settings = new TestRunnerSettings();
			this.settings.Changed += new TestRunnerSettings.SettingsChangedHandler(settings_Changed);
		}

		/// <summary>
		/// Protected constructor for runners that create their own
		/// specialized downstream runner.
		/// </summary>
		protected ProxyTestRunner( int runnerID )
		{
			this.runnerID = runnerID;
			this.settings = new TestRunnerSettings();
		}
		#endregion

		#region Properties
		public virtual int ID
		{
			get { return runnerID; }
		}

		public virtual bool Running
		{
			get { return testRunner != null && testRunner.Running; }
		}

		public virtual IList AssemblyInfo
		{
			get { return this.testRunner.AssemblyInfo; }
		}

		public virtual ITest Test
		{
			get { return testRunner == null ? null : testRunner.Test; }
		}

		public virtual TestResult TestResult
		{
			get { return testRunner == null ? null : testRunner.TestResult; }
		}

		public virtual IDictionary Settings
		{
			get { return settings; }
		}

		/// <summary>
		/// Protected property copies any settings to the downstream test runner
		/// when it is set. Derived runners overriding this should call the base
		/// or copy the settings themselves.
		/// </summary>
		protected virtual TestRunner TestRunner
		{
			get { return testRunner; }
			set 
			{ 
				testRunner = value; 

				if ( testRunner != null )
					foreach( string key in settings.Keys )
						testRunner.Settings[key] = settings[key];
			}
		}
		#endregion

		#region Load and Unload Methods
		public virtual bool Load( TestPackage package )
		{
			return this.testRunner.Load( package );
		}

		public virtual void Unload()
		{
			this.testRunner.Unload();
		}
		#endregion

		#region CountTestCases
		public virtual int CountTestCases( ITestFilter filter )
		{
			return this.testRunner.CountTestCases( filter );
		}
		#endregion

		#region Methods for Running Tests
		public virtual TestResult Run(EventListener listener)
		{
			// Save active listener for derived classes
			this.listener = listener;
			return this.testRunner.Run(listener);
		}

		public virtual TestResult Run(EventListener listener, ITestFilter filter)
		{
			// Save active listener for derived classes
			this.listener = listener;
			return this.testRunner.Run(listener, filter);
		}

		public virtual void BeginRun( EventListener listener )
		{
			// Save active listener for derived classes
			this.listener = listener;
			this.testRunner.BeginRun( listener );
		}

		public virtual void BeginRun( EventListener listener, ITestFilter filter )
		{
			// Save active listener for derived classes
			this.listener = listener;
			this.testRunner.BeginRun( listener, filter );
		}

		public virtual TestResult EndRun()
		{
			return this.testRunner.EndRun();
		}

		public virtual void CancelRun()
		{
			this.testRunner.CancelRun();
		}

		public virtual void Wait()
		{
			this.testRunner.Wait();
		}
		#endregion

		#region Settings Changed Handler
		private void settings_Changed(object key, object value)
		{
			if ( this.testRunner != null )
				testRunner.Settings[key] = value;
		}
		#endregion

		#region InitializeLifetimeService Override
		public override object InitializeLifetimeService()
		{
			return null;
		}
		#endregion

	}
}
