using System;
using System.Windows.Forms;
using NUnit.Framework;
using NUnit.TestUtilities;

namespace NUnit.UiKit.Tests
{
	[TestFixture]
	public class ErrorDisplayTests : ControlTester
	{
		[TestFixtureSetUp]
		public void CreateForm()
		{
			this.Control = new ErrorDisplay();
		}

		[TestFixtureTearDown]
		public void CloseForm()
		{
			this.Control.Dispose();
		}

		[Test]
		public void ControlsExist()
		{
			AssertControlExists( "detailList", typeof( ListBox ) );
			AssertControlExists( "tabSplitter", typeof( Splitter ) );
			AssertControlExists( "stackTrace", typeof( CP.Windows.Forms.ExpandingTextBox ) );
		}

		[Test]
		public void ControlsArePositionedCorrectly()
		{
			AssertControlsAreStackedVertically( "detailList", "tabSplitter", "stackTrace" );
		}
	}
}
