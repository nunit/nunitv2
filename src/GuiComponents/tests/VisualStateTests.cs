using System;
using System.IO;
using NUnit.Framework;

namespace NUnit.UiKit.Tests
{
	/// <summary>
	/// Summary description for VisualStateTests.
	/// </summary>
	[TestFixture]
	public class VisualStateTests
	{
		[Test]
		public void SaveAndRestoreVisualState()
		{
			VisualState state = new VisualState();
			state.ShowCheckBoxes = true;
			state.TopNode = "ABC.Test.dll";
			state.SelectedNode = "NUnit.Tests.MyFixture.MyTest";

			StringWriter writer = new StringWriter();
			state.Save( writer );

			string output = writer.GetStringBuilder().ToString();
			Console.WriteLine( output );

			StringReader reader = new StringReader( output );
			VisualState newState = VisualState.LoadFrom( reader );

			Assert.AreEqual( state.ShowCheckBoxes, newState.ShowCheckBoxes, "ShowCheckBoxes" );
			Assert.AreEqual( state.TopNode, newState.TopNode, "TopNode" );
			Assert.AreEqual( state.SelectedNode, newState.SelectedNode, "SelectedNode" );
		}
	}
}
