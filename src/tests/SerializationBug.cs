
namespace NUnit.Tests
{
	using System;
	using System.Runtime.Serialization;
	using System.IO;
	using System.Runtime.Serialization.Formatters.Binary;
	using NUnit.Framework;

	/// <summary>
	/// Summary description for SerializationBug.
	/// </summary>
	/// 
	[TestFixture]
	public class SerializationBug
	{
		[Serializable] 
		public class Perob 
		{ 
			public int i; 
			public int j; 
			public Perob(int ii,int jj) 
			{ 
				// 
				// TODO: Add constructor logic here 
				// 
				i = ii; 
				j = jj; 
			} 
			void SurpressWarning() 
			{ // just gets rid of the "unused variable" compiler warnings... 
				j = i; 
				i = j; 
			} 
			public void Serialize(string filename) 
			{ 
				StreamWriter stm = new StreamWriter(File.OpenWrite( filename )); 
				BinaryFormatter bf=new BinaryFormatter(); 
				bf.Serialize(stm.BaseStream,this); 
				stm.Close(); 
			} 
			public static Perob Deserialize(string filename) 
			{ 
				Perob rv; 
				try 
				{ 
					StreamReader stm = new StreamReader(File.OpenRead( filename )); 
					BinaryFormatter bf=new BinaryFormatter(); 
					rv = bf.Deserialize(stm.BaseStream) as Perob; 
					stm.Close(); 
					return rv; 
				} 
				catch ( Exception ex ) 
				{ 
					// Catch everything 
					ex = ex; 
				} 
				return null; 
			} 
		} 

		[Test] 
		public void SaveAndLoad() 
		{ 
			Perob p = new Perob(3,4); 
			string filename = "perob.tst"; 
			p.Serialize( filename ); 

			Perob np = Perob.Deserialize( filename ); 

			Assertion.AssertNotNull("np != null",np); 
			Assertion.AssertEquals("i neq", p.i, np.i ); 
			Assertion.AssertEquals("j neq", p.j, np.j ); 
		} 
	}
}

