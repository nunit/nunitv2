using System;

namespace NUnit.Framework
{
	/// <summary>
	/// Enumeration of OS and CLR values that can be
	/// used with PlatformAttribute to limit test execution.
	/// </summary>
	public enum TestPlatform
	{
		/// <summary>No particular platform or unknown platform</summary>
		Empty,
		
		/// <summary>Windows - Any Version</summary>
		Win32,
		/// <summary>Win32S 32-bit layer running over 16-bit Windows</summary>
		Win32S,
		/// <summary>Windows 95, 98 or Me</summary>
		Win32Windows,
		/// <summary>Windows NT, 2000, XP or later</summary>
		Win32NT,
		/// <summary>Windows CE</summary>
		WinCE,
		/// <summary>Windows 95</summary>
		Win95,
		/// <summary>Windows 98</summary>
		Win98,
		/// <summary>Windows Me</summary>
		WinMe,
		/// <summary>Windows NT 3</summary>
		NT3,
		/// <summary>Windows NT 4</summary>
		NT4,
		/// <summary>Windows NT 4</summary>
		NT5,
		/// <summary>Windows 2000, XP and Server 2003</summary>
		Win2K,
		/// <summary>Windows XP</summary>
		WinXP,
		/// <summary>Windows Server 2003</summary>
		Win2003Server,
		/// <summary>Unix - any type</summary>
		Unix,

		/// <summary>Microsoft .NET Framework</summary>
		Net,
		/// <summary>Microsoft .NET Framework 1.0</summary>
		Net10,
		/// <summary>Microsoft .NET Framework 1.1</summary>
		Net11,
		/// <summary>Microsoft .NET Framework 2.0</summary>
		Net20,
		/// <summary>Microsoft .NET Compact Framework</summary>
		NetCF,
		/// <summary>Microsoft Shared Source CLI</summary>
		SSCLI,
		/// <summary>Mono</summary>
		Mono,
		/// <summary>Mono 1.0 Profile</summary>
		Mono10,
		/// <summary>Mono 2.0 Profile</summary>
		Mono20,
	}
}
