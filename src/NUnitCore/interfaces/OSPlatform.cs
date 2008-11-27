// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************
using System;
using System.Runtime.InteropServices;

namespace NUnit.Core
{
    public class OSPlatform
    {
        PlatformID platform;
        Version version;
        ProductType product;

        #region Static Members
        private static OSPlatform currentPlatform;

        // Defined here and used in tests. We can't use PlatformID.Unix
        // if we are building on .NET 1.0 or 1.1 and the values are different on Mono
        public static readonly PlatformID UnixPlatformID_Microsoft = (PlatformID)4;
        public static readonly PlatformID UnixPlatformID_Mono = (PlatformID)128;

        public static OSPlatform CurrentPlatform
        {
            get
            {
                if (currentPlatform == null)
                {
                    OperatingSystem os = Environment.OSVersion;

                    if (os.Platform == PlatformID.Win32NT && os.Version.Major >= 6)
                    {
                        OSVERSIONINFOEX osvi = new OSVERSIONINFOEX();
                        osvi.dwOSVersionInfoSize = (uint)Marshal.SizeOf(osvi);
                        GetVersionEx(ref osvi);
                        currentPlatform = new OSPlatform(os.Platform, os.Version, (ProductType)osvi.ProductType);
                    }
                    else
                        currentPlatform = new OSPlatform(os.Platform, os.Version);
                }

                return currentPlatform;
            }
        }
        #endregion

        #region Members used for Win32NT platform only
        public enum ProductType
        {
            Unknown,
            WorkStation,
            DomainController,
            Server,
        }

        [StructLayout(LayoutKind.Sequential)]
        struct OSVERSIONINFOEX
        {
            public uint dwOSVersionInfoSize;
            public uint dwMajorVersion;
            public uint dwMinorVersion;
            public uint dwBuildNumber;
            public uint dwPlatformId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szCSDVersion;
            public Int16 wServicePackMajor;
            public Int16 wServicePackMinor;
            public Int16 wSuiteMask;
            public Byte ProductType;
            public Byte Reserved;
        }

        [DllImport("Kernel32.dll")]
        private static extern bool GetVersionEx(ref OSVERSIONINFOEX osvi);
        #endregion

        public OSPlatform(PlatformID platform, Version version)
        {
            this.platform = platform;
            this.version = version;
        }

        public OSPlatform(PlatformID platform, Version version, ProductType product)
            : this( platform, version )
        {
            this.product = product;
        }

        public PlatformID Platform
        {
            get { return platform; }
        }

        public Version Version
        {
            get { return version; }
        }

        public ProductType Product
        {
            get { return product; }
        }

        public bool IsWindows
        {
            get
            {
                return platform == PlatformID.Win32NT
                    || platform == PlatformID.Win32Windows
                    || platform == PlatformID.Win32S
                    || platform == PlatformID.WinCE;
            }
        }

        public bool IsUnix
        {
            get
            {
                return platform == UnixPlatformID_Microsoft
                    || platform == UnixPlatformID_Mono;
            }
        }

        public bool IsWin32S
        {
            get { return platform == PlatformID.Win32S; }
        }

        public bool IsWin32Windows
        {
            get { return platform == PlatformID.Win32Windows; }
        }

        public bool IsWin32NT
        {
            get { return platform == PlatformID.Win32NT; }
        }

        public bool IsWinCE
        {
            get { return (int)platform == 3; } // PlatformID.WinCE not defined in .NET 1.0
        }

        public bool IsWin95
        {
            get { return platform == PlatformID.Win32Windows && version.Major == 4 && version.Minor == 0; }
        }

        public bool IsWin98
        {
            get { return platform == PlatformID.Win32Windows && version.Major == 4 && version.Minor == 10; }
        }

        public bool IsWinME
        {
            get { return platform == PlatformID.Win32Windows && version.Major == 4 && version.Minor == 90; }
        }

        public bool IsNT3
        {
            get { return platform == PlatformID.Win32NT && version.Major == 3; }
        }

        public bool IsNT4
        {
            get { return platform == PlatformID.Win32NT && version.Major == 4; }
        }

        public bool IsNT5
        {
            get { return platform == PlatformID.Win32NT && version.Major == 5; }
        }

        public bool IsWin2K
        {
            get { return IsNT5 && version.Minor == 0; }
        }

        public bool IsWinXP
        {
            get { return IsNT5 && version.Minor == 1; }
        }

        public bool IsWin2003Server
        {
            get { return IsNT5 && version.Minor == 2; }
        }

        public bool IsNT6
        {
            get { return platform == PlatformID.Win32NT && version.Major == 6; }
        }

        public bool IsVista
        {
            get { return IsNT6 && version.Minor == 0 && Product == ProductType.WorkStation; }
        }

        public bool IsWin2008Server
        {
            get { return IsNT6 && version.Minor == 0 && Product == ProductType.Server; }
        }
    }
}
