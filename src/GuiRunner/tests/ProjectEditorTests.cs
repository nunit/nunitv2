// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************
using System;
using System.IO;
using System.Windows.Forms;
using NUnit.Framework;
using NUnit.Util;
using NUnit.TestUtilities;

namespace NUnit.Gui.Tests
{
	/// <summary>
	/// Summary description for ProjectEditorTests.
	/// </summary>
	[TestFixture]
	public class ProjectEditorTests : FormTester
	{
		private NUnitProject project;
		private ProjectEditor editor;

		[SetUp]
		public void CreateFixtureObjects()
		{
			project = new NUnitProject( "temp.nunit" );
			project.Configs.Add( "Debug" );
			project.Configs.Add( "Release" );

			editor = new ProjectEditor( project );
			editor.VisualStudioSupport = true;
			
			this.Form = editor;
		}

		[TearDown]
		public void Close()
		{
			editor.Close();
		}
		[Test]
		public void CheckControls()
		{
			AssertControlExists( "projectPathLabel" );
			AssertControlExists( "projectBaseTextBox" );
			AssertControlExists( "configComboBox" );
			AssertControlExists( "editConfigsButton" );
			AssertControlExists( "projectTabControl" );
			AssertControlExists( "closeButton" );
		}

		[Test]
		public void InitialFieldValues()
		{
			editor.Show();
			Assert.AreEqual( Path.GetFullPath( "temp.nunit" ), GetText( "projectPathLabel" ) );
			Assert.AreEqual( Environment.CurrentDirectory, GetText( "projectBaseTextBox" ) );
		}

		[Test, Platform(Exclude="Linux", Reason="Validate on focus change doesn't work on Mono")]
		public void SetProjectBase()
		{
			editor.Show();
			TextBox textBox = TextBoxes["projectBaseTextBox"];
			textBox.Focus();
			textBox.Text = Environment.SystemDirectory; // Guaranteed to exist
			Buttons["closeButton"].Focus();
			Assert.AreEqual( Environment.SystemDirectory, project.BasePath );
		}
	}
}
