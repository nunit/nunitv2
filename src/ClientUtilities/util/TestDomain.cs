// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org/?p=license&r=2.4.
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

			try
			{
				if ( this.domain == null )
					this.domain = Services.DomainManager.CreateDomain( package );

//				if ( this.agent == null )
//					this.agent = Services.TestAgency.GetAgent(
//						AgentType.DomainAgent,
//						5000);

				if ( this.agent == null )
					this.agent = DomainAgent.CreateInstance( domain, InternalTrace.Level );
            
				if ( this.TestRunner == null )
					this.TestRunner = this.agent.CreateRunner( this.ID );

				return TestRunner.Load( package );
			}
			catch
			{
				Unload();
				throw;
			}
		}

		public override void Unload()
		{
            if (this.TestRunner != null)
            {
                this.TestRunner.Unload();
                this.TestRunner = null;
            }

            if (this.agent != null)
            {
                this.agent.Dispose();
                this.agent = null;
            }

			if(domain != null) 
			{
				Services.DomainManager.Unload(domain);
				domain = null;
			}
		}
		#endregion
	}
}
