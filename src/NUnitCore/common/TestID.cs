using System;

namespace NUnit.Core
{
	/// <summary>
	/// TestID encapsulates a unique identifier for tests
	/// </summary>
	[Serializable]
	public class TestID
	{
		/// <summary>
		/// Static value to seed ids. It's started at 1000 so any
		/// uninitialized ids will stand out.
		/// </summary>
		private static int nextID = 1000;

		/// <summary>
		/// The id of this particular test id
		/// </summary>
		private int id;

		public TestID()
		{
			this.id = unchecked( nextID++ );
		}

		public override string ToString()
		{
			return id.ToString();
		}

		public override bool Equals(object obj)
		{
			TestID other = obj as TestID;
			if ( other != null )
				return this.id == other.id;

			return base.Equals (obj);
		}

		public override int GetHashCode()
		{
			return this.id.GetHashCode();
		}

		public int ID
		{
			get { return id; }
		}
	}
}
