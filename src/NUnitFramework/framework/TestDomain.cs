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

namespace NUnit.Core
{
	using System.Runtime.Remoting;
	using System.Security.Policy;
	using System.Reflection;
	using System.Collections;
	using System.Collections.Specialized;
	using System.Configuration;
	using System.IO;

	public class TestDomain
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
		/// The remote runner loaded in the test appdomain
		/// </summary>
		private RemoteTestRunner testRunner;

		#endregion

		#region Properties

		public bool IsTestLoaded
		{
			get { return testRunner != null; }
		}

		public Test Test
		{
			get { return IsTestLoaded ? testRunner.Test : null; }
		}

		public string TestName
		{
			get { return testRunner.TestName; }
			set { testRunner.TestName = value; }
		}

		#endregion

		#region Public Members

		public Test LoadAssembly( string assemblyFileName )
		{
			return LoadAssembly( assemblyFileName, null );
		}

		public Test LoadAssembly(string assemblyFileName, string testFixture)
		{
			AppDomainSetup setup = new AppDomainSetup();
			FileInfo testFile = new FileInfo( assemblyFileName );

			setup.ApplicationName = "Tests";
			setup.ShadowCopyFiles = "true";

			setup.ApplicationBase = testFile.DirectoryName;
			setup.ConfigurationFile =  testFile.FullName + ".config";

			setup.ShadowCopyDirectories = testFile.DirectoryName;
			setup.PrivateBinPath = testFile.DirectoryName;

			return LoadAssembly( setup, assemblyFileName, testFixture );
		}

		public Test LoadAssembly( AppDomainSetup setup, string assemblyName, string testFixture )
		{
			ThrowIfAlreadyLoaded();

			try
			{
				string assemblyPath = Path.GetFullPath( assemblyName );

				string domainName = string.Format( "domain-{0}", Path.GetFileName( assemblyName ) );
				domain = MakeAppDomain( domainName, setup );
				testRunner = MakeRemoteTestRunner( domain );

				if(testRunner != null)
				{
					testRunner.TestFileName = assemblyPath;
					if ( testFixture != null )
						testRunner.TestName = testFixture;
					domain.DoCallBack( new CrossAppDomainDelegate( testRunner.BuildSuite ) );
					return testRunner.Test;
				}
				else
				{
					Unload();
					return null;
				}
			}
			catch
			{
				Unload();
				throw;
			}
		}

		public Test LoadAssemblies( IList assemblies )
		{
			// TODO: Figure out a better way to do this
			string testFileName = Path.GetFullPath( (string)assemblies[0] );
			return LoadAssemblies( testFileName, assemblies );
		}

		public Test LoadAssemblies( IList assemblies, string testFixture )
		{
			// TODO: Figure out a better way to do this
			string testFileName = Path.GetFullPath( (string)assemblies[0] );
			return LoadAssemblies( testFileName, assemblies, testFixture );
		}

		public Test LoadAssemblies( string testFileName, IList assemblies )
		{
			return LoadAssemblies( testFileName, assemblies, null );
		}

		public Test LoadAssemblies( string testFileName, IList assemblies, string testFixture )
		{
			AppDomainSetup setup = new AppDomainSetup();
			FileInfo testFile = new FileInfo( testFileName );
			
			setup.ApplicationName = "Tests";
			setup.ShadowCopyFiles = "true";

			setup.ApplicationBase = testFile.DirectoryName;
			//setup.ConfigurationFile =  Path.ChangeExtension( testFile.FullName, ".config" );
			setup.ConfigurationFile =  testFile.FullName + ".config";

			string binPath = GetBinPath( assemblies );
			setup.ShadowCopyDirectories = binPath;
			setup.PrivateBinPath = binPath;

			return LoadAssemblies( setup, testFileName, assemblies, testFixture );
		}

		public Test LoadAssemblies( AppDomainSetup setup, string rootName, IList assemblies )
		{
			return LoadAssemblies( setup, rootName, assemblies, null );
		}

		public Test LoadAssemblies( AppDomainSetup setup, string rootName, IList assemblies, string testFixture )
		{
			ThrowIfAlreadyLoaded();

			try
			{
				string domainName = string.Format( "domain-{0}", Path.GetFileName( rootName ) );
				domain = MakeAppDomain( rootName, setup );
				testRunner = MakeRemoteTestRunner( domain );

				if(testRunner != null)
				{
					testRunner.TestFileName = rootName;
					testRunner.Assemblies = assemblies;
					if ( testFixture != null )
						testRunner.TestName = testFixture;
					domain.DoCallBack( new CrossAppDomainDelegate( testRunner.BuildSuite ) );
					return testRunner.Test;
				}
				else
				{
					Unload();
					return null;
				}
			}
			catch
			{
				Unload();
				throw;
			}
		}

		public TestResult Run(NUnit.Core.EventListener listener, TextWriter outStream, TextWriter errorStream )
		{
			return testRunner.Run(listener, outStream, errorStream);
		}

		public void Unload()
		{
			testRunner = null;

			if(domain != null) 
			{
				AppDomain.Unload(domain);
				DirectoryInfo cacheDir = new DirectoryInfo(cachePath);
				if(cacheDir.Exists) cacheDir.Delete(true);
			}
			domain = null;
		}

		#endregion

		#region Helper Methods

		private void ThrowIfAlreadyLoaded()
		{
			if ( domain != null || testRunner != null )
				throw new InvalidOperationException( "TestDomain already loaded" );
		}

		private AppDomain MakeAppDomain( string domainName, AppDomainSetup setup )
		{
			Evidence baseEvidence = AppDomain.CurrentDomain.Evidence;
			Evidence evidence = new Evidence(baseEvidence);

			AppDomain runnerDomain = AppDomain.CreateDomain(domainName, evidence, setup);
			
			ConfigureCachePath(runnerDomain);

			return runnerDomain;
		}

		private void ConfigureCachePath(AppDomain domain)
		{
			cachePath = String.Format(@"{0}\{1}", 
				ConfigurationSettings.AppSettings["shadowfiles.path"], DateTime.Now.Ticks);
			cachePath = Environment.ExpandEnvironmentVariables(cachePath);

			DirectoryInfo dir = new DirectoryInfo(cachePath);
			if(dir.Exists) dir.Delete(true);

			domain.SetCachePath(cachePath);

			return;
		}

		private static RemoteTestRunner MakeRemoteTestRunner( AppDomain runnerDomain )
		{
			RemoteTestRunner runner = (
				RemoteTestRunner) runnerDomain.CreateInstanceAndUnwrap(
				typeof(RemoteTestRunner).Assembly.FullName, 
				typeof(RemoteTestRunner).FullName,
				false, BindingFlags.Default,null,null,null,null,null);
			
			return runner;
		}

		public static string GetBinPath( IList assemblies )
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
						binPath = binPath + ";" + dir;
				}
			}

			return binPath;
		}

		#endregion
	}
}
