
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
		private static readonly string fileName = "perobj.tst";

		[Serializable] 
		public class Perob 
		{ 
			public int i; 
			public int j; 
			public Perob(int ii,int jj) 
			{ 
				i = ii; 
				j = jj; 
			} 

			void SuppressWarning() 
			{ 
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
				catch (Exception ex ) 
				{ 
					ex = ex; 
				} 
				return null; 
			} 
		} 

		[TearDown]
		public void CleanUp()
		{
			FileInfo file = new FileInfo(fileName);
			if(file.Exists)
				file.Delete();
		}

		[Test] 
		public void SaveAndLoad() 
		{ 
			Perob p = new Perob(3,4); 
			p.Serialize( fileName ); 

			Perob np = Perob.Deserialize( fileName ); 

			Assertion.AssertNotNull("np != null",np); 
			Assertion.AssertEquals("i neq", p.i, np.i ); 
			Assertion.AssertEquals("j neq", p.j, np.j ); 
		} 
	}
}

