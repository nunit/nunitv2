#region Copyright (c) 2002-2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright © 2000-2003 Philip A. Craig
'
' This software is provided 'as-is', without any express or implied warranty. In no 
' event will the authors be held liable for any damages arising from the use of this 
' software.
' 
' Permission is granted to anyone to use this software for any purpose, including 
' commercial applications, and to alter it and redistribute it freely, subject to the 
' following restrictions:
'
' 1. The origin of this software must not be misrepresented; you must not claim that 
' you wrote the original software. If you use this software in a product, an 
' acknowledgment (see the following) in the product documentation is required.
'
' Portions Copyright © 2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright © 2000-2003 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

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
		/// The path to our cache
		/// </summary>
		private string cachePath;
		
		/// <summary>
		/// Indicate whether files should be shadow copied
		/// </summary>
		private bool shadowCopyFiles = true;

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
				CreateDomain( package );

				this.TestRunner = MakeRemoteTestRunner( domain );

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
			this.TestRunner = null;

			if(domain != null) 
			{
				try
				{
					AppDomain.Unload(domain);
					if ( this.shadowCopyFiles )
						DeleteCacheDir( new DirectoryInfo( cachePath ) );
				}
				catch( Exception ex)
				{
					// We assume that the tests did something bad and just leave
					// the orphaned AppDomain "out there". 
					// TODO: Something useful.
					Trace.WriteLine( "Unable to unload AppDomain {0}", domain.FriendlyName );
					Trace.WriteLine( ex.ToString() );
				}
				finally
				{
					domain = null;
				}
			}
		}

		public static string GetBinPath( string[] assemblies )
		{
			ArrayList dirs = new ArrayList();
			string binPath = null;

			foreach( string path in assemblies )
			{
				string dir = Path.GetDirectoryName( Path.GetFullPath( path ) );
				if ( !dirs.Contains( dir ) )
				{
					dirs.Add( dir );

					if ( binPath == null )
						binPath = dir;
					else
						binPath = binPath + Path.PathSeparator + dir;
				}
			}

			return binPath;
		}
		#endregion

		#region Helpers Used in AppDomain Creation and Removal

		/// <summary>
		/// Construct an application domain for testing a single assembly
		/// </summary>
		/// <param name="assemblyFileName">The assembly file name</param>
		private void CreateDomain( string assemblyFileName )
		{
			FileInfo testFile = new FileInfo( assemblyFileName );
			
			string domainName = string.Format( "domain-{0}", Path.GetFileName( assemblyFileName ) );

			domain = MakeAppDomain( domainName, testFile.DirectoryName, testFile.Name + ".config", testFile.DirectoryName );
		}

		/// <summary>
		/// Construct an application domain for testing multiple assemblies
		/// </summary>
		/// <param name="testFileName">The file name of the project file</param>
		/// <param name="appBase">The application base path</param>
		/// <param name="configFile">The configuration file to use</param>
		/// <param name="binPath">The private bin path</param>
		private void CreateDomain( TestPackage package )
		{
			FileInfo testFile = new FileInfo( package.FullName );

			string appBase = package.BasePath;
			if ( appBase == null || appBase == string.Empty )
				appBase = testFile.DirectoryName;

			string configFile = package.ConfigurationFile;
			if ( configFile == null || configFile == string.Empty )
				configFile = testFile.Name + ".config";

			string binPath = package.PrivateBinPath;
			if ( binPath == null || binPath == string.Empty )
				binPath = testFile.DirectoryName;

			domain = MakeAppDomain( "domain-" + package.Name, appBase, configFile, binPath );
		}

		/// <summary>
		/// This method creates appDomains for the framework.
		/// </summary>
		/// <param name="domainName">Name of the domain</param>
		/// <param name="appBase">ApplicationBase for the domain</param>
		/// <param name="configFile">ConfigurationFile for the domain</param>
		/// <param name="binPath">PrivateBinPath for the domain</param>
		/// <returns></returns>
		private AppDomain MakeAppDomain( string domainName, string appBase, string configFile, string binPath )
		{
			Evidence baseEvidence = AppDomain.CurrentDomain.Evidence;
			Evidence evidence = new Evidence(baseEvidence);

			AppDomainSetup setup = new AppDomainSetup();

			// We always use the same application name
			setup.ApplicationName = "Tests";
			// Note that we do NOT
			// set ShadowCopyDirectories because we  rely on the default
			// setting of ApplicationBase plus PrivateBinPath

			if ( this.Settings.Contains("ShadowCopyFiles") )
				this.shadowCopyFiles = (bool)this.Settings["ShadowCopyFiles"];

			if ( this.shadowCopyFiles )
			{
				setup.ShadowCopyFiles = "true";
				setup.ShadowCopyDirectories = appBase;
				setup.CachePath = GetCachePath();
			}
			else
			{
				setup.ShadowCopyFiles = "false";
			}

			setup.ApplicationBase = appBase;
			// Note: Mono needs full path to config file...
			setup.ConfigurationFile =  Path.Combine( appBase, configFile );
			setup.PrivateBinPath = binPath;

			AppDomain runnerDomain = AppDomain.CreateDomain(domainName, evidence, setup);
			
			return runnerDomain;
		}

		/// <summary>
		/// Get the location for caching and delete any old cache info
		/// </summary>
		private string GetCachePath()
		{
			cachePath = ConfigurationSettings.AppSettings["shadowfiles.path"];
			if ( cachePath == "" || cachePath== null )
				cachePath = Path.Combine( Path.GetTempPath(), @"nunit20\ShadowCopyCache" );
			else
				cachePath = Environment.ExpandEnvironmentVariables(cachePath);
				
			cachePath = Path.Combine( cachePath, DateTime.Now.Ticks.ToString() ); 
				
			try 
			{
				DirectoryInfo dir = new DirectoryInfo(cachePath);		
				if(dir.Exists) dir.Delete(true);
			}
			catch( Exception ex)
			{
				throw new ApplicationException( 
					string.Format( "Invalid cache path: {0}",cachePath ),
					ex );
			}

			return cachePath;
		}

		/// <summary>
		/// Helper method to delete the cache dir. This method deals 
		/// with a bug that occurs when files are marked read-only
		/// and deletes each file separately in order to give better 
		/// exception information when problems occur.
		/// 
		/// TODO: This entire method is problematic. Should we be doing it?
		/// </summary>
		/// <param name="cacheDir"></param>
		private void DeleteCacheDir( DirectoryInfo cacheDir )
		{
//			Debug.WriteLine( "Modules:");
//			foreach( ProcessModule module in Process.GetCurrentProcess().Modules )
//				Debug.WriteLine( module.ModuleName );
			

			if(cacheDir.Exists)
			{
				foreach( DirectoryInfo dirInfo in cacheDir.GetDirectories() )
					DeleteCacheDir( dirInfo );

				foreach( FileInfo fileInfo in cacheDir.GetFiles() )
				{
					fileInfo.Attributes = FileAttributes.Normal;
					try 
					{
						fileInfo.Delete();
						Debug.WriteLine( "Deleted " + fileInfo.Name );
					}
					catch( Exception ex )
					{
						Debug.WriteLine( string.Format( 
							"Error deleting {0}, {1}", fileInfo.Name, ex.Message ) );
					}
				}

				cacheDir.Attributes = FileAttributes.Normal;

				try
				{
					cacheDir.Delete();
				}
				catch( Exception ex )
				{
					Debug.WriteLine( string.Format( 
						"Error deleting {0}, {1}", cacheDir.Name, ex.Message ) );
				}
			}
		}
		
		private TestRunner MakeRemoteTestRunner( AppDomain runnerDomain )
		{
			// Inject assembly resolver into remote domain to help locate our assemblies
			AssemblyResolver assemblyResolver = (AssemblyResolver)runnerDomain.CreateInstanceFromAndUnwrap(
				typeof(AssemblyResolver).Assembly.CodeBase,
				typeof(AssemblyResolver).FullName);

			// Tell resolver to use our core assembly in the test domain
			assemblyResolver.AddFile( typeof( NUnit.Core.RemoteTestRunner ).Assembly.Location );
			assemblyResolver.AddFile( typeof( NUnit.Core.AddinManager ).Assembly.Location );
			assemblyResolver.AddFile( typeof( NUnit.Core.ITest ).Assembly.Location );

			Type runnerType = typeof( RemoteTestRunner );
			object obj = runnerDomain.CreateInstanceAndUnwrap(
				runnerType.Assembly.FullName, 
				runnerType.FullName,
				false, BindingFlags.Default,null,new object[] { this.ID },null,null,null);
			
			RemoteTestRunner runner = (RemoteTestRunner) obj;

			return runner;
		}

//		private Type GetRunnerType ()
//		{
//			Type runnerType = null;
//	
//			if (threaded)
//			{
//				//runnerType = typeof(ThreadedRemoteRunner);
//				runnerType = typeof(RemoteTestRunner);
//			}
//			else
//			{
//				runnerType = typeof(RemoteTestRunner);
//			}
//			return runnerType;
//		}

		#endregion
	}
}
