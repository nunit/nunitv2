using System;
using System.Collections;
using System.Windows.Forms;
using NUnit.Framework;

namespace NUnit.TestUtilities
{
	/// <summary>
	/// Test fixtures inherit from this class in order to test
	/// Windows forms. 
	/// </summary>
	public class FormTester
	{
		// TODO: Rewrite using generics when we move to .NET 2.0

		// The form we are testing
		private Form form;

		// Various ways of looking at the form's controls
		private ControlCollection controls;
		private ButtonCollection buttons;
		private LabelCollection labels;
		private TextBoxCollection textboxes;
		private ComboBoxCollection combos;

		#region Constructor
		/// <summary>
		/// Default Constructor
		/// </summary>
		public FormTester() { }
		#endregion

		#region Properties
		/// <summary>
		/// Get and set the form to be tested
		/// </summary>
		public Form Form
		{
			get { return form; }
			set
			{ 
				form = value;
				InitCollections();
			}
		}

		private void InitCollections()
		{
			controls = new ControlCollection( form.Controls );
			
			// These will be initialized as needed
			buttons = null;
			labels = null;
			textboxes = null;
			combos = null;
		}

		/// <summary>
		/// Get our collection of all the controls on the form.
		/// </summary>
		public ControlCollection Controls
		{
			get { return controls; }
		}

		/// <summary>
		/// Get our collection of all the buttons on the form.
		/// </summary>
		public ButtonCollection Buttons
		{
			get 
			{ 
				if ( buttons == null )
					buttons = new ButtonCollection( form.Controls );

				return buttons;
			}
		}

		/// <summary>
		/// Get our collection of all the labels on the form.
		/// </summary>
		public LabelCollection Labels
		{
			get 
			{
				if (labels == null )
					labels = new LabelCollection( form.Controls );

				return labels;
			}
		}

		/// <summary>
		/// Get our collection of all the TextBoxes on the form.
		/// </summary>
		public TextBoxCollection TextBoxes
		{
			get 
			{
				if ( textboxes == null )
					textboxes = new TextBoxCollection( form.Controls );

				return textboxes;
			}
		}

		/// <summary>
		/// Get our collection of all ComboBoxes on the form
		/// </summary>
		public ComboBoxCollection Combos
		{
			get
			{
				if ( combos == null )
					combos = new ComboBoxCollection( form.Controls );

				return combos;
			}
		}
		#endregion

		#region Assertions
		/// <summary>
		/// Assert that a control with a given name exists on the form.
		/// </summary>
		/// <param name="name">The name of the control.</param>
		public void AssertControlExists( string name )
		{
			AssertControlExists( name, null );
		}
			
		public void AssertControlExists( string name, Type type )
		{
			bool gotName = false;
			System.Type gotType = null;
			foreach( Control control in form.Controls ) 
			{
				if ( control.Name == name )
				{
					gotName = true;
					if ( type == null )
						return;
					gotType = control.GetType();
					if ( type.IsAssignableFrom( gotType ) )
						return;
				}
			}

			if ( gotName )
				Assert.Fail( "Expected control {0} to be a {1} but was {2}", name, type.Name, gotType.Name );
			else
				Assert.Fail( "Form {0} does not contain {1} control", form.Name, name );
		}

		public void AssertControlsAreStackedVertically( params string[] names )
		{
			string prior = null;
			foreach( string current in names )
			{
				if ( prior != null )
				{
					if ( Controls[prior].Bottom > Controls[current].Top )
						Assert.Fail( "The {0} control should be above the {1} control", prior, current );
				}
				prior = current;
			}
		}
		#endregion

		#region Other public methods
		public string GetText( string name )
		{
			return Controls[ name ].Text;
		}
		#endregion

		#region Nested Collection Classes

		#region Enumerator used by all collections
		public class ControlEnumerator : IEnumerator
		{
			IEnumerator sourceEnum;
			System.Type typeFilter;

			public ControlEnumerator( Control.ControlCollection source, System.Type typeFilter )
			{
				this.sourceEnum = source.GetEnumerator();
				this.typeFilter = typeFilter;
			}

			#region IEnumerator Members

			public void Reset()
			{
				sourceEnum.Reset();
			}

			public object Current
			{
				get { return sourceEnum.Current; }
			}

			public bool MoveNext()
			{
				while( sourceEnum.MoveNext() )
				{
					if ( typeFilter == null || typeFilter.IsAssignableFrom( Current.GetType() ) )
						return true;
				}

				return false;
			}

			#endregion
		}
		#endregion

		#region Control Collection
		public class ControlCollection : IEnumerable
		{
			protected Control.ControlCollection source;
			private System.Type typeFilter;

			public ControlCollection( Control.ControlCollection source )
				: this( source, null ) { }

			public ControlCollection( Control.ControlCollection source, System.Type typeFilter )
			{
				this.source = source;
				this.typeFilter = typeFilter;
			}

			public Control this[string name]
			{
				get 
				{
					foreach( Control control in this )
					{				
						if ( control.Name == name )
							return control;
					}

					return null;
				}
			}

			#region IEnumerable Members

			public IEnumerator GetEnumerator()
			{
				return new ControlEnumerator( this.source, this.typeFilter );
			}
					
			#endregion
		}
		#endregion

		#region ButtonCollection
		public class ButtonCollection : ControlCollection
		{
			public ButtonCollection( Control.ControlCollection controls )
				: base( controls, typeof( Button ) ) { }

			public new Button this[string name]
			{
				get { return base[name] as Button; }
			}
		}
		#endregion

		#region LabelCollection
		public class LabelCollection : ControlCollection
		{
			public LabelCollection( Control.ControlCollection controls )
				: base( controls, typeof( Label ) ) { }

			public new Label this[string name]
			{
				get { return base[name] as Label; }
			}
		}
		#endregion

		#region TextBoxCollection
		public class TextBoxCollection : ControlCollection
		{
			public TextBoxCollection( Control.ControlCollection controls )
				: base( controls, typeof( TextBox ) ) { }

			public new TextBox this[string name]
			{
				get { return base[name] as TextBox; }
			}
		}
		#endregion

		#region ComboBoxCollection
		public class ComboBoxCollection : ControlCollection
		{
			public ComboBoxCollection( Control.ControlCollection controls )
				: base( controls, typeof( ComboBox ) ) { }

			public new ComboBox this[string name]
			{
				get { return base[name] as ComboBox; }
			}
		}
		#endregion

		#endregion
	}
}
