using System;

namespace NUnit.Core
{
	/// <summary>
	/// TestID encapsulates a unique identifier for tests, consisting of
	/// the runner identifier and a unique test key.
	/// </summary>
	[Serializable]
	public class TestID : ICloneable
	{
		#region Fields
		/// <summary>
		/// The int ID of the TestRunner that originally loaded this test.
		/// </summary>
		private int runnerID;

		/// <summary>
		/// The int key that distinguishes this test from all others created
		/// by the same runner.
		/// </summary>
		private int testKey;
		
		/// <summary>
		/// Static value to seed ids. It's started at 1000 so any
		/// uninitialized ids will stand out.
		/// </summary>
		private static int nextKey = 1000;

		#endregion

		#region Construction
		/// <summary>
		/// Construct a new TestID
		/// </summary>
		public TestID()
		{
			this.testKey = unchecked( nextKey++ );
		}
		#endregion

		#region Properties
		/// <summary>
		/// The int key that distinguishes this test from all
		/// others created by the same runner.
		/// </summary>
		public int TestKey
		{
			get { return testKey; }
		}

		/// <summary>
		/// The id of the runner that originally created a test
		/// </summary>
		public int RunnerID
		{
			get { return runnerID; }
			set { runnerID = value; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Override of Equals method to allow comparison of TestIDs
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			TestID other = obj as TestID;
			if ( other != null )
				return this.runnerID == other.runnerID && this.testKey == other.testKey;

			return base.Equals (obj);
		}

		/// <summary>
		/// Override of GetHashCode for TestIDs
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return this.testKey.GetHashCode();
		}

		public object Clone()
		{
			return this.MemberwiseClone();
		}
		#endregion
	}
}
