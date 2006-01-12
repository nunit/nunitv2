using System;

namespace NUnit.Core
{
	/// <summary>
	/// TestID encapsulates a unique identifier for tests, consisting of
	/// the runner identifier and a unique test key.
	/// </summary>
	[Serializable]
	public class TestID
	{
		private int runnerID;
		private int testKey;
		/// <summary>
		/// Static value to seed ids. It's started at 1000 so any
		/// uninitialized ids will stand out.
		/// </summary>
		private static int nextKey = 1000;

		public static int GetNextTestKey()
		{
			return unchecked( nextKey++ );
		}

		public TestID() : this( 0 ) { }

		public TestID( int runnerID ) 
		{ 
			this.runnerID = runnerID;
			this.testKey = GetNextTestKey();
		}

		public TestID( TestID other )
		{
			this.runnerID = other.runnerID;
			this.testKey = other.testKey;
		}

		public int TestKey
		{
			get { return testKey; }
		}

		public int RunnerID
		{
			get { return runnerID; }
			set { runnerID = value; }
		}

		public override bool Equals(object obj)
		{
			TestID other = obj as TestID;
			if ( other != null )
				return this.runnerID == other.runnerID && this.testKey == other.testKey;

			return base.Equals (obj);
		}

		public override int GetHashCode()
		{
			return this.testKey.GetHashCode();
		}

		public TestID Clone()
		{
			return new TestID( this );
		}
	}
}
