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
				this.domain = Services.DomainManager.CreateDomain( package );
                
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
				Services.DomainManager.Unload(domain);
				domain = null;
			}
		}
		#endregion

		#region MakeRemoteTestRunner Helper
		private TestRunner MakeRemoteTestRunner( AppDomain runnerDomain )
		{
			// Inject assembly resolver into remote domain to help locate our assemblies
			AssemblyResolver assemblyResolver = (AssemblyResolver)runnerDomain.CreateInstanceFromAndUnwrap(
				typeof(AssemblyResolver).Assembly.CodeBase,
				typeof(AssemblyResolver).FullName);

			// Tell resolver to use our core assemblies in the test domain
			assemblyResolver.AddFile( typeof( NUnit.Core.RemoteTestRunner ).Assembly.Location );
			assemblyResolver.AddFile( typeof( NUnit.Core.ITest ).Assembly.Location );

            // No reference to extensions, so we do it a different way
//            string moduleName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
//            string nunitDirPath = Path.GetDirectoryName(moduleName);
//            string coreExtensions = Path.Combine(nunitDirPath, "nunit.core.extensions.dll");
//			assemblyResolver.AddFile( coreExtensions );
//            //assemblyResolver.AddFiles( nunitDirPath, "*.dll" );
//
//            string addinsDirPath = Path.Combine(nunitDirPath, "addins");
//            assemblyResolver.AddFiles(addinsDirPath, "*.dll");

			Type runnerType = typeof( RemoteTestRunner );
			object obj = runnerDomain.CreateInstanceAndUnwrap(
				runnerType.Assembly.FullName, 
				runnerType.FullName,
				false, BindingFlags.Default,null,new object[] { this.ID },null,null,null);
			
			RemoteTestRunner runner = (RemoteTestRunner) obj;

			return runner;
		}
		#endregion
	}
}
