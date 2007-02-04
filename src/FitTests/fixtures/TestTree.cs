using System;

namespace NUnit.Fixtures
{
	/// <summary>
	/// Summary description for Tree.
	/// </summary>
	public class TestTree
	{
		string display;
		string signature;

		public static TestTree Parse( string display )
		{
			return new TestTree( display );
		}

		public TestTree( string display )
		{
			this.display = display;
			this.signature = display.Trim().Replace( Environment.NewLine, "+" ).Replace( " ", "+" );
		}

		public override string ToString()
		{
			return this.display;
		}

		public override bool Equals(object obj)
		{
			bool ok = obj is TestTree && ((TestTree)obj).signature == this.signature;
//			System.Diagnostics.Debug.Assert( ok );
			return ok;
		}

		public override int GetHashCode()
		{
			return signature.GetHashCode ();
		}



	}
}
