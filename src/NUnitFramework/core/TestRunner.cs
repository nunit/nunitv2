//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov
//
namespace NUnit.Core
{
	using System;
	using System.Runtime.Remoting;
	using System.Security.Policy;
	using System.Reflection;
	using System.IO;
	
	/// <summary>
	/// Summary description for TestRunner.
	/// </summary>
	public class TestRunner
	{
		private string assemblyName;
		private NUnit.Core.EventListener eventHandler;
		private AppDomain runnerDomain;
		private RemoteTestRunner runner;
		private string fullName;
		private TextWriter stdOutWriter;
		private TextWriter stdErrWriter;

		public TestRunner(string assemblyName, NUnit.Core.EventListener listener, TextWriter stdOutWriter, TextWriter stdErrWriter)
		{
			this.assemblyName = assemblyName;
			this.eventHandler = listener;
			this.stdErrWriter = stdErrWriter;
			this.stdOutWriter = stdOutWriter;
			LoadRunner();
		}

		public void LoadRunner()
		{
			FileInfo info = new FileInfo(assemblyName);
			
			AppDomainSetup setup = new AppDomainSetup();
			setup.ShadowCopyDirectories = info.DirectoryName;
			setup.ShadowCopyFiles = "true";

			Evidence baseEvidence = AppDomain.CurrentDomain.Evidence;
			Evidence evidence = new Evidence(baseEvidence);
			
			runnerDomain = AppDomain.CreateDomain("RemoteRunnerDomain", evidence, setup);
			//runnerDomain.ClearShadowCopyPath();
			ObjectHandle handle = runnerDomain.CreateInstance(
                typeof(RemoteTestRunner).Assembly.FullName, typeof(RemoteTestRunner).FullName,
				false,
				BindingFlags.Default,null,new object[]{assemblyName,eventHandler,stdOutWriter, stdErrWriter},null,null,null);
			Object runnerObject = handle.Unwrap();
			runner = (RemoteTestRunner)runnerObject;
		}

		public TestResult Run()
		{
			LoadRunner();

			//Assembly[] assemblies = runnerDomain.GetAssemblies();
			//foreach(Assembly assembly in assemblies)
			//	Console.Out.WriteLine("{0}", assembly.Location);
			runner.TestName = fullName;
			TestResult result = runner.Run();
			//Console.WriteLine("after");
			//foreach(Assembly assembly in runnerDomain.GetAssemblies())
			//	Console.Out.WriteLine("{0}", assembly.Location);
			return result;
		}

		public Test Test
		{
			get 
			{
				LoadRunner();
				return runner.Test;
			}
		}

		public string TestName 
		{
			get { return fullName; }
			set { fullName = value;}
		}
	}
}
