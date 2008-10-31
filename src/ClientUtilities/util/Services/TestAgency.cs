// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Services;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using NUnit.Core;

namespace NUnit.Util
{
	/// <summary>
	/// Enumeration of agent types used to request agents
	/// </summary>
	[Flags]
	public enum AgentType
	{
		Default = 0,
		DomainAgent = 1, // NYI
		ProcessAgent = 2
	}

	/// <summary>
	/// Enumeration used to report AgentStatus
	/// </summary>
	public enum AgentStatus
	{
		Unknown,
		Starting,
		Ready,
		Busy,
		Stopping
	}

	/// <summary>
	/// The TestAgency class provides RemoteTestAgents
	/// on request and tracks their status. Agents
	/// are wrapped in an instance of the TestAgent
	/// class. Multiple agent types are supported
	/// but only one, ProcessAgent is implemented
	/// at this time.
	/// </summary>
	public class TestAgency : ServerBase, IAgency, IService
	{
		static Logger log = InternalTrace.GetLogger(typeof(TestAgency));

		#region Private Fields
		private AgentDataBase agentData = new AgentDataBase();

		private AgentType supportedAgentTypes = AgentType.ProcessAgent;

		private AgentType defaultAgentType = AgentType.ProcessAgent;
		#endregion

		#region Constructors
		public TestAgency() : this( "TestAgency", 9100 ) { }

		public TestAgency( string uri, int port ) : base( uri, port ) { }
		#endregion

		#region ServerBase Overrides
		public override void Stop()
		{
			foreach( AgentRecord r in agentData )
			{
				if ( !r.Process.HasExited )
				{
					if ( r.Agent != null )
					{
						r.Agent.Stop();
						r.Process.WaitForExit(10000);
					}

					if ( !r.Process.HasExited )
						r.Process.Kill();
				}
			}

			agentData.Clear();

			base.Stop ();
		}
		#endregion

		#region Public Methods - Called by Agents
		public void Register( TestAgent agent )
		{
			AgentRecord r = agentData[agent.Id];
			if ( r == null )
                throw new ArgumentException(
                    string.Format("Agent {0} is not in the agency database", agent.Id),
                    "agentId");
            r.Agent = agent;
		}

		public void ReportStatus( Guid agentId, AgentStatus status )
		{
			AgentRecord r = agentData[agentId];

			if ( r == null )
                throw new ArgumentException(
                    string.Format("Agent {0} is not in the agency database", agentId),
                    "agentId" );

			r.Status = status;
		}
		#endregion

		#region Public Methods - Called by Clients
		public TestAgent GetAgent()
		{
			return GetAgent( AgentType.Default, RuntimeFramework.CurrentFramework, Timeout.Infinite );
		}

		public TestAgent GetAgent( AgentType type )
		{
			return GetAgent( type, RuntimeFramework.CurrentFramework, Timeout.Infinite );
		}

        public TestAgent GetAgent(AgentType type, int waitTime)
        {
            return GetAgent(type, RuntimeFramework.CurrentFramework, waitTime);
        }

		public TestAgent GetAgent(AgentType type, RuntimeFramework framework, int waitTime)
		{
			if ( type == AgentType.Default )
				type = defaultAgentType;

            log.Info("Getting agent type {0} running under {1}", type, framework.Name);
 
			if ( (type & supportedAgentTypes) == 0 )
				throw new ArgumentException( 
					string.Format( "AgentType {0} is not supported by this agency", type ),
					"type" );

            if (!framework.IsSupported)
                throw new ArgumentException(
                    string.Format("The {0} framework is not supported", framework.Name),
                    "framework");

            if (!framework.IsAvailable)
                throw new ArgumentException(
                    string.Format("The {0} framework is not available", framework.Name),
                    "framework");

            // TODO: Decide if we should reuse agents
            //AgentRecord r = FindAvailableRemoteAgent(type);
            //if ( r == null )
            //    r = CreateRemoteAgent(type, framework, waitTime);
            AgentRecord r = CreateRemoteAgent(type, framework, waitTime);

			return r == null ? null : r.Agent;
		}

		public void ReleaseAgent( TestAgent agent )
		{
			AgentRecord r = agentData[agent.Id];
			if ( r == null )
				log.Error( string.Format( "Unable to release agent {0} - not in database", agent.Id ) );
			else
			{
				r.Status = AgentStatus.Ready;
				log.Debug( "Releasing agent " + agent.Id.ToString() );
			}
		}

        //public void DestroyAgent( ITestAgent agent )
        //{
        //    AgentRecord r = agentData[agent.Id];
        //    if ( r != null )
        //    {
        //        if( !r.Process.HasExited )
        //            r.Agent.Stop();
        //        agentData[r.Id] = null;
        //    }
        //}
		#endregion

		#region Helper Methods
		private Guid LaunchAgentProcess(RuntimeFramework targetRuntime)
		{
            string agentExePath = NUnitConfiguration.TestAgentExePath;

            // TODO: Replace adhoc code
            if (targetRuntime.Version.Major == 1 && RuntimeFramework.CurrentFramework.Version.Major == 2)
            {
                agentExePath = agentExePath
                    .Replace("2.0", "1.1")
                    .Replace("vs2008", "vs2003")
                    .Replace("vs2005", "vs2003");
            }
            else if (targetRuntime.Version.Major == 2 && RuntimeFramework.CurrentFramework.Version.Major == 1)
            {
                agentExePath = agentExePath
                    .Replace("1.1", "2.0")
                    .Replace("1.0", "2.0")
                    .Replace("vs2003", "vs2008");
            }

            log.Debug("Using nunit-agent at " + agentExePath);

			Process p = new Process();
			p.StartInfo.UseShellExecute = false;
            Guid agentId = Guid.NewGuid();
            string arglist = agentId.ToString() + " " + ServerUtilities.MakeUrl(this.uri, this.port);

            switch( targetRuntime.Name )
            {
                case "mono-1.0":
                case "mono-2.0":
                    // TODO: Replace hard-coded path
				    p.StartInfo.FileName = @"C:\Program Files\Mono-2.0\bin\mono.exe";
				    p.StartInfo.Arguments = agentExePath + " " + arglist;
                    break;
                case "net-1.0":
                    p.StartInfo.FileName = agentExePath;
					p.StartInfo.EnvironmentVariables["COMPLUS_Version"]="v1.0.3705";
                    p.StartInfo.Arguments = arglist;
                    break;
                case "net-1.1":
                case "net-2.0":
                default:
				    p.StartInfo.FileName = agentExePath;
                    p.StartInfo.Arguments = arglist;
                    break;
			}
			
            //p.Exited += new EventHandler(OnProcessExit);
            p.Start();
            log.Info("Launched Agent process {0} - see nunit-agent_{0}.log", p.Id); 

			agentData.Add( new AgentRecord( agentId, p, null, AgentStatus.Starting ) );
		    return agentId;
		}

        //private void OnProcessExit(object sender, EventArgs e)
        //{
        //    Process p = sender as Process;
        //    if (p != null)
        //        agentData.Remove(p.Id);
        //}

		private AgentRecord FindAvailableRemoteAgent(AgentType type)
		{
			foreach( AgentRecord r in agentData )
				if ( r.Status == AgentStatus.Ready)
				{
					log.Debug( "Reusing agent {0}", r.Id );
					r.Status = AgentStatus.Busy;
					return r;
				}

			return null;
		}

		private AgentRecord CreateRemoteAgent(AgentType type, RuntimeFramework framework, int waitTime)
		{
            Guid agentId = LaunchAgentProcess(framework);

			log.Debug( "Waiting for agent {0} to register", agentId.ToString("B") );

            int pollTime = 200;
            bool infinite = waitTime == Timeout.Infinite;

			while( infinite || waitTime > 0 )
			{
				Thread.Sleep( pollTime );
				if ( !infinite ) waitTime -= pollTime;
				if ( agentData[agentId].Agent != null )
				{
					log.Debug( "Returning new agent record {0}", agentId.ToString("B") ); 
					return agentData[agentId];
				}
			}

			return null;
		}
		#endregion

		#region IService Members

		public void UnloadService()
		{
			this.Stop();
		}

		public void InitializeService()
		{
			this.Start();
		}

		#endregion

		#region Nested Class - AgentRecord
		private class AgentRecord
		{
			public Guid Id;
			public Process Process;
			public TestAgent Agent;
			public AgentStatus Status;

			public AgentRecord( Guid id, Process p, TestAgent a, AgentStatus s )
			{
				this.Id = id;
				this.Process = p;
				this.Agent = a;
				this.Status = s;
			}

		}
		#endregion

		#region Nested Class - AgentDataBase
		/// <summary>
		///  A simple class that tracks data about this
		///  agencies active and available agents
		/// </summary>
		private class AgentDataBase : IEnumerable
		{
			private ListDictionary agentData = new ListDictionary();

			public AgentRecord this[Guid id]
			{
				get { return (AgentRecord)agentData[id]; }
				set
				{
					if ( value == null )
						agentData.Remove( id );
					else
						agentData[id] = value;
				}
			}

			public AgentRecord this[TestAgent agent]
			{
				get
				{
					foreach( System.Collections.DictionaryEntry entry in agentData )
					{
						AgentRecord r = (AgentRecord)entry.Value;
						if ( r.Agent == agent )
							return r;
					}

					return null;
				}
			}

			public void Add( AgentRecord r )
			{
				agentData[r.Id] = r;
			}

            public void Remove(Guid agentId)
            {
                agentData.Remove(agentId);
            }

			public void Clear()
			{
				agentData.Clear();
			}

			#region IEnumerable Members
			public IEnumerator GetEnumerator()
			{
				return new AgentDataEnumerator( agentData );
			}
			#endregion

			#region Nested Class - AgentDataEnumerator
			public class AgentDataEnumerator : IEnumerator
			{
				IEnumerator innerEnum;

				public AgentDataEnumerator( IDictionary list )
				{
					innerEnum = list.GetEnumerator();
				}

				#region IEnumerator Members
				public void Reset()
				{
					innerEnum.Reset();
				}

				public object Current
				{
					get { return ((DictionaryEntry)innerEnum.Current).Value; }
				}

				public bool MoveNext()
				{
					return innerEnum.MoveNext();
				}
				#endregion
			}
			#endregion
		}

		#endregion
	}
}
