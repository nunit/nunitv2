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
	public class TestAgency : ServerBase, IService
	{
		#region Private Fields
		private AgentDataBase agentData = new AgentDataBase();

		private AgentType supportedAgentTypes = AgentType.ProcessAgent;

		private AgentType defaultAgentType = AgentType.ProcessAgent;
		#endregion

		#region Constructors
		public TestAgency() : this( "TestAgency", 9100 ) { }

		public TestAgency( string uri, int port ) : base( uri, port ) { }
		#endregion

		#region Static Property - TestAgentExePath
		public static string TestAgentExePath
		{
			get { return Path.Combine( NUnitConfiguration.NUnitDirectory, "nunit-agent.exe" ); }
            //{
            //    string agentPath = "nunit-agent.exe";

            //    if (!File.Exists(agentPath))
            //    {
            //        DirectoryInfo dir = new DirectoryInfo(Environment.CurrentDirectory);
            //        if (dir.Parent.Name == "bin")
            //            dir = dir.Parent.Parent.Parent.Parent;

            //        string path = PathUtils.Combine(dir.FullName, "NUnitTestServer", "nunit-agent-exe",
            //            "bin", NUnitConfiguration.BuildConfiguration, "nunit-agent.exe");
            //        if (File.Exists(path))
            //            agentPath = path;
            //    }

            //    return agentPath;
            //}
		}
		#endregion

		#region ServerBase Overrides
		public override void Stop()
		{
			foreach( AgentRecord r in agentData )
			{
				if ( !r.Process.HasExited )
				{
					if ( r.Agent != null )
						r.Agent.Stop();

					//r.Process.Kill();
				}
			}

			agentData.Clear();

			base.Stop ();
		}
		#endregion

		#region Public Methods - Called by Agents
		public void Register( RemoteTestAgent agent, int pid )
		{
			AgentRecord r = agentData[pid];
			if ( r == null )
				throw new ArgumentException( "Specified process is not in the agency database", "pid" );
			r.Agent = agent;
		}

		public void ReportStatus( int pid, AgentStatus status )
		{
			AgentRecord r = agentData[pid];

			if ( r == null )
				throw new ArgumentException( "Specified process is not in the agency database", "pid" );

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
            return GetAgent(type, RuntimeFramework.CurrentFramework, Timeout.Infinite);
        }

		public TestAgent GetAgent(AgentType type, RuntimeFramework framework, int waitTime)
		{
			if ( type == AgentType.Default )
				type = defaultAgentType;

			if ( (type & supportedAgentTypes) == 0 )
				throw new ArgumentException( 
					string.Format( "AgentType {0} is not supported by this agency", type ),
					"type" );

            // TODO: Decide if we should reuse agents
            //AgentRecord r = FindAvailableRemoteAgent(type);
            //if ( r == null )
            //    r = CreateRemoteAgent(type, framework, waitTime);
            AgentRecord r = CreateRemoteAgent(type, framework, waitTime);

			return new TestAgent( this, r.Process.Id, r.Agent );
		}

		public void ReleaseAgent( TestAgent agent )
		{
			AgentRecord r = agentData[agent.Id];
			if ( r == null )
				NTrace.Error( string.Format( "Unable to release agent {0} - not in database", agent.Id ) );
			else
			{
				r.Status = AgentStatus.Ready;
				NTrace.Debug( "Releasing agent " + agent.Id.ToString() );
			}
		}

		public void DestroyAgent( TestAgent agent )
		{
			AgentRecord r = agentData[agent.Id];
			if ( r != null )
			{
				if( !r.Process.HasExited )
					r.Agent.Stop();
				agentData[r.Process.Id] = null;
			}
		}
		#endregion

		#region Helper Methods
		private int LaunchAgentProcess(RuntimeFramework targetRuntime)
		{
            string agentExePath = TestAgentExePath;

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

			//ProcessStartInfo startInfo = new ProcessStartInfo( TestAgentExePath, ServerUtilities.MakeUrl( this.uri, this.port ) );
			//startInfo.CreateNoWindow = true;
			Process p = new Process();
			if ( targetRuntime.Runtime == RuntimeType.Mono )
			{
                // TODO: Replace hard-coded path
				p.StartInfo.FileName = @"C:\Program Files\Mono-2.0\bin\mono.exe";
				p.StartInfo.Arguments = agentExePath + " " + ServerUtilities.MakeUrl( this.uri, this.port );
			}
			else
			{
				p.StartInfo.FileName = agentExePath;
				p.StartInfo.Arguments = ServerUtilities.MakeUrl( this.uri, this.port );
			}
			
			//NTrace.Debug( "Launching {0}" p.StartInfo.FileName );
			p.Start();
            p.Exited += new EventHandler(OnProcessExit);
			agentData.Add( new AgentRecord( p.Id, p, null, AgentStatus.Starting ) );
			return p.Id;
		}

        private void OnProcessExit(object sender, EventArgs e)
        {
            Process p = sender as Process;
            if (p != null)
                agentData.Remove(p.Id);
        }

		private AgentRecord FindAvailableRemoteAgent(AgentType type)
		{
			foreach( AgentRecord r in agentData )
				if ( r.Status == AgentStatus.Ready)
				{
					NTrace.DebugFormat( "Reusing agent {0}", r.Id );
					r.Status = AgentStatus.Busy;
					return r;
				}

			return null;
		}

		private AgentRecord CreateRemoteAgent(AgentType type, RuntimeFramework framework, int waitTime)
		{
			int pid = LaunchAgentProcess(framework);

			NTrace.DebugFormat( "Waiting for agent {0} to register", pid );

            int pollTime = 200;
            bool infinite = waitTime == Timeout.Infinite;

			while( infinite || waitTime > 0 )
			{
				Thread.Sleep( pollTime );
				if ( !infinite ) waitTime -= pollTime;
				if ( agentData[pid].Agent != null )
				{
					NTrace.DebugFormat( "Returning new agent record {0}", pid ); 
					return agentData[pid];
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
			public int Id;
			public Process Process;
			public RemoteTestAgent Agent;
			public AgentStatus Status;

			public AgentRecord( int id, Process p, RemoteTestAgent a, AgentStatus s )
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

			public AgentRecord this[int id]
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

			public AgentRecord this[RemoteTestAgent agent]
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

            public void Remove(int agentId)
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
