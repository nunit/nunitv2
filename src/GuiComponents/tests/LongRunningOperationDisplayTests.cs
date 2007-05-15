using System;
using System.Windows.Forms;
using NUnit.Framework;

namespace NUnit.UiKit.Tests
{
	[TestFixture]
	public class LongRunningOperationDisplayTests : AssertionHelper
	{
		[Test]
		public void CreateDisplay()
		{
			Form form = new Form();
			LongRunningOperationDisplay display = new LongRunningOperationDisplay( form, "Loading..." );
			Expect( display.Owner, EqualTo( form ) );
			Expect( GetOperationText( display ), EqualTo( "Loading..." ) );
		}

		private string GetOperationText( Control display )
		{
			foreach( Control control in display.Controls )
				if ( control.Name == "operation" )
					return control.Text;

			return null;
		}
	}
}
