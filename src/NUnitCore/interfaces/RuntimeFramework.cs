// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org/?p=license&r=2.4.
// ****************************************************************

using System;
using System.Reflection;
using Microsoft.Win32;

namespace NUnit.Core
{
	/// <summary>
	/// Enumeration identifying a common language 
	/// runtime implementation.
	/// </summary>
	public enum RuntimeType
	{
        /// <summary>Any supported runtime framework</summary>
        Any,
		/// <summary>Microsoft .NET Framework</summary>
		Net,
		/// <summary>Microsoft .NET Compact Framework</summary>
		NetCF,
		/// <summary>Microsoft Shared Source CLI</summary>
		SSCLI,
		/// <summary>Mono</summary>
		Mono
	}

	/// <summary>
	/// RuntimeFramework represents a particular version
	/// of a common language runtime implementation.
	/// </summary>
    [Serializable]
	public sealed class RuntimeFramework
    {
        #region Static and Instance Fields
        private static RuntimeFramework currentFramework;

        private RuntimeType runtime;
		private Version version;
		private string displayName;
        #endregion

        #region Static Properties and Methods Members
        /// <summary>
        /// Static method to return a RuntimeFramework object
        /// for the framework that is currently in use.
        /// </summary>
        public static RuntimeFramework CurrentFramework
        {
            get
            {
                if (currentFramework == null)
                {
                    Type monoRuntimeType = Type.GetType("Mono.Runtime", false);
                    RuntimeType runtime = monoRuntimeType != null
                        ? RuntimeType.Mono : RuntimeType.Net;

                    if (monoRuntimeType != null)
                    {
                        currentFramework = new RuntimeFramework(RuntimeType.Mono, Environment.Version);
                        MethodInfo getDisplayNameMethod = monoRuntimeType.GetMethod(
                            "GetDisplayName", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.ExactBinding);
                        if (getDisplayNameMethod != null)
                            currentFramework.displayName = (string)getDisplayNameMethod.Invoke(null, new object[0]);
                    }
                    else
                        currentFramework = new RuntimeFramework(runtime, Environment.Version);
                }

                return currentFramework;
            }
        }

        /// <summary>
        /// Parses a string representing a RuntimeFramework.
        /// The string may be just a RuntimeType name or just
        /// a Version or a hyphentated RuntimeType-Version or
        /// a Version prefixed by 'v'.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static RuntimeFramework Parse(string s)
        {
            RuntimeType runtime;
            Version version;

            string[] parts = s.Split(new char[] { '-' });
            if (parts.Length == 2)
            {
                runtime = (RuntimeType)System.Enum.Parse(typeof(RuntimeType), parts[0], true);
                string vstring = parts[1];
                if (runtime == RuntimeType.Mono && vstring == "1.0")
                    vstring = "1.1";
                version = new Version(vstring);
            }
            else if (char.ToLower(s[0]) == 'v')
            {
                runtime = RuntimeType.Any;
                version = new Version(s.Substring(1));
            }
            else
            {
                runtime = (RuntimeType)System.Enum.Parse(typeof(RuntimeType), s, true);
                version = new Version();
            }

            return new RuntimeFramework(runtime, version);
        }
        #endregion

        #region Constructor
        /// <summary>
		/// Construct from a runtime type and version
		/// </summary>
		/// <param name="runtime">The runtime type of the framework</param>
		/// <param name="version">The version of the framework</param>
		public RuntimeFramework( RuntimeType runtime, Version version)
		{
			this.runtime = runtime;
			this.version = version;
            this.displayName = DefaultDisplayName(runtime, version);
        }
        #endregion

        #region Properties
        /// <summary>
        /// The type of this runtime framework
        /// </summary>
        public RuntimeType Runtime
        {
            get { return runtime; }
        }

        /// <summary>
        /// The version of this runtime framework
        /// </summary>
        public Version Version
        {
            get { return version; }
        }

        /// <summary>
        /// Returns the Display name for this framework
        /// </summary>
        public string DisplayName
        {
            get { return displayName; }
        }

        public bool IsAvailable
        {
            get
            {
                switch (runtime)
                {
                    case RuntimeType.Mono:
                        return CurrentFramework.Runtime == RuntimeType.Mono
                            || IsMonoInstalled();
                    case RuntimeType.Net:
                        return CurrentFramework.Matches(this)
                            || IsDotNetInstalled(this.Version);
                    default:
                        return false;
                }
            }
        }

        private static string DefaultDisplayName(RuntimeType runtime, Version version)
        {
            return runtime.ToString() + " " + version.ToString();
        }

        public bool IsMono
        {
            get { return this.runtime == RuntimeType.Mono; }
        }

        public bool IsNet
        {
            get { return this.runtime == RuntimeType.Net; }
        }

        public bool IsNetCF
        {
            get { return this.runtime == RuntimeType.NetCF; }
        }

        public bool IsSSCLI
        {
            get { return this.runtime == RuntimeType.SSCLI; }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Overridden to return the short name of the framework
        /// </summary>
        /// <returns></returns>
		public override string ToString()
		{
            string vstring = version.ToString();

            switch (runtime)
            {
                case RuntimeType.Any:
                    if (vstring != "")
                        vstring = "v" + vstring;
                    return vstring;
                case RuntimeType.Mono:
                    if (vstring == "1.1")
                        vstring = "1.0";
                    break;
                case RuntimeType.Net:
                case RuntimeType.NetCF:
                case RuntimeType.SSCLI:
                default:
                    break;
            }

            return runtime.ToString().ToLower() + "-" + vstring;
		}

        /// <summary>
        /// Returns true if the current framework matches the
        /// one supplied as an argument. Two frameworks match
        /// if their runtime types are the same or either one
        /// is RuntimeType.Any and all specified version components
        /// are equal. Negative (i.e. unspecified) version
        /// components are ignored.
        /// </summary>
        /// <param name="other">The RuntimeFramework to be matched.</param>
        /// <returns>True on match, otherwise false</returns>
        public bool Matches(RuntimeFramework other)
        {
            return (   this.Runtime == RuntimeType.Any
                    || other.Runtime == RuntimeType.Any
                    || this.Runtime == other.Runtime )
                && this.Version.Major == other.Version.Major
                && this.Version.Minor == other.Version.Minor
                && (   this.Version.Build < 0 
                    || other.Version.Build < 0 
                    || this.Version.Build == other.Version.Build ) 
                && (   this.Version.Revision < 0
                    || other.Version.Revision < 0
                    || this.Version.Revision == other.Version.Revision );
        }
        #endregion

        #region Private Methods
        private static bool IsMonoInstalled()
        {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
                return false;

            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\Novell\Mono");
            if (key == null) 
                return false;

            string version = key.GetValue("DefaultCLR") as string;
            if (version == null || version == "")
                return false;

            key = key.OpenSubKey(version);
            if (key == null)
                return false;

            return true;
        }

        private static bool IsDotNetInstalled(Version version)
        {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
                return false;

            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\.NETFramework\policy\v" + version.ToString(2));
            if ( key == null ) return false;
            
            return version.Build < 0 || key.GetValue(version.Build.ToString()) != null;
        }
        #endregion
    }
}
