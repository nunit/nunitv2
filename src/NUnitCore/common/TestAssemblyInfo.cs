using System;
using System.Collections;
using System.Reflection;

namespace NUnit.Core
{
	/// <summary>
	/// Summary description for TestAssemblyInfo.
	/// </summary>
	[Serializable]
	public class TestAssemblyInfo
	{
		private string assemblyName;
		private Version runtimeVersion;
		private IList testFrameworks;

		public TestAssemblyInfo( Assembly assembly )
		{
			this.assemblyName = assembly.FullName;
			this.runtimeVersion = new Version( assembly.ImageRuntimeVersion.Substring(1) );
			this.testFrameworks = TestFramework.GetReferencedFrameworks( assembly );
		}

		public string Name
		{
			get { return assemblyName; }
		}

		public Version RuntimeVersion
		{
			get { return runtimeVersion; }
		}

		public IList TestFrameworks
		{
			get { return testFrameworks; }
		}
	}
}
