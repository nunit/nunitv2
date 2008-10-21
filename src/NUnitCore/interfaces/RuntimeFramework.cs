// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org/?p=license&r=2.4.
// ****************************************************************

using System;
using System.Reflection;

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

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="runtime">The runtime type of the framework</param>
		/// <param name="version">The version of the framework</param>
		public RuntimeFramework( RuntimeType runtime, Version version )
		{
			this.runtime = runtime;
			this.version = version;
			this.name = string.Format( "{0}-{1}", 
				runtime.ToString().ToLower(),
				version.ToString(2));
			if ( name == "mono-1.1" )
				name = "mono-1.0";
			this.displayName = GetDisplayName();
		}

        public RuntimeFramework(string framework)
        {
			this.name = framework;

            switch (framework)
            {
                case "net-1.0":
                    this.runtime = RuntimeType.Net;
                    this.version = new Version(1, 0, 3075);
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
        }

		public string Name
		{
			get { return name; }
		}

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
                    RuntimeType runtime = Type.GetType("Mono.Runtime", false) != null
                        ? RuntimeType.Mono : RuntimeType.Net;

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

		public string DisplayName
		{
			get { return displayName; }
		}

		/// <summary>
		/// Gets a display string for the particular framework version
		/// </summary>
		/// <returns>A string used to display the framework in use</returns>
		private string GetDisplayName()
		{
			if ( runtime == RuntimeType.Mono )
			{
				Type monoRuntimeType = Type.GetType( "Mono.Runtime", false );
				MethodInfo getDisplayNameMethod = monoRuntimeType.GetMethod(
					"GetDisplayName", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.ExactBinding );
				if ( getDisplayNameMethod != null )
					return (string)getDisplayNameMethod.Invoke( null, new object[0] );
			}

			return runtime.ToString() + " " + Version.ToString();
		}
	}
}
