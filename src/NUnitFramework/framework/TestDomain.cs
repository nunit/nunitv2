#region Copyright (c) 2002, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov
' Copyright © 2000-2002 Philip A. Craig
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
' Portions Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov 
' or Copyright © 2000-2002 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

using System;

namespace NUnit.Framework
{
	using NUnit.Core;
	using System.Runtime.Remoting;
	using System.Security.Policy;
	using System.Reflection;
	using System.Collections;
	using System.Collections.Specialized;
	using System.Configuration;
	using System.IO;

	public class TestDomain
	{
		private AppDomain domain; 
	
		private string cachePath;
		private RemoteTestRunner testRunner;
		private TextWriter outStream;
		private TextWriter errorStream;

		public TestDomain(TextWriter outStream, TextWriter errorStream)
		{
			this.outStream = outStream;
			this.errorStream = errorStream;
		}

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

		private void ThrowIfAlreadyLoaded()
		{
			if ( domain != null || testRunner != null )
				throw new InvalidOperationException( "TestDomain already loaded" );
		}

		public Test Load( string projectPath, IList assemblies )
		{
			return Load( projectPath, assemblies, null );
		}

		public Test Load( IList assemblies )
		{
			// ToDo: Figure out a better way
			return Load( (string)assemblies[0], assemblies, null );
		}

		public Test Load( IList assemblies, string testFixture )
		{
			return Load( (string)assemblies[0], assemblies, testFixture );
		}

		public Test Load( string assemblyFileName )
		{
			return Load( assemblyFileName, null, null );
		}

		public Test Load(string assemblyFileName, string testFixture)
		{
			return Load( assemblyFileName, null, testFixture );
		}

		private Test Load( string testFileName, IList assemblies, string testFixture )
		{
			ThrowIfAlreadyLoaded();

			try
			{
				string testFilePath = Path.GetFullPath( testFileName );

				domain = MakeAppDomain( testFilePath, assemblies );
				testRunner = MakeRemoteTestRunner( domain );

				if(testRunner != null)
				{
					testRunner.Initialize( testFilePath, assemblies, testFixture );
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

		public TestResult Run(NUnit.Core.EventListener listener)
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

		private AppDomain MakeAppDomain(
			string testFileName,
			IList assemblies )
		{
			FileInfo testFile = new FileInfo( testFileName );

			AppDomainSetup setup = new AppDomainSetup();
			
			setup.ApplicationBase = testFile.DirectoryName;
			setup.ApplicationName = "Tests";
			setup.ShadowCopyFiles = "true";

			if ( assemblies == null )
			{
				setup.ConfigurationFile =  testFile.FullName + ".config";

				setup.ShadowCopyDirectories = testFile.DirectoryName;
				setup.PrivateBinPath = testFile.DirectoryName;
			}
			else
			{
				setup.ConfigurationFile =  Path.ChangeExtension( testFile.FullName, ".config" );

				string binPath = GetBinPath( assemblies );
				setup.ShadowCopyDirectories = binPath;
				setup.PrivateBinPath = binPath;
			}

			Evidence baseEvidence = AppDomain.CurrentDomain.Evidence;
			Evidence evidence = new Evidence(baseEvidence);

			string domainName = string.Format( "domain-{0}", testFile.Name );
			
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
				string dir = Path.GetDirectoryName( path );
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
	}
}
