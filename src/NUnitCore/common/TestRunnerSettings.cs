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
	public class TestRunnerSettings : MarshalByRefObject, IDictionary
	{
		private ListDictionary dictionary = new ListDictionary();

		/// <summary>
		/// Handler for changes in the settings, normally implemented by the runner
		/// </summary>
		public delegate void SettingsChangedHandler( object key, object value );

		/// <summary>
		/// Changed event used by runners to propogate changes to child runners.
		/// </summary>
		public event SettingsChangedHandler Changed;

		#region IDictionary Members

		public ICollection Keys
		{
			get { return dictionary.Keys; }
		}

		public object this[object key]
		{
			get { return dictionary[key]; }
			set 
			{ 
				dictionary[key] = value; 
			
				if ( Changed != null )
					Changed( key, value );
			}
		}

		public IDictionaryEnumerator GetEnumerator()
		{
			return dictionary.GetEnumerator();
		}

		public bool Contains( object key )
		{
			return dictionary.Contains( key );
		}

		public bool IsReadOnly
		{
			get { return dictionary.IsReadOnly; }
		}

		public void Remove(object key)
		{
			dictionary.Remove( key );
		}

		public void Clear()
		{
			dictionary.Clear();
		}

		public ICollection Values
		{
			get { return dictionary.Values; }
		}

		public void Add(object key, object value)
		{
			dictionary.Add( key, value );
		}

		public bool IsFixedSize
		{
			get { return dictionary.IsFixedSize; }
		}

		#endregion

		#region ICollection Members

		public bool IsSynchronized
		{
			get { return dictionary.IsSynchronized; }
		}

		public int Count
		{
			get { return dictionary.Count; }
		}

		public void CopyTo(Array array, int index)
		{
			dictionary.CopyTo( array, index );
		}

		public object SyncRoot
		{
			get { return dictionary.SyncRoot; }
		}

		#endregion

		#region IEnumerable Members

		IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return dictionary.GetEnumerator();
		}

		#endregion
	}
}
