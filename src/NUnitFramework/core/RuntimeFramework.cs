using System;

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
	public sealed class RuntimeFramework
	{
		private RuntimeType runtime;
		private Version version;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="runtime">The runtime type of the framework</param>
		/// <param name="version">The version of the framework</param>
		public RuntimeFramework( RuntimeType runtime, Version version )
		{
			this.runtime = runtime;
			this.version = version;
		}

		/// <summary>
		/// Static method to return a RuntimeFramework object
		/// for the frameowrk that is currently in use.
		/// </summary>
		public static RuntimeFramework CurrentFramework
		{
			get 
			{ 
				RuntimeType runtime = Type.GetType( "Mono.Runtime", false ) != null
					? RuntimeType.Mono : RuntimeType.Net;

				return new RuntimeFramework( runtime, Environment.Version );
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
	}
}
