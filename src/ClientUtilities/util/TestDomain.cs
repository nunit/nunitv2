// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org
// ****************************************************************

using System;

namespace NUnit.Util
{
	using System.Diagnostics;
	using System.Security.Policy;
	using System.Reflection;
	using System.Collections;
	using System.Configuration;
	using System.IO;

	using NUnit.Core;

	public class TestDomain : ProxyTestRunner, TestRunner
	{
        static Logger log = InternalTrace.GetLogger(typeof(TestDomain));

		#region Instance Variables

		/// <summary>
		/// The appdomain used  to load tests
		/// </summary>
		private AppDomain domain; 

		/// <summary>
		/// The TestAgent in the domain
		/// </summary>
		private DomainAgent agent;

        /// <summary>
        /// AssemblyResolver to handle mixed mode assembly dependency resolution in the default AppDomain.
        /// </summary>
        private AssemblyResolver assemblyResolver;

		#endregion

		#region Constructors
		public TestDomain() : base( 0 ) { }

		public TestDomain( int runnerID ) : base( runnerID ) { }
		#endregion

		#region Properties
		public AppDomain AppDomain
		{
			get { return domain; }
		}
		#endregion

		#region Loading and Unloading Tests
		public override bool Load( TestPackage package )
		{
			Unload();

            log.Info("Loading " + package.Name);
			try
			{
                // Add additional assembly resolve directories in the default AppDomain for test
                // assemblies that request them. See https://github.com/nunit/nunit/issues/681
                assemblyResolver = new AssemblyResolver();
                foreach (string assembly in package.Assemblies)
                {
                    if (TestAssemblyDirectoryResolveHelper.AssemblyNeedsResolver(assembly))
                    {
                        assemblyResolver.AddDirectory(Path.GetDirectoryName(assembly));
                    }
                }

				if ( this.domain == null )
					this.domain = Services.DomainManager.CreateDomain( package );

                if (this.agent == null)
                {
                    this.agent = DomainAgent.CreateInstance(domain);
                    this.agent.Start();
                }
            
				if ( this.TestRunner == null )
					this.TestRunner = this.agent.CreateRunner( this.ID );

                log.Info(
                    "Loading tests in AppDomain, see {0}_{1}.log", 
                    domain.FriendlyName, 
                    Process.GetCurrentProcess().Id);

				return TestRunner.Load( package );
			}
			catch
			{
                log.Error("Load failure");
				Unload();
				throw;
			}
		}

		public override void Unload()
		{
            if (this.TestRunner != null)
            {
                log.Info("Unloading");
                this.TestRunner.Unload();
                this.TestRunner = null;
            }

            if (this.agent != null)
            {
                log.Info("Stopping DomainAgent");
                this.agent.Dispose();
                this.agent = null;
            }

			if(domain != null) 
			{
                log.Info("Unloading AppDomain " + domain.FriendlyName);
				Services.DomainManager.Unload(domain);
				domain = null;
			}

            if (assemblyResolver != null)
            {
                assemblyResolver.Dispose();
                assemblyResolver = null;
            }
		}
		#endregion

        #region Running Tests
        public override void BeginRun(EventListener listener, ITestFilter filter, bool tracing, LoggingThreshold logLevel)
        {
            log.Info("BeginRun in AppDomain {0}", domain.FriendlyName);
            base.BeginRun(listener, filter, tracing, logLevel);
        }
        #endregion

        #region IDisposable Members

        public override void Dispose()
        {
            base.Dispose();

            Unload();
        }

        #endregion
    }
}
