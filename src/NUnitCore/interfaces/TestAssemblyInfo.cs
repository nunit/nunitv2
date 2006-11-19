using System;
using System.Collections;
using System.Reflection;
using System.IO;
using System.Text;

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

		public TestAssemblyInfo( string assemblyName, Version runtimeVersion, IList testFrameworks )
		{
			this.assemblyName = assemblyName;
			this.runtimeVersion = runtimeVersion;
			this.testFrameworks = testFrameworks;
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
