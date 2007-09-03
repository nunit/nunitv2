using System;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace NUnit.Util
{
	/// <summary>
	/// TestAgent represents a remote runner executing in another process
	/// and communicating with NUnit by TCP.
	/// </summary>
	public class TestAgent : MarshalByRefObject, IDisposable
	{
		int runnerID;
		string managerUrl;

		public TestAgent( int runnerID) : this(runnerID, "tcp://localhost:9100/TestAgentManager") { }

		public TestAgent( int runnerID, string managerUrl )
		{
			this.runnerID = runnerID;
			this.managerUrl = managerUrl;
		}

		public int ID
		{
			get { return runnerID; }
		}

		public void Start()
		{
			//TcpChannel channel = new TcpChannel(0);
			//ChannelServices.RegisterChannel( channel );

			TestAgentManager manager = (TestAgentManager)Activator.GetObject( typeof( TestAgentManager ), managerUrl );
			manager.Register( this, runnerID );
		}


		#region IDisposable Members

		public void Dispose()
		{
			// TODO:  Add TestAgent.Dispose implementation
		}

		#endregion
	}
}
