#region Copyright (c) 2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright  2000-2002 Philip A. Craig
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
' Portions Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright  2000-2002 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Reflection.Emit;

using NUnit.Core;
using NUnit.Framework;

namespace NUnit.Tests.Core
{
	[TestFixture]
	public class AssemblyVersionFixture
	{
		private static readonly string mockAssemblyName = "mock-test-assembly.dll";
		
		[TearDown]
		public void DeleteMockAssembly()
		{
			FileInfo info = new FileInfo(mockAssemblyName);
			if(info.Exists)
				info.Delete();
		}

		[Test]
		public void Version()
		{
			Version version = new Version("1.0.0.2002");
			string nameString = "TestAssembly";

			AssemblyName assemblyName = new AssemblyName(); 
			assemblyName.Name = nameString;
			assemblyName.Version = version;
			MakeDynamicAssembly(assemblyName, mockAssemblyName);

			Assembly assembly = FindAssemblyByName(nameString);

			System.Version foundVersion = assembly.GetName().Version;
			Assert.AreEqual(version, foundVersion);
		}

		private Assembly FindAssemblyByName(string name)
		{
			// Get all the assemblies currently loaded in the application domain.
			Assembly[] myAssemblies = Thread.GetDomain().GetAssemblies();

			Assembly assembly = null;
			for(int i = 0; i < myAssemblies.Length && assembly == null; i++)
			{
				if(String.Compare(myAssemblies[i].GetName().Name, name) == 0)
					assembly = myAssemblies[i];
			}
			return assembly;
		}

		public static void MakeDynamicAssembly(AssemblyName myAssemblyName, string fileName)
		{
			AssemblyBuilder myAssemblyBuilder = Thread.GetDomain().DefineDynamicAssembly(myAssemblyName, AssemblyBuilderAccess.RunAndSave);			
			myAssemblyBuilder.Save(fileName);
		}
	}
}
