using System;

namespace NUnit.Core
{
	/// <summary>
	/// Summary description for TestName.
	/// </summary>
	[Serializable]
	public class TestName : ICloneable
	{
		#region Fields
		/// <summary>
		/// TestID that uniquely identifies the test - may be null
		/// </summary>
		private TestID testID;

		/// <summary>
		/// The simple name of the test, without qualification
		/// </summary>
		private string name;

		/// <summary>
		/// The fully qualified name of the test
		/// </summary>
		private string fullName;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the TestID that uniquely identifies this test
		/// </summary>
		public TestID TestID
		{
			get { return testID; }
			set { testID = value; }
		}

		/// <summary>
		/// Gets the ID for the runner that created the test from
		/// the TestID, or returns -1 if the TestID is null.
		/// </summary>
		public int RunnerID
		{
			get { return testID == null ? -1 : testID.RunnerID; }
		}

		/// <summary>
		/// Gets or sets the simple name of the test
		/// </summary>
		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		/// <summary>
		/// Gets or sets the full (qualified) name of the test
		/// </summary>
		public string FullName
		{
			get { return fullName; }
			set { fullName = value; }
		}

		public string UniqueName
		{
			get
			{
				if ( this.testID == null )
					return this.fullName;
				else
					return string.Format( "[{0}-{1}]{2}", testID.RunnerID, testID.TestKey, this.fullName );
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Compares two TestNames for equality
		/// </summary>
		/// <param name="obj">the other TestID</param>
		/// <returns>True if the two TestIDs are equal</returns>
		public override bool Equals(object obj)
		{
			TestName other = obj as TestName;
			if ( other != null )
				return this.testID == other.testID && this.fullName == other.fullName;

			return base.Equals (obj);
		}

		/// <summary>
		/// Calculates a hashcode for this TestID
		/// </summary>
		/// <returns>The hash code.</returns>
		public override int GetHashCode()
		{
			return unchecked( this.testID.GetHashCode() + this.fullName.GetHashCode() );
		}

		/// <summary>
		/// Returns a duplicate of this TestName
		/// </summary>
		/// <returns></returns>
		public object Clone()
		{
			return this.MemberwiseClone();
		}

		#endregion
	}
}
