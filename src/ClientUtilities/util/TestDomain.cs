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
				if ( this.domain == null )
					this.domain = Services.DomainManager.CreateDomain( package );

//				if ( this.agent == null )
//					this.agent = Services.TestAgency.GetAgent(
//						AgentType.DomainAgent,
//						5000);

                if (this.agent == null)
                {
                    this.agent = DomainAgent.CreateInstance(domain);
                    this.agent.InitializeDomain(
						AppDomain.CurrentDomain.FriendlyName.StartsWith("test-domain-")
							? TraceLevel.Off : InternalTrace.Level);
                    this.agent.Start();
                }
            
				if ( this.TestRunner == null )
					this.TestRunner = this.agent.CreateRunner( this.ID );

                log.Info("Loading tests in AppDomain, see {0}.log", domain.FriendlyName);
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
                log.Info("Unloading");  // " + Path.GetFileName(Test.TestName.Name));
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
		}
		#endregion
	}
}
