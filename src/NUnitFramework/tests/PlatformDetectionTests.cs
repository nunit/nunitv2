using System;
using System.Collections;
using NUnit.Framework;

namespace NUnit.Framework.Tests
{
	/// <summary>
	/// Summary description for PlatformHelperTests.
	/// </summary>
	[TestFixture]
	public class PlatformDetectionTests
	{
		private static readonly PlatformHelper win95Helper = new PlatformHelper( 
			new OperatingSystem( PlatformID.Win32Windows , new Version( 4, 0 ) ),
			new RuntimeFramework( RuntimeType.Net, new Version( 1, 0, 4322, 0 ) ) );

		private static readonly PlatformHelper winXPHelper = new PlatformHelper( 
			new OperatingSystem( PlatformID.Win32NT , new Version( 5,1 ) ),
			new RuntimeFramework( RuntimeType.Net, new Version( 1, 0, 4322, 0 ) ) );

		private void CheckOSPlatforms( OperatingSystem os, 
			params TestPlatform[] expectedPlatforms )
		{
			CheckPlatforms( TestPlatform.Win32, TestPlatform.Unix,
				new PlatformHelper( os, RuntimeFramework.CurrentFramework ),
				expectedPlatforms );
		}

		private void CheckRuntimePlatforms( RuntimeFramework runtimeFramework, 
			params TestPlatform[] expectedPlatforms )
		{
			CheckPlatforms( TestPlatform.Net, TestPlatform.Mono20,
				new PlatformHelper( Environment.OSVersion, runtimeFramework ),
				expectedPlatforms );
		}

		private void CheckPlatforms( TestPlatform first, TestPlatform last, 
			PlatformHelper helper, TestPlatform[] expectedPlatforms )
		{
			for ( int index = (int)first; index <= (int)last; index++ )
			{
				TestPlatform testPlatform = (TestPlatform)index;
				bool shouldPass = false;

				foreach( TestPlatform platform in expectedPlatforms )
					if ( shouldPass = platform == testPlatform )
						break;

				bool didPass = helper.IsPlatformSupported( testPlatform );
				
				if ( shouldPass && !didPass )
					Assert.Fail( "Failed to detect {0}", testPlatform );
				else if ( didPass && !shouldPass )
					Assert.Fail( "False positive on {0}", testPlatform );
			}
		}

		[Test]
		public void DetectWin95()
		{
			CheckOSPlatforms( 
				new OperatingSystem( PlatformID.Win32Windows, new Version( 4, 0 ) ),
				new TestPlatform[] { TestPlatform.Win95, TestPlatform.Win32Windows, TestPlatform.Win32 } );
		}

		[Test]
		public void DetectWin98()
		{
			CheckOSPlatforms( 
				new OperatingSystem( PlatformID.Win32Windows, new Version( 4, 10 ) ),
				new TestPlatform[] { TestPlatform.Win98, TestPlatform.Win32Windows, TestPlatform.Win32 } );
		}

		[Test]
		public void DetectWinMe()
		{
			CheckOSPlatforms( 
				new OperatingSystem( PlatformID.Win32Windows, new Version( 4, 90 ) ),
				new TestPlatform[] { TestPlatform.WinMe, TestPlatform.Win32Windows, TestPlatform.Win32 } );
		}

		[Test]
		public void DetectWinCE()
		{
			CheckOSPlatforms( 
				new OperatingSystem( (PlatformID)3, new Version( 1, 0 ) ),
				new TestPlatform[] { TestPlatform.WinCE, TestPlatform.Win32 } );
		}

		[Test]
		public void DetectNT3()
		{
			CheckOSPlatforms( 
				new OperatingSystem( PlatformID.Win32NT, new Version( 3, 51 ) ),
				new TestPlatform[] { TestPlatform.NT3, TestPlatform.Win32NT, TestPlatform.Win32 } );
		}

		[Test]
		public void DetectNT4()
		{
			CheckOSPlatforms( 
				new OperatingSystem( PlatformID.Win32NT, new Version( 4, 0 ) ),
				new TestPlatform[] { TestPlatform.NT4, TestPlatform.Win32NT, TestPlatform.Win32 } );
		}

		[Test]
		public void DetectWin2K()
		{
			CheckOSPlatforms( 
				new OperatingSystem( PlatformID.Win32NT, new Version( 5, 0 ) ),
				new TestPlatform[] { TestPlatform.Win2K, TestPlatform.NT5, TestPlatform.Win32NT, TestPlatform.Win32 } );
		}

		[Test]
		public void DetectWinXP()
		{
			CheckOSPlatforms( 
				new OperatingSystem( PlatformID.Win32NT, new Version( 5, 1 ) ),
				new TestPlatform[] { TestPlatform.WinXP, TestPlatform.NT5, TestPlatform.Win32NT, TestPlatform.Win32 } );
		}

		[Test]
		public void DetectWin2003Server()
		{
			CheckOSPlatforms( 
				new OperatingSystem( PlatformID.Win32NT, new Version( 5, 2 ) ),
				new TestPlatform[] { TestPlatform.Win2003Server, TestPlatform.NT5, TestPlatform.Win32NT, TestPlatform.Win32 } );
		}

		[Test]
		public void DetectUnix()
		{
			CheckOSPlatforms( 
				new OperatingSystem( (PlatformID)128, new Version() ),
				new TestPlatform[] { TestPlatform.Unix } );
		}

		[Test]
		public void DetectNet10()
		{
			CheckRuntimePlatforms(
				new RuntimeFramework( RuntimeType.Net, new Version( 1, 0, 3705, 0 ) ),
				new TestPlatform[] { TestPlatform.Net, TestPlatform.Net10 } );
		}

		[Test]
		public void DetectNet11()
		{
			CheckRuntimePlatforms(
				new RuntimeFramework( RuntimeType.Net, new Version( 1, 1, 4322, 0 ) ),
				new TestPlatform[] { TestPlatform.Net, TestPlatform.Net11 } );
		}

		[Test]
		public void DetectNet20()
		{
			CheckRuntimePlatforms(
				new RuntimeFramework( RuntimeType.Net, new Version( 2, 0, 40607, 0 ) ),
				new TestPlatform[] { TestPlatform.Net, TestPlatform.Net20 } );
		}

		[Test]
		public void DetectNetCF()
		{
			CheckRuntimePlatforms(
				new RuntimeFramework( RuntimeType.NetCF, new Version( 1, 1, 4322, 0 ) ),
				new TestPlatform[] { TestPlatform.NetCF } );
		}

		[Test]
		public void DetectSSCLI()
		{
			CheckRuntimePlatforms(
				new RuntimeFramework( RuntimeType.Net, new Version( 1, 0, 3, 0 ) ),
				new TestPlatform[] { TestPlatform.Net, TestPlatform.Net10 } );
		}

		[Test]
		public void DetectMono10()
		{
			CheckRuntimePlatforms(
				new RuntimeFramework( RuntimeType.Mono, new Version( 1, 1, 4322, 0 ) ),
				new TestPlatform[] { TestPlatform.Mono, TestPlatform.Mono10 } );
		}

		[Test]
		public void DetectMono20()
		{
			CheckRuntimePlatforms(
				new RuntimeFramework( RuntimeType.Mono, new Version( 2, 0, 40607, 0 ) ),
				new TestPlatform[] { TestPlatform.Mono, TestPlatform.Mono20 } );
		}

		[Test]
		public void ArrayOfPlatforms()
		{
			TestPlatform[] platforms = new TestPlatform[] 
				{ TestPlatform.NT4, TestPlatform.Win2K, TestPlatform.WinXP };
			Assert.IsTrue( winXPHelper.IsPlatformSupported( platforms ) );
			Assert.IsFalse( win95Helper.IsPlatformSupported( platforms ) );
		}

		[Test]
		public void ArrayListOfPlatforms()
		{
			ArrayList platforms = new ArrayList();
			platforms.Add( TestPlatform.NT4 );
			platforms.Add( TestPlatform.WinXP );
			platforms.Add( TestPlatform.Win2K );
			Assert.IsTrue( winXPHelper.IsPlatformSupported( platforms ) );
			Assert.IsFalse( win95Helper.IsPlatformSupported( platforms ) );
		}

		[Test]
		public void ArrayOfAttributes_Include()
		{
			PlatformAttribute attr1 = new PlatformAttribute( TestPlatform.Win2K, TestPlatform.WinXP );
			PlatformAttribute attr2 = new PlatformAttribute( TestPlatform.NT4 );
			PlatformAttribute[] attrs = new PlatformAttribute[] { attr1, attr2 };
			Assert.IsTrue( winXPHelper.IsPlatformSupported( attrs ) );
			Assert.IsFalse( win95Helper.IsPlatformSupported( attrs ) );
		}

		[Test]
		public void ArrayOfAttributes_Exclude()
		{
			PlatformAttribute attr1 = new PlatformAttribute();
			attr1.ExcludeList = new TestPlatform[] { TestPlatform.Win2K, TestPlatform.WinXP };
			PlatformAttribute attr2 = new PlatformAttribute();
			attr2.Exclude = TestPlatform.NT4;
			PlatformAttribute[] attrs = new PlatformAttribute[] { attr1, attr2 };
		}

		[Test]
		public void ArrayOfAttributes_IncludeAndExclude()
		{
			PlatformAttribute attr1 = new PlatformAttribute(
				TestPlatform.Win2K, TestPlatform.WinXP, TestPlatform.NT4 );
			PlatformAttribute attr2 = new PlatformAttribute();
			attr2.Exclude = TestPlatform.Mono;
			PlatformAttribute[] attrs = new PlatformAttribute[] { attr1, attr2 };
			Assert.IsFalse( win95Helper.IsPlatformSupported( attrs ) );
			Assert.IsTrue( winXPHelper.IsPlatformSupported( attrs ) );
			attr2.Exclude = TestPlatform.Net;
			Assert.IsFalse( win95Helper.IsPlatformSupported( attrs ) );
			Assert.IsFalse( winXPHelper.IsPlatformSupported( attrs ) );
		}

	}
}
