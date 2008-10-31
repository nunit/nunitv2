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
		private string name;
		private RuntimeType runtime;
		private Version version;
		private string displayName;

        private static RuntimeFramework currentFramework;

        private static RuntimeFramework[] supportedFrameworks = new RuntimeFramework[] {
                    new RuntimeFramework("net-2.0"),
                    new RuntimeFramework("net-1.1"),
                    new RuntimeFramework("net-1.0"),
                    new RuntimeFramework("mono-2.0"),
                    new RuntimeFramework("mono-1.0") };

		/// <summary>
		/// Construct from a runtime type and version
		/// </summary>
		/// <param name="runtime">The runtime type of the framework</param>
		/// <param name="version">The version of the framework</param>
		public RuntimeFramework( RuntimeType runtime, Version version)
		{
			this.runtime = runtime;
			this.version = version;
			this.name = string.Format( "{0}-{1}", 
				runtime.ToString().ToLower(),
				version.ToString(2));
			if ( name == "mono-1.1" )
				name = "mono-1.0";
            this.displayName = DefaultDisplayName(runtime, version);
        }

        public bool IsSupported
        {
            get
            {
                return this.name == "net-2.0"
                    || this.name == "net-1.1"
                    || this.name == "net-1.0"
                    || this.name == "mono-2.0"
                    || this.name == "mono-1.0";
            }
        }

        public bool IsAvailable
        {
            get
            {
                switch (runtime)
                {
                    case RuntimeType.Mono:
                        return CurrentFramework.Runtime == RuntimeType.Mono;
                    case RuntimeType.Net:
                        return CurrentFramework.Name == this.Name
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

        /// <summary>
        /// Construct from a short name
        /// </summary>
        /// <param name="framework"></param>
        public RuntimeFramework(string framework)
        {
			this.name = framework;

            switch (framework)
            {
                case "net-1.0":
                    this.runtime = RuntimeType.Net;
                    this.version = new Version(1, 0, 3705);
                    break;
                case "net-1.1":
                    this.runtime = RuntimeType.Net;
                    this.version = new Version(1, 1, 4322);
                    break;
                case "net-2.0":
                    this.runtime = RuntimeType.Net;
                    this.version = new Version(2, 0, 50727);
                    break;
				case "mono-1.0":
					this.runtime = RuntimeType.Mono;
					this.version = new Version(1, 1, 4322);
					break;
				case "mono-2.0":
					this.runtime = RuntimeType.Mono;
					this.version = new Version(2, 0, 50727);
					break;
				default:
                    throw new InvalidOperationException("Unsupported runtime: " + framework);
            }

            this.displayName = DefaultDisplayName(runtime, version);
        }

        /// <summary>
        /// Returns the short name of this framework
        /// </summary>
		public string Name
		{
			get { return name; }
		}

        /// <summary>
        /// Overridden to return the short name of the framework
        /// </summary>
        /// <returns></returns>
		public override string ToString()
		{
			return name;
		}


		/// <summary>
		/// Static method to return a RuntimeFramework object
		/// for the frameowrk that is currently in use.
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

        private static bool IsDotNetInstalled(Version version)
        {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
                return false;

            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\.NETFramework\policy\v" + version.ToString(2));
            return key != null && key.GetValue(version.Build.ToString()) != null;
        }
	}
}
