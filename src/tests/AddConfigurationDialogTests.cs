using System;
using System.Windows.Forms;
using NUnit.Framework;
using NUnit.Util;
using NUnit.UiKit;

namespace NUnit.Tests.UiKit
{
	[TestFixture]
	public class AddConfigurationDialogTests
	{
		private NUnitProject project;
		private AddConfigurationDialog dlg;
		private FormTester tester;

		private class FormTester
		{
			private Form form;

			public FormTester( Form form )
			{ 
				this.form = form;
			}

			public Control FindControl( string name )
			{
				foreach( Control control in form.Controls )
				{
					if ( control.Name == name )
						return control;
				}

				return null;
			}

			public Control FindControl( string name, Type type )
			{
				Control control = FindControl( name );

				if ( control == null || control.GetType() != type )
					return null;
				
				return control;			
			}

			public void AssertControlExists( string name )
			{
				Assert.NotNull( string.Format( "Form {0} does not contain {1} control", form.Name, name ), FindControl( name ) );
			}

			public void AssertControlExists( string name, Type type )
			{
				Control control = FindControl( name );

				Assert.NotNull( string.Format( "Form {0} does not contain {1} control", form.Name, name ), control );
				Assert.Equals( type, control.GetType() );
			}

			public Button FindButton( string name )
			{
				return FindControl( name ) as Button;
			}

			public TextBox FindTextBox( string name )
			{
				return FindControl( name ) as TextBox;
			}

			public ComboBox FindComboBox( string name )
			{
				return FindControl( name ) as ComboBox;
			}

			public Label FindLabel( string name )
			{
				return FindControl( name ) as Label;
			}

			public string GetText( string name )
			{
				return FindControl(name).Text;
			}
		}

		[SetUp]
		public void SetUp()
		{
			project = new NUnitProject( "path" );
			project.Configs.Add( "Debug" );
			project.Configs.Add( "Release" );
			dlg = new AddConfigurationDialog( project );
			tester = new FormTester( dlg );
		}

		[TearDown]
		public void TearDown()
		{
			dlg.Close();
		}

		[Test]
		public void CheckForControls()
		{
			tester.AssertControlExists( "configurationNameTextBox", typeof( TextBox ) );
			tester.AssertControlExists( "configurationComboBox", typeof( ComboBox ) );
			tester.AssertControlExists( "okButton", typeof( Button ) );
			tester.AssertControlExists( "cancelButton", typeof( Button ) );
		}

		[Test]
		public void CheckTextBox()
		{
			TextBox configBox = tester.FindTextBox( "configurationNameTextBox" );
			Assert.Equals( "", configBox.Text );
		}

		[Test]
		public void CheckComboBox()
		{
			ComboBox combo = tester.FindComboBox( "configurationComboBox" );
			dlg.Show();
			Assert.Equals( 3, combo.Items.Count );
			Assert.Equals( "<none>", combo.Items[0] );
			Assert.Equals( "Debug", combo.Items[1] );
			Assert.Equals( "Release", combo.Items[2] );
			Assert.Equals( "Debug", combo.SelectedItem );
		}
		[Test]
		public void TestSimpleEntry()
		{
			dlg.Show();
			TextBox config = tester.FindTextBox( "configurationNameTextBox" );
			Button okButton = tester.FindButton( "okButton" );
			config.Text = "Super";
			okButton.PerformClick();
			Assert.Equals( "Super", dlg.ConfigurationName );
			Assert.Equals( "Debug", dlg.CopyConfigurationName );
		}

		[Test]
		public void TestComplexEntry()
		{
			dlg.Show();
			TextBox config = tester.FindTextBox( "configurationNameTextBox" );
			Button okButton = tester.FindButton( "okButton" );
			ComboBox combo = tester.FindComboBox( "configurationComboBox" );

			config.Text = "Super";
			combo.SelectedIndex = combo.FindStringExact( "<none>" );

			okButton.PerformClick();
			Assert.Equals( "Super", dlg.ConfigurationName );
			Assert.Null( dlg.CopyConfigurationName );
		}
	}
}
