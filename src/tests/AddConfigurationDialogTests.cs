#region Copyright (c) 2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright  2000-2002 Philip A. Craig
'
' This software is provided 'as-is', without any express or implied warranty. In no 
' event will the authors be held liable for any damages arising from the use of this 
' software.
' 
' Permission is granted to anyone to use this software for any purpose, including 
' commercial applications, and to alter it and redistribute it freely, subject to the 
' following restrictions:
'
' 1. The origin of this software must not be misrepresented; you must not claim that 
' you wrote the original software. If you use this software in a product, an 
' acknowledgment (see the following) in the product documentation is required.
'
' Portions Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright  2000-2002 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

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
				Assert.IsNotNull(FindControl( name ), 
					string.Format( "Form {0} does not contain {1} control", form.Name, name));
			}

			public void AssertControlExists( string name, Type type )
			{
				Control control = FindControl( name );

				Assert.IsNotNull(control, string.Format( "Form {0} does not contain {1} control", form.Name, name ));
				Assert.AreEqual( type, control.GetType() );
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
			Assert.AreEqual( "", configBox.Text );
		}

		[Test]
		public void CheckComboBox()
		{
			ComboBox combo = tester.FindComboBox( "configurationComboBox" );
			dlg.Show();
			Assert.AreEqual( 3, combo.Items.Count );
			Assert.AreEqual( "<none>", combo.Items[0] );
			Assert.AreEqual( "Debug", combo.Items[1] );
			Assert.AreEqual( "Release", combo.Items[2] );
			Assert.AreEqual( "Debug", combo.SelectedItem );
		}
		[Test]
		public void TestSimpleEntry()
		{
			dlg.Show();
			TextBox config = tester.FindTextBox( "configurationNameTextBox" );
			Button okButton = tester.FindButton( "okButton" );
			config.Text = "Super";
			okButton.PerformClick();
			Assert.AreEqual( "Super", dlg.ConfigurationName );
			Assert.AreEqual( "Debug", dlg.CopyConfigurationName );
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
			Assert.AreEqual( "Super", dlg.ConfigurationName );
			Assert.IsNull( dlg.CopyConfigurationName );
		}
	}
}
