using System;
using System.Collections;

namespace NUnit.Framework
{
	public class PlatformHelper
	{
		private OperatingSystem os;
		private RuntimeFramework rt;

		/// <summary>
		/// Default constructor uses the operating system and
		/// common language runtime of the system.
		/// </summary>
		public PlatformHelper()
		{
			this.os = Environment.OSVersion;
			this.rt = RuntimeFramework.CurrentFramework;	
		}

		/// <summary>
		/// Contruct a PlatformHelper for a particular operating
		/// system and common language runtime. Used in testing.
		/// </summary>
		/// <param name="os">OperatingSystem to be used</param>
		/// <param name="framework">RuntimeFramework to be used</param>
		public PlatformHelper( OperatingSystem os, RuntimeFramework rt )
		{
			this.os = os;
			this.rt = rt;
		}

		/// <summary>
		/// Test to determine if one of a collection of platforms
		/// is being used currently.
		/// </summary>
		/// <param name="platforms"></param>
		/// <returns></returns>
		public bool IsPlatformSupported( ICollection platforms )
		{
			foreach( TestPlatform platform in platforms )
				if ( IsPlatformSupported( platform ) )
					return true;

			return false;
		}

		/// <summary>
		/// Tests to determine if the current platform is supported
		/// based on an array of platform attributes.
		/// </summary>
		/// <param name="platformAttributes"></param>
		/// <returns></returns>
		public bool IsPlatformSupported( PlatformAttribute[] platformAttributes )
		{
			ArrayList includes = new ArrayList();
			ArrayList excludes = new ArrayList();

			foreach( PlatformAttribute platformAttribute in platformAttributes )
			{
				if ( platformAttribute.IncludeList != null )
					includes.AddRange( platformAttribute.IncludeList );
				if ( platformAttribute.ExcludeList != null )
					excludes.AddRange( platformAttribute.ExcludeList );
			}

			bool supported = includes.Count == 0;
			foreach( TestPlatform platform in includes )
			{
				supported = IsPlatformSupported( platform );
				if ( supported ) break;
			}

			if ( !supported ) return false;
			if ( excludes.Count == 0 ) return supported;
						
			foreach( TestPlatform platform in excludes )
				if ( IsPlatformSupported( platform ) )
					return false;

			return true;
		}

		/// <summary>
		/// Test to determine if the a particular platform is in use
		/// </summary>
		/// <param name="platform"></param>
		/// <returns>True if the platform is in use</returns>
		public bool IsPlatformSupported( TestPlatform platform )
		{
			switch( platform )
			{
				case TestPlatform.Win32:
					return os.Platform.ToString().StartsWith( "Win" );
				case TestPlatform.Win32S:
					return os.Platform == PlatformID.Win32S;
				case TestPlatform.Win32Windows:
					return os.Platform == PlatformID.Win32Windows;
				case TestPlatform.Win32NT:
					return os.Platform == PlatformID.Win32NT;
				case TestPlatform.WinCE:
					return (int)os.Platform == 3;  // Not defined in .NET 1.0
				case TestPlatform.Win95:
					return os.Platform == PlatformID.Win32Windows && os.Version.Major == 4 && os.Version.Minor == 0;
				case TestPlatform.Win98: 
					return os.Platform == PlatformID.Win32Windows && os.Version.Major == 4 && os.Version.Minor == 10;
				case TestPlatform.WinMe:
					return os.Platform == PlatformID.Win32Windows && os.Version.Major == 4 && os.Version.Minor == 90;
				case TestPlatform.NT3:
					return os.Platform == PlatformID.Win32NT && os.Version.Major == 3;
				case TestPlatform.NT4:
					return os.Platform == PlatformID.Win32NT && os.Version.Major == 4;
				case TestPlatform.NT5:
					return os.Platform == PlatformID.Win32NT && os.Version.Major == 5;
				case TestPlatform.Win2K:
					return os.Platform == PlatformID.Win32NT && os.Version.Major == 5 && os.Version.Minor == 0;
				case TestPlatform.WinXP:
					return os.Platform == PlatformID.Win32NT && os.Version.Major == 5 && os.Version.Minor == 1;
				case TestPlatform.Win2003Server:
					return os.Platform == PlatformID.Win32NT && os.Version.Major == 5 && os.Version.Minor == 2;
				case TestPlatform.Unix:
					return (int)os.Platform == 128;  // Not defined in .NET 1.0 or 1.1
				case TestPlatform.Net:
					return rt.Runtime == RuntimeType.Net;
				case TestPlatform.Net10:
					return rt.Runtime == RuntimeType.Net && rt.Version.Major == 1 && rt.Version.Minor == 0;
				case TestPlatform.Net11:
					return rt.Runtime == RuntimeType.Net && rt.Version.Major == 1 && rt.Version.Minor == 1;
				case TestPlatform.Net20:
					return rt.Runtime == RuntimeType.Net && rt.Version.Major == 2 && rt.Version.Minor == 0;
				case TestPlatform.NetCF:
					return rt.Runtime == RuntimeType.NetCF;
				case TestPlatform.SSCLI:
					return rt.Runtime == RuntimeType.SSCLI;
				case TestPlatform.Mono:
					return rt.Runtime == RuntimeType.Mono;
				case TestPlatform.Mono10:
					return rt.Runtime == RuntimeType.Mono && rt.Version.Major == 1 && rt.Version.Minor == 1;
				case TestPlatform.Mono20:
					return rt.Runtime == RuntimeType.Mono && rt.Version.Major == 2 && rt.Version.Minor == 0;
				default:
					throw new ArgumentException( "Invalid TestPlatform argument", platform.ToString() );
			}
		}
	}
}
