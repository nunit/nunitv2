// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
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
	public class ProcessRunner : ProxyTestRunner
	{
		private Process process;

		private TestServer server;

		public ProcessRunner() : base( 0 ) { }

		public ProcessRunner( int runnerID ) : base( runnerID ) { }

		public void Start()
		{
			ProcessStartInfo startInfo = new ProcessStartInfo( GetServerPath(), "TestServer" );
			startInfo.CreateNoWindow = true;
			this.process = Process.Start( startInfo );
			System.Threading.Thread.Sleep( 1000 );

			TcpChannel channel = ServerUtilities.GetTcpChannel( "ProcessRunner", 0 );

			this.server = (TestServer)Activator.GetObject( typeof( TestServer ), "tcp://localhost:9000/TestServer" );
			this.TestRunner = server.TestRunner;
		}

		public void Stop()
		{
			//RealProxy proxy = RemotingServices.GetRealProxy( this.testRunner );
			process.Kill();
		}

		public Process Process
		{
			get { return process; }
		}

		public static string GetServerPath()
		{
			string serverPath = "nunit-server.exe";
			
			if ( !File.Exists(serverPath) )
			{
				DirectoryInfo dir = new DirectoryInfo( Environment.CurrentDirectory );
				if ( dir.Parent.Name == "bin" )
					dir = dir.Parent.Parent.Parent.Parent;
				
				string path = Path.Combine( dir.FullName, "NUnitTestServer" );
				path = Path.Combine( path, "nunit-server-exe" );
				path = Path.Combine( path, "bin" );
				path = Path.Combine( path, NUnitFramework.BuildConfiguration );
				path = Path.Combine( path, "nunit-server.exe" );
				if( File.Exists( path ) )
					serverPath = path;
			}

			return serverPath;
		}

//		public override bool Load(TestPackage package)
//		{
//			this.Start();
//
//			return base.Load (package);
//		}
//
//
//		public override void Unload()
//		{
//			base.Unload ();
//
//			this.Stop();
//		}
	}
}
