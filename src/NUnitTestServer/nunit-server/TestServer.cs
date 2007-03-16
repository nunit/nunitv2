// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************
using System;
using System.Collections;
using System.Threading;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Services;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using NUnit.Core;

namespace NUnit.TestServer
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class TestServer : DelegatingTestRunner, IDisposable, ITrackingHandler
	{
		private string uri;

		private TcpChannel channel;

		private object theLock = new object();

		public TestServer( string uri ) : this( uri, 0 ) { }

		public TestServer( string uri, int runnerID ) : base( runnerID )
		{
			this.uri = uri;
			this.TestRunner = new RemoteTestRunner();
		}

		public string URI 
		{
			get { return uri; }
		}

		public void Start()
		{
			lock( theLock )
			{
				// TODO: Use settings
				Hashtable props = new Hashtable();
				props.Add( "port", 9000 );
				props.Add( "name", "TestServer" );
				props.Add( "bindTo", "127.0.0.1" );
				this.channel = ServerUtilities.GetTcpChannel( props );

				try
				{
					ChannelServices.RegisterChannel( channel );
				}
				catch( RemotingException )
				{
					// Channel already registered
				}

				RemotingServices.Marshal( this, uri );

				TrackingServices.RegisterTrackingHandler( this );
			}
		}

		public void Stop()
		{
			Console.WriteLine( "In Stop" );
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

		#region ITrackingHandler Members

		public void DisconnectedObject(object obj)
		{
			if ( obj == this )
				this.Stop();
		}

		public void UnmarshaledObject(object obj, ObjRef or)
		{
			// TODO:  Add TestServer.UnmarshaledObject implementation
		}

		public void MarshaledObject(object obj, ObjRef or)
		{
			// TODO:  Add TestServer.MarshaledObject implementation
		}

		#endregion
	}
}
