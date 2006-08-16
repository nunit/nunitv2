using System;
using System.Collections;
using System.Collections.Specialized;

namespace NUnit.Core
{
	/// <summary>
	/// TestRunnerSettings is essentially a property bag providing 
	/// extensible settings for use by TestRunners. Each setting 
	/// is given a string as its name and may be set to any 
	/// serializable value. 
	/// </summary>
	public class TestRunnerSettings : MarshalByRefObject
	{
		private ListDictionary dictionary = new ListDictionary();

		/// <summary>
		/// Handler for changes in the settings, normally implemented by the runner
		/// </summary>
		public delegate void SettingsChangedHandler( string name, object value );

		/// <summary>
		/// Changed event used by runners to propogate changes to child runners.
		/// </summary>
		public event SettingsChangedHandler Changed;

		public TestRunnerSettings( TestRunner runner )
		{
		}

		public ICollection Keys
		{
			get { return dictionary.Keys; }
		}

		public object this[string key]
		{
			get { return dictionary[key]; }
			set 
			{ 
				dictionary[key] = value; 
			
				if ( Changed != null )
					Changed( key, value );
			}
		}

		public bool Contains( string key )
		{
			return dictionary.Contains( key );
		}
	}
}
