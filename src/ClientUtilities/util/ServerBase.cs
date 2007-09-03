using System;
using System.Threading;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Services;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using NUnit.Core;

namespace NUnit.Util
{
	/// <summary>
	/// Summary description for ServerBase.
	/// </summary>
	public class ServerBase : MarshalByRefObject, IDisposable
	{
		private string uri;
		private int port;

		private TcpChannel channel;

		private object theLock = new object();

		public ServerBase(string uri, int port)
		{
			this.uri = uri;
			this.port = port;
		}
	
		public void Start()
		{
			lock( theLock )
			{
				this.channel = ServerUtilities.GetTcpChannel( uri + "Channel", port );
				RemotingServices.Marshal( this, uri );
			}
		}

		public void Stop()
		{
			lock( theLock )
			{
				RemotingServices.Disconnect( this );		
				if ( this.channel != null )
					ChannelServices.UnregisterChannel( this.channel );
				Monitor.PulseAll( theLock );
			}
		}

		public void WaitForStop()
		{
			lock( theLock )
			{
				Monitor.Wait( theLock );
			}
		}

		#region IDisposable Members

		public void Dispose()
		{
			Stop();
		}

		#endregion
	}
}
