using System;
using System.Text;

namespace NUnit.Framework
{
	/// <summary>
	/// PlatformAttribute is used to mark a test fixture or an
	/// individual method as applying to a particular platform only.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class|AttributeTargets.Method, AllowMultiple=true)]
	public sealed class PlatformAttribute : Attribute
	{
		private TestPlatform[] includeList;
		private TestPlatform[] excludeList;
		private PlatformHelper platformHelper = new PlatformHelper();

		public PlatformAttribute() { }

		public PlatformAttribute( params TestPlatform[] platforms )
		{
			this.IncludeList = platforms;
		}

		/// <summary>
		/// Array of TestPlatforms which must be present in
		/// order for this attribute to allow a test to run.
		/// </summary>
		public TestPlatform[] IncludeList
		{
			get { return this.includeList; }
			set { this.includeList = value; }
		}

		/// <summary>
		/// Single TestPlatform which must be present in
		/// order for this attribute to allow a test to run.
		/// </summary>
		public TestPlatform Include
		{
			get { return this.includeList[0]; }
			set { includeList = new TestPlatform[] { value }; }
		}

		public TestPlatform[] ExcludeList
		{
			get { return this.excludeList; }
			set { this.excludeList = value; }
		}

		/// <summary>
		/// Gets or sets the TestPlatform to exclude
		/// </summary>
		public TestPlatform Exclude
		{
			get { return this.excludeList[0]; }
			set { this.excludeList = new TestPlatform[] { value }; }
		}
		

		/// <summary>
		/// Indicates whether the platform we are running on
		/// satisfies the requirements of the attribute.
		/// </summary>
		/// <returns></returns>
		public bool IsPlatformSupported()
		{
			return ( includeList == null || IsPlatformIncluded() )
				&& ( excludeList == null || !IsPlatformExcluded() );
		}

		public bool IsPlatformIncluded()
		{
			return platformHelper.IsPlatformSupported( includeList );
		}

		public bool IsPlatformExcluded()
		{
			return platformHelper.IsPlatformSupported( excludeList );
		}
	}
}
