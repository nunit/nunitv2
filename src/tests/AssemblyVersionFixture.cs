/********************************************************************************************************************
'
' Copyright (c) 2002, James Newkirk, Michael C. Two, Alexei Vorontsov, Philip Craig
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
' THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
'
'*******************************************************************************************************************/
namespace NUnit.Tests
{
	using System;
	using System.IO;
	using System.Reflection;
	using System.Threading;
	using System.Reflection.Emit;

	using NUnit.Core;
	using NUnit.Framework;

	/// <summary>
	/// Summary description for AssemblyVersionFixture.
	/// </summary>
	/// 
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
			Assertion.AssertEquals(version, foundVersion);
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
