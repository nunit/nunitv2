#region Copyright (c) 2002, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov
' Copyright © 2000-2002 Philip A. Craig
'
' This software is provided 'as-is', without any express or implied warranty. In no 
' event will the authors be held liable for any damages arising from the use of this 
' software.
' 
' Permission is granted to anyone to use this software for any purpose, including 
' commercial applications, and to alter it and redistribute it freely, subject to the 
' following restrictions:
'
' 1. The origin of this software must not be misrepresented; you must not claim that 
' you wrote the original software. If you use this software in a product, an 
' acknowledgment (see the following) in the product documentation is required.
'
' Portions Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov 
' or Copyright © 2000-2002 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion


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

