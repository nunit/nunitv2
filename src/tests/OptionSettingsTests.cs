using System;
using NUnit.Util;
using NUnit.Framework;

namespace NUnit.Tests
{
	/// <summary>
	/// Summary description for OptionSettingsTests.
	/// </summary>
	[TestFixture]
	public class OptionSettingsTests
	{
		private OptionSettings opts;

		[SetUp]
		public void Init()
		{
			NUnitRegistry.TestMode = true;
			NUnitRegistry.ClearTestKeys();
			opts = UserSettings.Options;
		}

		[TearDown]
		public void Cleanup()
		{
			NUnitRegistry.TestMode = false;
		}

		[Test]
		public void LoadLastProject()
		{
			Assert.Equals( true, opts.LoadLastProject );
			opts.LoadLastProject = true;
			Assert.Equals( true, opts.LoadLastProject );
			opts.LoadLastProject = false;
			Assert.Equals( false, opts.LoadLastProject );
		}

		[Test]
		public void LoadLastInitialTreeDisplay()
		{
			Assert.Equals( 0, opts.InitialTreeDisplay );
			opts.InitialTreeDisplay = 1;
			Assert.Equals( 1, opts.InitialTreeDisplay );
			opts.InitialTreeDisplay = 2;
			Assert.Equals( 2, opts.InitialTreeDisplay );
		}

		[Test]
		public void ReloadOnChange()
		{
			Assert.Equals( true, opts.ReloadOnChange );
			opts.ReloadOnChange = true;
			Assert.Equals( true, opts.ReloadOnChange );
			opts.ReloadOnChange = false;
			Assert.Equals( false, opts.ReloadOnChange );
			
		}

		[Test]
		public void ReloadOnRun()
		{
			Assert.Equals( true, opts.ReloadOnRun );
			opts.ReloadOnRun = true;
			Assert.Equals( true, opts.ReloadOnRun );
			opts.ReloadOnRun = false;
			Assert.Equals( false, opts.ReloadOnRun );
			
		}

		[Test]
		public void ClearResults()
		{
			Assert.Equals( true, opts.ClearResults );
			opts.ClearResults = true;
			Assert.Equals( true, opts.ClearResults );
			opts.ClearResults = false;
			Assert.Equals( false, opts.ClearResults );

		}

		[Test]
		public void VisualStudioSupport()
		{
			Assert.Equals( false, opts.VisualStudioSupport );
			opts.VisualStudioSupport = true;
			Assert.Equals( true, opts.VisualStudioSupport );
			opts.VisualStudioSupport = false;
			Assert.Equals( false, opts.VisualStudioSupport );
		}
	}
}
