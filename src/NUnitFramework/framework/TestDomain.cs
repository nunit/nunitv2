using System;

namespace NUnit.Framework
{
	using NUnit.Core;
	using System.Runtime.Remoting;
	using System.Security.Policy;
	using System.Reflection;
	using System.IO;

	/// <summary>
	/// Summary description for TestDomain.
	/// </summary>
	public class TestDomain
	{
		private string assemblyName; 
		private AppDomain domain; 
		private RemoteTestRunner testRunner;
		private TextWriter outStream;
		private TextWriter errorStream;

		public TestDomain(TextWriter outStream, TextWriter errorStream)
		{
			this.outStream = outStream;
			this.errorStream = errorStream;
		}

		private void ThrowIfAlreadyLoaded()
		{
			if ( domain != null || testRunner != null )
				throw new InvalidOperationException( "TestDomain already loaded" );
		}

		public Test Load(string assemblyFileName)
		{
			ThrowIfAlreadyLoaded();

			assemblyName = assemblyFileName; 
			FileInfo file = new FileInfo(assemblyFileName);

			try
			{
				domain = MakeAppDomain(file);
				testRunner = MakeRemoteTestRunner(file, domain);
				return testRunner.Test;
			}
			catch
			{
				Unload();
				throw;
			}
		}

		public Test Load(string testFixture, string assemblyFileName)
		{
			ThrowIfAlreadyLoaded();

			assemblyName = assemblyFileName; 
			FileInfo file = new FileInfo(assemblyFileName);

			try
			{
				domain = MakeAppDomain(file);

				testRunner = (
					RemoteTestRunner) domain.CreateInstanceAndUnwrap(
					typeof(RemoteTestRunner).Assembly.FullName, 
					typeof(RemoteTestRunner).FullName,
					false, BindingFlags.Default,null,null,null,null,null);
			
				if(testRunner != null)
				{
					testRunner.Initialize(testFixture, file.FullName);
					domain.DoCallBack(new CrossAppDomainDelegate(testRunner.BuildSuite));
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

		public string AssemblyName
		{
			get { return assemblyName; }
		}

		public string TestFixture
		{
			get { return testRunner.TestName; }
			set { testRunner.TestName = value; }
		}

		public TestResult Run(NUnit.Core.EventListener listener)
		{
			return testRunner.Run(listener, outStream, errorStream);
		}

		public void Unload()
		{
			testRunner = null;

			if(domain != null) 
				AppDomain.Unload(domain);
			domain = null;
		}

		private static AppDomain MakeAppDomain(FileInfo file)
		{
			AppDomainSetup setup = new AppDomainSetup();
			setup.ApplicationBase = file.DirectoryName;
			setup.ApplicationName = "Tests";

			setup.ShadowCopyFiles = "true";
			setup.ShadowCopyDirectories = file.DirectoryName;

			Evidence baseEvidence = AppDomain.CurrentDomain.Evidence;
			Evidence evidence = new Evidence(baseEvidence);

			string domainName = String.Format("domain-{0}", file.Name);
			AppDomain runnerDomain = AppDomain.CreateDomain(domainName, evidence, setup);
			runnerDomain.InitializeLifetimeService();
			return runnerDomain;
		}

		private static RemoteTestRunner MakeRemoteTestRunner(FileInfo file, AppDomain runnerDomain)
		{
			RemoteTestRunner runner = (
				RemoteTestRunner) runnerDomain.CreateInstanceAndUnwrap(
				typeof(RemoteTestRunner).Assembly.FullName, 
				typeof(RemoteTestRunner).FullName,
				false, BindingFlags.Default,null,null,null,null,null);
			
			if(runner != null)
			{
				runner.Initialize(file.FullName);
				runnerDomain.DoCallBack(new CrossAppDomainDelegate(runner.BuildSuite));
			}
			return runner;
		}
	}
}
