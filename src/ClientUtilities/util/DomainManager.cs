using System;
using System.IO;
using System.Collections;
using System.Text;
using System.Configuration;
using System.Diagnostics;
using System.Security.Policy;
using NUnit.Core;

namespace NUnit.Util
{
	/// <summary>
	/// The DomainManager class handles the creation and unloading
	/// of domains as needed and keeps track of all existing domains.
	/// </summary>
	public class DomainManager
	{
		#region Properties
		private string shadowCopyPath;
		public string ShadowCopyPath
		{
			get
			{
				if ( shadowCopyPath == null )
				{
					shadowCopyPath = ConfigurationSettings.AppSettings["shadowfiles.path"];
					if ( shadowCopyPath == "" || shadowCopyPath== null )
						shadowCopyPath = Path.Combine( Path.GetTempPath(), @"nunit20\ShadowCopyCache" );
					else
						shadowCopyPath = Environment.ExpandEnvironmentVariables(shadowCopyPath);
				}

				return shadowCopyPath;
			}
		}
		#endregion

		#region Create and Unload Domains
		/// <summary>
		/// Construct an application domain for running a test package
		/// </summary>
		/// <param name="package">The TestPackage to be run</param>
		public AppDomain CreateDomain( TestPackage package )
		{
			FileInfo testFile = new FileInfo( package.FullName );

			AppDomainSetup setup = new AppDomainSetup();

			// We always use the same application name
			setup.ApplicationName = "Tests";

			string appBase = package.BasePath;
			if ( appBase == null || appBase == string.Empty )
				appBase = testFile.DirectoryName;
			setup.ApplicationBase = appBase;

			string configFile = package.ConfigurationFile;
			if ( configFile == null || configFile == string.Empty )
				configFile = testFile.Name + ".config";
			// Note: Mono needs full path to config file...
			setup.ConfigurationFile =  Path.Combine( appBase, configFile );

			string binPath = package.PrivateBinPath;
//			if ( binPath == null || binPath == string.Empty )
//				binPath = testFile.DirectoryName; //GetPrivateBinPath( appBase, package.Assemblies );
			setup.PrivateBinPath = binPath;

			bool shadowCopyFiles = true;
			if ( package.Settings.Contains("ShadowCopyFiles") )
				shadowCopyFiles = (bool)package.Settings["ShadowCopyFiles"];
			if ( shadowCopyFiles )
			{
				setup.ShadowCopyFiles = "true";
				setup.ShadowCopyDirectories = appBase;
				setup.CachePath = GetCachePath();
			}
			else
			{
				setup.ShadowCopyFiles = "false";
			}

			// Note that we do NOT
			// set ShadowCopyDirectories because we  rely on the default
			// setting of ApplicationBase plus PrivateBinPath

			string domainName = "domain-" + package.Name;
			Evidence baseEvidence = AppDomain.CurrentDomain.Evidence;
			Evidence evidence = new Evidence(baseEvidence);
			AppDomain runnerDomain = AppDomain.CreateDomain(domainName, evidence, setup);

			// HACK: Only pass down our AddinRegistry one level so that tests of NUnit
			// itself start without any addins defined.
			AppDomain thisDomain = AppDomain.CurrentDomain;
			if ( !IsTestDomain( AppDomain.CurrentDomain ) )
				runnerDomain.SetData("AddinRegistry", Services.AddinRegistry);

			return runnerDomain;
		}

		public void Unload( AppDomain domain )
		{
			bool shadowCopy = domain.ShadowCopyFiles;
			string cachePath = domain.SetupInformation.CachePath;
			string domainName = domain.FriendlyName;

			try
			{
				AppDomain.Unload( domain );

				if ( shadowCopy )
					DeleteCacheDir( new DirectoryInfo( cachePath ) );
			}
			catch( Exception ex)
			{
				// We assume that the tests did something bad and just leave
				// the orphaned AppDomain "out there". 
				// TODO: Something useful.
				Trace.WriteLine( "Unable to unload AppDomain {0}", domainName );
				Trace.WriteLine( ex.ToString() );
			}
		}
		#endregion

		#region Helper Methods
		/// <summary>
		/// Get the location for caching and delete any old cache info
		/// </summary>
		private string GetCachePath()
		{
				
			string cachePath = Path.Combine( ShadowCopyPath, DateTime.Now.Ticks.ToString() ); 
				
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

		private bool IsTestDomain(AppDomain domain)
		{
			return domain.FriendlyName.StartsWith( "domain-" );
		}

		public string GetPrivateBinPath( string basePath, IList assemblies )
		{
			StringBuilder sb = new StringBuilder(200);
			ArrayList dirList = new ArrayList();

			foreach( string assembly in assemblies )
			{
				string dir = PathUtils.RelativePath( basePath, Path.GetDirectoryName( assembly ) );
				if ( dir != null && dir != "." && !dirList.Contains( dir ) )
				{
					dirList.Add( dir );
					if ( sb.Length > 0 )
						sb.Append( Path.PathSeparator );
					sb.Append( dir );
				}
			}

			return sb.Length == 0 ? null : sb.ToString();
		}
		#endregion
	}
}
