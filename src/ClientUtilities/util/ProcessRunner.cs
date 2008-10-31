// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

using System;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting.Services;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using NUnit.Core;

namespace NUnit.Util
{
	/// <summary>
	/// Summary description for ProcessRunner.
	/// </summary>
	public class ProcessRunner : ProxyTestRunner, IDisposable
	{
        static Logger log = InternalTrace.GetLogger(typeof(ProcessRunner));

		private TestAgent agent;

		#region Constructors
		public ProcessRunner() : base( 0 ) { }

		public ProcessRunner( int runnerID ) : base( runnerID ) { }
		#endregion

		public override bool Load(TestPackage package)
		{
            log.Info("Loading " + package.Name);
			Unload();

            string targetRuntime = package.Settings["RuntimeFramework"] as string;

            RuntimeFramework runtimeFramework = targetRuntime == null
                ? RuntimeFramework.CurrentFramework
                : new RuntimeFramework(targetRuntime);

			try
			{
				if (this.agent == null)
					this.agent = Services.TestAgency.GetAgent( 
						AgentType.ProcessAgent, 
						runtimeFramework, 
						20000 );
		
				if (this.agent == null)
					return false;
	
				if ( this.TestRunner == null )
					this.TestRunner = agent.CreateRunner(this.runnerID);

				return base.Load (package);
			}
			catch
			{
				Unload();
				throw;
			}
		}

        public override void Unload()
        {
            if (TestRunner != null)
            {
                log.Info("Unloading " + Path.GetFileName(Test.TestName.Name));
                this.TestRunner.Unload();
                this.TestRunner = null;
            }

            if (this.agent != null)
            {
                log.Info("Stopping remote agent");
                agent.Stop();
                this.agent = null;
            }
		}

		#region IDisposable Members
		public void Dispose()
		{
			Unload();
		}
		#endregion
	}
}
